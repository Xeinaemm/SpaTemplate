﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpaTemplate.Core.SharedKernel;

namespace SpaTemplate.Core.Hateoas
{
	public class PagedList<T> : List<T>, IPagedList<T> where T : BaseEntity
	{
		public PagedList()
		{
		}

		private PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			PageSize = pageSize;
			CurrentPage = pageNumber;
			TotalPages = (int) Math.Ceiling(count / (double) pageSize);
			AddRange(items);
		}

		public int CurrentPage { get; }
		public int TotalPages { get; }
		public int PageSize { get; }
		public int TotalCount { get; }

		public bool HasPrevious => CurrentPage > 1;

		public bool HasNext => CurrentPage < TotalPages;

		public static PagedList<T> Create(List<T> source, int pageNumber, int pageSize)
		{
			var items = source.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize);
			return new PagedList<T>(items, source.Count, pageNumber, pageSize);
		}
	}
}