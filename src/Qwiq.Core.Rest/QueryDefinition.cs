using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace Microsoft.Qwiq.Client.Rest
{
    public class QueryDefinition : Qwiq.QueryDefinition
    {
        public QueryDefinition(QueryHierarchyItem queryDefinition)
            : base(queryDefinition.Id, queryDefinition.Name, queryDefinition.Wiql)
        {
        }
    }
}