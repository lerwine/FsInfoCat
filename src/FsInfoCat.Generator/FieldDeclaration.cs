using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class FieldDeclaration : BaseFieldDeclaration
    {
        public const string RootElementName = "Field";

        [XmlAttribute()]
        public string Name { get; set; }

        public FieldDeclaration() { }

        public FieldDeclaration(FieldDeclarationSyntax syntax) : base(syntax)
        {

        }
    }
}