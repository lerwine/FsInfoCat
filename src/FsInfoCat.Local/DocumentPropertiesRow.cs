﻿using FsInfoCat.Collections;
using System;

namespace FsInfoCat.Local
{
    public class DocumentPropertiesRow : PropertiesRow, IDocumentProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<string> _clientID;
        private readonly IPropertyChangeTracker<MultiStringValue> _contributor;
        private readonly IPropertyChangeTracker<DateTime?> _dateCreated;
        private readonly IPropertyChangeTracker<string> _lastAuthor;
        private readonly IPropertyChangeTracker<string> _revisionNumber;
        private readonly IPropertyChangeTracker<int?> _security;
        private readonly IPropertyChangeTracker<string> _division;
        private readonly IPropertyChangeTracker<string> _documentID;
        private readonly IPropertyChangeTracker<string> _manager;
        private readonly IPropertyChangeTracker<string> _presentationFormat;
        private readonly IPropertyChangeTracker<string> _version;

        #endregion

        #region Properties

        public string ClientID { get => _clientID.GetValue(); set => _clientID.SetValue(value); }

        public MultiStringValue Contributor { get => _contributor.GetValue(); set => _contributor.SetValue(value); }

        public DateTime? DateCreated { get => _dateCreated.GetValue(); set => _dateCreated.SetValue(value); }

        public string LastAuthor { get => _lastAuthor.GetValue(); set => _lastAuthor.SetValue(value); }

        public string RevisionNumber { get => _revisionNumber.GetValue(); set => _revisionNumber.SetValue(value); }

        public int? Security { get => _security.GetValue(); set => _security.SetValue(value); }

        public string Division { get => _division.GetValue(); set => _division.SetValue(value); }

        public string DocumentID { get => _documentID.GetValue(); set => _documentID.SetValue(value); }

        public string Manager { get => _manager.GetValue(); set => _manager.SetValue(value); }

        public string PresentationFormat { get => _presentationFormat.GetValue(); set => _presentationFormat.SetValue(value); }

        public string Version { get => _version.GetValue(); set => _version.SetValue(value); }

        #endregion

        public DocumentPropertiesRow()
        {
            _clientID = AddChangeTracker(nameof(ClientID), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _contributor = AddChangeTracker<MultiStringValue>(nameof(Contributor), null);
            _dateCreated = AddChangeTracker<DateTime?>(nameof(DateCreated), null);
            _lastAuthor = AddChangeTracker(nameof(LastAuthor), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _revisionNumber = AddChangeTracker(nameof(RevisionNumber), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _security = AddChangeTracker<int?>(nameof(Security), null);
            _division = AddChangeTracker(nameof(Division), null, FilePropertiesComparer.StringValueCoersion);
            _documentID = AddChangeTracker(nameof(DocumentID), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _manager = AddChangeTracker(nameof(Manager), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _presentationFormat = AddChangeTracker(nameof(PresentationFormat), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _version = AddChangeTracker(nameof(Version), null, FilePropertiesComparer.NormalizedStringValueCoersion);
        }
    }
}