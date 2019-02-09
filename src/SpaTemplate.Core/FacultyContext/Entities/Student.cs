// -----------------------------------------------------------------------
// <copyright file="Student.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using System.Collections.Generic;
    using SpaTemplate.Core.SharedKernel;

    public class Student : Person
    {
        public List<Course> Courses { get; } = new List<Course>();

        public bool IsDone { get; private set; }

        public void MarkComplete()
        {
            this.IsDone = true;
            this.Events?.Add(new StudentCompletedEvent(this));
        }
    }
}