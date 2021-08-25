using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(object), typeof(double))]
    public class ObjectToDoubleConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(double?), typeof(ObjectToDoubleConverter), new PropertyMetadata(0.0));

        public double? NullSource
        {
            get => (double?)GetValue(NullSourceProperty);
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty NotNullSourceProperty = DependencyProperty.Register(nameof(NotNullSource), typeof(double?), typeof(ObjectToDoubleConverter), new PropertyMetadata(1.0));

        public double? NotNullSource
        {
            get => (double?)GetValue(NotNullSourceProperty);
            set => SetValue(NotNullSourceProperty, value);
        }

        public double? Convert(object value) => (value == null) ? NullSource : NotNullSource;

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) => Convert(value);

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
