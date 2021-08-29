using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class RedundantSetListItemViewModel<TEntity> : RedundantSetRowViewModel<TEntity>
        where TEntity : DbEntity, IRedundantSetListItem
    {
        #region Length Property Members

        private static readonly DependencyPropertyKey LengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Length), typeof(long), typeof(RedundantSetListItemViewModel<TEntity>),
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

        private static readonly DependencyPropertyKey HashPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Hash), typeof(MD5Hash?), typeof(RedundantSetListItemViewModel<TEntity>),
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
            typeof(RedundantSetListItemViewModel<TEntity>), new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="RedundancyCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundancyCountProperty = RedundancyCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long RedundancyCount { get => (long)GetValue(RedundancyCountProperty); private set => SetValue(RedundancyCountPropertyKey, value); }

        #endregion

        public RedundantSetListItemViewModel(TEntity entity) : base(entity)
        {
            Length = entity.Length;
            Hash = entity.Hash;
            RedundancyCount = entity.RedundancyCount;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IRedundantSetListItem.Length):
                    Dispatcher.CheckInvoke(() => Length = Entity.Length);
                    break;
                case nameof(IRedundantSetListItem.Hash):
                    Dispatcher.CheckInvoke(() => Hash = Entity.Hash);
                    break;
                case nameof(IRedundantSetListItem.RedundancyCount):
                    Dispatcher.CheckInvoke(() => RedundancyCount = Entity.RedundancyCount);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
