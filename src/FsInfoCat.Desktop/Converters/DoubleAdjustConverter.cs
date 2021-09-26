using FsInfoCat.Desktop.ViewModel;
using System.Globalization;
using System.Windows;

namespace FsInfoCat.Desktop.Converters
{
    public class DoubleAdjustConverter : ToValueConverterBase<double, double>
    {
        #region NullSource Property Members

        /// <summary>
        /// Identifies the <see cref="NullSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NullSourceProperty = DependencyPropertyBuilder<DoubleAdjustConverter, double?>
            .Register(nameof(NullSource))
            .DefaultValue(null)
            .AsReadWrite();

        public override double? NullSource { get => (double?)GetValue(NullSourceProperty); set => SetValue(NullSourceProperty, value); }

        #endregion
        #region AddedValue Property Members

        /// <summary>
        /// Identifies the <see cref="AddedValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AddedValueProperty = DependencyPropertyBuilder<DoubleAdjustConverter, double>
            .Register(nameof(AddedValue))
            .DefaultValue(0.0)
            .AsReadWrite();

        public double AddedValue { get => (double)GetValue(AddedValueProperty); set => SetValue(AddedValueProperty, value); }

        #endregion
        public override double? Convert(double value, object parameter, CultureInfo culture) => value + AddedValue;
    }
}
