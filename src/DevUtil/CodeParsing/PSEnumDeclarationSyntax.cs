using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for enum declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.enumdeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSEnumDeclarationSyntax : PSBaseTypeDeclarationSyntax
    {
        internal EnumDeclarationSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal override BaseTypeDeclarationSyntax BaseTypeSyntax => Syntax;

        internal override MemberDeclarationSyntax BaseMemberSyntax => Syntax;

        internal PSEnumDeclarationSyntax([DisallowNull] EnumDeclarationSyntax syntax, [DisallowNull] PSSyntaxBase parent)
            : base(parent) => Syntax = syntax;
    }
}
