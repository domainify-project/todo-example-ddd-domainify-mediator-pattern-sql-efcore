using Domainify;
using Domainify.Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Persistence
{
    public class PreventIfTheEntityHasAlreadyExistedPreventer<TEntity, TDocument>
        : LogicalPreventer
        where TEntity : BaseEntity<TEntity>
    {
        private IMongoCollection<TDocument> _collection;
        private FilterDefinition<TDocument> _filter;

        public PreventIfTheEntityHasAlreadyExistedPreventer(
            IMongoCollection<TDocument> collection, FilterDefinition<TDocument> filter)
        {
            _collection = collection;
            _filter = filter;
        }

        public override async Task<bool> ResolveAsync()
        {
            return await _collection.Find(_filter).AnyAsync();
        }

        public override IIssue? GetIssue()
        {
            return new AnEntityWithTheseUniquenessConditionsHasAlreadyExisted(
                    typeof(TEntity).Name, Description);
        }
    }
}
