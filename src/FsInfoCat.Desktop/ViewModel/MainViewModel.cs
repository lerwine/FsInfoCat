using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MainViewModel : DependencyObject
    {
        public event DependencyPropertyChangedEventHandler IsLoggedInPropertyChanged;

        public static readonly DependencyPropertyKey IsLoggedInPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsLoggedIn), typeof(bool), typeof(MainViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as MainViewModel).OnIsLoggedInPropertyChanged(e)));

        public static readonly DependencyProperty IsLoggedInProperty = IsLoggedInPropertyKey.DependencyProperty;

        public bool IsLoggedIn
        {
            get { return (bool)GetValue(IsLoggedInProperty); }
            private set { SetValue(IsLoggedInPropertyKey, value); }
        }

        protected virtual void OnIsLoggedInPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnIsLoggedInPropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsLoggedInPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnIsLoggedInPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnIsLoggedInPropertyChanged Logic
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

        public event EventHandler LogOut;

        private Commands.RelayCommand _logOutCommand = null;

        public Commands.RelayCommand LogOutCommand
        {
            get
            {
                if (_logOutCommand == null)
                    _logOutCommand = new Commands.RelayCommand(parameter =>
                    {
                        try { OnLogOut(parameter); }
                        finally { LogOut?.Invoke(this, EventArgs.Empty); }
                    });
                return _logOutCommand;
            }
        }

        protected virtual void OnLogOut(object parameter)
        {
            // TODO: Implement OnLogOut Logic
        }

        public event EventHandler Options;

        private Commands.RelayCommand _optionsCommand = null;

        public Commands.RelayCommand OptionsCommand
        {
            get
            {
                if (_optionsCommand == null)
                    _optionsCommand = new Commands.RelayCommand(parameter =>
                    {
                        try { OnOptions(parameter); }
                        finally { Options?.Invoke(this, EventArgs.Empty); }
                    });
                return _optionsCommand;
            }
        }

        protected virtual void OnOptions(object parameter)
        {
            // TODO: Implement OnOptions Logic
        }

        public event EventHandler Help;

        private Commands.RelayCommand _helpCommand = null;

        public Commands.RelayCommand HelpCommand
        {
            get
            {
                if (_helpCommand == null)
                    _helpCommand = new Commands.RelayCommand(parameter =>
                    {
                        try { OnHelp(parameter); }
                        finally { Help?.Invoke(this, EventArgs.Empty); }
                    });
                return _helpCommand;
            }
        }

        protected virtual void OnHelp(object parameter)
        {
            // TODO: Implement OnHelp Logic
        }

    }
}
