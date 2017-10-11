using System;
using System.Collections.Generic;

namespace Microsoft.Qwiq
{
    public static class Comparer
    {
        public static IEqualityComparer<INode<IAreaOrIteration, int>> AreaOrIterationNode { get; } =
            NodeComparer<IAreaOrIteration, int>.Default;

        public static IEqualityComparer<IReadOnlyObjectWithIdCollection<INode<IAreaOrIteration, int>, int>>
            AreaOrIterationNodeCollection { get; } =
            ReadOnlyCollectionWithIdComparer<INode<IAreaOrIteration, int>, int>.Default;

        public static IEqualityComparer<IReadOnlyObjectWithIdCollection<IField, int>> FieldCollection { get; } =
            ReadOnlyCollectionWithIdComparer<IField, int>.Default;

        public static IEqualityComparer<IFieldDefinition> FieldDefinition { get; } = FieldDefinitionComparer.Default;

        public static IEqualityComparer<IFieldDefinitionCollection> FieldDefinitionCollection { get; } =
            FieldDefinitionCollectionComparer.Default;

        public static IEqualityComparer<IIdentifiable<int>> Identifiable { get; } = IdentifiableComparer.Default;

        public static IEqualityComparer<IIdentityDescriptor> IdentityDescriptor { get; } = IdentityDescriptorComparer.Default;

        public static IEqualityComparer<IIdentifiable<int?>> NullableIdentity { get; } = NullableIdentifiableComparer.Default;

        public static IEqualityComparer<string> OrdinalIgnoreCase { get; } = StringComparer.OrdinalIgnoreCase;

        public static IEqualityComparer<IProject> Project { get; } = ProjectComparer.Default;

        public static IEqualityComparer<ITeamFoundationIdentity> TeamFoundationIdentity { get; } = TeamFoundationIdentityComparer.Default;

        public static IEqualityComparer<IWorkItem> WorkItem { get; } = WorkItemComparer.Default;

        public static IEqualityComparer<IWorkItemCollection> WorkItemCollection { get; } = WorkItemCollectionComparer.Default;

        public static IEqualityComparer<IWorkItemLinkInfo> WorkItemLinkInfo { get; } = WorkItemLinkInfoComparer.Default;

        public static IEqualityComparer<IWorkItemLinkType> WorkItemLinkType { get; } = WorkItemLinkTypeComparer.Default;

        public static IEqualityComparer<IWorkItemLinkTypeEnd> WorkItemLinkTypeEnd { get; } = WorkItemLinkTypeEndComparer.Default;

        public static IEqualityComparer<IWorkItemType> WorkItemType { get; } = WorkItemTypeComparer.Default;

        public static IEqualityComparer<IWorkItemTypeCollection> WorkItemTypeCollection { get; } = WorkItemTypeCollectionComparer.Default;

        public static IEqualityComparer<IReadOnlyObjectWithIdCollection<IQueryFolder, Guid>> QueryFolderCollection { get; } = ReadOnlyCollectionWithIdComparer<IQueryFolder, Guid>.Default;
        public static IEqualityComparer<IQueryFolder> QueryFolder { get; } = QueryFolderComparer.Default;
        public static IEqualityComparer<IReadOnlyObjectWithIdCollection<IQueryDefinition, Guid>> QueryDefinitionCollection { get; } = ReadOnlyCollectionWithIdComparer<IQueryDefinition, Guid>.Default;
        public static IEqualityComparer<IQueryDefinition> QueryDefinition { get; } = QueryDefinitionComparer.Default;
    }
}