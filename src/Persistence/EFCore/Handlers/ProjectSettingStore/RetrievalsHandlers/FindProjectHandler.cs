using Domain.ProjectSettingAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.ProjectSettingStore
{
    internal class FindProjectHandler :
        IRequestHandler<FindProject, Project?>
    {
        private readonly TodoDbContext _dbContext;
        public FindProjectHandler(
            TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Project?> Handle(
            FindProject request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Projects
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            if (request.WithSprints)
                query = query.Include(i => i.Sprints);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            var retrievedItem = await query.FirstOrDefaultAsync();

            if(retrievedItem == null && request.PreventIfNoEntityWasFound)
            {
                await new LogicalState().AddFault(
                    new NoEntityWasFound(typeof(Project).Name))
                    .AssesstAsync();
            }

            return retrievedItem?.ToEntity();
        }
    }
}
