using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for external alias directive declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.externaliasdirectivesyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSExternAliasDirectiveSyntax : PSSyntaxBase
    {
        public PSBaseNamespaceDeclarationSyntax Parent { get; }

        internal ExternAliasDirectiveSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal PSExternAliasDirectiveSyntax([DisallowNull] ExternAliasDirectiveSyntax syntax, [DisallowNull] PSBaseNamespaceDeclarationSyntax parent)
        {
            Syntax = syntax;
            Parent = parent;
        }
    }
}
