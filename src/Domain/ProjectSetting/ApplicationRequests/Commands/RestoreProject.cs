using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class RestoreProject :
        RequestToRestoreById<Project, string>
    {
        public RestoreProject(string id) 
            : base(id)
        {
            ValidationState.Validate();
        }
        public override async Task<Project> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var project = (await mediator.Send(
                new FindProject(Id, includeDeleted: true,
                preventIfNoEntityWasFound: true)))!;

            base.Prepare(project);

            InvariantState.AddAnInvariantRequest(new PreventIfTheSameProjectHasAlreadyExisted(
                name: project.Name, projectId: project.Id));
            await InvariantState.AssestAsync(mediator);
 
            await base.ResolveAsync(mediator, project);
            return project;
        }
    }

    public class RestoreProjectHandler :
    IRequestHandler<RestoreProject>
    {
        private readonly IProjectSettingRepository _repository;
        public RestoreProjectHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            RestoreProject request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
