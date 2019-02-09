// -----------------------------------------------------------------------
// <copyright file="IPeopleCollectionApi.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Contracts.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Refit;
    using SpaTemplate.Contracts.Models;

    public interface IPeopleCollectionApi
    {
        [Post("/api/v1/people-collections")]
        Task<IEnumerable<StudentDto>> CreateStudentCollection([Body] IEnumerable<StudentForCreationDto> studentForCreationDtos);

        [Get("/api/v1/people-collections/({ids})")]
        Task<IEnumerable<StudentDto>> GetStudentCollection(IEnumerable<Guid> ids);
    }
}
