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
                    if (root is null)
                        Dispatcher.CheckInvoke(() => Root = null);
                    else
                    {
                        IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
                        jobFactory.StartNew("Loading data", "Opening database", root.Id, LoadSubdirectoryAsync).Task.ContinueWith(task => Dispatcher.Invoke(() =>
                        {
                            Dispatcher.ShowMessageBoxAsync("Unexpected error while reading from the database. See error logs for more information.",
                                "Database Error", CancellationToken.None);
                        }, DispatcherPriority.Background), TaskContinuationOptions.OnlyOnFaulted);
                    }
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
