using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public interface ITimeRange
    {
        ITime Start { get; }
        ITime End { get; }
    }
    public interface IExtendableTime : IRelativeTime
    {
        bool IsInTheFuture { get; }
    }
    public interface ITime : IComparable<DateTime>
    {
    }
    public interface IAbsoluteTime : ITime
    {
        DateTime Value { get; }
    }
    public interface IRelativeTime : ITime
    {
        int? Days { get; }
        int? Hours { get; }
    }
    public interface IRelativeTimeMonthly : IRelativeTime
    {
        int? Years { get; }
        int? Months { get; }
    }
    public interface IExtendableTimeMonthly : IRelativeTimeMonthly, IExtendableTime
    {
    }
    public interface IRelativeTimeWeekly : IRelativeTime
    {
        int? Weeks { get; }
    }
    public interface IExtendableTimeWeekly : IRelativeTimeWeekly, IExtendableTime
    {
        int? Weeks { get; }
    }
    public interface IRelativeNextCrawl
    {
        /// <summary>
        /// Start of range relative to days in past or future.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        int? From { get; }
        /// <summary>
        /// End of range relative to days in past or future.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        int? To { get; }

    }
    public class ReportOptionItem : DependencyObject
    {
        #region DisplayText Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayTextProperty = DependencyPropertyBuilder<ReportOptionItem, string>
            .Register(nameof(DisplayText))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string DisplayText { get => GetValue(DisplayTextProperty) as string; set => SetValue(DisplayTextProperty, value); }

        #endregion
        #region StatusOptions Property Members

        /// <summary>
        /// Identifies the <see cref="StatusOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusOptionsProperty = DependencyPropertyBuilder<ReportOptionItem, ObservableCollection<CrawlStatus>>
            .Register(nameof(StatusOptions))
            .DefaultValue(null)
            .AsReadWrite();

        public ObservableCollection<CrawlStatus> StatusOptions { get => (ObservableCollection<CrawlStatus>)GetValue(StatusOptionsProperty); set => SetValue(StatusOptionsProperty, value); }

        #endregion
        #region StatusNotEquals Property Members

        /// <summary>
        /// Identifies the <see cref="StatusNotEquals"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusNotEqualsProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(StatusNotEquals))
            .DefaultValue(false)
            .AsReadWrite();

        public bool StatusNotEquals { get => (bool)GetValue(StatusNotEqualsProperty); set => SetValue(StatusNotEqualsProperty, value); }

        #endregion
        #region IncludeMinLastCrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeMinLastCrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeMinLastCrawlEndProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncludeMinLastCrawlEnd))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeMinLastCrawlEnd { get => (bool)GetValue(IncludeMinLastCrawlEndProperty); set => SetValue(IncludeMinLastCrawlEndProperty, value); }

        #endregion
        #region IncludeLastCrawlEndLimit Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeLastCrawlEndLimit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeLastCrawlEndLimitProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncludeLastCrawlEndLimit))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeLastCrawlEndLimit { get => (bool)GetValue(IncludeLastCrawlEndLimitProperty); set => SetValue(IncludeLastCrawlEndLimitProperty, value); }

        #endregion
        #region RelativeLastCrawlEndDays Property Members

        /// <summary>
        /// Identifies the <see cref="RelativeLastCrawlEndDays"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeLastCrawlEndDaysProperty = DependencyPropertyBuilder<ReportOptionItem, int?>
            .Register(nameof(RelativeLastCrawlEndDays))
            .DefaultValue(null)
            .AsReadWrite();

        public int? RelativeLastCrawlEndDays { get => (int?)GetValue(RelativeLastCrawlEndDaysProperty); set => SetValue(RelativeLastCrawlEndDaysProperty, value); }

        #endregion
        #region UserSuppliedRelativeLastCrawlEndDays Property Members

        /// <summary>
        /// Identifies the <see cref="UserSuppliedRelativeLastCrawlEndDays"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UserSuppliedRelativeLastCrawlEndDaysProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(UserSuppliedRelativeLastCrawlEndDays))
            .DefaultValue(false)
            .AsReadWrite();

        public bool UserSuppliedRelativeLastCrawlEndDays { get => (bool)GetValue(UserSuppliedRelativeLastCrawlEndDaysProperty); set => SetValue(UserSuppliedRelativeLastCrawlEndDaysProperty, value); }

        #endregion
        #region HasLastCrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="HasLastCrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasLastCrawlEndProperty = DependencyPropertyBuilder<ReportOptionItem, bool?>
            .Register(nameof(HasLastCrawlEnd))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? HasLastCrawlEnd { get => (bool?)GetValue(HasLastCrawlEndProperty); set => SetValue(HasLastCrawlEndProperty, value); }

        #endregion
        #region RelativeLastCrawlEndIsBefore Property Members

        /// <summary>
        /// Identifies the <see cref="RelativeLastCrawlEndIsBefore"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeLastCrawlEndIsBeforeProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(RelativeLastCrawlEndIsBefore))
            .DefaultValue(false)
            .AsReadWrite();

        public bool RelativeLastCrawlEndIsBefore { get => (bool)GetValue(RelativeLastCrawlEndIsBeforeProperty); set => SetValue(RelativeLastCrawlEndIsBeforeProperty, value); }

        #endregion
        #region IncludeMinNextScheduledStart Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeMinNextScheduledStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeMinNextScheduledStartProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncludeMinNextScheduledStart))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeMinNextScheduledStart { get => (bool)GetValue(IncludeMinNextScheduledStartProperty); set => SetValue(IncludeMinNextScheduledStartProperty, value); }

        #endregion
        #region IncludeNextScheduledStartLimit Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeNextScheduledStartLimit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeNextScheduledStartLimitProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncludeNextScheduledStartLimit))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeNextScheduledStartLimit { get => (bool)GetValue(IncludeNextScheduledStartLimitProperty); set => SetValue(IncludeNextScheduledStartLimitProperty, value); }

        #endregion
        #region UserSuppliedRelativeNextScheduledStartDays Property Members

        /// <summary>
        /// Identifies the <see cref="UserSuppliedRelativeNextScheduledStartDays"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UserSuppliedRelativeNextScheduledStartDaysProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(UserSuppliedRelativeNextScheduledStartDays))
            .DefaultValue(false)
            .AsReadWrite();

        public bool UserSuppliedRelativeNextScheduledStartDays { get => (bool)GetValue(UserSuppliedRelativeNextScheduledStartDaysProperty); set => SetValue(UserSuppliedRelativeNextScheduledStartDaysProperty, value); }

        #endregion
        #region RelativeNextScheduledStartDays Property Members

        /// <summary>
        /// Identifies the <see cref="RelativeNextScheduledStartDays"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeNextScheduledStartDaysProperty = DependencyPropertyBuilder<ReportOptionItem, int?>
            .Register(nameof(RelativeNextScheduledStartDays))
            .DefaultValue(null)
            .AsReadWrite();

        public int? RelativeNextScheduledStartDays { get => (int?)GetValue(RelativeNextScheduledStartDaysProperty); set => SetValue(RelativeNextScheduledStartDaysProperty, value); }

        #endregion
        #region RelativeNextScheduledStartIsBefore Property Members

        /// <summary>
        /// Identifies the <see cref="RelativeNextScheduledStartIsBefore"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeNextScheduledStartIsBeforeProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(RelativeNextScheduledStartIsBefore))
            .DefaultValue(false)
            .AsReadWrite();

        public bool RelativeNextScheduledStartIsBefore { get => (bool)GetValue(RelativeNextScheduledStartIsBeforeProperty); set => SetValue(RelativeNextScheduledStartIsBeforeProperty, value); }

        #endregion
        #region NextScheduledStartDaysOverdue Property Members

        /// <summary>
        /// Identifies the <see cref="NextScheduledStartDaysOverdue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextScheduledStartDaysOverdueProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(NextScheduledStartDaysOverdue))
            .DefaultValue(false)
            .AsReadWrite();

        public bool NextScheduledStartDaysOverdue { get => (bool)GetValue(NextScheduledStartDaysOverdueProperty); set => SetValue(NextScheduledStartDaysOverdueProperty, value); }

        #endregion
        #region IsScheduled Property Members

        /// <summary>
        /// Identifies the <see cref="IsScheduled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsScheduledProperty = DependencyPropertyBuilder<ReportOptionItem, bool?>
            .Register(nameof(IsScheduled))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? IsScheduled { get => (bool?)GetValue(IsScheduledProperty); set => SetValue(IsScheduledProperty, value); }

        #endregion
        #region IncludeFileSystemTypeSelection Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeFileSystemTypeSelection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeFileSystemTypeSelectionProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncludeFileSystemTypeSelection))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeFileSystemTypeSelection { get => (bool)GetValue(IncludeFileSystemTypeSelectionProperty); set => SetValue(IncludeFileSystemTypeSelectionProperty, value); }

        #endregion
        #region IncludeVolumeSelection Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeVolumeSelection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeVolumeSelectionProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncludeVolumeSelection))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeVolumeSelection { get => (bool)GetValue(IncludeVolumeSelectionProperty); set => SetValue(IncludeVolumeSelectionProperty, value); }

        #endregion
        #region HasSucceededCount Property Members

        /// <summary>
        /// Identifies the <see cref="HasSucceededCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasSucceededCountProperty = DependencyPropertyBuilder<ReportOptionItem, bool?>
            .Register(nameof(HasSucceededCount))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? HasSucceededCount { get => (bool?)GetValue(HasSucceededCountProperty); set => SetValue(HasSucceededCountProperty, value); }

        #endregion
        #region HasTimedOutCount Property Members

        /// <summary>
        /// Identifies the <see cref="HasTimedOutCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasTimedOutCountProperty = DependencyPropertyBuilder<ReportOptionItem, bool?>
            .Register(nameof(HasTimedOutCount))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? HasTimedOutCount { get => (bool?)GetValue(HasTimedOutCountProperty); set => SetValue(HasTimedOutCountProperty, value); }

        #endregion
        #region HasItemLimitReachedCount Property Members

        /// <summary>
        /// Identifies the <see cref="HasItemLimitReachedCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasItemLimitReachedCountProperty = DependencyPropertyBuilder<ReportOptionItem, bool?>
            .Register(nameof(HasItemLimitReachedCount))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? HasItemLimitReachedCount { get => (bool?)GetValue(HasItemLimitReachedCountProperty); set => SetValue(HasItemLimitReachedCountProperty, value); }

        #endregion
        #region HasCanceledCount Property Members

        /// <summary>
        /// Identifies the <see cref="HasCanceledCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasCanceledCountProperty = DependencyPropertyBuilder<ReportOptionItem, bool?>
            .Register(nameof(HasCanceledCount))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? HasCanceledCount { get => (bool?)GetValue(HasCanceledCountProperty); set => SetValue(HasCanceledCountProperty, value); }

        #endregion
        #region HasFailedCount Property Members

        /// <summary>
        /// Identifies the <see cref="HasFailedCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasFailedCountProperty = DependencyPropertyBuilder<ReportOptionItem, bool?>
            .Register(nameof(HasFailedCount))
            .DefaultValue(null)
            .AsReadWrite();

        public bool? HasFailedCount { get => (bool?)GetValue(HasFailedCountProperty); set => SetValue(HasFailedCountProperty, value); }

        #endregion
        #region IncludeMinAverageDuration Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeMinAverageDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeMinAverageDurationProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncludeMinAverageDuration))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeMinAverageDuration { get => (bool)GetValue(IncludeMinAverageDurationProperty); set => SetValue(IncludeMinAverageDurationProperty, value); }

        #endregion
        #region IncudeAverageDurationLimit Property Members

        /// <summary>
        /// Identifies the <see cref="IncudeAverageDurationLimit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncudeAverageDurationLimitProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncudeAverageDurationLimit))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncudeAverageDurationLimit { get => (bool)GetValue(IncudeAverageDurationLimitProperty); set => SetValue(IncudeAverageDurationLimitProperty, value); }

        #endregion
        #region IncludeMinMaxDuration Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeMinMaxDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeMinMaxDurationProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncludeMinMaxDuration))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeMinMaxDuration { get => (bool)GetValue(IncludeMinMaxDurationProperty); set => SetValue(IncludeMinMaxDurationProperty, value); }

        #endregion
        #region IncludeMaxDurationLimit Property Members

        /// <summary>
        /// Identifies the <see cref="IncludeMaxDurationLimit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IncludeMaxDurationLimitProperty = DependencyPropertyBuilder<ReportOptionItem, bool>
            .Register(nameof(IncludeMaxDurationLimit))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IncludeMaxDurationLimit { get => (bool)GetValue(IncludeMaxDurationLimitProperty); set => SetValue(IncludeMaxDurationLimitProperty, value); }

        #endregion

        internal bool IsMatch(CrawlConfigReportItem entity, CrawlStateReportViewModel owner)
        {
            if (entity is null)
                return false;
            ObservableCollection<CrawlStatus> statusOptions = StatusOptions;
            if (statusOptions is not null && statusOptions.Count > 0 && statusOptions.Contains(entity.StatusValue) == StatusNotEquals)
                return false;

            DateTime? dtX = entity.LastCrawlEnd;
            bool? req = (IncludeMinLastCrawlEnd || IncludeLastCrawlEndLimit) ? HasLastCrawlEnd.Value : HasLastCrawlEnd;
            if (dtX.HasValue)
            {
                if (req.HasValue && !req.Value)
                    return false;
                DateTime? dtY = IncludeMinLastCrawlEnd ? owner.MinLastCrawlEnd?.Date : null;
                if (dtY.HasValue && dtX.Value < dtY.Value)
                    return false;
                dtY = IncludeLastCrawlEndLimit ? owner.LastCrawlEndLimit?.Date : null;
                if (dtY.HasValue && dtX.Value > dtY.Value)
                    return false;
                int? days = UserSuppliedRelativeLastCrawlEndDays ? RelativeLastCrawlEndDays : RelativeLastCrawlEndDays;
                if (days.HasValue)
                {
                    if (UserSuppliedRelativeLastCrawlEndDays ? NextScheduledStartDaysOverdue : owner.NextScheduledStartDaysOverdue)
                }
                if (days.HasValue && ((UserSuppliedRelativeLastCrawlEndDays ? owner.RelativeLastCrawlEndIsBefore : RelativeLastCrawlEndIsBefore) ?
                    DateTime.Today.AddDays((UserSuppliedRelativeLastCrawlEndDays ? NextScheduledStartDaysOverdue : owner.NextScheduledStartDaysOverdue) ? 0 - days.Value : days.Value) > dtX.Value :
                    DateTime.Today.AddDays((UserSuppliedRelativeLastCrawlEndDays ? NextScheduledStartDaysOverdue : owner.NextScheduledStartDaysOverdue) ? 0 - days.Value : days.Value) < dtX.Value))
                    return false;
            }
            else if (req.HasValue && req.Value)
                return false;
            req = (IncludeMinNextScheduledStart || IncludeNextScheduledStartLimit) ? owner.IsScheduled.Value : IsScheduled;
            if ((dtX = entity.NextScheduledStart).HasValue)
            {
                if (req.HasValue && !req.Value)
                    return false;
                DateTime? dtY = IncludeMinNextScheduledStart ? owner.MinNextScheduledStart?.Date : null;
                if (dtY.HasValue && dtX.Value < dtY.Value)
                    return false;
                dtY = IncludeNextScheduledStartLimit ? owner.NextScheduledStartLimit?.Date : null;
                if (dtY.HasValue && dtX.Value > dtY.Value)
                    return false;
                int? days = UserSuppliedRelativeNextScheduledStartDays ? owner.RelativeNextScheduledStartDays : RelativeNextScheduledStartDays;
                if (days.HasValue && ((UserSuppliedRelativeLastCrawlEndDays ? owner.RelativeLastCrawlEndIsBefore : RelativeLastCrawlEndIsBefore) ? DateTime.Today.AddDays(0 - days.Value) > dtX.Value : DateTime.Today.AddDays(0 - days.Value) < dtX.Value))
                    return false;
            }
            else if (req.HasValue && req.Value)
                return false;
            dateTimeX = IncludeLastCrawlEndLimit ? owner.MinNextScheduledStart?.Date : null;
            if (dateTimeX.HasValue && (dateTimeY = entity.NextScheduledStart).HasValue && dateTimeY.Value > dateTimeX.Value)
                return false;
            int? relativeDays = UserSuppliedRelativeLastCrawlEndDays ? owner.RelativeLastCrawlEndDays : RelativeLastCrawlEndDays;
            if (relativeDays.HasValue && (dateTimeX = entity.LastCrawlEnd).HasValue
            dateTimeX = IncludeMinNextScheduledStart ? owner.MinNextScheduledStart?.Date : null;
            if (dateTimeX.HasValue && (dateTimeY = entity.NextScheduledStart).HasValue && dateTimeY.Value < dateTimeX.Value)
                return false;
            dateTimeX = IncludeNextScheduledStartLimit ? owner.MinNextScheduledStart?.Date : null;
            if (dateTimeX.HasValue && (dateTimeY = entity.NextScheduledStart).HasValue && dateTimeY.Value > dateTimeX.Value)
                return false;
            throw new NotImplementedException();
        }

        internal bool IsSameAs(ReportOptionItem other)
        {
            throw new NotImplementedException();
        }
    }
    public class CrawlStateReportViewModel : ListingViewModel<CrawlConfigReportItem, ReportItemViewModel, ReportOptionItem>, INavigatedToNotifiable
    {
        private ReportOptionItem _currentReportOption;

        #region HasLastCrawlEnd Property Members

        private static readonly DependencyPropertyKey HasLastCrawlEndPropertyKey = DependencyPropertyBuilder<CrawlStateReportViewModel, ThreeStateViewModel>
            .Register(nameof(HasLastCrawlEnd))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasLastCrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasLastCrawlEndProperty = HasLastCrawlEndPropertyKey.DependencyProperty;

        public ThreeStateViewModel HasLastCrawlEnd { get => (ThreeStateViewModel)GetValue(HasLastCrawlEndProperty); private set => SetValue(HasLastCrawlEndPropertyKey, value); }

        #endregion
        #region MinLastCrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="MinLastCrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinLastCrawlEndProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, DateTime?>
            .Register(nameof(MinLastCrawlEnd))
            .DefaultValue(null)
            .AsReadWrite();

        public DateTime? MinLastCrawlEnd { get => (DateTime?)GetValue(MinLastCrawlEndProperty); set => SetValue(MinLastCrawlEndProperty, value); }

        #endregion
        #region LastCrawlEndLimit Property Members

        /// <summary>
        /// Identifies the <see cref="LastCrawlEndLimit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastCrawlEndLimitProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, DateTime?>
            .Register(nameof(LastCrawlEndLimit))
            .DefaultValue(null)
            .AsReadWrite();

        public DateTime? LastCrawlEndLimit { get => (DateTime?)GetValue(LastCrawlEndLimitProperty); set => SetValue(LastCrawlEndLimitProperty, value); }

        #endregion
        #region RelativeLastCrawlEndIsBefore Property Members

        /// <summary>
        /// Identifies the <see cref="RelativeLastCrawlEndIsBefore"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeLastCrawlEndIsBeforeProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, bool>
            .Register(nameof(RelativeLastCrawlEndIsBefore))
            .DefaultValue(false)
            .AsReadWrite();

        public bool RelativeLastCrawlEndIsBefore { get => (bool)GetValue(RelativeLastCrawlEndIsBeforeProperty); set => SetValue(RelativeLastCrawlEndIsBeforeProperty, value); }

        #endregion
        #region IsScheduled Property Members

        private static readonly DependencyPropertyKey IsScheduledPropertyKey = DependencyPropertyBuilder<CrawlStateReportViewModel, ThreeStateViewModel>
            .Register(nameof(IsScheduled))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="IsScheduled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsScheduledProperty = IsScheduledPropertyKey.DependencyProperty;

        public ThreeStateViewModel IsScheduled { get => (ThreeStateViewModel)GetValue(IsScheduledProperty); private set => SetValue(IsScheduledPropertyKey, value); }

        #endregion
        #region MinNextScheduledStart Property Members

        private static readonly DependencyPropertyKey MinNextScheduledStartPropertyKey = DependencyPropertyBuilder<CrawlStateReportViewModel, DateTime?>
            .Register(nameof(MinNextScheduledStart))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="MinNextScheduledStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinNextScheduledStartProperty = MinNextScheduledStartPropertyKey.DependencyProperty;

        public DateTime? MinNextScheduledStart { get => (DateTime?)GetValue(MinNextScheduledStartProperty); private set => SetValue(MinNextScheduledStartPropertyKey, value); }

        #endregion
        #region NextScheduledStartLimit Property Members

        /// <summary>
        /// Identifies the <see cref="NextScheduledStartLimit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextScheduledStartLimitProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, DateTime?>
            .Register(nameof(NextScheduledStartLimit))
            .DefaultValue(null)
            .AsReadWrite();

        public DateTime? NextScheduledStartLimit { get => (DateTime?)GetValue(NextScheduledStartLimitProperty); set => SetValue(NextScheduledStartLimitProperty, value); }

        #endregion
        #region RelativeLastCrawlEndDays Property Members

        /// <summary>
        /// Identifies the <see cref="RelativeLastCrawlEndDays"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeLastCrawlEndDaysProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, int?>
            .Register(nameof(RelativeLastCrawlEndDays))
            .DefaultValue(null)
            .AsReadWrite();

        public int? RelativeLastCrawlEndDays { get => (int?)GetValue(RelativeLastCrawlEndDaysProperty); set => SetValue(RelativeLastCrawlEndDaysProperty, value); }

        #endregion
        #region RelativeNextScheduledStartIsBefore Property Members

        /// <summary>
        /// Identifies the <see cref="RelativeNextScheduledStartIsBefore"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeNextScheduledStartIsBeforeProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, bool>
            .Register(nameof(RelativeNextScheduledStartIsBefore))
            .DefaultValue(false)
            .AsReadWrite();

        public bool RelativeNextScheduledStartIsBefore { get => (bool)GetValue(RelativeNextScheduledStartIsBeforeProperty); set => SetValue(RelativeNextScheduledStartIsBeforeProperty, value); }

        #endregion
        #region RelativeNextScheduledStartDays Property Members

        /// <summary>
        /// Identifies the <see cref="RelativeNextScheduledStartDays"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeNextScheduledStartDaysProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, int?>
            .Register(nameof(RelativeNextScheduledStartDays))
            .DefaultValue(null)
            .AsReadWrite();

        public int? RelativeNextScheduledStartDays { get => (int?)GetValue(RelativeNextScheduledStartDaysProperty); set => SetValue(RelativeNextScheduledStartDaysProperty, value); }

        #endregion
        #region NextScheduledStartDaysOverdue Property Members

        /// <summary>
        /// Identifies the <see cref="NextScheduledStartDaysOverdue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextScheduledStartDaysOverdueProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, bool>
            .Register(nameof(NextScheduledStartDaysOverdue))
            .DefaultValue(false)
            .AsReadWrite();

        public bool NextScheduledStartDaysOverdue { get => (bool)GetValue(NextScheduledStartDaysOverdueProperty); set => SetValue(NextScheduledStartDaysOverdueProperty, value); }

        #endregion
        #region Owner Attached Property Members

        /// <summary>
        /// The name of the <see cref="OwnerProperty">Owner</see> attached dependency property.
        /// </summary>
        public const string PropertyName_Owner = "Owner";

        private static readonly DependencyPropertyKey OwnerPropertyKey = DependencyPropertyBuilder<CrawlStateReportViewModel, CrawlStateReportViewModel>
            .RegisterAttached(PropertyName_Owner)
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Owner"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OwnerProperty = OwnerPropertyKey.DependencyProperty;

        public static CrawlStateReportViewModel GetOwner([DisallowNull] DependencyObject obj) => (CrawlStateReportViewModel)obj.GetValue(OwnerProperty);

        private static void SetOwner([DisallowNull] DependencyObject obj, CrawlStateReportViewModel value) => obj.SetValue(OwnerPropertyKey, value);

        #endregion
        #region IsSelected Attached Property Members

        /// <summary>
        /// The name of the <see cref="IsSelectedProperty">IsSelected</see> attached dependency property.
        /// </summary>
        public const string PropertyName_IsSelected = "IsSelected";

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, bool>
            .RegisterAttached(PropertyName_IsSelected)
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => RaiseIsSelectedPropertyChanged(d, oldValue, newValue))
            .AsReadWrite();

        public static bool GetIsSelected([DisallowNull] DependencyObject obj) => (bool)obj.GetValue(IsSelectedProperty);

        public static void SetIsSelected([DisallowNull] DependencyObject obj, bool value) => obj.SetValue(IsSelectedProperty, value);

        /// <summary>
        /// Called when the value of the <see cref="IsSelected"/> dependency property has changed.
        /// </summary>
        /// <param name="obj">The object whose attached property value has changed.</param>
        /// <param name="oldValue">The previous value of the <see cref="IsSelected"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsSelected"/> property.</param>
        private static void RaiseIsSelectedPropertyChanged(DependencyObject obj, bool oldValue, bool newValue)
        {
            if (obj is ReportOptionItem item)
                GetOwner(item)?.OnIsSelectedPropertyChanged(item, newValue);
        }

        private void OnIsSelectedPropertyChanged(ReportOptionItem item, bool newValue)
        {
            if (newValue)
                SelectedReportOption = item;
            else if (ReferenceEquals(item, SelectedReportOption))
                SelectedReportOption = ReportOptions.FirstOrDefault(i => i is not null && !ReferenceEquals(i, item));
        }

        #endregion
        #region ReportOptions Property Members

        private HashSet<ReportOptionItem> _distinctItems = new();

        /// <summary>
        /// Identifies the <see cref="ReportOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReportOptionsProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, ObservableCollection<ReportOptionItem>>
            .Register(nameof(ReportOptions))
            .OnChanged((d, oldValue, newValue) => (d as CrawlStateReportViewModel)?.OnReportOptionsPropertyChanged(oldValue, newValue))
            .CoerseWith((d, baseValue) => (baseValue as ObservableCollection<ReportOptionItem>) ?? new())
            .AsReadWrite();

        public ObservableCollection<ReportOptionItem> ReportOptions { get => (ObservableCollection<ReportOptionItem>)GetValue(ReportOptionsProperty); set => SetValue(ReportOptionsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ReportOptions"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ReportOptions"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ReportOptions"/> property.</param>
        protected virtual void OnReportOptionsPropertyChanged(ObservableCollection<ReportOptionItem> oldValue, ObservableCollection<ReportOptionItem> newValue)
        {
            ReportOptionItem oldOption = _currentReportOption ?? SelectedReportOption;
            foreach (ReportOptionItem item in _distinctItems)
            {
                if (ReferenceEquals(this, GetOwner(item)))
                    SetOwner(item, null);
            }
            _distinctItems.Clear();
            if (oldValue is not null)
            {
                oldValue.CollectionChanged -= ReportOptions_CollectionChanged;
                foreach (ReportOptionItem item in oldValue.Where(i => i is not null))
                {
                    if (ReferenceEquals(GetOwner(item), this))
                        SetOwner(item, null);
                }
            }
            foreach (ReportOptionItem item in newValue.Where(i => i is not null))
            {
                if (_distinctItems.Add(item) && !ReferenceEquals(GetOwner(item), this))
                    SetOwner(item, this);
            }
            _distinctItems.LastOrDefault(i => GetIsSelected(i));
            IEnumerable<ReportOptionItem> enumerator = _distinctItems.Reverse().SkipWhile(i => !GetIsSelected(i));
            ReportOptionItem newOption = enumerator.FirstOrDefault();
            foreach (ReportOptionItem item in enumerator.Skip(1).ToArray())
                SetIsSelected(item, false);
            _currentReportOption = (newOption is null && (oldOption is null || (newOption = _distinctItems.FirstOrDefault(other => oldOption.IsSameAs(other))) is null)) ? _distinctItems.FirstOrDefault() : newOption;
            SelectedReportOption = _currentReportOption;
            newValue.CollectionChanged += ReportOptions_CollectionChanged;
            if ((oldOption is null) ? _currentReportOption is not null : !ReferenceEquals(oldOption, _currentReportOption))
                ReloadAsync(_currentReportOption);
        }

        private void ReportOptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<ReportOptionItem> enumerable;
            ReportOptionItem oldCurrentItem = _currentReportOption, newCurrentItem = _currentReportOption;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if ((enumerable = e.NewItems?.OfType<ReportOptionItem>().Where(i => i is not null)) is not null)
                    {
                        foreach (ReportOptionItem item in enumerable)
                        {
                            if (!ReferenceEquals(GetOwner(item), this))
                                SetOwner(item, this);
                            if (_distinctItems.Add(item) && GetIsSelected(item))
                            {
                                if (newCurrentItem is null)
                                    newCurrentItem = item;
                                else
                                    SetIsSelected(item, false);
                            }
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (ReportOptionItem item in _distinctItems)
                    {
                        if (ReferenceEquals(this, GetOwner(item)))
                            SetOwner(item, null);
                    }
                    _distinctItems.Clear();
                    if (ReportOptions.Count > 0)
                    {
                        foreach (ReportOptionItem item in ReportOptions.Where(i => i is not null))
                        {
                            if (!ReferenceEquals(this, GetOwner(item)))
                                SetOwner(item, this);
                            _distinctItems.Add(item);
                        }
                        newCurrentItem = ReportOptions.FirstOrDefault(i => i is not null && GetIsSelected(i));
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if ((enumerable = e.OldItems?.OfType<ReportOptionItem>().Where(i => i is not null)) is not null)
                    {
                        foreach (ReportOptionItem item in enumerable)
                        {
                            if (!ReportOptions.Any(o => ReferenceEquals(item, o)))
                            {
                                if (ReferenceEquals(this, GetOwner(item)))
                                    SetOwner(item, null);
                                _distinctItems.Remove(item);
                                if (newCurrentItem is not null && ReferenceEquals(newCurrentItem, item))
                                    newCurrentItem = null;
                            }
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    if ((enumerable = e.OldItems?.OfType<ReportOptionItem>().Where(i => i is not null)) is not null)
                    {
                        foreach (ReportOptionItem item in enumerable)
                        {
                            if (!ReportOptions.Any(o => ReferenceEquals(item, o)))
                            {
                                if (ReferenceEquals(this, GetOwner(item)))
                                    SetOwner(item, null);
                                _distinctItems.Remove(item);
                                if (newCurrentItem is not null && ReferenceEquals(newCurrentItem, item))
                                    newCurrentItem = null;
                            }
                        }
                    }
                    if ((enumerable = e.NewItems?.OfType<ReportOptionItem>().Where(i => i is not null)) is not null)
                    {
                        foreach (ReportOptionItem item in enumerable)
                        {
                            if (!ReferenceEquals(GetOwner(item), this))
                                SetOwner(item, this);
                            if (_distinctItems.Add(item) && GetIsSelected(item))
                            {
                                if (newCurrentItem is null)
                                    newCurrentItem = item;
                                else
                                    SetIsSelected(item, false);
                            }
                        }
                    }
                    break;
                default:
                    return;
            }
            if (newCurrentItem is null && (newCurrentItem = ReportOptions.FirstOrDefault(i => GetIsSelected(i))) is null && (oldCurrentItem is null || (newCurrentItem = ReportOptions.FirstOrDefault(i => oldCurrentItem.IsSameAs(i))) is null) &&
                (newCurrentItem = ReportOptions.FirstOrDefault(i => i is not null)) is null)
                SelectedReportOption = _currentReportOption = null;
            else
            {
                SelectedReportOption = _currentReportOption = newCurrentItem;
                SetIsSelected(newCurrentItem, true);
            }
        }

        #endregion
        #region SelectedReportOption Property Members

        /// <summary>
        /// Identifies the <see cref="SelectedReportOption"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedReportOptionProperty = DependencyPropertyBuilder<CrawlStateReportViewModel, ReportOptionItem>
            .Register(nameof(SelectedReportOption))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStateReportViewModel)?.OnSelectedReportOptionPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ReportOptionItem SelectedReportOption { get => (ReportOptionItem)GetValue(SelectedReportOptionProperty); set => SetValue(SelectedReportOptionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SelectedReportOption"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SelectedReportOption"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SelectedReportOption"/> property.</param>
        protected virtual void OnSelectedReportOptionPropertyChanged(ReportOptionItem oldValue, ReportOptionItem newValue)
        {
            if (newValue is not null)
            {
                if (ReferenceEquals(this, GetOwner(newValue)))
                {
                    SetIsSelected(newValue, true);
                    if (oldValue is not null && ReferenceEquals(this, GetOwner(oldValue)))
                        SetIsSelected(oldValue, false);
                }
                else
                    SelectedReportOption = (oldValue is null || !ReferenceEquals(this, GetOwner(oldValue))) ? _currentReportOption : oldValue;
            }
            else if (oldValue is not null && ReferenceEquals(this, GetOwner(newValue)))
                SetIsSelected(oldValue, false);
        }

        #endregion
        public CrawlStateReportViewModel()
        {
            ReportOptions = new();
        }

        private void UpdatePageTitle(ReportOptionItem currentReportOption)
        {
            throw new NotImplementedException();
        }

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentReportOption);

        protected override bool ConfirmItemDelete([DisallowNull] ReportItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override ReportItemViewModel CreateItemViewModel([DisallowNull] CrawlConfigReportItem entity) => new(entity);

        protected override Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] CrawlConfigReportItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override bool EntityMatchesCurrentFilter([DisallowNull] CrawlConfigReportItem entity) => _currentReportOption?.IsMatch(entity) ?? true;

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetDetailPageAsync([DisallowNull] ReportItemViewModel item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new DetailsPage(new(new CrawlConfiguration(), null)));
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            CrawlConfiguration fs = await dbContext.CrawlConfigurations.FirstOrDefaultAsync(f => f.Id == id, statusListener.CancellationToken);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, statusListener.CancellationToken);
                ReloadAsync(_currentReportOption);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new DetailsPage(new(fs, item.Entity)));
        }

        private DispatcherOperation<PageFunction<ItemFunctionResultEventArgs>> CreateEditPageAsync(CrawlConfiguration crawlConfiguration, SubdirectoryListItemWithAncestorNames selectedRoot, CrawlConfigReportItem listitem,
            [DisallowNull] IWindowsStatusListener statusListener) => Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() =>
            {
                if (crawlConfiguration is null || selectedRoot is null)
                {
                    _ = MessageBox.Show(Application.Current.MainWindow, "Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    ReloadAsync(_currentReportOption);
                    return null;
                }
                return new EditPage(new(crawlConfiguration, listitem) { Root = new(selectedRoot) });
            }, DispatcherPriority.Normal, statusListener.CancellationToken);

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetEditPageAsync(ReportItemViewModel listItem, [DisallowNull] IWindowsStatusListener statusListener)
        {
            CrawlConfiguration crawlConfiguration;
            SubdirectoryListItemWithAncestorNames selectedRoot;
            CrawlConfigReportItem itemEntity;
            if (listItem is not null)
            {
                using IServiceScope serviceScope = Services.CreateScope();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Guid id = listItem.Entity.Id;
                crawlConfiguration = await dbContext.CrawlConfigurations.FirstOrDefaultAsync(c => c.Id == id, statusListener.CancellationToken);
                if (crawlConfiguration is null)
                    selectedRoot = null;
                else
                {
                    id = crawlConfiguration.RootId;
                    selectedRoot = await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id);
                }
                return await CreateEditPageAsync(crawlConfiguration, selectedRoot, listItem.Entity, statusListener);
            }

            itemEntity = null;
            selectedRoot = null;
            crawlConfiguration = null;
            while (selectedRoot is null)
            {
                string path = await Dispatcher.InvokeAsync(() =>
                {
                    using System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog()
                    {
                        Description = "Select root folder",
                        ShowNewFolderButton = false,
                        SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                    };
                    return (dialog.ShowDialog(new WindowOwner()) == System.Windows.Forms.DialogResult.OK) ? dialog.SelectedPath : null;
                }, DispatcherPriority.Background, statusListener.CancellationToken);
                if (string.IsNullOrEmpty(path))
                    return null;
                DirectoryInfo directoryInfo;
                try { directoryInfo = new(path); }
                catch (SecurityException exc)
                {
                    statusListener.Logger.LogError(exc, "Permission denied getting directory information for {Path}.", path);
                    switch (await Dispatcher.ShowMessageBoxAsync($"Permission denied while attempting to import subdirectory.", "Security Exception", MessageBoxButton.OKCancel, MessageBoxImage.Error,
                        statusListener.CancellationToken))
                    {
                        case MessageBoxResult.OK:
                            selectedRoot = null;
                            break;
                        default:
                            return null;
                    }
                    directoryInfo = null;
                }
                catch (PathTooLongException exc)
                {
                    statusListener.Logger.LogError(exc, "Error getting directory information for ({Path} is too long).", path);
                    switch (await Dispatcher.ShowMessageBoxAsync($"Path is too long. Cannnot import subdirectory as crawl root.", "Path Too Long", MessageBoxButton.OKCancel, MessageBoxImage.Error, statusListener.CancellationToken))
                    {
                        case MessageBoxResult.OK:
                            selectedRoot = null;
                            break;
                        default:
                            return null;
                    }
                    directoryInfo = null;
                }
                catch (Exception exc)
                {
                    statusListener.Logger.LogError(exc, "Error getting directory information for {Path}.", path);
                    switch (await Dispatcher.ShowMessageBoxAsync($"Unable to import subdirectory. See system logs for details.", "File System Error", MessageBoxButton.OKCancel, MessageBoxImage.Error,
                        statusListener.CancellationToken))
                    {
                        case MessageBoxResult.OK:
                            selectedRoot = null;
                            break;
                        default:
                            return null;
                    }
                    directoryInfo = null;
                }
                if (directoryInfo is null)
                {
                    selectedRoot = null;
                    crawlConfiguration = null;
                }
                else
                {
                    using IServiceScope serviceScope = Services.CreateScope();
                    using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                    Subdirectory root = await Subdirectory.FindByFullNameAsync(path, dbContext, statusListener.CancellationToken);
                    if (root is null)
                    {
                        crawlConfiguration = null;
                        root = (await Subdirectory.ImportBranchAsync(directoryInfo, dbContext, statusListener.CancellationToken))?.Entity;
                    }
                    else
                        crawlConfiguration = await dbContext.Entry(root).GetRelatedReferenceAsync(d => d.CrawlConfiguration, statusListener.CancellationToken);
                    Guid id = root.Id;
                    selectedRoot = await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id, statusListener.CancellationToken);
                }
                if (crawlConfiguration is not null)
                {
                    switch (await Dispatcher.ShowMessageBoxAsync($"There is already a configuration defined for that path. Would you like to edit that configuration, instead?", "Configuration exists", MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Warning, statusListener.CancellationToken))
                    {
                        case MessageBoxResult.Yes:
                            Guid id = crawlConfiguration.Id;
                            using (IServiceScope serviceScope = Services.CreateScope())
                            {
                                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                                itemEntity = await dbContext.CrawlConfigReport.FirstOrDefaultAsync(c => c.Id == id, statusListener.CancellationToken);
                                if (selectedRoot is null)
                                {
                                    id = crawlConfiguration.RootId;
                                    selectedRoot = await dbContext.SubdirectoryListingWithAncestorNames.FirstOrDefaultAsync(d => d.Id == id, statusListener.CancellationToken);
                                }
                            }
                            break;
                        case MessageBoxResult.No:
                            selectedRoot = null;
                            crawlConfiguration = null;
                            break;
                        default:
                            return null;
                    }
                }
            }
            return await CreateEditPageAsync(crawlConfiguration ?? new(), selectedRoot, itemEntity, statusListener);
        }

        protected override IQueryable<CrawlConfigReportItem> GetQueryableListing(ReportOptionItem options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override void OnApplyFilterOptionsCommand(object parameter) => ReloadAsync(SelectedReportOption);

        protected override void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ReportItemViewModel item)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while deleting the item from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ReportItemViewModel item)
        {
            UpdatePageTitle(_currentReportOption);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentReportOption);

        protected override void OnReloadTaskCanceled(ReportOptionItem options)
        {
            UpdatePageTitle(_currentReportOption);
            SelectedReportOption = _currentReportOption;
        }

        protected override void OnReloadTaskCompleted(ReportOptionItem options) => _currentReportOption = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, ReportOptionItem options)
        {
            UpdatePageTitle(_currentReportOption);
            SelectedReportOption = _currentReportOption;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
