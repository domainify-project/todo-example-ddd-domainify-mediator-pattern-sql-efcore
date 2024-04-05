using MediatR;
using Domain.TaskAggregation;
using EntityFrameworkCore.XSwift.Datastore;
using XSwift.Datastore;

namespace Persistence.EFCore.TaskRepository
{
    internal class ChangeTheTaskSprintHandler :
        IRequestHandler<ChangeTheTaskSprint>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public ChangeTheTaskSprintHandler(
           IMediator mediator, IDatabase database)
        {
            _mediator = mediator;
            _database = (Database)database;
        }

        public async Task<Unit> Handle(
            ChangeTheTaskSprint request,
            CancellationToken cancellationToken)
        {
            var entity = await request.ResolveAndGetEntityAsync(_mediator);
            await _database.UpdateAsync(request, entity);
            return new Unit();
        }
    }
}
