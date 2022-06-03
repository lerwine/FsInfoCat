using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for event field declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.eventfielddeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSEventFieldDeclarationSyntax : PSBaseFieldDeclarationSyntax
    {
        internal EventFieldDeclarationSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal override MemberDeclarationSyntax BaseMemberSyntax => Syntax;

        internal override BaseFieldDeclarationSyntax BaseFieldSyntax => Syntax;

        internal PSEventFieldDeclarationSyntax([DisallowNull] EventFieldDeclarationSyntax syntax, [DisallowNull] PSSyntaxBase parent)
            : base(parent) => Syntax = syntax;
    }
}
