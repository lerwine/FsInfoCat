using FsInfoCat.Desktop.ViewModel;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    /// <summary>
    /// Converts values from <see cref="ICrawlConfigurationRow.RescheduleInterval"/>,  <see cref="ICrawlConfigurationRow.RescheduleFromJobEnd"/>, and <see cref="ICrawlConfigurationRow.RescheduleAfterFail"/> into a descriptive display string.
    /// </summary>
    public class CrawlConfigRescheduleDescriptionConverter : DependencyObject, IMultiValueConverter
    {
        #region FromLastJobEndAlwaysFormat Property Members

        /// <summary>
        /// Identifies the <see cref="FromLastJobEndAlwaysFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FromLastJobEndAlwaysFormatProperty = DependencyPropertyBuilder<CrawlConfigRescheduleDescriptionConverter, string>
            .Register(nameof(FromLastJobEndAlwaysFormat))
            .DefaultValue(null)
            .CoerseWith(NullIfWhiteSpaceOrTrimmedStringCoersion.Default)
            .AsReadWrite();

        /// <summary>
        /// Conversion result format to use when <see cref="ICrawlConfigurationRow.RescheduleInterval"/>,  and both <see cref="ICrawlConfigurationRow.RescheduleFromJobEnd"/> and <see cref="ICrawlConfigurationRow.RescheduleAfterFail"/>
        /// are <see langword="true"/>.
        /// </summary>
        /// <value>
        /// The format string used to create the conversion result when a long integer value was found in the conversion parameters, and the first two boolean parameters were <see langword="true"/>.
        /// </value>
        public string FromLastJobEndAlwaysFormat { get => GetValue(FromLastJobEndAlwaysFormatProperty) as string; set => SetValue(FromLastJobEndAlwaysFormatProperty, value); }

        #endregion
        #region FromLastJobEndExceptAfterFailFormat Property Members

        /// <summary>
        /// Identifies the <see cref="FromLastJobEndExceptAfterFailFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FromLastJobEndExceptAfterFailFormatProperty = DependencyPropertyBuilder<CrawlConfigRescheduleDescriptionConverter, string>
            .Register(nameof(FromLastJobEndExceptAfterFailFormat))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        /// <summary>
        /// Conversion result format to use when <see cref="ICrawlConfigurationRow.RescheduleInterval"/>, <see cref="ICrawlConfigurationRow.RescheduleFromJobEnd"/> is <see langword="true"/> and <see cref="ICrawlConfigurationRow.RescheduleAfterFail"/>
        /// is <see langword="false"/>.
        /// </summary>
        /// <value>
        /// The format string used to create the conversion result when a long integer value was found in the conversion parameters, the first boolean parameter is <see langword="true"/> and the last boolean parameter is <see langword="false"/>.
        /// </value>
        public string FromLastJobEndExceptAfterFailFormat { get => GetValue(FromLastJobEndExceptAfterFailFormatProperty) as string; set => SetValue(FromLastJobEndExceptAfterFailFormatProperty, value); }

        #endregion
        #region FromLastJobStartAlwaysFormat Property Members

        /// <summary>
        /// Identifies the <see cref="FromLastJobStartAlwaysFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FromLastJobStartAlwaysFormatProperty = DependencyPropertyBuilder<CrawlConfigRescheduleDescriptionConverter, string>
            .Register(nameof(FromLastJobStartAlwaysFormat))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        /// <summary>
        /// Conversion result format to use when <see cref="ICrawlConfigurationRow.RescheduleInterval"/>, <see cref="ICrawlConfigurationRow.RescheduleFromJobEnd"/> is <see langword="false"/> and <see cref="ICrawlConfigurationRow.RescheduleAfterFail"/>
        /// is <see langword="true"/>.
        /// </summary>
        /// <value>
        /// The format string used to create the conversion result when a long integer value was found in the conversion parameters, the first boolean parameter is <see langword="false"/> and the last boolean parameter is <see langword="true"/>.
        /// </value>
        public string FromLastJobStartAlwaysFormat { get => GetValue(FromLastJobStartAlwaysFormatProperty) as string; set => SetValue(FromLastJobStartAlwaysFormatProperty, value); }

        #endregion
        #region FromLastJobStartExceptAfterFailFormat Property Members

        /// <summary>
        /// Identifies the <see cref="FromLastJobStartExceptAfterFailFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FromLastJobStartExceptAfterFailFormatProperty = DependencyPropertyBuilder<CrawlConfigRescheduleDescriptionConverter, string>
            .Register(nameof(FromLastJobStartExceptAfterFailFormat))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        /// <summary>
        /// Conversion result format to use when <see cref="ICrawlConfigurationRow.RescheduleInterval"/> and both <see cref="ICrawlConfigurationRow.RescheduleFromJobEnd"/> and <see cref="ICrawlConfigurationRow.RescheduleAfterFail"/>
        /// are <see langword="false"/>.
        /// </summary>
        /// <value>
        /// The format string used to create the conversion result when a long integer value was found in the conversion parameters, neither of the first 2 boolean parameters were <see langword="true"/>.
        /// </value>
        public string FromLastJobStartExceptAfterFailFormat { get => GetValue(FromLastJobStartExceptAfterFailFormatProperty) as string; set => SetValue(FromLastJobStartExceptAfterFailFormatProperty, value); }

        #endregion
        #region ManualSchedulingOnly Property Members

        /// <summary>
        /// Identifies the <see cref="ManualSchedulingOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ManualSchedulingOnlyProperty = DependencyPropertyBuilder<CrawlConfigRescheduleDescriptionConverter, string>
            .Register(nameof(ManualSchedulingOnly))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        /// <summary>
        /// Conversion result value when <see cref="ICrawlConfigurationRow.RescheduleInterval"/> is null.
        /// </summary>
        /// <value>
        /// The conversion result string when no long integer value was found in the conversion parameters.
        /// </value>
        public string ManualSchedulingOnly { get => GetValue(ManualSchedulingOnlyProperty) as string; set => SetValue(ManualSchedulingOnlyProperty, value); }

        #endregion
        #region UseShortDurationString Property Members

        /// <summary>
        /// Identifies the <see cref="UseShortDurationString"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UseShortDurationStringProperty = DependencyPropertyBuilder<CrawlConfigRescheduleDescriptionConverter, bool>
            .Register(nameof(UseShortDurationString))
            .DefaultValue(false)
            .AsReadWrite();

        public bool UseShortDurationString { get => (bool)GetValue(UseShortDurationStringProperty); set => SetValue(UseShortDurationStringProperty, value); }

        #endregion
        public string Convert(long? rescheduleIntervalSeconds, bool rescheduleFromLastJobEnd, bool rescheduleAfterFail)
        {
            if (rescheduleIntervalSeconds.HasValue)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(rescheduleIntervalSeconds.Value);
                if (rescheduleFromLastJobEnd)
                {
                    if (rescheduleAfterFail)
                        return string.Format(FromLastJobEndAlwaysFormat, UseShortDurationString ? timeSpan.ToString("g") : TimeSpanToStringConverter.ToLongTimeSpanString(timeSpan));
                    return string.Format(FromLastJobEndExceptAfterFailFormat, UseShortDurationString ? timeSpan.ToString("g") : TimeSpanToStringConverter.ToLongTimeSpanString(timeSpan));
                }
                if (rescheduleAfterFail)
                    return string.Format(FromLastJobStartAlwaysFormat, UseShortDurationString ? timeSpan.ToString("g") : TimeSpanToStringConverter.ToLongTimeSpanString(timeSpan));
                return string.Format(FromLastJobStartExceptAfterFailFormat, UseShortDurationString ? timeSpan.ToString("g") : TimeSpanToStringConverter.ToLongTimeSpanString(timeSpan));
            }
            return ManualSchedulingOnly;
        }

        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => (values is null) ? Convert(null, false, false) :
            Convert(values.OfType<long>().Cast<long?>().FirstOrDefault(), values.OfType<bool>().DefaultIfEmpty(false).First(), values.OfType<bool>().Skip(1).DefaultIfEmpty(false).First());

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
