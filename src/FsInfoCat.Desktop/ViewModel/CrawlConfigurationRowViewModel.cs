using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlConfigurationRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, ICrawlConfigurationRow
    {
        #region DisplayName Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string),
            typeof(CrawlConfigurationRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlConfigurationRowViewModel<TEntity>)?.OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DisplayName { get => GetValue(DisplayNameProperty) as string; set => SetValue(DisplayNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DisplayName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DisplayName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DisplayName"/> property.</param>
        protected void OnDisplayNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnDisplayNamePropertyChanged Logic
        }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string),
            typeof(CrawlConfigurationRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlConfigurationRowViewModel<TEntity>)?.OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Notes { get => GetValue(NotesProperty) as string; set => SetValue(NotesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Notes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Notes"/> property.</param>
        protected void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNotesPropertyChanged Logic
        }

        #endregion
        //public abstract CrawlStatus StatusValue { get; set; }
        //public abstract DateTime? LastCrawlStart { get; set; }
        //public abstract DateTime? LastCrawlEnd { get; set; }
        //public abstract DateTime? NextScheduledStart { get; set; }
        //public abstract TimeSpan? RescheduleInterval { get; set; }

        #region RescheduleFromJobEnd Property Members

        /// <summary>
        /// Identifies the <see cref="RescheduleFromJobEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RescheduleFromJobEndProperty = DependencyProperty.Register(nameof(RescheduleFromJobEnd), typeof(bool),
            typeof(CrawlConfigurationRowViewModel<TEntity>), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlConfigurationRowViewModel<TEntity>)?.OnRescheduleFromJobEndPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool RescheduleFromJobEnd { get => (bool)GetValue(RescheduleFromJobEndProperty); set => SetValue(RescheduleFromJobEndProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RescheduleFromJobEnd"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RescheduleFromJobEnd"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RescheduleFromJobEnd"/> property.</param>
        protected void OnRescheduleFromJobEndPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnRescheduleFromJobEndPropertyChanged Logic
        }

        #endregion
        #region RescheduleAfterFail Property Members

        /// <summary>
        /// Identifies the <see cref="RescheduleAfterFail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RescheduleAfterFailProperty = DependencyProperty.Register(nameof(RescheduleAfterFail), typeof(bool),
            typeof(CrawlConfigurationRowViewModel<TEntity>), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlConfigurationRowViewModel<TEntity>)?.OnRescheduleAfterFailPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool RescheduleAfterFail { get => (bool)GetValue(RescheduleAfterFailProperty); set => SetValue(RescheduleAfterFailProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RescheduleAfterFail"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RescheduleAfterFail"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RescheduleAfterFail"/> property.</param>
        protected void OnRescheduleAfterFailPropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnRescheduleAfterFailPropertyChanged Logic
        }

        #endregion
        #region MaxRecursionDepth Property Members

        /// <summary>
        /// Identifies the <see cref="MaxRecursionDepth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxRecursionDepthProperty = DependencyProperty.Register(nameof(MaxRecursionDepth), typeof(ushort),
            typeof(CrawlConfigurationRowViewModel<TEntity>), new PropertyMetadata(DbConstants.DbColDefaultValue_MaxRecursionDepth,
                (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as CrawlConfigurationRowViewModel<TEntity>)?.OnMaxRecursionDepthPropertyChanged((ushort)e.OldValue, (ushort)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public ushort MaxRecursionDepth { get => (ushort)GetValue(MaxRecursionDepthProperty); set => SetValue(MaxRecursionDepthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaxRecursionDepth"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaxRecursionDepth"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaxRecursionDepth"/> property.</param>
        protected void OnMaxRecursionDepthPropertyChanged(ushort oldValue, ushort newValue)
        {
            // TODO: Implement OnMaxRecursionDepthPropertyChanged Logic
        }

        #endregion
        #region MaxTotalItems Property Members

        /// <summary>
        /// Identifies the <see cref="MaxTotalItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxTotalItemsProperty = DependencyProperty.Register(nameof(MaxTotalItems), typeof(ulong?),
            typeof(CrawlConfigurationRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as CrawlConfigurationRowViewModel<TEntity>)?.OnMaxTotalItemsPropertyChanged((ulong?)e.OldValue, (ulong?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public ulong? MaxTotalItems { get => (ulong?)GetValue(MaxTotalItemsProperty); set => SetValue(MaxTotalItemsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaxTotalItems"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaxTotalItems"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaxTotalItems"/> property.</param>
        protected void OnMaxTotalItemsPropertyChanged(ulong? oldValue, ulong? newValue)
        {
            // TODO: Implement OnMaxTotalItemsPropertyChanged Logic
        }

        #endregion
        //public abstract TimeSpan? TTL { get; set; }
        #region MaxDuration Property Members

        private static readonly DependencyPropertyKey MaxDurationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxDuration), typeof(TimeSpanViewModel),
            typeof(CrawlConfigurationRowViewModel<TEntity>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MaxDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxDurationProperty = MaxDurationPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        // BUG: This is not acceptable for listing views. Needs to be TimeSpan?
        public TimeSpanViewModel MaxDuration => (TimeSpanViewModel)GetValue(MaxDurationProperty);

        #endregion

        public CrawlConfigurationRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            DisplayName = entity.DisplayName;
            Notes = entity.Notes;
            //StatusValue = entity.StatusValue;
            //LastCrawlStart = entity.LastCrawlStart;
            //LastCrawlEnd = entity.LastCrawlEnd;
            //NextScheduledStart = entity.NextScheduledStart;
            //long? seconds = entity.RescheduleInterval;
            //RescheduleInterval = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
            //RescheduleFromJobEnd = entity.RescheduleFromJobEnd;
            RescheduleAfterFail = entity.RescheduleAfterFail;
            MaxRecursionDepth = entity.MaxRecursionDepth;
            MaxTotalItems = entity.MaxTotalItems;
            long? seconds = entity.TTL;
            SetValue(MaxDurationPropertyKey, new TimeSpanViewModel(seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null));
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
                //case nameof(ICrawlConfigurationRow.StatusValue):
                //    Dispatcher.CheckInvoke(() => StatusValue = Entity.StatusValue);
                //    break;
                //case nameof(ICrawlConfigurationRow.LastCrawlStart):
                //    Dispatcher.CheckInvoke(() => LastCrawlStart = Entity.LastCrawlStart);
                //    break;
                //case nameof(ICrawlConfigurationRow.LastCrawlEnd):
                //    Dispatcher.CheckInvoke(() => LastCrawlEnd = Entity.LastCrawlEnd);
                //    break;
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
                case nameof(ICrawlConfigurationRow.MaxTotalItems):
                    Dispatcher.CheckInvoke(() => MaxTotalItems = Entity.MaxTotalItems);
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
