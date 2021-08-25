using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(object), typeof(int))]
    public class ObjectToInt32Converter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(int?), typeof(ObjectToInt32Converter), new PropertyMetadata(0));

        public int? NullSource
        {
            get => (int?)GetValue(NullSourceProperty);
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty NotNullSourceProperty = DependencyProperty.Register(nameof(NotNullSource), typeof(int?), typeof(ObjectToInt32Converter), new PropertyMetadata(1));

        public int? NotNullSource
        {
            get => (int?)GetValue(NotNullSourceProperty);
            set => SetValue(NotNullSourceProperty, value);
        }

        public int? Convert(object value) => (value == null) ? NullSource : NotNullSource;

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) => Convert(value);

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
