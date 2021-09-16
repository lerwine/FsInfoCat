using FsInfoCat.Desktop.ViewModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(CrawlStatus), typeof(string))]
    public class CrawlStatusToStringConverter : SchemaEnumToStringComverter<CrawlStatus>
    {
        #region NotRunning Property Members

        /// <summary>
        /// Identifies the <see cref="NotRunning"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotRunningProperty = DependencyPropertyBuilder<CrawlStatusToStringConverter, string>
            .Register(nameof(NotRunning))
            .DefaultValue(null)
            .AsReadWrite();

        public string NotRunning { get => GetValue(NotRunningProperty) as string; set => SetValue(NotRunningProperty, value); }

        #endregion
        #region InProgress Property Members

        /// <summary>
        /// Identifies the <see cref="InProgress"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InProgressProperty = DependencyPropertyBuilder<CrawlStatusToStringConverter, string>
            .Register(nameof(InProgress))
            .DefaultValue(null)
            .AsReadWrite();

        public string InProgress { get => GetValue(InProgressProperty) as string; set => SetValue(InProgressProperty, value); }

        #endregion
        #region Completed Property Members

        /// <summary>
        /// Identifies the <see cref="Completed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompletedProperty = DependencyPropertyBuilder<CrawlStatusToStringConverter, string>
            .Register(nameof(Completed))
            .DefaultValue(null)
            .AsReadWrite();

        public string Completed { get => GetValue(CompletedProperty) as string; set => SetValue(CompletedProperty, value); }

        #endregion
        #region AllottedTimeElapsed Property Members

        /// <summary>
        /// Identifies the <see cref="AllottedTimeElapsed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllottedTimeElapsedProperty = DependencyPropertyBuilder<CrawlStatusToStringConverter, string>
            .Register(nameof(AllottedTimeElapsed))
            .DefaultValue(null)
            .AsReadWrite();

        public string AllottedTimeElapsed { get => GetValue(AllottedTimeElapsedProperty) as string; set => SetValue(AllottedTimeElapsedProperty, value); }

        #endregion
        #region MaxItemCountReached Property Members

        /// <summary>
        /// Identifies the <see cref="MaxItemCountReached"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxItemCountReachedProperty = DependencyPropertyBuilder<CrawlStatusToStringConverter, string>
            .Register(nameof(MaxItemCountReached))
            .DefaultValue(null)
            .AsReadWrite();

        public string MaxItemCountReached { get => GetValue(MaxItemCountReachedProperty) as string; set => SetValue(MaxItemCountReachedProperty, value); }

        #endregion
        #region Canceled Property Members

        /// <summary>
        /// Identifies the <see cref="Canceled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanceledProperty = DependencyPropertyBuilder<CrawlStatusToStringConverter, string>
            .Register(nameof(Canceled))
            .DefaultValue(null)
            .AsReadWrite();

        public string Canceled { get => GetValue(CanceledProperty) as string; set => SetValue(CanceledProperty, value); }

        #endregion
        #region Failed Property Members

        /// <summary>
        /// Identifies the <see cref="Failed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FailedProperty = DependencyPropertyBuilder<CrawlStatusToStringConverter, string>
            .Register(nameof(Failed))
            .DefaultValue(null)
            .AsReadWrite();

        public string Failed { get => GetValue(FailedProperty) as string; set => SetValue(FailedProperty, value); }

        #endregion
        #region Disabled Property Members

        /// <summary>
        /// Identifies the <see cref="Disabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisabledProperty = DependencyPropertyBuilder<CrawlStatusToStringConverter, string>
            .Register(nameof(Disabled))
            .DefaultValue(null)
            .AsReadWrite();

        public string Disabled { get => GetValue(DisabledProperty) as string; set => SetValue(DisabledProperty, value); }

        #endregion

        public override string Convert(CrawlStatus value, object parameter, CultureInfo culture)
        {
            DependencyProperty property = value switch
            {
                CrawlStatus.InProgress => InProgressProperty,
                CrawlStatus.Completed => CompletedProperty,
                CrawlStatus.AllottedTimeElapsed => AllottedTimeElapsedProperty,
                CrawlStatus.MaxItemCountReached => MaxItemCountReachedProperty,
                CrawlStatus.Canceled => CanceledProperty,
                CrawlStatus.Failed => FailedProperty,
                CrawlStatus.Disabled => DisabledProperty,
                _ => NotRunningProperty,
            };

            return (ReadLocalValue(property) == DependencyProperty.UnsetValue) ? base.Convert(value, parameter, culture) : GetValue(property) as string;
        }
    }
}
