using MediatR;
using XSwift.Datastore;
using EntityFrameworkCore.XSwift.Datastore;
using Domain.TaskAggregation;

namespace Persistence.EFCore.TaskRepository
{
    internal class SetTheTasksOfTheSprintToNoSprintHandler :
        IRequestHandler<SetTheTasksOfTheSprintToNoSprint>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public SetTheTasksOfTheSprintToNoSprintHandler(
            IMediator mediator, IDatabase database)
        {
            _mediator = mediator;
            _database = (Database)database;
        }

        public async Task<Unit> Handle(
            SetTheTasksOfTheSprintToNoSprint request,
            CancellationToken cancellationToken)
        {
            var tasksIds = await _database.GetListAsync(
                           request: request,
                           selector: (IQueryable<Domain.TaskAggregation.Task> query) => {
                               return from task in query
                                      select task.Id; 
                           });

            await request.ResolveAsync(tasksIds, _mediator);
            return new Unit();
        }
    }
}
