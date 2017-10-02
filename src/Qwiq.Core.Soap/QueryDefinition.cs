namespace Microsoft.Qwiq.Client.Soap
{
    public class QueryDefinition : Qwiq.QueryDefinition
    {
        public QueryDefinition(TeamFoundation.WorkItemTracking.Client.QueryDefinition queryDefinition)
                : base(queryDefinition.Id, queryDefinition.Name, queryDefinition.QueryText)
        {
        }
    }
}