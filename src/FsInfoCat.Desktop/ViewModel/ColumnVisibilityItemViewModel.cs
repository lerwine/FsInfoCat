using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ColumnVisibilityItemViewModel : DependencyObject
    {
        private readonly DependencyProperty _property;

        #region Name Property Members

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyPropertyBuilder<ColumnVisibilityItemViewModel, string>
            .Register(nameof(Name))
            .DefaultValue("")
            .CoerseWith(NormalizedOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Name { get => GetValue(NameProperty) as string; private set => SetValue(NamePropertyKey, value); }

        #endregion
        #region DisplayName Property Members

        private static readonly DependencyPropertyKey DisplayNamePropertyKey = DependencyPropertyBuilder<ColumnVisibilityItemViewModel, string>
            .Register(nameof(DisplayName))
            .DefaultValue("")
            .CoerseWith(NormalizedOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
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
            .DefaultValue("")
            .CoerseWith(NormalizedOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = DescriptionPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Description { get => GetValue(DescriptionProperty) as string; private set => SetValue(DescriptionPropertyKey, value); }

        #endregion
        #region IsVisible Property Members

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyPropertyBuilder<ColumnVisibilityItemViewModel, bool>
            .Register(nameof(Name))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as ColumnVisibilityItemViewModel)?.OnIsVisiblePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsVisible { get => (bool)GetValue(IsVisibleProperty); set => SetValue(IsVisibleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsVisible"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsVisible"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsVisible"/> property.</param>
        private void OnIsVisiblePropertyChanged(bool oldValue, bool newValue) => SetValue(_property, newValue);

        internal static void NotifyPropertyChanged(ReadOnlyObservableCollection<ColumnVisibilityItemViewModel> columns, DependencyPropertyChangedEventArgs e)
        {
            ColumnVisibilityItemViewModel item = columns.FirstOrDefault(c => ReferenceEquals(c._property, e.Property));
            if (item is not null)
                item.IsVisible = (bool)e.NewValue;
        }

        #endregion

        [System.Obsolete("Use ColumnProperty constructor")]
        public ColumnVisibilityItemViewModel(DependencyProperty property)
        {
            _property = property;
            Name = _property.Name;
            IsVisible = (bool)GetValue(_property);
        }

        public ColumnVisibilityItemViewModel(ColumnProperty property)
        {
            _property = property.DependencyProperty;
            Name = property.ShortName;
            DisplayName = property.DisplayName;
            Description = property.Description;
            IsVisible = (bool)GetValue(_property);
        }
    }
}
