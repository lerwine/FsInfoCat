using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class UnsupportedBaseType : BaseTypeData
    {
        public const string RootElementName = "UnsupportedBaseType";

        public UnsupportedBaseType() { }

        public UnsupportedBaseType(BaseTypeSyntax syntax) : base(syntax) { }
    }
}
