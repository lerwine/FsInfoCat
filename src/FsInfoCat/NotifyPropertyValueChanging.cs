using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FsInfoCat
{
    // TODO: Document NotifyPropertyValueChanging class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class NotifyPropertyValueChanging : NotifyPropertyValueChanged, INotifyPropertyChanging
    {
        public event PropertyChangingEventHandler PropertyChanging;

        protected override bool CheckHashSetChanged<T>(HashSet<T> oldValue, HashSet<T> newValue, Action<HashSet<T>> setter, [CallerMemberName] string propertyName = null)
        {
            return base.CheckHashSetChanged(oldValue, newValue, h =>
            {
                RaisePropertyChanging(propertyName);
                setter(h);
            }, propertyName);
        }

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            PropertyChanging?.Invoke(this, args);
        }

        protected void RaisePropertyChanging([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
