using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class EnumDeclaration : BaseTypeDeclaration
    {
        public const string RootElementName = "Enum";

        [XmlAttribute()]
        public string Name { get; set; }

        public EnumDeclaration() { }

        public EnumDeclaration(EnumDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
            foreach (EnumMemberDeclarationSyntax memberDeclaration in syntax.Members)
                Components.Add(EnumMemberDeclaration.Create(memberDeclaration));
        }

        protected internal override string GetName() => Name;
    }
}
