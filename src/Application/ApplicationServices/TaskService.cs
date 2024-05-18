using MediatR;
using Domainify.Domain;
using Contract;
using Persistence;
using Domain.TaskAggregation;

namespace Application
{
    public class TaskService : ITaskService
    {
        private readonly IMediator _mediator;
        private readonly IDbTransaction _transaction;

        public TaskService(
            IMediator mediator,
            IDbTransaction transaction)
        {
            _mediator = mediator;
            _transaction = transaction;
        }
        public async Task<string> Process(AddTask request)
        {
            var id = await _mediator.Send(request);
            await _transaction.SaveChangesAsync();
            return id;
        }
        public async System.Threading.Tasks.Task Process(EditTask request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async System.Threading.Tasks.Task Process(DeleteTask request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async System.Threading.Tasks.Task Process(ChangeTaskStatus request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async System.Threading.Tasks.Task Process(RestoreTask request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }
        public async Task<TaskViewModel?> Process(GetTask request)
        {
            return (await _mediator.Send(request))!;
        }
        public async System.Threading.Tasks.Task Process(DeleteTaskPermanently request)
        {
            await _mediator.Send(request);
            await _transaction.SaveChangesAsync(concurrencyCheck: true);
        }

        public async Task<PaginatedList<TaskViewModel>> Process(GetTasksList request)
        {
            return await _mediator.Send(request);
        }
        public async Task<List<KeyValuePair<int, string>>> Process(GetTaskStatusList request)
        {
            return await _mediator.Send(request);
        }
    }
}
