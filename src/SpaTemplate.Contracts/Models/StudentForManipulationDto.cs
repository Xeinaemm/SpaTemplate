// -----------------------------------------------------------------------
// <copyright file="StudentForManipulationDto.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Contracts.Models
{
    public class StudentForManipulationDto
    {
        public int Age { get; set; }

        public bool IsDone { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}