using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class DestructorDeclaration : BaseMethodDeclaration
    {
        public const string RootElementName = "Destructor";

        protected internal override string GetName() => null;

        public DestructorDeclaration() { }

        public DestructorDeclaration(DestructorDeclarationSyntax syntax) : base(syntax) { }
    }
}
