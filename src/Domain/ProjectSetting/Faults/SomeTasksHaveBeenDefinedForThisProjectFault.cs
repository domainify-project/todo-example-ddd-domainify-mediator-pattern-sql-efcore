using Domainify.Domain;
using System.Globalization;
using Domain.Properties;

namespace Domain.ProjectSetting
{
    internal class SomeTasksHaveBeenDefinedForThisProjectFault : InvariantFault
    {
        public SomeTasksHaveBeenDefinedForThisProjectFault(
            string description = "")
            : base(outerDescription: description,
                innerDescription: string.Format(CultureInfo.CurrentCulture,
                Resource.Invariant_SomeTasksHaveBeenDefinedForThisProjectFault))
        {
        }
    }
}
