using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class FileWithBinaryProperties : DbFileRow, ILocalFileListItemWithBinaryProperties
    {
        private const string VIEW_NAME = "vFileListingWithBinaryProperties";

        public long Length { get; set; }

        public MD5Hash? Hash { get; set; }

        public long RedundancyCount { get; set; }

        public long ComparisonCount { get; set; }

        public long AccessErrorCount { get; set; }

        public long PersonalTagCount { get; set; }

        public long SharedTagCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<FileWithBinaryProperties> builder)
        {
            _ = builder.ToView(VIEW_NAME);
            _ = builder.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
        }
    }
}
