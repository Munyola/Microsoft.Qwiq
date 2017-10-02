using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JetBrains.Annotations;

using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;

namespace Microsoft.Qwiq.Client.Rest
{
    internal class Project : Qwiq.Project
    {
        internal Project([NotNull] TeamProjectReference project, [NotNull] WorkItemStore store)
            : base(
                project.Id,
                project.Name,
                new Uri(project.Url),
                new Lazy<IWorkItemTypeCollection>(
                    () =>
                        {
                            var wits = store.NativeWorkItemStore
                                            .Value
                                            .GetWorkItemTypesAsync(project.Name)
                                            .GetAwaiter()
                                            .GetResult();

                            var wits2 = new List<IWorkItemType>(wits.Count);
                            for (var i = 0; i < wits.Count; i++)
                            {
                                var wit = wits[i];
                                wits2.Add(new WorkItemType(wit));
                            }

                            return new WorkItemTypeCollection(wits2);
                        }),
                new Lazy<INodeCollection>(
                    () =>
                        {
                            var result = store.NativeWorkItemStore
                                              .Value
                                              .GetClassificationNodeAsync(
                                                  project.Id,
                                                  TreeStructureGroup.Areas,
                                                  null,
                                                  int.MaxValue)
                                              .GetAwaiter()
                                              .GetResult();

                            // SOAP Client does not return just the root, so return the root's children to match implementation
                            var n = new Node(result).ChildNodes;
                            return n;
                        }),
                new Lazy<INodeCollection>(
                    () =>
                        {
                            var result = store.NativeWorkItemStore
                                              .Value
                                              .GetClassificationNodeAsync(
                                                  project.Name,
                                                  TreeStructureGroup.Iterations,
                                                  null,
                                                  int.MaxValue)
                                              .GetAwaiter()
                                              .GetResult();

                            return new Node(result).ChildNodes;
                        }),
                new Lazy<IQueryFolderCollection>(() =>
                {
                    return new QueryFolderCollection(() =>
                    {
                        //BUGBUG: There is a bug in the GetQueryAsync in vsts where if a folder contains a '+' character it will return 404, even if the folder exists see here: https://developercommunity.visualstudio.com/content/problem/123660/when-a-saved-query-folder-contains-a-character-it.html
                        QueryHierarchyItem FolderExpansionFunc(string path)
                        {
                            try
                            {
                                return store.NativeWorkItemStore.Value.GetQueryAsync(project.Id, path, QueryExpand.Wiql, 2).Result;
                            }
                            catch(VssServiceResponseException ex){
                                throw new InvalidOperationException($"An error occured while trying to expand the saved query folder {path}. See inner exception for details.", ex);
                            }
                        }

                        var initialFolders = store.NativeWorkItemStore.Value.GetQueriesAsync(project.Id, QueryExpand.Wiql, 2);
                        return initialFolders.Result.Select(qf => new QueryFolder(qf, FolderExpansionFunc));
                    });
                }))
        {
        }
    }
}