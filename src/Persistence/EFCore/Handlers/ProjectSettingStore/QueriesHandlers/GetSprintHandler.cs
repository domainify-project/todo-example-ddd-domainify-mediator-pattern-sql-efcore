using Domain.ProjectSettingAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.ProjectSettingStore
{
    public class GetSprintHandler :
        IRequestHandler<GetSprint, Sprint?>
    {
        private readonly TodoDbContext _dbContext;
        public GetSprintHandler(
            TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sprint?> Handle(
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

            if (retrievedItem == null && request.PreventIfNoEntityWasFound)
            {
                await new LogicalState().AddFault(
                    new NoEntityWasFound(typeof(Sprint).Name))
                    .AssesstAsync();
            }

            return retrievedItem?.ToEntity();
        }
    }
}
