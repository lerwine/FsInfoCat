using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class UserLoginRequest
    {
        private string _loginName = "";
        private string _password = "";

        [Required()]
        [Display(Name = "Login Name")]
        [RegularExpression(@"\S+", ErrorMessage = "Please provide the login name")]
        /// <summary>
        /// Gets the user's login name.
        /// /// </summary>
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Password")]
        [RegularExpression(@"\S+", ErrorMessage = "Please enter the password")]
        /// <summary>
        /// Gets the hash for the user's password.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = (null == value) ? "" : value; }
        }
    }
}
