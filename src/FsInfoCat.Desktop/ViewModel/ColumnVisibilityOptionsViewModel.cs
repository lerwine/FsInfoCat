using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class ColumnVisibilityOptionsViewModel : DependencyObject
    {
        public event DependencyPropertyChangedEventHandler ColumnVisibilityPropertyChanged;

        #region CreatedOn Property Members

        /// <summary>
        /// Identifies the <see cref="CreatedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreatedOnProperty = DependencyProperty.Register(nameof(CreatedOn), typeof(bool), typeof(ColumnVisibilityOptionsViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ColumnVisibilityOptionsViewModel)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool CreatedOn { get => (bool)GetValue(CreatedOnProperty); set => SetValue(CreatedOnProperty, value); }

        #endregion
        #region ModifiedOn Property Members

        /// <summary>
        /// Identifies the <see cref="ModifiedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModifiedOnProperty = DependencyProperty.Register(nameof(ModifiedOn), typeof(bool), typeof(ColumnVisibilityOptionsViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ColumnVisibilityOptionsViewModel)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool ModifiedOn { get => (bool)GetValue(ModifiedOnProperty); set => SetValue(ModifiedOnProperty, value); }

        #endregion
        #region Columns Property Members

        private static readonly DependencyPropertyKey ColumnsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Columns), typeof(ReadOnlyObservableCollection<ColumnVisibilityItemViewModel>), typeof(ColumnVisibilityOptionsViewModel),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnsProperty = ColumnsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<ColumnVisibilityItemViewModel> Columns => (ReadOnlyObservableCollection<ColumnVisibilityItemViewModel>)GetValue(ColumnsProperty);

        #endregion

        [System.Obsolete("Use constructor with ColumnProperty elements")]
        protected ColumnVisibilityOptionsViewModel(IEnumerable<DependencyProperty> columnProperties)
        {
            ObservableCollection<ColumnVisibilityItemViewModel> backingColumns = new();
            foreach (ColumnVisibilityItemViewModel item in columnProperties.Concat(new DependencyProperty[] { CreatedOnProperty, ModifiedOnProperty }).Distinct()
                .Select(t => new ColumnVisibilityItemViewModel(t)))
            {
                backingColumns.Add(item);
            }
            SetValue(ColumnsPropertyKey, new ReadOnlyObservableCollection<ColumnVisibilityItemViewModel>(backingColumns));
        }

        protected ColumnVisibilityOptionsViewModel(IEnumerable<ColumnProperty> columnProperties)
        {
            ObservableCollection<ColumnVisibilityItemViewModel> backingColumns = new();
            foreach (ColumnVisibilityItemViewModel item in columnProperties.Distinct().Select((Column, OriginalIndex) => (Column, OriginalIndex, Order: Column.Order ?? int.MaxValue))
                .OrderBy(a => a.Order).ThenBy(a => a.OriginalIndex).Select(t => new ColumnVisibilityItemViewModel(t.Column)))
            {
                backingColumns.Add(item);
            }
            SetValue(ColumnsPropertyKey, new ReadOnlyObservableCollection<ColumnVisibilityItemViewModel>(backingColumns));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            ColumnVisibilityItemViewModel.NotifyPropertyChanged(Columns, e);
        }

        protected ColumnVisibilityOptionsViewModel(params DependencyProperty[] columnProperties) : this(columnProperties.AsEnumerable()) { }

        protected void RaiseColumnVisibilityPropertyChanged(DependencyPropertyChangedEventArgs args) => ColumnVisibilityPropertyChanged?.Invoke(this, args);
        public virtual bool IsColumnHidden(string name) => Columns.Where(c => c.Name == name).Any(c => !c.IsVisible);

        public virtual IEnumerable<string> GetHiddenColumns() => Columns.Where(c => !c.IsVisible).Select(c => c.Name);

        public virtual IEnumerable<string> GetVisibleColumns() => Columns.Where(c => c.IsVisible).Select(c => c.Name);

        public virtual IEnumerable<(string Name, bool IsVisible)> GetColumnVisibilities() => Columns.Select(c => (c.Name, c.IsVisible));
    }
}
