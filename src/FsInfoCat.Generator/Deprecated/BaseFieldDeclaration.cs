using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    public abstract class BaseFieldDeclaration : MemberDeclaration
    {
        protected BaseFieldDeclaration() { }

        protected BaseFieldDeclaration(BaseFieldDeclarationSyntax syntax) : base(syntax) { }
    }
}
