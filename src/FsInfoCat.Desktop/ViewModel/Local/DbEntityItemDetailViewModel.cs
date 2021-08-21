using FsInfoCat.Local;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public abstract class DbEntityItemDetailViewModel<TDbEntity, TItemVM> : DependencyObject, IHasAsyncWindowsBackgroundOperationManager
        where TDbEntity : LocalDbEntity, new()
        where TItemVM : DbEntityItemVM<TDbEntity>
    {
        #region BgOps Property Members

        private static readonly DependencyPropertyKey BgOpsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BgOps), typeof(AsyncOps.AsyncBgModalVM), typeof(DbEntityItemDetailViewModel<TDbEntity, TItemVM>),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BgOps"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BgOpsProperty = BgOpsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the asynchronous operation view model.
        /// </summary>
        /// <value>The view model to be bound to the <see cref="View.AsyncBgModalControl"/>.</value>
        public AsyncOps.AsyncBgModalVM BgOps => (AsyncOps.AsyncBgModalVM)GetValue(BgOpsProperty);

        #endregion
        #region CurrentItem Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="CurrentItem"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler CurrentItemPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="CurrentItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentItemProperty = DependencyProperty.Register(nameof(CurrentItem), typeof(TItemVM), typeof(DbEntityItemDetailViewModel<TDbEntity, TItemVM>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DbEntityItemDetailViewModel<TDbEntity, TItemVM>)?.OnCurrentItemPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public TItemVM CurrentItem { get => (TItemVM)GetValue(CurrentItemProperty); set => SetValue(CurrentItemProperty, value); }

        /// <summary>
        /// Called when the <see cref="PropertyChangedCallback">PropertyChanged</see> event on <see cref="CurrentItemProperty"/> is raised.
        /// </summary>
        /// <param name="args">The Event data that is issued by the event on <see cref="CurrentItemProperty"/> that tracks changes to its effective value.</param>
        protected virtual void OnCurrentItemPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnCurrentItemPropertyChanged((TItemVM)args.OldValue, (TItemVM)args.NewValue); }
            finally { CurrentItemPropertyChanged?.Invoke(this, args); }
        }

        /// <summary>
        /// Called when the value of the <see cref="CurrentItem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CurrentItem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CurrentItem"/> property.</param>
        protected abstract void OnCurrentItemPropertyChanged(TItemVM oldValue, TItemVM newValue);

        #endregion

        public DbEntityItemDetailViewModel()
        {
            SetValue(BgOpsPropertyKey, new AsyncOps.AsyncBgModalVM());
        }

        IAsyncWindowsBackgroundOperationManager IHasAsyncWindowsBackgroundOperationManager.GetAsyncBackgroundOperationManager()
        {
            if (CheckAccess())
                return BgOps;
            return Dispatcher.Invoke(() => BgOps);
        }

        IAsyncBackgroundOperationManager IHasAsyncBackgroundOperationManager.GetAsyncBackgroundOperationManager()
        {
            if (CheckAccess())
                return BgOps;
            return Dispatcher.Invoke(() => BgOps);
        }
    }
}
