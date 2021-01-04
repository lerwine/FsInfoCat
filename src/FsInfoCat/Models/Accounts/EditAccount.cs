using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FsInfoCat.Models.DB;

namespace FsInfoCat.Models.Accounts
{
    public class EditAccount : IAccount
    {
        public const string DisplayName_Password = "Password";
        public const string PropertyName_Password = "Password";
        public const string Error_Message_Password_Empty = "Password cannot be empty.";
        public const string DisplayName_Confirm = "Confirm password";
        public const string PropertyName_Confirm = "Confirm";
        public const string Error_Message_Confirm = "Password and confirmation do not match.";
        public const string DisplayName_Inactive = "Inactive";
        public const string DisplayName_Viewer = "Viewer";
        public const string DisplayName_Normal_User = "Normal User";
        public const string DisplayName_App_Contributor = "App Contributor";
        public const string DisplayName_App_Administrator = "App Administrator";
        private string _displayName = "";
        private string _loginName = "";
        private string _password = "";
        private string _confirm = "";
        private string _notes = "";
        private UserRole _role = UserRole.Viewer;
        private string _roleDisplay = null;
        private bool _isViewer = true;
        private bool _isUser = false;
        private bool _isAppContrib = false;
        private bool _isAdmin = false;

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
        [MaxLength(Account.Max_Length_DisplayName, ErrorMessage = Account.Error_Message_DisplayName)]
        [Display(Name = Account.DisplayName_DisplayName)]
        [DataType(DataType.Text)]
#endif
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

#if CORE
        [Required(ErrorMessage = Account.Error_Message_Login_Empty)]
        [MinLength(1, ErrorMessage = Account.Error_Message_Login_Empty)]
        [MaxLength(Account.Max_Length_Login_Name, ErrorMessage = Account.Error_Message_Login_Length)]
        [RegularExpression(ModelHelper.PATTERN_DOTTED_NAME, ErrorMessage = Account.Error_Message_Login_Invalid)]
        [Display(Name = Account.DisplayName_LoginName)]
#endif
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (null == value) ? "" : value; }
        }

#if CORE
        [Display(Name = "Change Password")]
#endif
        public bool ChangePassword { get; set; }

#if CORE
        [Display(Name = DisplayName_Password)]
        [DataType(DataType.Text)]
#endif
        public string Password
        {
            get { return _password; }
            set { _password = (null == value) ? "" : value; }
        }

#if CORE
        [Display(Name = DisplayName_Confirm)]
        [DataType(DataType.Text)]
#endif
        public string Confirm
        {
            get { return _confirm; }
            set { _confirm = (null == value) ? "" : value; }
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
                _isViewer = value != UserRole.None;
                if (_isViewer)
                {
                    _isUser = value != UserRole.Viewer;
                    if (_isUser)
                    {
                        _isAppContrib = value != UserRole.User;
                        _isAdmin = value == UserRole.Admin;
                        return;
                    }
                }
                else
                    _isUser = false;
                _isAppContrib = _isAdmin = false;
            }
        }

#if CORE
        [Display(Name = DisplayName_Inactive)]
#endif
        public bool IsInactive
        {
            get { return !_isViewer; }
            set
            {
                if (value)
                    Role = UserRole.None;
                else if (_role == UserRole.None)
                    Role = UserRole.Viewer;
                else
                    _isViewer = true;
            }
        }

#if CORE
        [Display(Name = DisplayName_Viewer)]
#endif
        public bool IsViewer
        {
            get { return _isViewer; }
            set
            {
                if (value)
                {
                    if (_role == UserRole.None)
                        Role = UserRole.Viewer;
                    else
                        _isViewer = true;
                }
                else
                    Role = UserRole.None;
            }
        }

#if CORE
        [Display(Name = DisplayName_Normal_User)]
#endif
        public bool IsUser
        {
            get { return _isUser; }
            set
            {
                switch (_role)
                {
                    case UserRole.None:
                    case UserRole.Viewer:
                        if (value)
                            Role = UserRole.User;
                        else
                            _isUser = false;
                        break;
                    default:
                        if (value)
                            _isUser = true;
                        else
                            Role = UserRole.Viewer;
                        break;
                }
            }
        }

#if CORE
        [Display(Name = DisplayName_App_Contributor)]
#endif
        public bool IsAppContrib
        {
            get { return _isAppContrib; }
            set
            {
                switch (_role)
                {
                    case UserRole.Admin:
                    case UserRole.Crawler:
                        if (value)
                            _isAppContrib = true;
                        else
                            Role = UserRole.User;
                        break;
                    default:
                        if (value)
                            Role = UserRole.Viewer;
                        else
                            _isAppContrib = false;
                        break;
                }
            }
        }

#if CORE
        [Display(Name = DisplayName_App_Administrator)]
#endif
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                if (value)
                    Role = UserRole.Admin;
                else if (_role == UserRole.Admin)
                    Role = UserRole.Crawler;
                else
                    _isAdmin = false;
            }
        }

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

        public EditAccount(IAccount user)
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
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case Account.PropertyName_DisplayName:
                case Account.DisplayName_DisplayName:
                    if (_displayName.Length > Account.Max_Length_DisplayName)
                        result.Add(new ValidationResult(Account.Error_Message_DisplayName, new string[] { Account.PropertyName_DisplayName }));
                    break;
                case Account.PropertyName_LoginName:
                case Account.DisplayName_LoginName:
                    if (_loginName.Length == 0)
                        result.Add(new ValidationResult(Account.Error_Message_Login_Empty, new string[] { Account.PropertyName_LoginName }));
                    else if (_loginName.Length > Account.Max_Length_Login_Name)
                        result.Add(new ValidationResult(Account.Error_Message_Login_Length, new string[] { Account.PropertyName_LoginName }));
                    else if (!ModelHelper.DottedNameRegex.IsMatch(_loginName))
                        result.Add(new ValidationResult(Account.Error_Message_Login_Invalid, new string[] { Account.PropertyName_LoginName }));
                    break;
                case PropertyName_Password:
                    if (ChangePassword && _password.Length == 0)
                        result.Add(new ValidationResult(Error_Message_Password_Empty, new string[] { PropertyName_Password }));
                    break;
                case PropertyName_Confirm:
                case DisplayName_Confirm:
                    if (ChangePassword && !_password.Equals(_confirm))
                        result.Add(new ValidationResult(Error_Message_Confirm, new string[] { PropertyName_Confirm }));
                    break;
            }
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, Account.PropertyName_DisplayName);
            Validate(result, Account.PropertyName_LoginName);
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
    }
}