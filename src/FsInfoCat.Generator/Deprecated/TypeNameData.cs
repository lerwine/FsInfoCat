using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    public abstract class TypeNameData : TypeData
    {
        [XmlAttribute()]
        public int Arity { get; set; }

        protected TypeNameData() { }

        protected TypeNameData(NameSyntax syntax) : base(syntax)
        {
            Arity = syntax.Arity;
        }

        public static TypeNameData CreateTypeNameData(NameSyntax syntax)
        {
            if (syntax is AliasQualifiedNameSyntax aliasQualifiedName) return new AliasQualifiedTypeName(aliasQualifiedName);
            if (syntax is QualifiedNameSyntax qualifiedName) return new QualifiedTypeName(qualifiedName);
            if (syntax is SimpleNameSyntax simpleName) return SimpleTypeName.CreateSimpleNameTypeData(simpleName);
            return new UnsupportedTypeNameData(syntax);
        }
    }
}
