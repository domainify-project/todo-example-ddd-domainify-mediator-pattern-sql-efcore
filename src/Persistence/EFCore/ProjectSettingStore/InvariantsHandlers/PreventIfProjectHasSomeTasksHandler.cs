using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectSettingStore
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
                .Where(i => !i.IsDeleted && i.Id == new Guid(request.Id) 
                && i.Tasks.Where(t => !t.IsDeleted).Any())
                .AnyAsync();

            await request.ResolveAsync(_mediator);
            return result;
        }
    }
}
