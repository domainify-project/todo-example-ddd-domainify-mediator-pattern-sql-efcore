﻿using Domainify;
using Domainify.Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class PreventIfNoEntityWasFound<TEntity, TModel>
        : LogicalPreventer
        where TEntity : BaseEntity<TEntity>
        where TModel : class
    {
        private IQueryable<TModel> _query;

        public PreventIfNoEntityWasFound(IQueryable<TModel> query)
        {
            _query = query;
        }

        public override async Task<bool> ResolveAsync()
        {
            return !await _query.AnyAsync();
        }

        public override IIssue? GetIssue()
        {
            return new NoEntityWasFound(typeof(TEntity).Name, Description);
        }
    }
}
