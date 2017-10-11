using System;
using System.Linq;

using Microsoft.Qwiq.Exceptions;

using Tfs = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Microsoft.Qwiq.Client.Soap
{
    internal class Project : Qwiq.Project, IIdentifiable<int>
    {
        internal Project(Tfs.Project project)
            : base(
                project.Guid,
                project.Name,
                project.Uri,
                new Lazy<IWorkItemTypeCollection>(() => new WorkItemTypeCollection(project.WorkItemTypes)),
                new Lazy<INodeCollection<IAreaOrIteration, int>>(
                    () => new NodeCollection<IAreaOrIteration, int>(
                        project.AreaRootNodes.Cast<Tfs.Node>()
                               .Select(item => ExceptionHandlingDynamicProxyFactory.Create<INode<IAreaOrIteration, int>>(new Node(item))))),
                new Lazy<INodeCollection<IAreaOrIteration, int>>(
                    () => new NodeCollection<IAreaOrIteration, int>(
                        project.IterationRootNodes.Cast<Tfs.Node>()
                               .Select(item => ExceptionHandlingDynamicProxyFactory.Create<INode<IAreaOrIteration, int>>(new Node(item))))),
                new Lazy<IQueryFolderCollection>(
                    () =>
                    {
                        return new QueryFolderCollection(
                            () =>
                            {
                                return project
                                    .QueryHierarchy
                                    .OfType<Tfs.QueryFolder>()
                                    .Select(qf => ExceptionHandlingDynamicProxyFactory.Create<IQueryFolder>(new QueryFolder(qf)));
                            });
                    }))
        {
            Id = project.Id;
        }

        public new int Id { get; }
    }
}