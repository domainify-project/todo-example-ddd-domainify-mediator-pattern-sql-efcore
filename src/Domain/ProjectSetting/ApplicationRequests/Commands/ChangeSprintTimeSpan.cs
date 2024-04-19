using Domainify.Domain;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Domain.ProjectSetting
{
    public class ChangeSprintTimeSpan :
        RequestToUpdateById<Sprint, string>
    {
        [BindTo(typeof(Sprint), nameof(Sprint.StartDate))]
        [Required]
        public DateTime StartDate { get; private set; }

        [BindTo(typeof(Sprint), nameof(Sprint.EndDate))]
        [Required]
        public DateTime EndDate { get; private set; }

        public ChangeSprintTimeSpan(string id, DateTime startDate, DateTime endDate) 
            : base(id)
        {
            StartDate = startDate;
            EndDate = endDate;

            ValidationState.AddAValidation(
                new PreventIfStartDateIsLaterThanEndDate<Sprint>
                (StartDate, EndDate));

            ValidationState.Validate();
        }

        public override async Task<Sprint> ResolveAndGetEntityAsync(
            IMediator mediator)
        {
            var sprint = (await mediator.Send(new FindSprint(Id, preventIfNoEntityWasFound: true)))!;

            base.Prepare(sprint);

            var lastYear = DateTime.UtcNow.AddYears(-1);
            InvariantState.DefineAnInvariant(
                condition: () => { return StartDate < lastYear || EndDate < lastYear; },
                fault: new StartDateAndEndDateOfSprintCanNotBeEarlierThanLastTwelveMonthsFault());

            await InvariantState.AssestAsync(mediator);

            sprint.SetStartDate(StartDate).SetEndDate(EndDate);
            await base.ResolveAsync(mediator, sprint);
            return sprint;
        }
    }

    public class ChangeSprintTimeSpanHandler :
        IRequestHandler<ChangeSprintTimeSpan>
    {
        private readonly IProjectSettingRepository _repository;
        public ChangeSprintTimeSpanHandler(IProjectSettingRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(
            ChangeSprintTimeSpan request,
            CancellationToken cancellationToken)
        {
            await _repository.Apply(request);
            return new Unit();
        }
    }
}
