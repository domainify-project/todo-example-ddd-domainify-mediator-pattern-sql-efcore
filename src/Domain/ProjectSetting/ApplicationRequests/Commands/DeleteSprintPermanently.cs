using Domain.TaskAggregation;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class DeleteSprintPermanently :
        RequestToDeletePermanentlyById<Sprint, string>
    {
        public bool DeleteAllRelatedTask { get; private set; }

        public DeleteSprintPermanently(
            string id, bool deleteAllRelatedTask = false)
            : base(id)
        {
            DeleteAllRelatedTask = deleteAllRelatedTask;
            ValidationState.Validate();
        }

        public override async Task<Sprint> ResolveAndGetEntityAsync(IMediator mediator)
        {
            var sprint = (await mediator.Send(
                new FindSprint(Id, 
                includeDeleted: true, 
                preventIfNoEntityWasFound: true)))!;

            base.Prepare(sprint);

            await InvariantState.AssestAsync(mediator);

            if (DeleteAllRelatedTask)
            {
                // It will be done automatically because of the database configuration.
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
}
