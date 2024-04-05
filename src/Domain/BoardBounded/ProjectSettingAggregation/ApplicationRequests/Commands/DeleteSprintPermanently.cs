using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class DeleteSprintPermanently :
        RequestToDeletePermanentlyById<Sprint, string>
    {
        public bool ToDeleteAllTaskStatus { get; private set; }

        public DeleteSprintPermanently(
            string id, bool toDeleteAllTaskStatus = false)
            : base(id)
        {
            ToDeleteAllTaskStatus = toDeleteAllTaskStatus;
            ValidationState.Validate();
        }

        public override async Task<Sprint> ResolveAndGetEntityAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);

            var sprint = (await mediator.Send(
                new FindSprint(Id, includeDeleted: true)))!;
            await base.ResolveAsync(mediator, sprint);

            return sprint;
        }
    }
}
