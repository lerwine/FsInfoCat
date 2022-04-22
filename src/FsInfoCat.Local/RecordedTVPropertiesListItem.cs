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

        public override bool Equals(IRecordedTVPropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IRecordedTVProperties other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            if (TryGetId(out Guid id)) return id.GetHashCode();
            // TODO: Implement GetHashCode()
            throw new NotImplementedException();
        }
    }
}
