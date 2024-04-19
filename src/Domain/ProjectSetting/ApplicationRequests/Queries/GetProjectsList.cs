using MediatR;
using Domainify.Domain;

namespace Domain.ProjectSetting
{
    public class GetProjectsList :
        QueryListRequest<Project, PaginatedList<ProjectViewModel>>
    {
        public bool? IsDeleted { get; set; }
        public string? SearchValue { get; set; } = string.Empty;
        public bool WithSprints { get; private set; } = false;
        public bool WithTasks { get; private set; } = false;
        public GetProjectsList(
            bool withSprints = false,
            bool withTasks = false)
        {
            WithSprints = withSprints;
            WithTasks = withTasks;
            ValidationState.Validate();
        }
        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }

    public class GetProjectsListHandler :
        IRequestHandler<GetProjectsList, PaginatedList<ProjectViewModel>>
    {
        private readonly IProjectSettingRepository _repository;
        public GetProjectsListHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<ProjectViewModel>> Handle(
            GetProjectsList request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
