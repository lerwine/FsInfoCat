using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public class CrawlConfigFilter : DependencyObject, IFilter
    {
        private readonly DataErrorInfo _errorInfo;

        /// <summary>
        /// Occurs when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler HasErrorsPropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #region HasErrors Property Members

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<CrawlConfigFilter, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as CrawlConfigFilter)?.RaiseHasErrorsPropertyChanged(e))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        public bool HasErrors { get => (bool)GetValue(HasErrorsProperty); private set => SetValue(HasErrorsPropertyKey, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="HasErrorsProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="HasErrorsProperty"/> that tracks changes to its effective value.</param>
        protected void RaiseHasErrorsPropertyChanged(DependencyPropertyChangedEventArgs args) => HasErrorsPropertyChanged?.Invoke(this, args);

        #endregion
        #region CrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="CrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlEndProperty = DependencyPropertyBuilder<CrawlConfigFilter, Historical.Range>
            .Register(nameof(CrawlEnd))
            .DefaultValue(null)
            .AsReadWrite();

        public Historical.Range CrawlEnd { get => (Historical.Range)GetValue(CrawlEndProperty); set => SetValue(CrawlEndProperty, value); }

        #endregion
        #region NextCrawlStart Property Members

        /// <summary>
        /// Identifies the <see cref="NextCrawlStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextCrawlStartProperty = DependencyPropertyBuilder<CrawlConfigFilter, Scheduled.Range>
            .Register(nameof(NextCrawlStart))
            .DefaultValue(null)
            .AsReadWrite();

        public Scheduled.Range NextCrawlStart { get => (Scheduled.Range)GetValue(NextCrawlStartProperty); set => SetValue(NextCrawlStartProperty, value); }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = DependencyPropertyBuilder<CrawlConfigFilter, ObservableCollection<CrawlStatus>>
            .Register(nameof(Status))
            .DefaultValue(null)
            .AsReadWrite();

        public ObservableCollection<CrawlStatus> Status { get => (ObservableCollection<CrawlStatus>)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        #endregion
        #region AverageDuration Property Members

        /// <summary>
        /// Identifies the <see cref="AverageDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AverageDurationProperty = DependencyPropertyBuilder<CrawlConfigFilter, DurationRange>
            .Register(nameof(AverageDuration))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as CrawlConfigFilter)?.OnAverageDurationPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DurationRange AverageDuration { get => (DurationRange)GetValue(AverageDurationProperty); set => SetValue(AverageDurationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AverageDuration"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AverageDuration"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AverageDuration"/> property.</param>
        protected virtual void OnAverageDurationPropertyChanged(DurationRange oldValue, DurationRange newValue)
        {
            // TODO: Implement OnAverageDurationPropertyChanged Logic
        }

        #endregion
        #region MaxDuration Property Members

        /// <summary>
        /// Identifies the <see cref="MaxDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxDurationProperty = DependencyPropertyBuilder<CrawlConfigFilter, DurationRange>
            .Register(nameof(MaxDuration))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as CrawlConfigFilter)?.OnMaxDurationPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DurationRange MaxDuration { get => (DurationRange)GetValue(MaxDurationProperty); set => SetValue(MaxDurationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaxDuration"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaxDuration"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaxDuration"/> property.</param>
        protected virtual void OnMaxDurationPropertyChanged(DurationRange oldValue, DurationRange newValue)
        {
            // TODO: Implement OnMaxDurationPropertyChanged Logic
        }

        #endregion
        #region HasCancel Property Members

        /// <summary>
        /// Identifies the <see cref="HasCancel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasCancelProperty = DependencyPropertyBuilder<CrawlConfigFilter, bool?>
            .Register(nameof(HasCancel))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? HasCancel { get => (bool?)GetValue(HasCancelProperty); set => SetValue(HasCancelProperty, value); }

        #endregion
        #region HasFail Property Members

        /// <summary>
        /// Identifies the <see cref="HasFail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasFailProperty = DependencyPropertyBuilder<CrawlConfigFilter, bool?>
            .Register(nameof(HasFail))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? HasFail { get => (bool?)GetValue(HasFailProperty); set => SetValue(HasFailProperty, value); }

        #endregion
        #region AnyReachedItemLimit Property Members

        /// <summary>
        /// Identifies the <see cref="AnyReachedItemLimit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnyReachedItemLimitProperty = DependencyPropertyBuilder<CrawlConfigFilter, bool?>
            .Register(nameof(AnyReachedItemLimit))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? AnyReachedItemLimit { get => (bool?)GetValue(AnyReachedItemLimitProperty); set => SetValue(AnyReachedItemLimitProperty, value); }

        #endregion
        #region AnySucceeded Property Members

        /// <summary>
        /// Identifies the <see cref="AnySucceeded"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnySucceededProperty = DependencyPropertyBuilder<CrawlConfigFilter, bool?>
            .Register(nameof(AnySucceeded))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? AnySucceeded { get => (bool?)GetValue(AnySucceededProperty); set => SetValue(AnySucceededProperty, value); }

        #endregion
        #region AnyTimedOut Property Members

        /// <summary>
        /// Identifies the <see cref="AnyTimedOut"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnyTimedOutProperty = DependencyPropertyBuilder<CrawlConfigFilter, bool?>
            .Register(nameof(AnyTimedOut))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? AnyTimedOut { get => (bool?)GetValue(AnyTimedOutProperty); set => SetValue(AnyTimedOutProperty, value); }

        #endregion

        public CrawlConfigFilter()
        {
            _errorInfo = new();
            _errorInfo.ErrorsChanged += ErrorInfo_ErrorsChanged;
        }

        internal static bool AreSame(CrawlConfigFilter x, CrawlConfigFilter y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            return Historical.Range.AreSame(x.CrawlEnd, y.CrawlEnd) && Scheduled.Range.AreSame(x.NextCrawlStart, y.NextCrawlStart) && DurationRange.AreSame(x.AverageDuration, y.AverageDuration) && DurationRange.AreSame(x.MaxDuration, y.MaxDuration) &&
                x.HasCancel == y.HasCancel && x.HasFail == y.HasFail && x.AnyReachedItemLimit == y.AnyReachedItemLimit & x.AnySucceeded == y.AnySucceeded && x.AnyTimedOut == y.AnyTimedOut &&
                (x.Status?.Distinct().OrderBy(e => e) ?? Enumerable.Empty<CrawlStatus>()).SequenceEqual(y.Status?.Distinct().OrderBy(e => e) ?? Enumerable.Empty<CrawlStatus>());
        }

        public bool IsMatch(ICrawlConfigReportItem item)
        {
            if (item is null)
                return false;
            Historical.Range crawlEnd = CrawlEnd;
            Scheduled.Range nextStart = NextCrawlStart;
            IEnumerable<CrawlStatus> crawlStatus;
            DurationRange dr;
            bool? b;
            return (crawlEnd is null || crawlEnd.IsValid(item.LastCrawlEnd)) && (nextStart is null || nextStart.IsValid(item.NextScheduledStart)) &&
                (!(crawlStatus = Status ?? Enumerable.Empty<CrawlStatus>()).Any() || crawlStatus.Contains(item.StatusValue)) && ((dr = AverageDuration) is null || dr.IsInRange(item.AverageDuration)) &&
                ((dr = MaxDuration) is null || dr.IsInRange(item.MaxDuration)) && (!(b = HasCancel).HasValue || b.Value == item.CanceledCount > 0) && (!(b = HasFail).HasValue || b.Value == item.FailedCount > 0) &&
                (!(b = AnyReachedItemLimit).HasValue || b.Value == item.ItemLimitReachedCount > 0) && (!(b = AnySucceeded).HasValue || b.Value == item.SucceededCount > 0) &&
                (!(b = AnyTimedOut).HasValue || b.Value == item.TimedOutCount > 0);
        }

        private void ErrorInfo_ErrorsChanged(object sender, DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

        public IEnumerable GetErrors(string propertyName) => _errorInfo.GetErrors(propertyName);
    }
}
