using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class RedundantSetListItem : RedundantSetRow, ILocalRedundantSetListItem, IEquatable<RedundantSetListItem>
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalRedundantSetListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IRedundantSetListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(RedundantSetListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IRedundantSetListItem other)
        {
            throw new System.NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
