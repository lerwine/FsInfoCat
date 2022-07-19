using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    public abstract class BaseMethodDeclaration : MemberDeclaration
    {
        protected BaseMethodDeclaration() { }

        protected BaseMethodDeclaration(BaseMethodDeclarationSyntax syntax) : base(syntax) { }
    }
}
