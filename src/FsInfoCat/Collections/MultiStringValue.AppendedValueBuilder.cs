using System.Text;

namespace FsInfoCat.Collections
{
    // TODO: Document MultiStringValue.MultiStringValue class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial class MultiStringValue
    {
        private class AppendedValueBuilder : IValueBuilder
        {
            private readonly StringBuilder _stringBuilder;

            internal AppendedValueBuilder(StringBuilder stringBuilder) { _stringBuilder = stringBuilder; }

            public IValueBuilder Append(string text)
            {
                if (!string.IsNullOrEmpty(text))
                    _ = _stringBuilder.Append(text);
                return this;
            }

            public IValueBuilder Append(char c)
            {
                _ = _stringBuilder.Append(c);
                return this;
            }

            public bool TryGetValue(out string text) { text = GetValue(); return true; }

            public string GetValue() => _stringBuilder.ToString();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
