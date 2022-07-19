using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class UnsupportedBaseTypeDeclaration : BaseTypeDeclaration
    {
        public const string RootElementName = "UnsupportedBaseTypeDeclaration";

        public UnsupportedBaseTypeDeclaration() { }

        public UnsupportedBaseTypeDeclaration(BaseTypeDeclarationSyntax syntax) : base(syntax) { }
    }
}
