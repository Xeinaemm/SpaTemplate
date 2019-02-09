// -----------------------------------------------------------------------
// <copyright file="ApiController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure.Api
{
    using System.Collections.Generic;
    using System.Net.Mime;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SpaTemplate.Core.SharedKernel;
    using Xeinaemm.AspNetCore;
    using Xeinaemm.AspNetCore.Api;
    using Xeinaemm.Hateoas;

    /// <summary>
    ///
    /// </summary>
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Route("api/v{version:apiVersion}")]
    [ValidateModel]
    [ApiController]
    public class ApiController : Controller
    {
        private readonly IUrlHelper urlHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiController"/> class.
        /// </summary>
        /// <param name="urlHelper"></param>
        public ApiController(IUrlHelper urlHelper) => this.urlHelper = urlHelper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns>
        ///
        /// </returns>
        [HttpGet(Name = RouteName.GetRoot)]
        [Produces(MediaType.OutputFormatterJson)]
        [RequestHeaderMatchesMediaType("Accept", MediaTypeNames.Application.Json, MediaType.OutputFormatterJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType) =>
            mediaType != MediaType.OutputFormatterJson ? this.NoContent() : (IActionResult)this.Ok(this.RootDtoLinks());

        private string CreateHref(string routeName) => this.urlHelper.Link(routeName, new { });

        private List<LinkDto> RootDtoLinks() => new List<LinkDto>
        {
            this.CreateHref(RouteName.GetRoot)
                .AddRelAndMethod(Rel.Self, HttpMethods.Get),
            this.CreateHref(RouteName.GetPeople)
                .AddRelAndMethod(SpaTemplateRel.People, HttpMethods.Get),
            this.CreateHref(RouteName.CreateStudent)
                .AddRelAndMethod(SpaTemplateRel.CreateStudent, HttpMethods.Post),
        };
    }
}