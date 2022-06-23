using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class NullableTypeData : TypeData
    {
        public const string RootElementName = "Nullable";

        public TypeData ElementType { get; set; }

        public NullableTypeData() { }

        public NullableTypeData(NullableTypeSyntax syntax) : base(syntax)
        {
            ElementType = CreateTypeData(syntax);
        }
    }
}
