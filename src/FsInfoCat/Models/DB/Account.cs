using FsInfoCat.Models.Accounts;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Models.DB
{

    [DisplayColumn("AccountID", "DisplayName", false)]
    public class Account : IAccount
    {
        #region Fields

        public const int Max_Length_DisplayName = 128;
        public const string DisplayName_DisplayName = "Display name";
        public const string PropertyName_DisplayName = "DisplayName";
        public const string Error_Message_DisplayName = "Display name too long.";
        public const int Max_Length_Login_Name = 32;
        public const string DisplayName_LoginName = "Login name";
        public const string PropertyName_LoginName = "LoginName";
        public const string Error_Message_Login_Empty = "Login name cannot be empty.";
        public const string Error_Message_Login_Length = "Login name too long.";
        public const string Error_Message_Login_Invalid = "Invalid login name.";
        private static readonly ReadOnlyDictionary<string, UserRole> _FromRoleNameMap;
        private static readonly ReadOnlyDictionary<UserRole, string> _ToRoleNameMap;

        private string _displayName = "";
        private string _loginName = "";
        private string _notes = "";
        private UserRole _role = UserRole.Viewer;
        private string _roleDisplay = null;

        #endregion

        #region Properties

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid AccountID { get; set; }

        public string Name
        {
            get
            {
                string n = _displayName;
                return (string.IsNullOrWhiteSpace(n)) ? _loginName : n;
            }
        }


        [MaxLength(Max_Length_DisplayName, ErrorMessage = Error_Message_DisplayName)]
        [Display(Name = DisplayName_DisplayName)]
        [DataType(DataType.Text)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (value is null) ? "" : value; }
        }

        [Required(ErrorMessage = Error_Message_Login_Empty)]
        [MinLength(1, ErrorMessage = Error_Message_Login_Empty)]
        [MaxLength(Max_Length_Login_Name, ErrorMessage = Error_Message_Login_Length)]
        [RegularExpression(ModelHelper.PATTERN_DOTTED_NAME, ErrorMessage = Error_Message_Login_Invalid)]
        [Display(Name = DisplayName_LoginName)]
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (value is null) ? "" : value; }
        }

        [Display(Name = "Role")]
        public string RoleDisplay
        {
            get
            {
                string n = _roleDisplay;
                if (n is null)
                    _roleDisplay = n = _role.GetDescription();
                return n;
            }
        }

        [Required()]
        [Display(Name = "Role")]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role
        {
            get { return _role; }
            set
            {
                // Normalize enum value
                UserRole role = ModelHelper.ToUserRole((byte)value);
                if (role != _role)
                {
                    _roleDisplay = null;
                    _role = role;
                }
            }
        }

        [Display(Name = "Is Inactive")]
        public bool IsInactive => Role == UserRole.None;

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (value is null) ? "" : value; }
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

        #endregion

        public UserCredential UserCredential { get; set; }

        static Account()
        {
            Dictionary<string, UserRole> fromRoleNameMap = new Dictionary<string, UserRole>(StringComparer.CurrentCultureIgnoreCase);
            Dictionary<UserRole, string> toRoleNameMap = new Dictionary<UserRole, string>();
            foreach (var m in Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Select(r =>
                new
                {
                    Value = r,
                    Name = r.GetName()
                }).Where(a => null != a.Name))
            {
                toRoleNameMap.Add(m.Value, m.Name);
                fromRoleNameMap.Add(m.Name, m.Value);
            }
            _ToRoleNameMap = new ReadOnlyDictionary<UserRole, string>(toRoleNameMap);
            _FromRoleNameMap = new ReadOnlyDictionary<string, UserRole>(fromRoleNameMap);
        }

        public Account() { }

        public Account(string loginName, string pwHash, UserRole role, Guid createdBy)
        {
            LoginName = loginName;
            Role = role;
            UserCredential = new UserCredential
            {
                HashString = pwHash,
                CreatedOn = DateTime.Now,
                CreatedBy = createdBy
            };
            CreatedOn = ModifiedOn = UserCredential.ModifiedOn = UserCredential.CreatedOn;
            CreatedBy = ModifiedBy = UserCredential.ModifiedBy = UserCredential.CreatedBy;
        }

        public Account(string loginName, UserRole role, Guid createdBy)
        {
            LoginName = loginName;
            Role = role;
            CreatedOn = ModifiedOn = UserCredential.CreatedOn = DateTime.Now;
            CreatedBy = ModifiedBy = UserCredential.CreatedBy = createdBy;
        }

        public Account(string loginName, string pwHash, UserRole role, Account creator)
        {
            if (creator is null)
                throw new ArgumentNullException("creator");
            LoginName = loginName;
            Role = role;
            Creator = creator;
            UserCredential = new UserCredential
            {
                HashString = pwHash,
                CreatedOn = DateTime.Now,
                Creator = creator
            };
            CreatedOn = ModifiedOn = UserCredential.ModifiedOn = UserCredential.CreatedOn;
            CreatedBy = ModifiedBy = UserCredential.ModifiedBy = UserCredential.CreatedBy = creator.AccountID;
        }

        public Account(string loginName, UserRole role, Account creator)
        {
            if (creator is null)
                throw new ArgumentNullException("creator");
            LoginName = loginName;
            Role = role;
            Creator = creator;
            CreatedOn = ModifiedOn = UserCredential.CreatedOn = DateTime.Now;
            CreatedBy = ModifiedBy = UserCredential.CreatedBy = creator.AccountID;
        }

        public Account(IAccount user)
        {
            if (user is null)
                throw new ArgumentNullException("user");
            AccountID = user.AccountID;
            Creator = user.Creator;
            CreatedBy = user.CreatedBy;
            CreatedOn = user.CreatedOn;
            Modifier = user.Modifier;
            ModifiedBy = user.ModifiedBy;
            LoginName = user.LoginName;
            DisplayName = user.DisplayName;
            ModifiedOn = user.ModifiedOn;
            Role = user.Role;
            Notes = user.Notes;
            if (user is Account account)
                UserCredential = account.UserCredential;
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case PropertyName_DisplayName:
                case DisplayName_DisplayName:
                    if (_displayName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_DisplayName, new string[] { PropertyName_DisplayName }));
                    break;
                case PropertyName_LoginName:
                case DisplayName_LoginName:
                    if (_loginName.Length == 0)
                        result.Add(new ValidationResult(Error_Message_Login_Empty, new string[] { PropertyName_LoginName }));
                    else if (_loginName.Length > Max_Length_Login_Name)
                        result.Add(new ValidationResult(Error_Message_Login_Length, new string[] { PropertyName_LoginName }));
                    else if (!ModelHelper.DottedNameRegex.IsMatch(_loginName))
                        result.Add(new ValidationResult(Error_Message_Login_Invalid, new string[] { PropertyName_LoginName }));
                    break;
            }
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, PropertyName_DisplayName);
            Validate(result, PropertyName_LoginName);
        }

        public void Normalize()
        {
            _loginName = _loginName.Trim();
            if ((_displayName = _displayName.CoerceAsWsNormalized()).Length == 0)
                _displayName = _loginName;
            _notes = _notes.Trim();
            CreatedOn = CreatedOn.CoerceAsLocalTime();
            ModifiedOn = ModifiedOn.CoerceAsLocalTime();
            if (null != Creator)
                CreatedBy = Creator.AccountID;
            if (null != Modifier)
                ModifiedBy = Modifier.AccountID;
        }
        public IList<ValidationResult> ValidateAll()
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            OnValidateAll(result);
            return result;
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

        public override string ToString()
        {
            return "[AccountID=" + AccountID.ToString("d") + "; LoginName=\"" + LoginName.Replace("\"", "\\\"") + "\"; LoginName=\"" + DisplayName.Replace("\"", "\\\"") + "\"; Role=" + Role.GetName() + "]";
        }

        public static string ToRoleName(UserRole role)
        {
            return (_ToRoleNameMap.ContainsKey(role)) ? _ToRoleNameMap[role] : null;
        }

        public static UserRole FromRoleName(string roleName)
        {
            return (null != roleName && _FromRoleNameMap.ContainsKey(roleName)) ? _FromRoleNameMap[roleName] : UserRole.None;
        }
    }
}
