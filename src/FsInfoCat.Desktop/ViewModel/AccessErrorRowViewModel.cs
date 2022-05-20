using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AccessErrorRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IAccessError
    {
        #region ErrorCode Property Members

        /// <summary>
        /// Identifies the <see cref="ErrorCode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorCodeProperty = ColumnPropertyBuilder<Model.ErrorCode, AccessErrorRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAccessError.ErrorCode))
            .DefaultValue(Model.ErrorCode.Unexpected)
            .OnChanged((d, oldValue, newValue) => (d as AccessErrorRowViewModel<TEntity>)?.OnErrorCodePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.ErrorCode ErrorCode { get => (Model.ErrorCode)GetValue(ErrorCodeProperty); set => SetValue(ErrorCodeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ErrorCode"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ErrorCode"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ErrorCode"/> property.</param>
        protected virtual void OnErrorCodePropertyChanged(Model.ErrorCode oldValue, Model.ErrorCode newValue) { }

        #endregion
        #region Details Property Members

        /// <summary>
        /// Identifies the <see cref="Details"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DetailsProperty = ColumnPropertyBuilder<string, AccessErrorRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAccessError.Details))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as AccessErrorRowViewModel<TEntity>)?.OnDetailsPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string Details { get => GetValue(DetailsProperty) as string; set => SetValue(DetailsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Details"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Details"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Details"/> property.</param>
        protected virtual void OnDetailsPropertyChanged(string oldValue, string newValue) { }

        #endregion

        public AccessErrorRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            ErrorCode = entity.ErrorCode;
            Details = entity.Details;
        }
    }
}
