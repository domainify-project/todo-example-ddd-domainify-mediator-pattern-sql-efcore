using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.TaskAggregation;

namespace Persistence.TaskStore
{
    public class ChangeTaskStatusHandler :
        IRequestHandler<ChangeTaskStatus>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public ChangeTaskStatusHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(
            ChangeTaskStatus request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
            {
                itemToModify.Status = preparedEntity.Status;
            }

            return new Unit();
        }
    }
}
