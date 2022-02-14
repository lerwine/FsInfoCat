using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class VolumeListItemWithFileSystem : VolumeListItem, ILocalVolumeListItemWithFileSystem, IEquatable<VolumeListItemWithFileSystem>
    {
        public const string VIEW_NAME_WITH_FILESYSTEM = "vVolumeListingWithFileSystem";

        private string _fileSystemDisplayName = string.Empty;

        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        public bool EffectiveReadOnly { get; set; }

        public uint EffectiveMaxNameLength { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<VolumeListItemWithFileSystem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME_WITH_FILESYSTEM).Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);

        protected virtual bool ArePropertiesEqual([DisallowNull] VolumeListItemWithFileSystem other)
        {
            throw new NotImplementedException();
        }

        protected override bool ArePropertiesEqual([DisallowNull] VolumeListItem other)
        {
            return base.ArePropertiesEqual(other);
        }

        public bool Equals(VolumeListItemWithFileSystem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(VolumeListItem other)
        {
            return base.Equals(other);
        }

        public bool Equals(IVolumeListItemWithFileSystem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IVolumeListItem other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
