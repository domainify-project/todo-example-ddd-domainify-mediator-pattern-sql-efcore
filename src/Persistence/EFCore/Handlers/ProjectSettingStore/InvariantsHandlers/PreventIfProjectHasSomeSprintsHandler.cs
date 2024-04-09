using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectSettingStore
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
                .Where(i => !i.IsDeleted && i.Id == new Guid(request.Id) 
                && i.Sprints.Where(s => !s.IsDeleted).Any())
                .AnyAsync();

            await request.ResolveAsync(_mediator);
            return result;
        }
    }
}
