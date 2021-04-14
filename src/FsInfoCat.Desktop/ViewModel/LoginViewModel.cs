using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class LoginViewModel : DependencyObject
    {

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register(nameof(UserName), typeof(string), typeof(LoginViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as LoginViewModel).OnUserNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string UserName
        {
            get { return GetValue(UserNameProperty) as string; }
            set { SetValue(UserNameProperty, value); }
        }

        protected virtual void OnUserNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnUserNamePropertyChanged Logic
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(LoginViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as LoginViewModel).OnPasswordPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Password
        {
            get { return GetValue(PasswordProperty) as string; }
            set { SetValue(PasswordProperty, value); }
        }

        protected virtual void OnPasswordPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnPasswordPropertyChanged Logic
        }

        public static readonly DependencyPropertyKey LoginErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(LoginError), typeof(string), typeof(LoginViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty LoginErrorProperty = LoginErrorPropertyKey.DependencyProperty;

        public string LoginError
        {
            get { return GetValue(LoginErrorProperty) as string; }
            private set { SetValue(LoginErrorPropertyKey, value); }
        }

        public event EventHandler Login;

        private Commands.RelayCommand _loginCommand = null;

        public Commands.RelayCommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new Commands.RelayCommand(parameter =>
                    {
                        try { OnLogin(parameter); }
                        finally { Login?.Invoke(this, EventArgs.Empty); }
                    });
                return _loginCommand;
            }
        }

        protected virtual void OnLogin(object parameter)
        {
            // TODO: Implement OnLogin Logic
        }

        public event EventHandler Cancel;

        private Commands.RelayCommand _cancelCommand = null;

        public Commands.RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new Commands.RelayCommand(parameter =>
                    {
                        try { OnCancel(parameter); }
                        finally { Cancel?.Invoke(this, EventArgs.Empty); }
                    });
                return _cancelCommand;
            }
        }

        protected virtual void OnCancel(object parameter)
        {
            // TODO: Implement OnCancel Logic
        }

    }
}
