using FsInfoCat.Collections;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
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

        #region Properties

        public string ClientID { get => _clientID; set => _clientID = value.AsWsNormalizedOrEmpty(); }

        public MultiStringValue Contributor { get; set; }

        public DateTime? DateCreated { get; set; }

        [NotNull]
        public string LastAuthor { get => _lastAuthor; set => _lastAuthor = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string RevisionNumber { get => _revisionNumber; set => _revisionNumber = value.AsWsNormalizedOrEmpty(); }

        public int? Security { get; set; }

        [NotNull]
        public string Division { get => _division; set => _division = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string DocumentID { get => _documentID; set => _documentID = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string Manager { get => _manager; set => _manager = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string PresentationFormat { get => _presentationFormat; set => _presentationFormat = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string Version { get => _version; set => _version = value.AsWsNormalizedOrEmpty(); }

        #endregion

        protected bool ArePropertiesEqual([DisallowNull] IDocumentProperties other)
        {
            throw new NotImplementedException();
        }

        public abstract bool Equals(IDocumentPropertiesRow other);

        public abstract bool Equals(IDocumentProperties other);
    }
}
