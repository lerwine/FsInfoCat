using System;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class ClassDeclaration : TypeDeclaration
    {
        public const string RootElementName = "Class";

        public ClassDeclaration() { }

        public ClassDeclaration(ClassDeclarationSyntax syntax) : base(syntax) { }
    }
}
