namespace FsInfoCat.Collections
{
    public partial class MultiStringValue
    {
        interface IValueBuilder
        {
            IValueBuilder Append(string text);
            IValueBuilder Append(char c);
            bool TryGetValue(out string text);
            string GetValue();
        }
    }
}
