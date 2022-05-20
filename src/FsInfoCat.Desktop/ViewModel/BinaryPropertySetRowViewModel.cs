using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class BinaryPropertySetRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IBinaryPropertySet
    {
        #region Hash Property Members

        /// <summary>
        /// Identifies the <see cref="Hash"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HashProperty = ColumnPropertyBuilder<Model.MD5Hash?, BinaryPropertySetRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IBinaryPropertySet.Hash))
            .OnChanged((DependencyObject d, Model.MD5Hash? oldValue, Model.MD5Hash? newValue) =>
                (d as BinaryPropertySetRowViewModel<TEntity>).OnHashPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.MD5Hash? Hash { get => (Model.MD5Hash?)GetValue(HashProperty); set => SetValue(HashProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Hash"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Hash"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Hash"/> property.</param>
        protected virtual void OnHashPropertyChanged(Model.MD5Hash? oldValue, Model.MD5Hash? newValue) { }

        #endregion
        #region Length Property Members

        /// <summary>
        /// Identifies the <see cref="Length"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LengthProperty = ColumnPropertyBuilder<long, BinaryPropertySetRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IBinaryPropertySet.Length))
            .DefaultValue(0L)
            .OnChanged((DependencyObject d, long oldValue, long newValue) =>
                (d as BinaryPropertySetRowViewModel<TEntity>).OnLengthPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public long Length { get => (long)GetValue(LengthProperty); set => SetValue(LengthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Length"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Length"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Length"/> property.</param>
        protected virtual void OnLengthPropertyChanged(long oldValue, long newValue) { }

        #endregion

        public BinaryPropertySetRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Hash = entity.Hash;
            Length = entity.Length;
        }
    }
}
