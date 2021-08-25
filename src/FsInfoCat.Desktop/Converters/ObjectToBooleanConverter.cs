using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class ObjectToBooleanConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(bool?), typeof(ObjectToBooleanConverter), new PropertyMetadata(false));

        public bool? NullSource
        {
            get => (bool?)GetValue(NullSourceProperty);
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty NotNullSourceProperty = DependencyProperty.Register(nameof(NotNullSource), typeof(bool?), typeof(ObjectToBooleanConverter), new PropertyMetadata(true));

        public bool? NotNullSource
        {
            get => (bool?)GetValue(NotNullSourceProperty);
            set => SetValue(NotNullSourceProperty, value);
        }

        public bool? Convert(object value) => (value == null) ? NullSource : NotNullSource;

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) => Convert(value);

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
