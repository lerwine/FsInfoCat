using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters.Local
{
    [ValueConversion(typeof(Desktop.LocalData.CrawlConfigurations.ListItemViewModel), typeof(string))]
    public class CrawlConfigurationToAutoRescheduleString : ToClassConverterBase<Desktop.LocalData.CrawlConfigurations.ListItemViewModel, string>
    {

        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(CrawlConfigurationToAutoRescheduleString), new PropertyMetadata(""));

        public override string NullSource
        {
            get => GetValue(NullSourceProperty) as string;
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty ShortProperty = DependencyProperty.Register(nameof(Short), typeof(bool), typeof(CrawlConfigurationToAutoRescheduleString), new PropertyMetadata(false));

        public bool Short
        {
            get => (bool)GetValue(ShortProperty);
            set => SetValue(ShortProperty, value);
        }

        public override string Convert(Desktop.LocalData.CrawlConfigurations.ListItemViewModel value, object parameter, CultureInfo culture)
        {
            if (value.RescheduleInterval.HasValue)
            {
                if (Short)
                {
                    if (value.RescheduleFromJobEnd)
                    {
                        if (value.RescheduleAfterFail)
                            return $"{value.RescheduleInterval.Value:g} after end";
                        return $"{value.RescheduleInterval.Value:g} after end unless fail";
                    }
                    if (value.RescheduleAfterFail)
                        return $"{value.RescheduleInterval.Value:g} after last start";
                    return $"{value.RescheduleInterval.Value:g} after last start unless fail";
                }

                if (value.RescheduleFromJobEnd)
                {
                    if (value.RescheduleAfterFail)
                        return $"{TimeSpanToStringConverter.ToLongTimeSpanString(value.RescheduleInterval.Value)} after last completion time.";
                    return $"{TimeSpanToStringConverter.ToLongTimeSpanString(value.RescheduleInterval.Value)} after last completion time unless last crawl failed.";
                }
                if (value.RescheduleAfterFail)
                    return $"{TimeSpanToStringConverter.ToLongTimeSpanString(value.RescheduleInterval.Value)} after last scheduled crawl start.";
                return $"{TimeSpanToStringConverter.ToLongTimeSpanString(value.RescheduleInterval.Value)} after last scheduled crawl start unless last crawl failed.";
            }
            return Short ? "None" : "No auto-reschedule";
        }
    }
}
