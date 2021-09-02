using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AccessErrorRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IAccessError
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region ErrorCode Property Members

        /// <summary>
        /// Identifies the <see cref="ErrorCode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorCodeProperty = DependencyProperty.Register(nameof(ErrorCode), typeof(AccessErrorCode),
            typeof(AccessErrorRowViewModel<TEntity>), new PropertyMetadata(AccessErrorCode.Unspecified, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AccessErrorRowViewModel<TEntity>)?.OnErrorCodePropertyChanged((AccessErrorCode)e.OldValue, (AccessErrorCode)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public AccessErrorCode ErrorCode { get => (AccessErrorCode)GetValue(ErrorCodeProperty); set => SetValue(ErrorCodeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ErrorCode"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ErrorCode"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ErrorCode"/> property.</param>
        protected void OnErrorCodePropertyChanged(AccessErrorCode oldValue, AccessErrorCode newValue) { }

        #endregion
        #region Details Property Members

        /// <summary>
        /// Identifies the <see cref="Details"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DetailsProperty = DependencyProperty.Register(nameof(Details), typeof(string), typeof(AccessErrorRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AccessErrorRowViewModel<TEntity>)?.OnDetailsPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Details { get => GetValue(DetailsProperty) as string; set => SetValue(DetailsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Details"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Details"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Details"/> property.</param>
        protected void OnDetailsPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnDetailsPropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public AccessErrorRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            ErrorCode = entity.ErrorCode;
            Details = entity.Details;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IAccessError.ErrorCode):
                    Dispatcher.CheckInvoke(() => ErrorCode = Entity.ErrorCode);
                    break;
                case nameof(IAccessError.Details):
                    Dispatcher.CheckInvoke(() => Details = Entity.Details);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
