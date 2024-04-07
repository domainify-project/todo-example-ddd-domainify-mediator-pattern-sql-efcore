using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectStore
{
    public class RestoreProjectHandler :
        IRequestHandler<RestoreProject>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public RestoreProjectHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(
            RestoreProject request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Projects
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                itemToModify.IsDeleted = false;

            return new Unit();
        }
    }
}
