using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectSettingStore
{
    internal class PreventIfTheSameProjectHasAlreadyExistedHandler :
        IRequestHandler<PreventIfTheSameProjectHasAlreadyExisted, bool>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public PreventIfTheSameProjectHasAlreadyExistedHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(
            PreventIfTheSameProjectHasAlreadyExisted request,
            CancellationToken cancellationToken)
        {
            await request.ResolveAsync(_mediator);

            var query = _dbContext.Projects
                .Where(i => !i.IsDeleted && i.Name == request.Name);

            if (!string.IsNullOrEmpty(request.ProjectId))
                query = query.Where(i => i.Id != new Guid(request.ProjectId));

            return await query.AnyAsync();
        }
    }
}
