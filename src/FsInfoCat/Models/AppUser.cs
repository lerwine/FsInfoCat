using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Models
{
    public class AppUser : IAppUser
    {
        #region Fields

        public const int Max_Length_DisplayName = 256;
        public const string DisplayName_DisplayName = "Display name";
        public const string PropertyName_DisplayName = "DisplayName";
        public const string Error_Message_DisplayName = "Display name too long.";
        public const int Max_Length_Login_Name = 32;
        public const string DisplayName_LoginName = "Login name";
        public const string PropertyName_LoginName = "LoginName";
        public const string Error_Message_Login_Empty = "Login name cannot be empty.";
        public const string Error_Message_Login_Length = "Login name too long.";
        public const string Error_Message_Login_Invalid = "Invalid login name.";
        public const string Role_Name_Viewer = "viewer";
        public const string Role_Name_User = "user";
        public const string Role_Name_Crawler = "crawler";
        public const string Role_Name_Admin = "admin";
        private static readonly ReadOnlyDictionary<string, UserRole> _FromRoleNameMap;
        private static readonly ReadOnlyDictionary<UserRole, string> _ToRoleNameMap;

        private string _displayName = "";
        private string _loginName = "";
        private string _notes = "";

        #endregion

        #region Properties

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid AccountID { get; set; }

        [MaxLength(Max_Length_DisplayName, ErrorMessage = Error_Message_DisplayName)]
        [Display(Name = DisplayName_DisplayName)]
        [DataType(DataType.Text)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        [Required()]
        [MinLength(1, ErrorMessage = Error_Message_Login_Empty)]
        [MaxLength(Max_Length_Login_Name, ErrorMessage = Error_Message_Login_Length)]
        [RegularExpression(ModelHelper.PATTERN_DOTTED_NAME, ErrorMessage = Error_Message_Login_Invalid)]
        [Display(Name = DisplayName_LoginName)]
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "User Role")]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Editable(false)]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Editable(false)]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Editable(false)]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        #endregion

        #region Constructors

        static AppUser()
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

        protected AppUser()
        {
            CreatedOn = ModifiedOn = DateTime.UtcNow;
        }

        protected AppUser(string userName, UserRole role, Guid createdBy) : this()
        {
            LoginName = userName;
            Role = role;
            CreatedBy = ModifiedBy = createdBy;
        }

        public AppUser(AppUser user)
        {
            if (null == user)
                throw new ArgumentNullException("user");
            AccountID = user.AccountID;
            CreatedBy = user.CreatedBy;
            CreatedOn = user.CreatedOn;
            ModifiedBy = user.ModifiedBy;
            LoginName = user.LoginName;
            DisplayName = user.DisplayName;
            ModifiedOn = user.ModifiedOn;
            Role = user.Role;
            Notes = user.Notes;
        }

        #endregion

        public void Normalize()
        {
            if (AccountID.Equals(Guid.Empty))
                AccountID = Guid.NewGuid();
            _loginName = _loginName.Trim();
            if ((_displayName = ModelHelper.CoerceAsWsNormalized(_displayName)).Length == 0)
                _displayName = _loginName;
            _notes = _notes.Trim();
        }

        protected virtual void Validate(List<ValidationResult> result, string propertyName)
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
                    else if (_loginName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_Login_Length, new string[] { PropertyName_LoginName }));
                    else if (!ModelHelper.DottedNameRegex.IsMatch(_loginName))
                        result.Add(new ValidationResult(Error_Message_Login_Invalid, new string[] { PropertyName_LoginName }));
                    break;
            }
        }

        public IList<ValidationResult> ValidateAll()
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            OnValidateAll(result);
            return result;
        }

        protected virtual void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, PropertyName_DisplayName);
            Validate(result, PropertyName_LoginName);
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
