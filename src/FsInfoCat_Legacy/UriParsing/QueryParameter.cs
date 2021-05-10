namespace FsInfoCat.UriParsing
{
    public class QueryParameter : IUriParameterElement
    {
        public string Key { get; }

        public string Value { get; }

        public bool IsWellFormed { get; }
    }
}
