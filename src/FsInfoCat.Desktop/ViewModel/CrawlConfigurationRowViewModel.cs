using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlConfigurationRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>, ICrawlConfigurationRowViewModel
        where TEntity : DbEntity, ICrawlConfigurationRow
    {
        #region DisplayName Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = ColumnPropertyBuilder<string, CrawlConfigurationRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationRow.DisplayName))
            .DefaultValue("")
            .OnChanged((DependencyObject d, string oldValue, string newValue) =>
                (d as CrawlConfigurationRowViewModel<TEntity>).OnDisplayNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string DisplayName { get => GetValue(DisplayNameProperty) as string; set => SetValue(DisplayNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DisplayName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DisplayName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DisplayName"/> property.</param>
        protected virtual void OnDisplayNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = ColumnPropertyBuilder<string, CrawlConfigurationRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationRow.Notes))
            .DefaultValue("")
            .OnChanged((DependencyObject d, string oldValue, string newValue) =>
                (d as CrawlConfigurationRowViewModel<TEntity>).OnNotesPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string Notes { get => GetValue(NotesProperty) as string; set => SetValue(NotesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Notes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Notes"/> property.</param>
        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region StatusValue Property Members

        /// <summary>
        /// Identifies the <see cref="StatusValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusValueProperty = ColumnPropertyBuilder<CrawlStatus, CrawlConfigurationRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.StatusValue))
            .DefaultValue(CrawlStatus.NotRunning)
            .AsReadWrite();

        public CrawlStatus StatusValue { get => (CrawlStatus)GetValue(StatusValueProperty); set => SetValue(StatusValueProperty, value); }

        #endregion
        #region LastCrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="LastCrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastCrawlEndProperty = ColumnPropertyBuilder<DateTime?, CrawlConfigurationRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.LastCrawlEnd))
            .DefaultValue(null)
            .AsReadWrite();

        public DateTime? LastCrawlEnd { get => (DateTime?)GetValue(LastCrawlEndProperty); set => SetValue(LastCrawlEndProperty, value); }

        #endregion
        #region LastCrawlStart Property Members

        /// <summary>
        /// Identifies the <see cref="LastCrawlStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastCrawlStartProperty = ColumnPropertyBuilder<DateTime?, CrawlConfigurationRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.LastCrawlStart))
            .DefaultValue(null)
            .AsReadWrite();

        public DateTime? LastCrawlStart { get => (DateTime?)GetValue(LastCrawlStartProperty); set => SetValue(LastCrawlStartProperty, value); }

        #endregion
        //public abstract DateTime? NextScheduledStart { get; set; }
        //public abstract TimeSpan? RescheduleInterval { get; set; }

        #region RescheduleFromJobEnd Property Members

        /// <summary>
        /// Identifies the <see cref="RescheduleFromJobEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RescheduleFromJobEndProperty = ColumnPropertyBuilder<bool, CrawlConfigurationRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationRow.RescheduleFromJobEnd))
            .DefaultValue(false)
            .OnChanged((DependencyObject d, bool oldValue, bool newValue) =>
                (d as CrawlConfigurationRowViewModel<TEntity>).OnRescheduleFromJobEndPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool RescheduleFromJobEnd { get => (bool)GetValue(RescheduleFromJobEndProperty); set => SetValue(RescheduleFromJobEndProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RescheduleFromJobEnd"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RescheduleFromJobEnd"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RescheduleFromJobEnd"/> property.</param>
        protected virtual void OnRescheduleFromJobEndPropertyChanged(bool oldValue, bool newValue) { }

        #endregion
        #region RescheduleAfterFail Property Members

        /// <summary>
        /// Identifies the <see cref="RescheduleAfterFail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RescheduleAfterFailProperty = ColumnPropertyBuilder<bool, CrawlConfigurationRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationRow.RescheduleAfterFail))
            .DefaultValue(false)
            .OnChanged((DependencyObject d, bool oldValue, bool newValue) =>
                (d as CrawlConfigurationRowViewModel<TEntity>).OnRescheduleAfterFailPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool RescheduleAfterFail { get => (bool)GetValue(RescheduleAfterFailProperty); set => SetValue(RescheduleAfterFailProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RescheduleAfterFail"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RescheduleAfterFail"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RescheduleAfterFail"/> property.</param>
        protected virtual void OnRescheduleAfterFailPropertyChanged(bool oldValue, bool newValue) { }

        #endregion
        #region MaxRecursionDepth Property Members

        /// <summary>
        /// Identifies the <see cref="MaxRecursionDepth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxRecursionDepthProperty = ColumnPropertyBuilder<ushort, CrawlConfigurationRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationRow.MaxRecursionDepth))
            .DefaultValue(0)
            .OnChanged((DependencyObject d, ushort oldValue, ushort newValue) =>
                (d as CrawlConfigurationRowViewModel<TEntity>).OnMaxRecursionDepthPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ushort MaxRecursionDepth { get => (ushort)GetValue(MaxRecursionDepthProperty); set => SetValue(MaxRecursionDepthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaxRecursionDepth"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaxRecursionDepth"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaxRecursionDepth"/> property.</param>
        protected virtual void OnMaxRecursionDepthPropertyChanged(ushort oldValue, ushort newValue) { }

        #endregion
        //public abstract TimeSpan? TTL { get; set; }

        ICrawlConfigurationRow ICrawlConfigurationRowViewModel.Entity => Entity;

        public CrawlConfigurationRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            DisplayName = entity.DisplayName;
            Notes = entity.Notes;
            StatusValue = entity.StatusValue;
            LastCrawlStart = entity.LastCrawlStart;
            LastCrawlEnd = entity.LastCrawlEnd;
            //NextScheduledStart = entity.NextScheduledStart;
            //long? seconds = entity.RescheduleInterval;
            //RescheduleInterval = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
            //RescheduleFromJobEnd = entity.RescheduleFromJobEnd;
            RescheduleAfterFail = entity.RescheduleAfterFail;
            MaxRecursionDepth = entity.MaxRecursionDepth;
        }

        protected bool CheckEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ICrawlConfigurationRow.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Entity.DisplayName);
                    break;
                case nameof(ICrawlConfigurationRow.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
                    break;
                case nameof(ICrawlConfigurationRow.StatusValue):
                    Dispatcher.CheckInvoke(() => StatusValue = Entity.StatusValue);
                    break;
                case nameof(ICrawlConfigurationRow.LastCrawlStart):
                    Dispatcher.CheckInvoke(() => LastCrawlStart = Entity.LastCrawlStart);
                    break;
                case nameof(ICrawlConfigurationRow.LastCrawlEnd):
                    Dispatcher.CheckInvoke(() => LastCrawlEnd = Entity.LastCrawlEnd);
                    break;
                //case nameof(ICrawlConfigurationRow.NextScheduledStart):
                //    Dispatcher.CheckInvoke(() => NextScheduledStart = Entity.NextScheduledStart);
                //    break;
                //case nameof(ICrawlConfigurationRow.RescheduleInterval):
                //    long? seconds = Entity.RescheduleInterval;
                //    RescheduleInterval = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
                //    break;
                case nameof(ICrawlConfigurationRow.RescheduleFromJobEnd):
                    Dispatcher.CheckInvoke(() => RescheduleFromJobEnd = Entity.RescheduleFromJobEnd);
                    break;
                case nameof(ICrawlConfigurationRow.RescheduleAfterFail):
                    Dispatcher.CheckInvoke(() => RescheduleAfterFail = Entity.RescheduleAfterFail);
                    break;
                case nameof(ICrawlConfigurationRow.MaxRecursionDepth):
                    Dispatcher.CheckInvoke(() => MaxRecursionDepth = Entity.MaxRecursionDepth);
                    break;
                //case nameof(ICrawlConfigurationRow.TTL):
                //    long? ttl = Entity.TTL;
                //    Dispatcher.CheckInvoke(() => TTL = ttl.HasValue ? TimeSpan.FromSeconds(ttl.Value) : null);
                //    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    return false;
            }
            return true;
        }
    }
}
