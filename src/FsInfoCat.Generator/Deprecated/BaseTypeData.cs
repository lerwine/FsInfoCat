using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [Obsolete("Soon to be removed")]
    public abstract class BaseTypeData : SyntaxEntity
    {
        protected BaseTypeData() { }

        protected BaseTypeData(BaseTypeSyntax syntax) : base(syntax) { }

        public static BaseTypeData CreateBaseTypeData(BaseTypeSyntax syntax)
        {
            if (syntax is PrimaryConstructorBaseTypeSyntax primaryConstructorBase) return new PrimaryConstructorBaseType(primaryConstructorBase);
            if (syntax is SimpleBaseTypeSyntax simpleBaseType) return new SimpleBaseType(simpleBaseType);
            return new UnsupportedBaseType(syntax);
        }
    }
}
