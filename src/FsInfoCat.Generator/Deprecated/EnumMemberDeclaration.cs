using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
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
