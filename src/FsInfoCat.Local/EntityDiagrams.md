# Entity Models

## Inheritance Diagram

```mermaid
- classDiagram
    class DbEntity
    class FileAccessError
    FileAccessError --|> DbEntity
    
    class SubdirectoryAccessError
    SubdirectoryAccessError --|> DbEntity
    
    class VolumeAccessError
    VolumeAccessError --|> DbEntity
    
    class LocalDbEntity
    LocalDbEntity --|> DbEntity

    class PropertiesRow
    PropertiesRow --|> LocalDbEntity

    class AudioPropertiesRow
    AudioPropertiesRow --|> PropertiesRow

    class AudioPropertiesListItem
    AudioPropertiesListItem --|> AudioPropertiesRow

    class AudioPropertySet
    AudioPropertySet --|> AudioPropertiesRow
    
    class DocumentPropertiesRow
    DocumentPropertiesRow --|> PropertiesRow
    
    class DocumentPropertiesListItem
    DocumentPropertiesListItem --|> DocumentPropertiesRow
    
    class DocumentPropertySet
    DocumentPropertySet --|> DocumentPropertiesRow
    
    class DRMPropertiesRow
    DRMPropertiesRow --|> PropertiesRow
    
    class DRMPropertiesListItem
    DRMPropertiesListItem --|> DRMPropertiesRow
    
    class DRMPropertySet
    DRMPropertySet --|> DRMPropertiesRow

    class GPSPropertiesRow
    GPSPropertiesRow --|> PropertiesRow
    
    class GPSPropertiesListItem
    GPSPropertiesListItem --|> GPSPropertiesRow
    
    class GPSPropertySet
    GPSPropertySet --|> GPSPropertiesRow
    
    class ImagePropertiesRow
    ImagePropertiesRow --|> PropertiesRow
    
    class ImagePropertiesListItem
    ImagePropertiesListItem --|> ImagePropertiesRow
    
    class ImagePropertySet
    ImagePropertySet --|> ImagePropertiesRow
    
    class MediaPropertiesRow
    MediaPropertiesRow --|> PropertiesRow
    
    class MediaPropertiesListItem
    MediaPropertiesListItem --|> MediaPropertiesRow
    
    class MediaPropertySet
    MediaPropertySet --|> MediaPropertiesRow
    
    class MusicPropertiesRow
    MusicPropertiesRow --|> PropertiesRow
    
    class MusicPropertiesListItem
    MusicPropertiesListItem --|> MusicPropertiesRow
    
    class MusicPropertySet
    MusicPropertySet --|> MusicPropertiesRow
    
    class PhotoPropertiesRow
    PhotoPropertiesRow --|> PropertiesRow
    
    class PhotoPropertiesListItem
    PhotoPropertiesListItem --|> PhotoPropertiesRow
    
    class PhotoPropertySet
    PhotoPropertySet --|> PhotoPropertiesRow
    
    class RecordedTVPropertiesRow
    RecordedTVPropertiesRow --|> PropertiesRow
    
    class RecordedTVPropertiesListItem
    RecordedTVPropertiesListItem --|> RecordedTVPropertiesRow
    
    class RecordedTVPropertySet
    RecordedTVPropertySet --|> RecordedTVPropertiesRow
    
    class SummaryPropertiesRow
    SummaryPropertiesRow --|> PropertiesRow
    
    class SummaryPropertiesListItem
    SummaryPropertiesListItem --|> SummaryPropertiesRow
    
    class SummaryPropertySet
    SummaryPropertySet --|> SummaryPropertiesRow
    
    class VideoPropertiesRow
    VideoPropertiesRow --|> PropertiesRow
    
    class VideoPropertiesListItem
    VideoPropertiesListItem --|> VideoPropertiesRow
    
    class VideoPropertySet
    VideoPropertySet --|> VideoPropertiesRow
    
    class BinaryPropertySet
    BinaryPropertySet --|> LocalDbEntity
    
    class CrawlConfigurationRow
    CrawlConfigurationRow --|> LocalDbEntity
    
    class CrawlConfigListItemBase
    CrawlConfigListItemBase --|> CrawlConfigurationRow
    
    class CrawlConfigListItem
    CrawlConfigListItem --|> CrawlConfigListItemBase
    
    class CrawlConfigReportItem
    CrawlConfigReportItem --|> CrawlConfigListItemBase
    
    class CrawlConfiguration
    CrawlConfiguration --|> CrawlConfigurationRow
    
    class CrawlJobLogRow
    CrawlJobLogRow --|> LocalDbEntity
    
    class CrawlJobLogListItem
    CrawlJobLogListItem --|> CrawlJobLogRow
    
    class CrawlJobLog
    CrawlJobLog --|> CrawlJobLogRow
    
    class DbFileRow
    DbFileRow --|> LocalDbEntity
    
    class FileWithAncestorNames
    FileWithAncestorNames --|> DbFileRow
    
    class FileWithBinaryProperties
    FileWithBinaryProperties --|> DbFileRow
    
    class FileWithBinaryPropertiesAndAncestorNames
    FileWithBinaryPropertiesAndAncestorNames --|> FileWithBinaryProperties
    
    class DbFile
    DbFile --|> DbFileRow
    
    class FileComparison
    FileComparison --|> LocalDbEntity
    
    class FileSystemRow
    FileSystemRow --|> LocalDbEntity
    
    class FileSystemListItem
    FileSystemListItem --|> FileSystemRow
    
    class FileSystem
    FileSystem --|> FileSystemRow
    
    class ItemTagRow
    ItemTagRow --|> LocalDbEntity
    
    class ItemTagListItem
    ItemTagListItem --|> ItemTagRow
    
    class PersonalFileTagListItem
    PersonalFileTagListItem --|> ItemTagListItem
    
    class PersonalSubdirectoryTagListItem
    PersonalSubdirectoryTagListItem --|> ItemTagListItem
    
    class PersonalVolumeTagListItem
    PersonalVolumeTagListItem --|> ItemTagListItem
    
    class SharedFileTagListItem
    SharedFileTagListItem --|> ItemTagListItem
    
    class SharedSubdirectoryTagListItem
    SharedSubdirectoryTagListItem --|> ItemTagListItem
    
    class SharedVolumeTagListItem
    SharedVolumeTagListItem --|> ItemTagListItem
    
    class ItemTag
    ItemTag --|> ItemTagRow
    
    class PersonalFileTag
    PersonalFileTag --|> ItemTag
    
    class PersonalSubdirectoryTag
    PersonalSubdirectoryTag --|> ItemTag
    
    class PersonalVolumeTag
    PersonalVolumeTag --|> ItemTag
    
    class SharedFileTag
    SharedFileTag --|> ItemTag
    
    class SharedSubdirectoryTag
    SharedSubdirectoryTag --|> ItemTag
    
    class SharedVolumeTag
    SharedVolumeTag --|> ItemTag
    
    class PersonalTagDefinitionRow
    PersonalTagDefinitionRow --|> LocalDbEntity
    
    class PersonalTagDefinitionListItem
    PersonalTagDefinitionListItem --|> PersonalTagDefinitionRow
    
    class PersonalTagDefinition
    PersonalTagDefinition --|> PersonalTagDefinitionRow
    
    class Redundancy
    Redundancy --|> LocalDbEntity
    
    class RedundantSetRow
    RedundantSetRow --|> LocalDbEntity
    
    class RedundantSetListItem
    RedundantSetListItem --|> RedundantSetRow
    
    class RedundantSet
    RedundantSet --|> RedundantSetRow
    
    class SharedTagDefinitionRow
    SharedTagDefinitionRow --|> LocalDbEntity
    
    class SharedTagDefinitionListItem
    SharedTagDefinitionListItem --|> SharedTagDefinitionRow
    
    class SharedTagDefinition
    SharedTagDefinition --|> SharedTagDefinitionRow
    
    class SubdirectoryRow
    SubdirectoryRow --|> LocalDbEntity
    
    class SubdirectoryListItem
    SubdirectoryListItem --|> SubdirectoryRow
    
    class SubdirectoryListItemWithAncestorNames
    SubdirectoryListItemWithAncestorNames --|> SubdirectoryListItem
    
    class Subdirectory
    Subdirectory --|> SubdirectoryRow
    
    class SubdirectoryAncestorNames
    
    class SymbolicNameRow
    SymbolicNameRow --|> LocalDbEntity
    
    class SymbolicNameListItem
    SymbolicNameListItem --|> SymbolicNameRow
    
    class SymbolicName
    SymbolicName --|> SymbolicNameRow
    
    class VolumeRow
    VolumeRow --|> LocalDbEntity
    
    class VolumeListItem
    VolumeListItem --|> VolumeRow
    
    class VolumeListItemWithFileSystem
    VolumeListItemWithFileSystem --|> VolumeListItem
    
    class Volume
    Volume --|> VolumeRow
```

