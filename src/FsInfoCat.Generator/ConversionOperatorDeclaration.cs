using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class ConversionOperatorDeclaration : BaseMethodDeclaration
    {
        public const string RootElementName = "ConversionOperator";

        protected internal override string GetName() => null;

        public ConversionOperatorDeclaration() { }

        public ConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax syntax) : base(syntax) { }
    }
}
