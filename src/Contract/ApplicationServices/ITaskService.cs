using Domain.Task;
using Domainify.Domain;

namespace Contract
{
    public interface ITaskService
    {
        public Task<string> Process(AddTask request);
        public System.Threading.Tasks.Task Process(EditTask request);
        public System.Threading.Tasks.Task Process(DeleteTask request);
        public System.Threading.Tasks.Task Process(DeleteTaskPermanently request);
        public System.Threading.Tasks.Task Process(ChangeTaskStatus request);
        public System.Threading.Tasks.Task Process(RestoreTask request);
        public Task<TaskViewModel?> Process(GetTask request);
        public Task<PaginatedList<TaskViewModel>> Process(GetTasksList request);
        public Task<List<KeyValuePair<int, string>>> Process(GetTaskStatusList request);
    }
}
