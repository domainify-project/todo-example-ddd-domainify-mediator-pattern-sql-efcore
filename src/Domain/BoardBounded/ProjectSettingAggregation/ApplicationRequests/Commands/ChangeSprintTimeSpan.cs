using Domainify.Domain;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Domain.ProjectSettingAggregation
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

            var lastYear = DateTime.UtcNow.AddYears(-1);
            InvariantState.DefineAnInvariant(
                condition: () => { return StartDate < lastYear || EndDate < lastYear; },
                fault: new StartDateAndEndDateOfSprintCanNotBeEarlierThanLastTwelveMonths());

            await InvariantState.AssestAsync(mediator);

            sprint.SetStartDate(StartDate).SetEndDate(EndDate);
            await base.ResolveAsync(mediator, sprint);
            return sprint;
        }
    }
}
