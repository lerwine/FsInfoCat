namespace FsInfoCat.UriParsing
{
    public interface IUserInfo : IUriComponent
    {
        string UserName { get; }
        string Password { get; }
    }
}
