using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Base class for method declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.basemethoddeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public abstract class PSBaseMethodDeclarationSyntax : PSMemberDeclarationSyntax
    {
        internal abstract BaseMethodDeclarationSyntax BaseMethodSyntax { get; }

        protected PSBaseMethodDeclarationSyntax([DisallowNull] PSSyntaxBase parent) : base(parent) { }
    }
}
