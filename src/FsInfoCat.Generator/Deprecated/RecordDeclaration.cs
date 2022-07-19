using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class RecordDeclaration : TypeDeclaration
    {
        public const string RootElementName = "Record";

        public RecordDeclaration() { }

        public RecordDeclaration(RecordDeclarationSyntax syntax) : base(syntax) { }
    }
}
