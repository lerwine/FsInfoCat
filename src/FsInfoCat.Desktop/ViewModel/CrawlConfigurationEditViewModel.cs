using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem> :
        CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>
        where TEntity : DbEntity, ICrawlConfiguration, ICrawlConfigurationRow
        where TSubdirectoryEntity : DbEntity, ISubdirectoryListItemWithAncestorNames
        where TSubdirectoryItem : SubdirectoryListItemWithAncestorNamesViewModel<TSubdirectoryEntity>
        where TCrawlJobLogEntity : DbEntity, ICrawlJobListItem
        where TCrawlJobLogItem : CrawlJobListItemViewModel<TCrawlJobLogEntity>
    {
        #region SaveChanges Command Property Members

        private static readonly DependencyPropertyKey SaveChangesPropertyKey = DependencyPropertyBuilder<CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, Commands.RelayCommand>
            .Register(nameof(SaveChanges))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SaveChanges"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaveChangesProperty = SaveChangesPropertyKey.DependencyProperty;

        public Commands.RelayCommand SaveChanges => (Commands.RelayCommand)GetValue(SaveChangesProperty);

        /// <summary>
        /// Called when the <see cref="SaveChanges">SaveChanges Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SaveChanges" />.</param>
        protected virtual void OnSaveChangesCommand(object parameter)
        {
            // TODO: Implement OnSaveChangesCommand Logic
        }

        #endregion
        #region DiscardChanges Command Property Members

        private static readonly DependencyPropertyKey DiscardChangesPropertyKey = DependencyPropertyBuilder<CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, Commands.RelayCommand>
            .Register(nameof(DiscardChanges))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="DiscardChanges"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DiscardChangesProperty = DiscardChangesPropertyKey.DependencyProperty;

        public Commands.RelayCommand DiscardChanges => (Commands.RelayCommand)GetValue(DiscardChangesProperty);

        /// <summary>
        /// Called when the <see cref="DiscardChanges">DiscardChanges Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="DiscardChanges" />.</param>
        protected virtual void OnDiscardChangesCommand(object parameter)
        {
            // TODO: Implement OnDiscardChangesCommand Logic
        }

        #endregion
        #region MaxDuration Property Members

        private static readonly DependencyPropertyKey MaxDurationPropertyKey = ColumnPropertyBuilder<TimeSpanViewModel, CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>>
            .RegisterEntityMapped<TEntity>(nameof(MaxDuration), nameof(ICrawlConfigurationRow.TTL))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="MaxDuration"/> lodependency property.
        /// </summary>
        public static readonly DependencyProperty MaxDurationProperty = MaxDurationPropertyKey.DependencyProperty;

        public TimeSpanViewModel MaxDuration => (TimeSpanViewModel)GetValue(MaxDurationProperty);

        #endregion
        #region RescheduleInterval Property Members

        private static readonly DependencyPropertyKey RescheduleIntervalPropertyKey = ColumnPropertyBuilder<TimeSpanViewModel, CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>>
            .RegisterEntityMapped<TEntity>(nameof(RescheduleInterval))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="RescheduleInterval"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RescheduleIntervalProperty = RescheduleIntervalPropertyKey.DependencyProperty;

        public TimeSpanViewModel RescheduleInterval { get => (TimeSpanViewModel)GetValue(RescheduleIntervalProperty); private set => SetValue(RescheduleIntervalPropertyKey, value); }

        #endregion
        #region NextScheduledStart Property Members

        private static readonly DependencyPropertyKey NextScheduledStartPropertyKey = ColumnPropertyBuilder<DateTimeViewModel, CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>>
            .Register(nameof(NextScheduledStart))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="NextScheduledStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextScheduledStartProperty = NextScheduledStartPropertyKey.DependencyProperty;

        public DateTimeViewModel NextScheduledStart { get => (DateTimeViewModel)GetValue(NextScheduledStartProperty); private set => SetValue(NextScheduledStartPropertyKey, value); }

        #endregion
        #region IsNew Property Members

        private static readonly DependencyPropertyKey IsNewPropertyKey = DependencyPropertyBuilder<CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, bool>
            .Register(nameof(IsNew))
            .DefaultValue(false)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="IsNew"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsNewProperty = IsNewPropertyKey.DependencyProperty;

        public bool IsNew { get => (bool)GetValue(IsNewProperty); private set => SetValue(IsNewPropertyKey, value); }

        #endregion

        public CrawlConfigurationEditViewModel([DisallowNull] TEntity entity, bool isNew, object state = null) : base(entity, state)
        {
            SetValue(SaveChangesPropertyKey, new Commands.RelayCommand(OnSaveChangesCommand));
            SetValue(DiscardChangesPropertyKey, new Commands.RelayCommand(OnDiscardChangesCommand));
            IsNew = isNew;
            long? seconds = entity.TTL;
            TimeSpanViewModel tsVm = new();
            SetValue(MaxDurationPropertyKey, tsVm);
            tsVm.SetValue(seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null);
            tsVm.ResultValuePropertyChanged += MaxDuration_ResultValuePropertyChanged;
            tsVm.HasComponentValueErrorsPropertyChanged += MaxDuration_HasComponentValueErrorsPropertyChanged;
            seconds = entity.RescheduleInterval;
            tsVm = new TimeSpanViewModel();
            SetValue(RescheduleIntervalPropertyKey, tsVm);
            tsVm.SetValue(seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null);
            tsVm.ResultValuePropertyChanged += RescheduleInterval_ResultValuePropertyChanged;
            tsVm.HasComponentValueErrorsPropertyChanged += RescheduleInterval_HasComponentValueErrorsPropertyChanged;
            DateTimeViewModel dtVm = new();
            SetValue(NextScheduledStartPropertyKey, dtVm);
            dtVm.SetValue(entity.NextScheduledStart);
            dtVm.ResultValuePropertyChanged += NextScheduledStart_ResultValuePropertyChanged;
            dtVm.HasComponentValueErrorsPropertyChanged += NextScheduledStart_HasComponentValueErrorsPropertyChanged;
        }

        private void MaxDuration_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RescheduleInterval_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NextScheduledStart_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MaxDuration_HasComponentValueErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RescheduleInterval_HasComponentValueErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NextScheduledStart_HasComponentValueErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
