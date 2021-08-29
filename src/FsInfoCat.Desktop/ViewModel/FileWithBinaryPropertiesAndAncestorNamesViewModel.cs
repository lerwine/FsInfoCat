using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileWithBinaryPropertiesAndAncestorNamesViewModel<TEntity> : FileWithAncestorNamesViewModel<TEntity>
        where TEntity : DbEntity, IFileListItemWithBinaryPropertiesAndAncestorNames
    {
        #region Length Property Members

        private static readonly DependencyPropertyKey LengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Length), typeof(long),
            typeof(FileWithBinaryPropertiesAndAncestorNamesViewModel<TEntity>), new PropertyMetadata(0L));

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

        private static readonly DependencyPropertyKey HashPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Hash), typeof(MD5Hash?),
            typeof(FileWithBinaryPropertiesAndAncestorNamesViewModel<TEntity>), new PropertyMetadata(null));

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

        public FileWithBinaryPropertiesAndAncestorNamesViewModel(TEntity entity) : base(entity)
        {
            Length = entity.Length;
            Hash = entity.Hash;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IFileListItemWithBinaryPropertiesAndAncestorNames.Length):
                    Dispatcher.CheckInvoke(() => Length = Entity.Length);
                    break;
                case nameof(IFileListItemWithBinaryPropertiesAndAncestorNames.Hash):
                    Dispatcher.CheckInvoke(() => Hash = Entity.Hash);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
