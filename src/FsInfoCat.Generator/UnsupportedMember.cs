using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class UnsupportedMember : MemberDeclaration
    {
        public const string RootElementName = "UnsupportedMember";

        public UnsupportedMember() { }

        public UnsupportedMember(MemberDeclarationSyntax syntax) : base(syntax) { }
    }
}
