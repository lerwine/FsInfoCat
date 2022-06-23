using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class EventPropertyDeclaration : BasePropertyDeclaration
    {
        public const string RootElementName = "EventProperty";

        [XmlAttribute()]
        public string Name { get; set; }

        public EventPropertyDeclaration() { }

        public EventPropertyDeclaration(EventDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
        }
    }
}
