using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class ConstructorDeclaration : BaseMethodDeclaration
    {
        public const string RootElementName = "Constructor";

        protected internal override string GetName() => null;

        public ConstructorDeclaration() { }

        public ConstructorDeclaration(ConstructorDeclarationSyntax syntax) : base(syntax) { }
    }
}
