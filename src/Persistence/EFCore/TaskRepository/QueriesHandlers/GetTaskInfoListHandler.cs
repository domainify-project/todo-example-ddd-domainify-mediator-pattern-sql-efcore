using MediatR;
using XSwift.Datastore;
using Domain.TaskAggregation;
using EntityFrameworkCore.XSwift.Datastore;
using XSwift.Domain;

namespace Persistence.EFCore.TaskRepository
{
    public class GetTaskInfoListHandler :
        IRequestHandler<GetTaskInfoList,
            PaginatedViewModel<TaskInfo>>
    {
        private readonly Database _database;
        public GetTaskInfoListHandler(IDatabase database)
        {
            _database = (Database)database;
        }

        public async Task<PaginatedViewModel<TaskInfo>> Handle(
            GetTaskInfoList request,
            CancellationToken cancellationToken)
        {
            var query = _database.MakeQuery
               <GetTaskInfoList, Domain.TaskAggregation.Task>(request);

            return await _database.GetPaginatedListAsync(
                request: request,
                selector: (IQueryable<Domain.TaskAggregation.Task> query) =>
                {
                    return TaskQueryable.SelectAsTaskInfo(_database, query); 
                },
                filter: delegate (IQueryable<Domain.TaskAggregation.Task> query)
                {
                    return from task in query
                           where
                                (!string.IsNullOrEmpty(request.DescriptionSearchKey) ? task.Description.Contains(request.DescriptionSearchKey!) : true) &&
                                (request.SprintId != null ? task.SprintId == request.SprintId : true) &&
                                (request.Status != null ? task.Status == request.Status : true)
                           select task;
                });
        }
    }
}
