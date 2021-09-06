using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class DocumentPropertiesListItem : DocumentPropertiesRow, ILocalDocumentPropertiesListItem
    {
        public const string VIEW_NAME = "vDocumentPropertiesListing";

        private readonly IPropertyChangeTracker<long> _existingFileCount;
        private readonly IPropertyChangeTracker<long> _totalFileCount;

        public long ExistingFileCount { get => _existingFileCount.GetValue(); set => _existingFileCount.SetValue(value); }

        public long TotalFileCount { get => _totalFileCount.GetValue(); set => _totalFileCount.SetValue(value); }

        public DocumentPropertiesListItem()
        {
            _existingFileCount = AddChangeTracker(nameof(ExistingFileCount), 0L);
            _totalFileCount = AddChangeTracker(nameof(TotalFileCount), 0L);
        }

        internal static void OnBuildEntity(EntityTypeBuilder<DocumentPropertiesListItem> builder)
        {
            (builder ?? throw new ArgumentOutOfRangeException(nameof(builder))).ToView(VIEW_NAME).Property(nameof(Contributor))
                .HasConversion(MultiStringValue.Converter);
        }
    }
}
