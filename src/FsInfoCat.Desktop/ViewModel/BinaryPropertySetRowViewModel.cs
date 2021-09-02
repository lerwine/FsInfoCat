using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class BinaryPropertySetRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IBinaryPropertySet
    {
        #region Hash Property Members

        /// <summary>
        /// Identifies the <see cref="Hash"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HashProperty = DependencyProperty.Register(nameof(Hash), typeof(MD5Hash?), typeof(BinaryPropertySetRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as BinaryPropertySetRowViewModel<TEntity>)?.OnHashPropertyChanged((MD5Hash?)e.OldValue, (MD5Hash?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public MD5Hash? Hash { get => (MD5Hash?)GetValue(HashProperty); set => SetValue(HashProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Hash"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Hash"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Hash"/> property.</param>
        protected void OnHashPropertyChanged(MD5Hash? oldValue, MD5Hash? newValue) { }

        #endregion
        #region Length Property Members

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(long), typeof(BinaryPropertySetRowViewModel<TEntity>),
                new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as BinaryPropertySetRowViewModel<TEntity>)?.OnLengthPropertyChanged((long)e.OldValue, (long)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public long Length { get => (long)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Length"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Length"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Length"/> property.</param>
        protected void OnLengthPropertyChanged(long oldValue, long newValue) { }

        #endregion

        public BinaryPropertySetRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Hash = entity.Hash;
            Length = entity.Length;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IBinaryPropertySet.Hash):
                    Dispatcher.CheckInvoke(() => Hash = Entity.Hash);
                    break;
                case nameof(IBinaryPropertySet.Length):
                    Dispatcher.CheckInvoke(() => Length = Entity.Length);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
