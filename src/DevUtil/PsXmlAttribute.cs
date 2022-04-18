using System;
using System.Xml;
using System.Xml.Linq;

namespace DevUtil
{
    public class PsXmlAttribute : IEquatable<PsXmlAttribute>
    {
        private readonly XAttribute _attribute;

        public PsXmlAttribute(string localName, string namespaceName, string value) => (_attribute = new XAttribute(XName.Get(localName, namespaceName), value)).AddAnnotation(this);

        public PsXmlAttribute(string expandedName, string value) => (_attribute = new XAttribute(XName.Get(expandedName), value)).AddAnnotation(this);

        private PsXmlAttribute(XAttribute attribute) => (_attribute = attribute).AddAnnotation(this);

        public string Name => _attribute.Name.LocalName;

        public string NamespaceName => _attribute.Name.NamespaceName;

        public string Value => _attribute.Value;

        public bool IsNamespaceDeclaration => _attribute.IsNamespaceDeclaration;

        public bool Equals(PsXmlAttribute other) => other is not null && (ReferenceEquals(this, other) || (_attribute.Value == other._attribute.Value && _attribute.Name == other._attribute.Name));

        public override bool Equals(object obj) => Equals(obj as PsXmlAttribute);

        public override int GetHashCode() => _attribute.GetHashCode();

        public PsXmlAttribute GetNextAttribute() => GetAssociatedNode(_attribute.NextAttribute);

        public PsXmlElement GetParent() => PsXmlElement.GetAssociatedNode(_attribute.Parent);

        public PsXmlAttribute GetPreviousAttribute() => GetAssociatedNode(_attribute.PreviousAttribute);

        public PsXmlNode.Document GetSource() => _attribute.Document?.Root?.Annotation<PsXmlNode.Document>();

        public void Remove() => _attribute.Remove();

        internal static PsXmlAttribute GetAssociatedNode(XAttribute attribute)
        {
            if (attribute is null) return null;
            PsXmlAttribute node = attribute.Annotation<PsXmlAttribute>();
            if (node is null) return new PsXmlAttribute(attribute);
            return node;
        }
    }
}
