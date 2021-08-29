using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DRMPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IDRMProperties
    {
        #region DatePlayExpires Property Members

        /// <summary>
        /// Identifies the <see cref="DatePlayExpires"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DatePlayExpiresProperty = DependencyProperty.Register(nameof(DatePlayExpires), typeof(DateTime?),
            typeof(DRMPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DRMPropertiesRowViewModel<TEntity>)?.OnDatePlayExpiresPropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? DatePlayExpires { get => (DateTime?)GetValue(DatePlayExpiresProperty); set => SetValue(DatePlayExpiresProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DatePlayExpires"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DatePlayExpires"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DatePlayExpires"/> property.</param>
        private void OnDatePlayExpiresPropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            // TODO: Implement OnDatePlayExpiresPropertyChanged Logic
        }

        #endregion
        #region DatePlayStarts Property Members

        /// <summary>
        /// Identifies the <see cref="DatePlayStarts"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DatePlayStartsProperty = DependencyProperty.Register(nameof(DatePlayStarts), typeof(DateTime?),
            typeof(DRMPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DRMPropertiesRowViewModel<TEntity>)?.OnDatePlayStartsPropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? DatePlayStarts { get => (DateTime?)GetValue(DatePlayStartsProperty); set => SetValue(DatePlayStartsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DatePlayStarts"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DatePlayStarts"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DatePlayStarts"/> property.</param>
        private void OnDatePlayStartsPropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            // TODO: Implement OnDatePlayStartsPropertyChanged Logic
        }

        #endregion
        #region Description Property Members

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string),
            typeof(DRMPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DRMPropertiesRowViewModel<TEntity>)?.OnDescriptionPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Description { get => GetValue(DescriptionProperty) as string; set => SetValue(DescriptionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Description"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Description"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Description"/> property.</param>
        private void OnDescriptionPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnDescriptionPropertyChanged Logic
        }

        #endregion
        #region IsProtected Property Members

        /// <summary>
        /// Identifies the <see cref="IsProtected"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsProtectedProperty = DependencyProperty.Register(nameof(IsProtected), typeof(bool?),
            typeof(DRMPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DRMPropertiesRowViewModel<TEntity>)?.OnIsProtectedPropertyChanged((bool?)e.OldValue, (bool?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool? IsProtected { get => (bool?)GetValue(IsProtectedProperty); set => SetValue(IsProtectedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsProtected"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsProtected"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsProtected"/> property.</param>
        private void OnIsProtectedPropertyChanged(bool? oldValue, bool? newValue)
        {
            // TODO: Implement OnIsProtectedPropertyChanged Logic
        }

        #endregion
        #region PlayCount Property Members

        /// <summary>
        /// Identifies the <see cref="PlayCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlayCountProperty = DependencyProperty.Register(nameof(PlayCount), typeof(uint?),
            typeof(DRMPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DRMPropertiesRowViewModel<TEntity>)?.OnPlayCountPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? PlayCount { get => (uint?)GetValue(PlayCountProperty); set => SetValue(PlayCountProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="PlayCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="PlayCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="PlayCount"/> property.</param>
        private void OnPlayCountPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnPlayCountPropertyChanged Logic
        }

        #endregion

        public DRMPropertiesRowViewModel(TEntity entity) : base(entity)
        {
            DatePlayExpires = entity.DatePlayExpires;
            DatePlayStarts = entity.DatePlayStarts;
            Description = entity.Description;
            IsProtected = entity.IsProtected;
            PlayCount = entity.PlayCount;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IDRMProperties.DatePlayExpires):
                    Dispatcher.CheckInvoke(() => DatePlayExpires = Entity.DatePlayExpires);
                    break;
                case nameof(IDRMProperties.DatePlayStarts):
                    Dispatcher.CheckInvoke(() => DatePlayStarts = Entity.DatePlayStarts);
                    break;
                case nameof(IDRMProperties.Description):
                    Dispatcher.CheckInvoke(() => Description = Entity.Description);
                    break;
                case nameof(IDRMProperties.IsProtected):
                    Dispatcher.CheckInvoke(() => IsProtected = Entity.IsProtected);
                    break;
                case nameof(IDRMProperties.PlayCount):
                    Dispatcher.CheckInvoke(() => PlayCount = Entity.PlayCount);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
