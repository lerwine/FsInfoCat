using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Represents a C# source code file
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis"/>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxtree?view=roslyn-dotnet-4.1.0"/>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.csharpsyntaxtree?view=roslyn-dotnet-4.1.0"/>
    public class SourceCodeFile : PSSyntaxBase
    {
        public string Path { get; }

        internal CSharpSyntaxTree SyntaxTree { get; }

        internal override CSharpSyntaxNode BaseSyntax { get; }

        internal SourceCodeFile([DisallowNull] CSharpSyntaxTree syntaxTree, [DisallowNull] string path)
        {
            SyntaxTree = syntaxTree;
            BaseSyntax = syntaxTree.GetCompilationUnitRoot();
            Path = path;
        }
    }
}
