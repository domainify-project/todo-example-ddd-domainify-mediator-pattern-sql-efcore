using MediatR;
using Domain.TaskAggregation;
using Microsoft.EntityFrameworkCore;

namespace Persistence.TaskStore
{
    internal class SetTasksOfTheSprintToNoSprintHandler :
        IRequestHandler<SetTasksOfTheSprintToNoSprint>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public SetTasksOfTheSprintToNoSprintHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(
            SetTasksOfTheSprintToNoSprint request,
            CancellationToken cancellationToken)
        {
            await request.ResolveAsync(_mediator);
            return new Unit();
        }
    }
}
