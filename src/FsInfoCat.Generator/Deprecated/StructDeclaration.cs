using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class StructDeclaration : TypeDeclaration
    {
        public const string RootElementName = "Struct";

        public StructDeclaration() { }

        public StructDeclaration(StructDeclarationSyntax syntax) : base(syntax) { }
    }
}