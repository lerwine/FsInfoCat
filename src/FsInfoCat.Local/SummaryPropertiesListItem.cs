using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class SummaryPropertiesListItem : SummaryPropertiesRow, ILocalSummaryPropertiesListItem, IEquatable<SummaryPropertiesListItem>
    {
        public const string VIEW_NAME = "vSummaryPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<SummaryPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Author)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Keywords)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(ItemAuthors)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Kind)).HasConversion(MultiStringValue.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalSummaryPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ISummaryPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(SummaryPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(ISummaryPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(ISummaryProperties other)
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
                    int hash = 103;
                    hash = hash * 109 + ApplicationName.GetHashCode();
                    hash = hash * 109 + Author.GetHashCode();
                    hash = hash * 109 + Comment.GetHashCode();
                    hash = (Keywords is null) ? hash * 109 : hash * 109 + (Keywords?.GetHashCode() ?? 0);
                    hash = hash * 109 + Subject.GetHashCode();
                    hash = hash * 109 + Title.GetHashCode();
                    hash = hash * 109 + Company.GetHashCode();
                    hash = hash * 109 + ContentType.GetHashCode();
                    hash = hash * 109 + Copyright.GetHashCode();
                    hash = hash * 109 + ParentalRating.GetHashCode();
                    hash = Rating.HasValue ? hash * 109 + (Rating ?? default).GetHashCode() : hash * 109;
                    hash = (ItemAuthors is null) ? hash * 109 : hash * 109 + (ItemAuthors?.GetHashCode() ?? 0);
                    hash = hash * 109 + ItemType.GetHashCode();
                    hash = hash * 109 + ItemTypeText.GetHashCode();
                    hash = (Kind is null) ? hash * 109 : hash * 109 + (Kind?.GetHashCode() ?? 0);
                    hash = hash * 109 + MIMEType.GetHashCode();
                    hash = hash * 109 + ParentalRatingReason.GetHashCode();
                    hash = hash * 109 + ParentalRatingsOrganization.GetHashCode();
                    hash = Sensitivity.HasValue ? hash * 109 + (Sensitivity ?? default).GetHashCode() : hash * 109;
                    hash = hash * 109 + SensitivityText.GetHashCode();
                    hash = SimpleRating.HasValue ? hash * 109 + (SimpleRating ?? default).GetHashCode() : hash * 109;
                    hash = hash * 109 + Trademarks.GetHashCode();
                    hash = hash * 109 + ProductName.GetHashCode();
                    hash = UpstreamId.HasValue ? hash * 109 + (UpstreamId ?? default).GetHashCode() : hash * 109;
                    hash = LastSynchronizedOn.HasValue ? hash * 109 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 109;
                    hash = hash * 109 + CreatedOn.GetHashCode();
                    hash = hash * 109 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
