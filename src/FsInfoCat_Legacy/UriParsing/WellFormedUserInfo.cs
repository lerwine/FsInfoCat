using System.Text.RegularExpressions;

namespace FsInfoCat.UriParsing
{
    public class WellFormedUserInfo : IUserInfo
    {
        // TODO: Get real expression
        public static readonly Regex UserNameRegex = new Regex(@"^[a-z][a-z\d-]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // TODO: Get real expression
        public static readonly Regex PasswordRegex = new Regex(@"^[a-z][a-z\d-]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private WellFormedUserInfo(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; }

        public string Password { get; }

        bool IUriComponent.IsWellFormed => true;

        public static bool TryCreate(string userName, string password, out WellFormedUserInfo userInfo)
        {
            if (userName is null)
            {
                if (password is null)
                    userInfo = null;
                else if (password.Length == 0 || PasswordRegex.IsMatch(password))
                    userInfo = new WellFormedUserInfo("", password);
                else
                {
                    userInfo = null;
                    return false;
                }
            }
            else if ((userName.Length == 0 || UserNameRegex.IsMatch(userName)) && (string.IsNullOrEmpty(password) || PasswordRegex.IsMatch(password)))
                userInfo = new WellFormedUserInfo(userName, password);
            else
            {
                userInfo = null;
                return false;
            }
            return true;
        }
    }
}
