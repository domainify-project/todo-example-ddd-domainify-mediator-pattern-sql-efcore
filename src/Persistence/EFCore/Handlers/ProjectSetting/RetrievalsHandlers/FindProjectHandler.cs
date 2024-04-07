using Domain.ProjectSettingAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.ProjectStore
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
                .Where(p => p.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(p => p.IsDeleted == false);

            if (request.WithSprints)
                query = query.Include(p => p.Sprints);

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
