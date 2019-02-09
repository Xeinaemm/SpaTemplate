// -----------------------------------------------------------------------
// <copyright file="ICourseService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using System;
    using Xeinaemm.Common;
    using Xeinaemm.Hateoas;

    public interface ICourseService
    {
        PagedListCollection<Course> GetPagedList(Guid studentId, IParameters parameters);

        Course GetCourse(Guid studentId, Guid courseId);

        bool AddCourse(Guid studentId, Course course);

        bool DeleteCourse(Course course);

        bool UpdateCourse(Course course);

        bool StudentExists(Guid studentId);

        bool CourseMappingExists(IParameters parameters);

        bool CoursePropertiesExists(IParameters parameters);
    }
}