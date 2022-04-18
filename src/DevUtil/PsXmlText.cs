using System;
using System.Xml.Linq;

namespace DevUtil
{
    public class PsXmlText : PsXmlNode, IEquatable<PsXmlText>
    {
        private readonly XText _text;

        public string Value { get => _text.Value; set => _text.Value = value; }

        public PsXmlText(string value) => (_text = new XText(value)).AddAnnotation(this);

        protected PsXmlText(XText text) => (_text = text).AddAnnotation(this);

        protected override XNode GetXNode() => _text;

        internal static PsXmlText GetAssociatedNode(XText text)
        {
            if (text is null) return null;
            if (text is XCData cData) return PsXmlCData.GetAssociatedNode(cData);
            PsXmlText node = text.Annotation<PsXmlText>();
            if (node is null) return new PsXmlText(text);
            return node;
        }

        public bool Equals(PsXmlText other) => EqualityComparer.Equals(this, other);

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is PsXmlText attribute) return Equals(attribute);
            return obj is string @string && Equals(@string);
        }

        public override int GetHashCode() => _text.GetHashCode();
    }
}
