using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VolumeListItemViewModel<TEntity> : VolumeRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IVolumeListItem
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(VolumeListItemViewModel<TEntity>), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(VolumeListItemViewModel<TEntity>), new PropertyMetadata(null));

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
        #region RootPath Property Members

        private static readonly DependencyPropertyKey RootPathPropertyKey = ColumnPropertyBuilder<string, VolumeListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeListItem.RootPath))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="RootPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootPathProperty = RootPathPropertyKey.DependencyProperty;

        public string RootPath { get => GetValue(RootPathProperty) as string; private set => SetValue(RootPathPropertyKey, value); }

        #endregion
        #region RootSubdirectoryCount Property Members

        private static readonly DependencyPropertyKey RootSubdirectoryCountPropertyKey = ColumnPropertyBuilder<long, VolumeListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeListItem.RootSubdirectoryCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="RootSubdirectoryCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootSubdirectoryCountProperty = RootSubdirectoryCountPropertyKey.DependencyProperty;

        public long RootSubdirectoryCount { get => (long)GetValue(RootSubdirectoryCountProperty); private set => SetValue(RootSubdirectoryCountPropertyKey, value); }

        #endregion
        #region RootFileCount Property Members

        private static readonly DependencyPropertyKey RootFileCountPropertyKey = ColumnPropertyBuilder<long, VolumeListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeListItem.RootFileCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="RootFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootFileCountProperty = RootFileCountPropertyKey.DependencyProperty;

        public long RootFileCount { get => (long)GetValue(RootFileCountProperty); private set => SetValue(RootFileCountPropertyKey, value); }

        #endregion
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = ColumnPropertyBuilder<long, VolumeListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeListItem.AccessErrorCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = AccessErrorCountPropertyKey.DependencyProperty;

        public long AccessErrorCount { get => (long)GetValue(AccessErrorCountProperty); private set => SetValue(AccessErrorCountPropertyKey, value); }

        #endregion
        #region SharedTagCount Property Members

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = ColumnPropertyBuilder<long, VolumeListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeListItem.SharedTagCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SharedTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagCountProperty = SharedTagCountPropertyKey.DependencyProperty;

        public long SharedTagCount { get => (long)GetValue(SharedTagCountProperty); private set => SetValue(SharedTagCountPropertyKey, value); }

        #endregion
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = ColumnPropertyBuilder<long, VolumeListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeListItem.PersonalTagCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PersonalTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagCountProperty = PersonalTagCountPropertyKey.DependencyProperty;

        public long PersonalTagCount { get => (long)GetValue(PersonalTagCountProperty); private set => SetValue(PersonalTagCountPropertyKey, value); }

        #endregion

        public VolumeListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            RootPath = entity.RootPath;
            RootSubdirectoryCount = entity.RootSubdirectoryCount;
            RootFileCount = entity.RootFileCount;
            AccessErrorCount = entity.AccessErrorCount;
            SharedTagCount = entity.SharedTagCount;
            PersonalTagCount = entity.PersonalTagCount;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IVolumeListItem.RootPath):
                    Dispatcher.CheckInvoke(() => RootPath = Entity.RootPath);
                    break;
                case nameof(IVolumeListItem.RootSubdirectoryCount):
                    Dispatcher.CheckInvoke(() => RootSubdirectoryCount = Entity.RootSubdirectoryCount);
                    break;
                case nameof(IVolumeListItem.RootFileCount):
                    Dispatcher.CheckInvoke(() => RootFileCount = Entity.RootFileCount);
                    break;
                case nameof(IVolumeListItem.AccessErrorCount):
                    Dispatcher.CheckInvoke(() => AccessErrorCount = Entity.AccessErrorCount);
                    break;
                case nameof(IVolumeListItem.SharedTagCount):
                    Dispatcher.CheckInvoke(() => SharedTagCount = Entity.SharedTagCount);
                    break;
                case nameof(IVolumeListItem.PersonalTagCount):
                    Dispatcher.CheckInvoke(() => PersonalTagCount = Entity.PersonalTagCount);
                    break;
                default:
                    CheckEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
