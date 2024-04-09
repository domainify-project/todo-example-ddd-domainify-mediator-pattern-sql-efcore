using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectSettingStore
{
    public class RestoreSprintHandler :
        IRequestHandler<RestoreSprint>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public RestoreSprintHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(
            RestoreSprint request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Sprints
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                itemToModify.IsDeleted = false;

            return new Unit();
        }
    }
}
