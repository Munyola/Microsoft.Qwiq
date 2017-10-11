using System;

namespace Microsoft.Qwiq
{
    public interface INode<T, TU> : IIdentifiable<TU>, IEquatable<INode<T, TU>> where T: IIdentifiable<TU>
    {
        INode<T, TU> Parent { get; }
        INodeCollection<T, TU> Children { get; }
        T Value { get; }
    }
}