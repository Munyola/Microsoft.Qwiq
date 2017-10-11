using System;
using System.Linq;

namespace Microsoft.Qwiq.Mocks
{
    public class MockProject : Project
    {
        internal const string ProjectName = "Mock Project";

        public MockProject(Guid id, string name, Uri uri, IWorkItemTypeCollection wits, INodeCollection<IAreaOrIteration, int> areas, INodeCollection<IAreaOrIteration, int> iterations)
            : base(
                   id,
                   name,
                   uri,
                   new Lazy<IWorkItemTypeCollection>(() => wits),
                   new Lazy<INodeCollection<IAreaOrIteration, int>>(() => areas),
                   new Lazy<INodeCollection<IAreaOrIteration, int>>(() => iterations),
                   new Lazy<IQueryFolderCollection>(() => new QueryFolderCollection(Enumerable.Empty<IQueryFolder>)))
        {
        }

        public MockProject(IWorkItemStore store, INode<AreaOrIteration, int> node)
            : base(
                   Guid.NewGuid(),
                   node.Value.Name,
                   node.Value.Uri,
                   new Lazy<IWorkItemTypeCollection>(() => new MockWorkItemTypeCollection(store)),
                   new Lazy<INodeCollection<IAreaOrIteration, int>>(() => CreateNodes(true)),
                   new Lazy<INodeCollection<IAreaOrIteration, int>>(() => CreateNodes(false)),
                   new Lazy<IQueryFolderCollection>(() => new QueryFolderCollection(Enumerable.Empty<IQueryFolder>)))
        {
        }

        public MockProject(IWorkItemStore store)
            :this(store, new Node<AreaOrIteration, int>(new AreaOrIteration(1, false, false, ProjectName, new Uri("http://localhost/projects/1"))))
        {
        }

        private static INodeCollection<IAreaOrIteration, int> CreateNodes(bool area)
        {
            var root = 
                new Node<IAreaOrIteration, int>(
                    new AreaOrIteration(1, area, !area, "Root", new Uri("http://localhost/nodes/1")));
            new Node<IAreaOrIteration, int>(new AreaOrIteration(
                    2,
                    area,
                    !area,
                    "L1",
                    new Uri("http://localhost/nodes/2")),
                () => root,
                n => new[]
                {
                    new Node<IAreaOrIteration, int>(new AreaOrIteration(
                            3,
                            area,
                            !area,
                            "L2",
                            new Uri("http://localhost/nodes/3")),
                        () => n,
                        c => Enumerable.Empty<INode<IAreaOrIteration, int>>())
                });

            return new NodeCollection<IAreaOrIteration, int>(new[] {root}.ToList().AsReadOnly());
        }
    }
}