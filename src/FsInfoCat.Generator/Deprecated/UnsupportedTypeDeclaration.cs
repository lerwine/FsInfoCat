using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class UnsupportedTypeDeclaration : TypeDeclaration
    {
        public const string RootElementName = "UnsupportedTypeDeclaration";

        public UnsupportedTypeDeclaration() { }

        public UnsupportedTypeDeclaration(TypeDeclarationSyntax syntax) : base(syntax) { }
    }
}
