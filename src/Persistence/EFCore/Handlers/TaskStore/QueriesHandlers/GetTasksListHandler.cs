using MediatR;
using Domainify.Domain;
using Microsoft.EntityFrameworkCore;
using Domain.TaskAggregation;

namespace Persistence.TaskStore
{
    public class GetTasksListHandler :
        IRequestHandler<GetTasksList,
            PaginatedList<TaskViewModel>>
    {
        private readonly TodoDbContext _dbContext;
        public GetTasksListHandler(
            TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<TaskViewModel>> Handle(
            GetTasksList request,
            CancellationToken cancellationToken)
        {
            var retrivalDeletationStatus = request.IsDeleted ?? false;
            if (request.IsDeleted == false && request.IncludeDeleted)
                retrivalDeletationStatus = true;

            var query = _dbContext.Tasks.AsNoTracking().Where(i => i.IsDeleted == retrivalDeletationStatus);

            query = query.SkipQuery(
                pageNumber: request.PageNumber, pageSize: request.PageSize);

            var retrievedItems = (await query.ToListAsync())
                .Select(i => i.ToEntity().ToViewModel()).ToList();

            var totalCount = await query.CountAsync();

            return new PaginatedList<TaskViewModel>(
                retrievedItems,
                numberOfTotalItems: totalCount,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);
        }
    }
}
