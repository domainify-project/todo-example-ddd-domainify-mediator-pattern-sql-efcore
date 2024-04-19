using Domainify.Domain;
using MediatR;

namespace Domain.Task
{
    public class ChangeTaskSprint :
        RequestToUpdateById<Task, string>
    {
        public string? SprintId { get; private set; }

        public ChangeTaskSprint(string id, string? sprintId = null) 
            : base(id)
        {
            SprintId = sprintId;

            ValidationState.Validate();
        }

        public override async Task<Task> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);

            var task = (await mediator.Send(new FindTask(Id)))!;
 
            await base.ResolveAsync(mediator, task);
            return task;
        }
    }

    public class ChangeTaskSprintHandler :
        IRequestHandler<ChangeTaskSprint>
    {
        private readonly ITaskRepository _repository;
        public ChangeTaskSprintHandler(ITaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            ChangeTaskSprint request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
