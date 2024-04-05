using Domainify.Domain;

namespace Domain.TaskAggregation
{
    public class GetTaskStatusList : BaseQueryRequest<List<KeyValuePair<int, string>>>
    {
    }
}
