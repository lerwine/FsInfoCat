using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class ArrayTypeData : TypeData
    {
        public const string RootElementName = "Array";

        public int[] RankSpecifier { get; set; }

        public TypeData ElementType { get; set; }

        public ArrayTypeData() { }

        public ArrayTypeData(ArrayTypeSyntax syntax) : base(syntax)
        {
            ElementType = CreateTypeData(syntax.ElementType);
            RankSpecifier = syntax.RankSpecifiers.Select(r => r.Rank).ToArray();
        }
    }
}
