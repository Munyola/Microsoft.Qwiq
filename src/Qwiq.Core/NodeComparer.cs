using JetBrains.Annotations;

namespace Microsoft.Qwiq
{
    internal class NodeComparer<T, TU> : GenericComparer<INode<T, TU>> where T: IIdentifiable<TU>
    {
        internal new static readonly NodeComparer<T, TU> Default = Nested.Instance;

        private NodeComparer()
        {
        }

        public override bool Equals(INode<T, TU> x, INode<T, TU> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            return x.Id.Equals(y.Id)
                && GenericComparer<T>.Default.Equals(x.Value, y.Value);
        }

        public override int GetHashCode([CanBeNull] INode<T, TU> obj)
        {
            if (ReferenceEquals(obj, null)) return 0;

            unchecked
            {
                var hash = 27;

                hash = (hash * 13) ^ (obj.Id != null ? obj.Id.GetHashCode() : 0);
                hash = (hash * 13) ^ GenericComparer<T>.Default.GetHashCode(obj.Value);

                return hash;
            }
        }

        private class Nested
        {
            // ReSharper disable MemberHidesStaticFromOuterClass
            internal static readonly NodeComparer<T, TU> Instance = new NodeComparer<T, TU>();

            // ReSharper restore MemberHidesStaticFromOuterClass

            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            static Nested()
            {
            }
        }
    }
}