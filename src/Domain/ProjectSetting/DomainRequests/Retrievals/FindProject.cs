using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class FindProject :
        QueryItemRequestById<Project, string, Project?>
    {
        public bool WithSprints { get; private set; } = false;
        public bool WithTasks { get; private set; } = false;

        public FindProject(string id,
            bool withSprints = false,
            bool withTasks = false,
            bool includeDeleted = false,
            bool preventIfNoEntityWasFound = false) : base(id)
        {
            WithSprints = withSprints;
            WithTasks = withTasks;
            IncludeDeleted = includeDeleted;
            PreventIfNoEntityWasFound = preventIfNoEntityWasFound;
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator, Project project)
        {
            base.Prepare(project);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, project);
        }
    }

    public class FindProjectHandler :
        IRequestHandler<FindProject, Project?>
    {
        private readonly IProjectSettingRepository _repository;
        public FindProjectHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Project?> Handle(
            FindProject request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