- AudioPropertySet.Files => DbFile
- BinaryPropertySet.Files => DbFile
- BinaryPropertySet.RedundantSets => RedundantSet
- CrawlConfiguration.Root -> Subdirectory
- CrawlConfiguration.Logs => CrawlJobLog
- CrawlJobLog.Configuration -> CrawlConfiguration
- DbFile.Parent -> Subdirectory
- DbFile.BinaryProperties -> BinaryPropertySet
- DbFile.SummaryProperties -> SummaryPropertySet
- DbFile.DocumentProperties -> DocumentPropertySet
- DbFile.AudioProperties -> AudioPropertySet
- DbFile.DRMProperties -> DRMPropertySet
- DbFile.GPSProperties -> GPSPropertySet
- DbFile.ImageProperties -> ImagePropertySet
- DbFile.MediaProperties -> MediaPropertySet
- DbFile.MusicProperties -> MusicPropertySet
- DbFile.PhotoProperties -> PhotoPropertySet
- DbFile.RecordedTVProperties -> RecordedTVPropertySet
- DbFile.VideoProperties -> VideoPropertySet
- DbFile.Redundancy -> Redundancy
- DbFile.AccessErrors => FileAccessError
- DbFile.BaselineComparisons => FileComparison
- DbFile.CorrelativeComparisons => FileComparison
- DbFile.PersonalTags => PersonalFileTag
- DbFile.SharedTags => SharedFileTag
- DocumentPropertySet.Files => DbFile
- DRMPropertySet.Files => DbFile
- FileAccessError.Target -> DbFile
- FileComparison.Baseline -> DbFile
- FileComparison.Correlative -> DbFile
- FileSystem.Volumes => Volume
- FileSystem.SymbolicNames => SymbolicName
- GPSPropertySet.Files => DbFile
- ImagePropertySet.Files => DbFile
- MediaPropertySet.Files => DbFile
- MusicPropertySet.Files => DbFile
- PersonalFileTag.Tagged -> DbFile
- PersonalFileTag.Definition -> PersonalTagDefinition
- PersonalSubdirectoryTag.Tagged -> Subdirectory
- PersonalSubdirectoryTag.Definition -> PersonalTagDefinition
- PersonalTagDefinition.FileTags => PersonalFileTag
- PersonalTagDefinition.SubdirectoryTags => PersonalSubdirectoryTag
- PersonalTagDefinition.VolumeTags => PersonalVolumeTag
- PersonalVolumeTag.Tagged -> Volume
- PersonalVolumeTag.Definition -> PersonalTagDefinition
- PhotoPropertySet.Files => DbFile
- RecordedTVPropertySet.Files => DbFile
- Redundancy.File -> DbFile
- Redundancy.RedundantSet -> RedundantSet
- RedundantSet.BinaryProperties -> BinaryPropertySet
- RedundantSet.Redundancies => Redundancy
- SharedFileTag.Tagged -> DbFile
- SharedFileTag.Definition -> SharedTagDefinition
- SharedSubdirectoryTag.Tagged -> Subdirectory
- SharedSubdirectoryTag.Definition -> SharedTagDefinition
- SharedTagDefinition.FileTags => SharedFileTag
- SharedTagDefinition.SubdirectoryTags => SharedSubdirectoryTag
- SharedTagDefinition.VolumeTags => SharedVolumeTag
- SharedVolumeTag.Tagged -> Volume
- SharedVolumeTag.Definition -> SharedTagDefinition
- Subdirectory.Parent -> Subdirectory
- Subdirectory.Volume -> Volume
- Subdirectory.CrawlConfiguration -> CrawlConfiguration
- Subdirectory.Files => DbFile
- Subdirectory.SubDirectories => Subdirectory
- Subdirectory.AccessErrors => SubdirectoryAccessError
- Subdirectory.PersonalTags => PersonalSubdirectoryTag
- Subdirectory.SharedTags => SharedSubdirectoryTag
- SubdirectoryAccessError.Target -> Subdirectory
- Volume.RootDirectory -> Subdirectory
- Volume.AccessErrors => VolumeAccessError
- Volume.PersonalTags => PersonalVolumeTag
- Volume.SharedTags => SharedVolumeTag
- VolumeAccessError.Target -> Volume

