using MediatR;
using Domain.ProjectSettingAggregation;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectStore
{
    public class DefineProjectHandler :
        IRequestHandler<DefineProject, string>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public DefineProjectHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<string> Handle(
            DefineProject request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);
            var newModel  = ProjectModel.InstanceOf(preparedEntity);
            _dbContext.Projects.Add(newModel);
 
            return newModel.Id.ToString();
        }
    }
} 
