using MediatR;
using Domain.TaskAggregation;
using EntityFrameworkCore.XSwift.Datastore;
using XSwift.Datastore;

namespace Persistence.EFCore.TaskRepository
{
    public class AddATaskHandler :
        IRequestHandler<AddATask, Guid>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public AddATaskHandler(
            IMediator mediator, IDatabase database)
        {
            _mediator = mediator;
            _database = (Database)database;
        }
        public async Task<Guid> Handle(
            AddATask request,
            CancellationToken cancellationToken)
        {
            var entity = await request.ResolveAndGetEntityAsync(_mediator);
            await _database.CreateAsync(request, entity);
            return entity.Id;
        }
    }
} 
