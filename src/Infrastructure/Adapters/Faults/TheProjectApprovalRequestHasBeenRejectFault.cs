using Domainify.Domain;
using System.Globalization;

namespace Infrastructure.Adapters
{
    public class TheProjectApprovalRequestHasBeenRejectFault : InvariantFault
    {
        public TheProjectApprovalRequestHasBeenRejectFault() :
            base (outerDescription: "",
                innerDescription: string.Format(CultureInfo.CurrentCulture,
                "The project approval request has been reject fault."))
        {
        }
    }
}