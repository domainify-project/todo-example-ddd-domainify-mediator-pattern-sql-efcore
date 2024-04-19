using Domain.Properties;
using Domainify;
using Domainify.Domain;
using MediatR;

namespace Domain.Task
{
    public class GetTaskStatusList : BaseQueryRequest<List<KeyValuePair<int, string>>>
    {
    }
    public class GetTaskStatusListHandler :
    IRequestHandler<GetTaskStatusList, List<KeyValuePair<int, string>>>
    {
        public Task<List<KeyValuePair<int, string>>> Handle(
            GetTaskStatusList request,
            CancellationToken cancellationToken)
        {
            return System.Threading.Tasks.Task.FromResult(
                EnumHelper.ToKeyValuePairList<TaskStatus>(Resource.ResourceManager));
        }
    }
}
