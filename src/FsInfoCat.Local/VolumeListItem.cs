using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class VolumeListItem : VolumeRow, ILocalVolumeListItem, IEquatable<VolumeListItem>
    {
        public const string VIEW_NAME = "vVolumeListing";

        private string _rootPath = string.Empty;

        public string RootPath { get => _rootPath; set => _rootPath = value ?? ""; }

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        public long RootSubdirectoryCount { get; set; }

        public long RootFileCount { get; set; }

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<VolumeListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME).Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);

        protected virtual bool ArePropertiesEqual([DisallowNull] VolumeListItem other)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(VolumeListItem other)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(IVolumeListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 41;
                    hash = hash * 47 + FileSystemId.GetHashCode();
                    hash = hash * 47 + RootPath.GetHashCode();
                    hash = hash * 47 + DisplayName.GetHashCode();
                    hash = hash * 47 + VolumeName.GetHashCode();
                    hash = hash * 47 + Identifier.GetHashCode();
                    hash = hash * 47 + Status.GetHashCode();
                    hash = hash * 47 + Type.GetHashCode();
                    hash = MaxNameLength.HasValue ? hash * 47 + (MaxNameLength ?? default).GetHashCode() : hash * 47;
                    hash = hash * 47 + Notes.GetHashCode();
                    hash = UpstreamId.HasValue ? hash * 47 + (UpstreamId ?? default).GetHashCode() : hash * 47;
                    hash = LastSynchronizedOn.HasValue ? hash * 47 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 47;
                    hash = hash * 47 + CreatedOn.GetHashCode();
                    hash = hash * 47 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
