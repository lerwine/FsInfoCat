using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class RedundantSetListItem : RedundantSetRow, ILocalRedundantSetListItem, IEquatable<RedundantSetListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vRedundantSetListing";

        public long Length { get; set; }

        public MD5Hash? Hash { get; set; }

        public long RedundancyCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<RedundantSetListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalRedundantSetListItem other) => ArePropertiesEqual((ILocalRedundantSetRow)other) && Length == other.Length && EqualityComparer<MD5Hash?>.Default.Equals(Hash, other.Hash);

        protected bool ArePropertiesEqual([DisallowNull] IRedundantSetListItem other) => ArePropertiesEqual((IRedundantSetRow)other) && Length == other.Length && EqualityComparer<MD5Hash?>.Default.Equals(Hash, other.Hash);

        public bool Equals(RedundantSetListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IRedundantSetListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
