﻿// -----------------------------------------------------------------------
// <copyright file="HateoasCollectionDto.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using System.Collections.ObjectModel;
    using Xeinaemm.Hateoas;

    public class HateoasCollectionDto<T>
    {
        public Collection<HateoasDto<T>> Values { get; } = new Collection<HateoasDto<T>>();

        public Collection<LinkDto> Links { get; } = new Collection<LinkDto>();
    }
}
