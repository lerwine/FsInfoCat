using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : Model.DbEntity, Model.IDocumentPropertiesListItem
        where TViewModel : DocumentPropertiesListItemViewModel<TEntity>
    {
        #region ClientID Property Members

        /// <summary>
        /// Identifies the <see cref="ClientID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClientIDProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ClientID))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ClientID { get => (bool)GetValue(ClientIDProperty); set => SetValue(ClientIDProperty, value); }

        #endregion
        #region Contributor Property Members

        /// <summary>
        /// Identifies the <see cref="Contributor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContributorProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Contributor))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Contributor { get => (bool)GetValue(ContributorProperty); set => SetValue(ContributorProperty, value); }

        #endregion
        #region DateCreated Property Members

        /// <summary>
        /// Identifies the <see cref="DateCreated"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateCreatedProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(DateCreated))
            .DefaultValue(false)
            .AsReadWrite();

        public bool DateCreated { get => (bool)GetValue(DateCreatedProperty); set => SetValue(DateCreatedProperty, value); }

        #endregion
        #region Division Property Members

        /// <summary>
        /// Identifies the <see cref="Division"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DivisionProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Division))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Division { get => (bool)GetValue(DivisionProperty); set => SetValue(DivisionProperty, value); }

        #endregion
        #region DocumentID Property Members

        /// <summary>
        /// Identifies the <see cref="DocumentID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DocumentIDProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(DocumentID))
            .DefaultValue(false)
            .AsReadWrite();

        public bool DocumentID { get => (bool)GetValue(DocumentIDProperty); set => SetValue(DocumentIDProperty, value); }

        #endregion
        #region LastAuthor Property Members

        /// <summary>
        /// Identifies the <see cref="LastAuthor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastAuthorProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(LastAuthor))
            .DefaultValue(false)
            .AsReadWrite();

        public bool LastAuthor { get => (bool)GetValue(LastAuthorProperty); set => SetValue(LastAuthorProperty, value); }

        #endregion
        #region Manager Property Members

        /// <summary>
        /// Identifies the <see cref="Manager"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ManagerProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Manager))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Manager { get => (bool)GetValue(ManagerProperty); set => SetValue(ManagerProperty, value); }

        #endregion
        #region PresentationFormat Property Members

        /// <summary>
        /// Identifies the <see cref="PresentationFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PresentationFormatProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(PresentationFormat))
            .DefaultValue(false)
            .AsReadWrite();

        public bool PresentationFormat { get => (bool)GetValue(PresentationFormatProperty); set => SetValue(PresentationFormatProperty, value); }

        #endregion
        #region RevisionNumber Property Members

        /// <summary>
        /// Identifies the <see cref="RevisionNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RevisionNumberProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(RevisionNumber))
            .DefaultValue(false)
            .AsReadWrite();

        public bool RevisionNumber { get => (bool)GetValue(RevisionNumberProperty); set => SetValue(RevisionNumberProperty, value); }

        #endregion
        #region Security Property Members

        /// <summary>
        /// Identifies the <see cref="Security"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SecurityProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Security))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Security { get => (bool)GetValue(SecurityProperty); set => SetValue(SecurityProperty, value); }

        #endregion
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region Version Property Members

        /// <summary>
        /// Identifies the <see cref="Version"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VersionProperty = DependencyPropertyBuilder<DocumentPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Version))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Version { get => (bool)GetValue(VersionProperty); set => SetValue(VersionProperty, value); }

        #endregion

        protected DocumentPropertiesColumnVisibilityOptions() : base() { }
    }
}
