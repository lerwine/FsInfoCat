using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DevUtil
{
    public abstract partial class PsXmlNode : IEquatable<PsXmlNode>
    {
        public static PsXmlEqualityComparer EqualityComparer { get; } = PsXmlEqualityComparer.Instance;

        protected abstract XNode GetXNode();

        internal static PsXmlNode GetAssociatedXmlNode(XNode node)
        {
            if (node is XElement element) return PsXmlElement.GetAssociatedNode(element);
            if (node is XCData cData) return PsXmlCData.GetAssociatedNode(cData);
            if (node is XText text) return PsXmlText.GetAssociatedNode(text);
            return (node is XComment comment) ? PsXmlComment.GetAssociatedNode(comment) : null;
        }

        public bool IsAfter(PsXmlNode node) => GetXNode().IsAfter(node?.GetXNode());

        public bool IsBefore(PsXmlNode node) => GetXNode().IsBefore(node?.GetXNode());

        public virtual void Remove() => GetXNode().Remove();

        public PsXmlNode GetPreviousNode()
        {
            XNode previous = GetXNode().PreviousNode;
            if (previous is null) return null;
            PsXmlNode node;
            while ((node = GetAssociatedXmlNode(previous)) is null)
                if ((previous = previous.PreviousNode) is null) return null;
            return node;
        }

        public PsXmlNode GetNextNode()
        {
            XNode next = GetXNode().NextNode;
            if (next is null) return null;
            PsXmlNode node;
            while ((node = GetAssociatedXmlNode(next)) is null)
                if ((next = next.NextNode) is null) return null;
            return node;
        }

        public Document GetSource() => GetXNode().Document?.Root?.Annotation<Document>();

        public PsXmlElement GetParent() => PsXmlElement.GetAssociatedNode(GetXNode().Parent);

        public PsXmlElement GetPreviousElement() => PsXmlElement.GetAssociatedNode(GetXNode().ElementsBeforeSelf().FirstOrDefault());

        public PsXmlElement GetNextElement() => PsXmlElement.GetAssociatedNode(GetXNode().ElementsAfterSelf().FirstOrDefault());

        public IEnumerable<PsXmlNode> GetNodesBeforeSelf() => GetXNode().NodesBeforeSelf().Select(e => GetAssociatedXmlNode(e)).Where(n => n is not null);

        public IEnumerable<PsXmlNode> GetNodesAfterSelf() => GetXNode().NodesAfterSelf().Select(e => GetAssociatedXmlNode(e)).Where(n => n is not null);

        public IEnumerable<PsXmlElement> GetElementsBeforeSelf() => GetXNode().ElementsBeforeSelf().Select(e => PsXmlElement.GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetElementsBeforeSelf(string localName, string namespaceName) => GetXNode().ElementsBeforeSelf(XName.Get(localName, namespaceName)).Select(e => PsXmlElement.GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetElementsAfterSelf(string localName, string namespaceName) => GetXNode().ElementsAfterSelf(XName.Get(localName, namespaceName)).Select(e => PsXmlElement.GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetElementsAfterSelf(string expandedName) => GetXNode().ElementsAfterSelf(XName.Get(expandedName)).Select(e => PsXmlElement.GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetElementsAfterSelf() => GetXNode().ElementsAfterSelf().Select(e => PsXmlElement.GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetAncestors(string localName, string namespaceName) => GetXNode().Ancestors(XName.Get(localName, namespaceName)).Select(e => PsXmlElement.GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetAncestors(string expandedName) => GetXNode().Ancestors(XName.Get(expandedName)).Select(e => PsXmlElement.GetAssociatedNode(e));

        public IEnumerable<PsXmlElement> GetAncestors() => GetXNode().Ancestors().Select(e => PsXmlElement.GetAssociatedNode(e));

        public static int CompareDocumentOrder(PsXmlNode n1, PsXmlNode n2) => XNode.CompareDocumentOrder(n1?.GetXNode(), n2?.GetXNode());

        public static bool DeepEquals(PsXmlNode n1, PsXmlNode n2) => XNode.DeepEquals(n1?.GetXNode(), n2?.GetXNode());

        public bool Equals(PsXmlNode other) => EqualityComparer.Equals(this, other);

        public static bool TryConvertToBoolean(string value, out bool other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToBoolean(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToByte(string value, out byte other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToByte(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToSByte(string value, out sbyte other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToSByte(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToInt16(string value, out short other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToInt16(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToUInt16(string value, out ushort other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToUInt16(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToInt32(string value, out int other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToInt32(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToUInt32(string value, out uint other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToUInt32(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToChar(string value, out char other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToChar(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToInt64(string value, out long other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToInt64(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToUInt64(string value, out ulong other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToUInt64(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToSingle(string value, out float other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToSingle(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToDouble(string value, out double other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToDouble(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToDecimal(string value, out decimal other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToDecimal(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToDateTime(string value, XmlDateTimeSerializationMode dateTimeOption, out DateTime other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToDateTime(value, dateTimeOption); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToTimeSpan(string value, out TimeSpan other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToTimeSpan(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertToGuid(string value, out Guid other)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                other = default;
                return false;
            }
            try { other = XmlConvert.ToGuid(value); }
            catch
            {
                other = default;
                return false;
            }
            return true;
        }
    }
}
