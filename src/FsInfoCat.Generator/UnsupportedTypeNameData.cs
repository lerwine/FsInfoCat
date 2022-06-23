using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class UnsupportedTypeNameData : TypeNameData
    {
        public const string RootElementName = "UnsupportedTypeName";

        public UnsupportedTypeNameData() { }

        public UnsupportedTypeNameData(NameSyntax syntax) : base(syntax) { }
    }
}
