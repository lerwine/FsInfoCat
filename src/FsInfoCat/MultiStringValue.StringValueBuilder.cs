using System.Text;

namespace FsInfoCat
{
    public partial class MultiStringValue
    {
        private class StringValueBuilder : IValueBuilder
        {
            private readonly string _value;
            internal StringValueBuilder(string text) { _value = text ?? ""; }
            public IValueBuilder Append(string text) => string.IsNullOrEmpty(text) ? this : new AppendedValueBuilder(new StringBuilder(_value).Append(text));
            public IValueBuilder Append(char c) => new AppendedValueBuilder(new StringBuilder(_value).Append(c));
            public bool TryGetValue(out string text) { text = _value; return true; }
            public string GetValue() => _value;
        }
    }
}
