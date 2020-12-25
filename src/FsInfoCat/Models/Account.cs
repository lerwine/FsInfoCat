using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class Account : AppUser
    {
        public const int Encoded_Pw_Hash_Length = 96;
        private string _pwHash = "";

        [Required()]
        [MinLength(Encoded_Pw_Hash_Length)]
        [MaxLength(Encoded_Pw_Hash_Length)]
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

        [Display(Name = "Is Inactive")]
        public bool IsInactive => Role == UserRole.None;

        public Account() { }

        public Account(string userName, string pwHash, UserRole role, Guid createdBy) : base(userName, role, createdBy)
        {
            PwHash = pwHash;
        }
    }
}
