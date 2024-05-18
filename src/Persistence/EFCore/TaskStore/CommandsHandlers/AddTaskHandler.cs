using MediatR;
using Domain.TaskAggregation;

namespace Persistence.TaskStore
{
    public class AddTaskHandler :
        IRequestHandler<AddTask, string>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public AddTaskHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<string> Handle(
            AddTask request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);
            var newItem  = TaskModel.InstanceOf(
                preparedEntity,
                projectId: request.ProjectId,
                sprintId: request.SprintId);

            _dbContext.Tasks.Add(newItem);
 
            return newItem.Id.ToString();
        }
    }
} 
