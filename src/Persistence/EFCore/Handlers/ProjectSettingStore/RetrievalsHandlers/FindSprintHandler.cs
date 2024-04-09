using Domain.ProjectSettingAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.ProjectSettingStore
{
    internal class FindSprintHandler :
        IRequestHandler<FindSprint, Sprint?>
    {
        private readonly TodoDbContext _dbContext;
        public FindSprintHandler(
            TodoDbContext dbContext)
        {
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

            if(retrievedItem == null && request.PreventIfNoEntityWasFound)
            {
                await new LogicalState().AddFault(
                    new NoEntityWasFound(typeof(Sprint).Name))
                    .AssesstAsync();
            }

            return retrievedItem?.ToEntity();
        }
    }
}
