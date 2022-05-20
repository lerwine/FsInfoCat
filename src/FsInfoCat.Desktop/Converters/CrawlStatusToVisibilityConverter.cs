using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(Model.CrawlStatus), typeof(Visibility))]
    public sealed class CrawlStatusToVisibilityConverter : ToValueConverterBase<Model.CrawlStatus, Visibility>
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(Visibility?), typeof(CrawlStatusToVisibilityConverter), new PropertyMetadata(null));

        public override Visibility? NullSource { get => (Visibility?)GetValue(NullSourceProperty); set => SetValue(NullSourceProperty, value); }

        public static readonly DependencyProperty NotRunningProperty = DependencyProperty.Register(nameof(NotRunning), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Visible));

        public Visibility? NotRunning { get => (Visibility?)GetValue(NotRunningProperty); set => SetValue(NotRunningProperty, value); }

        public static readonly DependencyProperty InProgressProperty = DependencyProperty.Register(nameof(InProgress), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? InProgress { get => (Visibility?)GetValue(InProgressProperty); set => SetValue(InProgressProperty, value); }

        public static readonly DependencyProperty CompletedProperty = DependencyProperty.Register(nameof(Completed), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? Completed { get => (Visibility?)GetValue(CompletedProperty); set => SetValue(CompletedProperty, value); }

        public static readonly DependencyProperty AllottedTimeElapsedProperty = DependencyProperty.Register(nameof(AllottedTimeElapsed), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? AllottedTimeElapsed { get => (Visibility?)GetValue(AllottedTimeElapsedProperty); set => SetValue(AllottedTimeElapsedProperty, value); }

        public static readonly DependencyProperty MaxItemCountReachedProperty = DependencyProperty.Register(nameof(MaxItemCountReached), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? MaxItemCountReached { get => (Visibility?)GetValue(MaxItemCountReachedProperty); set => SetValue(MaxItemCountReachedProperty, value); }

        public static readonly DependencyProperty CanceledProperty = DependencyProperty.Register(nameof(Canceled), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? Canceled { get => (Visibility?)GetValue(CanceledProperty); set => SetValue(CanceledProperty, value); }

        public static readonly DependencyProperty FailedProperty = DependencyProperty.Register(nameof(Failed), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? Failed { get => (Visibility?)GetValue(FailedProperty); set => SetValue(FailedProperty, value); }

        public static readonly DependencyProperty DisabledProperty = DependencyProperty.Register(nameof(Disabled), typeof(Visibility?),
            typeof(CrawlStatusToBooleanConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility? Disabled { get => (Visibility?)GetValue(DisabledProperty); set => SetValue(DisabledProperty, value); }

        public override Visibility? Convert(Model.CrawlStatus value, object parameter, CultureInfo culture) => value switch
        {
            Model.CrawlStatus.AllottedTimeElapsed => AllottedTimeElapsed,
            Model.CrawlStatus.Canceled => Canceled,
            Model.CrawlStatus.Completed => Completed,
            Model.CrawlStatus.Disabled => Disabled,
            Model.CrawlStatus.Failed => Failed,
            Model.CrawlStatus.InProgress => InProgress,
            Model.CrawlStatus.MaxItemCountReached => MaxItemCountReached,
            _ => NotRunning,
        };
    }
}
