using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class EventFieldDeclaration : MemberDeclaration
    {
        public const string RootElementName = "EventField";

        [XmlAttribute()]
        public string Name { get; set; }

        protected internal override string GetName() => Name;

        public EventFieldDeclaration() { }

        public EventFieldDeclaration(EventFieldDeclarationSyntax syntax) : base(syntax)
        {
        }
    }
}
