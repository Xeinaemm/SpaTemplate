// -----------------------------------------------------------------------
// <copyright file="StudentDto.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Contracts.Models
{
    using System;
    using System.Collections.Generic;
    using Xeinaemm.Domain;
    using Xeinaemm.Hateoas;

    // Note: doesn't expose events or behavior
    public class StudentDto : IDto
    {
        public int Age { get; set; }

        public Guid Id { get; set; }

        public bool IsDone { get; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public List<BaseDomainEvent> Events { get; }
    }
}