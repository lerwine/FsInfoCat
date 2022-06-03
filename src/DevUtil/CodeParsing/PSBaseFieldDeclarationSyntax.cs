using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Base class for field declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.basefielddeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public abstract class PSBaseFieldDeclarationSyntax : PSMemberDeclarationSyntax
    {
        internal abstract BaseFieldDeclarationSyntax BaseFieldSyntax { get; }

        protected PSBaseFieldDeclarationSyntax([DisallowNull] PSSyntaxBase parent) : base(parent) { }
    }
}
