using MediatR;
using Domainify.Domain;

namespace Domain.ProjectSettingAggregation
{
    public class GetProjectsList :
        QueryListRequest<Project, PaginatedList<ProjectViewModel>>
    {
        public bool? IsDeleted { get; set; }
        public string? SearchValue { get; set; } = string.Empty;

        public GetProjectsList()
        {
            ValidationState.Validate();
        }
        public override async Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }
}
