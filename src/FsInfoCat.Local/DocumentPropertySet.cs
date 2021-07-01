using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class DocumentPropertySet : LocalDbEntity, ILocalDocumentPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
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
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

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

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        #endregion

        public DocumentPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _clientID = AddChangeTracker<string>(nameof(ClientID), null);
            _contributor = AddChangeTracker<MultiStringValue>(nameof(Contributor), null);
            _dateCreated = AddChangeTracker<DateTime?>(nameof(DateCreated), null);
            _lastAuthor = AddChangeTracker<string>(nameof(LastAuthor), null);
            _revisionNumber = AddChangeTracker<string>(nameof(RevisionNumber), null);
            _security = AddChangeTracker<int?>(nameof(Security), null);
            _division = AddChangeTracker<string>(nameof(Division), null);
            _documentID = AddChangeTracker<string>(nameof(DocumentID), null);
            _manager = AddChangeTracker<string>(nameof(Manager), null);
            _presentationFormat = AddChangeTracker<string>(nameof(PresentationFormat), null);
            _version = AddChangeTracker<string>(nameof(Version), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<DocumentPropertySet> obj) => obj.Property(nameof(Contributor)).HasConversion(MultiStringValue.Converter);

        internal static async Task RefreshAsync(EntityEntry<DbFile> entry, IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
        {
            DocumentPropertySet oldDocumentPropertySet = entry.Entity.DocumentPropertySetId.HasValue ? await entry.GetRelatedReferenceAsync(f => f.DocumentProperties, cancellationToken) : null;
            IDocumentProperties currentDocumentProperties = await fileDetailProvider.GetDocumentPropertiesAsync(cancellationToken);
            // TODO: Implement RefreshAsync(EntityEntry<DbFile>, IFileDetailProvider, CancellationToken)
            throw new NotImplementedException();
        }
    }
}
