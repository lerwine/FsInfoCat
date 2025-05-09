using FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities containing extended file properties for document files.
    /// </summary>
    /// <seealso cref="DocumentPropertiesListItem" />
    /// <seealso cref="DocumentPropertySet" />
    public abstract class DocumentPropertiesRow : PropertiesRow, ILocalDocumentPropertiesRow
    {
        #region Fields

        private string _clientID = string.Empty;
        private string _lastAuthor = string.Empty;
        private string _revisionNumber = string.Empty;
        private string _division = string.Empty;
        private string _documentID = string.Empty;
        private string _manager = string.Empty;
        private string _presentationFormat = string.Empty;
        private string _version = string.Empty;

        #endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        #region Properties

        public string ClientID { get => _clientID; set => _clientID = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Contributor { get; set; }

        public DateTime? DateCreated { get; set; }

        [NotNull]
        [BackingField(nameof(_lastAuthor))]
        public string LastAuthor { get => _lastAuthor; set => _lastAuthor = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_revisionNumber))]
        public string RevisionNumber { get => _revisionNumber; set => _revisionNumber = value.AsWsNormalizedOrEmpty(); }

        public int? Security { get; set; }

        [NotNull]
        [BackingField(nameof(_division))]
        public string Division { get => _division; set => _division = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_documentID))]
        public string DocumentID { get => _documentID; set => _documentID = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_manager))]
        public string Manager { get => _manager; set => _manager = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_presentationFormat))]
        public string PresentationFormat { get => _presentationFormat; set => _presentationFormat = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        [BackingField(nameof(_version))]
        public string Version { get => _version; set => _version = value.AsWsNormalizedOrEmpty(); }

        #endregion

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalDocumentPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalDocumentPropertiesRow other) => ArePropertiesEqual((IDocumentPropertiesRow)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IDocumentPropertiesRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IDocumentPropertiesRow other) => ArePropertiesEqual((IDocumentProperties)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IDocumentProperties" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IDocumentProperties other) => _clientID == other.ClientID &&
            EqualityComparer<MultiStringValue>.Default.Equals(Contributor, other.Contributor) &&
            DateCreated == other.DateCreated &&
            _lastAuthor == other.LastAuthor &&
            _revisionNumber == other.RevisionNumber &&
            Security == other.Security &&
            _division == other.Division &&
            _documentID == other.DocumentID &&
            _manager == other.Manager &&
            _presentationFormat == other.PresentationFormat &&
            _version == other.Version;

        public abstract bool Equals(IDocumentPropertiesRow other);

        public abstract bool Equals(IDocumentProperties other);

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            HashCode hash = new();
            hash.Add(_clientID);
            hash.Add(Contributor);
            hash.Add(DateCreated);
            hash.Add(_lastAuthor);
            hash.Add(_revisionNumber);
            hash.Add(Security);
            hash.Add(_division);
            hash.Add(_documentID);
            hash.Add(_manager);
            hash.Add(_presentationFormat);
            hash.Add(_version);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }
        protected virtual string PropertiesToString() => $@"Security={Security}, DocumentID=""{ExtensionMethods.EscapeCsString(_documentID)}"", ClientID=""{ExtensionMethods.EscapeCsString(_clientID)}"", PresentationFormat=""{ExtensionMethods.EscapeCsString(_presentationFormat)}"",
    DateCreated={DateCreated:yyyy-mm-ddTHH:mm:ss.fffffff}, Version=""{ExtensionMethods.EscapeCsString(_version)}"", RevisionNumber=""{ExtensionMethods.EscapeCsString(_revisionNumber)}"",
    LastAuthor=""{ExtensionMethods.EscapeCsString(_lastAuthor)}"", Division=""{ExtensionMethods.EscapeCsString(_division)}"", Manager=""{ExtensionMethods.EscapeCsString(_manager)}"",
    Contributor={Contributor.ToCsString()}";

        public override string ToString() => $@"{{ Id={(TryGetId(out Guid id) ? id : null)}, {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId} }}";
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
