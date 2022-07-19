using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    public abstract class TypeData : SyntaxEntity
    {
        [XmlArray()]
        public bool IsVar { get; set; }
        [XmlArray()]
        public bool IsUnmanaged { get; set; }

        [XmlArray()]
        public bool IsNotNull { get; set; }

        [XmlArray()]
        public bool IsNint { get; set; }

        [XmlArray()]
        public bool IsNuint { get; set; }

        protected TypeData() { }

        protected TypeData(TypeSyntax syntax) : base(syntax)
        {
            IsNint = syntax.IsNint;
            IsNotNull = syntax.IsNotNull;
            IsNuint = syntax.IsNuint;
            IsUnmanaged = syntax.IsUnmanaged;
            IsVar = syntax.IsUnmanaged;
        }

        public static TypeData CreateTypeData(TypeSyntax syntax)
        {
            if (syntax is ArrayTypeSyntax arrayType) return new ArrayTypeData(arrayType);
            if (syntax is FunctionPointerTypeSyntax functionPointerType) return new FunctionPointerTypeData(functionPointerType);
            if (syntax is NameSyntax name) return TypeNameData.CreateTypeNameData(name);
            if (syntax is NullableTypeSyntax nullableType) return new NullableTypeData(nullableType);
            if (syntax is OmittedTypeArgumentSyntax omittedTypeArgument) return new OmittedTypeArgument(omittedTypeArgument);
            if (syntax is PointerTypeSyntax pointerType) return new PointerTypeData(pointerType);
            if (syntax is PredefinedTypeSyntax predefinedType) return new PredefinedTypeData(predefinedType);
            if (syntax is RefTypeSyntax refType) return new RefTypeData(refType);
            if (syntax is TupleTypeSyntax tupleType) return new TupleTypeData(tupleType);
            return new UnsupportedType(syntax);
        }
    }
}
