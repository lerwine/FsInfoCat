using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FsInfoCat.Models.Accounts;

namespace FsInfoCat.Models.DB
{

#if CORE
    [DisplayColumn("AccountID", "DisplayName", false)]
#endif
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

#if CORE
        [Required()]
        [Key()]
        [Display(Name = "ID")]
#endif
        public Guid AccountID { get; set; }

        public string Name
        {
            get
            {
                string n = _displayName;
                return (string.IsNullOrWhiteSpace(n)) ? _loginName : n;
            }
        }

#if CORE
        [MaxLength(Max_Length_DisplayName, ErrorMessage = Error_Message_DisplayName)]
        [Display(Name = DisplayName_DisplayName)]
        [DataType(DataType.Text)]
#endif
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

#if CORE
        [Required(ErrorMessage = Error_Message_Login_Empty)]
        [MinLength(1, ErrorMessage = Error_Message_Login_Empty)]
        [MaxLength(Max_Length_Login_Name, ErrorMessage = Error_Message_Login_Length)]
        [RegularExpression(ModelHelper.PATTERN_DOTTED_NAME, ErrorMessage = Error_Message_Login_Invalid)]
        [Display(Name = DisplayName_LoginName)]
#endif
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (null == value) ? "" : value; }
        }

#if CORE
        [Display(Name = "Role")]
#endif
        public string RoleDisplay
        {
            get
            {
                string n = _roleDisplay;
                if (null == n)
                {
                    n = Enum.GetName(_role.GetType(), _role);
                    _roleDisplay = n = _role.GetType().GetField(n).GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>()
                        .Select(a => a.Description).Where(s => !string.IsNullOrWhiteSpace(s)).DefaultIfEmpty(n).First();
                }
                return n;
            }
        }

#if CORE
        [Required()]
        [Display(Name = "Role")]
        [EnumDataType(typeof(UserRole))]
#endif
        public UserRole Role
        {
            get { return _role; }
            set
            {
                if (value != _role)
                {
                    _roleDisplay = null;
                    _role = value;
                }
            }
        }

#if CORE
        [Display(Name = "Is Inactive")]
#endif
        public bool IsInactive => Role == UserRole.None;

#if CORE
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
#endif
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

#if CORE
        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
#endif
        public DateTime CreatedOn { get; set; }

#if CORE
        [Editable(false)]
        [Display(Name = "Created By")]
#endif
        public Guid CreatedBy { get; set; }

        public Account Creator { get; set; }

        public string CreatorName
        {
            get
            {
                Account account = Creator;
                return (null == account) ? "" : account.Name;
            }
        }

#if CORE
        [Editable(false)]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
#endif
        public DateTime ModifiedOn { get; set; }

#if CORE
        [Editable(false)]
        [Display(Name = "Modified By")]
#endif
        public Guid ModifiedBy { get; set; }

        public Account Modifier { get; set; }

        public string ModifierName
        {
            get
            {
                Account account = Modifier;
                return (null == account) ? "" : account.Name;
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
                    Name = r.GetType().GetField(r.ToString("F")).GetCustomAttributes(typeof(AmbientValueAttribute), false)
                    .Cast<AmbientValueAttribute>().Select(a => a.Value as string).FirstOrDefault(v => null != v)
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
            if (null == creator)
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
            if (null == creator)
                throw new ArgumentNullException("creator");
            LoginName = loginName;
            Role = role;
            Creator = creator;
            CreatedOn = ModifiedOn = UserCredential.CreatedOn = DateTime.Now;
            CreatedBy = ModifiedBy = UserCredential.CreatedBy = creator.AccountID;
        }

        public Account(IAccount user)
        {
            if (null == user)
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
            if (user is Account)
                UserCredential = ((Account)user).UserCredential;
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
            if ((_displayName = ModelHelper.CoerceAsWsNormalized(_displayName)).Length == 0)
                _displayName = _loginName;
            _notes = _notes.Trim();
            CreatedOn = ModelHelper.CoerceAsLocalTime(CreatedOn);
            ModifiedOn = ModelHelper.CoerceAsLocalTime(ModifiedOn);
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
            return "[AccountID=" + AccountID.ToString("d") + "; LoginName=\"" + LoginName.Replace("\"", "\\\"") + "\"; LoginName=\"" + DisplayName.Replace("\"", "\\\"") + "\"; Role=" + Role.ToString("F") + "]";
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
