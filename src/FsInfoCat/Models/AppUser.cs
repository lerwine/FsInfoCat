using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Models
{
    public class AppUser
    {
        public const int Max_Length_Login_Name = 32;
        public const string Role_Name_Viewer = "viewer";
        public const string Role_Name_User = "user";
        public const string Role_Name_Crawler = "crawler";
        public const string Role_Name_Admin = "admin";
        private static readonly ReadOnlyDictionary<string, UserRole> _FromRoleNameMap;
        private static readonly ReadOnlyDictionary<UserRole, string> _ToRoleNameMap;

        public static string ToRoleName(UserRole role)
        {
            return (_ToRoleNameMap.ContainsKey(role)) ? _ToRoleNameMap[role] : null;
        }

        public static UserRole FromRoleName(string roleName)
        {
            return (null != roleName && _FromRoleNameMap.ContainsKey(roleName)) ? _FromRoleNameMap[roleName] : UserRole.None;
        }

        private string _displayName = "";
        private string _loginName = "";
        private string _notes = "";

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid UserID { get; set; }

        [Required()]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Required()]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Required()]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Required()]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        public string Name => (string.IsNullOrWhiteSpace(_displayName)) ? _loginName : _displayName;

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
        [Display(Name = "User Role")]
        [EnumDataType(typeof(UserRole))]
        /// <summary>
        /// Gets the role of the user.
        /// </summary>
        public UserRole Role { get; set; }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

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

        }

        public AppUser(AppUser user)
        {
            if (null == user)
                throw new ArgumentNullException("user");
            UserID = user.UserID;
            CreatedBy = user.CreatedBy;
            CreatedOn = user.CreatedOn;
            ModifiedBy = user.ModifiedBy;
            LoginName = user.LoginName;
            DisplayName = user.DisplayName;
            ModifiedOn = user.ModifiedOn;
            Role = user.Role;
            Notes = user.Notes;
        }
    }
}
