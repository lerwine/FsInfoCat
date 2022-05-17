using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    // TODO: Document VolumeListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class VolumeListItem : VolumeRow, ILocalVolumeListItem, IEquatable<VolumeListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vVolumeListing";

        private string _rootPath = string.Empty;

        [NotNull]
        [BackingField(nameof(_rootPath))]
        public string RootPath { get => _rootPath; set => _rootPath = value ?? ""; }

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        public long RootSubdirectoryCount { get; set; }

        public long RootFileCount { get; set; }

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<VolumeListItem> builder) => (builder ?? throw new ArgumentOutOfRangeException(nameof(builder)))
            .ToView(VIEW_NAME).Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalVolumeListItem other) => ArePropertiesEqual((ILocalVolumeRow)other) &&
                _rootPath == other.RootPath &&
                AccessErrorCount == other.AccessErrorCount &&
                PersonalTagCount == other.PersonalTagCount &&
                SharedTagCount == other.SharedTagCount &&
                RootSubdirectoryCount == other.RootSubdirectoryCount &&
                RootFileCount == other.RootFileCount;

        protected virtual bool ArePropertiesEqual([DisallowNull] IVolumeListItem other) => ArePropertiesEqual((IVolumeRow)other) &&
                _rootPath == other.RootPath &&
                AccessErrorCount == other.AccessErrorCount &&
                PersonalTagCount == other.PersonalTagCount &&
                SharedTagCount == other.SharedTagCount &&
                RootSubdirectoryCount == other.RootSubdirectoryCount &&
                RootFileCount == other.RootFileCount;

        public virtual bool Equals(VolumeListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public virtual bool Equals(IVolumeListItem other)
        {
            if (other is null) return false;
            if (other is VolumeListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalVolumeListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is VolumeListItem listItem) return Equals(listItem);
            if (obj is IVolumeListItem other)
            {
                if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
                return !other.TryGetId(out _) && (other is ILocalVolumeListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
            }
            return false;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
