using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.TaskAggregation;

namespace Persistence.TaskStore
{
    public class EditTaskHandler :
        IRequestHandler<EditTask>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public EditTaskHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(
            EditTask request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Tasks
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
            {
                itemToModify.Description = preparedEntity.Description;
                itemToModify.Status = preparedEntity.Status;
                itemToModify.SprintId = request.SprintId == null ? null : Guid.Parse(request.SprintId);
            }

            return new Unit();
        }
    }
}
