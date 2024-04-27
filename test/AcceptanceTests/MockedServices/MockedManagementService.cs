using Contract.InfrastructureServices;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public class MockedManagementService : IManagementService
    {
        public Task<bool> Process(RequestProjectToBeApproved request, bool throwOnFault = true)
        {
            return Task.FromResult(true);
        }
    }
}