- LocalDbEntity.FileSystems -> FileSystem
- LocalDbEntity.SymbolicNames -> SymbolicName
- LocalDbEntity.Volumes -> Volume
- LocalDbEntity.VolumeAccessErrors -> VolumeAccessError
- LocalDbEntity.Subdirectories -> Subdirectory
- LocalDbEntity.SubdirectoryAccessErrors -> SubdirectoryAccessError
- LocalDbEntity.Files -> DbFile
- LocalDbEntity.FileAccessErrors -> FileAccessError
- LocalDbEntity.SummaryPropertySets -> SummaryPropertySet
- LocalDbEntity.DocumentPropertySets -> DocumentPropertySet
- LocalDbEntity.AudioPropertySets -> AudioPropertySet
- LocalDbEntity.DRMPropertySets -> DRMPropertySet
- LocalDbEntity.GPSPropertySets -> GPSPropertySet
- LocalDbEntity.ImagePropertySets -> ImagePropertySet
- LocalDbEntity.MediaPropertySets -> MediaPropertySet
- LocalDbEntity.MusicPropertySets -> MusicPropertySet
- LocalDbEntity.PhotoPropertySets -> PhotoPropertySet
- LocalDbEntity.RecordedTVPropertySets -> RecordedTVPropertySet
- LocalDbEntity.VideoPropertySets -> VideoPropertySet
- LocalDbEntity.BinaryPropertySets -> BinaryPropertySet
- LocalDbEntity.PersonalTagDefinitions -> PersonalTagDefinition
- LocalDbEntity.PersonalFileTags -> PersonalFileTag
- LocalDbEntity.PersonalSubdirectoryTags -> PersonalSubdirectoryTag
- LocalDbEntity.PersonalVolumeTags -> PersonalVolumeTag
- LocalDbEntity.SharedTagDefinitions -> SharedTagDefinition
- LocalDbEntity.SharedFileTags -> SharedFileTag
- LocalDbEntity.SharedSubdirectoryTags -> SharedSubdirectoryTag
- LocalDbEntity.SharedVolumeTags -> SharedVolumeTag
- LocalDbEntity.Comparisons -> FileComparison
- LocalDbEntity.RedundantSets -> RedundantSet
- LocalDbEntity.Redundancies -> Redundancy
- LocalDbEntity.CrawlConfigurations -> CrawlConfiguration
- LocalDbEntity.CrawlJobLogs -> CrawlJobLog
- LocalDbEntity.SymbolicNameListing -> SymbolicNameListItem
- LocalDbEntity.FileSystemListing -> FileSystemListItem
- LocalDbEntity.PersonalTagDefinitionListing -> PersonalTagDefinitionListItem
- LocalDbEntity.SharedTagDefinitionListing -> SharedTagDefinitionListItem
- LocalDbEntity.RedundantSetListing -> RedundantSetListItem
- LocalDbEntity.VolumeListing -> VolumeListItem
- LocalDbEntity.VolumeListingWithFileSystem -> VolumeListItemWithFileSystem
- LocalDbEntity.SubdirectoryListing -> SubdirectoryListItem
- LocalDbEntity.SubdirectoryListingWithAncestorNames -> SubdirectoryListItemWithAncestorNames
- LocalDbEntity.SubdirectoryAncestorNames -> SubdirectoryAncestorNames
- LocalDbEntity.FileListingWithAncestorNames -> FileWithAncestorNames
- LocalDbEntity.FileListingWithBinaryProperties -> FileWithBinaryProperties
- LocalDbEntity.FileListingWithBinaryPropertiesAndAncestorNames -> FileWithBinaryPropertiesAndAncestorNames
- LocalDbEntity.CrawlConfigListing -> CrawlConfigListItem
- LocalDbEntity.CrawlConfigReport -> CrawlConfigReportItem
- LocalDbEntity.CrawlJobListing -> CrawlJobLogListItem
- LocalDbEntity.SummaryPropertiesListing -> SummaryPropertiesListItem
- LocalDbEntity.DocumentPropertiesListing -> DocumentPropertiesListItem
- LocalDbEntity.AudioPropertiesListing -> AudioPropertiesListItem
- LocalDbEntity.DRMPropertiesListing -> DRMPropertiesListItem
- LocalDbEntity.GPSPropertiesListing -> GPSPropertiesListItem
- LocalDbEntity.ImagePropertiesListing -> ImagePropertiesListItem
- LocalDbEntity.MediaPropertiesListing -> MediaPropertiesListItem
- LocalDbEntity.MusicPropertiesListing -> MusicPropertiesListItem
- LocalDbEntity.PhotoPropertiesListing -> PhotoPropertiesListItem
- LocalDbEntity.RecordedTVPropertiesListing -> RecordedTVPropertiesListItem
- LocalDbEntity.VideoPropertiesListing -> VideoPropertiesListItem
- LocalDbEntity.PersonalVolumeTagListing -> PersonalVolumeTagListItem
- LocalDbEntity.SharedVolumeTagListing -> SharedVolumeTagListItem
- LocalDbEntity.PersonalSubdirectoryTagListing -> PersonalSubdirectoryTagListItem
- LocalDbEntity.SharedSubdirectoryTagListing -> SharedSubdirectoryTagListItem
- LocalDbEntity.PersonalFileTagListing -> PersonalFileTagListItem
- LocalDbEntity.SharedFileTagListing -> SharedFileTagListItem

- ________________________________________________________________________._
- Referenc.s

- [Class Diagrams in Mermaid](https://mermaid.js.org/syntax/classDiagram.html)
- [Entity Diagrams in Mermaid](https://mermaid.js.org/syntax/entityRelationshipDiagram.html)
