using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlConfigReportItemViewModel<TEntity> : CrawlConfigListItemViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.ICrawlConfigReportItem
    {
        #region SucceededCount Property Members

        private static readonly DependencyPropertyKey SucceededCountPropertyKey = DependencyPropertyBuilder<CrawlConfigReportItemViewModel<TEntity>, long>
            .Register(nameof(SucceededCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SucceededCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SucceededCountProperty = SucceededCountPropertyKey.DependencyProperty;

        public long SucceededCount { get => (long)GetValue(SucceededCountProperty); private set => SetValue(SucceededCountPropertyKey, value); }

        #endregion
        #region TimedOutCount Property Members

        private static readonly DependencyPropertyKey TimedOutCountPropertyKey = DependencyPropertyBuilder<CrawlConfigReportItemViewModel<TEntity>, long>
            .Register(nameof(TimedOutCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="TimedOutCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TimedOutCountProperty = TimedOutCountPropertyKey.DependencyProperty;

        public long TimedOutCount { get => (long)GetValue(TimedOutCountProperty); private set => SetValue(TimedOutCountPropertyKey, value); }

        #endregion
        #region ItemLimitReachedCount Property Members

        private static readonly DependencyPropertyKey ItemLimitReachedCountPropertyKey = DependencyPropertyBuilder<CrawlConfigReportItemViewModel<TEntity>, long>
            .Register(nameof(ItemLimitReachedCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ItemLimitReachedCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemLimitReachedCountProperty = ItemLimitReachedCountPropertyKey.DependencyProperty;

        public long ItemLimitReachedCount { get => (long)GetValue(ItemLimitReachedCountProperty); private set => SetValue(ItemLimitReachedCountPropertyKey, value); }

        #endregion
        #region CanceledCount Property Members

        private static readonly DependencyPropertyKey CanceledCountPropertyKey = DependencyPropertyBuilder<CrawlConfigReportItemViewModel<TEntity>, long>
            .Register(nameof(CanceledCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="CanceledCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanceledCountProperty = CanceledCountPropertyKey.DependencyProperty;

        public long CanceledCount { get => (long)GetValue(CanceledCountProperty); private set => SetValue(CanceledCountPropertyKey, value); }

        #endregion
        #region FailedCount Property Members

        private static readonly DependencyPropertyKey FailedCountPropertyKey = DependencyPropertyBuilder<CrawlConfigReportItemViewModel<TEntity>, long>
            .Register(nameof(FailedCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="FailedCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FailedCountProperty = FailedCountPropertyKey.DependencyProperty;

        public long FailedCount { get => (long)GetValue(FailedCountProperty); private set => SetValue(FailedCountPropertyKey, value); }

        #endregion
        #region AverageDuration Property Members

        private static readonly DependencyPropertyKey AverageDurationPropertyKey = DependencyPropertyBuilder<CrawlConfigReportItemViewModel<TEntity>, TimeSpan?>
            .Register(nameof(AverageDuration))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="AverageDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AverageDurationProperty = AverageDurationPropertyKey.DependencyProperty;

        public TimeSpan? AverageDuration { get => (TimeSpan?)GetValue(AverageDurationProperty); private set => SetValue(AverageDurationPropertyKey, value); }

        #endregion
        #region MaxDuration Property Members

        private static readonly DependencyPropertyKey MaxDurationPropertyKey = DependencyPropertyBuilder<CrawlConfigReportItemViewModel<TEntity>, TimeSpan?>
            .Register(nameof(MaxDuration))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="MaxDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxDurationProperty = MaxDurationPropertyKey.DependencyProperty;

        public TimeSpan? MaxDuration { get => (TimeSpan?)GetValue(MaxDurationProperty); private set => SetValue(MaxDurationPropertyKey, value); }

        #endregion

        public CrawlConfigReportItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SucceededCount = entity.SucceededCount;
            TimedOutCount = entity.TimedOutCount;
            ItemLimitReachedCount = entity.ItemLimitReachedCount;
            CanceledCount = entity.CanceledCount;
            FailedCount = entity.FailedCount;
            long? seconds = entity.AverageDuration;
            AverageDuration = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
            seconds = entity.MaxDuration;
            MaxDuration = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
        }
    }
}
