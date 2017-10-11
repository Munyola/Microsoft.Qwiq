using System.Linq;
using JetBrains.Annotations;
using Tfs = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Microsoft.Qwiq.Client.Soap
{
    internal class Node : Node<IAreaOrIteration, int>
    {
        internal Node([NotNull] Tfs.Node node, [CanBeNull] INode<IAreaOrIteration, int> parentNode = null)
            : base(
                new AreaOrIteration(
                    node.Id,
                    node.IsAreaNode,
                    node.IsIterationNode,
                    node.Name,
                    node.Uri,
                    node.Path),
                () => parentNode,
                parent => node.ChildNodes.Cast<Tfs.Node>().Select(item => new Node(item, parent)).ToList())
        {
        }
    }
}