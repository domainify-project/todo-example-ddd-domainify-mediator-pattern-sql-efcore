using XSwift.Datastore;
using Domain.SprintAggregation;
using EntityFrameworkCore.XSwift.Datastore;
using MediatR;

namespace Persistence.EFCore.SprintRepository
{
    public class ChangeTheSprintTimeSpanHandler :
        IRequestHandler<ChangeTheSprintTimeSpan>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public ChangeTheSprintTimeSpanHandler(
            IMediator mediator, IDatabase database)
        {
            _mediator = mediator;
            _database = (Database)database;
        }

        public async Task<Unit> Handle(
            ChangeTheSprintTimeSpan request,
            CancellationToken cancellationToken)
        {
            var entity = await request.ResolveAndGetEntityAsync(_mediator);
            await _database.UpdateAsync(request , entity);
            return new Unit();
        }
    }
}
