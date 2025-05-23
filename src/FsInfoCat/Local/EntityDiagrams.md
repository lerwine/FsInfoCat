# Entity Diagrams

## Base Inheritance

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class IDbEntity

    class ILocalDbEntity
    ILocalDbEntity --|> IDbEntity

    class IAccessError

    class ILocalAccessError
    ILocalAccessError --|> ILocalDbEntity
    ILocalAccessError --|> IAccessError

    class IFileAccessError

    class ILocalFileAccessError
    ILocalFileAccessError --|> IFileAccessError
    ILocalFileAccessError --|> ILocalDbEntity
    ILocalFileAccessError --|> ILocalAccessError

    class ISubdirectoryAccessError

    class ILocalSubdirectoryAccessError
    ILocalSubdirectoryAccessError --|> ISubdirectoryAccessError
    ILocalSubdirectoryAccessError --|> ILocalDbEntity
    ILocalSubdirectoryAccessError --|> ILocalAccessError

    class IVolumeAccessError

    class ILocalVolumeAccessError
    ILocalVolumeAccessError --|> IVolumeAccessError
    ILocalVolumeAccessError --|> ILocalDbEntity
    ILocalVolumeAccessError --|> ILocalAccessError

    class ILocalBinaryPropertySet
    ILocalBinaryPropertySet --|> IBinaryPropertySet
    ILocalBinaryPropertySet --|> ILocalDbEntity

    class IBinaryPropertySet

    class IComparison

    class ILocalComparison
    ILocalComparison --|> IComparison
    ILocalComparison --|> ILocalDbEntity

    class ICrawlConfigurationRow

    class ICrawlSettings

    class ILocalCrawlConfigurationRow
    ILocalCrawlConfigurationRow --|> ILocalDbEntity
    ILocalCrawlConfigurationRow --|> ICrawlConfigurationRow
    ILocalCrawlConfigurationRow --|> ICrawlSettings

    class ICrawlConfigurationListItem

    class ILocalCrawlConfigurationListItem
    ILocalCrawlConfigurationListItem --|> ICrawlConfigurationListItem
    ILocalCrawlConfigurationListItem --|> ILocalCrawlConfigurationRow

    class ICrawlConfiguration

    class ILocalCrawlConfiguration
    ILocalCrawlConfiguration --|> ICrawlConfiguration
    ILocalCrawlConfiguration --|> ILocalCrawlConfigurationRow

    class ICrawlConfigReportItem

    class ILocalCrawlConfigReportItem
    ILocalCrawlConfigReportItem --|> ICrawlConfigReportItem
    ILocalCrawlConfigReportItem --|> ILocalCrawlConfigurationListItem

    class ICrawlJobLogRow

    class ILocalCrawlJobLogRow
    ILocalCrawlJobLogRow --|> ILocalDbEntity
    ILocalCrawlJobLogRow --|> ICrawlJobLogRow
    ILocalCrawlJobLogRow --|> ICrawlSettings

    class ICrawlJobListItem

    class ILocalCrawlJobListItem
    ILocalCrawlJobListItem --|> ICrawlJobListItem
    ILocalCrawlJobListItem --|> ILocalCrawlJobLogRow

    class ICrawlJobLog

    class ILocalCrawlJobLog
    ILocalCrawlJobLog --|> ICrawlJobLog
    ILocalCrawlJobLog --|> ILocalCrawlJobLogRow

    class IDbFsItemRow

    class ILocalDbFsItemRow
    ILocalDbFsItemRow --|> ILocalDbEntity
    ILocalDbFsItemRow --|> IDbFsItemRow

    class IDbFsItemListItem

    class ILocalDbFsItemListItem
    ILocalDbFsItemListItem --|> ILocalDbFsItemRow
    ILocalDbFsItemListItem --|> IDbFsItemListItem

    class IDbFsItemAncestorName

    class IDbFsItemListItemWithAncestorNames

    class ILocalDbFsItemListItemWithAncestorNames
    ILocalDbFsItemListItemWithAncestorNames --|> IDbFsItemAncestorName
    ILocalDbFsItemListItemWithAncestorNames --|> ILocalDbFsItemListItem
    ILocalDbFsItemListItemWithAncestorNames --|> IDbFsItemListItemWithAncestorNames

    class IDbFsItem

    class ILocalDbFsItem
    ILocalDbFsItem --|> ILocalDbFsItemRow
    ILocalDbFsItem --|> IDbFsItem

    class IFileRow

    class ILocalFileRow
    ILocalFileRow --|> ILocalDbFsItemRow
    ILocalFileRow  --|> IFileRow

    class IFileListItemWithAncestorNames

    class IFileAncestorName

    class ILocalFileListItemWithAncestorNames
    ILocalFileListItemWithAncestorNames --|> IFileListItemWithAncestorNames
    ILocalFileListItemWithAncestorNames --|> ILocalDbFsItemListItem
    ILocalFileListItemWithAncestorNames --|> ILocalDbFsItemListItemWithAncestorNames
    ILocalFileListItemWithAncestorNames  --|> ILocalFileRow
    ILocalFileListItemWithAncestorNames --|> IFileAncestorName

    class IFileListItemWithBinaryProperties

    class ILocalFileListItemWithBinaryProperties
    ILocalFileListItemWithBinaryProperties --|> IFileListItemWithBinaryProperties
    ILocalFileListItemWithBinaryProperties --|> ILocalDbFsItemListItem
    ILocalFileListItemWithBinaryProperties  --|> ILocalFileRow

    class IFileListItemWithBinaryPropertiesAndAncestorNames

    class ILocalFileListItemWithBinaryPropertiesAndAncestorNames
    ILocalFileListItemWithBinaryPropertiesAndAncestorNames --|> ILocalFileListItemWithAncestorNames
    ILocalFileListItemWithBinaryPropertiesAndAncestorNames --|> IFileListItemWithBinaryPropertiesAndAncestorNames

    class IFile

    class ILocalFile
    ILocalFile --|> IFile
    ILocalFile --|> ILocalDbFsItem
    ILocalFile  --|> ILocalFileRow

    class ISubdirectoryRow

    class ILocalSubdirectoryRow
    ILocalSubdirectoryRow --|> ILocalDbFsItemRow
    ILocalSubdirectoryRow --|> ISubdirectoryRow

    class ISubdirectoryListItem

    class ILocalSubdirectoryListItem
    ILocalSubdirectoryListItem --|> ISubdirectoryListItem
    ILocalSubdirectoryListItem --|> ILocalDbFsItemListItem
    ILocalSubdirectoryListItem --|> ILocalSubdirectoryRow

    class ISubdirectoryAncestorName

    class ISubdirectoryListItemWithAncestorNames

    class ILocalSubdirectoryListItemWithAncestorNames
    ILocalSubdirectoryListItemWithAncestorNames --|> ISubdirectoryAncestorName
    ILocalSubdirectoryListItemWithAncestorNames --|> ILocalSubdirectoryListItem
    ILocalSubdirectoryListItemWithAncestorNames --|> ISubdirectoryListItemWithAncestorNames
    ILocalSubdirectoryListItemWithAncestorNames --|> ILocalDbFsItemListItemWithAncestorNames

    class ISubdirectory

    class ILocalSubdirectory
    ILocalSubdirectory --|> ISubdirectory
    ILocalSubdirectory --|> ILocalDbFsItem
    ILocalSubdirectory --|> ILocalSubdirectoryRow

    class IFileSystemRow

    class IFileSystemProperties

    class ILocalFileSystemRow
    ILocalFileSystemRow --|> ILocalDbEntity
    ILocalFileSystemRow --|> IFileSystemRow
    ILocalFileSystemRow --|> IFileSystemProperties

    class IFileSystemListItem

    class ILocalFileSystemListItem
    ILocalFileSystemListItem --|> IFileSystemListItem
    ILocalFileSystemListItem --|> ILocalFileSystemRow

    class IFileSystem

    class ILocalFileSystem
    ILocalFileSystem --|> IFileSystem
    ILocalFileSystem --|> ILocalFileSystemRow

    class IItemTagRow

    class ILocalItemTagRow
    ILocalItemTagRow --|> ILocalDbEntity
    ILocalItemTagRow --|> IItemTagRow

    class IItemTagListItem

    class ILocalItemTagListItem
    ILocalItemTagListItem --|> IItemTagListItem
    ILocalItemTagListItem --|> ILocalItemTagRow

    class IItemTag

    class ILocalItemTag
    ILocalItemTag --|> ILocalItemTagRow
    ILocalItemTag --|> IItemTag

    class IFileTag

    class ILocalFileTag
    ILocalFileTag --|> ILocalItemTag
    ILocalFileTag --|> IFileTag

    class ISubdirectoryTag

    class ILocalSubdirectoryTag
    ILocalSubdirectoryTag --|> ILocalItemTag
    ILocalSubdirectoryTag --|> ISubdirectoryTag

    class IVolumeTag

    class ILocalVolumeTag
    ILocalVolumeTag --|> ILocalItemTag
    ILocalVolumeTag --|> IVolumeTag

    class IPersonalTag

    class ILocalPersonalTag
    ILocalPersonalTag --|> IPersonalTag
    ILocalPersonalTag --|> ILocalItemTag

    class IPersonalFileTag

    class ILocalPersonalFileTag
    ILocalPersonalFileTag --|> IPersonalFileTag
    ILocalPersonalFileTag --|> ILocalPersonalTag
    ILocalPersonalFileTag --|> ILocalFileTag

    class IPersonalSubdirectoryTag

    class ILocalPersonalSubdirectoryTag
    ILocalPersonalSubdirectoryTag --|> IPersonalSubdirectoryTag
    ILocalPersonalSubdirectoryTag --|> ILocalPersonalTag
    ILocalPersonalSubdirectoryTag --|> ILocalSubdirectoryTag

    class IPersonalVolumeTag

    class ILocalPersonalVolumeTag
    ILocalPersonalVolumeTag --|> IPersonalVolumeTag
    ILocalPersonalVolumeTag --|> ILocalPersonalTag
    ILocalPersonalVolumeTag --|> ILocalVolumeTag

    class ISharedTag

    class ILocalSharedTag
    ILocalSharedTag --|> ISharedTag
    ILocalSharedTag --|> ILocalItemTag

    class ISharedFileTag

    class ILocalSharedFileTag
    ILocalSharedFileTag --|> ISharedFileTag
    ILocalSharedFileTag --|> ILocalSharedTag
    ILocalSharedFileTag --|> ILocalFileTag

    class ISharedSubdirectoryTag

    class ILocalSharedSubdirectoryTag
    ILocalSharedSubdirectoryTag --|> ISharedSubdirectoryTag
    ILocalSharedSubdirectoryTag --|> ILocalSharedTag
    ILocalSharedSubdirectoryTag --|> ILocalSubdirectoryTag

    class ISharedVolumeTag

    class ILocalSharedVolumeTag
    ILocalSharedVolumeTag --|> ISharedVolumeTag
    ILocalSharedVolumeTag --|> ILocalSharedTag
    ILocalSharedVolumeTag --|> ILocalVolumeTag

    class IPropertiesRow

    class ILocalPropertiesRow
    ILocalPropertiesRow --|> ILocalDbEntity
    ILocalPropertiesRow --|> IPropertiesRow

    class IPropertiesListItem

    class ILocalPropertiesListItem
    ILocalPropertiesListItem --|> ILocalPropertiesRow
    ILocalPropertiesListItem --|> IPropertiesListItem

    class IPropertySet

    class ILocalPropertySet
    ILocalPropertySet --|> ILocalPropertiesRow
    ILocalPropertySet --|> IPropertySet

    class IAudioProperties

    class IAudioPropertiesRow

    class ILocalAudioPropertiesRow
    ILocalAudioPropertiesRow --|> IAudioProperties
    ILocalAudioPropertiesRow --|> ILocalPropertiesRow
    ILocalAudioPropertiesRow --|> IAudioPropertiesRow

    class IAudioPropertiesListItem

    class ILocalAudioPropertiesListItem
    ILocalAudioPropertiesListItem --|> IAudioPropertiesListItem
    ILocalAudioPropertiesListItem --|> ILocalPropertiesListItem
    ILocalAudioPropertiesListItem --|> ILocalAudioPropertiesRow

    class IAudioPropertySet

    class ILocalAudioPropertySet
    ILocalAudioPropertySet --|> IAudioPropertySet
    ILocalAudioPropertySet --|> ILocalPropertySet
    ILocalAudioPropertySet --|> ILocalAudioPropertiesRow

    class IDocumentProperties

    class IDocumentPropertiesRow

    class ILocalDocumentPropertiesRow
    ILocalDocumentPropertiesRow --|> IDocumentProperties
    ILocalDocumentPropertiesRow --|> ILocalPropertiesRow
    ILocalDocumentPropertiesRow --|> IDocumentPropertiesRow

    class IDocumentPropertiesListItem

    class ILocalDocumentPropertiesListItem
    ILocalDocumentPropertiesListItem --|> IDocumentPropertiesListItem
    ILocalDocumentPropertiesListItem --|> ILocalPropertiesListItem
    ILocalDocumentPropertiesListItem --|> ILocalDocumentPropertiesRow

    class IDocumentPropertySet

    class ILocalDocumentPropertySet
    ILocalDocumentPropertySet --|> IDocumentPropertySet
    ILocalDocumentPropertySet --|> ILocalPropertySet
    ILocalDocumentPropertySet --|> ILocalDocumentPropertiesRow

    class IDRMProperties

    class IDRMPropertiesRow

    class ILocalDRMPropertiesRow
    ILocalDRMPropertiesRow --|> IDRMProperties
    ILocalDRMPropertiesRow --|> ILocalPropertiesRow
    ILocalDRMPropertiesRow --|> IDRMPropertiesRow

    class IDRMPropertiesListItem

    class ILocalDRMPropertiesListItem
    ILocalDRMPropertiesListItem --|> IDRMPropertiesListItem
    ILocalDRMPropertiesListItem --|> ILocalPropertiesListItem
    ILocalDRMPropertiesListItem --|> ILocalDRMPropertiesRow

    class IDRMPropertySet

    class ILocalDRMPropertySet
    ILocalDRMPropertySet --|> IDRMPropertySet
    ILocalDRMPropertySet --|> ILocalPropertySet
    ILocalDRMPropertySet --|> ILocalDRMPropertiesRow

    class IGPSProperties

    class IGPSPropertiesRow

    class ILocalGPSPropertiesRow
    ILocalGPSPropertiesRow --|> IGPSProperties
    ILocalGPSPropertiesRow --|> ILocalPropertiesRow
    ILocalGPSPropertiesRow --|> IGPSPropertiesRow

    class IGPSPropertiesListItem

    class ILocalGPSPropertiesListItem
    ILocalGPSPropertiesListItem --|> IGPSPropertiesListItem
    ILocalGPSPropertiesListItem --|> ILocalPropertiesListItem
    ILocalGPSPropertiesListItem --|> ILocalGPSPropertiesRow

    class IGPSPropertySet

    class ILocalGPSPropertySet
    ILocalGPSPropertySet --|> IGPSPropertySet
    ILocalGPSPropertySet --|> ILocalPropertySet
    ILocalGPSPropertySet --|> ILocalGPSPropertiesRow

    class IImageProperties

    class IImagePropertiesRow

    class ILocalImagePropertiesRow
    ILocalImagePropertiesRow --|> IImageProperties
    ILocalImagePropertiesRow --|> ILocalPropertiesRow
    ILocalImagePropertiesRow --|> IImagePropertiesRow

    class IImagePropertiesListItem

    class ILocalImagePropertiesListItem
    ILocalImagePropertiesListItem --|> IImagePropertiesListItem
    ILocalImagePropertiesListItem --|> ILocalPropertiesListItem
    ILocalImagePropertiesListItem --|> ILocalImagePropertiesRow

    class IImagePropertySet

    class ILocalImagePropertySet
    ILocalImagePropertySet --|> IImagePropertySet
    ILocalImagePropertySet --|> ILocalPropertySet
    ILocalImagePropertySet --|> ILocalImagePropertiesRow

    class IMediaProperties

    class IMediaPropertiesRow

    class ILocalMediaPropertiesRow
    ILocalMediaPropertiesRow --|> IMediaProperties
    ILocalMediaPropertiesRow --|> ILocalPropertiesRow
    ILocalMediaPropertiesRow --|> IMediaPropertiesRow

    class IMediaPropertiesListItem

    class ILocalMediaPropertiesListItem
    ILocalMediaPropertiesListItem --|> IMediaPropertiesListItem
    ILocalMediaPropertiesListItem --|> ILocalPropertiesListItem
    ILocalMediaPropertiesListItem --|> ILocalMediaPropertiesRow

    class IMediaPropertySet

    class ILocalMediaPropertySet
    ILocalMediaPropertySet --|> IMediaPropertySet
    ILocalMediaPropertySet --|> ILocalPropertySet
    ILocalMediaPropertySet --|> ILocalMediaPropertiesRow

    class IMusicProperties

    class IMusicPropertiesRow

    class ILocalMusicPropertiesRow
    ILocalMusicPropertiesRow --|> IMusicProperties
    ILocalMusicPropertiesRow --|> ILocalPropertiesRow
    ILocalMusicPropertiesRow --|> IMusicPropertiesRow

    class IMusicPropertiesListItem

    class ILocalMusicPropertiesListItem
    ILocalMusicPropertiesListItem --|> IMusicPropertiesListItem
    ILocalMusicPropertiesListItem --|> ILocalPropertiesListItem
    ILocalMusicPropertiesListItem --|> ILocalMusicPropertiesRow

    class IMusicPropertySet

    class ILocalMusicPropertySet
    ILocalMusicPropertySet --|> IMusicPropertySet
    ILocalMusicPropertySet --|> ILocalPropertySet
    ILocalMusicPropertySet --|> ILocalMusicPropertiesRow

    class IPhotoProperties

    class IPhotoPropertiesRow

    class ILocalPhotoPropertiesRow
    ILocalPhotoPropertiesRow --|> IPhotoProperties
    ILocalPhotoPropertiesRow --|> ILocalPropertiesRow
    ILocalPhotoPropertiesRow --|> IPhotoPropertiesRow

    class IPhotoPropertiesListItem

    class ILocalPhotoPropertiesListItem
    ILocalPhotoPropertiesListItem --|> IPhotoPropertiesListItem
    ILocalPhotoPropertiesListItem --|> ILocalPropertiesListItem
    ILocalPhotoPropertiesListItem --|> ILocalPhotoPropertiesRow

    class IPhotoPropertySet

    class ILocalPhotoPropertySet
    ILocalPhotoPropertySet --|> IPhotoPropertySet
    ILocalPhotoPropertySet --|> ILocalPropertySet
    ILocalPhotoPropertySet --|> ILocalPhotoPropertiesRow

    class IRecordedTVProperties

    class IRecordedTVPropertiesRow

    class ILocalRecordedTVPropertiesRow
    ILocalRecordedTVPropertiesRow --|> IRecordedTVProperties
    ILocalRecordedTVPropertiesRow --|> ILocalPropertiesRow
    ILocalRecordedTVPropertiesRow --|> IRecordedTVPropertiesRow

    class IRecordedTVPropertiesListItem

    class ILocalRecordedTVPropertiesListItem
    ILocalRecordedTVPropertiesListItem --|> IRecordedTVPropertiesListItem
    ILocalRecordedTVPropertiesListItem --|> ILocalPropertiesListItem
    ILocalRecordedTVPropertiesListItem --|> ILocalRecordedTVPropertiesRow

    class IRecordedTVPropertySet

    class ILocalRecordedTVPropertySet
    ILocalRecordedTVPropertySet --|> IRecordedTVPropertySet
    ILocalRecordedTVPropertySet --|> ILocalPropertySet
    ILocalRecordedTVPropertySet --|> ILocalRecordedTVPropertiesRow

    class ISummaryProperties

    class ISummaryPropertiesRow

    class ILocalSummaryPropertiesRow
    ILocalSummaryPropertiesRow --|> ISummaryProperties
    ILocalSummaryPropertiesRow --|> ILocalPropertiesRow
    ILocalSummaryPropertiesRow --|> ISummaryPropertiesRow

    class ILocalSummaryPropertiesListItem
    ILocalSummaryPropertiesListItem --|> ISummaryPropertiesListItem
    ILocalSummaryPropertiesListItem --|> ILocalPropertiesListItem
    ILocalSummaryPropertiesListItem --|> ILocalSummaryPropertiesRow

    class ISummaryPropertiesListItem

    class ILocalSummaryPropertySet
    ILocalSummaryPropertySet --|> ISummaryPropertySet
    ILocalSummaryPropertySet --|> ILocalPropertySet
    ILocalSummaryPropertySet --|> ILocalSummaryPropertiesRow

    class ISummaryPropertySet

    class IVideoProperties

    class IVideoPropertiesRow

    class ILocalVideoPropertiesRow
    ILocalVideoPropertiesRow --|> IVideoProperties
    ILocalVideoPropertiesRow --|> ILocalPropertiesRow
    ILocalVideoPropertiesRow --|> IVideoPropertiesRow

    class IVideoPropertiesListItem

    class ILocalVideoPropertiesListItem
    ILocalVideoPropertiesListItem --|> IVideoPropertiesListItem
    ILocalVideoPropertiesListItem --|> ILocalPropertiesListItem
    ILocalVideoPropertiesListItem --|> ILocalVideoPropertiesRow

    class IVideoPropertySet

    class ILocalVideoPropertySet
    ILocalVideoPropertySet --|> IVideoPropertySet
    ILocalVideoPropertySet --|> ILocalPropertySet
    ILocalVideoPropertySet --|> ILocalVideoPropertiesRow

    class IRedundancy

    class ILocalRedundancy
    ILocalRedundancy --|> IRedundancy
    ILocalRedundancy --|> ILocalDbEntity

    class IRedundantSetRow

    class ILocalRedundantSetRow
    ILocalRedundantSetRow --|> ILocalDbEntity
    ILocalRedundantSetRow --|> IRedundantSetRow

    class IRedundantSetListItem

    class ILocalRedundantSetListItem
    ILocalRedundantSetListItem --|> IRedundantSetListItem
    ILocalRedundantSetListItem --|> ILocalRedundantSetRow

    class IRedundantSet

    class ILocalRedundantSet
    ILocalRedundantSet --|> IRedundantSet
    ILocalRedundantSet --|> ILocalRedundantSetRow

    class ISymbolicNameRow

    class ILocalSymbolicNameRow
    ILocalSymbolicNameRow --|> ILocalDbEntity
    ILocalSymbolicNameRow --|> ISymbolicNameRow

    class ISymbolicNameListItem

    class ILocalSymbolicNameListItem
    ILocalSymbolicNameListItem --|> ISymbolicNameListItem
    ILocalSymbolicNameListItem --|> ILocalSymbolicNameRow

    class ISymbolicName

    class ILocalSymbolicName
    ILocalSymbolicName --|> ISymbolicName
    ILocalSymbolicName --|> ILocalSymbolicNameRow

    class ITagDefinitionRow

    class ILocalTagDefinitionRow
    ILocalTagDefinitionRow --|> ILocalDbEntity
    ILocalTagDefinitionRow --|> ITagDefinitionRow

    class ITagDefinitionListItem

    class ILocalTagDefinitionListItem
    ILocalTagDefinitionListItem --|> ITagDefinitionListItem
    ILocalTagDefinitionListItem --|> ILocalTagDefinitionRow

    class ITagDefinition

    class ILocalTagDefinition
    ILocalTagDefinition --|> ILocalTagDefinitionRow
    ILocalTagDefinition --|> ITagDefinition

    class IPersonalTagDefinition

    class ILocalPersonalTagDefinition
    ILocalPersonalTagDefinition --|> IPersonalTagDefinition
    ILocalPersonalTagDefinition --|> ILocalTagDefinition

    class ISharedTagDefinition

    class ILocalSharedTagDefinition
    ILocalSharedTagDefinition --|> ISharedTagDefinition
    ILocalSharedTagDefinition --|> ILocalTagDefinition

    class IVolumeRow

    class ILocalVolumeRow
    ILocalVolumeRow --|> ILocalDbEntity
    ILocalVolumeRow --|> IVolumeRow

    class IVolumeListItem

    class ILocalVolumeListItem
    ILocalVolumeListItem --|> IVolumeListItem
    ILocalVolumeListItem --|> ILocalVolumeRow

    class IVolumeListItemWithFileSystem

    class ILocalVolumeListItemWithFileSystem
    ILocalVolumeListItemWithFileSystem --|> ILocalVolumeListItem
    ILocalVolumeListItemWithFileSystem --|> IVolumeListItemWithFileSystem

    class IVolume

    class ILocalVolume
    ILocalVolume --|> IVolume
    ILocalVolume --|> ILocalVolumeRow
```
