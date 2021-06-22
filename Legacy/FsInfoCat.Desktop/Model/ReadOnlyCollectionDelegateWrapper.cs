using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Desktop.Model
{
    public class ReadOnlyCollectionDelegateWrapper<TItem, TElement> : IReadOnlyCollection<TElement>
        where TItem : TElement
    {
        private readonly Func<ICollection<TItem>> _getter;

        public int Count
        {
            get
            {
                ICollection<TItem> collection = _getter();
                return (collection is null) ? 0 : collection.Count;
            }
        }

        public ReadOnlyCollectionDelegateWrapper(Func<ICollection<TItem>> getter)
        {
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            ICollection<TItem> collection = _getter();
            return ((collection is null) ? new TElement[0] : collection.Cast<TElement>()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            ICollection<TItem> collection = _getter();
            return ((collection is null) ? new TElement[0] : (IEnumerable)collection).GetEnumerator();
        }
    }
}
