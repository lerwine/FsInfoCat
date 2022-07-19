using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class PropertyDeclaration : BasePropertyDeclaration
    {
        public const string RootElementName = "Property";

        [XmlAttribute()]
        public string Name { get; set; }

        public PropertyDeclaration() { }

        public PropertyDeclaration(PropertyDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
        }
    }
}
