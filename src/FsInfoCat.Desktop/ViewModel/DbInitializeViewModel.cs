using FsInfoCat.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DbInitializeViewModel : DependencyObject
    {
        private static readonly Regex UserNameCheckRegex = new Regex(@"^[a-z]\w*(\.[a-z]\w*)*", RegexOptions.Compiled);
        private static readonly Regex PwCheckRegex = new Regex(@"^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d)(?=[^\p{P}\p{S}]*[\p{P}\p{S}]).{8}", RegexOptions.Compiled);
        private static readonly Regex WsRegex = new Regex(@"( \s+|(?! )\s+)", RegexOptions.Compiled);

        public event EventHandler InitializationSuccess;

        public event EventHandler InitializationCancelled;

        private const string ErrorMessage_UserNameRequired = "User name is required.";
        private const string ErrorMessage_UserNameInvalid = "User name is invalid.";
        private const string ErrorMessage_PasswordRequired = "Password is required.";
        private const string ErrorMessage_PasswordInvalid = "Password does not meet minimum length and complexity requirements (must contains at least 8 characters and at least 1 upper-case letter, 1 lower-case letter, 1 digit, and 1 symbol).";
        private const string ErrorMessage_PasswordMismatch = "Password and confirmation do not match.";
        private const string ErrorMessage_LastNameRequired = "Last name is required.";

        #region Dependency Properties

        public static readonly DependencyProperty IsWindowsAuthProperty =
            DependencyProperty.Register(nameof(IsWindowsAuth), typeof(bool), typeof(DbInitializeViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnIsWindowsAuthPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool IsWindowsAuth
        {
            get { return (bool)GetValue(IsWindowsAuthProperty); }
            set { this.SetValue(IsWindowsAuthProperty, value); }
        }

        private static readonly DependencyPropertyKey WindowsAccountNamePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(WindowsAccountName), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WindowsAccountNameProperty = WindowsAccountNamePropertyKey.DependencyProperty;

        public string WindowsAccountName
        {
            get { return GetValue(WindowsAccountNameProperty) as string; }
            private set { SetValue(WindowsAccountNamePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey WindowsSidPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(WindowsSid), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WindowsSidProperty = WindowsSidPropertyKey.DependencyProperty;

        public string WindowsSid
        {
            get { return GetValue(WindowsSidProperty) as string; }
            private set { SetValue(WindowsSidPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey WindowsDomainPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(WindowsDomain), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WindowsDomainProperty = WindowsDomainPropertyKey.DependencyProperty;

        public string WindowsDomain
        {
            get { return GetValue(WindowsDomainProperty) as string; }
            private set { SetValue(WindowsDomainPropertyKey, value); }
        }

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register(nameof(UserName), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnUserNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string UserName
        {
            get { return GetValue(UserNameProperty) as string; }
            set { SetValue(UserNameProperty, value); }
        }

        private static readonly DependencyPropertyKey IsUserNameValidPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsUserNameValid), typeof(bool), typeof(DbInitializeViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnIsUserNameValidPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public static readonly DependencyProperty IsUserNameValidProperty = IsUserNameValidPropertyKey.DependencyProperty;

        public bool IsUserNameValid
        {
            get { return (bool)GetValue(IsUserNameValidProperty); }
            private set { SetValue(IsUserNameValidPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey UserNameErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(UserNameError), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata(ErrorMessage_UserNameRequired, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnUserNameErrorPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public static readonly DependencyProperty UserNameErrorProperty = UserNameErrorPropertyKey.DependencyProperty;

        public string UserNameError
        {
            get { return GetValue(UserNameErrorProperty) as string; }
            private set { SetValue(UserNameErrorPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey PasswordPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Password), typeof(SecureString), typeof(DbInitializeViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnPasswordPropertyChanged((SecureString)e.OldValue, (SecureString)e.NewValue)));

        public static readonly DependencyProperty PasswordProperty = PasswordPropertyKey.DependencyProperty;

        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            private set { SetValue(PasswordPropertyKey, value); }
        }

        public static readonly DependencyProperty BindToPasswordModel = DependencyProperty.RegisterAttached(nameof(BindToPasswordModel), typeof(DbInitializeViewModel), typeof(DbInitializeViewModel),
            new PropertyMetadata(null, OnBindToPasswordModelPropertyChanged));

        public static DbInitializeViewModel GetBindToPasswordModel(DependencyObject obj) => (obj is null) ? null : (DbInitializeViewModel)obj.GetValue(BindToPasswordModel);

        public static void SetBindToPasswordModel(DependencyObject obj, DbInitializeViewModel newValue) => obj.SetValue(BindToPasswordModel, newValue);

        private static readonly DependencyPropertyKey PwConfirmPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(PwConfirm), typeof(SecureString), typeof(DbInitializeViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnPwConfirmPropertyChanged((SecureString)e.OldValue, (SecureString)e.NewValue)));

        public static readonly DependencyProperty PwConfirmProperty = PwConfirmPropertyKey.DependencyProperty;

        public SecureString PwConfirm
        {
            get { return (SecureString)GetValue(PwConfirmProperty); }
            private set { SetValue(PwConfirmPropertyKey, value); }
        }

        public static readonly DependencyProperty BindToPwConfirmModel = DependencyProperty.RegisterAttached(nameof(BindToPwConfirmModel), typeof(DbInitializeViewModel), typeof(DbInitializeViewModel),
            new PropertyMetadata(null, OnBindToPwConfirmModelPropertyChanged));

        public static DbInitializeViewModel GetBindToPwConfirmModel(DependencyObject obj) => (obj is null) ? null : (DbInitializeViewModel)obj.GetValue(BindToPwConfirmModel);

        public static void SetBindToPwConfirmModel(DependencyObject obj, DbInitializeViewModel newValue) => obj.SetValue(BindToPwConfirmModel, newValue);

        private static readonly DependencyPropertyKey PasswordErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(PasswordError), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata(ErrorMessage_PasswordRequired, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnPasswordErrorPropertyChanged(e.OldValue as string, e.NewValue as string)));

        private static readonly DependencyPropertyKey IsPasswordValidPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsPasswordValid), typeof(bool), typeof(DbInitializeViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnIsPasswordValidPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public static readonly DependencyProperty IsPasswordValidProperty = IsPasswordValidPropertyKey.DependencyProperty;

        public bool IsPasswordValid
        {
            get { return (bool)GetValue(IsPasswordValidProperty); }
            private set { SetValue(IsPasswordValidPropertyKey, value); }
        }

        public static readonly DependencyProperty PasswordErrorProperty = PasswordErrorPropertyKey.DependencyProperty;

        public string PasswordError
        {
            get { return GetValue(PasswordErrorProperty) as string; }
            private set { SetValue(PasswordErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnTitlePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Title
        {
            get { return GetValue(TitleProperty) as string; }
            set { SetValue(TitleProperty, value); }
        }

        private static readonly DependencyPropertyKey NormalizedTitlePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(NormalizedTitle), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnNormalizedTitlePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public static readonly DependencyProperty NormalizedTitleProperty = NormalizedTitlePropertyKey.DependencyProperty;

        public string NormalizedTitle
        {
            get { return GetValue(NormalizedTitleProperty) as string; }
            private set { SetValue(NormalizedTitlePropertyKey, value); }
        }

        public static readonly DependencyProperty LastNameProperty =
            DependencyProperty.Register(nameof(LastName), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnLastNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string LastName
        {
            get { return GetValue(LastNameProperty) as string; }
            set { SetValue(LastNameProperty, value); }
        }

        private static readonly DependencyPropertyKey NormalizedLastNamePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(NormalizedLastName), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnNormalizedLastNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public static readonly DependencyProperty NormalizedLastNameProperty = NormalizedLastNamePropertyKey.DependencyProperty;

        public string NormalizedLastName
        {
            get { return GetValue(NormalizedLastNameProperty) as string; }
            private set { SetValue(NormalizedLastNamePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey LastNameErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(LastNameError), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnLastNameErrorPropertyChanged(e.OldValue as string, e.NewValue as string)));

        private static readonly DependencyPropertyKey IsLastNameValidPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsLastNameValid), typeof(bool), typeof(DbInitializeViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnIsLastNameValidPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public static readonly DependencyProperty IsLastNameValidProperty = IsLastNameValidPropertyKey.DependencyProperty;

        public bool IsLastNameValid
        {
            get { return (bool)GetValue(IsLastNameValidProperty); }
            private set { SetValue(IsLastNameValidPropertyKey, value); }
        }

        public static readonly DependencyProperty LastNameErrorProperty = LastNameErrorPropertyKey.DependencyProperty;

        public string LastNameError
        {
            get { return GetValue(LastNameErrorProperty) as string; }
            private set { SetValue(LastNameErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty FirstNameProperty =
            DependencyProperty.Register(nameof(FirstName), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata(ErrorMessage_LastNameRequired, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnFirstNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string FirstName
        {
            get { return GetValue(FirstNameProperty) as string; }
            set { SetValue(FirstNameProperty, value); }
        }

        private static readonly DependencyPropertyKey NormalizedFirstNamePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(NormalizedFirstName), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnNormalizedFirstNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public static readonly DependencyProperty NormalizedFirstNameProperty = NormalizedFirstNamePropertyKey.DependencyProperty;

        public string NormalizedFirstName
        {
            get { return GetValue(NormalizedFirstNameProperty) as string; }
            private set { SetValue(NormalizedFirstNamePropertyKey, value); }
        }

        public static readonly DependencyProperty MiddleInitialProperty =
            DependencyProperty.Register(nameof(MiddleInitial), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnMiddleInitialPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string MiddleInitial
        {
            get { return GetValue(MiddleInitialProperty) as string; }
            set { SetValue(MiddleInitialProperty, value); }
        }

        private static readonly DependencyPropertyKey NormalizedMiddleInitialPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(NormalizedMiddleInitial), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnNormalizedMiddleInitialPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public static readonly DependencyProperty NormalizedMiddleInitialProperty = NormalizedMiddleInitialPropertyKey.DependencyProperty;

        public string NormalizedMiddleInitial
        {
            get { return GetValue(NormalizedMiddleInitialProperty) as string; }
            private set { SetValue(NormalizedMiddleInitialPropertyKey, value); }
        }

        public static readonly DependencyProperty SuffixProperty =
            DependencyProperty.Register(nameof(Suffix), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnSuffixPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Suffix
        {
            get { return GetValue(SuffixProperty) as string; }
            set { SetValue(SuffixProperty, value); }
        }

        private static readonly DependencyPropertyKey NormalizedSuffixPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(NormalizedSuffix), typeof(string), typeof(DbInitializeViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbInitializeViewModel).OnNormalizedSuffixPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public static readonly DependencyProperty NormalizedSuffixProperty = NormalizedSuffixPropertyKey.DependencyProperty;

        public string NormalizedSuffix
        {
            get { return GetValue(NormalizedSuffixProperty) as string; }
            private set { SetValue(NormalizedSuffixPropertyKey, value); }
        }

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(DbInitializeViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbInitializeViewModel).OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string DisplayName
        {
            get { return GetValue(DisplayNameProperty) as string; }
            set { SetValue(DisplayNameProperty, value); }
        }

        private static readonly DependencyPropertyKey NormalizedDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(NormalizedDisplayName), typeof(string),
            typeof(DbInitializeViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty NormalizedDisplayNameProperty = NormalizedDisplayNamePropertyKey.DependencyProperty;

        public string NormalizedDisplayName
        {
            get { return GetValue(NormalizedDisplayNameProperty) as string; }
            private set { SetValue(NormalizedDisplayNamePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey AutoDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(AutoDisplayName), typeof(string),
            typeof(DbInitializeViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbInitializeViewModel).OnAutoDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public static readonly DependencyProperty AutoDisplayNameProperty = AutoDisplayNamePropertyKey.DependencyProperty;

        public string AutoDisplayName
        {
            get { return GetValue(AutoDisplayNameProperty) as string; }
            private set { SetValue(AutoDisplayNamePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey ContinueCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ContinueCommand),
            typeof(Commands.RelayCommand), typeof(DbInitializeViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((DbInitializeViewModel)d).OnContinueExecute, false, true)));

        public static readonly DependencyProperty ContinueCommandProperty = ContinueCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand ContinueCommand => (Commands.RelayCommand)GetValue(ContinueCommandProperty);

        private static readonly DependencyPropertyKey CancelCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelCommand),
            typeof(Commands.RelayCommand), typeof(DbInitializeViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((DbInitializeViewModel)d).InvokeCancelCommand)));

        public static readonly DependencyProperty CancelCommandProperty = CancelCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand CancelCommand => (Commands.RelayCommand)GetValue(CancelCommandProperty);

        private void InvokeCancelCommand()
        {
            try { OnCancelExecute(); }
            finally { InitializationCancelled?.Invoke(this, EventArgs.Empty); }
        }

        #endregion

        private static string AsWsNormalized(string newValue)
        {
            if (newValue is null)
                return "";
            return ((newValue = newValue.Trim()).Length > 0 && WsRegex.IsMatch(newValue)) ? WsRegex.Replace(newValue, " ") : newValue;
        }

        private static void OnBindToPasswordModelPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PasswordBox passwordBox)
            {
                if (e.OldValue is DbInitializeViewModel wasBound)
                    passwordBox.PasswordChanged -= wasBound.OnPasswordChanged;
                if (e.NewValue is DbInitializeViewModel needToBind)
                {
                    passwordBox.PasswordChanged += needToBind.OnPasswordChanged;
                    needToBind.Password = passwordBox.SecurePassword;
                }
            }
        }

        internal void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
                Password = passwordBox.SecurePassword;
        }

        private static void OnBindToPwConfirmModelPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PasswordBox passwordBox)
            {
                if (e.OldValue is DbInitializeViewModel wasBound)
                    passwordBox.PasswordChanged -= wasBound.OnPasswordChanged;
                if (e.NewValue is DbInitializeViewModel needToBind)
                {
                    passwordBox.PasswordChanged += needToBind.OnPasswordChanged;
                    needToBind.PwConfirm = passwordBox.SecurePassword;
                }
            }
        }

        internal void OnPwConfirmChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
                Password = passwordBox.SecurePassword;
        }

        protected virtual void OnIsWindowsAuthPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
                IsUserNameValid = IsPasswordValid = true;
            else
            {
                IsUserNameValid = string.IsNullOrWhiteSpace(UserNameError);
                IsPasswordValid = string.IsNullOrWhiteSpace(PasswordError);
            }
        }

        protected virtual void OnUserNamePropertyChanged(string oldValue, string newValue)
        {
            ValidateUserName(newValue);
        }

        protected virtual void OnPasswordPropertyChanged(SecureString oldValue, SecureString newValue)
        {
            ValidatePassword(newValue, PwConfirm);
        }

        protected virtual void OnPwConfirmPropertyChanged(SecureString oldValue, SecureString newValue)
        {
            ValidatePassword(Password, newValue);
        }

        private void ValidateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                UserNameError = ErrorMessage_UserNameRequired;
        }

        private static string GetRawPw(SecureString secureString)
        {
            if (secureString is null || secureString.Length == 0)
                return "";
            IntPtr ptr = Marshal.SecureStringToBSTR(secureString);
            try
            {
                return Marshal.PtrToStringBSTR(ptr);
            }
            finally { Marshal.FreeBSTR(ptr); }
        }
        private void ValidatePassword(SecureString password, SecureString pwConfirm)
        {
            if (password is null || password.Length == 0)
                PasswordError = ErrorMessage_PasswordRequired;
            else if (GetRawPw(password).Equals(GetRawPw(pwConfirm)))
            {
                if (PwCheckRegex.IsMatch(GetRawPw(password)))
                    PasswordError = "";
                else
                    PasswordError = ErrorMessage_PasswordInvalid;
            }
            else
                PasswordError = ErrorMessage_PasswordMismatch;
        }

        protected virtual void OnUserNameErrorPropertyChanged(string oldValue, string newValue)
        {
            IsUserNameValid = IsWindowsAuth || string.IsNullOrWhiteSpace(newValue);
        }

        protected virtual void OnPasswordErrorPropertyChanged(string oldValue, string newValue)
        {
            IsPasswordValid = IsWindowsAuth || string.IsNullOrWhiteSpace(newValue);
        }

        protected virtual void OnTitlePropertyChanged(string oldValue, string newValue) => NormalizedTitle = AsWsNormalized(newValue);

        protected virtual void OnLastNamePropertyChanged(string oldValue, string newValue) => NormalizedLastName = AsWsNormalized(newValue);

        protected virtual void OnFirstNamePropertyChanged(string oldValue, string newValue) => NormalizedLastName = AsWsNormalized(newValue);

        protected virtual void OnMiddleInitialPropertyChanged(string oldValue, string newValue) => NormalizedMiddleInitial = AsWsNormalized(newValue);

        protected virtual void OnSuffixPropertyChanged(string oldValue, string newValue) => NormalizedSuffix = AsWsNormalized(newValue);

        protected virtual void OnDisplayNamePropertyChanged(string oldValue, string newValue) => NormalizedDisplayName = AsWsNormalized(newValue);

        protected virtual void OnNormalizedTitlePropertyChanged(string oldValue, string newValue) =>
            AutoDisplayName = BuildAutoDisplayName(newValue, NormalizedLastName, NormalizedFirstName, NormalizedMiddleInitial, NormalizedSuffix);

        protected virtual void OnNormalizedLastNamePropertyChanged(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
                LastNameError = ErrorMessage_LastNameRequired;
            else
                LastNameError = "";
            AutoDisplayName = BuildAutoDisplayName(NormalizedTitle, newValue, NormalizedFirstName, NormalizedMiddleInitial, NormalizedSuffix);
        }

        protected virtual void OnLastNameErrorPropertyChanged(string oldValue, string newValue)
        {
            IsLastNameValid = newValue.Length > 0;
        }

        protected virtual void OnNormalizedFirstNamePropertyChanged(string oldValue, string newValue) =>
            AutoDisplayName = BuildAutoDisplayName(NormalizedTitle, NormalizedLastName, newValue, NormalizedMiddleInitial, NormalizedSuffix);

        protected virtual void OnNormalizedMiddleInitialPropertyChanged(string oldValue, string newValue) =>
            AutoDisplayName = BuildAutoDisplayName(NormalizedTitle, NormalizedLastName, NormalizedFirstName, newValue, NormalizedSuffix);

        protected virtual void OnNormalizedSuffixPropertyChanged(string oldValue, string newValue) =>
            AutoDisplayName = BuildAutoDisplayName(NormalizedTitle, NormalizedLastName, NormalizedFirstName, NormalizedMiddleInitial, newValue);

        private static string BuildAutoDisplayName(string title, string lastName, string firstName, string mi, string suffix)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                if (string.IsNullOrEmpty(firstName))
                {
                    if (string.IsNullOrEmpty(mi))
                    {
                        if (string.IsNullOrEmpty(title))
                            return suffix;
                        if (string.IsNullOrEmpty(suffix))
                            return title;
                        return $"{title}, {suffix}";
                    }
                    if (string.IsNullOrEmpty(title))
                    {
                        if (string.IsNullOrEmpty(suffix))
                            return $"{mi}.";
                        return $"{mi}., {suffix}";
                    }
                    if (string.IsNullOrEmpty(suffix))
                        return $"{title} {mi}.";
                    return $"{title} {mi}., {suffix}";
                }
                if (string.IsNullOrEmpty(mi))
                {
                    if (string.IsNullOrEmpty(title))
                    {
                        if (string.IsNullOrEmpty(suffix))
                            return firstName;
                        return $"{firstName}, {suffix}";
                    }
                    if (string.IsNullOrEmpty(suffix))
                        return $"{title}  {firstName}";
                    return $"{title} {firstName}, {suffix}";
                }
                if (string.IsNullOrEmpty(title))
                {
                    if (string.IsNullOrEmpty(suffix))
                        return $"{firstName} {mi}.";
                    return $"{firstName} {mi}., {suffix}";
                }
                if (string.IsNullOrEmpty(suffix))
                    return $"{title} {firstName} {mi}.";
                return $"{title} {firstName} {mi}., {suffix}";
            }
            if (string.IsNullOrEmpty(firstName))
            {
                if (string.IsNullOrEmpty(mi))
                {
                    if (string.IsNullOrEmpty(title))
                    {
                        if (string.IsNullOrEmpty(suffix))
                            return lastName;
                        return $"{lastName}, {suffix}";
                    }
                    if (string.IsNullOrEmpty(suffix))
                        return $"{title} {lastName}";
                    return $"{title} {lastName}, {suffix}";
                }
                if (string.IsNullOrEmpty(title))
                {
                    if (string.IsNullOrEmpty(suffix))
                        return $"{lastName}, {mi}.";
                    return $"{lastName}, {mi}., {suffix}";
                }
                if (string.IsNullOrEmpty(suffix))
                    return $"{title} {lastName}, {mi}.";
                return $"{title} {lastName}, {mi}., {suffix}";
            }
            if (string.IsNullOrEmpty(mi))
            {
                if (string.IsNullOrEmpty(title))
                {
                    if (string.IsNullOrEmpty(suffix))
                        return $"{lastName}, {firstName}";
                    return $"{lastName}, {firstName}, {suffix}";
                }
                if (string.IsNullOrEmpty(suffix))
                    return $"{title} {lastName}, {firstName}";
                return $"{title} {lastName}, {firstName}, {suffix}";
            }
            if (string.IsNullOrEmpty(title))
            {
                if (string.IsNullOrEmpty(suffix))
                    return $"{lastName}, {firstName} {mi}.";
                return $"{lastName}, {firstName} {mi}., {suffix}";
            }
            if (string.IsNullOrEmpty(suffix))
                return $"{title} {lastName}, {firstName} {mi}.";
            return $"{title} {lastName}, {firstName} {mi}., {suffix}";
        }

        protected virtual void OnAutoDisplayNamePropertyChanged(string oldValue, string newValue)
        {
            if (NormalizedDisplayName.Length == 0 || string.Equals(NormalizedDisplayName, oldValue, StringComparison.InvariantCultureIgnoreCase))
                DisplayName = newValue;
        }

        protected virtual void OnIsUserNameValidPropertyChanged(bool oldValue, bool newValue)
        {
            ContinueCommand.IsEnabled = newValue && IsPasswordValid && IsLastNameValid;
        }

        protected virtual void OnIsPasswordValidPropertyChanged(bool oldValue, bool newValue)
        {
            ContinueCommand.IsEnabled = IsUserNameValid && newValue && IsLastNameValid;
        }

        protected virtual void OnIsLastNameValidPropertyChanged(bool oldValue, bool newValue)
        {
            ContinueCommand.IsEnabled = IsUserNameValid && IsPasswordValid && newValue;
        }

        private void OnContinueExecute()
        {
            if (NormalizedDisplayName.Length == 0)
                NormalizedDisplayName = BuildAutoDisplayName(NormalizedTitle, NormalizedLastName, NormalizedFirstName, NormalizedMiddleInitial, NormalizedSuffix);
            InitializeDbAsync(IsWindowsAuth ? null : UserName, Password, NormalizedTitle, NormalizedLastName, NormalizedFirstName,
                NormalizedMiddleInitial, NormalizedSuffix, NormalizedDisplayName, Dispatcher.ToInvocationAction<string, int>(UpdateState)).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                        Dispatcher.BeginInvoke(new Action<AggregateException>(OnIntitializationFailed), null);
                    else if (task.IsFaulted)
                        Dispatcher.BeginInvoke(new Action<AggregateException>(OnIntitializationFailed), task.Exception);
                    else
                        Dispatcher.BeginInvoke(new Action<bool>(OnIntitializationCompleted), task.Result);
                });
        }

        private void OnCancelExecute()
        {
            InitializationCancelled?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateState(string message, int progress)
        {
            // TODO: Implement UpdateState Logic
        }

        private void OnIntitializationCompleted(bool succeeded)
        {
            // TODO: Implement OnIntitializationCompleted Logic
        }

        private void OnIntitializationFailed(AggregateException exception)
        {
            // TODO: Implement OnIntitializationFailed Logic
        }

        // TODO: Need to put DB access on a common background thread.
        private static Task<bool> InitializeDbAsync(string userName, SecureString password, string title, string lastName, string firstName, string mi, string suffix,
            string displayName, Action<string, int> updateState) => Task.Run(() => InitializeDb(userName, password, title, lastName, firstName, mi, suffix, displayName, updateState));

        private static bool InitializeDb(string userName, SecureString password, string title, string lastName, string firstName, string mi, string suffix,
            string displayName, Action<string, int> updateState)
        {
            // TODO: Implement InitializeDb Logic
            return false;
        }
    }
}
