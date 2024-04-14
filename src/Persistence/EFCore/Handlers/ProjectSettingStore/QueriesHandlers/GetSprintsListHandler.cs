using MediatR;
using Domainify.Domain;
using Microsoft.EntityFrameworkCore;
using Domain.ProjectSettingAggregation;

namespace Persistence.ProjectSettingStore
{
    public class GetSprintsListHandler :
        IRequestHandler<GetSprintsList,
            PaginatedList<SprintViewModel>>
    {
        private readonly TodoDbContext _dbContext;
        public GetSprintsListHandler(
            TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<SprintViewModel>> Handle(
            GetSprintsList request,
            CancellationToken cancellationToken)
        {
            var retrivalDeletationStatus = request.IsDeleted ?? false;
            if (request.IsDeleted == false && request.IncludeDeleted)
                retrivalDeletationStatus = true;

            var query = _dbContext.Sprints
                .AsNoTracking().Where(i => i.IsDeleted == retrivalDeletationStatus);

            if (request.WithTasks)
                query = query.Include(i => i.Tasks);

            query = query.SkipQuery(
                pageNumber: request.PageNumber, pageSize: request.PageSize);

            var retrievedItems = (await query.Select
                (i => new { Sprint = i, ProjectName = i.Project.Name }).ToListAsync())
                .Select(i => i.Sprint.ToEntity().ToViewModel()).ToList();

            var totalCount = await query.CountAsync();

            return new PaginatedList<SprintViewModel>(
                retrievedItems,
                numberOfTotalItems: totalCount,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);
        }
    }
}
