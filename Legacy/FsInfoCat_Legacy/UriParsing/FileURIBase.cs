namespace FsInfoCat.UriParsing
{
    public class FileURIBase : IAbsoluteURIBase
    {
        public string Scheme { get; }

        public FileUriHost Host { get; }

        bool IUriComponent.IsWellFormed => true;

        IAuthority IAbsoluteURIBase.Authority => Host;

        string IAbsoluteURIBase.Delimiter => "://";
    }
}
