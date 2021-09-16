using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace FsInfoCat.Desktop.Converters
{
    public class RegexOptionsToScrollBarVisibilityConverter : ToValueConverterBase<RegexOptions, ScrollBarVisibility>
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(ScrollBarVisibility?),
            typeof(RegexOptionsToScrollBarVisibilityConverter), new PropertyMetadata(null));

        public override ScrollBarVisibility? NullSource
        {
            get { return (ScrollBarVisibility?)GetValue(NullSourceProperty); }
            set { SetValue(NullSourceProperty, value); }
        }

        public static readonly DependencyProperty OptionValueProperty = DependencyProperty.Register(nameof(OptionValue), typeof(RegexOptions),
            typeof(RegexOptionsToScrollBarVisibilityConverter), new PropertyMetadata(RegexOptions.None));

        public RegexOptions OptionValue
        {
            get { return (RegexOptions)GetValue(OptionValueProperty); }
            set { SetValue(OptionValueProperty, value); }
        }

        public ScrollBarVisibility IfAny
        {
            get { return (ScrollBarVisibility)GetValue(IfAnyProperty); }
            set { SetValue(IfAnyProperty, value); }
        }

        public static readonly DependencyProperty IfAnyProperty = DependencyProperty.Register(nameof(IfAny), typeof(ScrollBarVisibility), typeof(RegexOptionsToScrollBarVisibilityConverter),
            new PropertyMetadata(ScrollBarVisibility.Hidden));


        public ScrollBarVisibility IfAll
        {
            get { return (ScrollBarVisibility)GetValue(IfAllProperty); }
            set { SetValue(IfAllProperty, value); }
        }

        public static readonly DependencyProperty IfAllProperty = DependencyProperty.Register(nameof(IfAll), typeof(ScrollBarVisibility), typeof(RegexOptionsToScrollBarVisibilityConverter),
            new PropertyMetadata(ScrollBarVisibility.Hidden));

        public ScrollBarVisibility IfNone
        {
            get { return (ScrollBarVisibility)GetValue(IfNoneProperty); }
            set { SetValue(IfNoneProperty, value); }
        }

        public static readonly DependencyProperty IfNoneProperty = DependencyProperty.Register(nameof(IfNone), typeof(ScrollBarVisibility), typeof(RegexOptionsToScrollBarVisibilityConverter),
            new PropertyMetadata(ScrollBarVisibility.Auto));

        public override ScrollBarVisibility? Convert(RegexOptions value, object parameter, CultureInfo culture)
        {
            if (OptionValue == value)
                return IfAll;
            return (OptionValue != RegexOptions.None && value.HasFlag(OptionValue)) ? IfAny : IfNone;
        }
    }
}
