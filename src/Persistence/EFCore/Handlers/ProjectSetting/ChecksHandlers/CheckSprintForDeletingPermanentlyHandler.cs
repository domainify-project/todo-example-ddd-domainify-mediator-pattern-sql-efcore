using Domain.ProjectSettingAggregation;
using MediatR;

namespace Persistence.ProjectStore
{
    public class CheckSprintForDeletingPermanentlyHandler :
        IRequestHandler<CheckSprintForDeletingPermanently>
    {
        private readonly IMediator _mediator;
        public CheckSprintForDeletingPermanentlyHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(
            CheckSprintForDeletingPermanently request,
            CancellationToken cancellationToken)
        {
            await request.ResolveAsync(_mediator);

            return new Unit();
        }
    }
}
