using MediatR;
using XSwift.Datastore;
using Domain.SprintAggregation;
using EntityFrameworkCore.XSwift.Datastore;

namespace Persistence.EFCore.SprintRepository
{
    public class GetTheSprintInfoHandler :
        IRequestHandler<GetTheSprintInfo, SprintInfo?>
    {
        private readonly Database _database;
        public GetTheSprintInfoHandler(IDatabase database) 
        {
            _database = (Database)database;
        }

        public async Task<SprintInfo?> Handle(
            GetTheSprintInfo request,
            CancellationToken cancellationToken)
        {
            return await _database.
                GetItemAsync<GetTheSprintInfo, Sprint, SprintInfo>
                (request, selector: (IQueryable<Sprint> query) =>
                {
                    return SprintQueryable.SelectAsSprintInfo(_database, query);
                });
        }
    }
}
