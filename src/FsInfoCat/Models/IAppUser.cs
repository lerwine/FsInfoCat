using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IAppUser : ILogin, IModficationAuditable
    {
        [Required()]
        [Key()]
        [Display(Name = "ID")]
        Guid AccountID { get; set; }

        [MaxLength(AppUser.Max_Length_DisplayName, ErrorMessage = AppUser.Error_Message_DisplayName)]
        [Display(Name = AppUser.DisplayName_DisplayName)]
        [DataType(DataType.Text)]
        string DisplayName { get; set; }

        [Required()]
        [Display(Name = "User Role")]
        [EnumDataType(typeof(UserRole))]
        /// <summary>
        /// Gets the role of the user.
        /// </summary>
        UserRole Role { get; set; }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        string Notes { get; set; }
    }
}
