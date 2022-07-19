using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class DestructorDeclaration : BaseMethodDeclaration
    {
        public const string RootElementName = "Destructor";

        public DestructorDeclaration() { }

        public DestructorDeclaration(DestructorDeclarationSyntax syntax) : base(syntax) { }
    }
}
