using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface ILogin : IValidatableModel
    {
        [Required()]
        [MinLength(1, ErrorMessage = AppUser.Error_Message_Login_Empty)]
        [MaxLength(AppUser.Max_Length_Login_Name, ErrorMessage = AppUser.Error_Message_Login_Length)]
        [RegularExpression(ModelHelper.PATTERN_DOTTED_NAME, ErrorMessage = AppUser.Error_Message_Login_Invalid)]
        [Display(Name = AppUser.DisplayName_LoginName)]
        /// <summary>
        /// Gets the user's login name.
        /// </summary>
        string LoginName { get; set; }
    }
}
