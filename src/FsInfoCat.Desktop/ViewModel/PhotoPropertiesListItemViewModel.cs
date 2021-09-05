using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class PhotoPropertiesListItemViewModel<TEntity> : PhotoPropertiesRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IPhotoPropertiesListItem
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(PhotoPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(PhotoPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

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
        #region Event Property Members

        private static readonly DependencyPropertyKey EventPropertyKey = ColumnPropertyBuilder<string, PhotoPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoPropertiesListItem.Event))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Event"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EventProperty = EventPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Event { get => GetValue(EventProperty) as string; private set => SetValue(EventPropertyKey, value); }

        #endregion
        #region PeopleNames Property Members

        private static readonly DependencyPropertyKey PeopleNamesPropertyKey = ColumnPropertyBuilder<string, PhotoPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoPropertiesListItem.PeopleNames))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PeopleNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PeopleNamesProperty = PeopleNamesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string PeopleNames { get => GetValue(PeopleNamesProperty) as string; private set => SetValue(PeopleNamesPropertyKey, value); }

        #endregion
        #region ExistingFileCount Property Members

        private static readonly DependencyPropertyKey ExistingFileCountPropertyKey = ColumnPropertyBuilder<long, PhotoPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoPropertiesListItem.ExistingFileCount))
            .DefaultValue(0L)
            .OnChanged((DependencyObject d, long oldValue, long newValue) =>
                (d as PhotoPropertiesListItemViewModel<TEntity>).OnExistingFileCountPropertyChanged(oldValue, newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ExistingFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExistingFileCountProperty = ExistingFileCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long ExistingFileCount { get => (long)GetValue(ExistingFileCountProperty); private set => SetValue(ExistingFileCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="ExistingFileCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ExistingFileCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ExistingFileCount"/> property.</param>
        private void OnExistingFileCountPropertyChanged(long oldValue, long newValue) => Delete.IsEnabled = newValue == 0L;

        #endregion
        #region TotalFileCount Property Members

        private static readonly DependencyPropertyKey TotalFileCountPropertyKey = ColumnPropertyBuilder<long, PhotoPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoPropertiesListItem.TotalFileCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = TotalFileCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long TotalFileCount { get => (long)GetValue(TotalFileCountProperty); private set => SetValue(TotalFileCountPropertyKey, value); }

        #endregion

        public PhotoPropertiesListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Event = entity.Event.ToNormalizedDelimitedText();
            PeopleNames = entity.PeopleNames.ToNormalizedDelimitedText();
            ExistingFileCount = entity.ExistingFileCount;
            TotalFileCount = entity.TotalFileCount;
            CommonAttached.SetListItemTitle(this, CalculateDisplayText());
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            CommonAttached.SetListItemTitle(this, CalculateDisplayText());
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            // TODO: Ensure OrientationText is set to Orientation if it is empty
            switch (propertyName)
            {
                case nameof(IPhotoProperties.Event):
                    Dispatcher.CheckInvoke(() => Event = Entity.Event.ToNormalizedDelimitedText());
                    break;
                case nameof(IPhotoProperties.PeopleNames):
                    Dispatcher.CheckInvoke(() => PeopleNames = Entity.PeopleNames.ToNormalizedDelimitedText());
                    break;
                case nameof(ExistingFileCount):
                    Dispatcher.CheckInvoke(() => ExistingFileCount = Entity.ExistingFileCount);
                    break;
                case nameof(TotalFileCount):
                    Dispatcher.CheckInvoke(() => TotalFileCount = Entity.TotalFileCount);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
