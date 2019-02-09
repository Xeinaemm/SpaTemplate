// -----------------------------------------------------------------------
// <copyright file="CourseProfile.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using AutoMapper;
    using SpaTemplate.Contracts.Models;

    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            this.CreateMap<Course, CourseDto>();
            this.CreateMap<CourseDto, Course>();
            this.CreateMap<CourseForCreationDto, Course>();
            this.CreateMap<CourseForUpdateDto, Course>();
            this.CreateMap<Course, CourseForUpdateDto>();
        }
    }
}
