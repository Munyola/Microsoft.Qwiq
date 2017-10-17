using System;

namespace Microsoft.Qwiq
{
    public interface IProject : IIdentifiable<Guid>
    {
        INodeCollection<IAreaOrIteration, int> AreaRootNodes { get; }

        Guid Guid { get; }

        INodeCollection<IAreaOrIteration, int> IterationRootNodes { get; }

        string Name { get; }

        Uri Uri { get; }

        IWorkItemTypeCollection WorkItemTypes { get; }

        INodeCollection<IQueryFolder, Guid> QueryFolders { get; }
    }
}