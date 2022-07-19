using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class UnsupportedSimpleTypeName : SimpleTypeName
    {
        public const string RootElementName = "UnsupportedSimpleTypeName";

        public UnsupportedSimpleTypeName() { }

        public UnsupportedSimpleTypeName(SimpleNameSyntax syntax) : base(syntax) { }
    }
}
