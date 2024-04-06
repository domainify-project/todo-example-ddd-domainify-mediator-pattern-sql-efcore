using MediatR;
using XSwift.Datastore;
using Domain.TaskAggregation;
using EntityFrameworkCore.XSwift.Datastore;

namespace Persistence.EFCore.TaskRepository
{
    public class GetTheTaskInfoHandler :
        IRequestHandler<GetTheTaskInfo, TaskInfo?>
    {
        private readonly Database _database;
        public GetTheTaskInfoHandler(IDatabase database) 
        {
            _database = (Database)database;
        }

        public async Task<TaskInfo?> Handle(
            GetTheTaskInfo request,
            CancellationToken cancellationToken)
        {
            return await _database.
                GetItemAsync<GetTheTaskInfo, Domain.TaskAggregation.Task, TaskInfo>
                (request, selector: (IQueryable<Domain.TaskAggregation.Task> query) =>
                {
                    return TaskQueryable.SelectAsTaskInfo(_database, query);
                });
        }
    }
}
