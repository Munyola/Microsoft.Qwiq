using System;
using System.Collections.Generic;

namespace Microsoft.Qwiq
{
    public class NodeCollection<T, TU> : ReadOnlyObjectWithIdCollection<INode<T, TU>, TU>, INodeCollection<T, TU> where T : IIdentifiable<TU>
    {
        public NodeCollection(IEnumerable<INode<T, TU>> items) : base(items)
        {
        }

        public NodeCollection(Func<IEnumerable<INode<T, TU>>> itemFactory) : base(itemFactory, i => i.Value.)
        {
        }
    }
}