using Domainify.Domain;
using System.Globalization;
using Domain.Properties;

namespace Domain.ProjectSettingAggregation
{
    internal class SomeTasksHaveBeenDefinedForThisProject : InvariantFault
    {
        public SomeTasksHaveBeenDefinedForThisProject(
            string description = "")
            : base(outerDescription: description,
                innerDescription: string.Format(CultureInfo.CurrentCulture,
                Resource.Invariant_Issue_SomeTasksHaveBeenDefinedForThisProject))
        {
        }
    }
}
