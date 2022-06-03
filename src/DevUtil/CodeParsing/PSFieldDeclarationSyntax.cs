using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for field declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.fielddeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSFieldDeclarationSyntax : PSBaseFieldDeclarationSyntax
    {
        internal FieldDeclarationSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal override MemberDeclarationSyntax BaseMemberSyntax => Syntax;

        internal override BaseFieldDeclarationSyntax BaseFieldSyntax => Syntax;

        internal PSFieldDeclarationSyntax([DisallowNull] FieldDeclarationSyntax syntax, [DisallowNull] PSSyntaxBase parent)
            : base(parent) => Syntax = syntax;
    }
}
