using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DevHelper.PsHelp
{
    public abstract class PropertyChangeSupport : INotifyPropertyChanging, INotifyPropertyChanged
    {
        internal event PropertyChangedEventHandler PropertyChanged;
        internal event PropertyChangingEventHandler PropertyChanging;

        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add => PropertyChanging += value;
            remove => PropertyChanging -= value;
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => PropertyChanged += value;
            remove => PropertyChanged -= value;
        }

        protected void RaisePropertyChanging(string propertyName)
        {
            PropertyChangingEventArgs args = new PropertyChangingEventArgs(propertyName);
            OnPropertyChanging(args);
            PropertyChanging?.Invoke(this, args);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            try { OnPropertyChanged(args); }
            finally { PropertyChanged?.Invoke(this, args); }
        }

        protected void OnPropertyChanging(PropertyChangingEventArgs args) { }

        protected void OnPropertyChanged(PropertyChangedEventArgs args) { }

        protected bool CheckPropertyChange<T>(string propertyName, T oldValue, T newValue, Action<T> setter, IEqualityComparer<T> comparer = null)
        {
            if ((comparer ?? EqualityComparer<T>.Default).Equals(oldValue, newValue))
                return false;
            RaisePropertyChanging(propertyName);
            setter(newValue);
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected IList<string> CheckPropertyChange(params PropertyChanger[] propertyChangers)
        {
            if ((propertyChangers = propertyChangers.Where(p => p.IsChange()).ToArray()).Length == 0)
                return new string[0];
            foreach (PropertyChanger p in propertyChangers)
                RaisePropertyChanging(p.PropertyName);
            List<string> changeList = new List<string>();
            ApplyPropertyChanges(propertyChangers.GetEnumerator(), changeList);
            using (IEnumerator<string> enumerator = changeList.GetEnumerator())
            {
                if (enumerator.MoveNext())
                    RaisePropertyChanged(enumerator);
            }
            return changeList;
        }

        private void ApplyPropertyChanges(IEnumerator enumerator, List<string> changeList)
        {
            try
            {
                PropertyChanger propertyChanger = (PropertyChanger)enumerator.Current;
                propertyChanger.ApplyChange();
                changeList.Add(propertyChanger.PropertyName);
            }
            finally
            {
                if (enumerator.MoveNext())
                    ApplyPropertyChanges(enumerator, changeList);
            }
        }

        // Method is recursively called so all handlers are invoked, even if any of them throws an exception.
        private void RaisePropertyChanged(IEnumerator<string> enumerator)
        {
            try { RaisePropertyChanged(enumerator.Current); }
            finally
            {
                if (enumerator.MoveNext())
                    RaisePropertyChanged(enumerator);
            }
        }

        protected abstract class PropertyChanger
        {
            public string PropertyName { get; }
            public abstract bool IsChange();
            public abstract void ApplyChange();
            public PropertyChanger(string propertyName) { PropertyName = propertyName; }
        }

        protected class PropertyChanger<T> : PropertyChanger
        {
            public T OldValue { get; }
            public T NewValue { get; }
            public Action<T> Setter { get; }
            public IEqualityComparer<T> Compare { get; }
            public PropertyChanger(string propertyName, T oldValue, T newValue, Action<T> setter, IEqualityComparer<T> comparer = null)
                : base(propertyName)
            {
                OldValue = oldValue;
                NewValue = newValue;
                Setter = setter;
                Compare = comparer ?? EqualityComparer<T>.Default;
            }

            public override bool IsChange() => !Comparer.Equals(OldValue, NewValue);
            public override void ApplyChange() => Setter(NewValue);
        }
    }
}
