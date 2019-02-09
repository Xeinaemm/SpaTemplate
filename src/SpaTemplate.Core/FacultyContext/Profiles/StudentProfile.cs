// -----------------------------------------------------------------------
// <copyright file="StudentProfile.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using AutoMapper;
    using SpaTemplate.Contracts.Models;

    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            this.CreateMap<Student, StudentDto>();
            this.CreateMap<StudentForCreationDto, Student>();
            this.CreateMap<StudentForUpdateDto, Student>();
            this.CreateMap<Student, StudentForUpdateDto>();
        }
    }
}
