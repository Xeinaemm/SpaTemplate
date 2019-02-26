﻿using System;
using System.Collections.Generic;
using SpaTemplate.Core.Hateoas;

namespace SpaTemplate.Core.SharedKernel
{
    public interface IRepository
    {
        TEntity GetEntity<TEntity>(Guid id) where TEntity : BaseEntity;
        bool AddEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;
        bool DeleteEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;
        bool ExistsEntity<TEntity>(Guid entity) where TEntity : BaseEntity;
        bool UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

        List<TEntity> GetCollection<TEntity>(ISpecification<TEntity> specification = null, SpecificationQueryMode mode = SpecificationQueryMode.None)
            where TEntity : BaseEntity;

        PagedList<TEntity> GetCollection<TEntity, TDto>(IParameters parameters,
            ISpecification<TEntity> specification = null, SpecificationQueryMode mode = SpecificationQueryMode.None)
            where TDto : IDto where TEntity : BaseEntity;

        TEntity GetFirstOrDefault<TEntity>(ISpecification<TEntity> specification = null, SpecificationQueryMode mode = SpecificationQueryMode.None)
            where TEntity : BaseEntity;
    }
}