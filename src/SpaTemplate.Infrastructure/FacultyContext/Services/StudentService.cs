// -----------------------------------------------------------------------
// <copyright file="StudentService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure.FacultyContext
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using SpaTemplate.Contracts.Models;
    using SpaTemplate.Core.FacultyContext;
    using Xeinaemm.Common;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;

    public class StudentService : IHandle<StudentCompletedEvent>, IStudentService
    {
        private readonly IPropertyMappingService propertyMappingService;
        private readonly IHateoasRepository<ApplicationDbContext> repository;
        private readonly ITypeHelperService typeHelperService;

        public StudentService(
            IHateoasRepository<ApplicationDbContext> repository,
            ITypeHelperService typeHelperService,
            IPropertyMappingService propertyMappingService)
        {
            this.repository = repository;
            this.typeHelperService = typeHelperService;
            this.propertyMappingService = propertyMappingService;
        }

        public bool AddStudent(Student student) => this.repository.AddEntity(student);

        public bool DeleteStudent(Student student) => this.repository.DeleteEntity(student);

        public ReadOnlyCollection<Student> GetCollection(ICollection<Guid> ids) =>
            this.repository.GetCollection(new StudentSpecification(ids));

        public PagedListCollection<Student> GetPagedList(IParameters parameters) =>
            this.repository.GetCollection(new StudentParametersSpecification(parameters), parameters);

        public Student GetStudent(Guid studentId) => this.repository.GetEntity<Student>(studentId);

        public void Handle(StudentCompletedEvent domainEvent)
        {
            if (domainEvent == null)
                throw new NullReferenceException($"Collection {nameof(domainEvent)} cannot be null");

            // Do Nothing
        }

        public bool StudentExists(Guid studentId) => this.repository.ExistsEntity<Student>(studentId);

        public bool StudentMappingExists(IParameters parameters)
            => this.propertyMappingService.ValidMappingExistsFor<StudentDto, Student>(
                parameters.OrderBy);

        public bool StudentPropertiesExists(IParameters parameters)
            => this.typeHelperService.TypeHasProperties<StudentDto>(parameters.Fields);

        public bool UpdateStudent(Student student) => this.repository.UpdateEntity(student);
    }
}