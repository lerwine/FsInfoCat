using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DevUtil
{
    public class XElementSelector(IEnumerable<XElement> items) : IXContainerSelector<XElement>
    {
        private readonly IEnumerable<XElement> _elements = (items is null) ? [] : items.Where(e => e is not null);

        public XElementSelector(params XElement[] items) : this((IEnumerable<XElement>)items) { }

        public XElementSelector Ancestors() => new(_elements.Ancestors());

        public XElementSelector Ancestors(string localName) => new(_elements.Ancestors(XName.Get(localName)));

        public XElementSelector Ancestors(string localName, string namespaceURI) => new(_elements.Ancestors((namespaceURI is null) ? XName.Get(localName) : XNamespace.Get(namespaceURI).GetName(localName)));

        public XElementSelector Ancestors(XName name) => new(_elements.Ancestors(name));

        public bool Any() => _elements.Any();

        public XElementSelector Concat(params XElement[] other) => new((other is null) ? _elements : _elements.Concat(other));

        IXObjectSelector<XElement> IXObjectSelector<XElement>.Concat(params XElement[] other) => Concat(other);

        public XElementSelector Concat(IEnumerable<XElement> other) => new((other is null) ? _elements : _elements.Concat(other));

        IXObjectSelector<XElement> IXObjectSelector<XElement>.Concat(IEnumerable<XElement> other) => Concat(other);

        public XNodeSelector DescendantNodes() => new(_elements.DescendantNodes());

        public XElementSelector Descendants() => new(_elements.Descendants());

        public XElementSelector Descendants(string localName) => new(_elements.Descendants(XName.Get(localName)));

        public XElementSelector Descendants(string localName, string namespaceURI) => new(_elements.Descendants((namespaceURI is null) ? XName.Get(localName) : XNamespace.Get(namespaceURI).GetName(localName)));

        public XElementSelector Descendants(XName name) => new(_elements.Descendants(name));

        public XElementSelector Elements() => new(_elements.Elements());

        public XElementSelector Elements(string localName) => new(_elements.Elements(XName.Get(localName)));

        public XElementSelector Elements(string localName, string namespaceURI) => new(_elements.Elements((namespaceURI is null) ? XName.Get(localName) : XNamespace.Get(namespaceURI).GetName(localName)));

        public XElementSelector Elements(XName name) => new(_elements.Elements(name));

        public XElement FirstOrDefault() => _elements.FirstOrDefault();

        public IEnumerable<XElement> GetItems() => _elements;

        public IEnumerable<XElementSelector> SingleFromEach() => _elements.Select(e => new XElementSelector(e));

        IEnumerable<IXContainerSelector<XElement>> IXContainerSelector<XElement>.SingleFromEach() => SingleFromEach();

        IEnumerable<IXNodeSelector<XElement>> IXNodeSelector<XElement>.SingleFromEach() => SingleFromEach();

        IEnumerable<IXObjectSelector<XElement>> IXObjectSelector<XElement>.SingleFromEach() => SingleFromEach();

        public XElement ItemAtOrDefault(int index) => _elements.ElementAtOrDefault(index);

        public XElement LastOrDefault() => _elements.LastOrDefault();

        public XElementSelector InDocumentOrder() => new(_elements.InDocumentOrder());

        IXNodeSelector<XElement> IXNodeSelector<XElement>.InDocumentOrder() => InDocumentOrder();

        public XNodeSelector Nodes() => new(_elements.Nodes());

        public void Remove() => _elements.Remove();

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
                return new(_elements.Elements().Where(e => e.Attribute(name) is null));
            if (comparer is null) comparer = StringComparer.Ordinal;
            return new(_elements.Elements().Where(e =>
            {
                XAttribute a = e.Attribute(name);
                return a is not null && comparer.Equals(a.Value, value);
            }));
        }

        public XElementSelector ElementsWithAttribute(XName elementName, XName attributeName, string value) => ElementsWithAttribute(elementName, attributeName, value, null);

        public XElementSelector ElementsWithAttribute(XName elementName, XName attributeName, string value, IEqualityComparer<string> comparer)
        {
            if (value is null)
                return new(_elements.Elements(elementName).Where(e => e.Attribute(attributeName) is null));
            if (comparer is null) comparer = StringComparer.Ordinal;
            return new(_elements.Elements(elementName).Where(e =>
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
                return new(_elements.Elements().Where(e => e.Element(childElementName) is null));
            if (comparer is null) comparer = StringComparer.Ordinal;
            return new(_elements.Elements().Where(e =>
            {
                XElement c = e.Element(childElementName);
                return c is not null && !c.IsEmpty && comparer.Equals(c.Value, value);
            }));
        }

        public XElementSelector ElementsWithChildElement(XName selectedElementName, XName childElementName, string value) => ElementsWithChildElement(selectedElementName, childElementName, value, null);

        public XElementSelector ElementsWithChildElement(XName selectedElementName, XName childElementName, string value, IEqualityComparer<string> comparer)
        {
            if (value is null)
                return new(_elements.Elements(selectedElementName).Where(e => e.Element(childElementName) is null));
            if (comparer is null) comparer = StringComparer.Ordinal;
            return new(_elements.Elements(selectedElementName).Where(e =>
            {
                XElement c = e.Element(childElementName);
                return c is not null && !c.IsEmpty && comparer.Equals(c.Value, value);
            }));
        }

        public XElementSelector AncestorsAndSelf() => new(_elements.AncestorsAndSelf());

        public XElementSelector AncestorsAndSelf(string name) => AncestorsAndSelf(XName.Get(name));

        public XElementSelector AncestorsAndSelf(string name, string namespaceURI) => AncestorsAndSelf((namespaceURI == null) ? XName.Get(name) : XNamespace.Get(namespaceURI).GetName(name));

        public XElementSelector AncestorsAndSelf(XName name) => new(_elements.AncestorsAndSelf(name));

        public XAttributeSelector Attributes() => new(_elements.Attributes());

        public XAttributeSelector Attributes(string name) => Attributes(XName.Get(name));

        public XAttributeSelector Attributes(string name, string namespaceURI) => Attributes((namespaceURI == null) ? XName.Get(name) : XNamespace.Get(namespaceURI).GetName(name));

        public XAttributeSelector Attributes(XName name) => new(_elements.Attributes(name));

        public XNodeSelector DescendantNodesAndSelf() => new(_elements.DescendantNodesAndSelf());

        public XElementSelector DescendantsAndSelf() => new(_elements.DescendantsAndSelf());

        public XElementSelector DescendantsAndSelf(string name) => DescendantsAndSelf(XName.Get(name));

        public XElementSelector DescendantsAndSelf(string name, string namespaceURI) => DescendantsAndSelf((namespaceURI == null) ? XName.Get(name) : XNamespace.Get(namespaceURI).GetName(name));

        public XElementSelector DescendantsAndSelf(XName name) => new(_elements.DescendantsAndSelf(name));

        public bool Contains(XElement other) => other is not null && _elements.Any(e => ReferenceEquals(e, other));
    }
}
