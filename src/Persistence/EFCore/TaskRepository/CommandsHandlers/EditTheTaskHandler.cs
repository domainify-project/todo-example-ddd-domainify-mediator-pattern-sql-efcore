using XSwift.Datastore;
using Domain.TaskAggregation;
using EntityFrameworkCore.XSwift.Datastore;
using MediatR;

namespace Persistence.EFCore.TaskRepository
{
    public class EditTheTaskHandler :
        IRequestHandler<EditTheTask>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public EditTheTaskHandler(
            IMediator mediator, IDatabase database)
        {
            _mediator = mediator;
            _database = (Database)database;
        }

        public async Task<Unit> Handle(
            EditTheTask request,
            CancellationToken cancellationToken)
        {
            var entity = await request.ResolveAndGetEntityAsync(_mediator);
            await _database.UpdateAsync(request , entity);
            return new Unit();
        }
    }
}
