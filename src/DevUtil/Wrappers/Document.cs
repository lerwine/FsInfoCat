using CSharp = Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics.CodeAnalysis;

namespace DevUtil.Wrappers
{
    public class Document : ITextDocument
    {
        internal Microsoft.CodeAnalysis.Document BaseObject { get; }

        internal Document([DisallowNull] Microsoft.CodeAnalysis.Document baseObject) => BaseObject = baseObject;

        public TaskJob GetSyntaxTreeAsync() => TaskJob.Create<Microsoft.CodeAnalysis.SyntaxTree, ISyntaxTree>(token => BaseObject.GetSyntaxTreeAsync(token), intermediate =>
        {
            if (intermediate is null) return null;
            if (intermediate is CSharp.CSharpSyntaxTree syntaxTree) return new CsSyntaxTree(syntaxTree);
            return new SyntaxTree(intermediate);
        });

        public TaskJob GetTextAsync() => TaskJob.Create(token => BaseObject.GetTextAsync(token), intermediate => intermediate?.ToString());
    }
}
