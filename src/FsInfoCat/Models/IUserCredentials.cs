using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IUserCredentials : ILogin
    {
        [Required()]
        [Display(Name = UserLoginRequest.DisplayName_Password)]
        [RegularExpression(@"\S+", ErrorMessage = UserLoginRequest.Error_Message_Password)]
        /// <summary>
        /// Gets or set's the user's raw password.
        /// /// </summary>
        string Password { get; set; }
    }
}
