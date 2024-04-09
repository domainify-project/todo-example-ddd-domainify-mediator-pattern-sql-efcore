using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.TaskAggregation;

namespace Persistence.TaskStore
{
    internal class ChangeTaskSprintHandler :
        IRequestHandler<ChangeTaskSprint>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public ChangeTaskSprintHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(
            ChangeTaskSprint request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
            {
                itemToModify.SprintId = new Guid(request.SprintId!);
            }

            return new Unit();
        }
    }
}
