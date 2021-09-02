using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlConfigListItemViewModel<TEntity> : CrawlConfigurationRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, ICrawlConfigurationListItem
    {
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
            typeof(Commands.RelayCommand), typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(null));

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
        #region Path Property Members

        private static readonly DependencyPropertyKey PathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Path), typeof(string), typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Path"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Path { get => GetValue(PathProperty) as string; private set => SetValue(PathPropertyKey, value); }

        #endregion
        #region LastCrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="LastCrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastCrawlEndProperty = DependencyProperty.Register(nameof(LastCrawlEnd), typeof(DateTime?),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? LastCrawlEnd { get => (DateTime?)GetValue(LastCrawlEndProperty); set => SetValue(LastCrawlEndProperty, value); }

        #endregion
        #region LastCrawlStart Property Members

        /// <summary>
        /// Identifies the <see cref="LastCrawlStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastCrawlStartProperty = DependencyProperty.Register(nameof(LastCrawlStart), typeof(DateTime?),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? LastCrawlStart { get => (DateTime?)GetValue(LastCrawlStartProperty); set => SetValue(LastCrawlStartProperty, value); }

        #endregion
        #region NextScheduledStart Property Members

        /// <summary>
        /// Identifies the <see cref="NextScheduledStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextScheduledStartProperty = DependencyProperty.Register(nameof(NextScheduledStart), typeof(DateTime?),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? NextScheduledStart { get => (DateTime?)GetValue(NextScheduledStartProperty); set => SetValue(NextScheduledStartProperty, value); }

        #endregion
        #region RescheduleInterval Property Members

        /// <summary>
        /// Identifies the <see cref="RescheduleInterval"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RescheduleIntervalProperty = DependencyProperty.Register(nameof(RescheduleInterval), typeof(TimeSpan?),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public TimeSpan? RescheduleInterval { get => (TimeSpan?)GetValue(RescheduleIntervalProperty); set => SetValue(RescheduleIntervalProperty, value); }

        #endregion
        #region StatusValue Property Members

        /// <summary>
        /// Identifies the <see cref="StatusValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusValueProperty = DependencyProperty.Register(nameof(StatusValue), typeof(CrawlStatus),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(CrawlStatus.NotRunning));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public CrawlStatus StatusValue { get => (CrawlStatus)GetValue(StatusValueProperty); set => SetValue(StatusValueProperty, value); }

        #endregion
        #region TTL Property Members

        /// <summary>
        /// Identifies the <see cref="TTL"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TTLProperty = DependencyProperty.Register(nameof(TTL), typeof(TimeSpan?),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public TimeSpan? TTL { get => (TimeSpan?)GetValue(TTLProperty); set => SetValue(TTLProperty, value); }

        #endregion
        #region VolumeDisplayName Property Members

        private static readonly DependencyPropertyKey VolumeDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeDisplayName), typeof(string),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="VolumeDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeDisplayNameProperty = VolumeDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string VolumeDisplayName { get => GetValue(VolumeDisplayNameProperty) as string; private set => SetValue(VolumeDisplayNamePropertyKey, value); }

        #endregion
        #region VolumeName Property Members

        private static readonly DependencyPropertyKey VolumeNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeName), typeof(string),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="VolumeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeNameProperty = VolumeNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string VolumeName { get => GetValue(VolumeNameProperty) as string; private set => SetValue(VolumeNamePropertyKey, value); }

        #endregion
        #region VolumeIdentifier Property Members

        private static readonly DependencyPropertyKey VolumeIdentifierPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeIdentifier), typeof(VolumeIdentifier),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(VolumeIdentifier.Empty));

        /// <summary>
        /// Identifies the <see cref="VolumeIdentifier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeIdentifierProperty = VolumeIdentifierPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public VolumeIdentifier VolumeIdentifier { get => (VolumeIdentifier)GetValue(VolumeIdentifierProperty); private set => SetValue(VolumeIdentifierPropertyKey, value); }

        #endregion
        #region FileSystemDisplayName Property Members

        private static readonly DependencyPropertyKey FileSystemDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemDisplayName), typeof(string),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as CrawlConfigListItemViewModel<TEntity>)?.OnFileSystemDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Identifies the <see cref="FileSystemDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemDisplayNameProperty = FileSystemDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public string FileSystemDisplayName { get => GetValue(FileSystemDisplayNameProperty) as string; private set => SetValue(FileSystemDisplayNamePropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileSystemDisplayName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileSystemDisplayName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileSystemDisplayName"/> property.</param>
        protected void OnFileSystemDisplayNamePropertyChanged(string oldValue, string newValue) => SetFileSystemShortDescription(newValue, FileSystemSymbolicName);

        #endregion
        #region FileSystemSymbolicName Property Members

        private static readonly DependencyPropertyKey FileSystemSymbolicNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemSymbolicName), typeof(string),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as CrawlConfigListItemViewModel<TEntity>)?.OnFileSystemSymbolicNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Identifies the <see cref="FileSystemSymbolicName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemSymbolicNameProperty = FileSystemSymbolicNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public string FileSystemSymbolicName { get => GetValue(FileSystemSymbolicNameProperty) as string; private set => SetValue(FileSystemSymbolicNamePropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileSystemSymbolicName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileSystemSymbolicName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileSystemSymbolicName"/> property.</param>
        protected void OnFileSystemSymbolicNamePropertyChanged(string oldValue, string newValue) => SetFileSystemShortDescription(FileSystemDisplayName, newValue);

        #endregion
        #region FileSystemShortDescription Property Members

        private static readonly DependencyPropertyKey FileSystemShortDescriptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemShortDescription), typeof(string),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="FileSystemShortDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemShortDescriptionProperty = FileSystemShortDescriptionPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string FileSystemShortDescription { get => GetValue(FileSystemShortDescriptionProperty) as string; private set => SetValue(FileSystemShortDescriptionPropertyKey, value); }

        private void SetFileSystemShortDescription(string displayName, string symbolicName) => FileSystemShortDescription = string.IsNullOrWhiteSpace(displayName) ? symbolicName :
                (string.IsNullOrWhiteSpace(symbolicName) ? displayName : $"{displayName.AsWsNormalizedOrEmpty()} ({symbolicName.AsWsNormalizedOrEmpty()})");

        #endregion
        #region VolumeShortDescription Property Members

        private static readonly DependencyPropertyKey VolumeShortDescriptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeShortDescription), typeof(string),
            typeof(CrawlConfigListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="VolumeShortDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeShortDescriptionProperty = VolumeShortDescriptionPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string VolumeShortDescription { get => GetValue(VolumeShortDescriptionProperty) as string; private set => SetValue(VolumeShortDescriptionPropertyKey, value); }

        private void SetVolumeShortDescription(string displayName, string name, VolumeIdentifier identifier)
        {
            displayName = displayName.AsWsNormalizedOrEmpty();
            name = name.AsWsNormalizedOrEmpty();
            string idStr = identifier.IsEmpty() ? null : identifier.SerialNumber.HasValue ? VolumeIdentifier.ToVsnString(identifier.SerialNumber.Value, true) :
                identifier.UUID.HasValue ? identifier.UUID.Value.ToString("d") : identifier.Location.IsUnc ? identifier.Location.LocalPath : identifier.Location.ToString();
            if (name.Length > 0 && name.Equals(displayName, StringComparison.InvariantCultureIgnoreCase))
            {
                name = "";
                if (idStr.Length > 0 && idStr.Equals(displayName, StringComparison.InvariantCultureIgnoreCase))
                    idStr = "";
            }
            else if (idStr.Length > 0 && (idStr.Equals(displayName, StringComparison.InvariantCultureIgnoreCase) || idStr.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                idStr = "";
            if (displayName.Length > 0)
                VolumeShortDescription = (name.Length > 0) ? ((idStr.Length > 0) ? $"{displayName} ({name} / {idStr})" : $"{displayName} ({name})") :
                    (idStr.Length > 0) ? $"{displayName} ({idStr})" : displayName;
            else
                VolumeShortDescription = (name.Length > 0) ? ((idStr.Length > 0) ? $"{name} ({idStr})" : name) : idStr;
        }

        #endregion

        public CrawlConfigListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Path = EntityExtensions.AncestorNamesToPath(Entity.AncestorNames);
            VolumeDisplayName = entity.VolumeDisplayName;
            VolumeName = entity.VolumeName;
            VolumeIdentifier = entity.VolumeIdentifier;
            FileSystemDisplayName = entity.FileSystemDisplayName;
            FileSystemSymbolicName = entity.FileSystemSymbolicName;
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
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
                default:
                    CheckEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
