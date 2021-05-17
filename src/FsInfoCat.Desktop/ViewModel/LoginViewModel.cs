using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using FsInfoCat.Model.Remote;

namespace FsInfoCat.Desktop.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.DependencyObject" />
    /// <remarks>
    /// References
    /// <list type="bullet">
    ///     <item><term>Moser, C. (2014, June 22)</term>
    ///         <description>WPF PasswordBox Control
    ///         <para>Retrieved 2021, April 18, from WPF Tutorial: http://www.wpftutorial.net/PasswordBox.html</para></description>
    ///     </item>
    /// </list></remarks>
    public class LoginViewModel : DependencyObject
    {
        public event EventHandler LoginSucceeded;

        public event EventHandler LoginAborted;

        public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(nameof(UserName), typeof(string), typeof(LoginViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as LoginViewModel).OnUserNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string UserName
        {
            get { return GetValue(UserNameProperty) as string; }
            set { SetValue(UserNameProperty, value); }
        }

        protected virtual void OnUserNamePropertyChanged(string oldValue, string newValue) => OnCredentialsChanged(newValue, Password);

        public static readonly DependencyProperty BindToModel = DependencyProperty.RegisterAttached(nameof(BindToModel), typeof(LoginViewModel), typeof(LoginViewModel),
            new PropertyMetadata(null, OnBindToModelPropertyChanged));

        public static LoginViewModel GetBindToModel(DependencyObject obj) => (obj is null) ? null : (LoginViewModel)obj.GetValue(BindToModel);

        public static void SetBindToModel(DependencyObject obj, LoginViewModel newValue) => obj.SetValue(BindToModel, newValue);

        private static void OnBindToModelPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PasswordBox passwordBox)
            {
                if (e.OldValue is LoginViewModel wasBound)
                    passwordBox.PasswordChanged -= wasBound.OnPasswordChanged;
                if (e.NewValue is LoginViewModel needToBind)
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

        private static readonly DependencyPropertyKey PasswordPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Password), typeof(SecureString), typeof(LoginViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as LoginViewModel).OnPasswordPropertyChanged((SecureString)e.OldValue, (SecureString)e.NewValue)));

        public static readonly DependencyProperty PasswordProperty = PasswordPropertyKey.DependencyProperty;

        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            private set { SetValue(PasswordPropertyKey, value); }
        }

        protected virtual void OnPasswordPropertyChanged(SecureString oldValue, SecureString newValue) => OnCredentialsChanged(UserName, newValue);

        private static readonly DependencyPropertyKey CanSubmitPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanSubmit), typeof(bool), typeof(LoginViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as LoginViewModel).OnCanSubmitPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public static readonly DependencyProperty CanSubmitProperty = CanSubmitPropertyKey.DependencyProperty;

        public bool CanSubmit
        {
            get { return (bool)GetValue(CanSubmitProperty); }
            private set { SetValue(CanSubmitPropertyKey, value); }
        }

        protected virtual void OnCanSubmitPropertyChanged(bool oldValue, bool newValue)
        {
            LoginCommand.IsEnabled = EnableInputs && newValue;
        }

        private void OnCredentialsChanged(string userName, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                ErrorMessage = "User name is required.";
            else if (password is null || password.Length == 0)
                ErrorMessage = "Password is required.";
            else
            {
                CanSubmit = true;
                ErrorMessage = "";
                return;
            }
            CanSubmit = false;
        }

        private static readonly DependencyPropertyKey ErrorMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(ErrorMessage), typeof(string), typeof(LoginViewModel),
            new PropertyMetadata(""));

        public static readonly DependencyProperty ErrorMessageProperty = ErrorMessagePropertyKey.DependencyProperty;

        public string ErrorMessage
        {
            get { return GetValue(ErrorMessageProperty) as string; }
            private set { SetValue(ErrorMessagePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey LoginCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LoginCommand),
            typeof(Commands.RelayCommand), typeof(LoginViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((LoginViewModel)d).OnLoginExecute)));

        public static readonly DependencyProperty LoginCommandProperty = LoginCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand LoginCommand => (Commands.RelayCommand)GetValue(LoginCommandProperty);

        private void OnLoginExecute(object parameter)
        {
            EnableInputs = false;
            ErrorMessage = "";
        }

        private void OnLoginCompleted(IUserProfile result)
        {
            if (result is null)
            {
                if (string.IsNullOrWhiteSpace(ErrorMessage))
                    ErrorMessage = "Invalid user name or password";
            }
            else
                LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoginFailed(AggregateException exception)
        {
            if (exception is null)
                ErrorMessage = "Credential validation canceled.";
            else
                ErrorMessage = $"Error validating user name or password: {(string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message)}";
        }

        private static readonly DependencyPropertyKey CancelCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelCommand),
            typeof(Commands.RelayCommand), typeof(LoginViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((LoginViewModel)d).InvokeCancelCommand)));

        public static readonly DependencyProperty CancelCommandProperty = CancelCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand CancelCommand => (Commands.RelayCommand)GetValue(CancelCommandProperty);

        private void InvokeCancelCommand(object parameter) => LoginAborted?.Invoke(this, EventArgs.Empty);

        private static readonly DependencyPropertyKey ProgressValuePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ProgressValue), typeof(int), typeof(LoginViewModel),
                new PropertyMetadata(0));

        public static readonly DependencyProperty ProgressValueProperty = ProgressValuePropertyKey.DependencyProperty;

        public int ProgressValue
        {
            get { return (int)GetValue(ProgressValueProperty); }
            private set { SetValue(ProgressValuePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey StatusMessagePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(StatusMessage), typeof(string), typeof(LoginViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty StatusMessageProperty = StatusMessagePropertyKey.DependencyProperty;

        public string StatusMessage
        {
            get { return GetValue(StatusMessageProperty) as string; }
            private set { SetValue(StatusMessagePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey EnableInputsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(EnableInputs), typeof(bool), typeof(LoginViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as LoginViewModel).OnEnableInputsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public static readonly DependencyProperty EnableInputsProperty = EnableInputsPropertyKey.DependencyProperty;

        public bool EnableInputs
        {
            get { return (bool)GetValue(EnableInputsProperty); }
            private set { SetValue(EnableInputsPropertyKey, value); }
        }

        protected virtual void OnEnableInputsPropertyChanged(bool oldValue, bool newValue)
        {
            CancelCommand.IsEnabled = newValue;
            LoginCommand.IsEnabled = newValue && CanSubmit;
        }

        public void OnPasswordTextBoxChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordTextBox)
                Password = passwordTextBox.SecurePassword;
        }
    }
}
