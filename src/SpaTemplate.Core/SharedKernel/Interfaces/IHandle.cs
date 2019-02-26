﻿namespace SpaTemplate.Core.SharedKernel
{
	public interface IHandle<in T> where T : BaseDomainEvent
	{
		void Handle(T domainEvent);
	}
}