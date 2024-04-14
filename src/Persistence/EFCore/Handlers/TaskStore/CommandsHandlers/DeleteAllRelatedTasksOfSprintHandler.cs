using MediatR;
using Domain.TaskAggregation;

namespace Persistence.TaskStore
{
    internal class DeleteAllRelatedTasksOfSprintHandler :
        IRequestHandler<DeleteAllRelatedTasksOfSprint>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public DeleteAllRelatedTasksOfSprintHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(
            DeleteAllRelatedTasksOfSprint request,
            CancellationToken cancellationToken)
        {
            await request.ResolveAsync(_mediator);
            return new Unit();
        }
    }
}
