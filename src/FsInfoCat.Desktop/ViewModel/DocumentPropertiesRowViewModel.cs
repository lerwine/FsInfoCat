using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DocumentPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IDocumentProperties
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region ClientID Property Members

        /// <summary>
        /// Identifies the <see cref="ClientID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClientIDProperty = DependencyProperty.Register(nameof(ClientID), typeof(string),
            typeof(DocumentPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnClientIDPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ClientID { get => GetValue(ClientIDProperty) as string; set => SetValue(ClientIDProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ClientID"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ClientID"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ClientID"/> property.</param>
        protected void OnClientIDPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnClientIDPropertyChanged Logic
        }

        #endregion
        #region DateCreated Property Members

        /// <summary>
        /// Identifies the <see cref="DateCreated"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateCreatedProperty = DependencyProperty.Register(nameof(DateCreated), typeof(DateTime?),
            typeof(DocumentPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnDateCreatedPropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? DateCreated { get => (DateTime?)GetValue(DateCreatedProperty); set => SetValue(DateCreatedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DateCreated"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DateCreated"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DateCreated"/> property.</param>
        protected void OnDateCreatedPropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            // TODO: Implement OnDateCreatedPropertyChanged Logic
        }

        #endregion
        #region LastAuthor Property Members

        /// <summary>
        /// Identifies the <see cref="LastAuthor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastAuthorProperty = DependencyProperty.Register(nameof(LastAuthor), typeof(string),
            typeof(DocumentPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnLastAuthorPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string LastAuthor { get => GetValue(LastAuthorProperty) as string; set => SetValue(LastAuthorProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastAuthor"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastAuthor"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastAuthor"/> property.</param>
        protected void OnLastAuthorPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnLastAuthorPropertyChanged Logic
        }

        #endregion
        #region RevisionNumber Property Members

        /// <summary>
        /// Identifies the <see cref="RevisionNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RevisionNumberProperty = DependencyProperty.Register(nameof(RevisionNumber), typeof(string),
            typeof(DocumentPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnRevisionNumberPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string RevisionNumber { get => GetValue(RevisionNumberProperty) as string; set => SetValue(RevisionNumberProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RevisionNumber"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RevisionNumber"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RevisionNumber"/> property.</param>
        protected void OnRevisionNumberPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnRevisionNumberPropertyChanged Logic
        }

        #endregion
        #region Security Property Members

        /// <summary>
        /// Identifies the <see cref="Security"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SecurityProperty = DependencyProperty.Register(nameof(Security), typeof(int?), typeof(DocumentPropertiesRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnSecurityPropertyChanged((int?)e.OldValue, (int?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public int? Security { get => (int?)GetValue(SecurityProperty); set => SetValue(SecurityProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Security"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Security"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Security"/> property.</param>
        protected void OnSecurityPropertyChanged(int? oldValue, int? newValue)
        {
            // TODO: Implement OnSecurityPropertyChanged Logic
        }

        #endregion
        #region Division Property Members

        /// <summary>
        /// Identifies the <see cref="Division"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DivisionProperty = DependencyProperty.Register(nameof(Division), typeof(string),
            typeof(DocumentPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnDivisionPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Division { get => GetValue(DivisionProperty) as string; set => SetValue(DivisionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Division"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Division"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Division"/> property.</param>
        protected void OnDivisionPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnDivisionPropertyChanged Logic
        }

        #endregion
        #region DocumentID Property Members

        /// <summary>
        /// Identifies the <see cref="DocumentID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DocumentIDProperty = DependencyProperty.Register(nameof(DocumentID), typeof(string),
            typeof(DocumentPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnDocumentIDPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DocumentID { get => GetValue(DocumentIDProperty) as string; set => SetValue(DocumentIDProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DocumentID"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DocumentID"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DocumentID"/> property.</param>
        protected void OnDocumentIDPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnDocumentIDPropertyChanged Logic
        }

        #endregion
        #region Manager Property Members

        /// <summary>
        /// Identifies the <see cref="Manager"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ManagerProperty = DependencyProperty.Register(nameof(Manager), typeof(string),
            typeof(DocumentPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnManagerPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Manager { get => GetValue(ManagerProperty) as string; set => SetValue(ManagerProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Manager"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Manager"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Manager"/> property.</param>
        protected void OnManagerPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnManagerPropertyChanged Logic
        }

        #endregion
        #region PresentationFormat Property Members

        /// <summary>
        /// Identifies the <see cref="PresentationFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PresentationFormatProperty = DependencyProperty.Register(nameof(PresentationFormat), typeof(string),
            typeof(DocumentPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnPresentationFormatPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string PresentationFormat { get => GetValue(PresentationFormatProperty) as string; set => SetValue(PresentationFormatProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="PresentationFormat"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="PresentationFormat"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="PresentationFormat"/> property.</param>
        protected void OnPresentationFormatPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnPresentationFormatPropertyChanged Logic
        }

        #endregion
        #region Version Property Members

        /// <summary>
        /// Identifies the <see cref="Version"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(nameof(Version), typeof(string),
            typeof(DocumentPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DocumentPropertiesRowViewModel<TEntity>)?.OnVersionPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Version { get => GetValue(VersionProperty) as string; set => SetValue(VersionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Version"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Version"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Version"/> property.</param>
        protected void OnVersionPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnVersionPropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public DocumentPropertiesRowViewModel(TEntity entity) : base(entity)
        {
            ClientID = entity.ClientID;
            DateCreated = entity.DateCreated;
            LastAuthor = entity.LastAuthor;
            RevisionNumber = entity.RevisionNumber;
            Security = entity.Security;
            Division = entity.Division;
            DocumentID = entity.DocumentID;
            Manager = entity.Manager;
            PresentationFormat = entity.PresentationFormat;
            Version = entity.Version;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IDocumentProperties.ClientID):
                    Dispatcher.CheckInvoke(() => ClientID = Entity.ClientID);
                    break;
                case nameof(IDocumentProperties.DateCreated):
                    Dispatcher.CheckInvoke(() => DateCreated = Entity.DateCreated);
                    break;
                case nameof(IDocumentProperties.LastAuthor):
                    Dispatcher.CheckInvoke(() => LastAuthor = Entity.LastAuthor);
                    break;
                case nameof(IDocumentProperties.RevisionNumber):
                    Dispatcher.CheckInvoke(() => RevisionNumber = Entity.RevisionNumber);
                    break;
                case nameof(IDocumentProperties.Security):
                    Dispatcher.CheckInvoke(() => Security = Entity.Security);
                    break;
                case nameof(IDocumentProperties.Division):
                    Dispatcher.CheckInvoke(() => Division = Entity.Division);
                    break;
                case nameof(IDocumentProperties.DocumentID):
                    Dispatcher.CheckInvoke(() => DocumentID = Entity.DocumentID);
                    break;
                case nameof(IDocumentProperties.Manager):
                    Dispatcher.CheckInvoke(() => Manager = Entity.Manager);
                    break;
                case nameof(IDocumentProperties.PresentationFormat):
                    Dispatcher.CheckInvoke(() => PresentationFormat = Entity.PresentationFormat);
                    break;
                case nameof(IDocumentProperties.Version):
                    Dispatcher.CheckInvoke(() => Version = Entity.Version);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
