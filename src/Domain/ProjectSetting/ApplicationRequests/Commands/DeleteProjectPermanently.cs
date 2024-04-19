using Domain.Task;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSetting
{
    public class DeleteProjectPermanently :
        RequestToDeletePermanentlyById<Project, string>
    {
        public DeleteProjectPermanently(string id)
            : base(id)
        { 
            ValidationState.Validate();
        }

        public override async Task<Project> ResolveAndGetEntityAsync(IMediator mediator)
        {
            var project = (await mediator.Send(
                new FindProject(Id, 
                includeDeleted: true, 
                preventIfNoEntityWasFound: true)))!;

            base.Prepare(project);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, project);

            return project;
        }
    }

    public class DeleteProjectPermanentlyHandler :
    IRequestHandler<DeleteProjectPermanently>
    {
        private readonly IProjectSettingRepository _repository;
        public DeleteProjectPermanentlyHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            DeleteProjectPermanently request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
