﻿// -----------------------------------------------------------------------
// <copyright file="CourseSpecification.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
	using System;
	using SpaTemplate.Core.SharedKernel;

	public sealed class CourseSpecification : BaseSpecification<Course>
	{
		public CourseSpecification(Guid studentId)
			: base(course => course.StudentId == studentId) => this.AddInclude(b => b.Student);

		public CourseSpecification(Guid studentId, Guid courseId)
			: base(course =>
			CourseCriteria(course, studentId, courseId)) => this.AddInclude(b => b.Student);

		private static bool CourseCriteria(Course course, Guid studentId, Guid courseId)
		{
			if (course == null) return false;
			if (studentId != Guid.Empty && courseId != Guid.Empty)
				return course.StudentId == studentId && course.Id == courseId;
			if (studentId != Guid.Empty) return course.StudentId == studentId;
			return false;
		}
	}
}