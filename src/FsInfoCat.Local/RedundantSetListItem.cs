using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class RedundantSetListItem : RedundantSetRow, ILocalRedundantSetListItem
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
    }
}
