using MediatR;
using Domainify.Domain;

namespace Domain.Task
{
    public class FindTask :
        QueryItemRequestById<Task, string, Task?>
    {
        public FindTask(string id,
            bool includeDeleted = false, bool preventIfNoEntityWasFound = false)
            : base(id)
        {
            IncludeDeleted = includeDeleted;
            PreventIfNoEntityWasFound = preventIfNoEntityWasFound;
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator, Task task)
        {
            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, task);
        }
    }

    public class FindTaskHandler :
        IRequestHandler<FindTask, Task?>
    {
        private readonly ITaskRepository _repository;
        public FindTaskHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<Task?> Handle(
            FindTask request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
