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
