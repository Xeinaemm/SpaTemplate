// -----------------------------------------------------------------------
// <copyright file="Course.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using System;
    using Xeinaemm.Domain;

    public class Course : BaseEntity
    {
        public string Description { get; set; }

        public Student Student { get; set; }

        public Guid StudentId { get; set; }

        public string Title { get; set; }
    }
}