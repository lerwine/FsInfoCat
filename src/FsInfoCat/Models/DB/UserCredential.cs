using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Models.DB
{
    public class UserCredential : IModficationAuditable
    {
        public const int Encoded_Pw_Hash_Length = 96;
        public const string DisplayName_PwHash = "Password hash";
        public const string PropertyName_PwHash = "PwHash";
        public const string Error_Message_PwHash_Empty = "Password hash cannot be empty.";
        public const string Error_Message_PwHash_Short = "Password hash too short.";
        public const string Error_Message_PwHash_Long = "Password hash too long.";
        public const string Error_Message_PwHash_Invalid = "Password hash invalid.";
        private string _hashString = "";
        private PwHash? _pwHash = null;

        [Key()]
        [Required()]
        [Display(Name = "ID")]
        public Guid AccountID { get; set; }

        [Required(ErrorMessage = Error_Message_PwHash_Empty)]
        [MinLength(Encoded_Pw_Hash_Length, ErrorMessage = Error_Message_PwHash_Short)]
        [MaxLength(Encoded_Pw_Hash_Length, ErrorMessage = Error_Message_PwHash_Long)]
        [RegularExpression(ModelHelper.PATTERN_BASE64, ErrorMessage = Error_Message_PwHash_Invalid)]
        [Display(Name = DisplayName_PwHash)]
        [DataType(DataType.Text)]
        [Column("PwHash")]
        /// <summary>
        /// Gets the hash for the user's password.
        /// </summary>
        public string HashString
        {
            get
            {
                string h = _hashString;
                if (h is null)
                    _hashString = h = (_pwHash.HasValue) ? _pwHash.Value.ToString() : "";
                return h;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _hashString = "";
                    _pwHash = null;
                }
                else
                {
                    PwHash? h = PwHash.Import(value);
                    if (h.HasValue)
                    {
                        _pwHash = h;
                        _hashString = null;
                    }
                    else
                        throw new ArgumentException("Invalid hash string");
                }
            }
        }

        public PwHash? PasswordHash => _pwHash;

        public void SetPasswordHash(PwHash? value)
        {
            _pwHash = value;
            _hashString = null;
        }

        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Editable(false)]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        public Account Creator { get; set; }

        public string CreatorName
        {
            get
            {
                Account account = Creator;
                return (account is null) ? "" : account.Name;
            }
        }

        [Editable(false)]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Editable(false)]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        public Account Modifier { get; set; }

        public string ModifierName
        {
            get
            {
                Account account = Modifier;
                return (account is null) ? "" : account.Name;
            }
        }

        public Account Account { get; set; }

        public void Normalize()
        {
            CreatedOn = CreatedOn.CoerceAsLocalTime();
            ModifiedOn = ModifiedOn.CoerceAsLocalTime();
            if (null != Creator)
                CreatedBy = Creator.AccountID;
            if (null != Modifier)
                ModifiedBy = Modifier.AccountID;
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case PropertyName_PwHash:
                case DisplayName_PwHash:
                    if (_hashString.Length == 0)
                        result.Add(new ValidationResult(Error_Message_PwHash_Empty, new string[] { PropertyName_PwHash }));
                    else if (_hashString.Length > Encoded_Pw_Hash_Length)
                        result.Add(new ValidationResult(Error_Message_PwHash_Long, new string[] { PropertyName_PwHash }));
                    else if (_hashString.Length < Encoded_Pw_Hash_Length)
                        result.Add(new ValidationResult(Error_Message_PwHash_Short, new string[] { PropertyName_PwHash }));
                    else if (!ModelHelper.Base64Regex.IsMatch(_hashString))
                        result.Add(new ValidationResult(Error_Message_PwHash_Invalid, new string[] { PropertyName_PwHash }));
                    break;
            }
        }

        protected virtual void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, PropertyName_PwHash);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            if (string.IsNullOrEmpty(validationContext.DisplayName))
                OnValidateAll(result);
            else
                Validate(result, validationContext.DisplayName);
            return result;
        }

        public IList<ValidationResult> ValidateAll()
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            OnValidateAll(result);
            return result;
        }
    }
}
