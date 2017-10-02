using System;

namespace Microsoft.Qwiq
{
    public interface IQueryFolderCollection : IReadOnlyObjectWithIdCollection<IQueryFolder, Guid>, IEquatable<IQueryFolderCollection>
    {
    }
}