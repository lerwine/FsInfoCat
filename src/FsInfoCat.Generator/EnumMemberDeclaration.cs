using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class EnumMemberDeclaration : MemberDeclaration
    {
        public const string RootElementName = "Member";

        [XmlAttribute()]
        public string Name { get; set; }

        public EnumMemberDeclaration() { }

        public EnumMemberDeclaration(EnumMemberDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
        }
    }
}
