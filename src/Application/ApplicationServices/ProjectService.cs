using MediatR;
using Domainify.Domain;
using Contract;
using Domain.ProjectSettingAggregation;

namespace Application
{
    public class ProjectService : IProjectService
    {
        private readonly IMediator _mediator;
        private readonly IDbTransaction _transaction;

        public ProjectService(
            IMediator mediator,
            IDbTransaction transaction)
        {
            _mediator = mediator;
            _transaction = transaction;
        }
        public async Task<string> Process(DefineProject request)
        {
            var id = await _mediator.Send(request);
            await _transaction.SaveChangesAsync();
            return id;
        }
        public async Task Process(ChangeProjectName request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async Task Process(DeleteProject request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async Task Process(CheckProjectForDeletingPermanently request)
        {
            await _mediator.Send(request);
        }
        public async Task Process(RestoreProject request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async Task Process(DeleteProjectPermanently request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync();
        }
        public async Task<ProjectViewModel?> Process(GetProject request)
        {
            return (await _mediator.Send(request))!.ToViewModel();
        }

        public async Task<PaginatedList<ProjectViewModel>> Process(GetProjectsList request)
        {
            return await _mediator.Send(request);
        }
    }
}
