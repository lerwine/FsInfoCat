using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class AliasQualifiedTypeName : TypeNameData
    {
        public const string RootElementName = "AliasQualifiedName";

        [XmlAttribute()]
        public string Name { get; set; }

        public IdentifierName Alias { get; set; }

        public AliasQualifiedTypeName() { }

        public AliasQualifiedTypeName(AliasQualifiedNameSyntax syntax) : base(syntax)
        {
            Alias = new IdentifierName(syntax.Alias);
            Name = syntax.Name.GetText().ToString();
        }
    }
}
