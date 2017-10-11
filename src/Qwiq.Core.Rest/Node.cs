using System;
using System.Linq;

using JetBrains.Annotations;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace Microsoft.Qwiq.Client.Rest
{
    internal class Node : Node<IAreaOrIteration, int>
    {
        internal Node([NotNull] WorkItemClassificationNode node)
            : this(node, null)
        {
        }

        internal Node([NotNull] WorkItemClassificationNode node, [CanBeNull] INode<IAreaOrIteration, int> parentNode)
            : base(
                  new AreaOrIteration(
                    node.Id,
                    node.StructureType == TreeNodeStructureType.Area,
                    node.StructureType == TreeNodeStructureType.Iteration,
                    node.Name,
                    new Uri(node.Url),
                    new Lazy<INode<IAreaOrIteration, int>>(() => parentNode)),
                () => parentNode,
                n => node.Children?.Any() ?? false
                    ? node.Children.Select(s => new Node(s, n)).ToList()
                    : Enumerable.Empty<INode<IAreaOrIteration, int>>())
        {
        }
    }
}