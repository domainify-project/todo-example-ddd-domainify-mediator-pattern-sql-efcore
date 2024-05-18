using MediatR;
using Domainify.Domain;

namespace Domain.ProjectSettingAggregation
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
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
