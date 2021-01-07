using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FsInfoCat.Models.DB;

namespace FsInfoCat.Models.Accounts
{
    public class UserLoginRequest : IUserCredentials
    {
        public const string PropertyName_Password = "Password";
        public const string DisplayName_Password = "Password";
        public const string Error_Message_Password = "Please enter the password";

        private string _loginName = "";
        private string _password = "";

        [Required()]
        [MinLength(1, ErrorMessage = Account.Error_Message_Login_Empty)]
        [MaxLength(Account.Max_Length_Login_Name, ErrorMessage = Account.Error_Message_Login_Length)]
        [RegularExpression(ModelHelper.PATTERN_DOTTED_NAME, ErrorMessage = Account.Error_Message_Login_Invalid)]
        [Display(Name = Account.DisplayName_LoginName)]
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = UserLoginRequest.DisplayName_Password)]
        [RegularExpression(@"\S+", ErrorMessage = UserLoginRequest.Error_Message_Password)]
        public string Password
        {
            get { return _password; }
            set { _password = (null == value) ? "" : value; }
        }

        public void Normalize()
        {
            _loginName = _loginName.Trim();
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, Account.PropertyName_LoginName);
            Validate(result, PropertyName_Password);
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case PropertyName_Password:
                    if (_password.Length == 0)
                        result.Add(new ValidationResult(Error_Message_Password, new string[] { PropertyName_Password }));
                    break;
                case Account.PropertyName_LoginName:
                case Account.DisplayName_LoginName:
                    if (_loginName.Length == 0)
                        result.Add(new ValidationResult(Account.Error_Message_Login_Empty, new string[] { Account.PropertyName_LoginName }));
                    else if (_loginName.Length > Account.Max_Length_DisplayName)
                        result.Add(new ValidationResult(Account.Error_Message_Login_Length, new string[] { Account.PropertyName_LoginName }));
                    else if (!ModelHelper.DottedNameRegex.IsMatch(_loginName))
                        result.Add(new ValidationResult(Account.Error_Message_Login_Invalid, new string[] { Account.PropertyName_LoginName }));
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
    }
}
