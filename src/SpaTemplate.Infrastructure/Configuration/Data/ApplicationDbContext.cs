// -----------------------------------------------------------------------
// <copyright file="ApplicationDbContext.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SpaTemplate.Core.FacultyContext;
    using Xeinaemm.Domain;

    public class ApplicationDbContext : DbContext
    {
        private readonly IDomainEventDispatcher dispatcher;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options) => this.dispatcher = dispatcher;

        public DbSet<Course> Courses { get; set; }

        public DbSet<Student> People { get; set; }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();

            var entitiesWithEvents = this.ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Count > 0);

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events;
                entity.Events.Clear();
                foreach (var domainEvent in events) this.dispatcher.Dispatch(domainEvent);
            }

            return result;
        }
    }
}