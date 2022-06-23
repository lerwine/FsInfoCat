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

        internal static TupleElement Create(TupleElementSyntax syntax) => new TupleElement()
        {
            Type = TypeData.CreateTypeData(syntax.Type),
            Identifier = syntax.Identifier.ValueText
        };
    }
}
