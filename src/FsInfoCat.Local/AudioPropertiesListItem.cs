using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FsInfoCat.Local
{
    public class AudioPropertiesListItem : AudioPropertiesRow, ILocalAudioPropertiesListItem
    {
        public const string VIEW_NAME = "vAudioPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<AudioPropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));
    }
}
