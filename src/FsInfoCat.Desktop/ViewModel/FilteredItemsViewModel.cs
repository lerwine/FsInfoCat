using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class FilteredItemsViewModel : DependencyObject
    {
        #region ShowFilterOptions Command Property Members

        private static readonly DependencyPropertyKey ShowFilterOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ShowFilterOptions),
            typeof(Commands.RelayCommand), typeof(FilteredItemsViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowFilterOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowFilterOptionsProperty = ShowFilterOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand ShowFilterOptions => (Commands.RelayCommand)GetValue(ShowFilterOptionsProperty);

        protected virtual void OnShowFilterOptionsCommand(object parameter) => ViewOptionsVisible = true;

        #endregion
        #region SaveFilterOptions Command Property Members

        private static readonly DependencyPropertyKey SaveFilterOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveFilterOptions),
            typeof(Commands.RelayCommand), typeof(FilteredItemsViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SaveFilterOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaveFilterOptionsProperty = SaveFilterOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand SaveFilterOptions => (Commands.RelayCommand)GetValue(SaveFilterOptionsProperty);

        private void RaiseSaveFilterOptionsCommand(object parameter)
        {
            ViewOptionsVisible = false;
            OnApplyFilterOptionsCommand(parameter);
        }

        #endregion
        #region CancelFilterOptions Command Property Members

        private static readonly DependencyPropertyKey CancelFilterOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelFilterOptions),
            typeof(Commands.RelayCommand), typeof(FilteredItemsViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="CancelFilterOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelFilterOptionsProperty = CancelFilterOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand CancelFilterOptions => (Commands.RelayCommand)GetValue(CancelFilterOptionsProperty);

        protected virtual void OnCancelFilterOptionsCommand(object parameter) => ViewOptionsVisible = false;

        #endregion
        #region ViewOptionsVisible Property Members

        private static readonly DependencyPropertyKey ViewOptionsVisiblePropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptionsVisible), typeof(bool),
            typeof(FilteredItemsViewModel), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="ViewOptionsVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsVisibleProperty = ViewOptionsVisiblePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool ViewOptionsVisible { get => (bool)GetValue(ViewOptionsVisibleProperty); private set => SetValue(ViewOptionsVisiblePropertyKey, value); }

        #endregion
        #region AddNewItem Command Property Members

        private static readonly DependencyPropertyKey AddNewItemPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AddNewItem), typeof(Commands.RelayCommand),
            typeof(FilteredItemsViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="AddNewItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AddNewItemProperty = AddNewItemPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand AddNewItem => (Commands.RelayCommand)GetValue(AddNewItemProperty);

        #endregion
        #region Refresh Command Property Members

        private static readonly DependencyPropertyKey RefreshPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Refresh), typeof(Commands.RelayCommand),
            typeof(FilteredItemsViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Refresh"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RefreshProperty = RefreshPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Refresh => (Commands.RelayCommand)GetValue(RefreshProperty);

        #endregion
        #region ItemDisplayText Attached Property Members

        public const string PropertyName_ItemDisplayText = "ItemDisplayText";

        public static string GetItemDisplayText(DependencyObject obj)
        {
            return (string)obj.GetValue(ItemDisplayTextProperty);
        }

        protected internal static void SetItemDisplayText(DependencyObject obj, string value)
        {
            obj.SetValue(ItemDisplayTextProperty, value);
        }

        protected internal static readonly DependencyPropertyKey ItemDisplayTextPropertyKey = DependencyProperty.RegisterAttachedReadOnly(PropertyName_ItemDisplayText, typeof(string),
            typeof(FilteredItemsViewModel), new PropertyMetadata(0));

        public static readonly DependencyProperty ItemDisplayTextProperty = ItemDisplayTextPropertyKey.DependencyProperty;

        #endregion

        protected FilteredItemsViewModel()
        {
            SetValue(AddNewItemPropertyKey, new Commands.RelayCommand(OnAddNewItemCommand));
            SetValue(RefreshPropertyKey, new Commands.RelayCommand(OnRefreshCommand));
            SetValue(ShowFilterOptionsPropertyKey, new Commands.RelayCommand(OnShowFilterOptionsCommand));
            SetValue(SaveFilterOptionsPropertyKey, new Commands.RelayCommand(RaiseSaveFilterOptionsCommand));
            SetValue(CancelFilterOptionsPropertyKey, new Commands.RelayCommand(OnCancelFilterOptionsCommand));
        }

        protected abstract void OnRefreshCommand(object parameter);

        protected abstract void OnAddNewItemCommand(object parameter);

        protected abstract void OnApplyFilterOptionsCommand(object parameter);
    }
}
