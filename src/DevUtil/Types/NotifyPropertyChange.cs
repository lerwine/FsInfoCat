using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DevUtil.Types
{
    public class NotifyPropertyChange : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanging<T>(T currentValue, T newValue, [CallerMemberName] string propertyName = null) => OnPropertyChanging(new PropertyChangingEventArgs<T>(currentValue, newValue, propertyName));

        protected virtual void OnPropertyChanging<T>(PropertyChangingEventArgs<T> args) => PropertyChanging?.Invoke(this, args);

        protected void RaisePropertyChanged<T>(T oldValue, T currentValue, [CallerMemberName] string propertyName = null) => OnPropertyChanged(new PropertyChangedEventArgs<T>(oldValue, currentValue, propertyName));

        protected virtual void OnPropertyChanged<T>(PropertyChangedEventArgs<T> args) => PropertyChanged?.Invoke(this, args);
    }
}
