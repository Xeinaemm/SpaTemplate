// -----------------------------------------------------------------------
// <copyright file="IStudentService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xeinaemm.Common;
    using Xeinaemm.Hateoas;

    public interface IStudentService
    {
        ReadOnlyCollection<Student> GetCollection(ICollection<Guid> ids);

        Student GetStudent(Guid studentId);

        bool AddStudent(Student student);

        bool DeleteStudent(Student student);

        bool UpdateStudent(Student student);

        bool StudentExists(Guid studentId);

        bool StudentMappingExists(IParameters parameters);

        bool StudentPropertiesExists(IParameters parameters);

        PagedListCollection<Student> GetPagedList(IParameters parameters);
    }
}