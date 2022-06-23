using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class ClassDeclaration : TypeDeclaration
    {
        public const string RootElementName = "Class";

        public ClassDeclaration(ClassDeclarationSyntax syntax) : base(syntax) { }
    }
}
