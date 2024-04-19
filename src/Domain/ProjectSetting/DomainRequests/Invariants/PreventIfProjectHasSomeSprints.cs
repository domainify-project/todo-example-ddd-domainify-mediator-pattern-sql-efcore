using MediatR;
using Domainify;
using Domainify.Domain;

namespace Domain.ProjectSetting
{
    public class PreventIfProjectHasSomeSprints
        : InvariantRequestById<Project, string>
    {
        public PreventIfProjectHasSomeSprints(string id) : base(id)
        {
        }

        public override IFault? GetFault()
        {
            return new TheProjectHasSomeSprintsFault();
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }

    public class PreventIfProjectHasSomeSprintsHandler :
        IRequestHandler<PreventIfProjectHasSomeSprints, bool>
    {
        private readonly IProjectSettingRepository _repository;
        public PreventIfProjectHasSomeSprintsHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            PreventIfProjectHasSomeSprints request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
