using Domainify.Domain;
using MediatR;

namespace Domain.Task
{
    public class DeleteAllRelatedTasksOfSprint :
        BulkCommandRequest<Task, Unit>
    {
        public string SprintId { get; private set; } = string.Empty;

        public DeleteAllRelatedTasksOfSprint()
        {
            ValidationState.Validate();
        }

        public DeleteAllRelatedTasksOfSprint SetSprintId(string value)
        {
            SprintId = value;
            return this;
        }

        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);

            var tasksIdsList = await mediator.Send(
                new RetrieveTasksIdsListOfTheSprint().SetSprintId(SprintId));

            foreach (string taskId in tasksIdsList)
                await mediator.Send(new DeleteTask(taskId));
        }
    }

    public class DeleteAllRelatedTasksOfSprintHandler :
        IRequestHandler<DeleteAllRelatedTasksOfSprint>
    {
        private readonly ITaskRepository _repository;
        public DeleteAllRelatedTasksOfSprintHandler(ITaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            DeleteAllRelatedTasksOfSprint request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
