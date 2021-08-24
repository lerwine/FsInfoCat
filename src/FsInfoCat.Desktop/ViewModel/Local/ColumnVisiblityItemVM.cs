using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public sealed class ColumnVisiblityItemVM : DependencyObject
    {
        #region Name Property Members

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(ColumnVisiblityItemVM), new PropertyMetadata(""));

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
        #region Description Property Members

        private static readonly DependencyPropertyKey DescriptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Description), typeof(string), typeof(ColumnVisiblityItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
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
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(nameof(IsVisible), typeof(bool), typeof(ColumnVisiblityItemVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ColumnVisiblityItemVM)?.OnIsVisiblePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));
        private readonly ColumnVisibilitiesViewModel _owner;
        private readonly DependencyProperty _property;

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
        private void OnIsVisiblePropertyChanged(bool oldValue, bool newValue)
        {
            _owner.SetValue(_property, newValue);
        }

        #endregion
        internal ColumnVisiblityItemVM(ColumnVisibilitiesViewModel owner, DependencyProperty property, string description)
        {
            _owner = owner;
            _property = property;
            IsVisible = (owner.GetValue(property) as bool?) ?? false;
            Name = property.Name;
            Description = string.IsNullOrWhiteSpace(description) ? property.Name : description;
        }
    }
}
