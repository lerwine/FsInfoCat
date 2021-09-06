using System.Text;

namespace FsInfoCat.Collections
{
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
}
