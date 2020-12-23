using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class RegisteredUser
    {
        public const int Max_Length_Login_Name = 32;
        private string _createdBy = "";
        private string _modifiedBy = "";
        private string _displayName = "";
        private string _loginName = "";
        private string _pwHash = "";
        private string _role = "";
        private string _notes = "";

        [MinLength(32)]
        [MaxLength(32)]
        [Required()]
        [Key()]
        [RegularExpression(@"^[\da-f]{32}$")]
        [Display(Name = "ID")]
        public Guid UserID { get; set; }

        [Required()]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Required()]
        [MinLength(1)]
        [MaxLength(Max_Length_Login_Name)]
        [Display(Name = "Created By")]
        [DataType(DataType.Text)]
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Required()]
        [MinLength(1)]
        [MaxLength(Max_Length_Login_Name)]
        [Display(Name = "Modified By")]
        [DataType(DataType.Text)]
        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = (null == value) ? "" : value; }
        }

        [MaxLength(256)]
        [Display(Name = "Display Name")]
        [DataType(DataType.Text)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        [Required()]
        [MinLength(1)]
        [MaxLength(Max_Length_Login_Name)]
        [Display(Name = "Login Name")]
        [DataType(DataType.Text)]
        /// <summary>
        /// Gets the user's login name.
        /// /// </summary>
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (null == value) ? "" : value; }
        }

        [Required()]
        [MinLength(80)]
        [MaxLength(80)]
        [Display(Name = "Password Hash")]
        [DataType(DataType.Text)]
        /// <summary>
        /// Gets the hash for the user's password.
        /// </summary>
        public string PwHash
        {
            get { return _pwHash; }
            set { _pwHash = (null == value) ? "" : value; }
        }

        [Required()]
        [MinLength(1)]
        [MaxLength(128)]
        [Display(Name = "User Role")]
        [DataType(DataType.Text)]
        /// <summary>
        /// Gets the role of the user.
        /// </summary>
        public string Role
        {
            get { return _role; }
            set { _role = (null == value) ? "" : value; }
        }

        [Display(Name = "Is Inactive")]
        public bool IsInactive { get; set; }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }
    }
}
