using MediatR;
using Domainify.Domain;

namespace Domain.Task
{
    public class GetTask :
        QueryItemRequestById<Task, string, TaskViewModel?>
    {
        public GetTask(string id) : base(id)
        {
            PreventIfNoEntityWasFound = true;
        }

        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator, Task task)
        {
            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, task);
        }
    }

    public class GetTaskHandler :
        IRequestHandler<GetTask, TaskViewModel?>
    {
        private readonly ITaskRepository _repository;
        public GetTaskHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskViewModel?> Handle(
            GetTask request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
