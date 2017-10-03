using System.Linq;
using Microsoft.Qwiq.Exceptions;

namespace Microsoft.Qwiq.Client.Soap
{
    internal class QueryFolder : Qwiq.QueryFolder
    {
        internal QueryFolder(TeamFoundation.WorkItemTracking.Client.QueryFolder queryFolder)
            : base(
                queryFolder.Id,
                queryFolder.Name,
                new QueryFolderCollection(() =>
                {
                    return queryFolder.OfType<TeamFoundation.WorkItemTracking.Client.QueryFolder>()
                        .Select(qf => ExceptionHandlingDynamicProxyFactory.Create<IQueryFolder>(new QueryFolder(qf)));
                }),
                new QueryDefinitionCollection(() =>
                {
                    return queryFolder.OfType<TeamFoundation.WorkItemTracking.Client.QueryDefinition>()
                        .Select(qd => ExceptionHandlingDynamicProxyFactory.Create<IQueryDefinition>(new QueryDefinition(qd)));
                }))
        {
        }
    }
}