using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SubdirectoryListItemViewModel<TEntity> : SubdirectoryRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, ISubdirectoryListItem
    {
#pragma warning disable IDE0060 // Remove unused parameter
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

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
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

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
        #region SubdirectoryCount Property Members

        private static readonly DependencyPropertyKey SubdirectoryCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SubdirectoryCount), typeof(long), typeof(SubdirectoryListItemViewModel<TEntity>),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SubdirectoryListItemViewModel<TEntity>).OnSubdirectoryCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="SubdirectoryCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubdirectoryCountProperty = SubdirectoryCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SubdirectoryCount { get => (long)GetValue(SubdirectoryCountProperty); private set => SetValue(SubdirectoryCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="SubdirectoryCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SubdirectoryCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SubdirectoryCount"/> property.</param>
        protected void OnSubdirectoryCountPropertyChanged(long oldValue, long newValue) => SetHasDependentItems(newValue, FileCount, HasCrawlConfig);

        #endregion
        #region FileCount Property Members

        private static readonly DependencyPropertyKey FileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileCount), typeof(long), typeof(SubdirectoryListItemViewModel<TEntity>),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SubdirectoryListItemViewModel<TEntity>).OnFileCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="FileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileCountProperty = FileCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long FileCount { get => (long)GetValue(FileCountProperty); private set => SetValue(FileCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileCount"/> property.</param>
        protected void OnFileCountPropertyChanged(long oldValue, long newValue) => SetHasDependentItems(SubdirectoryCount, newValue, HasCrawlConfig);

        #endregion
        #region CrawlConfigDisplayName Property Members

        private static readonly DependencyPropertyKey CrawlConfigDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CrawlConfigDisplayName), typeof(string),
            typeof(SubdirectoryListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="CrawlConfigDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlConfigDisplayNameProperty = CrawlConfigDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string CrawlConfigDisplayName { get => GetValue(CrawlConfigDisplayNameProperty) as string; private set => SetValue(CrawlConfigDisplayNamePropertyKey, value); }

        #endregion
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AccessErrorCount), typeof(long),
            typeof(SubdirectoryListItemViewModel<TEntity>), new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = AccessErrorCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long AccessErrorCount { get => (long)GetValue(AccessErrorCountProperty); private set => SetValue(AccessErrorCountPropertyKey, value); }

        #endregion
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PersonalTagCount), typeof(long),
            typeof(SubdirectoryListItemViewModel<TEntity>), new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="PersonalTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagCountProperty = PersonalTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long PersonalTagCount { get => (long)GetValue(PersonalTagCountProperty); private set => SetValue(PersonalTagCountPropertyKey, value); }

        #endregion
        #region SharedTagCount Property Members

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SharedTagCount), typeof(long), typeof(SubdirectoryListItemViewModel<TEntity>),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="SharedTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagCountProperty = SharedTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SharedTagCount { get => (long)GetValue(SharedTagCountProperty); private set => SetValue(SharedTagCountPropertyKey, value); }

        #endregion
        #region HasCrawlConfig Property Members

        private static readonly DependencyPropertyKey HasCrawlConfigPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HasCrawlConfig), typeof(bool), typeof(SubdirectoryListItemViewModel<TEntity>),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SubdirectoryListItemViewModel<TEntity>).OnHasCrawlConfigPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="HasCrawlConfig"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasCrawlConfigProperty = HasCrawlConfigPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool HasCrawlConfig { get => (bool)GetValue(HasCrawlConfigProperty); private set => SetValue(HasCrawlConfigPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="HasCrawlConfig"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HasCrawlConfig"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HasCrawlConfig"/> property.</param>
        protected void OnHasCrawlConfigPropertyChanged(bool oldValue, bool newValue) => SetHasDependentItems(SubdirectoryCount, FileCount, newValue);

        #endregion
        #region HasDependentItems Property Members

        private static readonly DependencyPropertyKey HasDependentItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HasDependentItems), typeof(bool), typeof(SubdirectoryListItemViewModel<TEntity>),
                new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="HasDependentItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasDependentItemsProperty = HasDependentItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool HasDependentItems { get => (bool)GetValue(HasDependentItemsProperty); private set => SetValue(HasDependentItemsPropertyKey, value); }

        private void SetHasDependentItems(long subdirectoryCount, long fileCount, bool hasCrawlConfig) => HasDependentItems = hasCrawlConfig || subdirectoryCount > 0L || fileCount > 0L;

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public SubdirectoryListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
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
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
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
