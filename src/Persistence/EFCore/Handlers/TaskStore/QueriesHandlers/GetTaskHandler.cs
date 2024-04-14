using Domain.TaskAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.TaskStore
{
    public class GetTaskHandler :
        IRequestHandler<GetTask, Domain.TaskAggregation.Task?>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public GetTaskHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
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
            var task = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, task!);

            return task;
        }
    }
}
