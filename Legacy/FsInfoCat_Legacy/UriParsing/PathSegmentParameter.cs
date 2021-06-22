namespace FsInfoCat.UriParsing
{
    public class PathSegmentParameter : IUriParameterElement
    {
        public string Key { get; }

        public string Value { get; }

        public bool IsWellFormed { get; }
    }
}
