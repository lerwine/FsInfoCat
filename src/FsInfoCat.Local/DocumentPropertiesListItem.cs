using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class DocumentPropertiesListItem : DocumentPropertiesRow, ILocalDocumentPropertiesListItem, IEquatable<DocumentPropertiesListItem>
    {
        public const string VIEW_NAME = "vDocumentPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<DocumentPropertiesListItem> builder)
        {
            _ = (builder ?? throw new ArgumentOutOfRangeException(nameof(builder))).ToView(VIEW_NAME).Property(nameof(Contributor))
                .HasConversion(MultiStringValue.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalDocumentPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IDocumentPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(DocumentPropertiesListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IDocumentPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IDocumentProperties other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 47;
                    hash = hash * 59 + ClientID.GetHashCode();
                    hash = EntityExtensions.HashObject(Contributor, hash, 59);
                    hash = EntityExtensions.HashNullable(DateCreated, hash, 59);
                    hash = hash * 59 + LastAuthor.GetHashCode();
                    hash = hash * 59 + RevisionNumber.GetHashCode();
                    hash = EntityExtensions.HashNullable(Security, hash, 59);
                    hash = hash * 59 + Division.GetHashCode();
                    hash = hash * 59 + DocumentID.GetHashCode();
                    hash = hash * 59 + Manager.GetHashCode();
                    hash = hash * 59 + PresentationFormat.GetHashCode();
                    hash = hash * 59 + Version.GetHashCode();
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 59);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 59);
                    hash = hash * 59 + CreatedOn.GetHashCode();
                    hash = hash * 59 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
