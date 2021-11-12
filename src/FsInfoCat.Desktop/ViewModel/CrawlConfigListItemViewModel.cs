using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlConfigListItemViewModel<TEntity> : CrawlConfigurationRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, ICrawlConfigurationListItem
    {
        #region Open Command Property Members

        /// <summary>
        /// Occurs when the <see cref="Open"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenCommand;

        private static readonly DependencyPropertyKey OpenPropertyKey = DependencyPropertyBuilder<CrawlConfigListItemViewModel<TEntity>, Commands.RelayCommand>
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
            typeof(Commands.RelayCommand), typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(null));

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
        #region Path Property Members

        private static readonly DependencyPropertyKey PathPropertyKey = ColumnPropertyBuilder<string, CrawlConfigListItemViewModel<TEntity>>
            .Register(nameof(Path))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Path"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        public string Path { get => GetValue(PathProperty) as string; private set => SetValue(PathPropertyKey, value); }

        #endregion
        #region NextScheduledStart Property Members

        /// <summary>
        /// Identifies the <see cref="NextScheduledStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextScheduledStartProperty = ColumnPropertyBuilder<DateTime?, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.NextScheduledStart))
            .DefaultValue(null)
            .AsReadWrite();

        public DateTime? NextScheduledStart { get => (DateTime?)GetValue(NextScheduledStartProperty); set => SetValue(NextScheduledStartProperty, value); }

        #endregion
        #region RescheduleInterval Property Members

        /// <summary>
        /// Identifies the <see cref="RescheduleInterval"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RescheduleIntervalProperty = ColumnPropertyBuilder<TimeSpan?, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.RescheduleInterval))
            .DefaultValue(null)
            .AsReadWrite();

        public TimeSpan? RescheduleInterval { get => (TimeSpan?)GetValue(RescheduleIntervalProperty); set => SetValue(RescheduleIntervalProperty, value); }

        #endregion
        #region TTL Property Members

        /// <summary>
        /// Identifies the <see cref="TTL"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TTLProperty = ColumnPropertyBuilder<TimeSpan?, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.TTL))
            .DefaultValue(null)
            .AsReadWrite();

        public TimeSpan? TTL { get => (TimeSpan?)GetValue(TTLProperty); set => SetValue(TTLProperty, value); }

        #endregion
        #region MaxTotalItems Property Members

        /// <summary>
        /// Identifies the <see cref="MaxTotalItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxTotalItemsProperty = ColumnPropertyBuilder<ulong?, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationRow.MaxTotalItems))
            .DefaultValue(0)
            .AsReadWrite();

        public ulong? MaxTotalItems { get => (ulong?)GetValue(MaxTotalItemsProperty); set => SetValue(MaxTotalItemsProperty, value); }

        #endregion
        #region VolumeDisplayName Property Members

        private static readonly DependencyPropertyKey VolumeDisplayNamePropertyKey = ColumnPropertyBuilder<string, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.VolumeDisplayName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeDisplayNameProperty = VolumeDisplayNamePropertyKey.DependencyProperty;

        public string VolumeDisplayName { get => GetValue(VolumeDisplayNameProperty) as string; private set => SetValue(VolumeDisplayNamePropertyKey, value); }

        #endregion
        #region VolumeName Property Members

        private static readonly DependencyPropertyKey VolumeNamePropertyKey = ColumnPropertyBuilder<string, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.VolumeName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeNameProperty = VolumeNamePropertyKey.DependencyProperty;

        public string VolumeName { get => GetValue(VolumeNameProperty) as string; private set => SetValue(VolumeNamePropertyKey, value); }

        #endregion
        #region VolumeIdentifier Property Members

        private static readonly DependencyPropertyKey VolumeIdentifierPropertyKey = ColumnPropertyBuilder<VolumeIdentifier, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.VolumeIdentifier))
            .DefaultValue(VolumeIdentifier.Empty)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeIdentifier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeIdentifierProperty = VolumeIdentifierPropertyKey.DependencyProperty;

        public VolumeIdentifier VolumeIdentifier { get => (VolumeIdentifier)GetValue(VolumeIdentifierProperty); private set => SetValue(VolumeIdentifierPropertyKey, value); }

        #endregion
        #region FileSystemDisplayName Property Members

        private static readonly DependencyPropertyKey FileSystemDisplayNamePropertyKey = ColumnPropertyBuilder<string, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.FileSystemDisplayName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as CrawlConfigListItemViewModel<TEntity>)?.OnFileSystemDisplayNamePropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="FileSystemDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemDisplayNameProperty = FileSystemDisplayNamePropertyKey.DependencyProperty;

        public string FileSystemDisplayName { get => GetValue(FileSystemDisplayNameProperty) as string; private set => SetValue(FileSystemDisplayNamePropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileSystemDisplayName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileSystemDisplayName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileSystemDisplayName"/> property.</param>
        protected virtual void OnFileSystemDisplayNamePropertyChanged(string oldValue, string newValue) => SetFileSystemShortDescription(newValue, FileSystemSymbolicName);

        #endregion
        #region FileSystemSymbolicName Property Members

        private static readonly DependencyPropertyKey FileSystemSymbolicNamePropertyKey = ColumnPropertyBuilder<string, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ICrawlConfigurationListItem.FileSystemSymbolicName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as CrawlConfigListItemViewModel<TEntity>)?.OnFileSystemSymbolicNamePropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="FileSystemSymbolicName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemSymbolicNameProperty = FileSystemSymbolicNamePropertyKey.DependencyProperty;

        public string FileSystemSymbolicName { get => GetValue(FileSystemSymbolicNameProperty) as string; private set => SetValue(FileSystemSymbolicNamePropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileSystemSymbolicName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileSystemSymbolicName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileSystemSymbolicName"/> property.</param>
        protected virtual void OnFileSystemSymbolicNamePropertyChanged(string oldValue, string newValue) => SetFileSystemShortDescription(FileSystemDisplayName, newValue);

        #endregion
        #region FileSystemShortDescription Property Members

        private static readonly DependencyPropertyKey FileSystemShortDescriptionPropertyKey = ColumnPropertyBuilder<string, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(FileSystemShortDescription), nameof(ICrawlConfigurationListItem.FileSystemDisplayName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="FileSystemShortDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemShortDescriptionProperty = FileSystemShortDescriptionPropertyKey.DependencyProperty;

        public string FileSystemShortDescription { get => GetValue(FileSystemShortDescriptionProperty) as string; private set => SetValue(FileSystemShortDescriptionPropertyKey, value); }

        private void SetFileSystemShortDescription(string displayName, string symbolicName) => FileSystemShortDescription = string.IsNullOrWhiteSpace(displayName) ? symbolicName :
                (string.IsNullOrWhiteSpace(symbolicName) ? displayName : $"{displayName.AsWsNormalizedOrEmpty()} ({symbolicName.AsWsNormalizedOrEmpty()})");

        #endregion
        #region VolumeShortDescription Property Members

        private static readonly DependencyPropertyKey VolumeShortDescriptionPropertyKey = ColumnPropertyBuilder<string, CrawlConfigListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(VolumeShortDescription), nameof(ICrawlConfigurationListItem.VolumeDisplayName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeShortDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeShortDescriptionProperty = VolumeShortDescriptionPropertyKey.DependencyProperty;

        public string VolumeShortDescription { get => GetValue(VolumeShortDescriptionProperty) as string; private set => SetValue(VolumeShortDescriptionPropertyKey, value); }

        private void SetVolumeShortDescription(string displayName, string name, VolumeIdentifier identifier)
        {
            displayName = displayName.AsWsNormalizedOrEmpty();
            name = name.AsWsNormalizedOrEmpty();
            string idStr = identifier.IsEmpty() ? "" : identifier.SerialNumber.HasValue ? VolumeIdentifier.ToVsnString(identifier.SerialNumber.Value, true) :
                identifier.UUID.HasValue ? identifier.UUID.Value.ToString("d") : identifier.Location.IsUnc ? identifier.Location.LocalPath : identifier.Location.ToString();
            if (name.Length > 0 && name.Equals(displayName, StringComparison.InvariantCultureIgnoreCase))
            {
                name = "";
                if (idStr.Length > 0 && idStr.Equals(displayName, StringComparison.InvariantCultureIgnoreCase))
                    idStr = "";
            }
            else if (idStr.Length > 0 && (idStr.Equals(displayName, StringComparison.InvariantCultureIgnoreCase) || idStr.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                idStr = "";
            VolumeShortDescription = displayName.Length > 0
                ? (name.Length > 0) ? ((idStr.Length > 0) ? $"{displayName} ({name} / {idStr})" : $"{displayName} ({name})") :
                    (idStr.Length > 0) ? $"{displayName} ({idStr})" : displayName
                : (name.Length > 0) ? ((idStr.Length > 0) ? $"{name} ({idStr})" : name) : idStr;
        }

        #endregion

        public CrawlConfigListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(OpenPropertyKey, new Commands.RelayCommand(RaiseOpenCommand));
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            Path = EntityExtensions.AncestorNamesToPath(Entity.AncestorNames);
            VolumeDisplayName = entity.VolumeDisplayName;
            VolumeName = entity.VolumeName;
            VolumeIdentifier = entity.VolumeIdentifier;
            FileSystemDisplayName = entity.FileSystemDisplayName;
            FileSystemSymbolicName = entity.FileSystemSymbolicName;
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
                case nameof(ICrawlConfigurationListItem.AncestorNames):
                    Dispatcher.CheckInvoke(() => Path = EntityExtensions.AncestorNamesToPath(Entity.AncestorNames));
                    break;
                case nameof(ICrawlConfigurationListItem.VolumeDisplayName):
                    Dispatcher.CheckInvoke(() => VolumeDisplayName = Entity.VolumeDisplayName);
                    break;
                case nameof(ICrawlConfigurationListItem.VolumeName):
                    Dispatcher.CheckInvoke(() => VolumeName = Entity.VolumeName);
                    break;
                case nameof(ICrawlConfigurationListItem.VolumeIdentifier):
                    Dispatcher.CheckInvoke(() => VolumeIdentifier = Entity.VolumeIdentifier);
                    break;
                case nameof(ICrawlConfigurationListItem.FileSystemDisplayName):
                    Dispatcher.CheckInvoke(() => FileSystemDisplayName = Entity.FileSystemDisplayName);
                    break;
                case nameof(ICrawlConfigurationListItem.FileSystemSymbolicName):
                    Dispatcher.CheckInvoke(() => FileSystemSymbolicName = Entity.FileSystemSymbolicName);
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
