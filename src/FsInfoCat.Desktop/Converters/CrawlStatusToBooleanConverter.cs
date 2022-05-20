using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(Model.CrawlStatus), typeof(bool))]
    public sealed class CrawlStatusToBooleanConverter : ToValueConverterBase<Model.CrawlStatus, bool>
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(bool?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(null));

        public override bool? NullSource
        {
            get => (bool?)GetValue(NullSourceProperty);
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty NotRunningProperty = DependencyProperty.Register(nameof(NotRunning), typeof(bool?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(true));

        public bool? NotRunning
        {
            get => (bool?)GetValue(NotRunningProperty);
            set => SetValue(NotRunningProperty, value);
        }

        public static readonly DependencyProperty InProgressProperty = DependencyProperty.Register(nameof(InProgress), typeof(bool?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(true));

        public bool? InProgress
        {
            get => (bool?)GetValue(InProgressProperty);
            set => SetValue(InProgressProperty, value);
        }

        public static readonly DependencyProperty CompletedProperty = DependencyProperty.Register(nameof(Completed), typeof(bool?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(true));

        public bool? Completed
        {
            get => (bool?)GetValue(CompletedProperty);
            set => SetValue(CompletedProperty, value);
        }

        public static readonly DependencyProperty AllottedTimeElapsedProperty = DependencyProperty.Register(nameof(AllottedTimeElapsed),
            typeof(bool?), typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(true));

        public bool? AllottedTimeElapsed
        {
            get => (bool?)GetValue(AllottedTimeElapsedProperty);
            set => SetValue(AllottedTimeElapsedProperty, value);
        }

        public static readonly DependencyProperty MaxItemCountReachedProperty = DependencyProperty.Register(nameof(MaxItemCountReached),
            typeof(bool?), typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(true));

        public bool? MaxItemCountReached
        {
            get => (bool?)GetValue(MaxItemCountReachedProperty);
            set => SetValue(MaxItemCountReachedProperty, value);
        }

        public static readonly DependencyProperty CanceledProperty = DependencyProperty.Register(nameof(Canceled), typeof(bool?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(true));

        public bool? Canceled
        {
            get => (bool?)GetValue(CanceledProperty);
            set => SetValue(CanceledProperty, value);
        }

        public static readonly DependencyProperty FailedProperty = DependencyProperty.Register(nameof(Failed), typeof(bool?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(true));

        public bool? Failed
        {
            get => (bool?)GetValue(FailedProperty);
            set => SetValue(FailedProperty, value);
        }

        public static readonly DependencyProperty DisabledProperty = DependencyProperty.Register(nameof(Disabled), typeof(bool?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(false));

        public bool? Disabled
        {
            get => (bool?)GetValue(DisabledProperty);
            set => SetValue(DisabledProperty, value);
        }

        public override bool? Convert(Model.CrawlStatus value, object parameter, CultureInfo culture)
        {
            return value switch
            {
                Model.CrawlStatus.InProgress => InProgress,
                Model.CrawlStatus.Completed => Completed,
                Model.CrawlStatus.AllottedTimeElapsed => AllottedTimeElapsed,
                Model.CrawlStatus.MaxItemCountReached => MaxItemCountReached,
                Model.CrawlStatus.Canceled => Canceled,
                Model.CrawlStatus.Failed => Failed,
                Model.CrawlStatus.Disabled => Disabled,
                _ => NotRunning,
            };
        }
    }
}
