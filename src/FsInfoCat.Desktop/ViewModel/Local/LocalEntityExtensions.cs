using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public static class LocalEntityExtensions
    {
        public static CrawlConfigItemVM ToItemViewModel([AllowNull] this CrawlConfiguration entity, AsyncOps.AsyncBgModalVM bgOpMgr = null) =>
            (entity is null) ? null : new CrawlConfigItemVM(entity, bgOpMgr);

        public static SharedTagDefinitionItemVM ToItemViewModel([AllowNull] this SharedTagDefinition entity) => (entity is null) ? null : new SharedTagDefinitionItemVM(entity);

        public static RedundantSetItemVM ToItemViewModel([AllowNull] this RedundantSet entity) => (entity is null) ? null : new RedundantSetItemVM(entity);

        public static RedundancyItemVM ToItemViewModel([AllowNull] this Redundancy entity) => (entity is null) ? null : new RedundancyItemVM(entity);

        public static PersonalTagDefinitionItemVM ToItemViewModel([AllowNull] this PersonalTagDefinition entity) => (entity is null) ? null : new PersonalTagDefinitionItemVM(entity);

        public static BinaryPropertySetItemVM ToItemViewModel([AllowNull] this BinaryPropertySet entity) => (entity is null) ? null : new BinaryPropertySetItemVM(entity);

        public static FileSystemItemVM ToItemViewModel([AllowNull] this FileSystem entity) => (entity is null) ? null : new FileSystemItemVM(entity);

        public static SymbolicNameItemVM ToItemViewModel([AllowNull] this SymbolicName entity) => (entity is null) ? null : new SymbolicNameItemVM(entity);

        public static VolumeItemVM ToItemViewModel([AllowNull] this Volume entity, AsyncOps.AsyncBgModalVM bgOpMgr = null) => (entity is null) ? null : new VolumeItemVM(entity, bgOpMgr);

        public static FileItemVM ToItemViewModel([AllowNull] this DbFile entity) => (entity is null) ? null : new FileItemVM(entity);

        public static SubdirectoryItemVM ToItemViewModel([AllowNull] this Subdirectory entity, AsyncOps.AsyncBgModalVM bgOpMgr = null) => (entity is null) ? null : new SubdirectoryItemVM(entity, bgOpMgr);

        public static AudioPropertiesItemVM ToItemViewModel([AllowNull] this AudioPropertySet entity) => (entity is null) ? null : new AudioPropertiesItemVM(entity);

        public static SummaryPropertiesItemVM ToItemViewModel([AllowNull] this SummaryPropertySet entity) => (entity is null) ? null : new SummaryPropertiesItemVM(entity);

        public static ImagePropertiesItemVM ToItemViewModel([AllowNull] this ImagePropertySet entity) => (entity is null) ? null : new ImagePropertiesItemVM(entity);

        public static MusicPropertiesItemVM ToItemViewModel([AllowNull] this MusicPropertySet entity) => (entity is null) ? null : new MusicPropertiesItemVM(entity);

        public static RecordedTVPropertiesItemVM ToItemViewModel([AllowNull] this RecordedTVPropertySet entity) => (entity is null) ? null : new RecordedTVPropertiesItemVM(entity);

        public static VideoPropertiesItemVM ToItemViewModel([AllowNull] this VideoPropertySet entity) => (entity is null) ? null : new VideoPropertiesItemVM(entity);

        public static PhotoPropertiesItemVM ToItemViewModel([AllowNull] this PhotoPropertySet entity) => (entity is null) ? null : new PhotoPropertiesItemVM(entity);

        public static MediaPropertiesItemVM ToItemViewModel([AllowNull] this MediaPropertySet entity) => (entity is null) ? null : new MediaPropertiesItemVM(entity);

        public static GPSPropertiesItemVM ToItemViewModel([AllowNull] this GPSPropertySet entity) => (entity is null) ? null : new GPSPropertiesItemVM(entity);

        public static DRMPropertiesItemVM ToItemViewModel([AllowNull] this DRMPropertySet entity) => (entity is null) ? null : new DRMPropertiesItemVM(entity);

        public static DocumentPropertiesItemVM ToItemViewModel([AllowNull] this DocumentPropertySet entity) => (entity is null) ? null : new DocumentPropertiesItemVM(entity);
    }
}
