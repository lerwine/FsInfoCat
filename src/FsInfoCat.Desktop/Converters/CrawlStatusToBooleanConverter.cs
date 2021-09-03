using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(CrawlStatus), typeof(Visibility))]
    public sealed class CrawlStatusToVisibilityConverter : ToValueConverterBase<CrawlStatus, Visibility>
    {
        // TODO: Implement NullSource { get; set; }
        public override Visibility? NullSource { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


        public static readonly DependencyProperty NotRunningProperty = DependencyProperty.Register(nameof(NotRunning), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Visible));

        public Visibility? NotRunning
        {
            get => (Visibility?)GetValue(NotRunningProperty);
            set => SetValue(NotRunningProperty, value);
        }

        public static readonly DependencyProperty InProgressProperty = DependencyProperty.Register(nameof(InProgress), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? InProgress
        {
            get => (Visibility?)GetValue(InProgressProperty);
            set => SetValue(InProgressProperty, value);
        }

        public static readonly DependencyProperty CompletedProperty = DependencyProperty.Register(nameof(Completed), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? Completed
        {
            get => (Visibility?)GetValue(CompletedProperty);
            set => SetValue(CompletedProperty, value);
        }

        public static readonly DependencyProperty AllottedTimeElapsedProperty = DependencyProperty.Register(nameof(AllottedTimeElapsed), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? AllottedTimeElapsed
        {
            get => (Visibility?)GetValue(AllottedTimeElapsedProperty);
            set => SetValue(AllottedTimeElapsedProperty, value);
        }

        public static readonly DependencyProperty MaxItemCountReachedProperty = DependencyProperty.Register(nameof(MaxItemCountReached), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? MaxItemCountReached
        {
            get => (Visibility?)GetValue(MaxItemCountReachedProperty);
            set => SetValue(MaxItemCountReachedProperty, value);
        }

        public static readonly DependencyProperty CanceledProperty = DependencyProperty.Register(nameof(Canceled), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? Canceled
        {
            get => (Visibility?)GetValue(CanceledProperty);
            set => SetValue(CanceledProperty, value);
        }

        public static readonly DependencyProperty FailedProperty = DependencyProperty.Register(nameof(Failed), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? Failed
        {
            get => (Visibility?)GetValue(FailedProperty);
            set => SetValue(FailedProperty, value);
        }

        public static readonly DependencyProperty DisabledProperty = DependencyProperty.Register(nameof(Disabled), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? Disabled
        {
            get => (Visibility?)GetValue(DisabledProperty);
            set => SetValue(DisabledProperty, value);
        }

        public override Visibility? Convert(CrawlStatus value, object parameter, CultureInfo culture)
        {
            // TODO: Implement Convert(CrawlStatus, object, CultureInfo)
            throw new System.NotImplementedException();
        }
    }
    [ValueConversion(typeof(CrawlStatus), typeof(bool))]
    public sealed class CrawlStatusToBooleanConverter : ToValueConverterBase<CrawlStatus, bool>
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

        public override bool? Convert(CrawlStatus value, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case CrawlStatus.InProgress:
                    return InProgress;
                case CrawlStatus.Completed:
                    return Completed;
                case CrawlStatus.AllottedTimeElapsed:
                    return AllottedTimeElapsed;
                case CrawlStatus.MaxItemCountReached:
                    return MaxItemCountReached;
                case CrawlStatus.Canceled:
                    return Canceled;
                case CrawlStatus.Failed:
                    return Failed;
                case CrawlStatus.Disabled:
                    return Disabled;
                default:
                    return NotRunning;
            }
        }
    }
}
