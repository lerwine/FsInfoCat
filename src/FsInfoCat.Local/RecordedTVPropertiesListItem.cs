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
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 41;
                    hash = ChannelNumber.HasValue ? hash * 47 + (ChannelNumber ?? default).GetHashCode() : hash * 47;
                    hash = hash * 47 + EpisodeName.GetHashCode();
                    hash = IsDTVContent.HasValue ? hash * 47 + (IsDTVContent ?? default).GetHashCode() : hash * 47;
                    hash = IsHDContent.HasValue ? hash * 47 + (IsHDContent ?? default).GetHashCode() : hash * 47;
                    hash = hash * 47 + NetworkAffiliation.GetHashCode();
                    hash = OriginalBroadcastDate.HasValue ? hash * 47 + (OriginalBroadcastDate ?? default).GetHashCode() : hash * 47;
                    hash = hash * 47 + ProgramDescription.GetHashCode();
                    hash = hash * 47 + StationCallSign.GetHashCode();
                    hash = hash * 47 + StationName.GetHashCode();
                    hash = UpstreamId.HasValue ? hash * 47 + (UpstreamId ?? default).GetHashCode() : hash * 47;
                    hash = LastSynchronizedOn.HasValue ? hash * 47 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 47;
                    hash = hash * 47 + CreatedOn.GetHashCode();
                    hash = hash * 47 + ModifiedOn.GetHashCode();
                    return hash;
                }
        }
    }
}
