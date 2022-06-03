using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for record declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.recorddeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSRecordDeclarationSyntax : PSTypeDeclarationSyntax
    {
        internal RecordDeclarationSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal override TypeDeclarationSyntax TypeSyntax => Syntax;

        internal override BaseTypeDeclarationSyntax BaseTypeSyntax => Syntax;

        internal override MemberDeclarationSyntax BaseMemberSyntax => Syntax;

        internal PSRecordDeclarationSyntax([DisallowNull] RecordDeclarationSyntax syntax, [DisallowNull] PSSyntaxBase parent)
            : base(parent) => Syntax = syntax;
    }
}
