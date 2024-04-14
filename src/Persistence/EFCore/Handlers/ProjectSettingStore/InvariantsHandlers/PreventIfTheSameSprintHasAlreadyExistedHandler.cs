using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectSettingStore
{
    internal class PreventIfTheSameSprintHasAlreadyExistedHandler :
        IRequestHandler<PreventIfTheSameSprintHasAlreadyExisted, bool>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public PreventIfTheSameSprintHasAlreadyExistedHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(
            PreventIfTheSameSprintHasAlreadyExisted request,
            CancellationToken cancellationToken)
        {
            await request.ResolveAsync(_mediator);

            var query = _dbContext.Sprints
                .Where(i => !i.IsDeleted && i.ProjectId == new Guid(request.ParentProjectId!) 
                && i.Name == request.Name);

            if (!string.IsNullOrEmpty(request.SprintId))
                query = query.Where(i => i.Id != new Guid(request.SprintId));
            var res = await query.AnyAsync();

            return await query.AnyAsync();
        }
    }
}
