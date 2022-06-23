using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
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

        protected internal override string GetName() => Name;
    }
}
