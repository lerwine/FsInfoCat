using System;
using System.Diagnostics.CodeAnalysis;
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

        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
        #region Author Property Members

        private static readonly DependencyPropertyKey AuthorPropertyKey = ColumnPropertyBuilder<string, SummaryPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISummaryPropertiesListItem.Author))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Author"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AuthorProperty = AuthorPropertyKey.DependencyProperty;

        public string Author { get => GetValue(AuthorProperty) as string; private set => SetValue(AuthorPropertyKey, value); }

        #endregion
        #region Keywords Property Members

        private static readonly DependencyPropertyKey KeywordsPropertyKey = ColumnPropertyBuilder<string, SummaryPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISummaryPropertiesListItem.Keywords))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Keywords"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KeywordsProperty = KeywordsPropertyKey.DependencyProperty;

        public string Keywords { get => GetValue(KeywordsProperty) as string; private set => SetValue(KeywordsPropertyKey, value); }

        #endregion
        #region ItemAuthors Property Members

        private static readonly DependencyPropertyKey ItemAuthorsPropertyKey = ColumnPropertyBuilder<string, SummaryPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISummaryPropertiesListItem.ItemAuthors))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ItemAuthors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemAuthorsProperty = ItemAuthorsPropertyKey.DependencyProperty;

        public string ItemAuthors { get => GetValue(ItemAuthorsProperty) as string; private set => SetValue(ItemAuthorsPropertyKey, value); }

        #endregion
        #region Kind Property Members

        private static readonly DependencyPropertyKey KindPropertyKey = ColumnPropertyBuilder<string, SummaryPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISummaryPropertiesListItem.Kind))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Kind"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KindProperty = KindPropertyKey.DependencyProperty;

        public string Kind { get => GetValue(KindProperty) as string; private set => SetValue(KindPropertyKey, value); }

        #endregion
        #region ExistingFileCount Property Members

        private static readonly DependencyPropertyKey ExistingFileCountPropertyKey = ColumnPropertyBuilder<long, SummaryPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISummaryPropertiesListItem.ExistingFileCount))
            .DefaultValue(0L)
            .OnChanged((DependencyObject d, long oldValue, long newValue) =>
                (d as SummaryPropertiesListItemViewModel<TEntity>).OnExistingFileCountPropertyChanged(newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ExistingFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExistingFileCountProperty = ExistingFileCountPropertyKey.DependencyProperty;

        public long ExistingFileCount { get => (long)GetValue(ExistingFileCountProperty); private set => SetValue(ExistingFileCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="ExistingFileCount"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="ExistingFileCount"/> property.</param>
        /// 
        private void OnExistingFileCountPropertyChanged(long newValue) => Delete.IsEnabled = newValue == 0L;

        #endregion
        #region TotalFileCount Property Members

        private static readonly DependencyPropertyKey TotalFileCountPropertyKey = ColumnPropertyBuilder<long, SummaryPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(ISummaryPropertiesListItem.TotalFileCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = TotalFileCountPropertyKey.DependencyProperty;

        public long TotalFileCount { get => (long)GetValue(TotalFileCountProperty); private set => SetValue(TotalFileCountPropertyKey, value); }

        #endregion

        public SummaryPropertiesListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Author = entity.Author.ToNormalizedDelimitedText();
            Keywords = entity.Keywords.ToNormalizedDelimitedText();
            ItemAuthors = entity.ItemAuthors.ToNormalizedDelimitedText();
            Kind = entity.Kind.ToNormalizedDelimitedText();
            ExistingFileCount = entity.ExistingFileCount;
            TotalFileCount = entity.TotalFileCount;
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            CommonAttached.SetListItemTitle(this, CalculateDisplayText());
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            CommonAttached.SetListItemTitle(this, CalculateDisplayText());
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            // TODO: Ensure ItemTypeText is set to ItemType if it is empty
            // TODO: Ensure SensitivityText is set to Sensitivity if it is empty

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
