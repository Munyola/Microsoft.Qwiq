using System;

namespace Microsoft.Qwiq.Client.Soap
{
    internal class AreaOrIteration : Qwiq.AreaOrIteration
    {
        internal AreaOrIteration(
            int id,
            bool isAreaNode,
            bool isIterationNode,
            string name,
            Uri uri,
            string path)
            : base(id, isAreaNode, isIterationNode, name, uri)
        {
            Path = path;
        }

        public override string Path { get; }
    }
}