using System;

namespace Microsoft.Qwiq
{
    public interface IQueryDefinitionCollection : IReadOnlyObjectWithIdCollection<IQueryDefinition, Guid>, IEquatable<IQueryDefinitionCollection>
    {
    }
}