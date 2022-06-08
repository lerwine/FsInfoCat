using CSharp = Microsoft.CodeAnalysis.CSharp;
using Syntax = Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.CodeAnalysis;

namespace DevUtil.Wrappers
{
    public class CsSyntaxTree : ISyntaxTree
    {
        internal CSharp.CSharpSyntaxTree BaseObject { get; }

        internal Syntax.CompilationUnitSyntax CompilationUnitRoot { get; }

        public string FilePath => BaseObject.FilePath;

        public int Length => BaseObject.Length;

        public bool HasLeadingTrivia => CompilationUnitRoot?.HasLeadingTrivia ?? false;

        public bool HasStructuredTrivia => CompilationUnitRoot?.HasStructuredTrivia ?? false;

        public bool HasTrailingTrivia => CompilationUnitRoot?.HasTrailingTrivia ?? false;

        internal CsSyntaxTree([DisallowNull] CSharp.CSharpSyntaxTree baseObject) => CompilationUnitRoot = (BaseObject = baseObject).HasCompilationUnitRoot ? baseObject.GetCompilationUnitRoot() : null;

        internal TaskJob GetTextAsync() => TaskJob.Create(token => BaseObject.GetTextAsync(token), intermediate => intermediate?.ToString());
    }
}
