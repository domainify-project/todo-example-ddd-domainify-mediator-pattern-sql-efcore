using Domainify.Domain;
using MediatR;

namespace Domain.Task
{
    public class DeleteTaskPermanently :
        RequestToDeletePermanentlyById<Task, string>
    {
        public DeleteTaskPermanently(string id)
            : base(id)
        { 
            ValidationState.Validate();
        }

        public override async Task<Task> ResolveAndGetEntityAsync(IMediator mediator)
        {
            var task = (await mediator.Send(
                new FindTask(Id, includeDeleted: true, preventIfNoEntityWasFound: true)))!;

            base.Prepare(task);

            await InvariantState.AssestAsync(mediator);

            await base.ResolveAsync(mediator, task);

            return task;
        }
    }

    public class DeleteTaskPermanentlyHandler :
        IRequestHandler<DeleteTaskPermanently>
    {
        private readonly ITaskRepository _repository;
        public DeleteTaskPermanentlyHandler(ITaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            DeleteTaskPermanently request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
