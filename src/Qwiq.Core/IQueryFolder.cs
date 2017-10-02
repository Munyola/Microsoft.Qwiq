using System;

namespace Microsoft.Qwiq
{
    public interface IQueryFolder : IIdentifiable<Guid>, IEquatable<IQueryFolder>
    {
        string Name { get; }
        IQueryFolderCollection SubFolders { get; }
        IQueryDefinitionCollection SavedQueries { get; }
    }
}