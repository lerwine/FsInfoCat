using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class VolumeListItem : VolumeRow, ILocalVolumeListItem, IEquatable<VolumeListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
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
    }
}
