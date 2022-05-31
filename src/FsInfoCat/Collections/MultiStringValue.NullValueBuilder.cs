namespace FsInfoCat.Collections
{
    public partial class MultiStringValue
    {
        private class NullValueBuilder : IValueBuilder
        {
            public IValueBuilder Append(string text) => (text is null) ? this : new StringValueBuilder(text);

            public IValueBuilder Append(char c) => new CharValueBuilder(c);

            public bool TryGetValue(out string text) { text = null; return false; }

            public string GetValue() => "";
        }
    }
}
