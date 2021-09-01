using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileSystemListItemViewModel<TEntity> : FileSystemRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IFileSystemListItem
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(FileSystemListItemViewModel<TEntity>), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(FileSystemListItemViewModel<TEntity>), new PropertyMetadata(null));

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
        #region PrimarySymbolicName Property Members

        private static readonly DependencyPropertyKey PrimarySymbolicNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(PrimarySymbolicName), typeof(string),
            typeof(FileSystemListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="PrimarySymbolicName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PrimarySymbolicNameProperty = PrimarySymbolicNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string PrimarySymbolicName { get => GetValue(PrimarySymbolicNameProperty) as string; private set => SetValue(PrimarySymbolicNamePropertyKey, value); }

        #endregion
        #region SymbolicNameCount Property Members

        private static readonly DependencyPropertyKey SymbolicNameCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SymbolicNameCount), typeof(long),
            typeof(FileSystemListItemViewModel<TEntity>), new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="SymbolicNameCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolicNameCountProperty = SymbolicNameCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SymbolicNameCount { get => (long)GetValue(SymbolicNameCountProperty); private set => SetValue(SymbolicNameCountPropertyKey, value); }

        #endregion
        #region VolumeCount Property Members

        private static readonly DependencyPropertyKey VolumeCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeCount), typeof(long),
            typeof(FileSystemListItemViewModel<TEntity>), new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as FileSystemListItemViewModel<TEntity>).OnVolumeCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="VolumeCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeCountProperty = VolumeCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long VolumeCount { get => (long)GetValue(VolumeCountProperty); private set => SetValue(VolumeCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="VolumeCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VolumeCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VolumeCount"/> property.</param>
        private void OnVolumeCountPropertyChanged(long oldValue, long newValue) => Delete.IsEnabled = newValue > 0L;

        #endregion

        public FileSystemListItemViewModel(TEntity entity) : base(entity)
        {
            PrimarySymbolicName = entity.PrimarySymbolicName;
            SymbolicNameCount = entity.SymbolicNameCount;
            VolumeCount = entity.VolumeCount;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IFileSystemListItem.PrimarySymbolicName):
                    Dispatcher.CheckInvoke(() => PrimarySymbolicName = Entity.PrimarySymbolicName);
                    break;
                case nameof(IFileSystemListItem.SymbolicNameCount):
                    Dispatcher.CheckInvoke(() => SymbolicNameCount = Entity.SymbolicNameCount);
                    break;
                case nameof(IFileSystemListItem.VolumeCount):
                    Dispatcher.CheckInvoke(() => VolumeCount = Entity.VolumeCount);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
