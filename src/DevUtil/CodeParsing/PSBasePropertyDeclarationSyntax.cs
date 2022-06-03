using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Base class for property declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.basepropertydeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public abstract class PSBasePropertyDeclarationSyntax : PSMemberDeclarationSyntax
    {
        internal abstract BasePropertyDeclarationSyntax BasePropertySyntax { get; }

        protected PSBasePropertyDeclarationSyntax([DisallowNull] PSSyntaxBase parent) : base(parent) { }
    }
}
