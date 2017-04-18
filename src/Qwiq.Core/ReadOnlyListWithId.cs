using System;
using System.Collections.Generic;

namespace Microsoft.Qwiq
{
    public abstract class ReadOnlyListWithId<T, TId> : ReadOnlyList<T>, IReadOnlyListWithId<T, TId>
        where T : IIdentifiable<TId>
    {
        private readonly Func<T, TId> _idFunc;
        private readonly IDictionary<TId, int> _mapById;

        protected ReadOnlyListWithId(IEnumerable<T> items, Func<T, string> nameFunc)
            :this(items, nameFunc, arg => arg.Id)
        {
        }

        protected ReadOnlyListWithId(IEnumerable<T> items, Func<T, string> nameFunc, Func<T, TId> idFunc)
            :base(items, nameFunc)
        {
            _idFunc = idFunc ?? throw new ArgumentNullException(nameof(idFunc));
            _mapById = new Dictionary<TId, int>();
        }

        public virtual bool Contains(TId id)
        {
            Ensure();
            return _mapById.ContainsKey(id);
        }

        public virtual bool TryGetById(TId id, out T value)
        {
            Ensure();
            int index;
            if (_mapById.TryGetValue(id, out index))
            {
                value = this[index];
                return true;
            }
            value = default(T);
            return false;
        }

        public virtual T GetById(TId id)
        {
            if (!TryGetById(id, out T byId)) throw new DeniedOrNotExistException();
            return byId;
        }

        protected override void Add(T value, int index)
        {
            base.Add(value, index);
            var id = _idFunc(value);

            AddById(id, index);
        }

        protected void AddById(TId id, int index)
        {
            try
            {
                _mapById.Add(id, index);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"An item with the ID {id} already exists.", e);
            }
        }

        public virtual bool Equals(IReadOnlyListWithId<T, TId> other)
        {
            return ReadOnlyListWithIdComparer<T, TId>.Default.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            return ReadOnlyListWithIdComparer<T, TId>.Default.Equals(this, obj as IReadOnlyListWithId<T, TId>);
        }

        public override int GetHashCode()
        {
            return ReadOnlyListWithIdComparer<T, TId>.Default.GetHashCode(this);
        }
    }
}