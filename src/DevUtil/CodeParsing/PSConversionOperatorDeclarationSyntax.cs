using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for constructor declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.conversionoperatordeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSConversionOperatorDeclarationSyntax : PSBaseMethodDeclarationSyntax
    {
        internal ConversionOperatorDeclarationSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal override BaseMethodDeclarationSyntax BaseMethodSyntax => Syntax;

        internal override MemberDeclarationSyntax BaseMemberSyntax => Syntax;

        internal PSConversionOperatorDeclarationSyntax([DisallowNull] ConversionOperatorDeclarationSyntax syntax, [DisallowNull] PSSyntaxBase parent)
            : base(parent) => Syntax = syntax;
    }
}
