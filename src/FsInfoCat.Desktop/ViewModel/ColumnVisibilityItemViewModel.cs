using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public sealed class ColumnVisibilityItemViewModel : DependencyObject
    {
        private readonly ColumnProperty _property;

        #region PropertyName Property Members

        private static readonly DependencyPropertyKey PropertyNamePropertyKey = DependencyPropertyBuilder<ColumnVisibilityItemViewModel, string>
            .Register(nameof(PropertyName))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PropertyName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty = PropertyNamePropertyKey.DependencyProperty;

        public string PropertyName { get => GetValue(PropertyNameProperty) as string; private set => SetValue(PropertyNamePropertyKey, value); }

        #endregion
        #region ShortName Property Members

        private static readonly DependencyPropertyKey ShortNamePropertyKey = DependencyPropertyBuilder<ColumnVisibilityItemViewModel, string>
            .Register(nameof(ShortName))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShortName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShortNameProperty = ShortNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets property name of the column.
        /// </summary>
        /// <value>The .</value>
        public string ShortName { get => GetValue(ShortNameProperty) as string; private set => SetValue(ShortNamePropertyKey, value); }

        #endregion
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyPropertyBuilder<ColumnVisibilityItemViewModel, string>
            .Register(nameof(DisplayName))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DisplayName { get => GetValue(DisplayNameProperty) as string; private set => SetValue(DisplayNamePropertyKey, value); }

        #endregion
        #region Description Property Members

        private static readonly DependencyPropertyKey DescriptionPropertyKey = DependencyPropertyBuilder<ColumnVisibilityItemViewModel, string>
            .Register(nameof(Description))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ShortName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = DescriptionPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Description { get => GetValue(DescriptionProperty) as string; private set => SetValue(DescriptionPropertyKey, value); }

        #endregion
        #region IsVisible Property Members

        public event DependencyPropertyChangedEventHandler IsVisiblePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyPropertyBuilder<ColumnVisibilityItemViewModel, bool>
            .Register(nameof(IsVisible))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as ColumnVisibilityItemViewModel)?.IsVisiblePropertyChanged?.Invoke(d, e))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsVisible { get => (bool)GetValue(IsVisibleProperty); set => SetValue(IsVisibleProperty, value); }

        /// <summary>
        /// Called when a property on the parent <see cref="ColumnVisibilityOptionsViewModel{TEntity, TViewModel}"/>, which represents a column's visibility, has changed.
        /// </summary>
        /// <param name="columns">The columns from the parent <see cref="ColumnVisibilityOptionsViewModel{TEntity, TViewModel}"/>.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>THe matching <see cref="ColumnProperty"/> or <see langword="null"/> if not match was found.</returns>
        internal static ColumnProperty NotifyBooleanPropertyChanged(ReadOnlyObservableCollection<ColumnVisibilityItemViewModel> columns, string name, bool value)
        {
            ColumnVisibilityItemViewModel item = columns.FirstOrDefault(c => c.PropertyName == name);
            if (item is null)
                return null;
            item.IsVisible = value;
            return item._property;
        }

        #endregion

        public ColumnVisibilityItemViewModel(ColumnProperty property)
        {
            _property = property;
            PropertyName = property.Name;
            if (string.IsNullOrWhiteSpace(property.ShortName))
                ShortName = DisplayName = string.IsNullOrWhiteSpace(property.DisplayName) ? property.Name : property.DisplayName;
            else
                DisplayName = string.IsNullOrWhiteSpace(property.DisplayName) ? property.ShortName : property.DisplayName;
            Description = property.Description;
            IsVisible = (bool)GetValue(_property.DependencyProperty);
        }

        internal ColumnProperty GetProperty() => _property;
    }
}
