using System;
using System.Xml.Linq;

namespace DevUtil
{
    public class PsXmlCData : PsXmlText, IEquatable<PsXmlCData>
    {
        public PsXmlCData(string value) : base(new XCData(value)) { }

        private PsXmlCData(XCData cData) : base(cData) { }

        internal static PsXmlCData GetAssociatedNode(XCData cData)
        {
            if (cData is null) return null;
            PsXmlCData node = cData.Annotation<PsXmlCData>();
            if (node is null) return new PsXmlCData(cData);
            return node;
        }

        public bool Equals(PsXmlCData other) => EqualityComparer.Equals(this, other);

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is PsXmlCData attribute) return Equals(attribute);
            return obj is string @string && Equals(@string);
        }
    }
}
