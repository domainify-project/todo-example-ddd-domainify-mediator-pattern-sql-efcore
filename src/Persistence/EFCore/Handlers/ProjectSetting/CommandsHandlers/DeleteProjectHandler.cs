using MediatR;
using Domain.ProjectSettingAggregation;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectStore
{
    public class DeleteProjectHandler :
        IRequestHandler<DeleteProject>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public DeleteProjectHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(
            DeleteProject request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Projects
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                itemToModify.IsDeleted = true;

            return new Unit();
        }
    }
}
