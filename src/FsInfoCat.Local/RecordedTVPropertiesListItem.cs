using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class RecordedTVPropertiesListItem : RecordedTVPropertiesRow, ILocalRecordedTVPropertiesListItem, IEquatable<RecordedTVPropertiesListItem>
    {
        public const string VIEW_NAME = "vRecordedTVPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<RecordedTVPropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalRecordedTVPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IRecordedTVPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(RecordedTVPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IRecordedTVPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IRecordedTVProperties other)
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
                    int hash = EntityExtensions.HashNullable(ChannelNumber, 41, 47);
                    hash = hash * 47 + EpisodeName.GetHashCode();
                    hash = EntityExtensions.HashNullable(IsDTVContent, hash, 47);
                    hash = EntityExtensions.HashNullable(IsHDContent, hash, 47);
                    hash = hash * 47 + NetworkAffiliation.GetHashCode();
                    hash = EntityExtensions.HashNullable(OriginalBroadcastDate, hash, 47);
                    hash = hash * 47 + ProgramDescription.GetHashCode();
                    hash = hash * 47 + StationCallSign.GetHashCode();
                    hash = hash * 47 + StationName.GetHashCode();
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 47);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 47);
                    hash = hash * 47 + CreatedOn.GetHashCode();
                    hash = hash * 47 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
