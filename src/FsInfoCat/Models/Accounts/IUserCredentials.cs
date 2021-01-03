using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models.Accounts
{
    public interface IUserCredentials : ILogin
    {
        /// <summary>
        /// Gets or set's the user's raw password.
        /// /// </summary>
        string Password { get; set; }
    }
}
