using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Base class for member declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.memberdeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public abstract class PSMemberDeclarationSyntax : PSSyntaxBase
    {
        internal abstract MemberDeclarationSyntax BaseMemberSyntax { get; }

        public PSSyntaxBase Parent { get; }

        protected PSMemberDeclarationSyntax([DisallowNull] PSSyntaxBase parent) => Parent = parent;
    }
}
