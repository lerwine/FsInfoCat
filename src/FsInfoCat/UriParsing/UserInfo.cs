namespace FsInfoCat.UriParsing
{
    public class UserInfo : IUserInfo
    {
        public string UserName { get; }

        public string Password { get; }

        public bool IsWellFormed { get; }
    }
}
