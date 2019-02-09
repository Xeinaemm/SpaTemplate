// -----------------------------------------------------------------------
// <copyright file="CustomIdentityDbContext.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class CustomIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
