using Domain.Task;
using Domainify.Domain;
using ThreadTask = System.Threading.Tasks.Task;

namespace Domain.Task
{
    public interface ITaskRepository
    {
        #region application requests
        public Task<string> Apply(AddTask request);
        public ThreadTask Apply(EditTask request);
        public ThreadTask Apply(DeleteTask request);
        public ThreadTask Apply(DeleteTaskPermanently request);
        public ThreadTask Apply(ChangeTaskStatus request);
        public ThreadTask Apply(RestoreTask request);
        public Task<TaskViewModel?> Apply(GetTask request);
        public Task<PaginatedList<TaskViewModel>> Apply(GetTasksList request);
        #endregion

        #region domain requests
        public ThreadTask Apply(ChangeTaskSprint request);
        public ThreadTask Apply(DeleteAllRelatedTasksOfSprint request);
        public Task<Task?> Apply(FindTask request);
        public Task<List<string>> Apply(RetrieveTasksIdsListOfTheSprint request);
        #endregion
    }
}
