using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class DefineProject
        : RequestToCreate<Project, string>
    {
        [BindTo(typeof(Project), nameof(Project.Name))]
        public string Name { get; private set; }

        public DefineProject(string name)
        {
            Name = name.Trim();
            ValidationState.Validate();
        }

        public override async Task<Project> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var project = Project.NewInstance()
                .SetName(Name);

            base.Prepare(project);

            InvariantState.AddAnInvariantRequest(new PreventIfTheSameProjectHasAlreadyExisted(name: project.Name));
            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, project);
            return project;
        }
    }

    public class DefineProjectHandler :
    IRequestHandler<DefineProject, string>
    {
        private readonly IProjectSettingRepository _repository;
        public DefineProjectHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> Handle(
            DefineProject request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
