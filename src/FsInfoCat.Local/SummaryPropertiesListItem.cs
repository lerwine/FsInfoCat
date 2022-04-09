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

        public override bool Equals(ISummaryPropertiesRow other)
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
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 103;
                    hash = hash * 109 + ApplicationName.GetHashCode();
                    hash = hash * 109 + Author.GetHashCode();
                    hash = hash * 109 + Comment.GetHashCode();
                    hash = EntityExtensions.HashObject(Keywords, hash, 109);
                    hash = hash * 109 + Subject.GetHashCode();
                    hash = hash * 109 + Title.GetHashCode();
                    hash = hash * 109 + Company.GetHashCode();
                    hash = hash * 109 + ContentType.GetHashCode();
                    hash = hash * 109 + Copyright.GetHashCode();
                    hash = hash * 109 + ParentalRating.GetHashCode();
                    hash = EntityExtensions.HashNullable(Rating, hash, 109);
                    hash = EntityExtensions.HashObject(ItemAuthors, hash, 109);
                    hash = hash * 109 + ItemType.GetHashCode();
                    hash = hash * 109 + ItemTypeText.GetHashCode();
                    hash = EntityExtensions.HashObject(Kind, hash, 109);
                    hash = hash * 109 + MIMEType.GetHashCode();
                    hash = hash * 109 + ParentalRatingReason.GetHashCode();
                    hash = hash * 109 + ParentalRatingsOrganization.GetHashCode();
                    hash = EntityExtensions.HashNullable(Sensitivity, hash, 109);
                    hash = hash * 109 + SensitivityText.GetHashCode();
                    hash = EntityExtensions.HashNullable(SimpleRating, hash, 109);
                    hash = hash * 109 + Trademarks.GetHashCode();
                    hash = hash * 109 + ProductName.GetHashCode();
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 109);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 109);
                    hash = hash * 109 + CreatedOn.GetHashCode();
                    hash = hash * 109 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
