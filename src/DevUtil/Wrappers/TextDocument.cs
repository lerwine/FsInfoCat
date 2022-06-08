using System.Diagnostics.CodeAnalysis;

namespace DevUtil.Wrappers
{
    public class TextDocument : ITextDocument
    {
        internal Microsoft.CodeAnalysis.TextDocument BaseObject { get; }

        internal TextDocument([DisallowNull] Microsoft.CodeAnalysis.TextDocument baseObject) => BaseObject = baseObject;

        public TaskJob GetTextAsync() => TaskJob.Create(token => BaseObject.GetTextAsync(token), intermediate => intermediate?.ToString());
    }
}
