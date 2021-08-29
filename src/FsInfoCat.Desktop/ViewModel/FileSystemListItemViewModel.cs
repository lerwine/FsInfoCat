using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileSystemListItemViewModel<TEntity> : FileSystemRowViewModel<TEntity>
        where TEntity : DbEntity, IFileSystemListItem
    {
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

        private static readonly DependencyPropertyKey VolumeCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeCount), typeof(long), typeof(FileSystemListItemViewModel<TEntity>),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="VolumeCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeCountProperty = VolumeCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long VolumeCount { get => (long)GetValue(VolumeCountProperty); private set => SetValue(VolumeCountPropertyKey, value); }

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
