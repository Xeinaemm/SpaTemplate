// -----------------------------------------------------------------------
// <copyright file="StudentCompletedEvent.cs" company="Piotr Xeinaemm Czech">
// Copyright (c) Piotr Xeinaemm Czech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaTemplate.Core.FacultyContext
{
    using Xeinaemm.Domain;

    public class StudentCompletedEvent : BaseDomainEvent
    {
        public StudentCompletedEvent(Student completedItem) => this.CompletedItem = completedItem;

        public Student CompletedItem { get; }
    }
}