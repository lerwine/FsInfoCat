using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class ColumnVisibilityOptionsViewModel<TEntity, TViewModel> : DependencyObject
        where TEntity : DbEntity
        where TViewModel : DbEntityRowViewModel<TEntity>
    {
        public event DependencyPropertyChangedEventHandler ColumnVisibilityPropertyChanged;

        /// <summary>
        /// Occurs when <see cref="SetSummaryColumnText(DependencyObject, string)"/> needs to be invoked on all <see cref="DbEntityRowViewModel{TEntity}"/> items.
        /// </summary>
        public event EventHandler<ResummarizeRowsEventArgs> ResummarizeRows;

        /// <summary>
        /// Occurs when the clear summary text for all <see cref="DbEntityRowViewModel{TEntity}"/> items needs to be cleared by invoking <see cref="SetSummaryColumnText(DependencyObject, string)"/>.
        /// </summary>
        public event EventHandler ClearRowSummaries;

        #region CreatedOn Property Members

        /// <summary>
        /// Identifies the <see cref="CreatedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreatedOnProperty = DependencyPropertyBuilder<ColumnVisibilityOptionsViewModel<TEntity, TViewModel>, bool>
            .Register(nameof(CreatedOn))
            .DefaultValue(false)
            .AsReadWrite();

        public bool CreatedOn { get => (bool)GetValue(CreatedOnProperty); set => SetValue(CreatedOnProperty, value); }

        #endregion

        #region ModifiedOn Property Members

        /// <summary>
        /// Identifies the <see cref="ModifiedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModifiedOnProperty = DependencyPropertyBuilder<ColumnVisibilityOptionsViewModel<TEntity, TViewModel>, bool>
            .Register(nameof(ModifiedOn))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ModifiedOn { get => (bool)GetValue(ModifiedOnProperty); set => SetValue(ModifiedOnProperty, value); }

        #endregion

        #region Columns Property Members

        private static readonly DependencyPropertyKey ColumnsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Columns), typeof(ReadOnlyObservableCollection<ColumnVisibilityItemViewModel>), typeof(ColumnVisibilityOptionsViewModel<TEntity, TViewModel>),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnsProperty = ColumnsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<ColumnVisibilityItemViewModel> Columns => (ReadOnlyObservableCollection<ColumnVisibilityItemViewModel>)GetValue(ColumnsProperty);

        #endregion

        #region ShowSummaryColumn Property Members

        private static readonly DependencyPropertyKey ShowSummaryColumnPropertyKey = DependencyPropertyBuilder<ColumnVisibilityOptionsViewModel<TEntity, TViewModel>, bool>
            .Register(nameof(ShowSummaryColumn))
            .DefaultValue(true)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShowSummaryColumn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowSummaryColumnProperty = ShowSummaryColumnPropertyKey.DependencyProperty;

        public bool ShowSummaryColumn { get => (bool)GetValue(ShowSummaryColumnProperty); private set => SetValue(ShowSummaryColumnPropertyKey, value); }

        #endregion

        protected ColumnVisibilityOptionsViewModel()
        {
            ObservableCollection<ColumnVisibilityItemViewModel> backingColumns = new();
            foreach (ColumnVisibilityItemViewModel item in ColumnProperty.GetProperties<TViewModel>().Select((Column, OriginalIndex) => (Column, OriginalIndex, Order: Column.Order ?? int.MaxValue))
                .OrderBy(a => a.Order).ThenBy(a => a.OriginalIndex).Select(t => new ColumnVisibilityItemViewModel(t.Column)))
            {
                backingColumns.Add(item);
                item.IsVisiblePropertyChanged += Item_IsVisiblePropertyChanged;
            }
            SetValue(ColumnsPropertyKey, new ReadOnlyObservableCollection<ColumnVisibilityItemViewModel>(backingColumns));
        }

        public IReadOnlyList<ColumnProperty> GetSummaryColumns()
        {
            ColumnProperty[] toSummarize = Columns.Where(c => !c.IsVisible).Select(c => c.GetProperty()).ToArray();
            if (toSummarize.Length > 1)
                return toSummarize;
            return Array.Empty<ColumnProperty>();
        }

        private void Item_IsVisiblePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ColumnProperty[] toSummarize = Columns.Where(c => !c.IsVisible).Select(c => c.GetProperty()).ToArray();
            if (!(bool)e.NewValue && toSummarize.Length == 1)
            {
                ShowSummaryColumn = false;
                ClearRowSummaries?.Invoke(this, EventArgs.Empty);
            }
            if (toSummarize.Length > 1)
            {
                ResummarizeRows?.Invoke(this, new(toSummarize));
                ShowSummaryColumn = true;
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.NewValue is bool isVisible)
                _ = ColumnVisibilityItemViewModel.NotifyBooleanPropertyChanged(Columns, e.Property.Name, isVisible);
        }

        public virtual bool IsColumnHidden(string name) => Columns.Where(c => c.ShortName == name).Any(c => !c.IsVisible);

        public virtual IEnumerable<string> GetHiddenColumns() => Columns.Where(c => !c.IsVisible).Select(c => c.ShortName);

        public virtual IEnumerable<string> GetVisibleColumns() => Columns.Where(c => c.IsVisible).Select(c => c.ShortName);

        public virtual IEnumerable<(string Name, bool IsVisible)> GetColumnVisibilities() => Columns.Select(c => (c.ShortName, c.IsVisible));
    }
}
