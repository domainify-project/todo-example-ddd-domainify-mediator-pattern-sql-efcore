using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectStore
{
    internal class PreventIfProjectHasSomeSprintsHandler :
        IRequestHandler<PreventIfProjectHasSomeSprints, bool>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public PreventIfProjectHasSomeSprintsHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(
            PreventIfProjectHasSomeSprints request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Projects
                .Where(p => !p.IsDeleted && p.Id == new Guid(request.Id) 
                && p.Sprints.Where(s => !s.IsDeleted).Any())
                .AnyAsync();

            await request.ResolveAsync(_mediator);
            return result;
        }
    }
}
