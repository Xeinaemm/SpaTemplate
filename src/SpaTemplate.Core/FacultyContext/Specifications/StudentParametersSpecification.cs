// -----------------------------------------------------------------------
// <copyright file="StudentParametersSpecification.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using System;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;

    public sealed class StudentParametersSpecification : BaseSpecification<Student>
    {
        public StudentParametersSpecification(Guid studentId)
            : base(student => student.Id == studentId) => this.AddInclude(student => student.Courses);

        public StudentParametersSpecification(IParameters parameters)
            : base(student => CriteriaExpression(student, parameters)) => this.AddInclude(student => student.Courses);

        private static bool CriteriaExpression(Student student, IParameters parameters) =>
            parameters.SearchQuery == null
            || student.Name.IndexOf(parameters.SearchQuery.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0
            || student.Surname.IndexOf(parameters.SearchQuery.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0;
    }
}