using Domain.ProjectSettingAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.ProjectSettingStore
{
    internal class FindSprintHandler :
        IRequestHandler<FindSprint, Sprint?>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public FindSprintHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Sprint?> Handle(
            FindSprint request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Sprints
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var sprint = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, sprint!);

            return sprint;
        }
    }
}
