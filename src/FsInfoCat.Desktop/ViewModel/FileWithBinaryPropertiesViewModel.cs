using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileWithBinaryPropertiesViewModel<TEntity> : FileRowViewModel<TEntity>
        where TEntity : DbEntity, IFileListItemWithBinaryProperties
    {
        #region Length Property Members

        private static readonly DependencyPropertyKey LengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Length), typeof(long), typeof(FileWithBinaryPropertiesViewModel<TEntity>),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = LengthPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long Length { get => (long)GetValue(LengthProperty); private set => SetValue(LengthPropertyKey, value); }

        #endregion
        #region Hash Property Members

        private static readonly DependencyPropertyKey HashPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Hash), typeof(MD5Hash?), typeof(FileWithBinaryPropertiesViewModel<TEntity>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Hash"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HashProperty = HashPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public MD5Hash? Hash { get => (MD5Hash?)GetValue(HashProperty); private set => SetValue(HashPropertyKey, value); }

        #endregion
        #region RedundancyCount Property Members

        private static readonly DependencyPropertyKey RedundancyCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RedundancyCount), typeof(long),
            typeof(FileWithBinaryPropertiesViewModel<TEntity>), new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as FileWithBinaryPropertiesViewModel<TEntity>).OnRedundancyCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="RedundancyCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundancyCountProperty = RedundancyCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long RedundancyCount { get => (long)GetValue(RedundancyCountProperty); private set => SetValue(RedundancyCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="RedundancyCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RedundancyCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RedundancyCount"/> property.</param>
        private void OnRedundancyCountPropertyChanged(long oldValue, long newValue)
        {
            // TODO: Implement OnRedundancyCountPropertyChanged Logic
        }

        #endregion
        #region ComparisonCount Property Members

        private static readonly DependencyPropertyKey ComparisonCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ComparisonCount), typeof(long),
            typeof(FileWithBinaryPropertiesViewModel<TEntity>), new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as FileWithBinaryPropertiesViewModel<TEntity>).OnComparisonCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="ComparisonCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ComparisonCountProperty = ComparisonCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long ComparisonCount { get => (long)GetValue(ComparisonCountProperty); private set => SetValue(ComparisonCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="ComparisonCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ComparisonCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ComparisonCount"/> property.</param>
        private void OnComparisonCountPropertyChanged(long oldValue, long newValue)
        {
            // TODO: Implement OnComparisonCountPropertyChanged Logic
        }

        #endregion
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AccessErrorCount), typeof(long),
            typeof(FileWithBinaryPropertiesViewModel<TEntity>), new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as FileWithBinaryPropertiesViewModel<TEntity>).OnAccessErrorCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = AccessErrorCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long AccessErrorCount { get => (long)GetValue(AccessErrorCountProperty); private set => SetValue(AccessErrorCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="AccessErrorCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AccessErrorCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AccessErrorCount"/> property.</param>
        private void OnAccessErrorCountPropertyChanged(long oldValue, long newValue)
        {
            // TODO: Implement OnAccessErrorCountPropertyChanged Logic
        }

        #endregion
        #region PersonalTagCount Property Members

        private static readonly DependencyPropertyKey PersonalTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PersonalTagCount), typeof(long),
            typeof(FileWithBinaryPropertiesViewModel<TEntity>), new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as FileWithBinaryPropertiesViewModel<TEntity>).OnPersonalTagCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="PersonalTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagCountProperty = PersonalTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long PersonalTagCount { get => (long)GetValue(PersonalTagCountProperty); private set => SetValue(PersonalTagCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="PersonalTagCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="PersonalTagCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="PersonalTagCount"/> property.</param>
        private void OnPersonalTagCountPropertyChanged(long oldValue, long newValue)
        {
            // TODO: Implement OnPersonalTagCountPropertyChanged Logic
        }

        #endregion
        #region SharedTagCount Property Members

        private static readonly DependencyPropertyKey SharedTagCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SharedTagCount), typeof(long),
            typeof(FileWithBinaryPropertiesViewModel<TEntity>), new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as FileWithBinaryPropertiesViewModel<TEntity>).OnSharedTagCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Identifies the <see cref="SharedTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagCountProperty = SharedTagCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long SharedTagCount { get => (long)GetValue(SharedTagCountProperty); private set => SetValue(SharedTagCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="SharedTagCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SharedTagCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SharedTagCount"/> property.</param>
        private void OnSharedTagCountPropertyChanged(long oldValue, long newValue)
        {
            // TODO: Implement OnSharedTagCountPropertyChanged Logic
        }

        #endregion

        public FileWithBinaryPropertiesViewModel(TEntity entity) : base(entity)
        {
            Length = entity.Length;
            Hash = entity.Hash;
            RedundancyCount = entity.RedundancyCount;
            ComparisonCount = entity.ComparisonCount;
            AccessErrorCount = entity.AccessErrorCount;
            PersonalTagCount = entity.PersonalTagCount;
            SharedTagCount = entity.SharedTagCount;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IFileListItemWithBinaryProperties.Length):
                    Dispatcher.CheckInvoke(() => Length = Entity.Length);
                    break;
                case nameof(IFileListItemWithBinaryProperties.Hash):
                    Dispatcher.CheckInvoke(() => Hash = Entity.Hash);
                    break;
                case nameof(IFileListItemWithBinaryProperties.RedundancyCount):
                    Dispatcher.CheckInvoke(() => RedundancyCount = Entity.RedundancyCount);
                    break;
                case nameof(IFileListItemWithBinaryProperties.ComparisonCount):
                    Dispatcher.CheckInvoke(() => ComparisonCount = Entity.ComparisonCount);
                    break;
                case nameof(IFileListItemWithBinaryProperties.AccessErrorCount):
                    Dispatcher.CheckInvoke(() => AccessErrorCount = Entity.AccessErrorCount);
                    break;
                case nameof(IFileListItemWithBinaryProperties.PersonalTagCount):
                    Dispatcher.CheckInvoke(() => PersonalTagCount = Entity.PersonalTagCount);
                    break;
                case nameof(IFileListItemWithBinaryProperties.SharedTagCount):
                    Dispatcher.CheckInvoke(() => SharedTagCount = Entity.SharedTagCount);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
