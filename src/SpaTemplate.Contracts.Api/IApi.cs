// -----------------------------------------------------------------------
// <copyright file="IApi.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Contracts.Api
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Refit;
    using Xeinaemm.Hateoas;

    public interface IApi
    {
        [Get("/api/v1")]
        Task<IEnumerable<LinkDto>> GetRoot([Header("Accept")] string mediaType = MediaType.OutputFormatterJson);
    }
}
