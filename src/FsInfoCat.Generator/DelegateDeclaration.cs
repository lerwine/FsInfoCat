using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class DelegateDeclaration : MemberDeclaration
    {
        public const string RootElementName = "Delegate";

        public string Name { get; set; }

        protected internal override string GetName() => Name;

        public DelegateDeclaration() { }

        public DelegateDeclaration(DelegateDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
        }
    }
}
