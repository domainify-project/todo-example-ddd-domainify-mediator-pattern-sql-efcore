using Domain.ProjectSettingAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.ProjectSettingStore
{
    public class GetSprintHandler :
        IRequestHandler<GetSprint, SprintViewModel?>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public GetSprintHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<SprintViewModel?> Handle(
            GetSprint request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Sprints.AsNoTracking()
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var sprint = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, sprint!);

            return new SprintViewModel(sprint!);
        }
    }
}
