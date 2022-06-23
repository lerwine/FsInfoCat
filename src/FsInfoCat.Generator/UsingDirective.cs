using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class UsingDirective : ModelCollection.Component
    {
        public const string RootElementName = "Using";

        protected internal override string GetName() => Name;

        [XmlAttribute()]
        public string Name { get; set; }

        internal static UsingDirective Create(UsingDirectiveSyntax syntax)
        {
            UsingDirective result = new UsingDirective()
            {
                Name = syntax.Name.GetText().ToString()
            };
            return result;
        }
    }
}
