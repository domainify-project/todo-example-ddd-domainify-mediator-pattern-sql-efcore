using Domainify.Domain;
using System.Globalization;
using Domain.Properties;

namespace Domain.ProjectSettingAggregation
{
    public class StartDateAndEndDateOfSprintCanNotBeEarlierThanLastTwelveMonths : InvariantFault
    {
        public StartDateAndEndDateOfSprintCanNotBeEarlierThanLastTwelveMonths(
            string description = "") : base(outerDescription: description,
                innerDescription: string.Format(CultureInfo.CurrentCulture,
                Resource.Invariant_Issue_TheStartDateAndEndDateOfTheSprintCanNotBeEarlierThanTheLastTwelveMonths))
        {
        }
    }
}
