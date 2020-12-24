using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class UserLoginRequest
    {
        private string _loginName = "";
        private string _password = "";

        [Display(Name = "Login Name")]
        /// <summary>
        /// Gets the user's login name.
        /// /// </summary>
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (null == value) ? "" : value; }
        }

        [Display(Name = "Password")]
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
