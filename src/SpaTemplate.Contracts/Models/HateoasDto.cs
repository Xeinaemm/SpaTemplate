// -----------------------------------------------------------------------
// <copyright file="HateoasDto.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Contracts.Models
{
    using System.Collections.ObjectModel;
    using Xeinaemm.Hateoas;

    public class HateoasDto<T>
    {
        public Collection<T> Values { get; } = new Collection<T>();

        public Collection<LinkDto> Links { get; } = new Collection<LinkDto>();
    }
}
