using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class ConversionOperatorDeclaration : BaseMethodDeclaration
    {
        public const string RootElementName = "ConversionOperator";

        public ConversionOperatorDeclaration() { }

        public ConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) : base(syntax) { }
    }
}
