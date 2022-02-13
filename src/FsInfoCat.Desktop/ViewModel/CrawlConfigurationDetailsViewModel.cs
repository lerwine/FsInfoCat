using FsInfoCat.Activities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem> :
        CrawlConfigurationViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, IItemFunctionViewModel<TEntity>
        where TEntity : DbEntity, ICrawlConfiguration, ICrawlConfigurationRow
        where TSubdirectoryEntity : DbEntity, ISubdirectoryListItemWithAncestorNames
        where TSubdirectoryItem : SubdirectoryListItemWithAncestorNamesViewModel<TSubdirectoryEntity>
        where TCrawlJobLogEntity : DbEntity, ICrawlJobListItem
        where TCrawlJobLogItem : CrawlJobListItemViewModel<TCrawlJobLogEntity>
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Edit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditProperty = EditPropertyKey.DependencyProperty;

        public Commands.RelayCommand Edit => (Commands.RelayCommand)GetValue(EditProperty);

        /// <summary>
        /// Called when the Edit event is raised by <see cref="Edit" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Edit" />.</param>
        protected virtual void RaiseEditCommand(object parameter) => EditCommand?.Invoke(this, new(parameter));

        #endregion
        #region GoToRootSubdirectory Command Property Members

        /// <summary>
        /// Occurs when the <see cref="GoToRootSubdirectory"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> GoToRootSubdirectoryCommand;

        private static readonly DependencyPropertyKey GoToRootSubdirectoryPropertyKey = DependencyPropertyBuilder<CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, Commands.RelayCommand>
            .Register(nameof(GoToRootSubdirectory))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="GoToRootSubdirectory"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GoToRootSubdirectoryProperty = GoToRootSubdirectoryPropertyKey.DependencyProperty;

        public Commands.RelayCommand GoToRootSubdirectory => (Commands.RelayCommand)GetValue(GoToRootSubdirectoryProperty);

        /// <summary>
        /// Called when the GoToRootSubdirectory event is raised by <see cref="GoToRootSubdirectory" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="GoToRootSubdirectory" />.</param>
        protected void RaiseGoToRootSubdirectoryCommand(object parameter) // => GoToRootSubdirectoryCommand?.Invoke(this, new(parameter));
        {
            try { OnGoToRootSubdirectoryCommand(parameter); }
            finally { GoToRootSubdirectoryCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="GoToRootSubdirectory">GoToRootSubdirectory Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="GoToRootSubdirectory" />.</param>
        protected virtual void OnGoToRootSubdirectoryCommand(object parameter)
        {
            // TODO: Implement OnGoToRootSubdirectoryCommand Logic
        }

        #endregion
        #region ViewLogs Command Property Members

        /// <summary>
        /// Occurs when the <see cref="ViewLogs"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> ViewLogsCommand;

        private static readonly DependencyPropertyKey ViewLogsPropertyKey = DependencyPropertyBuilder<CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, Commands.RelayCommand>
            .Register(nameof(ViewLogs))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ViewLogs"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewLogsProperty = ViewLogsPropertyKey.DependencyProperty;

        public Commands.RelayCommand ViewLogs => (Commands.RelayCommand)GetValue(ViewLogsProperty);

        /// <summary>
        /// Called when the ViewLogs event is raised by <see cref="ViewLogs" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ViewLogs" />.</param>
        protected void RaiseViewLogsCommand(object parameter) // => ViewLogsCommand?.Invoke(this, new(parameter));
        {
            try { OnViewLogsCommand(parameter); }
            finally { ViewLogsCommand?.Invoke(this, new(parameter)); }
        }

        /// <summary>
        /// Called when the <see cref="ViewLogs">ViewLogs Command</see> is invoked.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="ViewLogs" />.</param>
        protected virtual void OnViewLogsCommand(object parameter)
        {
            // TODO: Implement OnViewLogsCommand Logic
        }

        #endregion
        #region MaxTotalItems Property Members

        /// <summary>
        /// Identifies the <see cref="MaxTotalItems"/> dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey MaxTotalItemsPropertyKey = DependencyPropertyBuilder<CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, ulong?>
            .Register(nameof(MaxTotalItems))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="MaxTotalItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxTotalItemsProperty = MaxTotalItemsPropertyKey.DependencyProperty;

        public ulong? MaxTotalItems { get => (ulong?)GetValue(MaxTotalItemsProperty); private set => SetValue(MaxTotalItemsPropertyKey, value); }

        #endregion
        #region TTL Property Members

        private static readonly DependencyPropertyKey TTLPropertyKey = DependencyPropertyBuilder<CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, TimeSpan?>
            .Register(nameof(TTL))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="TTL"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TTLProperty = TTLPropertyKey.DependencyProperty;

        public TimeSpan? TTL { get => (TimeSpan?)GetValue(TTLProperty); private set => SetValue(TTLPropertyKey, value); }

        #endregion
        #region NextScheduledStart Property Members

        private static readonly DependencyPropertyKey NextScheduledStartPropertyKey = DependencyPropertyBuilder<CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, DateTime?>
            .Register(nameof(NextScheduledStart))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="NextScheduledStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextScheduledStartProperty = NextScheduledStartPropertyKey.DependencyProperty;

        public DateTime? NextScheduledStart { get => (DateTime?)GetValue(NextScheduledStartProperty); private set => SetValue(NextScheduledStartPropertyKey, value); }

        #endregion
        #region RescheduleInterval Property Members

        private static readonly DependencyPropertyKey RescheduleIntervalPropertyKey = DependencyPropertyBuilder<CrawlConfigurationDetailsViewModel<TEntity, TSubdirectoryEntity, TSubdirectoryItem, TCrawlJobLogEntity, TCrawlJobLogItem>, TimeSpan?>
            .Register(nameof(RescheduleInterval))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="RescheduleInterval"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RescheduleIntervalProperty = RescheduleIntervalPropertyKey.DependencyProperty;

        public TimeSpan? RescheduleInterval { get => (TimeSpan?)GetValue(RescheduleIntervalProperty); private set => SetValue(RescheduleIntervalPropertyKey, value); }

        #endregion

        public CrawlConfigurationDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity, state)
        {
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(GoToRootSubdirectoryPropertyKey, new Commands.RelayCommand(RaiseGoToRootSubdirectoryCommand));
            SetValue(ViewLogsPropertyKey, new Commands.RelayCommand(RaiseViewLogsCommand));
            MaxTotalItems = entity.MaxTotalItems;
            NextScheduledStart = entity.NextScheduledStart;
            long? seconds = entity.RescheduleInterval;
            RescheduleInterval = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
            seconds = entity.TTL;
            TTL = seconds.HasValue ? TimeSpan.FromSeconds(seconds.Value) : null;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Entity.Root):
                    ISubdirectory root = Entity.Root;
                    SetRootSubdirectory(root);
                    break;
                case nameof(ICrawlConfigurationRow.MaxTotalItems):
                    Dispatcher.CheckInvoke(() => MaxTotalItems = Entity.MaxTotalItems);
                    break;
                case nameof(ICrawlConfigurationRow.TTL):
                    Dispatcher.CheckInvoke(() =>
                    {
                        long? value = Entity.TTL;
                        TTL = value.HasValue ? TimeSpan.FromSeconds(value.Value) : null;
                    });
                    break;
                case nameof(ICrawlConfigurationRow.NextScheduledStart):
                    Dispatcher.CheckInvoke(() => NextScheduledStart = Entity.NextScheduledStart);
                    break;
                case nameof(ICrawlConfigurationRow.RescheduleInterval):
                    Dispatcher.CheckInvoke(() =>
                    {
                        long? value = Entity.RescheduleInterval;
                        RescheduleInterval = value.HasValue ? TimeSpan.FromSeconds(value.Value) : null;
                    });
                    break;
                default:
                    CheckEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
