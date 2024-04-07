using Domain.TaskAggregation;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class DeleteSprint :
        RequestToDeleteById<Sprint, string>
    {
        public bool ToDeleteAllTaskStatus { get; private set; }

        public DeleteSprint(string id,
            bool toDeleteAllTaskStatus = false) 
            : base(id)
        {
            ToDeleteAllTaskStatus = toDeleteAllTaskStatus;
            ValidationState.Validate();
        }
        public override async Task<Sprint> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);

            var sprint = (await mediator.Send(
                new FindSprint(Id, includeDeleted: true, preventIfNoEntityWasFound: true)))!;
            await base.ResolveAsync(mediator, sprint);

            if (ToDeleteAllTaskStatus)
            {
                await mediator.Send(new SetTasksOfTheSprintToNoSprint()
                    .SetSprintId(Id));
            }
            else
            {
                var tasksIdsList = await mediator.Send(new RetrieveTasksIdsListOfTheSprint()
                    .SetSprintId(Id));

                foreach (var taskId in tasksIdsList)
                    await mediator.Send(new ChangeTaskSprint(id: taskId, sprintId: null));
            }

            return sprint;
        }
    }
}