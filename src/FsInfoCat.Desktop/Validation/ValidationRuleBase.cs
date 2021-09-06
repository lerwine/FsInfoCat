using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace FsInfoCat.Desktop.Validation
{
    public abstract class ValidationRuleBase : ValidationRule, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) => OnPropertyChanged(new(propertyName));

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);
    }
}
