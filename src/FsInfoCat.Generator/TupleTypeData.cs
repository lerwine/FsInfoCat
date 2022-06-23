using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class TupleTypeData : TypeData
    {
        public const string RootElementName = "Tuple";

        public TupleElement[] Elements { get; set; }

        public TupleTypeData() { }

        public TupleTypeData(TupleTypeSyntax syntax) : base(syntax)
        {
            Elements = syntax.Elements.Select(e => TupleElement.Create(e)).ToArray();
        }
    }
}
