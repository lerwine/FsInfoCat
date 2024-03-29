using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileSystemListItemViewModel<TEntity> : FileSystemRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IFileSystemListItem
    {
        #region Open Command Property Members

        /// <summary>
        /// Occurs when the <see cref="Open"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenCommand;

        private static readonly DependencyPropertyKey OpenPropertyKey = DependencyPropertyBuilder<FileSystemListItemViewModel<TEntity>, Commands.RelayCommand>
            .Register(nameof(Open))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Open"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenProperty = OpenPropertyKey.DependencyProperty;

        public Commands.RelayCommand Open => (Commands.RelayCommand)GetValue(OpenProperty);

        /// <summary>
        /// Called when the Open event is raised by <see cref="Open" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Open" />.</param>
        protected void RaiseOpenCommand(object parameter) => OpenCommand?.Invoke(this, new(parameter));

        #endregion
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

        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
        #region PrimarySymbolicName Property Members

        private static readonly DependencyPropertyKey PrimarySymbolicNamePropertyKey = ColumnPropertyBuilder<string, FileSystemListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IFileSystemListItem.PrimarySymbolicName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PrimarySymbolicName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PrimarySymbolicNameProperty = PrimarySymbolicNamePropertyKey.DependencyProperty;

        public string PrimarySymbolicName { get => GetValue(PrimarySymbolicNameProperty) as string; private set => SetValue(PrimarySymbolicNamePropertyKey, value); }

        #endregion
        #region SymbolicNameCount Property Members

        private static readonly DependencyPropertyKey SymbolicNameCountPropertyKey = ColumnPropertyBuilder<long, FileSystemListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IFileSystemListItem.SymbolicNameCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SymbolicNameCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolicNameCountProperty = SymbolicNameCountPropertyKey.DependencyProperty;

        public long SymbolicNameCount { get => (long)GetValue(SymbolicNameCountProperty); private set => SetValue(SymbolicNameCountPropertyKey, value); }

        #endregion
        #region VolumeCount Property Members

        private static readonly DependencyPropertyKey VolumeCountPropertyKey = ColumnPropertyBuilder<long, FileSystemListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IFileSystemListItem.VolumeCount))
            .DefaultValue(0L)
            .OnChanged((d, oldValue, newValue) => (d as FileSystemListItemViewModel<TEntity>)?.OnVolumeCountPropertyChanged(newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeCountProperty = VolumeCountPropertyKey.DependencyProperty;

        public long VolumeCount { get => (long)GetValue(VolumeCountProperty); private set => SetValue(VolumeCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="VolumeCount"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="VolumeCount"/> property.</param>
        ///
        private void OnVolumeCountPropertyChanged(long newValue) => Delete.IsEnabled = newValue > 0L;

        #endregion

        public FileSystemListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(OpenPropertyKey, new Commands.RelayCommand(RaiseOpenCommand));
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            PrimarySymbolicName = entity.PrimarySymbolicName;
            SymbolicNameCount = entity.SymbolicNameCount;
            VolumeCount = entity.VolumeCount;
        }
    }
}
