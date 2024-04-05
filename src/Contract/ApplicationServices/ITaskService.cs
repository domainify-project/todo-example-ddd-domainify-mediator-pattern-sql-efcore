using Domain.TaskAggregation;
using Domainify.Domain;

namespace Contract
{
    public interface ITaskService
    {
        public Task<Guid> Process(AddNewTask request);
        public System.Threading.Tasks.Task Process(EditTask request);
        public System.Threading.Tasks.Task Process(DeleteTask request);
        public System.Threading.Tasks.Task Process(ChangeTaskStatus request);
        public System.Threading.Tasks.Task Process(RestoreTask request);
        public Task<TaskViewModel?> Process(GetTask request);
        public Task<PaginatedList<TaskViewModel>> Process(GetTaskList request);
        public Task<List<KeyValuePair<int, string>>> Process(GetTaskStatusList request);
    }
}
