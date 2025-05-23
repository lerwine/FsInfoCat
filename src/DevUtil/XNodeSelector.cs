using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DevUtil
{
    public class XNodeSelector(IEnumerable<XNode> items) : IXNodeSelector<XNode>
    {
        private readonly IEnumerable<XNode> _nodes = (items is null) ? [] : items.Where(e => e is not null);

        public XNodeSelector(params XNode[] items) : this((IEnumerable<XNode>)items) { }

        public XElementSelector Ancestors() => new(_nodes.Ancestors());

        public XElementSelector Ancestors(string localName) => new(_nodes.Ancestors(XName.Get(localName)));

        public XElementSelector Ancestors(string localName, string namespaceURI) => new(_nodes.Ancestors((namespaceURI is null) ? XName.Get(localName) : XNamespace.Get(namespaceURI).GetName(localName)));

        public XElementSelector Ancestors(XName name) => new(_nodes.Ancestors(name));

        public bool Any() => _nodes.Any();

        public XNodeSelector Concat(params XNode[] other) => new((other is null) ? _nodes : _nodes.Concat(other));

        IXObjectSelector<XNode> IXObjectSelector<XNode>.Concat(params XNode[] other) => Concat(other);

        public XNodeSelector Concat(IEnumerable<XNode> other) => new((other is null) ? _nodes : _nodes.Concat(other));

        IXObjectSelector<XNode> IXObjectSelector<XNode>.Concat(IEnumerable<XNode> other) => Concat(other);

        public XNode FirstOrDefault() => _nodes.FirstOrDefault();

        public IEnumerable<XNode> GetItems() => _nodes;

        public IEnumerable<XNodeSelector> SingleFromEach() => _nodes.Select(e => new XNodeSelector(e));

        IEnumerable<IXNodeSelector<XNode>> IXNodeSelector<XNode>.SingleFromEach() => SingleFromEach();

        IEnumerable<IXObjectSelector<XNode>> IXObjectSelector<XNode>.SingleFromEach() => SingleFromEach();

        public XNode ItemAtOrDefault(int index) => _nodes.ElementAtOrDefault(index);

        public XNode LastOrDefault() => _nodes.LastOrDefault();

        public XNodeSelector InDocumentOrder() => new(_nodes.InDocumentOrder());

        IXNodeSelector<XNode> IXNodeSelector<XNode>.InDocumentOrder() => InDocumentOrder();

        public void Remove() => _nodes.Remove();

        public bool Contains(XNode other) => other is not null && _nodes.Any(n => ReferenceEquals(n, other));
    }
}
