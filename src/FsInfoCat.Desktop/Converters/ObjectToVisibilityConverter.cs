using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class ObjectToVisibilityConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(Visibility?), typeof(ObjectToVisibilityConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? NullSource
        {
            get => (Visibility?)GetValue(NullSourceProperty);
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty NotNullSourceProperty = DependencyProperty.Register(nameof(NotNullSource), typeof(Visibility?), typeof(ObjectToVisibilityConverter), new PropertyMetadata(Visibility.Visible));

        public Visibility? NotNullSource
        {
            get => (Visibility?)GetValue(NotNullSourceProperty);
            set => SetValue(NotNullSourceProperty, value);
        }

        public Visibility? Convert(object value) => (value == null) ? NullSource : NotNullSource;

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) => Convert(value);

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
