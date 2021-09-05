using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToStringConverter : ToClassConverterBase<bool, string>
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(BooleanToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue as string) ?? ""
        });

        public override string NullSource
        {
            get => GetValue(NullSourceProperty) as string;
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty TrueValueProperty = DependencyProperty.Register(nameof(TrueValue), typeof(string), typeof(BooleanToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue is string s) ? s : FsInfoCat.Properties.Resources.DisplayName_Yes
        });

        public string TrueValue
        {
            get => GetValue(TrueValueProperty) as string;
            set => SetValue(TrueValueProperty, value);
        }

        public static readonly DependencyProperty FalseValueProperty = DependencyProperty.Register(nameof(FalseValue), typeof(string), typeof(BooleanToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue is string s) ? s : FsInfoCat.Properties.Resources.DisplayName_No
        });

        public string FalseValue
        {
            get => GetValue(FalseValueProperty) as string;
            set => SetValue(FalseValueProperty, value);
        }

        public static string Convert(bool? value, string trueValue = null, string falseValue = null, string nullValue = "")
        {
            if (value.HasValue)
                return value.Value ? (trueValue ?? FsInfoCat.Properties.Resources.DisplayName_Yes) : (falseValue ?? FsInfoCat.Properties.Resources.DisplayName_No);
            return nullValue;
        }

        public override string Convert(bool value, object parameter, CultureInfo culture) => value ? TrueValue : FalseValue;

        protected override object ConvertBack(string target, object parameter, CultureInfo culture) => (target == NullSource) ? null : target == TrueValue ? true : target == FalseValue ? false : target;
    }
}
