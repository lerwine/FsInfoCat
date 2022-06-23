using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class InterfaceDeclaration : TypeDeclaration
    {
        public const string RootElementName = "Interface";

        public InterfaceDeclaration() { }

        public InterfaceDeclaration(InterfaceDeclarationSyntax syntax) : base(syntax) { }
    }
}
