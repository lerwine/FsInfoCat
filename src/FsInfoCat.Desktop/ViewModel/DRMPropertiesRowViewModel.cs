using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DRMPropertiesListItemViewModel<TEntity> : DRMPropertiesRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IDRMPropertiesListItem
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(DRMPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Edit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditProperty = EditPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Edit => (Commands.RelayCommand)GetValue(EditProperty);

        /// <summary>
        /// Called when the Edit event is raised by <see cref="Edit" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Edit" />.</param>
        protected virtual void RaiseEditCommand(object parameter) => EditCommand?.Invoke(this, new(parameter));

        #endregion
        #region Delete Property Members

        /// <summary>
        /// Occurs when the <see cref="Delete">Delete Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> DeleteCommand;

        private static readonly DependencyPropertyKey DeletePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Delete),
            typeof(Commands.RelayCommand), typeof(DRMPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Delete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteProperty = DeletePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
        #region FileCount Property Members

        private static readonly DependencyPropertyKey FileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileCount), typeof(long), typeof(DRMPropertiesListItemViewModel<TEntity>),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="FileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileCountProperty = FileCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long FileCount { get => (long)GetValue(FileCountProperty); private set => SetValue(FileCountPropertyKey, value); }

        #endregion

        public DRMPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            FileCount = entity.FileCount;
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(FileCount))
                Dispatcher.CheckInvoke(() => FileCount = Entity.FileCount);
        }
    }

    public class DRMPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IDRMProperties
    {
#pragma warning disable IDE0060 // Remove unused parameter
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
        protected void OnDatePlayExpiresPropertyChanged(DateTime? oldValue, DateTime? newValue)
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
        protected void OnDatePlayStartsPropertyChanged(DateTime? oldValue, DateTime? newValue)
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
        protected void OnDescriptionPropertyChanged(string oldValue, string newValue)
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
        protected void OnIsProtectedPropertyChanged(bool? oldValue, bool? newValue)
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
        protected void OnPlayCountPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnPlayCountPropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

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
