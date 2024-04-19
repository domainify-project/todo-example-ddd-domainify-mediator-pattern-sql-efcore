using Domainify;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class PreventIfTheSameSprintHasAlreadyExisted
        : InvariantRequest<Sprint>
    {
        [BindTo(typeof(Project), nameof(Project.Id))]
        public string ParentProjectId { get; private set; }

        [BindTo(typeof(Sprint), nameof(Sprint.Id))]
        public string? SprintId { get; private set; }

        [BindTo(typeof(Sprint), nameof(Sprint.Name))]
        public string Name { get; protected set; }

        public PreventIfTheSameSprintHasAlreadyExisted(
            string name,
            string parentProjectId,
            string? sprintId = null)
        {
            Name = name;
            ParentProjectId = parentProjectId;
            SprintId = sprintId;
        }
        public override IFault? GetFault()
        {
            return new AnEntityWithThesePropertiesHasAlreadyExistedFault(
                    typeof(Sprint).Name, Description);
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }

    public class PreventIfTheSameSprintHasAlreadyExistedHandler :
        IRequestHandler<PreventIfTheSameSprintHasAlreadyExisted, bool>
    {
        private readonly IProjectSettingRepository _repository;
        public PreventIfTheSameSprintHasAlreadyExistedHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            PreventIfTheSameSprintHasAlreadyExisted request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
