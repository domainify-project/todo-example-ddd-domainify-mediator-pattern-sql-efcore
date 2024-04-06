using MediatR;
using Domain.ProjectSettingAggregation;
using Microsoft.EntityFrameworkCore;

namespace Persistence.BookletStore
{
    public class ChangeProjectNameHandler :
        IRequestHandler<ChangeProjectName>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public ChangeProjectNameHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(
            ChangeProjectName request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);
            
            var itemToUpdate = await _dbContext.Projects
                .FirstAsync(p => p.Id == new Guid(request.Id));  

            if (itemToUpdate != null)
            {
                itemToUpdate.Name = preparedEntity.Name;
            }

            return new Unit();
        }
    }
}
