using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectSettingStore
{
    internal class PreventIfSprintHasSomeTasksHandler :
        IRequestHandler<PreventIfSprintHasSomeTasks, bool>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public PreventIfSprintHasSomeTasksHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(
            PreventIfSprintHasSomeTasks request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Sprints
                .Where(i => !i.IsDeleted && i.Id == new Guid(request.Id)
                && i.Tasks.Where(t => !t.IsDeleted).Any())
                .AnyAsync();

            await request.ResolveAsync(_mediator);
            return result;
        }
    }
}
