using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
{
    public class GetProject :
        QueryItemRequestById<Project, string, Project?>
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
        public override async Task ResolveAsync(IMediator mediator, Project project)
        {
            base.Prepare(project);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, project);
        }
    }
}
