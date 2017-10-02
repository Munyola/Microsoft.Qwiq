using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace Microsoft.Qwiq.Client.Rest
{
    internal class QueryDefinition : Qwiq.QueryDefinition
    {
        internal QueryDefinition(QueryHierarchyItem queryDefinition)
            : base(queryDefinition.Id, queryDefinition.Name, queryDefinition.Wiql)
        {
        }
    }
}