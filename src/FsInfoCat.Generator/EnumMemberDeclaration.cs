using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public class EnumMemberDeclaration : ModelCollection.Component
    {
        public const string RootElementName = "Member";

        private readonly object _syncRoot = new object();

        protected internal override string GetName() => Name;

        [XmlAttribute()]
        public string Name { get; set; }

        internal static EnumMemberDeclaration Create(EnumMemberDeclarationSyntax syntax)
        {
            EnumMemberDeclaration result = new EnumMemberDeclaration()
            {
                Name = syntax.Identifier.ValueText
            };
            return result;
        }
    }
}
