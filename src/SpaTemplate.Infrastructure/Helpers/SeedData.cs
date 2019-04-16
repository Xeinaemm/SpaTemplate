﻿// -----------------------------------------------------------------------
// <copyright file="SeedData.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure
{
	using System.Collections.Generic;
	using SpaTemplate.Core.FacultyContext;

	public static class SeedData
	{
		public static void PopulateTestData(AppDbContext dbContext)
		{
			dbContext.People.RemoveRange(dbContext.People);
			_ = dbContext.SaveChanges();
			for (var i = 0; i < 20; i++) AddStudent(dbContext, $"Name{i}", $"Surname{i}", i);
			_ = dbContext.SaveChanges();
		}

		private static void AddStudent(AppDbContext dbContext, string name, string surname, int age) => dbContext.People.Add(new Student
		{
			Name = name,
			Surname = surname,
			Age = age,
			Courses = SeedCourses(),
		});

		private static List<Course> SeedCourses()
		{
			var list = new List<Course>();
			for (var i = 0; i < 20; i++)
			{
				list.Add(new Course
				{
					Title = $"Title{i}",
					Description = $"Description{i}",
				});
			}

			return list;
		}
	}
}