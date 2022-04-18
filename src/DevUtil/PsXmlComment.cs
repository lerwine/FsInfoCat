using System;
using System.Xml.Linq;

namespace DevUtil
{
    public class PsXmlComment : PsXmlNode, IEquatable<PsXmlComment>
    {
        private readonly XComment _comment;

        public string Value { get => _comment.Value; set => _comment.Value = value; }

        public PsXmlComment(string value) => (_comment = new XComment(value)).AddAnnotation(this);

        private PsXmlComment(XComment comment) => (_comment = comment).AddAnnotation(this);

        protected override XNode GetXNode() => _comment;

        internal static PsXmlComment GetAssociatedNode(XComment text)
        {
            if (text is null) return null;
            PsXmlComment node = text.Annotation<PsXmlComment>();
            if (node is null) return new PsXmlComment(text);
            return node;
        }

        public bool Equals(PsXmlComment other) => EqualityComparer.Equals(this, other);

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is PsXmlComment attribute) return Equals(attribute);
            return obj is string @string && Equals(@string);
        }

        public override int GetHashCode() => _comment.GetHashCode();
    }
}
