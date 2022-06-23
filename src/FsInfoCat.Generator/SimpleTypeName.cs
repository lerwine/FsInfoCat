using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public abstract class SimpleTypeName : TypeNameData
    {
        [XmlAttribute]
        public string Name { get; set; }

        protected SimpleTypeName() { }

        protected SimpleTypeName(SimpleNameSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
        }

        public static SimpleTypeName CreateSimpleNameTypeData(SimpleNameSyntax syntax)
        {
            if (syntax is GenericNameSyntax genericName) return new GenericTypeName(genericName);
            if (syntax is IdentifierNameSyntax identifierName) return new IdentifierName(identifierName);
            return new UnsupportedSimpleTypeName(syntax);
        }
    }
}
