using MediatR;
using Domain.TaskAggregation;
using EntityFrameworkCore.XSwift.Datastore;
using XSwift.Datastore;

namespace Persistence.EFCore.TaskRepository
{
    public class ChangeTheTaskStatusHandler :
        IRequestHandler<ChangeTheTaskStatus>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public ChangeTheTaskStatusHandler(
           IMediator mediator, IDatabase database)
        {
            _mediator = mediator;
            _database = (Database)database;
        }

        public async Task<Unit> Handle(
            ChangeTheTaskStatus request,
            CancellationToken cancellationToken)
        {
            var entity = await request.ResolveAndGetEntityAsync(_mediator);
            await _database.UpdateAsync(request, entity);
            return new Unit();
        }
    }
}
