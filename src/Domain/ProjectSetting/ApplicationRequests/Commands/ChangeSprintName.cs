using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
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
            var sprint = (await mediator.Send(new FindSprint(id: Id, preventIfNoEntityWasFound: true)))!;
            sprint.SetName(Name);

            base.Prepare(sprint);

            var parentProjectId = await mediator.Send(
                new FindProjectIdOfSprint(sprintId: sprint.Id));

            InvariantState.AddAnInvariantRequest(
                new PreventIfTheSameSprintHasAlreadyExisted(
                    name: sprint.Name, parentProjectId: parentProjectId!, sprintId: sprint.Id));
            await InvariantState.AssestAsync(mediator);
 
            await base.ResolveAsync(mediator, sprint);
            return sprint;
        }
    }

    public class ChangeSprintNameHandler :
        IRequestHandler<ChangeSprintName>
    {
        private readonly IProjectSettingRepository _repository;
        public ChangeSprintNameHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            ChangeSprintName request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
