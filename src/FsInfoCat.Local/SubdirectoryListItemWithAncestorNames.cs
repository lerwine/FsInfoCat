using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class SubdirectoryListItemWithAncestorNames : SubdirectoryListItem, ILocalSubdirectoryListItemWithAncestorNames, IEquatable<SubdirectoryListItemWithAncestorNames>
    {
        public const string VIEW_NAME_WITH_ANCESTOR_NAMES = "vSubdirectoryListingWithAncestorNames";

        private string _ancestorNames = string.Empty;
        private string _volumeDisplayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _fileSystemDisplayName = string.Empty;
        private string _fileSystemSymbolicName = string.Empty;

        [NotNull]
        public string AncestorNames { get => _ancestorNames; set => _ancestorNames = value.AsNonNullTrimmed(); }

        public Guid EffectiveVolumeId { get; set; }

        [NotNull]
        public string VolumeDisplayName { get => _volumeDisplayName; set => _volumeDisplayName = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string VolumeName { get => _volumeName; set => _volumeName = value.AsNonNullTrimmed(); }

        public VolumeIdentifier VolumeIdentifier { get; set; }

        [NotNull]
        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        [NotNull]
        public string FileSystemSymbolicName { get => _fileSystemSymbolicName; set => _fileSystemSymbolicName = value.AsNonNullTrimmed(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryListItemWithAncestorNames> builder)
        {
            _ = builder.ToView(VIEW_NAME_WITH_ANCESTOR_NAMES);
            _ = builder.Property(nameof(VolumeIdentifier)).HasConversion(VolumeIdentifier.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalSubdirectoryListItemWithAncestorNames other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ISubdirectoryListItemWithAncestorNames other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(SubdirectoryListItemWithAncestorNames other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(SubdirectoryListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISubdirectoryAncestorName other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISubdirectoryListItemWithAncestorNames other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(ISubdirectoryListItem other)
        {
            return base.Equals(other);
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
                    hash = hash * 59 + AncestorNames.GetHashCode();
                    hash = hash * 59 + CrawlConfigDisplayName.GetHashCode();
                    hash = (ParentId is null) ? hash * 59 : hash * 59 + (ParentId?.GetHashCode() ?? 0);
                    hash = (VolumeId is null) ? hash * 59 : hash * 59 + (VolumeId?.GetHashCode() ?? 0);
                    hash = hash * 59 + Name.GetHashCode();
                    hash = hash * 59 + Options.GetHashCode();
                    hash = hash * 59 + LastAccessed.GetHashCode();
                    hash = hash * 59 + Notes.GetHashCode();
                    hash = hash * 59 + Status.GetHashCode();
                    hash = hash * 59 + CreationTime.GetHashCode();
                    hash = hash * 59 + LastWriteTime.GetHashCode();
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
