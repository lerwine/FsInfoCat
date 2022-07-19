using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class MethodDeclaration : BaseMethodDeclaration
    {
        public const string RootElementName = "Method";

        [XmlAttribute()]
        public string Name { get; set; }

        public MethodDeclaration() { }

        public MethodDeclaration(MethodDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
        }
    }
}
