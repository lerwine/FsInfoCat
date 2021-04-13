using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DevHelperUI.Converters
{
    [ValueConversion(typeof(TextFormat), typeof(bool))]
    public class TextFormatToBooleanConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty IfNullProperty = DependencyProperty.Register(nameof(IfNull), typeof(bool?), typeof(TextFormatToBooleanConverter),
            new PropertyMetadata(null));

        public bool? IfNull
        {
            get { return (bool?)GetValue(IfNullProperty); }
            set { SetValue(IfNullProperty, value); }
        }

        public static readonly DependencyProperty TrueValueProperty = DependencyProperty.Register(nameof(TrueValue), typeof(TextFormat),
            typeof(TextFormatToBooleanConverter), new PropertyMetadata(TextFormat.Normal));

        public TextFormat TrueValue
        {
            get { return (TextFormat)GetValue(TrueValueProperty); }
            set { SetValue(TrueValueProperty, value); }
        }

        public static readonly DependencyProperty FalseValueProperty =
            DependencyProperty.Register(nameof(FalseValue), typeof(TextFormat?), typeof(TextFormatToBooleanConverter),
                new PropertyMetadata(null));

        public TextFormat? FalseValue
        {
            get { return (TextFormat?)GetValue(FalseValueProperty); }
            set { SetValue(FalseValueProperty, value); }
        }

        public bool? Convert(TextFormat? value) => value.HasValue ? value.Value == TrueValue : IfNull;

        public TextFormat? ConvertBack(bool? value, TextFormat? ifFalse = null) => (value.HasValue || (value = IfNull).HasValue || (value = IfNull).HasValue) ?
            (value.Value ? TrueValue : (ifFalse.HasValue ? ifFalse : FalseValue)) : null;

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value is bool b) ? b : Convert(value as TextFormat?);

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (value is TextFormat textFormat) ? textFormat :
            ConvertBack(value as bool?, parameter as TextFormat?);
    }
}
