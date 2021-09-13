using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SubdirectoryListItemViewModel<TEntity> : SubdirectoryRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>, ISubdirectoryListItemViewModel
        where TEntity : DbEntity, ISubdirectoryListItem
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(SubdirectoryListItemViewModel<TEntity>), new PropertyMetadata(null));

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
        #region Delete Property Members

        /// <summary>
        /// Occurs when the <see cref="Delete">Delete Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> DeleteCommand;

        private static readonly DependencyPropertyKey DeletePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Delete),
            typeof(Commands.RelayCommand), typeof(SubdirectoryListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Delete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteProperty = DeletePropertyKey.DependencyProperty;

        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
        #region SubdirectoryCount Property Members

        private static readonly DependencyPropertyKey SubdirectoryCountPropertyKey = ColumnPropertyBuilder<long, SubdirectoryListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISubdirectoryListItem.SubdirectoryCount))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as SubdirectoryListItemViewModel<TEntity>)?.OnSubdirectoryCountPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SubdirectoryCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubdirectoryCountProperty = SubdirectoryCountPropertyKey.DependencyProperty;

        public long SubdirectoryCount { get => (long)GetValue(SubdirectoryCountProperty); private set => SetValue(SubdirectoryCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="SubdirectoryCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SubdirectoryCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SubdirectoryCount"/> property.</param>
        protected virtual void OnSubdirectoryCountPropertyChanged(long oldValue, long newValue) => SetHasDependentItems(newValue, FileCount, HasCrawlConfig);

        #endregion
        #region FileCount Property Members

        private static readonly DependencyPropertyKey FileCountPropertyKey = ColumnPropertyBuilder<long, SubdirectoryListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISubdirectoryListItem.FileCount))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as SubdirectoryListItemViewModel<TEntity>)?.OnFileCountPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="FileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileCountProperty = FileCountPropertyKey.DependencyProperty;

        public long FileCount { get => (long)GetValue(FileCountProperty); private set => SetValue(FileCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileCount"/> property.</param>
        protected virtual void OnFileCountPropertyChanged(long oldValue, long newValue) => SetHasDependentItems(SubdirectoryCount, newValue, HasCrawlConfig);

        #endregion
        #region CrawlConfigDisplayName Property Members

        private static readonly DependencyPropertyKey CrawlConfigDisplayNamePropertyKey = ColumnPropertyBuilder<string, SubdirectoryListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISubdirectoryListItem.CrawlConfigDisplayName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="CrawlConfigDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlConfigDisplayNameProperty = CrawlConfigDisplayNamePropertyKey.DependencyProperty;

        public string CrawlConfigDisplayName { get => GetValue(CrawlConfigDisplayNameProperty) as string; private set => SetValue(CrawlConfigDisplayNamePropertyKey, value); }

        #endregion
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = ColumnPropertyBuilder<long, SubdirectoryListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISubdirectoryListItem.AccessErrorCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = AccessErrorCountPropertyKey.DependencyProperty;

        public long AccessErrorCount { get => (long)GetValue(AccessErrorCountProperty); private set => SetValue(AccessErrorCountPropertyKey, value); }

        #endregion
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = ColumnPropertyBuilder<long, SubdirectoryListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISubdirectoryListItem.PersonalTagCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PersonalTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagCountProperty = PersonalTagCountPropertyKey.DependencyProperty;

        public long PersonalTagCount { get => (long)GetValue(PersonalTagCountProperty); private set => SetValue(PersonalTagCountPropertyKey, value); }

        #endregion
        #region SharedTagCount Property Members

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = ColumnPropertyBuilder<long, SubdirectoryListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISubdirectoryListItem.SharedTagCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SharedTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagCountProperty = SharedTagCountPropertyKey.DependencyProperty;

        public long SharedTagCount { get => (long)GetValue(SharedTagCountProperty); private set => SetValue(SharedTagCountPropertyKey, value); }

        #endregion
        #region HasCrawlConfig Property Members

        private static readonly DependencyPropertyKey HasCrawlConfigPropertyKey = ColumnPropertyBuilder<bool, SubdirectoryListItemViewModel<TEntity>>
            .Register(nameof(HasCrawlConfig))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as SubdirectoryListItemViewModel<TEntity>)?.OnHasCrawlConfigPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasCrawlConfig"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasCrawlConfigProperty = HasCrawlConfigPropertyKey.DependencyProperty;

        public bool HasCrawlConfig { get => (bool)GetValue(HasCrawlConfigProperty); private set => SetValue(HasCrawlConfigPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="HasCrawlConfig"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HasCrawlConfig"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HasCrawlConfig"/> property.</param>
        protected virtual void OnHasCrawlConfigPropertyChanged(bool oldValue, bool newValue) => SetHasDependentItems(SubdirectoryCount, FileCount, newValue);

        #endregion
        #region HasDependentItems Property Members

        private static readonly DependencyPropertyKey HasDependentItemsPropertyKey = ColumnPropertyBuilder<bool, SubdirectoryListItemViewModel<TEntity>>
            .Register(nameof(HasDependentItems))
            .DefaultValue(false)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="HasDependentItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasDependentItemsProperty = HasDependentItemsPropertyKey.DependencyProperty;

        public bool HasDependentItems { get => (bool)GetValue(HasDependentItemsProperty); private set => SetValue(HasDependentItemsPropertyKey, value); }

        private void SetHasDependentItems(long subdirectoryCount, long fileCount, bool hasCrawlConfig) => HasDependentItems = hasCrawlConfig || subdirectoryCount > 0L || fileCount > 0L;

        #endregion

        IDbFsItemListItem IFsItemListItemViewModel.Entity => Entity;

        ISubdirectoryListItem ISubdirectoryListItemViewModel.Entity => Entity;

        public SubdirectoryListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            SubdirectoryCount = entity.SubdirectoryCount;
            FileCount = entity.FileCount;
            CrawlConfigDisplayName = entity.CrawlConfigDisplayName;
            AccessErrorCount = entity.AccessErrorCount;
            PersonalTagCount = entity.PersonalTagCount;
            SharedTagCount = entity.SharedTagCount;
            string name = entity.CrawlConfigDisplayName;
            if (name is null)
            {
                HasCrawlConfig = false;
                CrawlConfigDisplayName = "";
            }
            else
            {
                HasCrawlConfig = true;
                CrawlConfigDisplayName = name;
            }
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ISubdirectoryListItem.SubdirectoryCount):
                    Dispatcher.CheckInvoke(() => SubdirectoryCount = Entity.SubdirectoryCount);
                    break;
                case nameof(ISubdirectoryListItem.FileCount):
                    Dispatcher.CheckInvoke(() => FileCount = Entity.FileCount);
                    break;
                case nameof(ISubdirectoryListItem.CrawlConfigDisplayName):
                    string name = Entity.CrawlConfigDisplayName;
                    if (name is null)
                        Dispatcher.CheckInvoke(() =>
                        {
                            HasCrawlConfig = false;
                            CrawlConfigDisplayName = "";
                        });

                    else
                        Dispatcher.CheckInvoke(() =>
                        {
                            HasCrawlConfig = true;
                            CrawlConfigDisplayName = name;
                        });
                    break;
                case nameof(ISubdirectoryListItem.AccessErrorCount):
                    Dispatcher.CheckInvoke(() => AccessErrorCount = Entity.AccessErrorCount);
                    break;
                case nameof(ISubdirectoryListItem.PersonalTagCount):
                    Dispatcher.CheckInvoke(() => PersonalTagCount = Entity.PersonalTagCount);
                    break;
                case nameof(ISubdirectoryListItem.SharedTagCount):
                    Dispatcher.CheckInvoke(() => SharedTagCount = Entity.SharedTagCount);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
