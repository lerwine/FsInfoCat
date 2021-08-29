using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VolumeListItemWithFileSystemViewModel<TEntity> : VolumeListItemViewModel<TEntity>
        where TEntity : DbEntity, IVolumeListItemWithFileSystem
    {
        #region FileSystemDisplayName Property Members

        private static readonly DependencyPropertyKey FileSystemDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileSystemDisplayName), typeof(string),
            typeof(VolumeListItemWithFileSystemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="FileSystemDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemDisplayNameProperty = FileSystemDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string FileSystemDisplayName { get => GetValue(FileSystemDisplayNameProperty) as string; private set => SetValue(FileSystemDisplayNamePropertyKey, value); }

        #endregion
        #region EffectiveReadOnly Property Members

        private static readonly DependencyPropertyKey EffectiveReadOnlyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EffectiveReadOnly), typeof(bool),
            typeof(VolumeListItemWithFileSystemViewModel<TEntity>), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="EffectiveReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectiveReadOnlyProperty = EffectiveReadOnlyPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool EffectiveReadOnly { get => (bool)GetValue(EffectiveReadOnlyProperty); private set => SetValue(EffectiveReadOnlyPropertyKey, value); }

        #endregion
        #region EffectiveMaxNameLength Property Members

        private static readonly DependencyPropertyKey EffectiveMaxNameLengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EffectiveMaxNameLength), typeof(uint),
            typeof(VolumeListItemWithFileSystemViewModel<TEntity>), new PropertyMetadata(DbConstants.DbColDefaultValue_MaxNameLength));

        /// <summary>
        /// Identifies the <see cref="EffectiveMaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectiveMaxNameLengthProperty = EffectiveMaxNameLengthPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public uint EffectiveMaxNameLength { get => (uint)GetValue(EffectiveMaxNameLengthProperty); private set => SetValue(EffectiveMaxNameLengthPropertyKey, value); }

        #endregion

        public VolumeListItemWithFileSystemViewModel(TEntity entity) : base(entity)
        {
            FileSystemDisplayName = entity.FileSystemDisplayName;
            EffectiveReadOnly = entity.EffectiveReadOnly;
            EffectiveMaxNameLength = entity.EffectiveMaxNameLength;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IVolumeListItemWithFileSystem.FileSystemDisplayName):
                    Dispatcher.CheckInvoke(() => FileSystemDisplayName = Entity.FileSystemDisplayName);
                    break;
                case nameof(IVolumeListItemWithFileSystem.EffectiveReadOnly):
                    Dispatcher.CheckInvoke(() => EffectiveReadOnly = Entity.EffectiveReadOnly);
                    break;
                case nameof(IVolumeListItemWithFileSystem.EffectiveMaxNameLength):
                    Dispatcher.CheckInvoke(() => EffectiveMaxNameLength = Entity.EffectiveMaxNameLength);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
