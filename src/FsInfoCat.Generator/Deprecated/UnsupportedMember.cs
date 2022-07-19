using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class UnsupportedMember : MemberDeclaration
    {
        public const string RootElementName = "UnsupportedMember";

        public UnsupportedMember() { }

        public UnsupportedMember(MemberDeclarationSyntax syntax) : base(syntax) { }
    }
}
