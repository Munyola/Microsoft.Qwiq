using System;

using JetBrains.Annotations;

namespace Microsoft.Qwiq
{
    public interface IQueryDefinition : IIdentifiable<Guid>, IEquatable<IQueryDefinition>
    {
        [NotNull]
        string Name { get; }
        [NotNull]
        string Wiql { get; }
    }
}