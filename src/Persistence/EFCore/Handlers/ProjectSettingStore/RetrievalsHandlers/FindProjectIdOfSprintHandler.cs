using Domain.ProjectSettingAggregation;
using Domainify.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Persistence.ProjectSettingStore
{
    internal class FindProjectIdOfSprintHandler :
        IRequestHandler<FindProjectIdOfSprint, string?>
    {
        private readonly IMediator _mediator;
        private readonly TodoDbContext _dbContext;
        public FindProjectIdOfSprintHandler(
            IMediator mediator, TodoDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }
        public async Task<string?> Handle(
            FindProjectIdOfSprint request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Sprints
                .Where(i => i.Id == new Guid(request.Id));

            if (request.IncludeDeleted == false)
                query = query.Where(i => i.IsDeleted == false);

            var retrievedItem = await query.FirstOrDefaultAsync();
            var sprint = retrievedItem?.ToEntity();

            await request.ResolveAsync(_mediator, sprint!);

            return retrievedItem?.ProjectId.ToString();
        }
    }
}
