// -----------------------------------------------------------------------
// <copyright file="PeopleController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
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
    [Route("api/v{version:apiVersion}/people")]
    [ValidateModel]
    [ApiController]
    public partial class PeopleController : Controller
    {
        private readonly IMapper mapper;
        private readonly IStudentService studentService;
        private readonly IUrlHelper urlHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleController"/> class.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="studentService"></param>
        /// <param name="urlHelper"></param>
        public PeopleController(
            IMapper mapper,
            IStudentService studentService,
            IUrlHelper urlHelper)
        {
            this.studentService = studentService;
            this.urlHelper = urlHelper;
            this.mapper = mapper;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="studentForCreationDto"></param>
        /// <returns></returns>
        [HttpPost(Name = RouteName.CreateStudent)]
        [Consumes(MediaTypeNames.Application.Json, MediaType.InputFormatterJson)]
        [RequestHeaderMatchesMediaType("Content-Type", MediaTypeNames.Application.Json, MediaType.InputFormatterJson)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult CreateStudent([FromBody] StudentForCreationDto studentForCreationDto)
        {
            if (studentForCreationDto == null) return this.BadRequest();

            var student = this.mapper.Map<Student>(studentForCreationDto);
            if (!this.studentService.AddStudent(student)) throw new Exception("Creating an Student failed on save.");
            return this.CreatedAtRoute(
                RouteName.GetStudent,
                new { id = student.Id },
                student.ShapeDataWithoutParameters(this.CreateLinksStudent));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = RouteName.DeleteStudent)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteStudent(Guid id)
        {
            var student = this.studentService.GetStudent(id);
            if (student == null) return this.NotFound();

            if (!this.studentService.DeleteStudent(student)) throw new Exception($"Deleting Student {id} failed on save.");
            return this.NoContent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        [HttpGet(Name = RouteName.GetPeople)]
        [Produces(MediaType.OutputFormatterJson)]
        [RequestHeaderMatchesMediaType("Accept", MediaTypeNames.Application.Json, MediaType.OutputFormatterJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeople(
            StudentParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!this.studentService.StudentMappingExists(parameters))
                return this.BadRequest();
            if (!this.studentService.StudentPropertiesExists(parameters))
                return this.BadRequest();

            var people = this.studentService.GetPagedList(parameters);
            var studentDtos = this.mapper.Map<List<StudentDto>>(people);

            if (mediaType == MediaType.OutputFormatterJson)
            {
                this.Response.Headers.Add(Header.XPagination, JsonConvert.SerializeObject(people.CreateBasePagination()));
                var values = studentDtos.ShapeDataCollectionWithHateoasLinks(parameters.Fields, this.CreateLinksStudent);
                var links = this.urlHelper.CreateLinks(RouteName.GetPeople, parameters, people);
                return this.Ok(new HateoasDto(values, links));
            }

            this.Response.Headers.Add(Header.XPagination, JsonConvert.SerializeObject(
                this.urlHelper.CreateHateoasPagination(RouteName.GetPeople, parameters, people)));

            return this.Ok(studentDtos.ShapeDataCollection(parameters.Fields));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters"></param>
        /// <param name="mediaType"></param>
        /// <returns>
        ///
        /// </returns>
        [HttpGet("{id}", Name = RouteName.GetStudent)]
        [Produces(MediaType.OutputFormatterJson)]
        [RequestHeaderMatchesMediaType("Accept", MediaTypeNames.Application.Json, MediaType.OutputFormatterJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetStudent(
            Guid id,
            StudentParameters parameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!this.studentService.StudentPropertiesExists(parameters)) return this.BadRequest();

            var student = this.studentService.GetStudent(id);
            return student == null
                ? this.NotFound()
                : (IActionResult)(mediaType == MediaType.OutputFormatterJson
                ? this.Ok(student.ShapeDataWithoutParameters(this.CreateLinksStudent))
                : this.Ok(this.mapper.Map<StudentDto>(student)));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = RouteName.PartiallyUpdateStudent)]
        [Consumes(MediaType.PatchFormatterJson)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult PartiallyUpdateStudent(
            Guid id,
            [FromBody] JsonPatchDocument<StudentForUpdateDto> patchDoc)
        {
            if (patchDoc == null) return this.BadRequest();
            if (!this.studentService.StudentExists(id)) return this.NotFound();
            var student = this.studentService.GetStudent(id);

            var studentForUpdateDto = this.mapper.Map<StudentForUpdateDto>(student);

            patchDoc.ApplyTo(studentForUpdateDto, this.ModelState);

            if (studentForUpdateDto.Name == studentForUpdateDto.Surname)
                this.ModelState.AddModelError(nameof(StudentForUpdateDto), "The provided surname should be different from the name.");

            this.TryValidateModel(studentForUpdateDto);
            if (!this.ModelState.IsValid) return new UnprocessableEntityObjectResult(this.ModelState);

            this.mapper.Map(studentForUpdateDto, student);
            if (!this.studentService.UpdateStudent(student)) throw new Exception($"Patching student {id} failed on save.");

            return this.NoContent();
        }
    }

    public partial class PeopleController
    {
        private string CreateHref(Guid id, string routeName, string fields = "") =>
            this.urlHelper.Link(routeName, new { id, fields });

        private IEnumerable<LinkDto> CreateLinksStudent(Guid id, string fields = null) => new List<LinkDto>
        {
            string.IsNullOrWhiteSpace(fields)
                ? this.CreateHref(id, RouteName.GetStudent)
                    .AddRelAndMethod(Rel.Self, HttpMethods.Get)
                : this.CreateHref(id, RouteName.GetStudent, fields)
                    .AddRelAndMethod(Rel.Self, HttpMethods.Get),
            this.CreateHref(id, RouteName.CreateStudent)
                .AddRelAndMethod(SpaTemplateRel.CreateStudent, HttpMethods.Post),
            this.CreateHref(id, RouteName.PartiallyUpdateStudent)
                .AddRelAndMethod(SpaTemplateRel.PatchStudent, HttpMethods.Patch),
            this.CreateHref(id, RouteName.DeleteStudent)
                .AddRelAndMethod(SpaTemplateRel.DeleteStudent, HttpMethods.Delete),
        };
    }
}