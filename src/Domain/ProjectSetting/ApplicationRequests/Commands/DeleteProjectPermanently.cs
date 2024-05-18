using Domain.TaskAggregation;
using Domainify.Domain;
using MediatR;

namespace Domain.ProjectSettingAggregation
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
}
