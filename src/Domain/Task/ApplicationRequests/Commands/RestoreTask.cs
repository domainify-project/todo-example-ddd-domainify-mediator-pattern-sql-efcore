using Domainify.Domain;
using MediatR;

namespace Domain.Task
{
    public class RestoreTask :
        RequestToRestoreById<Task, string>
    {
        public RestoreTask(string id)
            : base(id)
        {
            ValidationState.Validate();
        }
        public override async Task<Task> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var task = (await mediator.Send(
                new FindTask(Id, includeDeleted: true,
                preventIfNoEntityWasFound: true)))!;

            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, task);

            return task;
        }
    }

    public class RestoreTaskHandler :
        IRequestHandler<RestoreTask>
    {
        private readonly ITaskRepository _repository;
        public RestoreTaskHandler(ITaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            RestoreTask request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
