﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using SpaTemplate.Core;

namespace SpaTemplate.Infrastructure
{
	public class AppDbContext : DbContext
	{
		private readonly IDomainEventDispatcher _dispatcher;

		public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher)
			: base(options) => _dispatcher = dispatcher;

		public DbSet<Person> People { get; set; }
		public DbSet<Course> Courses { get; set; }

		public override int SaveChanges()
		{
			var result = base.SaveChanges();

			var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
				.Select(e => e.Entity)
				.Where(e => e.Events.Any());

			foreach (var entity in entitiesWithEvents)
			{
				var events = entity.Events;
				entity.Events.Clear();
				foreach (var domainEvent in events) _dispatcher.Dispatch(domainEvent);
			}

			return result;
		}
	}
}