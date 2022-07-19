using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    public abstract class BasePropertyDeclaration : MemberDeclaration
    {
        protected BasePropertyDeclaration() { }

        protected BasePropertyDeclaration(BasePropertyDeclarationSyntax syntax) : base(syntax) { }
    }
}
