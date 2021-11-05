using System.Text;

namespace FsInfoCat.Collections
{
    public partial class MultiStringValue
    {
        private class CharValueBuilder : IValueBuilder
        {
            private readonly char _value;
            internal CharValueBuilder(char c) { _value = c; }
            public IValueBuilder Append(string text) => string.IsNullOrEmpty(text) ? this : new AppendedValueBuilder(new StringBuilder().Append(_value).Append(text));
            public IValueBuilder Append(char c) => new AppendedValueBuilder(new StringBuilder().Append(_value).Append(c));
            public bool TryGetValue(out string text) { text = GetValue(); return true; }
            public string GetValue() => new(new char[] { _value });
        }
    }
}
