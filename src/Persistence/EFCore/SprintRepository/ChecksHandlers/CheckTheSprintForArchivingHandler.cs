using MediatR;
using XSwift.Datastore;
using EntityFrameworkCore.XSwift.Datastore;
using Domain.SprintAggregation;

namespace Persistence.EFCore.SprintAggregation
{
    public class CheckTheSprintForArchivingHandler :
        IRequestHandler<CheckTheSprintForArchiving>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public CheckTheSprintForArchivingHandler(
           IMediator mediator,
           IDatabase database)
        {
            _mediator = mediator;
            _database = (Database)database;
        }

        public async Task<Unit> Handle(
            CheckTheSprintForArchiving request,
            CancellationToken cancellationToken)
        {
            await _database.CheckInvariantsAsync<
                 CheckTheSprintForArchiving, Sprint>(request);
            await request.ResolveAsync(_mediator);
            return new Unit();
        }
    }
}
