using System;

namespace Microsoft.Qwiq
{
    public interface IAreaOrIteration : IEquatable<IAreaOrIteration>, IIdentifiable<int>
    {
        bool IsAreaNode { get; }
        bool IsIterationNode { get; }
        string Name { get; }
        Uri Uri { get; }
        string Path { get; }
    }
}