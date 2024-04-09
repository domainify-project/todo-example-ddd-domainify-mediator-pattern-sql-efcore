using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.TaskAggregation;

namespace Persistence.TaskStore
{
    internal class RetrieveTasksIdsListOfTheSprintHandler :
        IRequestHandler<RetrieveTasksIdsListOfTheSprint,
            List<string>>
    {
        private readonly TodoDbContext _dbContext;
        public RetrieveTasksIdsListOfTheSprintHandler(
            TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> Handle(
            RetrieveTasksIdsListOfTheSprint request,
            CancellationToken cancellationToken)
        {
            var tasksIdsList = await _dbContext.Tasks
                .Where(i => i.SprintId == new Guid(request.SprintId))
                .Select(i => i.Id.ToString()).ToListAsync();

            return tasksIdsList;
        }
    }
}
