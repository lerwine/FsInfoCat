using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public abstract class BaseMethodDeclaration : MemberDeclaration
    {
        protected BaseMethodDeclaration() { }

        protected BaseMethodDeclaration(BaseMethodDeclarationSyntax syntax) : base(syntax) { }
    }
}
