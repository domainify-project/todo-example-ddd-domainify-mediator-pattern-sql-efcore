using Domain.TaskAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.TaskStore
{
    internal class FindTaskHandler :
        IRequestHandler<FindTask, Domain.TaskAggregation.Task?>
    {
        private readonly TodoDbContext _dbContext;
        public FindTaskHandler(
            TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Domain.TaskAggregation.Task?> Handle(
            FindTask request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Tasks
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);


            var retrievedItem = await query.FirstOrDefaultAsync();

            if(retrievedItem == null && request.PreventIfNoEntityWasFound)
            {
                await new LogicalState().AddFault(
                    new NoEntityWasFound(typeof(Domain.TaskAggregation.Task).Name))
                    .AssesstAsync();
            }

            return retrievedItem?.ToEntity();
        }
    }
}
