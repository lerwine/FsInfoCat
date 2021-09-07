using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Numerics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    /// <summary>
    /// Converts <seealso cref="bool"/> values to  <seealso cref="Visibility"/> values.
    /// </summary>
    [ValueConversion(typeof(long), typeof(string))]
    public class DenominatedFileSizeConverter : ToClassConverterBase<long, string>
    {
        /// <summary>
        /// Identifies the <see cref="NullSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NullSourceProperty = DependencyPropertyBuilder<DenominatedFileSizeConverter, string>
            .Register(nameof(NullSource))
            .AsReadWrite();

        public override string NullSource { get => GetValue(NullSourceProperty) as string; set => SetValue(NullSourceProperty, value); }

        public override string Convert(long value, object parameter, CultureInfo culture) => new BinaryDenominatedInt64F(value).ToString(culture);

        protected override object ConvertBack(string target, object parameter, CultureInfo culture) => BinaryDenominatedInt64F.TryParse(target, culture, out BinaryDenominatedInt64F result) ? result.BinaryValue : null;
    }
}
