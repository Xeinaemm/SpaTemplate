// -----------------------------------------------------------------------
// <copyright file="StudentForCreationDto.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Contracts.Models
{
    using System.Collections.Generic;

    public class StudentForCreationDto : StudentForManipulationDto
    {
        public List<CourseForCreationDto> CourseForCreationDtos { get; }
            = new List<CourseForCreationDto>();
    }
}