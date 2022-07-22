using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DevUtil
{
    public class XAttributeSelector : IXObjectSelector<XAttribute>
    {
        private readonly IEnumerable<XAttribute> _attributes;

        public XAttributeSelector(params XAttribute[] items) : this((IEnumerable<XAttribute>)items) { }

        public XAttributeSelector(IEnumerable<XAttribute> items) => _attributes = (items is null) ? Array.Empty<XAttribute>() : items.Where(e => e is not null);

        public bool Any() => _attributes.Any();

        public XAttributeSelector Concat(params XAttribute[] other) => new((other is null) ? _attributes : _attributes.Concat(other));

        IXObjectSelector<XAttribute> IXObjectSelector<XAttribute>.Concat(params XAttribute[] other) => Concat(other);

        public XAttributeSelector Concat(IEnumerable<XAttribute> other) => new((other is null) ? _attributes : _attributes.Concat(other));

        IXObjectSelector<XAttribute> IXObjectSelector<XAttribute>.Concat(IEnumerable<XAttribute> other) => Concat(other);

        public XAttribute FirstOrDefault() => _attributes.FirstOrDefault();

        public IEnumerable<XAttribute> GetItems() => _attributes;

        public XAttribute ItemAtOrDefault(int index) => _attributes.ElementAtOrDefault(index);

        public XAttribute LastOrDefault() => _attributes.LastOrDefault();

        public void Remove() => _attributes.Remove();
    }
}
