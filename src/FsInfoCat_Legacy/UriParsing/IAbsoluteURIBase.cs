namespace FsInfoCat.UriParsing
{
    public interface IAbsoluteURIBase : IUriComponent
    {
        string Scheme { get; }
        string Delimiter { get; }
        IAuthority Authority { get; }
    }
}
