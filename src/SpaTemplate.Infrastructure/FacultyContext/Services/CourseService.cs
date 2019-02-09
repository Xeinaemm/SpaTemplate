// -----------------------------------------------------------------------
// <copyright file="CourseService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure.FacultyContext
{
    using System;
    using SpaTemplate.Contracts.Models;
    using SpaTemplate.Core.FacultyContext;
    using Xeinaemm.Common;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;

    public class CourseService : IHandle<CourseCompletedEvent>, ICourseService
    {
        private readonly IPropertyMappingService propertyMappingService;
        private readonly IHateoasRepository<ApplicationDbContext> repository;
        private readonly ITypeHelperService typeHelperService;

        public CourseService(
            IHateoasRepository<ApplicationDbContext> repository,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            this.repository = repository;
            this.propertyMappingService = propertyMappingService;
            this.typeHelperService = typeHelperService;
        }

        public bool AddCourse(Guid studentId, Course course)
        {
            var people = this.repository.GetFirstOrDefault(new StudentSpecification(studentId));
            if (people == null) return false;
            if (course.Id == Guid.Empty) course.Id = Guid.NewGuid();
            return this.repository.AddEntity(course);
        }

        public bool CourseMappingExists(IParameters parameters) =>
            this.propertyMappingService.ValidMappingExistsFor<CourseDto, Course>(
                parameters.OrderBy);

        public bool CoursePropertiesExists(IParameters parameters) =>
            this.typeHelperService.TypeHasProperties<CourseDto>(parameters.Fields);

        public bool DeleteCourse(Course course) => this.repository.DeleteEntity(course);

        public Course GetCourse(Guid studentId, Guid courseId) =>
            this.repository.GetFirstOrDefault(new CourseSpecification(studentId, courseId));

        public PagedListCollection<Course> GetPagedList(Guid studentId, IParameters parameters) =>
            this.repository.GetCollection(
                new CourseParametersSpecification(parameters, studentId), parameters);

        public void Handle(CourseCompletedEvent domainEvent)
        {
            if (domainEvent == null)
                throw new NullReferenceException($"Collection {nameof(domainEvent)} cannot be null");

            // Do Nothing
        }

        public bool StudentExists(Guid studentId) => this.repository.ExistsEntity<Student>(studentId);

        public bool UpdateCourse(Course course) => this.repository.UpdateEntity(course);
    }
}