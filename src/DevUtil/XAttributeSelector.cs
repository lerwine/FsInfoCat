using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DevUtil
{
    public class XAttributeSelector(IEnumerable<XAttribute> items) : IXObjectSelector<XAttribute>
    {
        private readonly IEnumerable<XAttribute> _attributes = (items is null) ? [] : items.Where(e => e is not null);

        public XAttributeSelector(params XAttribute[] items) : this((IEnumerable<XAttribute>)items) { }

        public bool Any() => _attributes.Any();

        public XAttributeSelector Concat(params XAttribute[] other) => new((other is null) ? _attributes : _attributes.Concat(other));

        IXObjectSelector<XAttribute> IXObjectSelector<XAttribute>.Concat(params XAttribute[] other) => Concat(other);

        public XAttributeSelector Concat(IEnumerable<XAttribute> other) => new((other is null) ? _attributes : _attributes.Concat(other));

        IXObjectSelector<XAttribute> IXObjectSelector<XAttribute>.Concat(IEnumerable<XAttribute> other) => Concat(other);

        public XAttribute FirstOrDefault() => _attributes.FirstOrDefault();

        public IEnumerable<XAttribute> GetItems() => _attributes;

        public IEnumerable<XAttributeSelector> SingleFromEach() => _attributes.Select(e => new XAttributeSelector(e));

        IEnumerable<IXObjectSelector<XAttribute>> IXObjectSelector<XAttribute>.SingleFromEach() => SingleFromEach();

        public XAttribute ItemAtOrDefault(int index) => _attributes.ElementAtOrDefault(index);

        public XAttribute LastOrDefault() => _attributes.LastOrDefault();

        public void Remove() => _attributes.Remove();

        public bool Contains(XAttribute other) => other is not null && _attributes.Any(a => ReferenceEquals(a, other));
    }
}
