using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VolumeListItemWithFileSystemViewModel<TEntity> : VolumeListItemViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IVolumeListItemWithFileSystem
    {
        #region FileSystemDisplayName Property Members

        private static readonly DependencyPropertyKey FileSystemDisplayNamePropertyKey = ColumnPropertyBuilder<string, VolumeListItemWithFileSystemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeListItemWithFileSystem.FileSystemDisplayName))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="FileSystemDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemDisplayNameProperty = FileSystemDisplayNamePropertyKey.DependencyProperty;

        public string FileSystemDisplayName { get => GetValue(FileSystemDisplayNameProperty) as string; private set => SetValue(FileSystemDisplayNamePropertyKey, value); }

        #endregion
        #region EffectiveReadOnly Property Members

        private static readonly DependencyPropertyKey EffectiveReadOnlyPropertyKey = ColumnPropertyBuilder<bool, VolumeListItemWithFileSystemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeListItemWithFileSystem.EffectiveReadOnly))
            .DefaultValue(false)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="EffectiveReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectiveReadOnlyProperty = EffectiveReadOnlyPropertyKey.DependencyProperty;

        public bool EffectiveReadOnly { get => (bool)GetValue(EffectiveReadOnlyProperty); private set => SetValue(EffectiveReadOnlyPropertyKey, value); }

        #endregion
        #region EffectiveMaxNameLength Property Members

        private static readonly DependencyPropertyKey EffectiveMaxNameLengthPropertyKey = ColumnPropertyBuilder<uint, VolumeListItemWithFileSystemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeListItemWithFileSystem.EffectiveMaxNameLength))
            .DefaultValue(DbConstants.DbColDefaultValue_MaxNameLength)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="EffectiveMaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectiveMaxNameLengthProperty = EffectiveMaxNameLengthPropertyKey.DependencyProperty;

        public uint EffectiveMaxNameLength { get => (uint)GetValue(EffectiveMaxNameLengthProperty); private set => SetValue(EffectiveMaxNameLengthPropertyKey, value); }

        #endregion

        public VolumeListItemWithFileSystemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            FileSystemDisplayName = entity.FileSystemDisplayName;
            EffectiveReadOnly = entity.EffectiveReadOnly;
            EffectiveMaxNameLength = entity.EffectiveMaxNameLength;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.IVolumeListItemWithFileSystem.FileSystemDisplayName):
                    Dispatcher.CheckInvoke(() => FileSystemDisplayName = Entity.FileSystemDisplayName);
                    break;
                case nameof(Model.IVolumeListItemWithFileSystem.EffectiveReadOnly):
                    Dispatcher.CheckInvoke(() => EffectiveReadOnly = Entity.EffectiveReadOnly);
                    break;
                case nameof(Model.IVolumeListItemWithFileSystem.EffectiveMaxNameLength):
                    Dispatcher.CheckInvoke(() => EffectiveMaxNameLength = Entity.EffectiveMaxNameLength);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
