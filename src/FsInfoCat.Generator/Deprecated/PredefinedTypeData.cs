using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class PredefinedTypeData : TypeData
    {
        public const string RootElementName = "Predefined";

        [XmlAttribute]
        public string Keyword { get; set; }

        public PredefinedTypeData() { }

        public PredefinedTypeData(PredefinedTypeSyntax syntax) : base(syntax)
        {
            Keyword = syntax.Keyword.ValueText;
        }
    }
}
