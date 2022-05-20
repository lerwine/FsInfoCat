using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SubdirectoryListItemWithAncestorNamesViewModel<TEntity> : SubdirectoryListItemViewModel<TEntity>, ISubdirectoryListItemWithAncestorNamesViewModel
        where TEntity : Model.DbEntity, Model.ISubdirectoryListItemWithAncestorNames
    {
        #region VolumeDisplayName Property Members

        private static readonly DependencyPropertyKey VolumeDisplayNamePropertyKey = ColumnPropertyBuilder<string, SubdirectoryListItemWithAncestorNamesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISubdirectoryListItemWithAncestorNames.VolumeDisplayName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeDisplayNameProperty = VolumeDisplayNamePropertyKey.DependencyProperty;

        public string VolumeDisplayName { get => GetValue(VolumeDisplayNameProperty) as string; private set => SetValue(VolumeDisplayNamePropertyKey, value); }

        #endregion
        #region VolumeName Property Members

        private static readonly DependencyPropertyKey VolumeNamePropertyKey = ColumnPropertyBuilder<string, SubdirectoryListItemWithAncestorNamesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISubdirectoryListItemWithAncestorNames.VolumeName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeNameProperty = VolumeNamePropertyKey.DependencyProperty;

        public string VolumeName { get => GetValue(VolumeNameProperty) as string; private set => SetValue(VolumeNamePropertyKey, value); }

        #endregion
        #region VolumeIdentifier Property Members

        private static readonly DependencyPropertyKey VolumeIdentifierPropertyKey = ColumnPropertyBuilder<Model.VolumeIdentifier, SubdirectoryListItemWithAncestorNamesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISubdirectoryListItemWithAncestorNames.VolumeIdentifier))
            .DefaultValue(Model.VolumeIdentifier.Empty)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeIdentifier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeIdentifierProperty = VolumeIdentifierPropertyKey.DependencyProperty;

        public Model.VolumeIdentifier VolumeIdentifier { get => (Model.VolumeIdentifier)GetValue(VolumeIdentifierProperty); private set => SetValue(VolumeIdentifierPropertyKey, value); }

        #endregion
        #region FileSystemDisplayName Property Members

        private static readonly DependencyPropertyKey FileSystemDisplayNamePropertyKey = ColumnPropertyBuilder<string, SubdirectoryListItemWithAncestorNamesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISubdirectoryListItemWithAncestorNames.FileSystemDisplayName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SubdirectoryListItemWithAncestorNamesViewModel<TEntity>)?.OnFileSystemDisplayNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadOnly();

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

        private static readonly DependencyPropertyKey FileSystemSymbolicNamePropertyKey = ColumnPropertyBuilder<string, SubdirectoryListItemWithAncestorNamesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISubdirectoryListItemWithAncestorNames.FileSystemSymbolicName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SubdirectoryListItemWithAncestorNamesViewModel<TEntity>)?.OnFileSystemSymbolicNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadOnly();

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
        #region Path Property Members

        private static readonly DependencyPropertyKey PathPropertyKey = ColumnPropertyBuilder<string, SubdirectoryListItemWithAncestorNamesViewModel<TEntity>>
            .Register(nameof(Path))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Path"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        public string Path { get => GetValue(PathProperty) as string; private set => SetValue(PathPropertyKey, value); }

        #endregion
        #region FileSystemShortDescription Property Members

        private static readonly DependencyPropertyKey FileSystemShortDescriptionPropertyKey = ColumnPropertyBuilder<string, SubdirectoryListItemWithAncestorNamesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(FileSystemShortDescription), nameof(Model.ISubdirectoryListItemWithAncestorNames.FileSystemDisplayName))
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

        private static readonly DependencyPropertyKey VolumeShortDescriptionPropertyKey = ColumnPropertyBuilder<string, SubdirectoryListItemWithAncestorNamesViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(VolumeShortDescription), nameof(Model.ISubdirectoryListItemWithAncestorNames.VolumeDisplayName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VolumeShortDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeShortDescriptionProperty = VolumeShortDescriptionPropertyKey.DependencyProperty;

        public string VolumeShortDescription { get => GetValue(VolumeShortDescriptionProperty) as string; private set => SetValue(VolumeShortDescriptionPropertyKey, value); }

        private void SetVolumeShortDescription(string displayName, string name, Model.VolumeIdentifier identifier)
        {
            displayName = displayName.AsWsNormalizedOrEmpty();
            name = name.AsWsNormalizedOrEmpty();
            string idStr = identifier.IsEmpty() ? "" : identifier.SerialNumber.HasValue ? Model.VolumeIdentifier.ToVsnString(identifier.SerialNumber.Value, true) :
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

        Model.ISubdirectoryListItemWithAncestorNames ISubdirectoryListItemWithAncestorNamesViewModel.Entity => Entity;

        public SubdirectoryListItemWithAncestorNamesViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            VolumeDisplayName = entity.VolumeDisplayName;
            VolumeName = entity.VolumeName;
            VolumeIdentifier = entity.VolumeIdentifier;
            FileSystemDisplayName = entity.FileSystemDisplayName;
            FileSystemSymbolicName = entity.FileSystemSymbolicName;
            string parentDirectory = EntityExtensions.AncestorNamesToPath(entity.AncestorNames);
            Path = string.IsNullOrEmpty(parentDirectory) ? Name : System.IO.Path.Combine(parentDirectory, Name);
        }
    }
}
