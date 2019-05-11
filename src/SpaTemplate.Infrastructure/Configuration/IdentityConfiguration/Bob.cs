﻿// -----------------------------------------------------------------------
// <copyright file="Bob.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Infrastructure
{
	using System.Collections.ObjectModel;
	using System.Security.Claims;
	using IdentityModel;
	using IdentityServer4;
	using Xeinaemm.Identity;

	public class Bob : IUser
	{
		public string SubjectId { get; } = "2";

		public string Username { get; } = "bob";

		public string Password { get; } = "bob";

		public Collection<Claim> Claims { get; } = new Collection<Claim>
		{
			new Claim(JwtClaimTypes.Name, "Bob Smith"),
			new Claim(JwtClaimTypes.GivenName, "Bob"),
			new Claim(JwtClaimTypes.FamilyName, "Smith"),
			new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
			new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
			new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
			new Claim(JwtClaimTypes.Address, "{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServerConstants.ClaimValueTypes.Json),
		};
	}
}
