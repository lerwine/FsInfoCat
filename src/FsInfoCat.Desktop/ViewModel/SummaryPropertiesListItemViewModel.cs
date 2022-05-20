using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SummaryPropertiesListItemViewModel<TEntity> : SummaryPropertiesRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.ISummaryPropertiesListItem
    {
        #region Open Command Property Members

        /// <summary>
        /// Occurs when the <see cref="Open"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenCommand;

        private static readonly DependencyPropertyKey OpenPropertyKey = DependencyPropertyBuilder<SummaryPropertiesListItemViewModel<TEntity>, Commands.RelayCommand>
            .Register(nameof(Open))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Open"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenProperty = OpenPropertyKey.DependencyProperty;

        public Commands.RelayCommand Open => (Commands.RelayCommand)GetValue(OpenProperty);

        /// <summary>
        /// Called when the Open event is raised by <see cref="Open" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Open" />.</param>
        protected void RaiseOpenCommand(object parameter) => OpenCommand?.Invoke(this, new(parameter));

        #endregion
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
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryPropertiesListItem.Author))
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
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryPropertiesListItem.Keywords))
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
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryPropertiesListItem.ItemAuthors))
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
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryPropertiesListItem.Kind))
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
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryPropertiesListItem.ExistingFileCount))
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
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryPropertiesListItem.TotalFileCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = TotalFileCountPropertyKey.DependencyProperty;

        public long TotalFileCount { get => (long)GetValue(TotalFileCountProperty); private set => SetValue(TotalFileCountPropertyKey, value); }

        #endregion

        public SummaryPropertiesListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(OpenPropertyKey, new Commands.RelayCommand(RaiseOpenCommand));
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            Author = entity.Author.ToNormalizedDelimitedText();
            Keywords = entity.Keywords.ToNormalizedDelimitedText();
            ItemAuthors = entity.ItemAuthors.ToNormalizedDelimitedText();
            Kind = entity.Kind.ToNormalizedDelimitedText();
            ExistingFileCount = entity.ExistingFileCount;
            TotalFileCount = entity.TotalFileCount;
            CommonAttached.SetListItemTitle(this, CalculateDisplayText());
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            CommonAttached.SetListItemTitle(this, CalculateDisplayText());
        }
    }
}
