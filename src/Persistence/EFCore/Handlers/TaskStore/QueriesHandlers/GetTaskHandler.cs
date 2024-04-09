using Domain.TaskAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.TaskStore
{
    public class GetTaskHandler :
        IRequestHandler<GetTask, Domain.TaskAggregation.Task?>
    {
        private readonly TodoDbContext _dbContext;
        public GetTaskHandler(
            TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.TaskAggregation.Task?> Handle(
            GetTask request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Tasks.AsNoTracking()
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            var retrievedItem = await query.FirstOrDefaultAsync();

            if (retrievedItem == null && request.PreventIfNoEntityWasFound)
            {
                await new LogicalState().AddFault(
                    new NoEntityWasFound(typeof(Domain.TaskAggregation.Task).Name))
                    .AssesstAsync();
            }

            return retrievedItem?.ToEntity();
        }
    }
}
