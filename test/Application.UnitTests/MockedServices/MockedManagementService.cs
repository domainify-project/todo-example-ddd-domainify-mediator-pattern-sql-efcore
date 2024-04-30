using Contract.InfrastructureServices;
using System.Threading.Tasks;


namespace Application.UnitTests.MockedServices
{
    public class MockedManagementService : IManagementService
    {
        public Task<bool> Process(RequestProjectToBeApproved request, bool throwOnFault = true)
        {
            return Task.FromResult(true);
        }
    }
}
