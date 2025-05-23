using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FsInfoCat
{
    // TODO: Document NotifyPropertyValueChanged class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class NotifyPropertyValueChanged : INotifyPropertyValueChanged
    {
        private readonly ILogger<NotifyPropertyValueChanged> _logger = Hosting.ServiceProvider.GetRequiredService<ILogger<NotifyPropertyValueChanged>>();

        public event PropertyValueChangedEventHandler PropertyValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        protected object SyncRoot { get; } = new();

        protected virtual bool CheckHashSetChanged<T>(HashSet<T> oldValue, HashSet<T> newValue, Action<HashSet<T>> setter, [CallerMemberName] string propertyName = null)
        {
            if (newValue is null)
            {
                if (oldValue.Count == 0)
                    return false;
                setter([]);
            }
            else
            {
                if (ReferenceEquals(oldValue, newValue))
                    return false;
                setter(newValue);
            }
            RaisePropertyChanged(oldValue, newValue, propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged(PropertyValueChangedEventArgs args)
        {
            try { PropertyValueChanged?.Invoke(this, args); }
            finally { PropertyChanged?.Invoke(this, args); }
        }

        protected void RaisePropertyChanged<T>(T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            using (_logger.BeginScope("Property {propertyName} changed", propertyName))
                OnPropertyChanged(new PropertyValueChangedEventArgs(propertyName, oldValue, newValue));
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
