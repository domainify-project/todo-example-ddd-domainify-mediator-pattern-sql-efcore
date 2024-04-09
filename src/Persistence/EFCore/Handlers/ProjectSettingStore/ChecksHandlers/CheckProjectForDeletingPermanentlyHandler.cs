using Domain.ProjectSettingAggregation;
using MediatR;

namespace Persistence.ProjectSettingStore
{
    public class CheckProjectForDeletingPermanentlyHandler :
        IRequestHandler<CheckProjectForDeletingPermanently>
    {
        private readonly IMediator _mediator;
        public CheckProjectForDeletingPermanentlyHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(
            CheckProjectForDeletingPermanently request,
            CancellationToken cancellationToken)
        {
            await request.ResolveAsync(_mediator);

            return new Unit();
        }
    }
}
