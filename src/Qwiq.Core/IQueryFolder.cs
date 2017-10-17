using System;
using JetBrains.Annotations;

namespace Microsoft.Qwiq
{
    public interface IQueryFolder : IIdentifiable<Guid>, IEquatable<IQueryFolder>
    {
        [NotNull]
        string Name { get; }
        [NotNull]
        string Path { get; }
        [NotNull]
        IQueryDefinitionCollection Queries { get; }
    }
}