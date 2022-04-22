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
                    int hash = 23;
                    hash = hash * 31 + Length.GetHashCode();
                    hash = Hash.HasValue ? hash * 31 + (Hash ?? default).GetHashCode() : hash * 31;
                    hash = hash * 31 + Reference.GetHashCode();
                    hash = hash * 31 + Status.GetHashCode();
                    hash = hash * 31 + Notes.GetHashCode();
                    hash = UpstreamId.HasValue ? hash * 31 + (UpstreamId ?? default).GetHashCode() : hash * 31;
                    hash = LastSynchronizedOn.HasValue ? hash * 31 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 31;
                    hash = hash * 31 + CreatedOn.GetHashCode();
                    hash = hash * 31 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
