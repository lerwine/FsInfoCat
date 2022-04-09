using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class VolumeListItemWithFileSystem : VolumeListItem, ILocalVolumeListItemWithFileSystem, IEquatable<VolumeListItemWithFileSystem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME_WITH_FILESYSTEM = "vVolumeListingWithFileSystem";

        private string _fileSystemDisplayName = string.Empty;

        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        public bool EffectiveReadOnly { get; set; }

        public uint EffectiveMaxNameLength { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<VolumeListItemWithFileSystem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME_WITH_FILESYSTEM).Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);

        protected virtual bool ArePropertiesEqual([DisallowNull] IVolumeListItemWithFileSystem other) => ArePropertiesEqual((IVolumeListItem)other) &&
            EffectiveReadOnly == other.EffectiveReadOnly &&
            EffectiveMaxNameLength == other.EffectiveMaxNameLength &&
            _fileSystemDisplayName == other.FileSystemDisplayName;

        public bool Equals(VolumeListItemWithFileSystem other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Id.Equals(Guid.Empty)) return other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other);
            return Id.Equals(other.Id);
        }

        public override bool Equals(VolumeListItem other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other is VolumeListItemWithFileSystem volumeListItemWithFileSystem) return Equals(volumeListItemWithFileSystem);
            if (Id.Equals(Guid.Empty)) return other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other);
            return Id.Equals(other.Id);
        }

        public bool Equals(IVolumeListItemWithFileSystem other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other is VolumeListItemWithFileSystem volumeListItemWithFileSystem) return Equals(volumeListItemWithFileSystem);
            if (Id.Equals(Guid.Empty)) return other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other);
            return Id.Equals(other.Id);
        }

        public override bool Equals(IVolumeListItem other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other is VolumeListItemWithFileSystem volumeListItemWithFileSystem) return Equals(volumeListItemWithFileSystem);
            if (other is VolumeListItem volumeListItem)
            {
                if (Id.Equals(Guid.Empty)) return volumeListItem.Id.Equals(Guid.Empty) && ArePropertiesEqual(volumeListItem);
                return Id.Equals(volumeListItem.Id);
            }
            if (other is IVolumeListItemWithFileSystem iVolumeListItemWithFileSystem)
            {
            }
            if (Id.Equals(Guid.Empty)) return other.Id.Equals(Guid.Empty) && ArePropertiesEqual(other);
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is VolumeListItemWithFileSystem volumeListItemWithFileSystem) return Equals(volumeListItemWithFileSystem);
            if (obj is VolumeListItem volumeListItem)
            {
                if (Id.Equals(Guid.Empty)) return volumeListItem.Id.Equals(Guid.Empty) && ArePropertiesEqual(volumeListItem);
                return Id.Equals(volumeListItem.Id);
            }
            if (obj is IVolumeListItemWithFileSystem iVolumeListItemWithFileSystem)
            {
                if (Id.Equals(Guid.Empty)) return iVolumeListItemWithFileSystem.Id.Equals(Guid.Empty) && ArePropertiesEqual(iVolumeListItemWithFileSystem);
                return Id.Equals(iVolumeListItemWithFileSystem.Id);
            }
            if (obj is IVolumeListItem iVolumeListItem)
            {
                if (Id.Equals(Guid.Empty)) return iVolumeListItem.Id.Equals(Guid.Empty) && ArePropertiesEqual(iVolumeListItem);
                return Id.Equals(iVolumeListItem.Id);
            }
            if (obj is ILocalVolumeRow localVolumeRow)
            {
                if (Id.Equals(Guid.Empty)) return localVolumeRow.Id.Equals(Guid.Empty) && ArePropertiesEqual(localVolumeRow);
                return Id.Equals(localVolumeRow.Id);
            }
            if (obj is not IVolumeRow volumeRow) return false;
            if (Id.Equals(Guid.Empty)) return volumeRow.Id.Equals(Guid.Empty) && ArePropertiesEqual(volumeRow);
            return Id.Equals(volumeRow.Id);
        }
    }
}
