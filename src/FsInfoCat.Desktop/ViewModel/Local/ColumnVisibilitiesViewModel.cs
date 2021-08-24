using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public abstract class ColumnVisibilitiesViewModel : DependencyObject
    {
        #region AllColumns Property Members

        private static readonly DependencyPropertyKey AllColumnsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AllColumns), typeof(ReadOnlyObservableCollection<ColumnVisiblityItemVM>), typeof(ColumnVisibilitiesViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="AllColumns"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllColumnsProperty = AllColumnsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ReadOnlyObservableCollection<ColumnVisiblityItemVM> AllColumns => (ReadOnlyObservableCollection<ColumnVisiblityItemVM>)GetValue(AllColumnsProperty);

        #endregion

        protected ColumnVisibilitiesViewModel(params (DependencyProperty Property, string Description, int Order)[] properties) : this(properties.AsEnumerable()) { }

        protected ColumnVisibilitiesViewModel(params (DependencyProperty Property, string Description)[] properties) : this(properties.AsEnumerable()) { }

        protected ColumnVisibilitiesViewModel(params (DependencyProperty Property, int Order)[] properties)
            : this(properties.Select(p => (p.Property, p.Property.Name, p.Order))) { }

        protected ColumnVisibilitiesViewModel(IEnumerable<(DependencyProperty Property, string Description, int Order)> properties)
        {
            SetValue(AllColumnsPropertyKey, new ReadOnlyObservableCollection<ColumnVisiblityItemVM>(new ObservableCollection<ColumnVisiblityItemVM>(properties
                .Distinct().OrderBy(p => p.Order).Select(p => new ColumnVisiblityItemVM(this, p.Property, p.Description)).ToArray())));
        }

        protected ColumnVisibilitiesViewModel(IEnumerable<(DependencyProperty Property, string Description)> properties)
        {
            SetValue(AllColumnsPropertyKey, new ReadOnlyObservableCollection<ColumnVisiblityItemVM>(new ObservableCollection<ColumnVisiblityItemVM>(properties
                .Distinct().Select(p => new ColumnVisiblityItemVM(this, p.Property, p.Description)).ToArray())));
        }

        protected ColumnVisibilitiesViewModel(IEnumerable<(DependencyProperty Property, int Order)> properties)
            : this(properties.Select(p => (p.Property, p.Property.Name, p.Order))) { }

        protected ColumnVisibilitiesViewModel(params DependencyProperty[] properties) : this(properties.Select(p => (p, p.Name))) { }

        protected ColumnVisibilitiesViewModel(IEnumerable<DependencyProperty> properties) : this(properties.Select(p => (p, p.Name))) { }
    }
}
