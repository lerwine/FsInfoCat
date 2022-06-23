using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class RecordDeclaration : TypeDeclaration
    {
        public const string RootElementName = "Record";

        public RecordDeclaration() { }

        public RecordDeclaration(RecordDeclarationSyntax syntax) : base(syntax) { }
    }
}
