namespace FsInfoCat.UriParsing
{
    public interface IUriPathSegment : IUriComponent
    {
        char? Delimiter { get; }
        string Name { get; }
        IUriComponentList<IUriParameterElement> Parameters { get; }
        IAnyURI Parent { get; }
    }
}
