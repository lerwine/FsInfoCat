using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class EnumValueItem<TEnum> : DependencyObject
        where TEnum : struct, Enum
    {
        #region Value Property Members

        private static readonly DependencyPropertyKey ValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Value), typeof(TEnum), typeof(EnumValueItem<TEnum>),
                new PropertyMetadata(default(TEnum)));

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = ValuePropertyKey.DependencyProperty;

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
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(EnumValueItem<TEnum>),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EnumValueItem<TEnum>)?.IsSelectedPropertyChanged?.Invoke(d, e)));

        public bool IsSelected { get => (bool)GetValue(IsSelectedProperty); set => SetValue(IsSelectedProperty, value); }

        #endregion
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayName), typeof(string), typeof(EnumValueItem<TEnum>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        public string DisplayName { get => GetValue(DisplayNameProperty) as string; private set => SetValue(DisplayNamePropertyKey, value); }

        #endregion
        public EnumValueItem(TEnum value)
        {
            Value = value;
            DisplayName = value.GetDisplayName();
        }
    }
}
