// -----------------------------------------------------------------------
// <copyright file="CourseParameters.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Contracts.Parameters
{
    using Xeinaemm.Hateoas;

    public class CourseParameters : IParameters
    {
        private const int MaxPageSize = 50;

        private int pageSize = 10;

        public string Fields { get; set; }

        public string OrderBy { get; set; } = "Title";

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => this.pageSize;
            set => this.pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string SearchQuery { get; set; }
    }
}