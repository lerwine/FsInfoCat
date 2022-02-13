using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FsInfoCat.Local
{
    public class DocumentPropertiesListItem : DocumentPropertiesRow, ILocalDocumentPropertiesListItem
    {
        public const string VIEW_NAME = "vDocumentPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<DocumentPropertiesListItem> builder)
        {
            _ = (builder ?? throw new ArgumentOutOfRangeException(nameof(builder))).ToView(VIEW_NAME).Property(nameof(Contributor))
                .HasConversion(MultiStringValue.Converter);
        }
    }
}
