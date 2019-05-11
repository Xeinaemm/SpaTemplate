// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.IdP
{
	using System.Threading.Tasks;
	using IdentityServer4.Services;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Xeinaemm.AspNetCore.Identity;

	[SecurityHeaders]
	[AllowAnonymous]
	public class HomeController : Controller
	{
		private readonly IIdentityServerInteractionService interaction;
		private readonly IHostingEnvironment environment;

		public HomeController(IIdentityServerInteractionService interaction, IHostingEnvironment environment)
		{
			this.interaction = interaction;
			this.environment = environment;
		}

		public IActionResult Index() => this.environment.IsDevelopment() ? this.View() : (IActionResult)this.NotFound();

		public async Task<IActionResult> ErrorAsync(string errorId)
		{
			var vm = new ErrorViewModel();

			var message = await this.interaction.GetErrorContextAsync(errorId).ConfigureAwait(false);
			if (message != null)
			{
				vm.Error = message;

				if (!this.environment.IsDevelopment()) message.ErrorDescription = null;
			}

			return this.View(nameof(this.ErrorAsync), vm);
		}
	}
}