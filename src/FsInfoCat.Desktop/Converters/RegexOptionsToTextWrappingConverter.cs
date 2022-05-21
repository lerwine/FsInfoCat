using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(RegexOptions), typeof(TextWrapping))]
    public class RegexOptionsToTextWrappingConverter : ToValueConverterBase<RegexOptions, TextWrapping>
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(TextWrapping?),
            typeof(RegexOptionsToTextWrappingConverter), new PropertyMetadata(null));

        public override TextWrapping? NullSource
        {
            get { return (TextWrapping?)GetValue(NullSourceProperty); }
            set { SetValue(NullSourceProperty, value); }
        }

        public static readonly DependencyProperty OptionValueProperty = DependencyProperty.Register(nameof(OptionValue), typeof(RegexOptions),
            typeof(RegexOptionsToTextWrappingConverter), new PropertyMetadata(RegexOptions.None));

        public RegexOptions OptionValue
        {
            get { return (RegexOptions)GetValue(OptionValueProperty); }
            set { SetValue(OptionValueProperty, value); }
        }

        public TextWrapping IfAny
        {
            get { return (TextWrapping)GetValue(IfAnyProperty); }
            set { SetValue(IfAnyProperty, value); }
        }

        public static readonly DependencyProperty IfAnyProperty = DependencyProperty.Register(nameof(IfAny), typeof(TextWrapping), typeof(RegexOptionsToTextWrappingConverter),
            new PropertyMetadata(TextWrapping.NoWrap));

        public TextWrapping IfAll
        {
            get { return (TextWrapping)GetValue(IfAllProperty); }
            set { SetValue(IfAllProperty, value); }
        }

        public static readonly DependencyProperty IfAllProperty = DependencyProperty.Register(nameof(IfAll), typeof(TextWrapping), typeof(RegexOptionsToTextWrappingConverter),
            new PropertyMetadata(TextWrapping.NoWrap));

        public TextWrapping IfNone
        {
            get { return (TextWrapping)GetValue(IfNoneProperty); }
            set { SetValue(IfNoneProperty, value); }
        }

        public static readonly DependencyProperty IfNoneProperty = DependencyProperty.Register(nameof(IfNone), typeof(TextWrapping), typeof(RegexOptionsToTextWrappingConverter),
            new PropertyMetadata(TextWrapping.Wrap));

        public override TextWrapping? Convert(RegexOptions value, object parameter, CultureInfo culture)
        {
            if (OptionValue == value)
                return IfAll;
            return (OptionValue != RegexOptions.None && value.HasFlag(OptionValue)) ? IfAny : IfNone;
        }
    }
}
