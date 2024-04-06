using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.ProjectStore
{
    internal class PreventIfTheSamProjectHasAlreadyExistedHandler :
        IRequestHandler<PreventIfTheSameProjectHasAlreadyExisted, bool>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public PreventIfTheSamProjectHasAlreadyExistedHandler(
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
                .Where(p => p.Name == request.Project.Name);

            if (!string.IsNullOrEmpty(request.Project.Id))
                query = query.Where(p => p.Id != new Guid(request.Project.Id));

            if (request.Project.Uniqueness() != null && request.Project.Uniqueness()!.Condition != null)
                return await query.AnyAsync();

            return false;
        }
    }
}
