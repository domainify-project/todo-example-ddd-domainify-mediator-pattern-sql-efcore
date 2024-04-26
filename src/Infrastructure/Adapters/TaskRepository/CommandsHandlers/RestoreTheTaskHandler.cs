using MediatR;
using Domain.TaskAggregation;
using EntityFrameworkCore.XSwift.Datastore;
using XSwift.Datastore;

namespace Persistence.EFCore.TaskRepository
{
    public class RestoreTheTaskHandler :
        IRequestHandler<RestoreTheTask>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public RestoreTheTaskHandler(
           IMediator mediator, IDatabase database)
        {
            _mediator = mediator;
            _database = (Database)database;

            _database.ResolveSoftDeleteConfiguration(
                new ModuleDeletabilityConfigurationFactory()
                .CreateInstance(database));
        }

        public async Task<Unit> Handle(
            RestoreTheTask request,
            CancellationToken cancellationToken)
        {
            var entity = await request.ResolveAndGetEntityAsync(_mediator);
            await _database.RestoreAsync(request , entity);
            return new Unit();
        }
    }
}
