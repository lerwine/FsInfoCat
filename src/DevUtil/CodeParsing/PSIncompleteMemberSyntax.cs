using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for incomplete member declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.incompletemembersyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSIncompleteMemberSyntax : PSMemberDeclarationSyntax
    {
        internal IncompleteMemberSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal override MemberDeclarationSyntax BaseMemberSyntax => Syntax;

        internal PSIncompleteMemberSyntax([DisallowNull] IncompleteMemberSyntax syntax, [DisallowNull] PSSyntaxBase parent)
            : base(parent) => Syntax = syntax;
    }
}
