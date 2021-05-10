namespace FsInfoCat.UriParsing
{
    public class AnyURIBase : IAbsoluteURIBase
    {
        public string Scheme { get; }

        public string Delimiter { get; }

        public Authority Authority { get; }

        public bool IsWellFormed { get; }

        IAuthority IAbsoluteURIBase.Authority => Authority;
    }
}
