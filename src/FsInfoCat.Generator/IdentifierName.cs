using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class IdentifierName : SimpleTypeName
    {
        public const string RootElementName = "Identifier";

        public IdentifierName() { }

        public IdentifierName(IdentifierNameSyntax syntax) : base(syntax) { }
    }
}
