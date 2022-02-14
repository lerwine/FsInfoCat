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
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
