using Domain.TaskAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.TaskStore
{
    internal class FindTaskHandler :
        IRequestHandler<FindTask, Domain.TaskAggregation.Task?>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public FindTaskHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
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
            var task = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, task!);

            return task;
        }
    }
}
