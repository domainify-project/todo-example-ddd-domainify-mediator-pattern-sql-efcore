using MediatR;
using Domain.SprintAggregation;
using EntityFrameworkCore.XSwift.Datastore;
using XSwift.Datastore;

namespace Persistence.EFCore.SprintRepository
{
    public class ArchiveTheSprintHandler :
        IRequestHandler<ArchiveTheSprint>
    {
        private readonly IMediator _mediator;
        private readonly Database _database;
        public ArchiveTheSprintHandler(
           IMediator mediator, IDatabase database)
        {
            _mediator = mediator;
            _database = (Database)database;

            _database.ResolveSoftDeleteConfiguration(
                new ModuleDeletabilityConfigurationFactory()
                .CreateInstance(database));
        }

        public async Task<Unit> Handle(
            ArchiveTheSprint request,
            CancellationToken cancellationToken)
        {
            var entity = await request.ResolveAndGetEntityAsync(_mediator);
            await _database.ArchiveAsync(request, entity);
            return new Unit();
        }
    }
}
