using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    /// <summary>
    /// View model to select multiple <see cref="Enum"/> values.
    /// </summary>
    /// <typeparam name="TEnum">The type of the <see cref="Enum"/> value.</typeparam>
    /// <seealso cref="DependencyObject" />
    public class EnumValueSelectorVM<TEnum> : DependencyObject
        where TEnum : struct, Enum
    {
        public event NotifyCollectionChangedEventHandler SelectionChanged;

        #region EnumValueSelectorVM Attached Property Members

        public static EnumValueSelectorVM<TEnum> GetEnumValueSelectorVM(DependencyObject obj) => (EnumValueSelectorVM<TEnum>)obj.GetValue(EnumValueSelectorVMProperty);

        private static void SetEnumValueSelectorVM(DependencyObject obj, EnumValueSelectorVM<TEnum> value) => obj.SetValue(EnumValueSelectorVMPropertyKey, value);

        private static readonly DependencyPropertyKey EnumValueSelectorVMPropertyKey = DependencyProperty.RegisterAttachedReadOnly(nameof(EnumValueSelectorVM<TEnum>),
            typeof(EnumValueSelectorVM<TEnum>), typeof(EnumValueSelectorVM<TEnum>), new PropertyMetadata(null));

        public static readonly DependencyProperty EnumValueSelectorVMProperty = EnumValueSelectorVMPropertyKey.DependencyProperty;

        #endregion
        #region Choices Property Members

        private readonly ObservableCollection<EnumValueItem<TEnum>> _backingChoices = new();

        private static readonly DependencyPropertyKey ChoicesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Choices),
            typeof(ReadOnlyObservableCollection<EnumValueItem<TEnum>>), typeof(EnumValuePickerVM<TEnum>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Choices"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChoicesProperty = ChoicesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<EnumValueItem<TEnum>> Choices => (ReadOnlyObservableCollection<EnumValueItem<TEnum>>)GetValue(ChoicesProperty);

        #endregion
        #region SelectedItems Property Members

        private readonly ObservableCollection<EnumValueItem<TEnum>> _backingSelectedItems = new();

        private static readonly DependencyPropertyKey SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectedItems), typeof(ReadOnlyObservableCollection<EnumValueItem<TEnum>>), typeof(EnumValueSelectorVM<TEnum>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SelectedItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<EnumValueItem<TEnum>> SelectedItems => (ReadOnlyObservableCollection<EnumValueItem<TEnum>>)GetValue(SelectedItemsProperty);

        #endregion

        public EnumValueSelectorVM()
        {
            SetValue(ChoicesPropertyKey, new ReadOnlyObservableCollection<EnumValueItem<TEnum>>(_backingChoices));
            SetValue(SelectedItemsPropertyKey, new ReadOnlyObservableCollection<EnumValueItem<TEnum>>(_backingSelectedItems));
            _backingSelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
            foreach (TEnum value in Enum.GetValues<TEnum>())
            {
                EnumValueItem<TEnum> item = new(value);
                SetEnumValueSelectorVM(item, this);
                _backingChoices.Add(new(value));
                item.IsSelectedPropertyChanged += Item_IsSelectedPropertyChanged;
            }
        }

        private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        private void Item_IsSelectedPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is EnumValueItem<TEnum> item)
            {
                if ((bool)e.NewValue)
                    _backingSelectedItems.Add(item);
                else
                    _ = _backingSelectedItems.Remove(item);
            }
        }
    }
}
