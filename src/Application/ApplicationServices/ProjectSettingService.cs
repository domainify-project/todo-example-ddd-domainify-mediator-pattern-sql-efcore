using MediatR;
using Domainify.Domain;
using Contract;
using Domain.ProjectSettingAggregation;
using Persistence;

namespace Application
{
    public class ProjectSettingService : IProjectSettingService
    {
        private readonly IMediator _mediator;
        private readonly IDbTransaction _transaction;

        public ProjectSettingService(
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
        public async Task Process(CheckProjectForDeleting request)
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
            return (await _mediator.Send(request))!;
        }

        public async Task<PaginatedList<ProjectViewModel>> Process(GetProjectsList request)
        {
            return await _mediator.Send(request);
        }
        public async Task<string> Process(DefineSprint request)
        {
            var id = await _mediator.Send(request);
            await _transaction.SaveChangesAsync();
            return id;
        }
        public async Task Process(ChangeSprintName request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async Task Process(ChangeSprintTimeSpan request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async Task Process(DeleteSprint request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async Task Process(DeleteSprintPermanently request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync();
        }
        public async Task Process(CheckSprintForDeleting request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }

        public async Task Process(RestoreSprint request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async Task<SprintViewModel?> Process(GetSprint request)
        {
            return (await _mediator.Send(request))!;

        }

        public async Task<PaginatedList<SprintViewModel>> Process(GetSprintsList request)
        {
            return await _mediator.Send(request);
        }
    }
}
