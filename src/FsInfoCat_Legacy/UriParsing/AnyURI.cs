namespace FsInfoCat.UriParsing
{
    public class AnyURI : IAnyURI
    {
        public AnyURIBase AbsoluteBase { get; }

        public PathSegmentList<PathSegment> Segments { get; }

        public QueryParameterComponentList<QueryParameter> Query { get; }

        public string Fragment { get; }

        public bool IsWellFormed { get; }

        IAbsoluteURIBase IAnyURI.AbsoluteBase => AbsoluteBase;

        IPathSegmentList<IUriPathSegment> IAnyURI.Segments => Segments?.GetGenericWraper();

        IUriComponentList<IUriParameterElement> IAnyURI.Query => Query?.GetGenericWraper();
    }
}
