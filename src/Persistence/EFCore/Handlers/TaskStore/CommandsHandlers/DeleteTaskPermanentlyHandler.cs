using Domain.TaskAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.TaskStore
{
    public class DeleteTaskPermanentlyHandler :
        IRequestHandler<DeleteTaskPermanently>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public DeleteTaskPermanentlyHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(
            DeleteTaskPermanently request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                _dbContext.Tasks.Remove(itemToModify);

            return new Unit();
        }
    }
}
