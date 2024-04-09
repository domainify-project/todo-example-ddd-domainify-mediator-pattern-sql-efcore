using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectSettingStore
{
    public class DeleteSprintPermanentlyHandler :
        IRequestHandler<DeleteSprintPermanently>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public DeleteSprintPermanentlyHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(
            DeleteSprintPermanently request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Sprints
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                _dbContext.Sprints.Remove(itemToModify);

            return new Unit();
        }
    }
}
