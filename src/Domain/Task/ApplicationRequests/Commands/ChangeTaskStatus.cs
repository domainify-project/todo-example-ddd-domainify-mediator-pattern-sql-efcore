using Domainify.Domain;
using MediatR;

namespace Domain.Task
{
    public class ChangeTaskStatus :
        RequestToUpdateById<Task, string>
    {
        public TaskStatus Status { get; private set; }

        public ChangeTaskStatus(string id, TaskStatus status) 
            : base(id)
        {
            Status = status;

            ValidationState.Validate();
        }

        public override async Task<Task> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var task = (await mediator.Send(new FindTask(Id)))!;
            task.SetStatus(Status);

            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);
 
            await base.ResolveAsync(mediator, task);
            return task;
        }
    }

    public class ChangeTaskStatusHandler :
        IRequestHandler<ChangeTaskStatus>
    {
        private readonly ITaskRepository _repository;
        public ChangeTaskStatusHandler(ITaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            ChangeTaskStatus request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
