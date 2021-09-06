using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class SummaryPropertiesListItem : SummaryPropertiesRow, ILocalSummaryPropertiesListItem
    {
        public const string VIEW_NAME = "vSummaryPropertiesListing";
        private readonly IPropertyChangeTracker<long> _existingFileCount;
        private readonly IPropertyChangeTracker<long> _totalFileCount;

        public long ExistingFileCount { get => _existingFileCount.GetValue(); set => _existingFileCount.SetValue(value); }

        public long TotalFileCount { get => _totalFileCount.GetValue(); set => _totalFileCount.SetValue(value); }

        public SummaryPropertiesListItem()
        {
            _existingFileCount = AddChangeTracker(nameof(ExistingFileCount), 0L);
            _totalFileCount = AddChangeTracker(nameof(TotalFileCount), 0L);
        }

        internal static void OnBuildEntity(EntityTypeBuilder<SummaryPropertiesListItem> builder)
        {
            builder.ToView(VIEW_NAME);
            builder.Property(nameof(Author)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(Keywords)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(ItemAuthors)).HasConversion(MultiStringValue.Converter);
            builder.Property(nameof(Kind)).HasConversion(MultiStringValue.Converter);
        }
    }
}
