using Domainify.Domain;

namespace Contract.InfrastructureServices
{
    public class RequestProjectToBeApproved : BaseRequest
    {
        public string ProjectId { get; private set; }

        public RequestProjectToBeApproved(string projectId) {
            ProjectId = projectId;
        }
    }
}
