using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Base class for type declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.basetypedeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public abstract class PSBaseTypeDeclarationSyntax : PSMemberDeclarationSyntax
    {
        internal abstract BaseTypeDeclarationSyntax BaseTypeSyntax { get; }

        protected PSBaseTypeDeclarationSyntax([DisallowNull] PSSyntaxBase parent) : base(parent) { }
    }
}
