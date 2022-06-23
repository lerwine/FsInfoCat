using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class RefTypeData : TypeData
    {
        public const string RootElementName = "Ref";

        public bool ReadOnly { get; set; }

        public TypeData Type { get; set; }

        public RefTypeData() { }

        public RefTypeData(RefTypeSyntax syntax) : base(syntax)
        {
            ReadOnly = !string.IsNullOrWhiteSpace(syntax.ReadOnlyKeyword.ValueText);
            Type = TypeData.CreateTypeData(syntax.Type);
        }
    }
}
