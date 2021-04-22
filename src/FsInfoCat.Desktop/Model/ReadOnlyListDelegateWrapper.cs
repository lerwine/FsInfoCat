using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Desktop.Model
{
    public class ReadOnlyListDelegateWrapper<TItem, TElement> : IReadOnlyList<TElement>
        where TItem : TElement
    {
        private readonly Func<IList<TItem>> _getter;

        public TElement this[int index] => (_getter() ?? throw new IndexOutOfRangeException())[index];

        public int Count
        {
            get
            {
                IList<TItem> list = _getter();
                return (list is null) ? 0 : list.Count;
            }
        }

        public ReadOnlyListDelegateWrapper(Func<IList<TItem>> getter)
        {
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            IList<TItem> list = _getter();
            return ((list is null) ? new TElement[0] : list.Cast<TElement>()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IList<TItem> list = _getter();
            return ((list is null) ? new TElement[0] : (IEnumerable)list).GetEnumerator();
        }
    }
}
