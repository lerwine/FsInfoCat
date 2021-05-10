namespace FsInfoCat.UriParsing
{
    public class FileUriSegment : IUriPathSegment
    {
        public bool HasDelimiter { get; }

        public FileURI Parent { get; }

        public string Name { get; }

        IUriComponentList<IUriParameterElement> IUriPathSegment.Parameters => null;

        char? IUriPathSegment.Delimiter => HasDelimiter ? (char?)'/' : null;

        IAnyURI IUriPathSegment.Parent => Parent;

        bool IUriComponent.IsWellFormed => true;
    }
}
