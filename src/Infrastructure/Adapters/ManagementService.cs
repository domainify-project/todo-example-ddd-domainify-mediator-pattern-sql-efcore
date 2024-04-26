using Contract.InfrastructureServices;

namespace Infrastructure.Adapters
{
    public class ManagementService : IManagementService
    {
        public async Task<bool> Process(RequestProjectToBeApproved request, bool throwOnFault = true)
        {
            // Here just a foo code has been written.

            var approved = false;

            if (request.ProjectId != null)
            {
                approved = true;
            }
   
            if(throwOnFault)
            {
                request.ValidationState.DefineAValidation(
                    condition: approved!,
                    new TheProjectApprovalRequestHasBeenRejectFault());

                request.ValidationState.Validate();
            }

            return await Task.FromResult(approved);
        }
    }
}
