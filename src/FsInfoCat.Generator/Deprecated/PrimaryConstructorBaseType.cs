using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class PrimaryConstructorBaseType : BaseTypeData
    {
        public const string RootElementName = "PrimaryConstructorBaseType";

        public PrimaryConstructorBaseType() { }

        public PrimaryConstructorBaseType(PrimaryConstructorBaseTypeSyntax syntax) : base(syntax)
        {

        }
    }
}
