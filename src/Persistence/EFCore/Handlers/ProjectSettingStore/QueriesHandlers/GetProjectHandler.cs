using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.ProjectSettingStore
{
    public class GetProjectHandler :
        IRequestHandler<GetProject, Project?>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public GetProjectHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Project?> Handle(
            GetProject request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Projects.AsNoTracking()
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            if (request.WithSprints)
                query = query.Include(i => i.Sprints);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var project = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, project!);

            return project;
        }
    }
}
