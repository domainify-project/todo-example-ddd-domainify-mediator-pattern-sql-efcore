using MediatR;
using Domain.ProjectSettingAggregation;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectSettingStore
{
    public class ChangeSprintNameHandler :
        IRequestHandler<ChangeSprintName>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public ChangeSprintNameHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(
            ChangeSprintName request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);
            
            var itemToModify = await _dbContext.Sprints
                .FirstAsync(p => p.Id == new Guid(request.Id));  

            if (itemToModify != null)
            {
                itemToModify.Name = preparedEntity.Name;
            }

            return new Unit();
        }
    }
}
