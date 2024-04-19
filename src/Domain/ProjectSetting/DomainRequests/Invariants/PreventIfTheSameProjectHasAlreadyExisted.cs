using Domainify;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class PreventIfTheSameProjectHasAlreadyExisted
        : InvariantRequest<Project>
    {
        [BindTo(typeof(Project), nameof(Project.Id))]
        public string? ProjectId { get; private set; }

        [BindTo(typeof(Project), nameof(Project.Name))]
        public string Name { get; protected set; }
        public PreventIfTheSameProjectHasAlreadyExisted(string name, string? projectId = null)
        {
            Name = name;
            ProjectId = projectId;
        }
        public override IFault? GetFault()
        {
            return new AnEntityWithThesePropertiesHasAlreadyExistedFault(
                    typeof(Project).Name, Description);
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }

    public class PreventIfTheSameProjectHasAlreadyExistedHandler :
        IRequestHandler<PreventIfTheSameProjectHasAlreadyExisted, bool>
    {
        private readonly IProjectSettingRepository _repository;
        public PreventIfTheSameProjectHasAlreadyExistedHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            PreventIfTheSameProjectHasAlreadyExisted request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
