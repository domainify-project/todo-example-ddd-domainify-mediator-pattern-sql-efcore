using MediatR;
using Domainify.Domain;
using Contract;

namespace Application
{
    public class SprintService : ISprintService
    {
        private readonly IMediator _mediator;
        private readonly IDbTransaction _transaction;

        public SprintService(
            IMediator mediator,
            IDbTransaction transaction)
        {
            _mediator = mediator;
            _transaction = transaction;
        }
        public async Task<Guid> Process(DefineNewSprint request)
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
        public async Task Process(ArchiveSprint request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async Task Process(CheckSprintForArchiving request)
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
            return await _mediator.Send(request);
        }

        public async Task<PaginatedViewModel<SprintViewModel>> Process(GetSprintList request)
        {
            return await _mediator.Send(request);
        }
    }
}
