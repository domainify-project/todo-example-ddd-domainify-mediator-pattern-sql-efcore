using MediatR;
using XSwift.Datastore;
using Domain.SprintAggregation;
using EntityFrameworkCore.XSwift.Datastore;

namespace Persistence.EFCore.SprintRepository
{
    internal class GetTheSprintHandler :
        IRequestHandler<GetTheSprint, Sprint?>
    {
        private readonly Database _database;
        public GetTheSprintHandler(IDatabase database)
        {
            _database = (Database)database;
        }

        public async Task<Sprint?> Handle(
            GetTheSprint request,
            CancellationToken cancellationToken)
        {
            return await _database.GetItemAsync<GetTheSprint, Sprint>(request: request);
        }
    }
}
