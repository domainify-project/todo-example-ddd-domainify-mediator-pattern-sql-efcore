using Domain.ProjectSettingAggregation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectSettingStore
{
    public class DeleteProjectPermanentlyHandler :
        IRequestHandler<DeleteProjectPermanently>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public DeleteProjectPermanentlyHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(
            DeleteProjectPermanently request,
            CancellationToken cancellationToken)
        {
            var preparedEntity = await request.ResolveAndGetEntityAsync(_mediator);

            var itemToModify = await _dbContext.Projects
                .FirstAsync(p => p.Id == new Guid(request.Id));

            if (itemToModify != null)
                _dbContext.Projects.Remove(itemToModify);

            return new Unit();
        }
    }
}
