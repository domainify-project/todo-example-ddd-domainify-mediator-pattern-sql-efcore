using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class GetProject :
        QueryItemRequestById<Project, string, ProjectViewModel?>
    {
        public bool WithSprints { get; private set; } = false;
        public bool WithTasks { get; private set; } = false;
        public GetProject(string id,
            bool withSprints = false,
            bool withTasks = false,
            bool includeDeleted = false) : base(id)
        {
            WithSprints = withSprints;
            WithTasks = withTasks;
            PreventIfNoEntityWasFound = true;
            IncludeDeleted = includeDeleted;
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator, Project project)
        {
            base.Prepare(project);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, project);
        }
    }

    public class GetProjectHandler :
    IRequestHandler<GetProject, ProjectViewModel?>
    {
        private readonly IProjectSettingRepository _repository;
        public GetProjectHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectViewModel?> Handle(
            GetProject request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
