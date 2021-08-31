using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class EnumChoiceItem<TEnum> : EnumValueViewModel<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// Called when the value of the <see cref="IsSelected"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsSelected"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsSelected"/> property.</param>
        protected override void OnIsSelectedPropertyChanged(bool oldValue, bool newValue)
        {
            EnumValuePickerVM<TEnum> valuePicker = EnumValuePickerVM<TEnum>.GetEnumValuePickerVM(this);
            if (valuePicker is null)
                return;
            if (newValue)
                valuePicker.SelectedItem = this;
            else if (ReferenceEquals(valuePicker.SelectedItem, this))
                valuePicker.SelectedItem = null;
        }

        public EnumChoiceItem([DisallowNull] string displayName) : base(displayName) { }

        public EnumChoiceItem(TEnum value) : base(value) { }
    }
}
