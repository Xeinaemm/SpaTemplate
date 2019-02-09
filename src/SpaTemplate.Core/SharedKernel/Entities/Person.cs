// -----------------------------------------------------------------------
// <copyright file="Person.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.SharedKernel
{
    using Xeinaemm.Domain;

    public class Person : BaseEntity
    {
        public int Age { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
