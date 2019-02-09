// -----------------------------------------------------------------------
// <copyright file="RouteName.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.SharedKernel
{
    public static class RouteName
    {
        public const string GetRoot = "GetRoot";

        public const string GetPeople = "GetPeople";
        public const string GetStudent = "GetStudent";
        public const string DeleteStudent = "DeleteStudent";
        public const string CreateStudent = "CreateStudent";
        public const string PartiallyUpdateStudent = "PartiallyUpdateStudent";

        public const string GetStudentCollection = "GetStudentCollection";

        public const string GetCoursesForStudent = "GetCoursesForStudent";
        public const string DeleteCourseForStudent = "DeleteCourseForStudent";
        public const string UpdateCourseForStudent = "UpdateCourseForStudent";
        public const string PartiallyUpdateCourseForStudent = "PartiallyUpdateCourseForStudent";
        public const string CreateCourseForStudent = "CreateCourseForStudent";
        public const string GetCourseForStudent = "GetCourseForStudent";
    }
}