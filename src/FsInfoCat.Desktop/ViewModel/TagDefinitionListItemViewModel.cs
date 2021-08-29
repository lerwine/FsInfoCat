using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class TagDefinitionListItemViewModel<TEntity> : TagDefinitionRowViewModel<TEntity>
        where TEntity : DbEntity, ITagDefinitionListItem
    {
        #region FileTagCount Property Members

        private static readonly DependencyPropertyKey FileTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileTagCount), typeof(long), typeof(TagDefinitionListItemViewModel<TEntity>),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="FileTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileTagCountProperty = FileTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long FileTagCount { get => (long)GetValue(FileTagCountProperty); private set => SetValue(FileTagCountPropertyKey, value); }

        #endregion
        #region SubdirectoryTagCount Property Members

        private static readonly DependencyPropertyKey SubdirectoryTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SubdirectoryTagCount), typeof(long),
            typeof(TagDefinitionListItemViewModel<TEntity>), new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="SubdirectoryTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubdirectoryTagCountProperty = SubdirectoryTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SubdirectoryTagCount { get => (long)GetValue(SubdirectoryTagCountProperty); private set => SetValue(SubdirectoryTagCountPropertyKey, value); }

        #endregion
        #region VolumeTagCount Property Members

        private static readonly DependencyPropertyKey VolumeTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VolumeTagCount), typeof(long),
            typeof(TagDefinitionListItemViewModel<TEntity>), new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="VolumeTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeTagCountProperty = VolumeTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long VolumeTagCount { get => (long)GetValue(VolumeTagCountProperty); private set => SetValue(VolumeTagCountPropertyKey, value); }

        #endregion

        public TagDefinitionListItemViewModel(TEntity entity) : base(entity)
        {
            FileTagCount = entity.FileTagCount;
            SubdirectoryTagCount = entity.SubdirectoryTagCount;
            VolumeTagCount = entity.VolumeTagCount;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ITagDefinitionListItem.FileTagCount):
                    Dispatcher.CheckInvoke(() => FileTagCount = Entity.FileTagCount);
                    break;
                case nameof(ITagDefinitionListItem.SubdirectoryTagCount):
                    Dispatcher.CheckInvoke(() => SubdirectoryTagCount = Entity.SubdirectoryTagCount);
                    break;
                case nameof(ITagDefinitionListItem.VolumeTagCount):
                    Dispatcher.CheckInvoke(() => VolumeTagCount = Entity.VolumeTagCount);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
