using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class RegisteredUser
    {
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

        private string _createdBy = "";
        [Required()]
        [MinLength(1)]
        [MaxLength(256)]
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

        private string _modifiedBy = "";
        [Required()]
        [MinLength(1)]
        [MaxLength(256)]
        [Display(Name = "Modified By")]
        [DataType(DataType.Text)]
        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = (null == value) ? "" : value; }
        }

        private string _loginName = "";
        [Required()]
        [MinLength(1)]
        [MaxLength(32)]
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

        private string _pwHash = "";
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

        private string _role = "";
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

        private string _notes = "";
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
