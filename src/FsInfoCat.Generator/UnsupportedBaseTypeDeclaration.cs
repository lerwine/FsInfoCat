using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class UnsupportedBaseTypeDeclaration : BaseTypeDeclaration
    {
        public const string RootElementName = "UnsupportedBaseTypeDeclaration";

        public UnsupportedBaseTypeDeclaration() { }

        public UnsupportedBaseTypeDeclaration(BaseTypeDeclarationSyntax syntax) : base(syntax) { }

        protected internal override string GetName() => null;
    }
}
