using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class OperatorDeclaration : BaseMethodDeclaration
    {
        public const string RootElementName = "Operator";

        public OperatorDeclaration() { }

        public OperatorDeclaration(OperatorDeclarationSyntax syntax) : base(syntax) { }
    }
}
