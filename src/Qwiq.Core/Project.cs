using System;
using System.Globalization;

namespace Microsoft.Qwiq
{
    public class Project : IProject, IEquatable<IProject>
    {
        private readonly Lazy<INodeCollection<IAreaOrIteration, int>> _area;

        private readonly Lazy<INodeCollection<IAreaOrIteration, int>> _iteration;

        private readonly Lazy<IWorkItemTypeCollection> _wits;

        private readonly Lazy<INodeCollection<IQueryFolder, Guid>> _queryFolders;

        internal Project(
            Guid guid,
            string name,
            Uri uri,
            Lazy<IWorkItemTypeCollection> wits,
            Lazy<INodeCollection<IAreaOrIteration, int>> area,
            Lazy<INodeCollection<IAreaOrIteration, int>> iteration,
            Lazy<INodeCollection<IQueryFolder, Guid>> queryFolders)
        {
            Guid = guid;
            Name = name != null ? string.Intern(name) : throw new ArgumentNullException(nameof(name));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            _wits = wits ?? throw new ArgumentNullException(nameof(wits));
            _area = area ?? throw new ArgumentNullException(nameof(area));
            _iteration = iteration ?? throw new ArgumentNullException(nameof(iteration));
            _queryFolders = queryFolders ?? throw new ArgumentNullException(nameof(queryFolders));
        }

        private Project()
        {
        }

        public INodeCollection<IAreaOrIteration, int> AreaRootNodes => _area.Value;

        public Guid Guid { get; }

        public INodeCollection<IAreaOrIteration, int> IterationRootNodes => _iteration.Value;

        public string Name { get; }

        public Uri Uri { get; }

        public IWorkItemTypeCollection WorkItemTypes => _wits.Value;

        public INodeCollection<IQueryFolder, Guid> QueryFolders => _queryFolders.Value;

        public override bool Equals(object obj)
        {
            return ProjectComparer.Default.Equals(this, obj as IProject);
        }

        public bool Equals(IProject other)
        {
            return ProjectComparer.Default.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return ProjectComparer.Default.GetHashCode(this);
        }

        public override string ToString()
        {
            FormattableString s = $"{Guid.ToString()} ({Name})";
            return s.ToString(CultureInfo.InvariantCulture);
        }

        public Guid Id => Guid;
    }
}