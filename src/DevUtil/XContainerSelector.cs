using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DevUtil
{
    public class XContainerSelector : IXContainerSelector<XContainer>
    {
        private readonly IEnumerable<XContainer> _nodes;

        public XContainerSelector(params XContainer[] items) : this((IEnumerable<XContainer>)items) { }

        public XContainerSelector(IEnumerable<XContainer> items) => _nodes = (items is null) ? Array.Empty<XContainer>() : items.Where(e => e is not null);

        public XElementSelector Ancestors() => new(_nodes.Ancestors());

        public XElementSelector Ancestors(string localName) => new(_nodes.Ancestors(XName.Get(localName)));

        public XElementSelector Ancestors(string localName, string namespaceURI) => new(_nodes.Ancestors((namespaceURI is null) ? XName.Get(localName) : XNamespace.Get(namespaceURI).GetName(localName)));

        public XElementSelector Ancestors(XName name) => new(_nodes.Ancestors(name));

        public bool Any() => _nodes.Any();

        public XContainerSelector Concat(params XContainer[] other) => new((other is null) ? _nodes : _nodes.Concat(other));

        IXObjectSelector<XContainer> IXObjectSelector<XContainer>.Concat(params XContainer[] other) => Concat(other);

        public XContainerSelector Concat(IEnumerable<XContainer> other) => new((other is null) ? _nodes : _nodes.Concat(other));

        IXObjectSelector<XContainer> IXObjectSelector<XContainer>.Concat(IEnumerable<XContainer> other) => Concat(other);

        public XNodeSelector DescendantNodes() => new(_nodes.DescendantNodes());

        public XElementSelector Descendants() => new(_nodes.Descendants());

        public XElementSelector Descendants(string localName) => new(_nodes.Descendants(XName.Get(localName)));

        public XElementSelector Descendants(string localName, string namespaceURI) => new(_nodes.Descendants((namespaceURI is null) ? XName.Get(localName) : XNamespace.Get(namespaceURI).GetName(localName)));

        public XElementSelector Descendants(XName name) => new(_nodes.Descendants(name));

        public XElementSelector Elements() => new(_nodes.Elements());

        public XElementSelector Elements(string localName) => new(_nodes.Elements(XName.Get(localName)));

        public XElementSelector Elements(string localName, string namespaceURI) => new(_nodes.Elements((namespaceURI is null) ? XName.Get(localName) : XNamespace.Get(namespaceURI).GetName(localName)));

        public XElementSelector Elements(XName name) => new(_nodes.Elements(name));

        public XContainer FirstOrDefault() => _nodes.FirstOrDefault();

        public IEnumerable<XContainer> GetItems() => _nodes;

        public XContainer ItemAtOrDefault(int index) => _nodes.ElementAtOrDefault(index);

        public XContainer LastOrDefault() => _nodes.LastOrDefault();

        public XContainerSelector InDocumentOrder() => new(_nodes.InDocumentOrder());

        IXNodeSelector<XContainer> IXNodeSelector<XContainer>.InDocumentOrder() => InDocumentOrder();

        public XNodeSelector Nodes() => new(_nodes.Nodes());

        public void Remove() => _nodes.Remove();

        public XElementSelector ElementsWithAttribute(string attributeName, string value) => ElementsWithAttribute(XName.Get(attributeName), value, null);

        public XElementSelector ElementsWithAttribute(string attributeName, string value, IEqualityComparer<string> comparer) => ElementsWithAttribute(XName.Get(attributeName), value, comparer);

        public XElementSelector ElementsWithAttribute(string elementName, string attributeName, string value) =>
            ElementsWithAttribute(XName.Get(elementName), XName.Get(attributeName), value, null);

        public XElementSelector ElementsWithAttribute(string elementName, string attributeName, string value, IEqualityComparer<string> comparer) =>
            ElementsWithAttribute(XName.Get(elementName), XName.Get(attributeName), value, comparer);

        public XElementSelector ElementsWithAttribute(string elementName, string namespaceURI, string attributeName, string value) =>
            ElementsWithAttribute((namespaceURI == null) ? XName.Get(elementName) : XNamespace.Get(namespaceURI).GetName(elementName), XName.Get(attributeName), value, null);

        public XElementSelector ElementsWithAttribute(string elementName, string namespaceURI, string attributeName, string value, IEqualityComparer<string> comparer) =>
            ElementsWithAttribute((namespaceURI == null) ? XName.Get(elementName) : XNamespace.Get(namespaceURI).GetName(elementName), XName.Get(attributeName), value, comparer);

        public XElementSelector ElementsWithAttribute(string elementName, string elementNamespace, string attributeName, string attributeNamespace, string value) =>
            ElementsWithAttribute((elementNamespace == null) ? XName.Get(elementName) : XNamespace.Get(elementNamespace).GetName(elementName),
            (attributeNamespace == null) ? XName.Get(attributeName) : XNamespace.Get(attributeNamespace).GetName(attributeName), value, null);

        public XElementSelector ElementsWithAttribute(string elementName, string elementNamespace, string attributeName, string attributeNamespace, string value, IEqualityComparer<string> comparer) =>
            ElementsWithAttribute((elementNamespace == null) ? XName.Get(elementName) : XNamespace.Get(elementNamespace).GetName(elementName),
            (attributeNamespace == null) ? XName.Get(attributeName) : XNamespace.Get(attributeNamespace).GetName(attributeName), value, comparer);

        public XElementSelector ElementsWithAttribute(XName name, string value) => ElementsWithAttribute(name, value, null);

        public XElementSelector ElementsWithAttribute(XName name, string value, IEqualityComparer<string> comparer)
        {
            if (value is null)
                return new(_nodes.Elements().Where(e => e.Attribute(name) is null));
            if (comparer is null) comparer = StringComparer.Ordinal;
            return new(_nodes.Elements().Where(e =>
            {
                XAttribute a = e.Attribute(name);
                return a is not null && comparer.Equals(a.Value, value);
            }));
        }

        public XElementSelector ElementsWithAttribute(XName elementName, XName attributeName, string value) => ElementsWithAttribute(elementName, attributeName, value, null);

        public XElementSelector ElementsWithAttribute(XName elementName, XName attributeName, string value, IEqualityComparer<string> comparer)
        {
            if (value is null)
                return new(_nodes.Elements(elementName).Where(e => e.Attribute(attributeName) is null));
            if (comparer is null) comparer = StringComparer.Ordinal;
            return new(_nodes.Elements(elementName).Where(e =>
            {
                XAttribute a = e.Attribute(attributeName);
                return a is not null && comparer.Equals(a.Value, value);
            }));
        }

        public XElementSelector ElementsWithChildElement(string childElementName, string value) => ElementsWithChildElement(XName.Get(childElementName), value, null);

        public XElementSelector ElementsWithChildElement(string childElementName, string value, IEqualityComparer<string> comparer) => ElementsWithChildElement(XName.Get(childElementName), value, comparer);

        public XElementSelector ElementsWithChildElement(string selectedElementName, string childElementName, string value) =>
            ElementsWithChildElement(XName.Get(selectedElementName), XName.Get(childElementName), value, null);

        public XElementSelector ElementsWithChildElement(string selectedElementName, string childElementName, string value, IEqualityComparer<string> comparer) =>
            ElementsWithChildElement(XName.Get(selectedElementName), XName.Get(childElementName), value, comparer);

        public XElementSelector ElementsWithChildElement(string selectedElementName, string namespaceURI, string childElementName, string value) =>
            ElementsWithChildElement((namespaceURI == null) ? XName.Get(selectedElementName) : XNamespace.Get(namespaceURI).GetName(selectedElementName), XName.Get(childElementName), value, null);

        public XElementSelector ElementsWithChildElement(string selectedElementName, string namespaceURI, string childElementName, string value, IEqualityComparer<string> comparer) =>
            ElementsWithChildElement((namespaceURI == null) ? XName.Get(selectedElementName) : XNamespace.Get(namespaceURI).GetName(selectedElementName), XName.Get(childElementName), value, comparer);

        public XElementSelector ElementsWithChildElement(string selectedElementName, string selectedElementNamespace, string childElementName, string childElementNamespace, string value) =>
            ElementsWithChildElement((selectedElementNamespace == null) ? XName.Get(selectedElementName) : XNamespace.Get(selectedElementNamespace).GetName(selectedElementName),
            (childElementNamespace == null) ? XName.Get(childElementName) : XNamespace.Get(childElementNamespace).GetName(childElementName), value, null);

        public XElementSelector ElementsWithChildElement(string selectedElementName, string selectedElementNamespace, string childElementName, string childElementNamespace, string value,
            IEqualityComparer<string> comparer) => ElementsWithChildElement((selectedElementNamespace == null) ? XName.Get(selectedElementName) : XNamespace.Get(selectedElementNamespace).GetName(selectedElementName),
                (childElementNamespace == null) ? XName.Get(childElementName) : XNamespace.Get(childElementNamespace).GetName(childElementName), value, comparer);

        public XElementSelector ElementsWithChildElement(XName childElementName, string value) => ElementsWithChildElement(childElementName, value, null);

        public XElementSelector ElementsWithChildElement(XName childElementName, string value, IEqualityComparer<string> comparer)
        {
            if (value is null)
                return new(_nodes.Elements().Where(e => e.Element(childElementName) is null));
            if (comparer is null) comparer = StringComparer.Ordinal;
            return new(_nodes.Elements().Where(e =>
            {
                XElement c = e.Element(childElementName);
                return c is not null && !c.IsEmpty && comparer.Equals(c.Value, value);
            }));
        }

        public XElementSelector ElementsWithChildElement(XName selectedElementName, XName childElementName, string value) => ElementsWithChildElement(selectedElementName, childElementName, value, null);

        public XElementSelector ElementsWithChildElement(XName selectedElementName, XName childElementName, string value, IEqualityComparer<string> comparer)
        {
            if (value is null)
                return new(_nodes.Elements(selectedElementName).Where(e => e.Element(childElementName) is null));
            if (comparer is null) comparer = StringComparer.Ordinal;
            return new(_nodes.Elements(selectedElementName).Where(e =>
            {
                XElement c = e.Element(childElementName);
                return c is not null && !c.IsEmpty && comparer.Equals(c.Value, value);
            }));
        }
    }
}
