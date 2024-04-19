using Domainify.Domain;
using MediatR;

namespace Domain.Task
{
    public class DeleteTask :
        RequestToDeleteById<Task, string>
    {
        public DeleteTask(string id)
            : base(id)
        {
            ValidationState.Validate();
        }
        public override async Task<Task> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var task = (await mediator.Send(
                new FindTask(Id, includeDeleted: true)))!;

            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, task);

            return task;
        }
    }

    public class DeleteTaskHandler :
        IRequestHandler<DeleteTask>
    {
        private readonly ITaskRepository _repository;
        public DeleteTaskHandler(ITaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            DeleteTask request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}