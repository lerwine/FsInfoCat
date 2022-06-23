using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class TupleElement
    {
        public const string RootElementName = "Element";

        [XmlAttribute()]
        public string Identifier { get; set; }

        public TypeData Type { get; set; }

        public TupleElement(TupleElementSyntax syntax)
        {
            Type = TypeData.CreateTypeData(syntax.Type);
            Identifier = syntax.Identifier.ValueText;
        }
    }
}
