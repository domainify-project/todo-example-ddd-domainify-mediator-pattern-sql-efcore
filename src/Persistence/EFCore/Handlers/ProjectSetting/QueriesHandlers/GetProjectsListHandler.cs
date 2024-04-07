using MediatR;
using Domainify.Domain;
using Domain.ProjectSettingAggregation;
using Microsoft.EntityFrameworkCore;

namespace Persistence.ProjectStore
{
    public class GetProjectsListHandler :
        IRequestHandler<GetProjectsList,
            PaginatedList<ProjectViewModel>>
    {
        private readonly TodoDbContext _dbContext;
        public GetProjectsListHandler(
            TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<ProjectViewModel>> Handle(
            GetProjectsList request,
            CancellationToken cancellationToken)
        {
            var retrivalDeletationStatus = request.IsDeleted ?? false;
            if (request.IsDeleted == false && request.IncludeDeleted)
                retrivalDeletationStatus = true;

            var query = _dbContext.Projects.AsNoTracking().Where(p => p.IsDeleted == retrivalDeletationStatus);

            query = query.SkipQuery(
                pageNumber: request.PageNumber, pageSize: request.PageSize);

            var retrievedItems = (await query.ToListAsync())
                .Select(i => i.ToEntity().ToViewModel()).ToList();

            var totalCount = await query.CountAsync();

            return new PaginatedList<ProjectViewModel>(
                retrievedItems,
                numberOfTotalItems: totalCount,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);
        }
    }
}
