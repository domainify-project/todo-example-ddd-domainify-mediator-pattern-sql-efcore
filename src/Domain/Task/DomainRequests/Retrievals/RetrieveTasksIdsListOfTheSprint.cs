using MediatR;
using Domainify.Domain;

namespace Domain.Task
{
    public class RetrieveTasksIdsListOfTheSprint :
        QueryListRequest<Task, List<string>> 
    {
        public string SprintId { get; private set; } = string.Empty;

        public RetrieveTasksIdsListOfTheSprint()
        {
            ValidationState.Validate();
        }

        public RetrieveTasksIdsListOfTheSprint SetSprintId(string value)
        {
            SprintId = value;
            return this;
        }

        public override async System.Threading.Tasks.Task ResolveAsync(IMediator mediator)
        {
            await InvariantState.AssestAsync(mediator);
        }
    }

    public class RetrieveTasksIdsListOfTheSprintHandler :
        IRequestHandler<RetrieveTasksIdsListOfTheSprint, List<string>>
    {
        private readonly ITaskRepository _repository;
        public RetrieveTasksIdsListOfTheSprintHandler(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<string>> Handle(
            RetrieveTasksIdsListOfTheSprint request,
            CancellationToken cancellationToken)
        {
            return await _repository.Apply(request);
        }
    }
}
