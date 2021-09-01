using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MediaPropertiesListItemViewModel<TEntity> : MediaPropertiesRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IMediaPropertiesListItem
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(MediaPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(MediaPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

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
        #region Producer Property Members

        private static readonly DependencyPropertyKey ProducerPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Producer), typeof(string), typeof(MediaPropertiesListItemViewModel<TEntity>),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Producer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProducerProperty = ProducerPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Producer { get => GetValue(ProducerProperty) as string; private set => SetValue(ProducerPropertyKey, value); }

        #endregion
        #region Writer Property Members

        private static readonly DependencyPropertyKey WriterPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Writer), typeof(string), typeof(MediaPropertiesListItemViewModel<TEntity>),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Writer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WriterProperty = WriterPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Writer { get => GetValue(WriterProperty) as string; private set => SetValue(WriterPropertyKey, value); }

        #endregion
        #region ExistingFileCount Property Members

        private static readonly DependencyPropertyKey ExistingFileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ExistingFileCount), typeof(long),
            typeof(MediaPropertiesListItemViewModel<TEntity>), new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as MediaPropertiesListItemViewModel<TEntity>).OnExistingFileCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

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

        private static readonly DependencyPropertyKey TotalFileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TotalFileCount), typeof(long), typeof(MediaPropertiesListItemViewModel<TEntity>),
                new PropertyMetadata(0L));

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

        public MediaPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            Writer = entity.Writer.ToNormalizedDelimitedText();
            Producer = entity.Producer.ToNormalizedDelimitedText();
            ExistingFileCount = entity.ExistingFileCount;
            TotalFileCount = entity.TotalFileCount;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IMediaProperties.Producer):
                    Dispatcher.CheckInvoke(() => Producer = Entity.Producer.ToNormalizedDelimitedText());
                    break;
                case nameof(IMediaProperties.Writer):
                    Dispatcher.CheckInvoke(() => Writer = Entity.Writer.ToNormalizedDelimitedText());
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
