namespace FsInfoCat.UriParsing
{
    public class FileURI : IAnyURI
    {
        private FileURI(FileURIBase absoluteBase, PathSegmentList<FileUriSegment> segments)
        {
            AbsoluteBase = absoluteBase;
            Segments = segments;
        }

        public FileURIBase AbsoluteBase { get; }

        public PathSegmentList<FileUriSegment> Segments { get; }

        IUriComponentList<IUriParameterElement> IAnyURI.Query => null;

        IAbsoluteURIBase IAnyURI.AbsoluteBase => AbsoluteBase;

        IPathSegmentList<IUriPathSegment> IAnyURI.Segments => Segments?.GetGenericWraper();

        string IAnyURI.Fragment => null;

        bool IUriComponent.IsWellFormed => true;
    }
}
