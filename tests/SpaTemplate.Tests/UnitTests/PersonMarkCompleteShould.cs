﻿// -----------------------------------------------------------------------
// <copyright file="PersonMarkCompleteShould.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Tests.UnitTests
{
	using SpaTemplate.Core.FacultyContext;
	using Xunit;

	public class PersonMarkCompleteShould
	{
		[Fact]
		public void RaiseCompletedEvent()
		{
			var item = new Student();
			item.MarkComplete();

			_ = Assert.Single(item.Events);
			_ = Assert.IsType<StudentCompletedEvent>(item.Events[0]);
		}

		[Fact]
		public void ReturnsTrue_SetIsDone()
		{
			var sut = new Student();
			sut.MarkComplete();

			Assert.True(sut.IsDone);
		}
	}
}