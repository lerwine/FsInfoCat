using System.Diagnostics.CodeAnalysis;

namespace DevUtil.Wrappers
{
    public class SyntaxTree : ISyntaxTree
    {
        internal Microsoft.CodeAnalysis.SyntaxTree BaseObject { get; }

        internal Microsoft.CodeAnalysis.SyntaxNode CompilationUnitRoot { get; }

        public string FilePath => BaseObject.FilePath;

        public int Length => BaseObject.Length;

        public bool HasLeadingTrivia => CompilationUnitRoot?.HasLeadingTrivia ?? false;

        public bool HasStructuredTrivia => CompilationUnitRoot?.HasStructuredTrivia ?? false;

        public bool HasTrailingTrivia => CompilationUnitRoot?.HasTrailingTrivia ?? false;

        internal SyntaxTree([DisallowNull] Microsoft.CodeAnalysis.SyntaxTree baseObject) => CompilationUnitRoot = (BaseObject = baseObject).HasCompilationUnitRoot ? baseObject.GetRoot() : null;

        internal TaskJob GetTextAsync() => TaskJob.Create(token => BaseObject.GetTextAsync(token), intermediate => intermediate?.ToString());
    }
}
