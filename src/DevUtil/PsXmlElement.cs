using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DevUtil
{
    public class NodeSet<T> where T : PsXmlNode
    {
        private readonly IEnumerable<T> _values;

        public NodeSet(IEnumerable<T> values) => _values = values;
    }

    public class PsXmlElement : PsXmlNode, IEquatable<PsXmlElement>
    {
        private readonly XElement _element;

        public string Name => _element.Name.LocalName;

        public string NamespaceName => _element.Name.NamespaceName;

        public string Value
        {
            get => _element.IsEmpty ? null : _element.Value;
            set
            {
                if (value is null)
                    foreach (XText text in _element.Nodes().OfType<XText>().ToArray())
                        text.Remove();
                else
                    _element.Value = value;
            }
        }

        public bool IsEmpty => _element.IsEmpty;

        public bool HasElements => _element.HasElements;

        public bool HasAttributes => _element.HasAttributes;

        private PsXmlElement(XElement element) => (_element = element).AddAnnotation(this);

        public PsXmlElement(string localName, string namespaceName) => (_element = new XElement(XName.Get(localName, namespaceName))).AddAnnotation(this);

        public PsXmlElement(string expandedName) => (_element = new XElement(XName.Get(expandedName))).AddAnnotation(this);

        protected override XNode GetXNode() => _element;

        public PsXmlElement AddElement(PsXmlElement element)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));
            _element.Add(element);
            return this;
        }

        public PsXmlElement AddNewElement(string localName, string namespaceName)
        {
            PsXmlElement builder = new PsXmlElement(localName, namespaceName);
            _element.Add(builder._element);
            return builder;
        }

        public PsXmlElement AddNewElement(string expandedName)
        {
            PsXmlElement builder = new PsXmlElement(expandedName);
            _element.Add(builder._element);
            return builder;
        }

        public override bool Equals(object obj) => Equals(obj as PsXmlElement);

        public override int GetHashCode() => _element.GetHashCode();

        public IEnumerable<PsXmlElement> GetAncestorsAndSelf(string localName, string namespaceName) => _element.AncestorsAndSelf(XName.Get(localName, namespaceName)).Select(e => GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetAncestorsAndSelf(string expandedName) => _element.AncestorsAndSelf(XName.Get(expandedName)).Select(e => GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetAncestorsAndSelf() => _element.AncestorsAndSelf().Select(e => GetAssociatedNode(e));

        public PsXmlAttribute GetAttribute(string localName, string namespaceName) => PsXmlAttribute.GetAssociatedNode(_element.Attribute(XName.Get(localName, namespaceName)));

        public PsXmlAttribute GetAttribute(string expandedName) => PsXmlAttribute.GetAssociatedNode(_element.Attribute(XName.Get(expandedName)));

        public IEnumerable<PsXmlAttribute> GetAttributes(string localName, string namespaceName) => _element.Attributes(XName.Get(localName, namespaceName)).Select(e => PsXmlAttribute.GetAssociatedNode(e));

        public IEnumerable<PsXmlAttribute> GetAttributes(string expandedName) => _element.Attributes(XName.Get(expandedName)).Select(e => PsXmlAttribute.GetAssociatedNode(e));

        public IEnumerable<PsXmlAttribute> GetAttributes() => _element.Attributes().Select(e => PsXmlAttribute.GetAssociatedNode(e));

        public IEnumerable<PsXmlNode> GetDescendantNodesAndSelf() => _element.DescendantNodesAndSelf().Select(n => GetAssociatedXmlNode(n)).Where(n => n is not null);

        public IEnumerable<PsXmlElement> GetDescendantsAndSelf(string localName, string namespaceName) => _element.DescendantsAndSelf(XName.Get(localName, namespaceName)).Select(e => GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetDescendantsAndSelf(string expandedName) => _element.DescendantsAndSelf(XName.Get(expandedName)).Select(e => GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetDescendantsAndSelf() => _element.DescendantsAndSelf().Select(e => GetAssociatedNode(e));

        public PsXmlNode GetElement(string localName, string namespaceName) => GetAssociatedNode(_element.Element(XName.Get(localName, namespaceName)));

        public PsXmlNode GetElement(string expandedName) => GetAssociatedNode(_element.Element(XName.Get(expandedName)));

        public IEnumerable<PsXmlNode> GetElements(string localName, string namespaceName) => _element.Elements(XName.Get(localName, namespaceName)).Select(e => GetAssociatedNode(e));

        public IEnumerable<PsXmlNode> GetElements(string expandedName) => _element.Elements(XName.Get(expandedName)).Select(e => GetAssociatedNode(e));

        public IEnumerable<PsXmlNode> GetElements() => _element.Elements().Select(e => GetAssociatedNode(e));

        private static XName GetXName(string elementLocalName, string elementNamespaceName, string attributeLocalName, string attributeNamespaceName, out XName attributeName)
        {
            if (string.IsNullOrWhiteSpace(elementLocalName)) throw new ArgumentException($"'{nameof(elementLocalName)}' cannot be null or whitespace.", nameof(elementLocalName));
            if (elementNamespaceName is null)
                elementNamespaceName = "";
            else if (elementNamespaceName.Length > 0 && !Uri.IsWellFormedUriString(elementNamespaceName, UriKind.Absolute))
                throw new ArgumentException(Uri.IsWellFormedUriString(elementNamespaceName, UriKind.Relative) ? $"'{nameof(elementNamespaceName)}' is not an absolute URI." :
                    $"'{nameof(elementNamespaceName)}' is not a well-formed absolute URI.", nameof(elementNamespaceName));
            if (string.IsNullOrWhiteSpace(attributeLocalName)) throw new ArgumentException($"'{nameof(attributeLocalName)}' cannot be null or whitespace.", nameof(attributeLocalName));
            if (attributeNamespaceName is null)
                attributeNamespaceName = "";
            else if (attributeNamespaceName.Length > 0 && !Uri.IsWellFormedUriString(attributeNamespaceName, UriKind.Absolute))
                throw new ArgumentException(Uri.IsWellFormedUriString(attributeNamespaceName, UriKind.Relative) ? $"'{nameof(attributeNamespaceName)}' is not an absolute URI." :
                    $"'{nameof(attributeNamespaceName)}' is not a well-formed absolute URI.", nameof(attributeNamespaceName));

            try { attributeName = XName.Get(attributeLocalName, attributeNamespaceName); }
            catch (Exception exception) { throw new ArgumentException(string.IsNullOrWhiteSpace(exception.Message) ? $"'{nameof(attributeLocalName)}' is not a valid NCName." : exception.Message, nameof(attributeLocalName)); }

            try { return XName.Get(elementLocalName, elementNamespaceName); }
            catch (Exception exception) { throw new ArgumentException(string.IsNullOrWhiteSpace(exception.Message) ? $"'{nameof(elementLocalName)}' is not a valid NCName." : exception.Message, nameof(elementLocalName)); }
        }

        private static XName GetXName(string elementLocalName, string elementNamespaceName, string attributeExpandedName, out XName attributeName)
        {
            if (string.IsNullOrWhiteSpace(elementLocalName)) throw new ArgumentException($"'{nameof(elementLocalName)}' cannot be null or whitespace.", nameof(elementLocalName));
            if (elementNamespaceName is null)
                elementNamespaceName = "";
            else if (elementNamespaceName.Length > 0 && !Uri.IsWellFormedUriString(elementNamespaceName, UriKind.Absolute))
                throw new ArgumentException(Uri.IsWellFormedUriString(elementNamespaceName, UriKind.Relative) ? $"'{nameof(elementNamespaceName)}' is not an absolute URI." :
                    $"'{nameof(elementNamespaceName)}' is not a well-formed absolute URI.", nameof(elementNamespaceName));
            if (string.IsNullOrWhiteSpace(attributeExpandedName)) throw new ArgumentException($"'{nameof(attributeExpandedName)}' cannot be null or whitespace.", nameof(attributeExpandedName));

            try { attributeName = XName.Get(attributeExpandedName); }
            catch (Exception exception) { throw new ArgumentException(string.IsNullOrWhiteSpace(exception.Message) ? $"'{nameof(attributeExpandedName)}' is not a valid name." : exception.Message, nameof(attributeExpandedName)); }

            try { return XName.Get(elementLocalName, elementNamespaceName); }
            catch (Exception exception) { throw new ArgumentException(string.IsNullOrWhiteSpace(exception.Message) ? $"'{nameof(elementLocalName)}' is not a valid NCName." : exception.Message, nameof(elementLocalName)); }
        }

        private static XName GetXName(string elementExpandedName, string attributeExpandedName, out XName attributeName)
        {
            if (string.IsNullOrWhiteSpace(elementExpandedName)) throw new ArgumentException($"'{nameof(elementExpandedName)}' cannot be null or whitespace.", nameof(elementExpandedName));
            if (string.IsNullOrWhiteSpace(attributeExpandedName)) throw new ArgumentException($"'{nameof(attributeExpandedName)}' cannot be null or whitespace.", nameof(attributeExpandedName));

            try { attributeName = XName.Get(attributeExpandedName); }
            catch (Exception exception) { throw new ArgumentException(string.IsNullOrWhiteSpace(exception.Message) ? $"'{nameof(attributeExpandedName)}' is not a valid name." : exception.Message, nameof(attributeExpandedName)); }

            try { return XName.Get(elementExpandedName); }
            catch (Exception exception) { throw new ArgumentException(string.IsNullOrWhiteSpace(exception.Message) ? $"'{nameof(elementExpandedName)}' is not a valid name." : exception.Message, nameof(elementExpandedName)); }
        }

        public PsXmlElement GetElement(string localName, string namespaceName, Func<PsXmlElement, bool> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _element.Elements(XName.Get(localName, namespaceName)).Select(e => GetAssociatedNode(e)).FirstOrDefault(predicate);
        }

        public IEnumerable<PsXmlElement> GetElements(string localName, string namespaceName, Func<PsXmlElement, bool> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _element.Elements(XName.Get(localName, namespaceName)).Select(e => GetAssociatedNode(e)).Where(predicate);
        }

        public PsXmlElement GetElementWithAttribute(string elementLocalName, string elementNamespaceName, string attributeLocalName, string attributeNamespaceName, string value)
        {
            XName elementName = GetXName(elementLocalName, elementNamespaceName, attributeLocalName, attributeNamespaceName, out XName attributeName);
            if (value is null)
                return GetAssociatedNode(_element.Elements(elementName).FirstOrDefault(e => e.Attribute(attributeName) is null));
            return GetAssociatedNode(_element.Elements(elementName).Attributes(attributeName).Where(a => a.Value == value).Select(a => a.Parent).FirstOrDefault());
        }

        public PsXmlElement GetElementWithAttribute(string elementLocalName, string elementNamespaceName, string attributeExpandedName, string value)
        {
            XName elementName = GetXName(elementLocalName, elementNamespaceName, attributeExpandedName, out XName attributeName);
            if (value is null)
                return GetAssociatedNode(_element.Elements(elementName).FirstOrDefault(e => e.Attribute(attributeName) is null));
            return GetAssociatedNode(_element.Elements(elementName).Attributes(attributeName).Where(a => a.Value == value).Select(a => a.Parent).FirstOrDefault());
        }

        public PsXmlElement GetElementWithAttribute(string elementExpandedName, string attributeExpandedName, string value)
        {
            XName elementName = GetXName(elementExpandedName, attributeExpandedName, out XName attributeName);
            if (value is null)
                return GetAssociatedNode(_element.Elements(elementName).FirstOrDefault(e => e.Attribute(attributeName) is null));
            return GetAssociatedNode(_element.Elements(elementName).Attributes(attributeName).Where(a => a.Value == value).Select(a => a.Parent).FirstOrDefault());
        }

        public IEnumerable<PsXmlElement> GetElementsWithAttribute(string elementLocalName, string elementNamespaceName, string attributeLocalName, string attributeNamespaceName, string value)
        {
            XName elementName = GetXName(elementLocalName, elementNamespaceName, attributeLocalName, attributeNamespaceName, out XName attributeName);
            if (value is null)
                return _element.Elements(elementName).Where(e => e.Attribute(attributeName) is null).Select(e => GetAssociatedNode(e));
            return _element.Elements(elementName).Attributes(attributeName).Where(a => a.Value == value).Select(a => GetAssociatedNode(a.Parent));
        }

        public IEnumerable<PsXmlElement> GetElementsWithAttribute(string elementLocalName, string elementNamespaceName, string attributeExpandedName, string value)
        {
            XName elementName = GetXName(elementLocalName, elementNamespaceName, attributeExpandedName, out XName attributeName);
            if (value is null)
                return _element.Elements(elementName).Where(e => e.Attribute(attributeName) is null).Select(e => GetAssociatedNode(e));
            return _element.Elements(elementName).Attributes(attributeName).Where(a => a.Value == value).Select(a => GetAssociatedNode(a.Parent));
        }

        public IEnumerable<PsXmlElement> GetElementsWithAttribute(string elementExpandedName, string attributeExpandedName, string value)
        {
            XName elementName = GetXName(elementExpandedName, attributeExpandedName, out XName attributeName);
            if (value is null)
                return _element.Elements(elementName).Where(e => e.Attribute(attributeName) is null).Select(e => GetAssociatedNode(e));
            return _element.Elements(elementName).Attributes(attributeName).Where(a => a.Value == value).Select(a => GetAssociatedNode(a.Parent));
        }

        public PsXmlAttribute GetFirstAttribute() => PsXmlAttribute.GetAssociatedNode(_element.FirstAttribute);

        public PsXmlAttribute GetLastAttribute() => PsXmlAttribute.GetAssociatedNode(_element.LastAttribute);

        public override void Remove()
        {
            Document source = GetSource();
            if (source is not null && ReferenceEquals(this, source.Root))
                throw new InvalidOperationException();
            base.Remove();
        }

        public void RemoveAll() => _element.RemoveAll();

        public void RemoveAttributes() => _element.RemoveAttributes();

        public void SetAttributeValue(string expandedName, string value) => _element.SetAttributeValue(XName.Get(expandedName), value);

        public void SetElementValue(string expandedName, string value) => _element.SetElementValue(XName.Get(expandedName), value);

        internal static PsXmlElement GetAssociatedNode(XElement element)
        {
            if (element is null) return null;
            PsXmlElement node = element.Annotation<PsXmlElement>();
            if (node is null) return new PsXmlElement(element);
            return node;
        }

        public bool Equals(PsXmlElement other) => EqualityComparer.Equals(this, other);
    }
}
