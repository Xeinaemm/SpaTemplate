// -----------------------------------------------------------------------
// <copyright file="PropertyMappingService.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.SharedKernel
{
    using System;
    using System.Collections.Generic;
    using Xeinaemm.Hateoas;

    public class PropertyMappingService : PropertyMappingServiceBase
    {
        public PropertyMappingService()
            : base(new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Name", new PropertyMappingValue(new List<string> { "Name" }) },
                { "Surname", new PropertyMappingValue(new List<string> { "Surname" }) },
                { "Age", new PropertyMappingValue(new List<string> { "Age" }, true) },
                { "IsDone", new PropertyMappingValue(new List<string> { "IsDone" }, true) },
                { "FullName", new PropertyMappingValue(new List<string> { "Name", "Surname" }) },
                { "Dummy", new PropertyMappingValue(new List<string> { "Dummy" }) },
                { "Title", new PropertyMappingValue(new List<string> { "Title" }) },
                { "Description", new PropertyMappingValue(new List<string> { "Description" }) },
            })
        {
        }
    }
}