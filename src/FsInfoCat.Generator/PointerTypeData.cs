using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class PointerTypeData : TypeData
    {
        public const string RootElementName = "Pointer";

        public TypeData ElementType { get; set; }

        public PointerTypeData() { }

        public PointerTypeData(PointerTypeSyntax syntax) : base(syntax)
        {
            ElementType = CreateTypeData(syntax);
        }
    }
}
