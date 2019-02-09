// -----------------------------------------------------------------------
// <copyright file="StudentSpecification.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using System;
    using System.Collections.Generic;
    using Xeinaemm.Domain;

    public sealed class StudentSpecification : BaseSpecification<Student>
    {
        public StudentSpecification(Guid studentId)
            : base(student => student.Id == studentId) => this.AddInclude(student => student.Courses);

        public StudentSpecification(ICollection<Guid> studentIds)
            : base(student => studentIds.Contains(student.Id)) => this.AddInclude(student => student.Courses);
    }
}