using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class SubdirectoryListItem : SubdirectoryRow, ILocalSubdirectoryListItem, IEquatable<SubdirectoryListItem>
    {
        public const string VIEW_NAME = "vSubdirectoryListing";

        private string _crawlConfigDisplayName;

        public long SubdirectoryCount { get; set; }

        public long FileCount { get; set; }

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        [NotNull]
        public string CrawlConfigDisplayName { get => _crawlConfigDisplayName; set => _crawlConfigDisplayName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalSubdirectoryListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ISubdirectoryListItem other)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(SubdirectoryListItem other)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(ISubdirectoryListItem other)
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
                    int hash = 43;
                    hash = hash * 53 + CrawlConfigDisplayName.GetHashCode();
                    hash = (ParentId is null) ? hash * 53 : hash * 53 + (ParentId?.GetHashCode() ?? 0);
                    hash = (VolumeId is null) ? hash * 53 : hash * 53 + (VolumeId?.GetHashCode() ?? 0);
                    hash = hash * 53 + Name.GetHashCode();
                    hash = hash * 53 + Options.GetHashCode();
                    hash = hash * 53 + LastAccessed.GetHashCode();
                    hash = hash * 53 + Notes.GetHashCode();
                    hash = hash * 53 + Status.GetHashCode();
                    hash = hash * 53 + CreationTime.GetHashCode();
                    hash = hash * 53 + LastWriteTime.GetHashCode();
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 53);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 53);
                    hash = hash * 53 + CreatedOn.GetHashCode();
                    hash = hash * 53 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
