namespace FsInfoCat.UriParsing
{
    public interface IPathSegmentList<TSegment> : IUriComponentList<TSegment>
        where TSegment : class, IUriPathSegment
    {
        bool IsRooted { get; }
    }
}
