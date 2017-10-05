using System;
using JetBrains.Annotations;

namespace Microsoft.Qwiq.Mocks
{
    public class MockQueryDefinition: QueryDefinition
    {
        public MockQueryDefinition(Guid id, [NotNull] string name, [NotNull] string wiql, [NotNull] string path)
            : base(id, name, wiql, path)
        {
        }
    }
}
