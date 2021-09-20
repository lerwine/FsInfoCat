using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem> :
        CrawlConfigurationViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>
        where TEntity : DbEntity, ICrawlConfiguration, ICrawlConfigurationRow
        where TSubdirectoryEntity : DbEntity, ISubdirectoryListItemWithAncestorNames
        where TSubdirectoryItem : SubdirectoryListItemWithAncestorNamesViewModel<TSubdirectoryEntity>
        where TCrawlJobLogEntity : DbEntity, ICrawlJobListItem
        where TCrawlJobLogItem : CrawlJobListItemViewModel<TCrawlJobLogEntity>
    {
        #region ToggleActivation Command Property Members

        private CrawlStatus? _previousValue;

        private static readonly DependencyPropertyKey ToggleActivationPropertyKey = DependencyPropertyBuilder<CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, Commands.RelayCommand>
            .Register(nameof(ToggleActivation))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ToggleActivation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ToggleActivationProperty = ToggleActivationPropertyKey.DependencyProperty;

        public Commands.RelayCommand ToggleActivation => (Commands.RelayCommand)GetValue(ToggleActivationProperty);

        /// <summary>
        /// Called when the <see cref="ToggleActivation">ToggleActivation Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ToggleActivation" />.</param>
        protected virtual void OnToggleActivationCommand(object parameter)
        {
            if (StatusValue == CrawlStatus.Disabled)
                StatusValue = _previousValue ?? CrawlStatus.NotRunning;
            else
            {
                _previousValue = StatusValue;
                StatusValue = CrawlStatus.Disabled;
            }
        }

        #endregion
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
        #region SaveAndRun Command Property Members

        private static readonly DependencyPropertyKey SaveAndRunPropertyKey = DependencyPropertyBuilder<CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, Commands.RelayCommand>
            .Register(nameof(SaveAndRun))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SaveAndRun"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaveAndRunProperty = SaveAndRunPropertyKey.DependencyProperty;

        public Commands.RelayCommand SaveAndRun => (Commands.RelayCommand)GetValue(SaveAndRunProperty);

        /// <summary>
        /// Called when the <see cref="SaveAndRun">SaveAndRun Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="SaveAndRun" />.</param>
        protected virtual void OnSaveAndRunCommand(object parameter)
        {
            // TODO: Implement OnSaveAndRunCommand Logic
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

        //private void MaxDuration_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void MaxDuration_HasComponentValueErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HasErrors = (bool)e.NewValue || MaxTotalItems.HasErrors || NextScheduledStart.HasComponentValueErrors || RescheduleInterval.HasComponentValueErrors;
        }

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

        //private void RescheduleInterval_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void RescheduleInterval_HasComponentValueErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HasErrors = (bool)e.NewValue || MaxTotalItems.HasErrors || NextScheduledStart.HasComponentValueErrors || MaxDuration.HasComponentValueErrors;
        }

        #endregion
        #region MaxTotalItems Property Members

        private static readonly DependencyPropertyKey MaxTotalItemsPropertyKey = DependencyPropertyBuilder<CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, OptionalValueViewModel<ulong>>
            .Register(nameof(MaxTotalItems))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="MaxTotalItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxTotalItemsProperty = MaxTotalItemsPropertyKey.DependencyProperty;

        public OptionalValueViewModel<ulong> MaxTotalItems => (OptionalValueViewModel<ulong>)GetValue(MaxTotalItemsProperty);

        private void MaxTotalItems_HasErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HasErrors = (bool)e.NewValue || NextScheduledStart.HasComponentValueErrors || RescheduleInterval.HasComponentValueErrors || MaxDuration.HasComponentValueErrors;
        }

        //private void MaxTotalItems_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void MaxTotalItems_ValidateInputValue(object sender, PropertyValidatingEventArgs<ulong> e)
        {
            throw new NotImplementedException();
        }

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

        //private void NextScheduledStart_ResultValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void NextScheduledStart_HasComponentValueErrorsPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HasErrors = (bool)e.NewValue || MaxTotalItems.HasErrors || RescheduleInterval.HasComponentValueErrors || MaxDuration.HasComponentValueErrors;
        }

        #endregion
        #region ScheduleOption Property Members

        private static readonly DependencyPropertyKey ScheduleOptionPropertyKey = DependencyPropertyBuilder<CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, ThreeStateViewModel>
            .Register(nameof(ScheduleOption))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ScheduleOption"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ScheduleOptionProperty = ScheduleOptionPropertyKey.DependencyProperty;

        public ThreeStateViewModel ScheduleOption => (ThreeStateViewModel)GetValue(ScheduleOptionProperty);

        private void ScheduleOption_ValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool? value = e.NewValue as bool?;
            RescheduleInterval.ForceNullResult = !value.HasValue;
        }

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
        #region HasErrors Property Members

        private static readonly DependencyPropertyKey HasErrorsPropertyKey = DependencyPropertyBuilder<CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, bool>
            .Register(nameof(HasErrors))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlConfigurationEditViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>)?.OnHasErrorsPropertyChanged(newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasErrorsProperty = HasErrorsPropertyKey.DependencyProperty;

        public bool HasErrors { get => (bool)GetValue(HasErrorsProperty); private set => SetValue(HasErrorsPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="HasErrors"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="HasErrors"/> property.</param>
        protected virtual void OnHasErrorsPropertyChanged(bool newValue) => SaveChanges.IsEnabled = !newValue;

        #endregion

        public CrawlConfigurationEditViewModel([DisallowNull] TEntity entity, bool isNew, object state = null) : base(entity, state)
        {
            SetValue(ToggleActivationPropertyKey, new Commands.RelayCommand(OnToggleActivationCommand));
            SetValue(SaveChangesPropertyKey, new Commands.RelayCommand(OnSaveChangesCommand));
            SetValue(SaveAndRunPropertyKey, new Commands.RelayCommand(OnSaveAndRunCommand));
            SetValue(DiscardChangesPropertyKey, new Commands.RelayCommand(OnDiscardChangesCommand));
            OptionalValueViewModel<ulong> maxTotalItems = new();
            SetValue(MaxTotalItemsPropertyKey, maxTotalItems);
            ThreeStateViewModel scheduleOption = new(entity.RescheduleInterval.HasValue ? entity.RescheduleFromJobEnd : null);
            SetValue(ScheduleOptionPropertyKey, scheduleOption);
            TimeSpanViewModel maxDuration = new();
            SetValue(MaxDurationPropertyKey, maxDuration);
            TimeSpanViewModel rescheduleInterval = new();
            SetValue(RescheduleIntervalPropertyKey, rescheduleInterval);
            DateTimeViewModel nextScheduledStart = new();
            SetValue(NextScheduledStartPropertyKey, nextScheduledStart);
            maxTotalItems.ValidateInputValue += MaxTotalItems_ValidateInputValue;
            //maxTotalItems.ResultValuePropertyChanged += MaxTotalItems_ResultValuePropertyChanged;
            maxTotalItems.HasErrorsPropertyChanged += MaxTotalItems_HasErrorsPropertyChanged;
            scheduleOption.ValuePropertyChanged += ScheduleOption_ValuePropertyChanged;
            //maxDuration.ResultValuePropertyChanged += MaxDuration_ResultValuePropertyChanged;
            maxDuration.HasComponentValueErrorsPropertyChanged += MaxDuration_HasComponentValueErrorsPropertyChanged;
            //rescheduleInterval.ResultValuePropertyChanged += RescheduleInterval_ResultValuePropertyChanged;
            rescheduleInterval.HasComponentValueErrorsPropertyChanged += RescheduleInterval_HasComponentValueErrorsPropertyChanged;
            //nextScheduledStart.ResultValuePropertyChanged += NextScheduledStart_ResultValuePropertyChanged;
            nextScheduledStart.HasComponentValueErrorsPropertyChanged += NextScheduledStart_HasComponentValueErrorsPropertyChanged;
            IsNew = isNew;
            maxTotalItems.SetValue(entity.MaxTotalItems);
            long? seconds = entity.TTL;
            maxDuration.SetValue(seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null);
            seconds = entity.RescheduleInterval;
            rescheduleInterval.SetValue(seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null);
            nextScheduledStart.SetValue(entity.NextScheduledStart);
        }
    }
}
