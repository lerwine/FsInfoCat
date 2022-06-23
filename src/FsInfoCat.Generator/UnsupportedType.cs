using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class UnsupportedType : TypeData
    {
        public const string RootElementName = "UnsupportedType";

        public UnsupportedType() { }

        public UnsupportedType(TypeSyntax syntax) : base(syntax) { }
    }
}
