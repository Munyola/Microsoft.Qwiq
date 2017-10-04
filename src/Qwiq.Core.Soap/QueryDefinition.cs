using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace Microsoft.Qwiq.Client.Soap
{
    internal class QueryDefinition : Qwiq.QueryDefinition
    {
        internal QueryDefinition([NotNull] TeamFoundation.WorkItemTracking.Client.QueryDefinition queryDefinition)
                : base(queryDefinition.Id, queryDefinition.Name, queryDefinition.QueryText)
        {
        }
    }
}