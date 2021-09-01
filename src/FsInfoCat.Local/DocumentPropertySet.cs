using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class DocumentPropertiesListItem : DocumentPropertiesRow, ILocalDocumentPropertiesListItem
    {
        public const string VIEW_NAME = "vDocumentPropertiesListing";

        private readonly IPropertyChangeTracker<long> _existingFileCount;
        private readonly IPropertyChangeTracker<long> _totalFileCount;

        public long ExistingFileCount { get => _existingFileCount.GetValue(); set => _existingFileCount.SetValue(value); }

        public long TotalFileCount { get => _totalFileCount.GetValue(); set => _totalFileCount.SetValue(value); }

        public DocumentPropertiesListItem()
        {
            _existingFileCount = AddChangeTracker(nameof(ExistingFileCount), 0L);
            _totalFileCount = AddChangeTracker(nameof(TotalFileCount), 0L);
        }

        internal static void OnBuildEntity(EntityTypeBuilder<DocumentPropertiesListItem> builder) => builder.ToView(VIEW_NAME);
    }
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
    /// <summary>
    /// Class DocumentPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalDocumentPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalDocumentPropertySet" />
    public class DocumentPropertySet : DocumentPropertiesRow, ILocalDocumentPropertySet, ISimpleIdentityReference<DocumentPropertySet>
    {
        private HashSet<DbFile> _files = new();

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        DocumentPropertySet IIdentityReference<DocumentPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<DocumentPropertySet> builder) =>
            (builder ?? throw new ArgumentOutOfRangeException(nameof(builder))).Property(nameof(Contributor)).HasConversion(MultiStringValue.Converter);

        internal static async Task RefreshAsync([DisallowNull] EntityEntry<DbFile> entry, [DisallowNull] IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));
            if (fileDetailProvider is null)
                throw new ArgumentNullException(nameof(fileDetailProvider));
            switch (entry.State)
            {
                case EntityState.Detached:
                    throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is detached");
                case EntityState.Deleted:
                    throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is flagged for deletion");
            }
            if (entry.Context is not LocalDbContext dbContext)
                throw new ArgumentOutOfRangeException(nameof(entry), "Invalid database context");
            DbFile entity;
            DocumentPropertySet oldPropertySet = (entity = entry.Entity).DocumentPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.DocumentProperties, cancellationToken) : null;
            IDocumentProperties currentProperties = await fileDetailProvider.GetDocumentPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            if (currentProperties.IsNullOrAllPropertiesEmpty())
                entity.DocumentProperties = null;
            else
                entity.DocumentProperties = await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        dbContext.DocumentPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
