using Domain.TaskAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.TaskStore
{
    public class GetTaskHandler :
        IRequestHandler<GetTask, TaskViewModel?>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public GetTaskHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<TaskViewModel?> Handle(
            GetTask request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Tasks
                .Include(i => i.Project)
                .Include(i => i.Sprint!)
                .AsNoTracking()
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var task = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, task!);

            return  new TaskViewModel(task!, 
                projectName: retrievedItem!.Project.Name,
                sprintName: retrievedItem.Sprint?.Name) ;
        }
    }
}
