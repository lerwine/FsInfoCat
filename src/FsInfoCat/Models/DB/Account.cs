using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models.DB
{
    [DisplayColumn("AccountID", "DisplayName", false)]
    public class Account : AppUser
    {
        public const int Encoded_Pw_Hash_Length = 96;
        public const string DisplayName_PwHash = "Password hash";
        public const string PropertyName_PwHash = "PwHash";
        public const string Error_Message_PwHash_Empty = "Password hash cannot be empty.";
        public const string Error_Message_PwHash_Short = "Password hash too short.";
        public const string Error_Message_PwHash_Long = "Password hash too long.";
        public const string Error_Message_PwHash_Invalid = "Password hash invalid.";
        private string _pwHash = "";

        [Required(ErrorMessage = Error_Message_PwHash_Empty)]
        [MinLength(Encoded_Pw_Hash_Length, ErrorMessage = Error_Message_PwHash_Short)]
        [MaxLength(Encoded_Pw_Hash_Length, ErrorMessage = Error_Message_PwHash_Long)]
        [RegularExpression(ModelHelper.PATTERN_BASE64, ErrorMessage = Error_Message_PwHash_Invalid)]
        [Display(Name = DisplayName_PwHash)]
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

        protected override void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case PropertyName_PwHash:
                case DisplayName_PwHash:
                    if (_pwHash.Length == 0)
                        result.Add(new ValidationResult(Error_Message_PwHash_Empty, new string[] { PropertyName_PwHash }));
                    else if (_pwHash.Length > Encoded_Pw_Hash_Length)
                        result.Add(new ValidationResult(Error_Message_PwHash_Long, new string[] { PropertyName_PwHash }));
                    else if (_pwHash.Length < Encoded_Pw_Hash_Length)
                        result.Add(new ValidationResult(Error_Message_PwHash_Short, new string[] { PropertyName_PwHash }));
                    else if (!ModelHelper.Base64Regex.IsMatch(_pwHash))
                        result.Add(new ValidationResult(Error_Message_PwHash_Invalid, new string[] { PropertyName_PwHash }));
                    break;
                default:
                    base.Validate(result, propertyName);
                    break;
            }
        }

        protected override void OnValidateAll(List<ValidationResult> result)
        {
            base.OnValidateAll(result);
            Validate(result, PropertyName_PwHash);
        }

    }
}
