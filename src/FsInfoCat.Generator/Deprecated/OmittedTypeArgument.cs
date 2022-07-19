using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class OmittedTypeArgument : TypeData
    {
        public const string RootElementName = "Omitted";

        public OmittedTypeArgument() { }

        public OmittedTypeArgument(OmittedTypeArgumentSyntax syntax) : base(syntax) { }
    }
}
