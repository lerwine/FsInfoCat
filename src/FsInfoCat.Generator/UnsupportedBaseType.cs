using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class UnsupportedBaseType : BaseTypeData
    {
        public const string RootElementName = "UnsupportedBaseType";

        public UnsupportedBaseType() { }

        public UnsupportedBaseType(BaseTypeSyntax syntax) : base(syntax) { }
    }
}
