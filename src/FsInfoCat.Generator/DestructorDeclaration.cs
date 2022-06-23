using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class DestructorDeclaration : BaseMethodDeclaration
    {
        public const string RootElementName = "Destructor";

        public DestructorDeclaration() { }

        public DestructorDeclaration(DestructorDeclarationSyntax syntax) : base(syntax) { }
    }
}
