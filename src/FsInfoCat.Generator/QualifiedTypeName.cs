using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class QualifiedTypeName : TypeNameData
    {
        public const string RootElementName = "QualifiedName";

        public SimpleTypeName Right { get; set; }

        public TypeNameData Left { get; set; }

        public QualifiedTypeName() { }

        public QualifiedTypeName(QualifiedNameSyntax syntax) : base(syntax)
        {
            Right = SimpleTypeName.CreateSimpleNameTypeData(syntax.Right);
            Left = CreateTypeNameData(syntax.Left);
        }
    }
}
