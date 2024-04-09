using MediatR;
using Domain.ProjectSettingAggregation;

namespace Persistence.ProjectSettingStore
{
    public class DefineSprintHandler :
        IRequestHandler<DefineSprint, string>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public DefineSprintHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<string> Handle(
            DefineSprint request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);
            var newItem  = SprintModel.InstanceOf(preparedEntity, request.ProjectId);
            _dbContext.Sprints.Add(newItem);
 
            return newItem.Id.ToString();
        }
    }
} 
