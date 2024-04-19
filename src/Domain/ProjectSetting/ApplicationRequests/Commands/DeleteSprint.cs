using Domain.Task;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class DeleteSprint :
        RequestToDeleteById<Sprint, string>
    {
        public bool DeleteAllRelatedTask { get; private set; }

        public DeleteSprint(string id,
            bool deleteAllRelatedTask = false) 
            : base(id)
        {
            DeleteAllRelatedTask = deleteAllRelatedTask;
            ValidationState.Validate();
        }
        public override async Task<Sprint> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var sprint = (await mediator.Send(
                new FindSprint(Id, includeDeleted: true, preventIfNoEntityWasFound: true)))!;

            base.Prepare(sprint);

            await InvariantState.AssestAsync(mediator);

            if (DeleteAllRelatedTask)
            {
                await mediator.Send(new DeleteAllRelatedTasksOfSprint().SetSprintId(Id));
            }
            else
            {
                var tasksIdsList = await mediator.Send(new RetrieveTasksIdsListOfTheSprint()
                    .SetSprintId(Id));

                foreach (var taskId in tasksIdsList)
                    await mediator.Send(new ChangeTaskSprint(id: taskId, sprintId: null));
            }

            await base.ResolveAsync(mediator, sprint);

            return sprint;
        }
    }

    public class DeleteSprintHandler :
    IRequestHandler<DeleteSprint>
    {
        private readonly IProjectSettingRepository _repository;
        public DeleteSprintHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            DeleteSprint request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}