namespace Contract.InfrastructureServices
{
    public interface IManagementService
    {
        public Task<bool> Process(RequestProjectToBeApproved request, bool throwOnFault = true);
    }
}
