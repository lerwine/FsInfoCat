using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class GenericTypeName : SimpleTypeName
    {
        public const string RootElementName = "GenericName";

        [XmlAttribute]
        public bool IsUnboundGenericName { get; set; }

        public TypeData[] TypeArguments { get; set; }

        public GenericTypeName() { }

        public GenericTypeName(GenericNameSyntax syntax) : base(syntax)
        {
            IsUnboundGenericName = syntax.IsUnboundGenericName;
            TypeArguments = syntax.TypeArgumentList.Arguments.Select(t => CreateTypeData(t)).ToArray();
        }
    }
}
