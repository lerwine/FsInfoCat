using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DocumentPropertiesListItemViewModel<TEntity> : DocumentPropertiesRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IDocumentPropertiesListItem
    {
        #region Open Command Property Members

        /// <summary>
        /// Occurs when the <see cref="Open"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenCommand;

        private static readonly DependencyPropertyKey OpenPropertyKey = DependencyPropertyBuilder<DocumentPropertiesListItemViewModel<TEntity>, Commands.RelayCommand>
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
            typeof(Commands.RelayCommand), typeof(DocumentPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

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
            typeof(Commands.RelayCommand), typeof(DocumentPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

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
        #region Contributor Property Members

        private static readonly DependencyPropertyKey ContributorPropertyKey = ColumnPropertyBuilder<string, DocumentPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentPropertiesListItem.Contributor))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Contributor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContributorProperty = ContributorPropertyKey.DependencyProperty;

        public string Contributor { get => GetValue(ContributorProperty) as string; private set => SetValue(ContributorPropertyKey, value); }

        #endregion
        #region ExistingFileCount Property Members

        private static readonly DependencyPropertyKey ExistingFileCountPropertyKey = ColumnPropertyBuilder<long,DocumentPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentPropertiesListItem.ExistingFileCount))
            .DefaultValue(0L)
            .OnChanged((DependencyObject d, long oldValue, long newValue) =>
                (d as DocumentPropertiesListItemViewModel<TEntity>).OnExistingFileCountPropertyChanged(newValue))
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
        private void OnExistingFileCountPropertyChanged(long newValue) => Delete.IsEnabled = newValue == 0L;

        #endregion
        #region TotalFileCount Property Members

        private static readonly DependencyPropertyKey TotalFileCountPropertyKey = ColumnPropertyBuilder<long, DocumentPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentPropertiesListItem.TotalFileCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = TotalFileCountPropertyKey.DependencyProperty;

        public long TotalFileCount { get => (long)GetValue(TotalFileCountProperty); private set => SetValue(TotalFileCountPropertyKey, value); }

        #endregion

        public DocumentPropertiesListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(OpenPropertyKey, new Commands.RelayCommand(RaiseOpenCommand));
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            Contributor = Entity.Contributor.ToNormalizedDelimitedText();
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
