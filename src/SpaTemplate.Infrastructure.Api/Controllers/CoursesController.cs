// -----------------------------------------------------------------------
// <copyright file="CoursesController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure.Api
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.Mime;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json;
    using SpaTemplate.Contracts.Models;
    using SpaTemplate.Contracts.Parameters;
    using SpaTemplate.Core.FacultyContext;
    using SpaTemplate.Core.SharedKernel;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Api;
    using Xeinaemm.Hateoas;

    /// <summary>
    ///
    /// </summary>
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Route("api/v{version:apiVersion}/people/{studentId}/courses")]
    [ValidateModel]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService courseService;
        private readonly IUrlHelper urlHelper;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoursesController"/> class.
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="mapper"></param>
        /// <param name="courseService"></param>
        public CoursesController(
            IUrlHelper urlHelper,
            IMapper mapper,
            ICourseService courseService)
        {
            this.urlHelper = urlHelper;
            this.mapper = mapper;
            this.courseService = courseService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseForCreationDto"></param>
        /// <returns>
        ///
        /// </returns>
        [HttpPost(Name = RouteName.CreateCourseForStudent)]
        [Consumes(MediaTypeNames.Application.Json, MediaType.InputFormatterJson)]
        [RequestHeaderMatchesMediaType("Content-Type", MediaTypeNames.Application.Json, MediaType.InputFormatterJson)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult CreateCourseForStudent(
            Guid studentId,
            [FromBody] CourseForCreationDto courseForCreationDto)
        {
            if (courseForCreationDto == null) return this.BadRequest();

            if (courseForCreationDto.Description == courseForCreationDto.Title)
                this.ModelState.AddModelError(nameof(CourseForCreationDto), "The provided description should be different from the title.");

            if (!this.ModelState.IsValid) return new UnprocessableEntityObjectResult(this.ModelState);
            if (!this.courseService.StudentExists(studentId)) return this.NotFound();

            var course = this.mapper.Map<Course>(courseForCreationDto);

            if (!this.courseService.AddCourse(studentId, course)) throw new Exception($"Creating a course for student {studentId} failed on save.");

            return this.CreatedAtRoute(
                RouteName.GetCourseForStudent,
                new { studentId, course.Id },
                course.ShapeDataWithoutParameters(this.CreateLinksForCourse));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="id"></param>
        /// <returns>
        ///
        /// </returns>
        [HttpDelete("{id}", Name = RouteName.DeleteCourseForStudent)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCourseForStudent(Guid studentId, Guid id)
        {
            if (!this.courseService.StudentExists(studentId)) return this.NotFound();

            var course = this.courseService.GetCourse(studentId, id);
            if (course == null) return this.NotFound();

            if (!this.courseService.DeleteCourse(course))
                throw new Exception($"Deleting course {id} for student {studentId} failed on save.");

            return this.NoContent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="id"></param>
        /// <param name="mediaType"></param>
        /// <returns>
        ///
        /// </returns>
        [HttpGet("{id}", Name = RouteName.GetCourseForStudent)]
        [Produces(MediaType.OutputFormatterJson)]
        [RequestHeaderMatchesMediaType("Accept", MediaTypeNames.Application.Json, MediaType.OutputFormatterJson)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCourseForStudent(
            Guid studentId,
            Guid id,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!this.courseService.StudentExists(studentId)) return this.NotFound();

            var course = this.courseService.GetCourse(studentId, id);
            return course == null
                ? this.NotFound()
                : (IActionResult)(mediaType != MediaType.OutputFormatterJson
                ? this.Ok(course)
                : this.Ok(course.ShapeDataWithoutParameters(this.CreateLinksForCourse)));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="parameters"></param>
        /// <param name="mediaType"></param>
        /// <returns>
        ///
        /// </returns>
        [HttpGet(Name = RouteName.GetCoursesForStudent)]
        [Produces(MediaType.OutputFormatterJson)]
        [RequestHeaderMatchesMediaType("Accept", MediaTypeNames.Application.Json, MediaType.OutputFormatterJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCoursesForStudent(
            Guid studentId,
            CourseParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!this.courseService.StudentExists(studentId)) return this.NotFound();
            if (!this.courseService.CourseMappingExists(parameters)) return this.BadRequest();
            if (!this.courseService.CoursePropertiesExists(parameters)) return this.BadRequest();

            var courses = this.courseService.GetPagedList(studentId, parameters);
            var courseDtos = this.mapper.Map<IEnumerable<CourseDto>>(courses);

            if (mediaType == MediaType.OutputFormatterJson)
            {
                this.Response.Headers.Add(Header.XPagination, JsonConvert.SerializeObject(courses.CreateBasePagination()));
                var values = courseDtos.ShapeDataCollectionWithHateoasLinks(parameters.Fields, this.CreateLinksForCourse);
                var links = this.urlHelper.CreateLinks(RouteName.GetCoursesForStudent, parameters, courses);
                return this.Ok(new HateoasDto(values, links));
            }

            this.Response.Headers.Add(Header.XPagination, JsonConvert.SerializeObject(
                this.urlHelper.CreateHateoasPagination(RouteName.GetCoursesForStudent, parameters, courses)));

            return this.Ok(courseDtos.ShapeDataCollection(parameters.Fields));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns>
        ///
        /// </returns>
        [HttpPatch("{id}", Name = RouteName.PartiallyUpdateCourseForStudent)]
        [Consumes(MediaType.PatchFormatterJson)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult PartiallyUpdateCourseForStudent(
            Guid studentId,
            Guid id,
            [FromBody] JsonPatchDocument<CourseForUpdateDto> patchDoc)
        {
            if (patchDoc == null) return this.BadRequest();
            if (!this.courseService.StudentExists(studentId)) return this.NotFound();

            var course = this.courseService.GetCourse(studentId, id);

            if (course == null)
            {
                var courseForUpdateDto = new CourseForUpdateDto();
                patchDoc.ApplyTo(courseForUpdateDto, this.ModelState);

                if (courseForUpdateDto.Description == courseForUpdateDto.Title)
                    this.ModelState.AddModelError(nameof(CourseForUpdateDto), "The provided description should be different from the title.");

                this.TryValidateModel(courseForUpdateDto);

                if (!this.ModelState.IsValid) return new UnprocessableEntityObjectResult(this.ModelState);

                var mapCourse = this.mapper.Map<Course>(courseForUpdateDto);
                mapCourse.Id = id;

                if (!this.courseService.AddCourse(studentId, mapCourse))
                    throw new Exception($"Upserting course {id} for student {studentId} failed on save.");

                var courseDto = this.mapper.Map<CourseDto>(mapCourse);
                return this.CreatedAtRoute(
                    RouteName.GetCourseForStudent,
                    new { studentId, courseDto.Id },
                    courseDto);
            }

            var courseToPatch = this.mapper.Map<CourseForUpdateDto>(course);

            patchDoc.ApplyTo(courseToPatch, this.ModelState);

            if (courseToPatch.Description == courseToPatch.Title)
                this.ModelState.AddModelError(nameof(CourseForUpdateDto), "The provided description should be different from the title.");

            this.TryValidateModel(courseToPatch);
            if (!this.ModelState.IsValid) return new UnprocessableEntityObjectResult(this.ModelState);

            this.mapper.Map(courseToPatch, course);

            if (!this.courseService.UpdateCourse(course))
                throw new Exception($"Patching course {id} for student {studentId} failed on save.");

            return this.NoContent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="id"></param>
        /// <param name="courseForUpdateDto"></param>
        /// <returns>
        ///
        /// </returns>
        [HttpPut("{id}", Name = RouteName.UpdateCourseForStudent)]
        [Consumes(MediaTypeNames.Application.Json, MediaType.InputFormatterJson)]
        [RequestHeaderMatchesMediaType("Content-Type", MediaTypeNames.Application.Json, MediaType.InputFormatterJson)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult UpdateCourseForStudent(
            Guid studentId,
            Guid id,
            [FromBody] CourseForUpdateDto courseForUpdateDto)
        {
            if (courseForUpdateDto == null) return this.BadRequest();

            if (courseForUpdateDto.Description == courseForUpdateDto.Title)
                this.ModelState.AddModelError(nameof(CourseForUpdateDto), "The provided description should be different from the title.");

            if (!this.ModelState.IsValid) return new UnprocessableEntityObjectResult(this.ModelState);
            if (!this.courseService.StudentExists(studentId)) return this.NotFound();

            var course = this.courseService.GetCourse(studentId, id);
            if (course == null)
            {
                var mapCourse = this.mapper.Map<Course>(courseForUpdateDto);
                mapCourse.Id = id;

                if (!this.courseService.AddCourse(studentId, mapCourse))
                    throw new Exception($"Upserting course {id} for student {studentId} failed on save.");

                var courseDto = this.mapper.Map<CourseDto>(mapCourse);

                return this.CreatedAtRoute(
                    RouteName.GetCourseForStudent,
                    new { studentId, courseDto.Id },
                    courseDto);
            }

            this.mapper.Map(courseForUpdateDto, course);
            if (!this.courseService.UpdateCourse(course))
                throw new Exception($"Updating course {id} for student {studentId} failed on save.");

            return this.NoContent();
        }

        private string CreateHref(Guid id, string routeName, string fields = null) =>
            this.urlHelper.Link(routeName, new { id, fields });

        private Collection<LinkDto> CreateLinksForCourse(Guid id, string fields = null) => new Collection<LinkDto>
        {
            string.IsNullOrWhiteSpace(fields)
                ? this.CreateHref(id, RouteName.GetCoursesForStudent)
                    .AddRelAndMethod(Rel.Self, HttpMethods.Get)
                : this.CreateHref(id, RouteName.GetCoursesForStudent, fields)
                    .AddRelAndMethod(Rel.Self, HttpMethods.Get),
            this.CreateHref(id, RouteName.CreateCourseForStudent)
                .AddRelAndMethod(SpaTemplateRel.CreateCourseForStudent, HttpMethods.Post),
            this.CreateHref(id, RouteName.PartiallyUpdateCourseForStudent)
                .AddRelAndMethod(SpaTemplateRel.PartiallyUpdateCourse, HttpMethods.Patch),
            this.CreateHref(id, RouteName.UpdateCourseForStudent)
                .AddRelAndMethod(SpaTemplateRel.UpdateCourse, HttpMethods.Put),
            this.CreateHref(id, RouteName.DeleteCourseForStudent)
                .AddRelAndMethod(SpaTemplateRel.DeleteCourse, HttpMethods.Delete),
        };
    }
}