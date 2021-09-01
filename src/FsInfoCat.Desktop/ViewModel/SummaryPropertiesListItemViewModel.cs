using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SummaryPropertiesListItemViewModel<TEntity> : SummaryPropertiesRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, ISummaryPropertiesListItem
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

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
        #region Author Property Members

        private static readonly DependencyPropertyKey AuthorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Author), typeof(string),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Author"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AuthorProperty = AuthorPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Author { get => GetValue(AuthorProperty) as string; private set => SetValue(AuthorPropertyKey, value); }

        #endregion
        #region Keywords Property Members

        private static readonly DependencyPropertyKey KeywordsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Keywords), typeof(string),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Keywords"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KeywordsProperty = KeywordsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Keywords { get => GetValue(KeywordsProperty) as string; private set => SetValue(KeywordsPropertyKey, value); }

        #endregion
        #region ItemAuthors Property Members

        private static readonly DependencyPropertyKey ItemAuthorsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ItemAuthors), typeof(string),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="ItemAuthors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemAuthorsProperty = ItemAuthorsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ItemAuthors { get => GetValue(ItemAuthorsProperty) as string; private set => SetValue(ItemAuthorsPropertyKey, value); }

        #endregion
        #region Kind Property Members

        private static readonly DependencyPropertyKey KindPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Kind), typeof(string),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Kind"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KindProperty = KindPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Kind { get => GetValue(KindProperty) as string; private set => SetValue(KindPropertyKey, value); }

        #endregion
        #region ExistingFileCount Property Members

        private static readonly DependencyPropertyKey ExistingFileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ExistingFileCount), typeof(long),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(0L, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as SummaryPropertiesListItemViewModel<TEntity>).OnExistingFileCountPropertyChanged((long)e.OldValue, (long)e.NewValue)));

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

        private static readonly DependencyPropertyKey TotalFileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TotalFileCount), typeof(long),
            typeof(SummaryPropertiesListItemViewModel<TEntity>), new PropertyMetadata(0L));

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

        public SummaryPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            Author = entity.Author.ToNormalizedDelimitedText();
            Keywords = entity.Keywords.ToNormalizedDelimitedText();
            ItemAuthors = entity.ItemAuthors.ToNormalizedDelimitedText();
            Kind = entity.Kind.ToNormalizedDelimitedText();
            ExistingFileCount = entity.ExistingFileCount;
            TotalFileCount = entity.TotalFileCount;
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ISummaryProperties.Author):
                    Dispatcher.CheckInvoke(() => Author = Entity.Author.ToNormalizedDelimitedText());
                    break;
                case nameof(ISummaryProperties.Keywords):
                    Dispatcher.CheckInvoke(() => Keywords = Entity.Keywords.ToNormalizedDelimitedText());
                    break;
                case nameof(ISummaryProperties.ItemAuthors):
                    Dispatcher.CheckInvoke(() => ItemAuthors = Entity.ItemAuthors.ToNormalizedDelimitedText());
                    break;
                case nameof(ISummaryProperties.Kind):
                    Dispatcher.CheckInvoke(() => Kind = Entity.Kind.ToNormalizedDelimitedText());
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
