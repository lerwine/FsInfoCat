# Local Entity Interfaces

- [File Properties Interfaces](#file-properties-interfaces)
- [Tag Interfaces](#tag-interfaces)
- [File System Interfaces](#file-system-interfaces)
- [Crawl Interfaces](#crawl-interfaces)
- [Other Interfaces](#other-interfaces)

See Also:

- [Base Entity Interfaces](../Base-Entity-Interfaces.md)
- [Upstream Entity Interfaces](./Upstream/Entity-Interfaces.md)

## File Properties Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class FsInfoCat.Model.IDbEntity

    class ILocalDbEntity
    ILocalDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.IPropertiesListItem

    class FsInfoCat.Model.IPropertiesRow

    class ILocalPropertiesRow
    ILocalPropertiesRow --|> ILocalDbEntity
    ILocalPropertiesRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    ILocalPropertiesRow --|> FsInfoCat.Model.IPropertiesRow

    class ILocalPropertiesListItem
    ILocalPropertiesListItem --|> FsInfoCat.Model.IPropertiesListItem
    ILocalPropertiesListItem --|> ILocalPropertiesRow

    class FsInfoCat.Model.IPropertySet

    class ILocalPropertySet
    ILocalPropertySet --|> FsInfoCat.Model.IPropertySet
    ILocalPropertySet --|> ILocalPropertiesRow

    class FsInfoCat.Model.ISummaryProperties

    class FsInfoCat.Model.ISummaryPropertiesRow

    class ILocalSummaryPropertiesRow
    ILocalSummaryPropertiesRow --|> FsInfoCat.Model.ISummaryProperties
    ILocalSummaryPropertiesRow --|> FsInfoCat.Model.ISummaryPropertiesRow
    ILocalSummaryPropertiesRow --|> ILocalPropertiesRow

    class FsInfoCat.Model.ISummaryPropertiesListItem

    class ILocalSummaryPropertiesListItem
    ILocalSummaryPropertiesListItem --|> FsInfoCat.Model.ISummaryPropertiesListItem
    ILocalSummaryPropertiesListItem --|> ILocalPropertiesListItem
    ILocalSummaryPropertiesListItem --|> ILocalSummaryPropertiesRow

    class FsInfoCat.Model.ISummaryPropertySet

    class ILocalSummaryPropertySet
    ILocalSummaryPropertySet --|> ILocalPropertySet
    ILocalSummaryPropertySet --|> FsInfoCat.Model.ISummaryPropertySet
    ILocalSummaryPropertySet --|> ILocalSummaryPropertiesRow

    class FsInfoCat.Model.IAudioProperties

    class FsInfoCat.Model.IAudioPropertiesRow

    class ILocalAudioPropertiesRow
    ILocalAudioPropertiesRow --|> FsInfoCat.Model.IAudioProperties
    ILocalAudioPropertiesRow --|> FsInfoCat.Model.IAudioPropertiesRow
    ILocalAudioPropertiesRow --|> ILocalPropertiesRow

    class FsInfoCat.Model.IAudioPropertiesListItem

    class ILocalAudioPropertiesListItem
    ILocalAudioPropertiesListItem --|> FsInfoCat.Model.IAudioPropertiesListItem
    ILocalAudioPropertiesListItem --|> ILocalAudioPropertiesRow
    ILocalAudioPropertiesListItem --|> ILocalPropertiesListItem

    class FsInfoCat.Model.IAudioPropertySet

    class ILocalAudioPropertySet
    ILocalAudioPropertySet --|> ILocalPropertySet
    ILocalAudioPropertySet --|> FsInfoCat.Model.IAudioPropertySet
    ILocalAudioPropertySet --|> ILocalAudioPropertiesRow

    class FsInfoCat.Model.IDocumentProperties

    class FsInfoCat.Model.IDocumentPropertiesListItem

    class FsInfoCat.Model.IDocumentPropertiesRow

    class ILocalDocumentPropertiesRow
    ILocalDocumentPropertiesRow --|> FsInfoCat.Model.IDocumentProperties
    ILocalDocumentPropertiesRow --|> FsInfoCat.Model.IDocumentPropertiesRow
    ILocalDocumentPropertiesRow --|> ILocalPropertiesRow

    class ILocalDocumentPropertiesListItem
    ILocalDocumentPropertiesListItem --|> FsInfoCat.Model.IDocumentPropertiesListItem
    ILocalDocumentPropertiesListItem --|> ILocalDocumentPropertiesRow
    ILocalDocumentPropertiesListItem --|> ILocalPropertiesListItem

    class FsInfoCat.Model.IDocumentPropertySet

    class ILocalDocumentPropertySet
    ILocalDocumentPropertySet --|> ILocalPropertySet
    ILocalDocumentPropertySet --|> FsInfoCat.Model.IDocumentPropertySet
    ILocalDocumentPropertySet --|> ILocalDocumentPropertiesRow

    class FsInfoCat.Model.IDRMProperties

    class FsInfoCat.Model.IDRMPropertiesListItem

    class FsInfoCat.Model.IDRMPropertiesRow

    class ILocalDRMPropertiesRow
    ILocalDRMPropertiesRow --|> FsInfoCat.Model.IDRMProperties
    ILocalDRMPropertiesRow --|> FsInfoCat.Model.IDRMPropertiesRow
    ILocalDRMPropertiesRow --|> ILocalPropertiesRow

    class ILocalDRMPropertiesListItem
    ILocalDRMPropertiesListItem --|> FsInfoCat.Model.IDRMPropertiesListItem
    ILocalDRMPropertiesListItem --|> ILocalDRMPropertiesRow
    ILocalDRMPropertiesListItem --|> ILocalPropertiesListItem

    class FsInfoCat.Model.IDRMPropertySet

    class ILocalDRMPropertySet
    ILocalDRMPropertySet --|> ILocalPropertySet
    ILocalDRMPropertySet --|> FsInfoCat.Model.IDRMPropertySet
    ILocalDRMPropertySet --|> ILocalDRMPropertiesRow

    class FsInfoCat.Model.IGPSProperties

    class FsInfoCat.Model.IGPSPropertiesListItem

    class FsInfoCat.Model.IGPSPropertiesRow

    class ILocalGPSPropertiesRow
    ILocalGPSPropertiesRow --|> FsInfoCat.Model.IGPSProperties
    ILocalGPSPropertiesRow --|> FsInfoCat.Model.IGPSPropertiesRow
    ILocalGPSPropertiesRow --|> ILocalPropertiesRow

    class ILocalGPSPropertiesListItem
    ILocalGPSPropertiesListItem --|> FsInfoCat.Model.IGPSPropertiesListItem
    ILocalGPSPropertiesListItem --|> ILocalGPSPropertiesRow
    ILocalGPSPropertiesListItem --|> ILocalPropertiesListItem

    class FsInfoCat.Model.IGPSPropertySet

    class ILocalGPSPropertySet
    ILocalGPSPropertySet --|> ILocalPropertySet
    ILocalGPSPropertySet --|> FsInfoCat.Model.IGPSPropertySet
    ILocalGPSPropertySet --|> ILocalGPSPropertiesRow

    class FsInfoCat.Model.IImageProperties

    class FsInfoCat.Model.IImagePropertiesRow

    class ILocalImagePropertiesRow
    ILocalImagePropertiesRow --|> FsInfoCat.Model.IImageProperties
    ILocalImagePropertiesRow --|> FsInfoCat.Model.IImagePropertiesRow
    ILocalImagePropertiesRow --|> ILocalPropertiesRow

    class FsInfoCat.Model.IImagePropertiesListItem

    class ILocalImagePropertiesListItem
    ILocalImagePropertiesListItem --|> FsInfoCat.Model.IImagePropertiesListItem
    ILocalImagePropertiesListItem --|> ILocalImagePropertiesRow
    ILocalImagePropertiesListItem --|> ILocalPropertiesListItem

    class FsInfoCat.Model.IImagePropertySet

    class ILocalImagePropertySet
    ILocalImagePropertySet --|> ILocalPropertySet
    ILocalImagePropertySet --|> FsInfoCat.Model.IImagePropertySet
    ILocalImagePropertySet --|> ILocalImagePropertiesRow

    class FsInfoCat.Model.IMediaProperties

    class FsInfoCat.Model.IMediaPropertiesRow

    class ILocalMediaPropertiesRow
    ILocalMediaPropertiesRow --|> FsInfoCat.Model.IMediaProperties
    ILocalMediaPropertiesRow --|> FsInfoCat.Model.IMediaPropertiesRow
    ILocalMediaPropertiesRow --|> ILocalPropertiesRow

    class FsInfoCat.Model.IMediaPropertiesListItem

    class ILocalMediaPropertiesListItem
    ILocalMediaPropertiesListItem --|> FsInfoCat.Model.IMediaPropertiesListItem
    ILocalMediaPropertiesListItem --|> ILocalMediaPropertiesRow
    ILocalMediaPropertiesListItem --|> ILocalPropertiesListItem

    class FsInfoCat.Model.IMediaPropertySet

    class ILocalMediaPropertySet
    ILocalMediaPropertySet --|> ILocalPropertySet
    ILocalMediaPropertySet --|> FsInfoCat.Model.IMediaPropertySet
    ILocalMediaPropertySet --|> ILocalMediaPropertiesRow

    class FsInfoCat.Model.IMusicProperties

    class FsInfoCat.Model.IMusicPropertiesRow

    class ILocalMusicPropertiesRow
    ILocalMusicPropertiesRow --|> FsInfoCat.Model.IMusicProperties
    ILocalMusicPropertiesRow --|> FsInfoCat.Model.IMusicPropertiesRow
    ILocalMusicPropertiesRow --|> ILocalPropertiesRow

    class FsInfoCat.Model.IMusicPropertiesListItem

    class ILocalMusicPropertiesListItem
    ILocalMusicPropertiesListItem --|> FsInfoCat.Model.IMusicPropertiesListItem
    ILocalMusicPropertiesListItem --|> ILocalMusicPropertiesRow
    ILocalMusicPropertiesListItem --|> ILocalPropertiesListItem

    class FsInfoCat.Model.IMusicPropertySet

    class ILocalMusicPropertySet
    ILocalMusicPropertySet --|> ILocalPropertySet
    ILocalMusicPropertySet --|> FsInfoCat.Model.IMusicPropertySet
    ILocalMusicPropertySet --|> ILocalMusicPropertiesRow

    class FsInfoCat.Model.IPhotoProperties

    class FsInfoCat.Model.IPhotoPropertiesRow

    class ILocalPhotoPropertiesRow
    ILocalPhotoPropertiesRow --|> FsInfoCat.Model.IPhotoProperties
    ILocalPhotoPropertiesRow --|> FsInfoCat.Model.IPhotoPropertiesRow
    ILocalPhotoPropertiesRow --|> ILocalPropertiesRow

    class FsInfoCat.Model.IPhotoPropertiesListItem

    class ILocalPhotoPropertiesListItem
    ILocalPhotoPropertiesListItem --|> FsInfoCat.Model.IPhotoPropertiesListItem
    ILocalPhotoPropertiesListItem --|> ILocalPhotoPropertiesRow
    ILocalPhotoPropertiesListItem --|> ILocalPropertiesListItem

    class FsInfoCat.Model.IPhotoPropertySet

    class ILocalPhotoPropertySet
    ILocalPhotoPropertySet --|> ILocalPropertySet
    ILocalPhotoPropertySet --|> FsInfoCat.Model.IPhotoPropertySet
    ILocalPhotoPropertySet --|> ILocalPhotoPropertiesRow

    class FsInfoCat.Model.IRecordedTVProperties

    class FsInfoCat.Model.IRecordedTVPropertiesRow

    class ILocalRecordedTVPropertiesRow
    ILocalRecordedTVPropertiesRow --|> FsInfoCat.Model.IRecordedTVProperties
    ILocalRecordedTVPropertiesRow --|> FsInfoCat.Model.IRecordedTVPropertiesRow
    ILocalRecordedTVPropertiesRow --|> ILocalPropertiesRow

    class FsInfoCat.Model.IRecordedTVPropertiesListItem

    class ILocalRecordedTVPropertiesListItem
    ILocalRecordedTVPropertiesListItem --|> FsInfoCat.Model.IRecordedTVPropertiesListItem
    ILocalRecordedTVPropertiesListItem --|> ILocalPropertiesListItem
    ILocalRecordedTVPropertiesListItem --|> ILocalRecordedTVPropertiesRow

    class FsInfoCat.Model.IRecordedTVPropertySet

    class ILocalRecordedTVPropertySet
    ILocalRecordedTVPropertySet --|> ILocalPropertySet
    ILocalRecordedTVPropertySet --|> FsInfoCat.Model.IRecordedTVPropertySet
    ILocalRecordedTVPropertySet --|> ILocalRecordedTVPropertiesRow

    class FsInfoCat.Model.IVideoProperties

    class FsInfoCat.Model.IVideoPropertiesRow

    class ILocalVideoPropertiesRow
    ILocalVideoPropertiesRow --|> FsInfoCat.Model.IVideoProperties
    ILocalVideoPropertiesRow --|> FsInfoCat.Model.IVideoPropertiesRow
    ILocalVideoPropertiesRow --|> ILocalPropertiesRow

    class FsInfoCat.Model.IVideoPropertiesListItem

    class ILocalVideoPropertiesListItem
    ILocalVideoPropertiesListItem --|> FsInfoCat.Model.IVideoPropertiesListItem
    ILocalVideoPropertiesListItem --|> ILocalPropertiesListItem
    ILocalVideoPropertiesListItem --|> ILocalVideoPropertiesRow

    class FsInfoCat.Model.IVideoPropertySet

    class ILocalVideoPropertySet
    ILocalVideoPropertySet --|> ILocalPropertySet
    ILocalVideoPropertySet --|> FsInfoCat.Model.IVideoPropertySet
    ILocalVideoPropertySet --|> ILocalVideoPropertiesRow
```

## Tag Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class FsInfoCat.Model.IDbEntity

    class ILocalDbEntity
    ILocalDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.IItemTagRow

    class ILocalItemTagRow
    ILocalItemTagRow --|> ILocalDbEntity
    ILocalItemTagRow --|> FsInfoCat.Model.IHasCompoundIdentifier
    ILocalItemTagRow --|> FsInfoCat.Model.IHasIdentifierPair
    ILocalItemTagRow --|> FsInfoCat.Model.IItemTagRow

    class FsInfoCat.Model.IItemTagListItem

    class ILocalItemTagListItem
    ILocalItemTagListItem --|> FsInfoCat.Model.IItemTagListItem
    ILocalItemTagListItem --|> ILocalItemTagRow

    class FsInfoCat.Model.IItemTag

    class ILocalItemTag
    ILocalItemTag --|> FsInfoCat.Model.IItemTag
    ILocalItemTag --|> ILocalItemTagRow

    class FsInfoCat.Model.IFileTag

    class ILocalFileTag
    ILocalFileTag --|> FsInfoCat.Model.IFileTag
    ILocalFileTag --|> ILocalItemTag
    ILocalFileTag --|> FsInfoCat.Model.IHasMembershipKeyReference

    class FsInfoCat.Model.ISubdirectoryTag

    class ILocalSubdirectoryTag
    ILocalSubdirectoryTag --|> FsInfoCat.Model.ISubdirectoryTag
    ILocalSubdirectoryTag --|> ILocalItemTag
    ILocalSubdirectoryTag --|> FsInfoCat.Model.IHasMembershipKeyReference

    class FsInfoCat.Model.IVolumeTag

    class ILocalVolumeTag
    ILocalVolumeTag --|> FsInfoCat.Model.IVolumeTag
    ILocalVolumeTag --|> ILocalItemTag
    ILocalVolumeTag --|> FsInfoCat.Model.IHasMembershipKeyReference

    class FsInfoCat.Model.IPersonalTag

    class ILocalPersonalTag
    ILocalPersonalTag --|> FsInfoCat.Model.IPersonalTag
    ILocalPersonalTag --|> ILocalItemTag

    class FsInfoCat.Model.IPersonalFileTag

    class ILocalPersonalFileTag
    ILocalPersonalFileTag --|> ILocalPersonalTag
    ILocalPersonalFileTag --|> ILocalFileTag
    ILocalPersonalFileTag --|> FsInfoCat.Model.IPersonalFileTag

    class FsInfoCat.Model.IPersonalSubdirectoryTag

    class ILocalPersonalSubdirectoryTag
    ILocalPersonalSubdirectoryTag --|> ILocalPersonalTag
    ILocalPersonalSubdirectoryTag --|> ILocalSubdirectoryTag
    ILocalPersonalSubdirectoryTag --|> FsInfoCat.Model.IPersonalSubdirectoryTag

    class FsInfoCat.Model.IPersonalVolumeTag

    class ILocalPersonalVolumeTag
    ILocalPersonalVolumeTag --|> ILocalPersonalTag
    ILocalPersonalVolumeTag --|> ILocalVolumeTag
    ILocalPersonalVolumeTag --|> FsInfoCat.Model.IPersonalVolumeTag

    class FsInfoCat.Model.ISharedTag

    class ILocalSharedTag
    ILocalSharedTag --|> FsInfoCat.Model.ISharedTag
    ILocalSharedTag --|> ILocalItemTag

    class FsInfoCat.Model.ISharedFileTag

    class ILocalSharedFileTag
    ILocalSharedFileTag --|> ILocalSharedTag
    ILocalSharedFileTag --|> ILocalFileTag
    ILocalSharedFileTag --|> FsInfoCat.Model.ISharedFileTag

    class FsInfoCat.Model.ISharedSubdirectoryTag

    class ILocalSharedSubdirectoryTag
    ILocalSharedSubdirectoryTag --|> ILocalSharedTag
    ILocalSharedSubdirectoryTag --|> ILocalSubdirectoryTag
    ILocalSharedSubdirectoryTag --|> FsInfoCat.Model.ISharedSubdirectoryTag

    class FsInfoCat.Model.ISharedVolumeTag

    class ILocalSharedVolumeTag
    ILocalSharedVolumeTag --|> ILocalSharedTag
    ILocalSharedVolumeTag --|> ILocalVolumeTag
    ILocalSharedVolumeTag --|> FsInfoCat.Model.ISharedVolumeTag

    class FsInfoCat.Model.ITagDefinitionRow

    class ILocalTagDefinitionRow
    ILocalTagDefinitionRow --|> ILocalDbEntity
    ILocalTagDefinitionRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    ILocalTagDefinitionRow --|> FsInfoCat.Model.ITagDefinitionRow

    class FsInfoCat.Model.ITagDefinitionListItem

    class ILocalTagDefinitionListItem
    ILocalTagDefinitionListItem --|> FsInfoCat.Model.ITagDefinitionListItem
    ILocalTagDefinitionListItem --|> ILocalTagDefinitionRow

    class FsInfoCat.Model.ITagDefinition

    class ILocalTagDefinition
    ILocalTagDefinition --|> FsInfoCat.Model.ITagDefinition
    ILocalTagDefinition --|> ILocalTagDefinitionRow

    class FsInfoCat.Model.IPersonalTagDefinition

    class ILocalPersonalTagDefinition
    ILocalPersonalTagDefinition --|> ILocalTagDefinition
    ILocalPersonalTagDefinition --|> FsInfoCat.Model.IPersonalTagDefinition

    class FsInfoCat.Model.ISharedTagDefinition

    class ILocalSharedTagDefinition
    ILocalSharedTagDefinition --|> ILocalTagDefinition
    ILocalSharedTagDefinition --|> FsInfoCat.Model.ISharedTagDefinition
```

## File System Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class FsInfoCat.Model.IDbEntity

    class ILocalDbEntity
    ILocalDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.ICrawlJobLog

    class ILocalCrawlJobLog
    ILocalCrawlJobLog --|> FsInfoCat.Model.ICrawlJobLog
    ILocalCrawlJobLog --|> ILocalCrawlJobLogRow

    class FsInfoCat.Model.IDbFsItemRow

    class ILocalDbFsItemRow
    ILocalDbFsItemRow --|> ILocalDbEntity
    ILocalDbFsItemRow --|> FsInfoCat.Model.IDbFsItemRow
    ILocalDbFsItemRow --|> FsInfoCat.Model.IHasSimpleIdentifier

    class FsInfoCat.Model.IDbFsItemListItem

    class ILocalDbFsItemListItem
    ILocalDbFsItemListItem --|> FsInfoCat.Model.IDbFsItemListItem
    ILocalDbFsItemListItem --|> ILocalDbFsItemRow

    class FsInfoCat.Model.IDbFsItemAncestorName

    class FsInfoCat.Model.IDbFsItemListItemWithAncestorNames

    class ILocalDbFsItemListItemWithAncestorNames
    ILocalDbFsItemListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemAncestorName
    ILocalDbFsItemListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemListItemWithAncestorNames
    ILocalDbFsItemListItemWithAncestorNames --|> ILocalDbFsItemListItem

    class FsInfoCat.Model.IDbFsItem

    class ILocalDbFsItem
    ILocalDbFsItem --|> FsInfoCat.Model.IDbFsItem
    ILocalDbFsItem --|> ILocalDbFsItemRow

    class FsInfoCat.Model.IFileRow

    class ILocalFileRow
    ILocalFileRow --|> FsInfoCat.Model.IFileRow
    ILocalFileRow --|> ILocalDbFsItemRow

    class FsInfoCat.Model.IFileAncestorName

    class FsInfoCat.Model.IFileListItemWithAncestorNames

    class ILocalFileListItemWithAncestorNames
    ILocalFileListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemAncestorName
    ILocalFileListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemListItemWithAncestorNames
    ILocalFileListItemWithAncestorNames --|> FsInfoCat.Model.IFileAncestorName
    ILocalFileListItemWithAncestorNames --|> FsInfoCat.Model.IFileListItemWithAncestorNames
    ILocalFileListItemWithAncestorNames --|> ILocalDbFsItemListItem
    ILocalFileListItemWithAncestorNames --|> ILocalFileRow

    class FsInfoCat.Model.IFileListItemWithBinaryProperties

    class ILocalFileListItemWithBinaryProperties
    ILocalFileListItemWithBinaryProperties --|> FsInfoCat.Model.IFileListItemWithBinaryProperties
    ILocalFileListItemWithBinaryProperties --|> ILocalDbFsItemListItem
    ILocalFileListItemWithBinaryProperties --|> ILocalFileRow

    class FsInfoCat.Model.IFileListItemWithBinaryPropertiesAndAncestorNames

    class ILocalFileListItemWithBinaryPropertiesAndAncestorNames
    ILocalFileListItemWithBinaryPropertiesAndAncestorNames --|> FsInfoCat.Model.IFileListItemWithBinaryPropertiesAndAncestorNames
    ILocalFileListItemWithBinaryPropertiesAndAncestorNames --|> ILocalFileListItemWithAncestorNames

    class FsInfoCat.Model.IFile

    class ILocalFile
    ILocalFile --|> FsInfoCat.Model.IFile
    ILocalFile --|> ILocalDbFsItem
    ILocalFile --|> ILocalFileRow

    class FsInfoCat.Model.ISubdirectoryRow

    class ILocalSubdirectoryRow
    ILocalSubdirectoryRow --|> FsInfoCat.Model.ISubdirectoryRow
    ILocalSubdirectoryRow --|> ILocalDbFsItemRow

    class FsInfoCat.Model.ISubdirectoryListItem

    class ILocalSubdirectoryListItem
    ILocalSubdirectoryListItem --|> FsInfoCat.Model.ISubdirectoryListItem
    ILocalSubdirectoryListItem --|> FsInfoCat.Model.IDbFsItemListItem
    ILocalSubdirectoryListItem --|> ILocalSubdirectoryRow

    class FsInfoCat.Model.ISubdirectoryAncestorName

    class FsInfoCat.Model.ISubdirectoryListItemWithAncestorNames

    class ILocalSubdirectoryListItemWithAncestorNames
    ILocalSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.ISubdirectoryListItem
    ILocalSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.ISubdirectoryAncestorName
    ILocalSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemAncestorName
    ILocalSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemListItem
    ILocalSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemListItemWithAncestorNames
    ILocalSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.ISubdirectoryListItemWithAncestorNames
    ILocalSubdirectoryListItemWithAncestorNames --|> ILocalSubdirectoryRow

    class FsInfoCat.Model.ISubdirectory

    class ILocalSubdirectory
    ILocalSubdirectory --|> FsInfoCat.Model.ISubdirectory
    ILocalSubdirectory --|> ILocalDbFsItem
    ILocalSubdirectory --|> ILocalSubdirectoryRow

    class FsInfoCat.Model.IFileSystemProperties

    class FsInfoCat.Model.IFileSystemRow

    class ILocalFileSystemRow
    ILocalFileSystemRow --|> FsInfoCat.Model.IFileSystemProperties
    ILocalFileSystemRow --|> ILocalDbEntity
    ILocalFileSystemRow --|> FsInfoCat.Model.IFileSystemRow
    ILocalFileSystemRow --|> FsInfoCat.Model.IHasSimpleIdentifier

    class FsInfoCat.Model.IFileSystemListItem

    class ILocalFileSystemListItem
    ILocalFileSystemListItem --|> FsInfoCat.Model.IFileSystemListItem
    ILocalFileSystemListItem --|> ILocalFileSystemRow

    class FsInfoCat.Model.IFileSystem

    class ILocalFileSystem
    ILocalFileSystem --|> FsInfoCat.Model.IFileSystemProperties
    ILocalFileSystem --|> FsInfoCat.Model.IFileSystem
    ILocalFileSystem --|> ILocalDbEntity
    ILocalFileSystem --|> FsInfoCat.Model.IFileSystemRow
    ILocalFileSystem --|> FsInfoCat.Model.IHasSimpleIdentifier

    class FsInfoCat.Model.ISymbolicNameRow

    class ILocalSymbolicNameRow
    ILocalSymbolicNameRow --|> ILocalDbEntity
    ILocalSymbolicNameRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    ILocalSymbolicNameRow --|> FsInfoCat.Model.ISymbolicNameRow

    class FsInfoCat.Model.ISymbolicNameListItem

    class ILocalSymbolicNameListItem
    ILocalSymbolicNameListItem --|> FsInfoCat.Model.ISymbolicNameListItem
    ILocalSymbolicNameListItem --|> ILocalSymbolicNameRow

    class FsInfoCat.Model.ISymbolicName

    class ILocalSymbolicName
    ILocalSymbolicName --|> FsInfoCat.Model.ISymbolicName
    ILocalSymbolicName --|> ILocalSymbolicNameRow

    class FsInfoCat.Model.IVolumeRow

    class ILocalVolumeRow
    ILocalVolumeRow --|> ILocalDbEntity
    ILocalVolumeRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    ILocalVolumeRow --|> FsInfoCat.Model.IVolumeRow

    class FsInfoCat.Model.IVolumeListItem

    class ILocalVolumeListItem
    ILocalVolumeListItem --|> FsInfoCat.Model.IVolumeListItem
    ILocalVolumeListItem --|> ILocalVolumeRow

    class FsInfoCat.Model.IVolumeListItemWithFileSystem

    class ILocalVolumeListItemWithFileSystem
    ILocalVolumeListItemWithFileSystem --|> FsInfoCat.Model.IVolumeListItemWithFileSystem
    ILocalVolumeListItemWithFileSystem --|> ILocalVolumeListItem

    class FsInfoCat.Model.IVolume

    class ILocalVolume
    ILocalVolume --|> FsInfoCat.Model.IVolume
    ILocalVolume --|> ILocalVolumeRow
```

## Crawl Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class FsInfoCat.Model.IDbEntity

    class ILocalDbEntity
    ILocalDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.ICrawlConfigurationRow

    class FsInfoCat.Model.ICrawlSettings

    class ILocalCrawlConfigurationRow
    ILocalCrawlConfigurationRow --|> ILocalDbEntity
    ILocalCrawlConfigurationRow --|> FsInfoCat.Model.ICrawlConfigurationRow
    ILocalCrawlConfigurationRow --|> FsInfoCat.Model.ICrawlSettings
    ILocalCrawlConfigurationRow --|> FsInfoCat.Model.IHasSimpleIdentifier

    class FsInfoCat.Model.ICrawlConfigurationListItem

    class ILocalCrawlConfigurationListItem
    ILocalCrawlConfigurationListItem --|> FsInfoCat.Model.ICrawlConfigurationListItem
    ILocalCrawlConfigurationListItem --|> ILocalCrawlConfigurationRow

    class FsInfoCat.Model.ICrawlConfiguration

    class ILocalCrawlConfiguration
    ILocalCrawlConfiguration --|> FsInfoCat.Model.ICrawlConfiguration
    ILocalCrawlConfiguration --|> ILocalCrawlConfigurationRow

    class FsInfoCat.Model.ICrawlConfigReportItem

    class ILocalCrawlConfigReportItem
    ILocalCrawlConfigReportItem --|> FsInfoCat.Model.ICrawlConfigReportItem
    ILocalCrawlConfigReportItem --|> ILocalCrawlConfigurationListItem

    class FsInfoCat.Model.ICrawlJobLogRow

    class ILocalCrawlJobLogRow
    ILocalCrawlJobLogRow --|> ILocalDbEntity
    ILocalCrawlJobLogRow --|> FsInfoCat.Model.ICrawlJobLogRow
    ILocalCrawlJobLogRow --|> FsInfoCat.Model.ICrawlSettings
    ILocalCrawlJobLogRow --|> FsInfoCat.Model.IHasSimpleIdentifier

    class FsInfoCat.Model.ICrawlJobListItem

    class ILocalCrawlJobListItem
    ILocalCrawlJobListItem --|> FsInfoCat.Model.ICrawlJobListItem
    ILocalCrawlJobListItem --|> ILocalCrawlJobLogRow
```

## Other Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class FsInfoCat.Model.IDbEntity

    class ILocalDbEntity
    ILocalDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.IAccessError

    class FsInfoCat.Model.IHasSimpleIdentifier

    class ILocalAccessError
    ILocalAccessError --|> FsInfoCat.Model.IAccessError
    ILocalAccessError --|> FsInfoCat.Model.IDbEntity
    ILocalAccessError --|> FsInfoCat.Model.IHasSimpleIdentifier

    class FsInfoCat.Model.IFileAccessError

    class ILocalFileAccessError
    ILocalFileAccessError --|> ILocalAccessError
    ILocalFileAccessError --|> FsInfoCat.Model.IFileAccessError

    class FsInfoCat.Model.ISubdirectoryAccessError

    class ILocalSubdirectoryAccessError
    ILocalSubdirectoryAccessError --|> ILocalAccessError
    ILocalSubdirectoryAccessError --|> FsInfoCat.Model.ISubdirectoryAccessError

    class FsInfoCat.Model.IVolumeAccessError

    class ILocalVolumeAccessError
    ILocalVolumeAccessError --|> ILocalAccessError
    ILocalVolumeAccessError --|> FsInfoCat.Model.IVolumeAccessError

    class FsInfoCat.Model.IBinaryPropertySet

    class ILocalBinaryPropertySet
    ILocalBinaryPropertySet --|> FsInfoCat.Model.IBinaryPropertySet
    ILocalBinaryPropertySet --|> ILocalDbEntity
    ILocalBinaryPropertySet --|> FsInfoCat.Model.IHasSimpleIdentifier

    class FsInfoCat.Model.IComparison

    class FsInfoCat.Model.IHasCompoundIdentifier

    class FsInfoCat.Model.IHasIdentifierPair

    class FsInfoCat.Model.IHasMembershipKeyReference

    class ILocalComparison
    ILocalComparison --|> FsInfoCat.Model.IComparison
    ILocalComparison --|> ILocalDbEntity
    ILocalComparison --|> FsInfoCat.Model.IHasCompoundIdentifier
    ILocalComparison --|> FsInfoCat.Model.IHasIdentifierPair
    ILocalComparison --|> FsInfoCat.Model.IHasMembershipKeyReference

    class FsInfoCat.Model.IRedundancy

    class ILocalRedundancy
    ILocalRedundancy --|> FsInfoCat.Model.IRedundancy
    ILocalRedundancy --|> ILocalDbEntity
    ILocalRedundancy --|> FsInfoCat.Model.IHasCompoundIdentifier
    ILocalRedundancy --|> FsInfoCat.Model.IHasIdentifierPair
    ILocalRedundancy --|> FsInfoCat.Model.IHasMembershipKeyReference

    class FsInfoCat.Model.IRedundantSetRow

    class ILocalRedundantSetRow
    ILocalRedundantSetRow --|> ILocalDbEntity
    ILocalRedundantSetRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    ILocalRedundantSetRow --|> FsInfoCat.Model.IRedundantSetRow

    class FsInfoCat.Model.IRedundantSetListItem

    class ILocalRedundantSetListItem
    ILocalRedundantSetListItem --|> FsInfoCat.Model.IRedundantSetListItem
    ILocalRedundantSetListItem --|> ILocalRedundantSetRow

    class FsInfoCat.Model.IRedundantSet

    class ILocalRedundantSet
    ILocalRedundantSet --|> FsInfoCat.Model.IRedundantSet
    ILocalRedundantSet --|> ILocalRedundantSetRow
```
