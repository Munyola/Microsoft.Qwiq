namespace Microsoft.Qwiq
{
    public interface INodeCollection<T, TU> : IReadOnlyObjectWithIdCollection<INode<T, TU>, TU> where T : IIdentifiable<TU>
    {
    }
}