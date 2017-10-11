using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Microsoft.Qwiq
{
    public class Node<T, TU> : INode<T, TU> where T : IIdentifiable<TU>
    {
        private readonly Lazy<INode<T, TU>> _parent;
        private readonly Lazy<INodeCollection<T, TU>> _children;

        internal Node(
            [NotNull] T value,
            [NotNull] Func<INode<T, TU>> parentFactory,
            [NotNull] Func<INode<T, TU>, IEnumerable<INode<T, TU>>> childrenFactory)
        {
            if (parentFactory == null)
            {
                throw new ArgumentNullException(nameof(parentFactory));
            }

            if (childrenFactory == null)
            {
                throw new ArgumentNullException(nameof(childrenFactory));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            _parent = new Lazy<INode<T, TU>>(parentFactory);
            _children = new Lazy<INodeCollection<T, TU>>(() => new NodeCollection<T, TU>(childrenFactory(this)));
        }

        internal Node(T value)
            : this(value, () => null, n => Enumerable.Empty<INode<T, TU>>())
        {
        }

        public TU Id => Value.Id;
        public T Value { get; }
        public INode<T, TU> Parent => _parent.Value;
        public INodeCollection<T, TU> Children => _children.Value;

        [DebuggerStepThrough]
        public bool Equals(INode<T, TU> other)
        {
            return NodeComparer<T, TU>.Default.Equals(this, other);
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return NodeComparer<T, TU>.Default.Equals(this, obj as INode<T, TU>);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return NodeComparer<T, TU>.Default.GetHashCode(this);
        }
    }
}