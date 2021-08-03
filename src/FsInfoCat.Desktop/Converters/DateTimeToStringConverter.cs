using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeToStringConverter : ToClassConverterBase<DateTime, string>
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(DateTimeToStringConverter), new PropertyMetadata(""));

        public override string NullSource
        {
            get => GetValue(NullSourceProperty) as string;
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string), typeof(DateTimeToStringConverter), new PropertyMetadata("ddd, MMM d, yyyy HH:mm:ss"));

        public string Format
        {
            get => GetValue(FormatProperty) as string;
            set => SetValue(FormatProperty, value);
        }

        public override string Convert(DateTime value, object parameter, CultureInfo culture) => string.IsNullOrWhiteSpace(Format) ? value.ToString() : value.ToString(Format);
    }
}
