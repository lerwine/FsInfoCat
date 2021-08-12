using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class ValidationStateTracker : DependencyObject
    {
        private readonly Collection<string> _invalidNames = new();

        public event EventHandler AnyInvalidPropertyChanged;

        private static readonly DependencyPropertyKey AnyInvalidPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AnyInvalid), typeof(bool), typeof(ValidationStateTracker),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ValidationStateTracker).OnAnyInvalidPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public static readonly DependencyProperty AnyInvalidProperty = AnyInvalidPropertyKey.DependencyProperty;

        public bool AnyInvalid
        {
            get => (bool)GetValue(AnyInvalidProperty);
            private set => SetValue(AnyInvalidPropertyKey, value);
        }

        protected virtual void OnAnyInvalidPropertyChanged(bool oldValue, bool newValue) => AnyInvalidPropertyChanged?.Invoke(this, EventArgs.Empty);

        protected void SetValidationState(string propertyName, bool isValid)
        {
            VerifyAccess();
            if (isValid)
            {
                if (_invalidNames.Remove(propertyName) && _invalidNames.Count == 0)
                    AnyInvalid = false;
            }
            else if (!_invalidNames.Contains(propertyName))
            {
                if (_invalidNames.Count == 0)
                {
                    _invalidNames.Add(propertyName);
                    AnyInvalid = true;
                }
                else
                    _invalidNames.Add(propertyName);
            }
        }
    }
}
