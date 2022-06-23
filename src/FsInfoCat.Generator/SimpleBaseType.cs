using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class SimpleBaseType : BaseTypeData
    {
        public const string RootElementName = "BaseType";

        public SimpleBaseType() { }

        public SimpleBaseType(SimpleBaseTypeSyntax syntax) : base(syntax) { }
    }
}
