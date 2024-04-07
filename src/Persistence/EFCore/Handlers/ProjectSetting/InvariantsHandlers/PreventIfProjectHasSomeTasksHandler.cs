using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectStore
{
    internal class PreventIfProjectHasSomeTasksHandler :
        IRequestHandler<PreventIfProjectHasSomeTasks, bool>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public PreventIfProjectHasSomeTasksHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(
            PreventIfProjectHasSomeTasks request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Projects
                .Where(p => !p.IsDeleted && p.Id == new Guid(request.Id) 
                && p.Tasks.Where(t => !t.IsDeleted).Any())
                .AnyAsync();

            await request.ResolveAsync(_mediator);
            return result;
        }
    }
}
