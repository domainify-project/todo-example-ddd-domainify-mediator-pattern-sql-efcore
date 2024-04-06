using XSwift.Datastore;
using Domain.SprintAggregation;

namespace Persistence.EFCore.SprintRepository
{
    public class SprintQueryable
    {
        public static IQueryable<SprintInfo> SelectAsSprintInfo(
            IDatabase database,
            IQueryable<Sprint> query)
        {
            var dbContext = database.GetDbContext<ModuleDbContext>();
            return from sprint in query
                   join project in dbContext.Projects on
                   sprint.ProjectId equals project.Id into projectGroup
                   from groupedItem in projectGroup.DefaultIfEmpty()
                   select SprintInfo.ToModel(
                       sprint,
                       groupedItem.Name);
        }
    }
}
