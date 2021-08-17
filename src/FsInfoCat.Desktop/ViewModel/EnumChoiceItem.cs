using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class EnumChoiceItem<TEnum> : DependencyObject
        where TEnum : struct, Enum
    {
        #region Value Property Members

        private static readonly DependencyPropertyKey ValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Value), typeof(TEnum), typeof(EnumChoiceItem<TEnum>),
                new PropertyMetadata(default(TEnum)));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = ValuePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public TEnum Value { get => (TEnum)GetValue(ValueProperty); private set => SetValue(ValuePropertyKey, value); }

        #endregion
        #region IsSelected Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="IsSelected"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsSelectedPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(EnumChoiceItem<TEnum>),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EnumChoiceItem<TEnum>)?.OnIsSelectedPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsSelected { get => (bool)GetValue(IsSelectedProperty); set => SetValue(IsSelectedProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="IsSelectedProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="IsSelectedProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnIsSelectedPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnIsSelectedPropertyChanged((bool)args.OldValue, (bool)args.NewValue); }
            finally { IsSelectedPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="IsSelected"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsSelected"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsSelected"/> property.</param>
        protected virtual void OnIsSelectedPropertyChanged(bool oldValue, bool newValue)
        {
            EnumValuePickerVM<TEnum> valuePicker = EnumValuePickerVM<TEnum>.GetEnumValuePickerVM(this);
            if (newValue)
                valuePicker.SelectedItem = this;
            else if (ReferenceEquals(valuePicker.SelectedItem, this))
                valuePicker.SelectedItem = null;
        }

        #endregion
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(EnumChoiceItem<TEnum>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DisplayName { get => GetValue(DisplayNameProperty) as string; private set => SetValue(DisplayNamePropertyKey, value); }

        #endregion
        public EnumChoiceItem(TEnum value)
        {
            Value = value;
            DisplayName = value.GetDisplayName();
        }
    }
}
