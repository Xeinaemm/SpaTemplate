// -----------------------------------------------------------------------
// <copyright file="SeedData.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SpaTemplate.Core.FacultyContext;

    public static class SeedData
    {
        public static void EnsureSeedTestData(this IServiceProvider provider)
        {
            var dbContext = provider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            for (var i = 0; i < 20; i++) AddStudent(dbContext, $"Name{i}", $"Surname{i}", i);
            dbContext.SaveChanges();
        }

        private static void AddStudent(ApplicationDbContext dbContext, string name, string surname, int age)
        {
            var student = new Student
            {
                Name = name,
                Surname = surname,
                Age = age,
            };
            student.Courses.Clear();
            student.Courses.AddRange(SeedCourses());
            dbContext.People.Add(student);
        }

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