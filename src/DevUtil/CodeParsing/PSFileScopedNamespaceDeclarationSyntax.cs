using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for file-scoped namespace declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.filescopednamespacedeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSFileScopedNamespaceDeclarationSyntax : PSBaseNamespaceDeclarationSyntax
    {
        internal FileScopedNamespaceDeclarationSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal override BaseNamespaceDeclarationSyntax BaseNamespaceSyntax => Syntax;

        internal override MemberDeclarationSyntax BaseMemberSyntax => Syntax;

        internal PSFileScopedNamespaceDeclarationSyntax([DisallowNull] FileScopedNamespaceDeclarationSyntax syntax, [DisallowNull] PSSyntaxBase parent)
            : base(parent) => Syntax = syntax;
    }
}
