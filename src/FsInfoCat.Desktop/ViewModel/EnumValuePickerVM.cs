using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class EnumValuePickerVM<TEnum> : DependencyObject
        where TEnum : struct, Enum
    {
        #region EnumValuePickerVM Attached Property Members

        public static EnumValuePickerVM<TEnum> GetEnumValuePickerVM(DependencyObject obj) => (EnumValuePickerVM<TEnum>)obj.GetValue(EnumValuePickerVMProperty);

        private static void SetEnumValuePickerVM(DependencyObject obj, EnumValuePickerVM<TEnum> value) => obj.SetValue(EnumValuePickerVMProperty, value);

        private static readonly DependencyPropertyKey EnumValuePickerVMPropertyKey = DependencyProperty.RegisterAttachedReadOnly(nameof(EnumValuePickerVM<TEnum>),
            typeof(EnumValuePickerVM<TEnum>), typeof(EnumValuePickerVM<TEnum>), new PropertyMetadata(null));

        public static readonly DependencyProperty EnumValuePickerVMProperty = EnumValuePickerVMPropertyKey.DependencyProperty;

        #endregion
        #region Choices Property Members

        private readonly ObservableCollection<EnumChoiceItem<TEnum>> _backingChoices = new();

        private static readonly DependencyPropertyKey ChoicesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Choices),
            typeof(ReadOnlyObservableCollection<EnumChoiceItem<TEnum>>), typeof(EnumValuePickerVM<TEnum>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Choices"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChoicesProperty = ChoicesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<EnumChoiceItem<TEnum>> Choices => (ReadOnlyObservableCollection<EnumChoiceItem<TEnum>>)GetValue(ChoicesProperty);

        #endregion
        #region SelectedItem Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="SelectedItem"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler SelectedItemPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(EnumChoiceItem<TEnum>), typeof(EnumValuePickerVM<TEnum>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EnumValuePickerVM<TEnum>)?.OnSelectedItemPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public EnumChoiceItem<TEnum> SelectedItem { get => (EnumChoiceItem<TEnum>)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="SelectedItemProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="SelectedItemProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnSelectedItemPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnSelectedItemPropertyChanged((EnumChoiceItem<TEnum>)args.OldValue, (EnumChoiceItem<TEnum>)args.NewValue); }
            finally { SelectedItemPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="SelectedItem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedItem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedItem"/> property.</param>
        private void OnSelectedItemPropertyChanged(EnumChoiceItem<TEnum> oldValue, EnumChoiceItem<TEnum> newValue)
        {
            if (oldValue is not null)
                oldValue.IsSelected = false;
            if (newValue is null)
            {
                SelectedIndex = -1;
                SelectedValue = null;
            }
            else
            {
                newValue.IsSelected = true;
                SelectedIndex = _backingChoices.IndexOf(newValue);
                SelectedValue = newValue.Value;
            }
        }

        #endregion
        #region SelectedValue Property Members

        /// <summary>
        /// Identifies the <see cref="SelectedValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(nameof(SelectedValue), typeof(TEnum?), typeof(EnumValuePickerVM<TEnum>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EnumValuePickerVM<TEnum>)?.OnSelectedValuePropertyChanged((TEnum?)e.OldValue, (TEnum?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public TEnum? SelectedValue { get => (TEnum?)GetValue(SelectedValueProperty); set => SetValue(SelectedValueProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SelectedValue"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedValue"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedValue"/> property.</param>
        private void OnSelectedValuePropertyChanged(TEnum? oldValue, TEnum? newValue)
        {
            TEnum? currentValue = SelectedItem?.Value;
            if (newValue.HasValue)
            {
                if (newValue.HasValue && newValue.Value.Equals(currentValue.Value))
                    return;
                for (int i = 0; i < _backingChoices.Count; i++)
                {
                    if (_backingChoices[i].Value.Equals(newValue.Value))
                    {
                        SelectedItem = _backingChoices[i];
                        return;
                    }
                }
            }
            else if (!newValue.HasValue)
                return;
            SelectedItem = null;
        }

        #endregion
        #region SelectedIndex Property Members

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(EnumValuePickerVM<TEnum>),
                new PropertyMetadata(-1, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EnumValuePickerVM<TEnum>)?.OnSelectedIndexPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public int SelectedIndex { get => (int)GetValue(SelectedIndexProperty); set => SetValue(SelectedIndexProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SelectedIndex"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedIndex"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedIndex"/> property.</param>
        private void OnSelectedIndexPropertyChanged(int oldValue, int newValue)
        {
            SelectedItem = (newValue < 0 || newValue >= _backingChoices.Count) ? null : _backingChoices[newValue];
        }

        #endregion
        public EnumValuePickerVM()
        {
            SetValue(ChoicesPropertyKey, new ReadOnlyObservableCollection<EnumChoiceItem<TEnum>>(_backingChoices));
            foreach (TEnum value in Enum.GetValues<TEnum>())
            {
                EnumChoiceItem<TEnum> item = new(value);
                SetEnumValuePickerVM(item, this);
                _backingChoices.Add(new(value));
            }
            SelectedItem = _backingChoices.First();
        }
    }
}