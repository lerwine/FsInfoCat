using System;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class UsingDirective : SyntaxEntity
    {
        public const string RootElementName = "Using";

        [XmlAttribute()]
        public string Name { get; set; }

        public UsingDirective(UsingDirectiveSyntax syntax) : base(syntax)
        {
            Name = syntax.Name.GetText().ToString();
        }
    }
}
