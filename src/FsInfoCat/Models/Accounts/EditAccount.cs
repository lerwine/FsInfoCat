using FsInfoCat.Models.DB;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [MaxLength(Account.Max_Length_DisplayName, ErrorMessage = Account.Error_Message_DisplayName)]
        [Display(Name = Account.DisplayName_DisplayName)]
        [DataType(DataType.Text)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (value is null) ? "" : value; }
        }

        [Required(ErrorMessage = Account.Error_Message_Login_Empty)]
        [MinLength(1, ErrorMessage = Account.Error_Message_Login_Empty)]
        [MaxLength(Account.Max_Length_Login_Name, ErrorMessage = Account.Error_Message_Login_Length)]
        [RegularExpression(ModelHelper.PATTERN_LOGIN_NAME_VALIDATION, ErrorMessage = Account.Error_Message_Login_Invalid)]
        [Display(Name = Account.DisplayName_LoginName)]
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (value is null) ? "" : value; }
        }

        [Display(Name = "Change Password")]
        public bool ChangePassword { get; set; }

        [Display(Name = DisplayName_Password)]
        [DataType(DataType.Text)]
        public string Password
        {
            get { return _password; }
            set { _password = (value is null) ? "" : value; }
        }

        [Display(Name = DisplayName_Confirm)]
        [DataType(DataType.Text)]
        public string Confirm
        {
            get { return _confirm; }
            set { _confirm = (value is null) ? "" : value; }
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
                _isViewer = role != UserRole.None;
                if (_isViewer)
                {
                    _isUser = role != UserRole.Viewer;
                    if (_isUser)
                    {
                        _isAppContrib = role != UserRole.User;
                        _isAdmin = role == UserRole.Admin;
                        return;
                    }
                }
                else
                    _isUser = false;
                _isAppContrib = _isAdmin = false;
            }
        }

        [Display(Name = DisplayName_Inactive)]
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

        [Display(Name = DisplayName_Viewer)]
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

        [Display(Name = DisplayName_Normal_User)]
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

        [Display(Name = DisplayName_App_Contributor)]
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

        [Display(Name = DisplayName_App_Administrator)]
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

        public EditAccount(IAccount user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
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
                    else if (!ModelHelper.LOGIN_NAME_VALIDATION_REGEX.IsMatch(_loginName))
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
            if ((_displayName = _displayName.CoerceAsWsNormalized()).Length == 0)
                _displayName = _loginName;
            _notes = _notes.Trim();
            CreatedOn = CreatedOn.CoerceAsLocalTime();
            ModifiedOn = ModifiedOn.CoerceAsLocalTime();
            if (!(Creator is null))
                CreatedBy = Creator.AccountID;
            if (!(Modifier is null))
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
    }
}
