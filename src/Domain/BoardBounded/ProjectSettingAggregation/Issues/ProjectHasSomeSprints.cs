using System.Globalization;
using Domain.Properties;
using Domainify.Domain;

namespace Domain.ProjectSettingAggregation
{
    internal class ProjectHasSomeSprints : InvariantFault
    {
        public ProjectHasSomeSprints(
            string description = "") :
            base(outerDescription: description,
                innerDescription: string.Format(CultureInfo.CurrentCulture,
                Resource.Invariant_Issue_TheProjectHasSomeSprints))
        {
        }
    }
}
