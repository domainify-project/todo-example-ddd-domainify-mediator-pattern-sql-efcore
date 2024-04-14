using Domainify.Domain;
using System.Globalization;
using Domain.Properties;

namespace Domain.ProjectSettingAggregation
{
    public class StartDateAndEndDateOfSprintCanNotBeEarlierThanLastTwelveMonthsFault : InvariantFault
    {
        public StartDateAndEndDateOfSprintCanNotBeEarlierThanLastTwelveMonthsFault(
            string description = "") : base(outerDescription: description,
                innerDescription: string.Format(CultureInfo.CurrentCulture,
                Resource.Invariant_TheStartDateAndEndDateOfTheSprintCanNotBeEarlierThanTheLastTwelveMonthsFault))
        {
        }
    }
}
