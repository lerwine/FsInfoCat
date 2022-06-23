using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public abstract class TypeData
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

        public CommentDocumentation DocumentationComments { get; set; }

        public static IEnumerable<TypeData> CreateFromBaseList(BaseListSyntax syntax)
        {
            if (syntax != null)
                foreach (TypeSyntax type in syntax.Types.Select(t => t.Type))
                    yield return CreateTypeData(type);
        }

        protected TypeData() { }

        protected TypeData(TypeSyntax syntax)
        {
            IsNint = syntax.IsNint;
            IsNotNull = syntax.IsNotNull;
            IsNuint = syntax.IsNuint;
            IsUnmanaged = syntax.IsUnmanaged;
            IsVar = syntax.IsUnmanaged;
            DocumentationComments = CommentDocumentation.Create(syntax);
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
