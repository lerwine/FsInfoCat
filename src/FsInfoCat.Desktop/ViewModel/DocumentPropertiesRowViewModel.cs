using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DocumentPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IDocumentProperties
    {
        #region ClientID Property Members

        /// <summary>
        /// Identifies the <see cref="ClientID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClientIDProperty = ColumnPropertyBuilder<string, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.ClientID))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnClientIDPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ClientID { get => GetValue(ClientIDProperty) as string; set => SetValue(ClientIDProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ClientID"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ClientID"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ClientID"/> property.</param>
        protected virtual void OnClientIDPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region DateCreated Property Members

        /// <summary>
        /// Identifies the <see cref="DateCreated"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateCreatedProperty = ColumnPropertyBuilder<DateTime?, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.DateCreated))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnDateCreatedPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime? DateCreated { get => (DateTime?)GetValue(DateCreatedProperty); set => SetValue(DateCreatedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DateCreated"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DateCreated"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DateCreated"/> property.</param>
        protected virtual void OnDateCreatedPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion
        #region LastAuthor Property Members

        /// <summary>
        /// Identifies the <see cref="LastAuthor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastAuthorProperty = ColumnPropertyBuilder<string, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.LastAuthor))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnLastAuthorPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string LastAuthor { get => GetValue(LastAuthorProperty) as string; set => SetValue(LastAuthorProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LastAuthor"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LastAuthor"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LastAuthor"/> property.</param>
        protected virtual void OnLastAuthorPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region RevisionNumber Property Members

        /// <summary>
        /// Identifies the <see cref="RevisionNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RevisionNumberProperty = ColumnPropertyBuilder<string, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.RevisionNumber))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnRevisionNumberPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string RevisionNumber { get => GetValue(RevisionNumberProperty) as string; set => SetValue(RevisionNumberProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RevisionNumber"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RevisionNumber"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RevisionNumber"/> property.</param>
        protected virtual void OnRevisionNumberPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Security Property Members

        /// <summary>
        /// Identifies the <see cref="Security"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SecurityProperty = ColumnPropertyBuilder<int?, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.Security))
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnSecurityPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public int? Security { get => (int?)GetValue(SecurityProperty); set => SetValue(SecurityProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Security"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Security"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Security"/> property.</param>
        protected virtual void OnSecurityPropertyChanged(int? oldValue, int? newValue) { }

        #endregion
        #region Division Property Members

        /// <summary>
        /// Identifies the <see cref="Division"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DivisionProperty = ColumnPropertyBuilder<string, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.Division))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnDivisionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string Division { get => GetValue(DivisionProperty) as string; set => SetValue(DivisionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Division"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Division"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Division"/> property.</param>
        protected virtual void OnDivisionPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region DocumentID Property Members

        /// <summary>
        /// Identifies the <see cref="DocumentID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DocumentIDProperty = ColumnPropertyBuilder<string, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.DocumentID))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnDocumentIDPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string DocumentID { get => GetValue(DocumentIDProperty) as string; set => SetValue(DocumentIDProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DocumentID"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DocumentID"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DocumentID"/> property.</param>
        protected virtual void OnDocumentIDPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Manager Property Members

        /// <summary>
        /// Identifies the <see cref="Manager"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ManagerProperty = ColumnPropertyBuilder<string, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.Manager))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnManagerPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string Manager { get => GetValue(ManagerProperty) as string; set => SetValue(ManagerProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Manager"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Manager"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Manager"/> property.</param>
        protected virtual void OnManagerPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region PresentationFormat Property Members

        /// <summary>
        /// Identifies the <see cref="PresentationFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PresentationFormatProperty = ColumnPropertyBuilder<string, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.PresentationFormat))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnPresentationFormatPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string PresentationFormat { get => GetValue(PresentationFormatProperty) as string; set => SetValue(PresentationFormatProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="PresentationFormat"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="PresentationFormat"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="PresentationFormat"/> property.</param>
        protected virtual void OnPresentationFormatPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Version Property Members

        /// <summary>
        /// Identifies the <see cref="Version"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VersionProperty = ColumnPropertyBuilder<string, DocumentPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDocumentProperties.Version))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as DocumentPropertiesRowViewModel<TEntity>)?.OnVersionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string Version { get => GetValue(VersionProperty) as string; set => SetValue(VersionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Version"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Version"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Version"/> property.</param>
        protected virtual void OnVersionPropertyChanged(string oldValue, string newValue) { }

        #endregion

        public DocumentPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
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

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_DocumentID, DocumentID.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ClientID, ClientID.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_LastAuthor, LastAuthor.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Manager, Manager.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Division, Division.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_DateCreated, DateCreated?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_PresentationFormat, PresentationFormat.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Security, Security?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_RevisionNumber, RevisionNumber.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Version, Version.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));
    }
}
