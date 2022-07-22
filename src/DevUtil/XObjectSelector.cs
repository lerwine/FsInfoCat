using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DevUtil
{
    public class XObjectSelector : IXObjectSelector<XObject>
    {
        private readonly IEnumerable<XObject> _items;

        public XObjectSelector(params XObject[] items) : this((IEnumerable<XObject>)items) { }

        public XObjectSelector(IEnumerable<XObject> items) => _items = (items is null) ? Array.Empty<XObject>() : items.Where(e => e is not null);

        public bool Any() => _items.Any();

        public XObjectSelector Concat(params XNode[] other) => new((other is null) ? _items : _items.Concat(other));

        IXObjectSelector<XObject> IXObjectSelector<XObject>.Concat(params XObject[] other) => Concat(other);

        public XObjectSelector Concat(IEnumerable<XObject> other) => new((other is null) ? _items : _items.Concat(other));

        IXObjectSelector<XObject> IXObjectSelector<XObject>.Concat(IEnumerable<XObject> other) => Concat(other);

        public XObject FirstOrDefault() => _items.FirstOrDefault();

        public IEnumerable<XObject> GetItems() => _items;

        public XObject ItemAtOrDefault(int index) => _items.ElementAtOrDefault(index);

        public XObject LastOrDefault() => _items.LastOrDefault();
    }
}
