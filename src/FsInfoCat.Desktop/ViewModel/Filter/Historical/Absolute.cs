using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter.Historical
{
    public class Absolute : HistoricalTimeReference, IAbsoluteTimeReference
    {
        #region Value Property Members

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyPropertyBuilder<Absolute, DateTime>
            .Register(nameof(Value))
            .AsReadWrite();

        public DateTime Value { get => (DateTime)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        #endregion

        public override DateTime ToDateTime() => Value;

        protected override int CompareTo(DateTime other) => Value.CompareTo(other);
    }
}
