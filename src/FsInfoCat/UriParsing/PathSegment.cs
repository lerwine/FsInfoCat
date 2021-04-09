namespace FsInfoCat.UriParsing
{
    public class PathSegment : IUriPathSegment
    {
        public char? Delimiter { get; }

        public string Name { get; }

        public SegmentParameterComponentList<PathSegmentParameter> Parameters { get; }

        public AnyURI Parent { get; }

        public bool IsWellFormed { get; }

        IAnyURI IUriPathSegment.Parent => Parent;

        IUriComponentList<IUriParameterElement> IUriPathSegment.Parameters => Parameters?.GetGenericWraper();
    }
}
