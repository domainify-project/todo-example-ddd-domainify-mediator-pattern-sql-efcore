using Domainify;
using Domainify.Domain;

namespace Persistence
{
    public class PreventIfTheEntityHasAlreadyExisted<TEntity, TDocument>
        : LogicalPreventer
        where TEntity : BaseEntity<TEntity>
    {
        //private IMongoCollection<TDocument> _collection;
        //private FilterDefinition<TDocument> _filter;

        //public PreventIfTheEntityHasAlreadyExistedPreventer(
        //    IMongoCollection<TDocument> collection, FilterDefinition<TDocument> filter)
        //{
        //    _collection = collection;
        //    _filter = filter;
        //}

        public override async Task<bool> ResolveAsync()
        {
            throw new NotImplementedException();
            //return await _collection.Find(_filter).AnyAsync();
        }

        public override IFault? GetFault()
        {
            return new AnEntityWithTheseUniquenessConditionsHasAlreadyExisted(
                    typeof(TEntity).Name, Description);
        }
    }
}
