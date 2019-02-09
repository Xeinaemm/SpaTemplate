// -----------------------------------------------------------------------
// <copyright file="ICoursesApi.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Contracts.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using Refit;
    using SpaTemplate.Contracts.Models;
    using SpaTemplate.Contracts.Parameters;
    using Xeinaemm.Hateoas;

    public interface ICoursesApi
    {
        [Post("/api/v1/people/{studentId}/courses")]
        Task<CourseDto> CreateCourseForStudent(Guid studentId, [Body] CourseForCreationDto courseForCreationDto);

        [Delete("/api/v1/people/{studentId}/courses/{id}")]
        Task DeleteCourseForStudent(Guid studentId, Guid id);

        [Get("/api/v1/people/{studentId}/courses/{id}")]
        Task<CourseDto> GetCourseForStudent(Guid studentId, Guid id, [Header("Accept")] string contentType = MediaTypeNames.Application.Json);

        [Get("/api/v1/people/{studentId}/courses/{id}")]
        Task<HateoasCollectionDto<CourseDto>> GetCourseForStudentHateoas(Guid studentId, Guid id, [Header("Accept")] string mediaType = MediaType.OutputFormatterJson);

        [Get("/api/v1/people/{studentId}/courses")]
        Task<IEnumerable<CourseDto>> GetCoursesForStudent(Guid studentId, CourseParameters parameters, [Header("Accept")] string contentType = MediaTypeNames.Application.Json);

        [Get("/api/v1/people/{studentId}/courses")]
        Task<HateoasCollectionDto<CourseDto>> GetCoursesForStudentHateoas(Guid studentId, CourseParameters parameters, [Header("Accept")] string mediaType = MediaType.OutputFormatterJson);

        [Patch("/api/v1/people/{studentId}/courses/{id}")]
        Task<CourseDto> PartiallyUpdateCourseForStudent(Guid studentId, Guid id, [Body] JsonPatchDocument<CourseForUpdateDto> patchDoc);

        [Put("/api/v1/people/{studentId}/courses/{id}")]
        Task<CourseDto> UpdateCourseForStudent(Guid studentId, Guid id, [Body] CourseForUpdateDto courseForUpdateDto);
    }
}
