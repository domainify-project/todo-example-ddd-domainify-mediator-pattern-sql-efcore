using XSwift.Datastore;
using Domain.TaskAggregation;

namespace Persistence.EFCore.TaskRepository
{
    public class TaskQueryable
    {
        public static IQueryable<Guid> SelectAsTaskId(
            IDatabase database, 
            IQueryable<Domain.TaskAggregation.Task> query)
        {
            var dbContext = database.GetDbContext<ModuleDbContext>();
            return from task in query
                   select task.Id;
        }

        public static IQueryable<TaskInfo> SelectAsTaskInfo(
            IDatabase database,
            IQueryable<Domain.TaskAggregation.Task> query)
        {
            var dbContext = database.GetDbContext<ModuleDbContext>();
            return from task in query
                   join project in dbContext.Projects on
                   task.ProjectId equals project.Id
                   join sprint in dbContext.Sprints on
                   task.SprintId equals sprint.Id into sprintGroup
                   from groupedItem in sprintGroup.DefaultIfEmpty()
                   select TaskInfo.ToModel(task, project.Name, groupedItem.Id, groupedItem.Name, null, "");
        }
    }
}
