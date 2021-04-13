using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(RegexOptions), typeof(bool))]
    public class RegexOptionsToBooleanConverter : ToValueConverterBase<RegexOptions, bool>
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(bool?),
            typeof(RegexOptionsToBooleanConverter), new PropertyMetadata(null));

        public override bool? NullSource
        {
            get { return (bool?)(GetValue(NullSourceProperty)); }
            set { SetValue(NullSourceProperty, value); }
        }

        public static readonly DependencyProperty OptionValueProperty = DependencyProperty.Register(nameof(OptionValue), typeof(RegexOptions),
            typeof(RegexOptionsToBooleanConverter), new PropertyMetadata(RegexOptions.None));

        public RegexOptions OptionValue
        {
            get { return (RegexOptions)GetValue(OptionValueProperty); }
            set { SetValue(OptionValueProperty, value); }
        }

        public bool IfAny
        {
            get { return (bool)GetValue(IfAnyProperty); }
            set { SetValue(IfAnyProperty, value); }
        }

        public static readonly DependencyProperty IfAnyProperty = DependencyProperty.Register(nameof(IfAny), typeof(bool), typeof(RegexOptionsToBooleanConverter),
            new PropertyMetadata(true));


        public bool IfAll
        {
            get { return (bool)GetValue(IfAllProperty); }
            set { SetValue(IfAllProperty, value); }
        }

        public static readonly DependencyProperty IfAllProperty = DependencyProperty.Register(nameof(IfAll), typeof(bool), typeof(RegexOptionsToBooleanConverter),
            new PropertyMetadata(true));

        public bool IfNone
        {
            get { return (bool)GetValue(IfNoneProperty); }
            set { SetValue(IfNoneProperty, value); }
        }

        public static readonly DependencyProperty IfNoneProperty = DependencyProperty.Register(nameof(IfNone), typeof(bool), typeof(RegexOptionsToBooleanConverter),
            new PropertyMetadata(false));

        public override bool? Convert(RegexOptions value, object parameter, CultureInfo culture)
        {
            if (OptionValue == value)
                return IfAll;
            return (OptionValue != RegexOptions.None && value.HasFlag(OptionValue)) ? IfAny : IfNone;
        }
    }
}
