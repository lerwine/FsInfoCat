using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class FunctionPointerTypeData : TypeData
    {
        public const string RootElementName = "FunctionPointer";

        public TypeData[] Parameters { get; set; }

        public FunctionPointerTypeData() { }

        public FunctionPointerTypeData(FunctionPointerTypeSyntax syntax) : base(syntax)
        {
            Parameters = syntax.ParameterList.Parameters.Select(p => CreateTypeData(p.Type)).ToArray();
        }
    }
}
