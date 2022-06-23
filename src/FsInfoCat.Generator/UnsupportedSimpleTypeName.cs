using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class UnsupportedSimpleTypeName : SimpleTypeName
    {
        public const string RootElementName = "UnsupportedSimpleTypeName";

        public UnsupportedSimpleTypeName(SimpleNameSyntax syntax) : base(syntax) { }
    }
}
