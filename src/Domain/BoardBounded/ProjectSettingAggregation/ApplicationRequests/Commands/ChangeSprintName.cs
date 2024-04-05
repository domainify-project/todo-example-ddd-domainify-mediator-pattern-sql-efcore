using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class ChangeSprintName :
        RequestToUpdateById<Sprint, string>
    {
        [BindTo(typeof(Sprint), nameof(Sprint.Name))]
        public string Name { get; private set; }
 
        public ChangeSprintName(string id, string name) 
            : base(id)
        {
            Name = name.Trim();
            ValidationState.Validate();
        }

        public override async Task<Sprint> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var sprint = (await mediator.Send(new FindSprint(id: Id)))!;
            sprint.SetName(Name);

            InvariantState.AddAnInvariantRequest(
                new PreventIfTheSameSprintHasAlreadyExisted(sprint));
            await InvariantState.AssestAsync(mediator);
 
            await base.ResolveAsync(mediator, sprint);
            return sprint;
        }
    }
}
