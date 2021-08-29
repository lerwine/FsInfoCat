using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VolumeListItemViewModel<TEntity> : VolumeRowViewModel<TEntity>
        where TEntity : DbEntity, IVolumeListItem
    {
        #region RootPath Property Members

        private static readonly DependencyPropertyKey RootPathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RootPath), typeof(string), typeof(VolumeListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="RootPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootPathProperty = RootPathPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string RootPath { get => GetValue(RootPathProperty) as string; private set => SetValue(RootPathPropertyKey, value); }

        #endregion
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AccessErrorCount), typeof(long), typeof(VolumeListItemViewModel<TEntity>),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = AccessErrorCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long AccessErrorCount { get => (long)GetValue(AccessErrorCountProperty); private set => SetValue(AccessErrorCountPropertyKey, value); }

        #endregion
        #region SharedTagCount Property Members

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SharedTagCount), typeof(long), typeof(VolumeListItemViewModel<TEntity>),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="SharedTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagCountProperty = SharedTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SharedTagCount { get => (long)GetValue(SharedTagCountProperty); private set => SetValue(SharedTagCountPropertyKey, value); }

        #endregion
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PersonalTagCount), typeof(long), typeof(VolumeListItemViewModel<TEntity>),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="PersonalTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagCountProperty = PersonalTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long PersonalTagCount { get => (long)GetValue(PersonalTagCountProperty); private set => SetValue(PersonalTagCountPropertyKey, value); }

        #endregion

        public VolumeListItemViewModel(TEntity entity) : base(entity)
        {
            RootPath = entity.RootPath;
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
