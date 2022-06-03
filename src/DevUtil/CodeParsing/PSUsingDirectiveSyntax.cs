using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for using directive declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.usingdirectivesyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSUsingDirectiveSyntax : PSSyntaxBase
    {
        public PSBaseNamespaceDeclarationSyntax Parent { get; }

        internal UsingDirectiveSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal PSUsingDirectiveSyntax([DisallowNull] UsingDirectiveSyntax syntax, [DisallowNull] PSBaseNamespaceDeclarationSyntax parent)
        {
            Syntax = syntax;
            Parent = parent;
        }
    }
}
