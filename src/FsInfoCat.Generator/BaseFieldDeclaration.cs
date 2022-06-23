using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public abstract class BaseFieldDeclaration : MemberDeclaration
    {
        protected BaseFieldDeclaration() { }

        protected BaseFieldDeclaration(BaseFieldDeclarationSyntax syntax) : base(syntax) { }
    }
}
