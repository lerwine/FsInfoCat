# Upstream Entity Interfaces

- [File Properties Interfaces](#file-properties-interfaces)
- [Tag Interfaces](#tag-interfaces)
- [File System Interfaces](#file-system-interfaces)
- [Action, Membership and Task Interfaces](#action-membership-and-task-interfaces)
- [Crawl Interfaces](#crawl-interfaces)
- [Other Interfaces](#other-interfaces)

See Also:

- [Base Entity Interfaces](../Base-Entity-Interfaces.md)
- [Local Entity Interfaces](./Local/Entity-Interfaces.md)

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

    class IUpstreamDbEntity
    IUpstreamDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.IPropertiesListItem

    class FsInfoCat.Model.IPropertiesRow

    class IUpstreamPropertiesRow
    IUpstreamPropertiesRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamPropertiesRow --|> IUpstreamDbEntity
    IUpstreamPropertiesRow --|> FsInfoCat.Model.IPropertiesRow

    class IUpstreamPropertiesListItem
    IUpstreamPropertiesListItem --|> FsInfoCat.Model.IPropertiesListItem
    IUpstreamPropertiesListItem --|> IUpstreamPropertiesRow

    class FsInfoCat.Model.IPropertySet

    class IUpstreamPropertySet
    IUpstreamPropertySet --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamPropertySet --|> FsInfoCat.Model.IPropertySet
    IUpstreamPropertySet --|> IUpstreamDbEntity
    IUpstreamPropertySet --|> FsInfoCat.Model.IPropertiesRow

    class FsInfoCat.Model.ISummaryProperties

    class FsInfoCat.Model.ISummaryPropertiesRow

    class IUpstreamSummaryPropertiesRow
    IUpstreamSummaryPropertiesRow --|> FsInfoCat.Model.ISummaryProperties
    IUpstreamSummaryPropertiesRow --|> FsInfoCat.Model.ISummaryPropertiesRow
    IUpstreamSummaryPropertiesRow --|> IUpstreamPropertiesRow

    class FsInfoCat.Model.ISummaryPropertiesListItem

    class IUpstreamSummaryPropertiesListItem
    IUpstreamSummaryPropertiesListItem --|> FsInfoCat.Model.ISummaryPropertiesListItem
    IUpstreamSummaryPropertiesListItem --|> IUpstreamPropertiesListItem
    IUpstreamSummaryPropertiesListItem --|> IUpstreamSummaryPropertiesRow

    class FsInfoCat.Model.ISummaryPropertySet

    class IUpstreamSummaryPropertySet
    IUpstreamSummaryPropertySet --|> IUpstreamPropertySet
    IUpstreamSummaryPropertySet --|> FsInfoCat.Model.ISummaryPropertySet
    IUpstreamSummaryPropertySet --|> IUpstreamSummaryPropertiesRow

    class FsInfoCat.Model.IAudioProperties

    class FsInfoCat.Model.IAudioPropertiesRow

    class IUpstreamAudioPropertiesRow
    IUpstreamAudioPropertiesRow --|> FsInfoCat.Model.IAudioProperties
    IUpstreamAudioPropertiesRow --|> FsInfoCat.Model.IAudioPropertiesRow
    IUpstreamAudioPropertiesRow --|> IUpstreamPropertiesRow

    class FsInfoCat.Model.IAudioPropertiesListItem

    class IUpstreamAudioPropertiesListItem
    IUpstreamAudioPropertiesListItem --|> FsInfoCat.Model.IAudioPropertiesListItem
    IUpstreamAudioPropertiesListItem --|> IUpstreamAudioPropertiesRow
    IUpstreamAudioPropertiesListItem --|> IUpstreamPropertiesListItem

    class FsInfoCat.Model.IAudioPropertySet

    class IUpstreamAudioPropertySet
    IUpstreamAudioPropertySet --|> IUpstreamPropertySet
    IUpstreamAudioPropertySet --|> FsInfoCat.Model.IAudioPropertySet
    IUpstreamAudioPropertySet --|> IUpstreamAudioPropertiesRow

    class FsInfoCat.Model.IDocumentProperties

    class FsInfoCat.Model.IDocumentPropertiesListItem

    class FsInfoCat.Model.IDocumentPropertiesRow

    class IUpstreamDocumentPropertiesRow
    IUpstreamDocumentPropertiesRow --|> FsInfoCat.Model.IDocumentProperties
    IUpstreamDocumentPropertiesRow --|> FsInfoCat.Model.IDocumentPropertiesRow
    IUpstreamDocumentPropertiesRow --|> IUpstreamPropertiesRow

    class IUpstreamDocumentPropertiesListItem
    IUpstreamDocumentPropertiesListItem --|> FsInfoCat.Model.IDocumentPropertiesListItem
    IUpstreamDocumentPropertiesListItem --|> IUpstreamDocumentPropertiesRow
    IUpstreamDocumentPropertiesListItem --|> IUpstreamPropertiesListItem

    class FsInfoCat.Model.IDocumentPropertySet

    class IUpstreamDocumentPropertySet
    IUpstreamDocumentPropertySet --|> IUpstreamPropertySet
    IUpstreamDocumentPropertySet --|> FsInfoCat.Model.IDocumentPropertySet
    IUpstreamDocumentPropertySet --|> IUpstreamDocumentPropertiesRow

    class FsInfoCat.Model.IDRMProperties

    class FsInfoCat.Model.IDRMPropertiesListItem

    class FsInfoCat.Model.IDRMPropertiesRow

    class IUpstreamDRMPropertiesRow
    IUpstreamDRMPropertiesRow --|> FsInfoCat.Model.IDRMProperties
    IUpstreamDRMPropertiesRow --|> FsInfoCat.Model.IDRMPropertiesRow
    IUpstreamDRMPropertiesRow --|> IUpstreamPropertiesRow

    class IUpstreamDRMPropertiesListItem
    IUpstreamDRMPropertiesListItem --|> FsInfoCat.Model.IDRMPropertiesListItem
    IUpstreamDRMPropertiesListItem --|> IUpstreamDRMPropertiesRow
    IUpstreamDRMPropertiesListItem --|> IUpstreamPropertiesListItem

    class FsInfoCat.Model.IDRMPropertySet

    class IUpstreamDRMPropertySet
    IUpstreamDRMPropertySet --|> IUpstreamPropertySet
    IUpstreamDRMPropertySet --|> FsInfoCat.Model.IDRMPropertySet
    IUpstreamDRMPropertySet --|> IUpstreamDRMPropertiesRow

    class FsInfoCat.Model.IGPSProperties

    class FsInfoCat.Model.IGPSPropertiesListItem

    class FsInfoCat.Model.IGPSPropertiesRow

    class IUpstreamGPSPropertiesRow
    IUpstreamGPSPropertiesRow --|> FsInfoCat.Model.IGPSProperties
    IUpstreamGPSPropertiesRow --|> FsInfoCat.Model.IGPSPropertiesRow
    IUpstreamGPSPropertiesRow --|> IUpstreamPropertiesRow

    class IUpstreamGPSPropertiesListItem
    IUpstreamGPSPropertiesListItem --|> FsInfoCat.Model.IGPSPropertiesListItem
    IUpstreamGPSPropertiesListItem --|> IUpstreamGPSPropertiesRow
    IUpstreamGPSPropertiesListItem --|> IUpstreamPropertiesListItem

    class FsInfoCat.Model.IGPSPropertySet

    class IUpstreamGPSPropertySet
    IUpstreamGPSPropertySet --|> IUpstreamPropertySet
    IUpstreamGPSPropertySet --|> FsInfoCat.Model.IGPSPropertySet
    IUpstreamGPSPropertySet --|> IUpstreamGPSPropertiesRow

    class FsInfoCat.Model.IImageProperties

    class FsInfoCat.Model.IImagePropertiesRow

    class IUpstreamImagePropertiesRow
    IUpstreamImagePropertiesRow --|> FsInfoCat.Model.IImageProperties
    IUpstreamImagePropertiesRow --|> FsInfoCat.Model.IImagePropertiesRow
    IUpstreamImagePropertiesRow --|> IUpstreamPropertiesRow

    class FsInfoCat.Model.IImagePropertiesListItem

    class IUpstreamImagePropertiesListItem
    IUpstreamImagePropertiesListItem --|> FsInfoCat.Model.IImagePropertiesListItem
    IUpstreamImagePropertiesListItem --|> IUpstreamImagePropertiesRow
    IUpstreamImagePropertiesListItem --|> IUpstreamPropertiesListItem

    class FsInfoCat.Model.IImagePropertySet

    class IUpstreamImagePropertySet
    IUpstreamImagePropertySet --|> IUpstreamPropertySet
    IUpstreamImagePropertySet --|> FsInfoCat.Model.IImagePropertySet
    IUpstreamImagePropertySet --|> IUpstreamImagePropertiesRow

    class FsInfoCat.Model.IMediaProperties

    class FsInfoCat.Model.IMediaPropertiesRow

    class IUpstreamMediaPropertiesRow
    IUpstreamMediaPropertiesRow --|> FsInfoCat.Model.IMediaProperties
    IUpstreamMediaPropertiesRow --|> FsInfoCat.Model.IMediaPropertiesRow
    IUpstreamMediaPropertiesRow --|> IUpstreamPropertiesRow

    class FsInfoCat.Model.IMediaPropertiesListItem

    class IUpstreamMediaPropertiesListItem
    IUpstreamMediaPropertiesListItem --|> FsInfoCat.Model.IMediaPropertiesListItem
    IUpstreamMediaPropertiesListItem --|> IUpstreamMediaPropertiesRow
    IUpstreamMediaPropertiesListItem --|> IUpstreamPropertiesListItem

    class FsInfoCat.Model.IMediaPropertySet

    class IUpstreamMediaPropertySet
    IUpstreamMediaPropertySet --|> IUpstreamPropertySet
    IUpstreamMediaPropertySet --|> FsInfoCat.Model.IMediaPropertySet
    IUpstreamMediaPropertySet --|> IUpstreamMediaPropertiesRow

    class FsInfoCat.Model.IMusicProperties

    class FsInfoCat.Model.IMusicPropertiesRow

    class IUpstreamMusicPropertiesRow
    IUpstreamMusicPropertiesRow --|> FsInfoCat.Model.IMusicProperties
    IUpstreamMusicPropertiesRow --|> FsInfoCat.Model.IMusicPropertiesRow
    IUpstreamMusicPropertiesRow --|> IUpstreamPropertiesRow

    class FsInfoCat.Model.IMusicPropertiesListItem

    class IUpstreamMusicPropertiesListItem
    IUpstreamMusicPropertiesListItem --|> FsInfoCat.Model.IMusicPropertiesListItem
    IUpstreamMusicPropertiesListItem --|> IUpstreamMusicPropertiesRow
    IUpstreamMusicPropertiesListItem --|> IUpstreamPropertiesListItem

    class FsInfoCat.Model.IMusicPropertySet

    class IUpstreamMusicPropertySet
    IUpstreamMusicPropertySet --|> IUpstreamPropertySet
    IUpstreamMusicPropertySet --|> FsInfoCat.Model.IMusicPropertySet
    IUpstreamMusicPropertySet --|> IUpstreamMusicPropertiesRow

    class FsInfoCat.Model.IPhotoProperties

    class FsInfoCat.Model.IPhotoPropertiesRow

    class IUpstreamPhotoPropertiesRow
    IUpstreamPhotoPropertiesRow --|> FsInfoCat.Model.IPhotoProperties
    IUpstreamPhotoPropertiesRow --|> FsInfoCat.Model.IPhotoPropertiesRow
    IUpstreamPhotoPropertiesRow --|> IUpstreamPropertiesRow

    class FsInfoCat.Model.IPhotoPropertiesListItem

    class IUpstreamPhotoPropertiesListItem
    IUpstreamPhotoPropertiesListItem --|> FsInfoCat.Model.IPhotoProperties
    IUpstreamPhotoPropertiesListItem --|> FsInfoCat.Model.IPhotoPropertiesListItem
    IUpstreamPhotoPropertiesListItem --|> FsInfoCat.Model.IPhotoPropertiesRow
    IUpstreamPhotoPropertiesListItem --|> IUpstreamPropertiesListItem

    class FsInfoCat.Model.IPhotoPropertySet

    class IUpstreamPhotoPropertySet
    IUpstreamPhotoPropertySet --|> IUpstreamPropertySet
    IUpstreamPhotoPropertySet --|> FsInfoCat.Model.IPhotoPropertySet
    IUpstreamPhotoPropertySet --|> IUpstreamPhotoPropertiesRow

    class FsInfoCat.Model.IRecordedTVProperties

    class FsInfoCat.Model.IRecordedTVPropertiesRow

    class IUpstreamRecordedTVPropertiesRow
    IUpstreamRecordedTVPropertiesRow --|> FsInfoCat.Model.IRecordedTVProperties
    IUpstreamRecordedTVPropertiesRow --|> FsInfoCat.Model.IRecordedTVPropertiesRow
    IUpstreamRecordedTVPropertiesRow --|> IUpstreamPropertiesRow

    class FsInfoCat.Model.IRecordedTVPropertiesListItem

    class IUpstreamRecordedTVPropertiesListItem
    IUpstreamRecordedTVPropertiesListItem --|> FsInfoCat.Model.IRecordedTVPropertiesListItem
    IUpstreamRecordedTVPropertiesListItem --|> IUpstreamPropertiesListItem
    IUpstreamRecordedTVPropertiesListItem --|> IUpstreamRecordedTVPropertiesRow

    class FsInfoCat.Model.IRecordedTVPropertySet

    class IUpstreamRecordedTVPropertySet
    IUpstreamRecordedTVPropertySet --|> IUpstreamPropertySet
    IUpstreamRecordedTVPropertySet --|> FsInfoCat.Model.IRecordedTVPropertySet
    IUpstreamRecordedTVPropertySet --|> IUpstreamRecordedTVPropertiesRow

    class FsInfoCat.Model.IVideoProperties

    class FsInfoCat.Model.IVideoPropertiesRow

    class IUpstreamVideoPropertiesRow
    IUpstreamVideoPropertiesRow --|> FsInfoCat.Model.IVideoProperties
    IUpstreamVideoPropertiesRow --|> FsInfoCat.Model.IVideoPropertiesRow
    IUpstreamVideoPropertiesRow --|> IUpstreamPropertiesRow

    class FsInfoCat.Model.IVideoPropertiesListItem

    class IUpstreamVideoPropertiesListItem
    IUpstreamVideoPropertiesListItem --|> FsInfoCat.Model.IVideoPropertiesListItem
    IUpstreamVideoPropertiesListItem --|> IUpstreamPropertiesListItem
    IUpstreamVideoPropertiesListItem --|> IUpstreamVideoPropertiesRow

    class FsInfoCat.Model.IVideoPropertySet

    class IUpstreamVideoPropertySet
    IUpstreamVideoPropertySet --|> IUpstreamPropertySet
    IUpstreamVideoPropertySet --|> FsInfoCat.Model.IVideoPropertySet
    IUpstreamVideoPropertySet --|> IUpstreamVideoPropertiesRow
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

    class IUpstreamDbEntity
    IUpstreamDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.IItemTagRow

    class IUpstreamItemTagRow
    IUpstreamItemTagRow --|> FsInfoCat.Model.IHasCompoundIdentifier
    IUpstreamItemTagRow --|> FsInfoCat.Model.IHasIdentifierPair
    IUpstreamItemTagRow --|> FsInfoCat.Model.IItemTagRow
    IUpstreamItemTagRow --|> IUpstreamDbEntity

    class FsInfoCat.Model.IItemTagListItem

    class IUpstreamItemTagListItem
    IUpstreamItemTagListItem --|> FsInfoCat.Model.IItemTagListItem
    IUpstreamItemTagListItem --|> IUpstreamItemTagRow

    class FsInfoCat.Model.IItemTag

    class IUpstreamItemTag
    IUpstreamItemTag --|> FsInfoCat.Model.IItemTag
    IUpstreamItemTag --|> IUpstreamItemTagRow

    class FsInfoCat.Model.IFileTag

    class IUpstreamFileTag
    IUpstreamFileTag --|> FsInfoCat.Model.IFileTag
    IUpstreamFileTag --|> FsInfoCat.Model.IHasMembershipKeyReference
    IUpstreamFileTag --|> IUpstreamItemTag

    class FsInfoCat.Model.ISubdirectoryTag

    class IUpstreamSubdirectoryTag
    IUpstreamSubdirectoryTag --|> FsInfoCat.Model.ISubdirectoryTag
    IUpstreamSubdirectoryTag --|> FsInfoCat.Model.IHasMembershipKeyReference
    IUpstreamSubdirectoryTag --|> IUpstreamItemTag

    class FsInfoCat.Model.IVolumeTag

    class IUpstreamVolumeTag
    IUpstreamVolumeTag --|> FsInfoCat.Model.IVolumeTag
    IUpstreamVolumeTag --|> FsInfoCat.Model.IHasMembershipKeyReference
    IUpstreamVolumeTag --|> IUpstreamItemTag

    class FsInfoCat.Model.IPersonalTag

    class IUpstreamPersonalTag
    IUpstreamPersonalTag --|> FsInfoCat.Model.IPersonalTag
    IUpstreamPersonalTag --|> IUpstreamItemTag

    class FsInfoCat.Model.IPersonalFileTag

    class IUpstreamPersonalFileTag
    IUpstreamPersonalFileTag --|> IUpstreamPersonalTag
    IUpstreamPersonalFileTag --|> IUpstreamFileTag
    IUpstreamPersonalFileTag --|> FsInfoCat.Model.IPersonalFileTag

    class FsInfoCat.Model.IPersonalSubdirectoryTag

    class IUpstreamPersonalSubdirectoryTag
    IUpstreamPersonalSubdirectoryTag --|> IUpstreamPersonalTag
    IUpstreamPersonalSubdirectoryTag --|> IUpstreamSubdirectoryTag
    IUpstreamPersonalSubdirectoryTag --|> FsInfoCat.Model.IPersonalSubdirectoryTag

    class FsInfoCat.Model.IPersonalVolumeTag

    class IUpstreamPersonalVolumeTag
    IUpstreamPersonalVolumeTag --|> IUpstreamPersonalTag
    IUpstreamPersonalVolumeTag --|> IUpstreamVolumeTag
    IUpstreamPersonalVolumeTag --|> FsInfoCat.Model.IPersonalVolumeTag

    class FsInfoCat.Model.ISharedTag

    class IUpstreamSharedTag
    IUpstreamSharedTag --|> FsInfoCat.Model.ISharedTag
    IUpstreamSharedTag --|> IUpstreamItemTag

    class FsInfoCat.Model.ISharedFileTag

    class IUpstreamSharedFileTag
    IUpstreamSharedFileTag --|> IUpstreamSharedTag
    IUpstreamSharedFileTag --|> IUpstreamFileTag
    IUpstreamSharedFileTag --|> FsInfoCat.Model.ISharedFileTag

    class FsInfoCat.Model.ISharedSubdirectoryTag

    class IUpstreamSharedSubdirectoryTag
    IUpstreamSharedSubdirectoryTag --|> IUpstreamSharedTag
    IUpstreamSharedSubdirectoryTag --|> IUpstreamSubdirectoryTag
    IUpstreamSharedSubdirectoryTag --|> FsInfoCat.Model.ISharedSubdirectoryTag

    class FsInfoCat.Model.ISharedVolumeTag

    class IUpstreamSharedVolumeTag
    IUpstreamSharedVolumeTag --|> IUpstreamSharedTag
    IUpstreamSharedVolumeTag --|> IUpstreamVolumeTag
    IUpstreamSharedVolumeTag --|> FsInfoCat.Model.ISharedVolumeTag

    class FsInfoCat.Model.ITagDefinitionRow

    class IUpstreamTagDefinitionRow
    IUpstreamTagDefinitionRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamTagDefinitionRow --|> FsInfoCat.Model.ITagDefinitionRow
    IUpstreamTagDefinitionRow --|> IUpstreamDbEntity

    class FsInfoCat.Model.ITagDefinitionListItem

    class IUpstreamTagDefinitionListItem
    IUpstreamTagDefinitionListItem --|> FsInfoCat.Model.ITagDefinitionListItem
    IUpstreamTagDefinitionListItem --|> IUpstreamTagDefinitionRow

    class FsInfoCat.Model.ITagDefinition

    class IUpstreamTagDefinition
    IUpstreamTagDefinition --|> FsInfoCat.Model.ITagDefinition
    IUpstreamTagDefinition --|> IUpstreamTagDefinitionRow

    class FsInfoCat.Model.IPersonalTagDefinition

    class IUpstreamPersonalTagDefinition
    IUpstreamPersonalTagDefinition --|> FsInfoCat.Model.IPersonalTagDefinition
    IUpstreamPersonalTagDefinition --|> IUpstreamTagDefinition

    class FsInfoCat.Model.ISharedTagDefinition

    class IUpstreamSharedTagDefinition
    IUpstreamSharedTagDefinition --|> FsInfoCat.Model.ISharedTagDefinition
    IUpstreamSharedTagDefinition --|> IUpstreamTagDefinition
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

    class IUpstreamDbEntity
    IUpstreamDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.IDbFsItemRow

    class IUpstreamDbFsItemRow
    IUpstreamDbFsItemRow --|> FsInfoCat.Model.IDbFsItemRow
    IUpstreamDbFsItemRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamDbFsItemRow --|> IUpstreamDbEntity

    class FsInfoCat.Model.IDbFsItemListItem

    class IUpstreamDbFsItemListItem
    IUpstreamDbFsItemListItem --|> FsInfoCat.Model.IDbFsItemListItem
    IUpstreamDbFsItemListItem --|> IUpstreamDbFsItemRow

    class FsInfoCat.Model.IDbFsItemAncestorName

    class FsInfoCat.Model.IDbFsItemListItemWithAncestorNames

    class IUpstreamDbFsItemListItemWithAncestorNames
    IUpstreamDbFsItemListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemAncestorName
    IUpstreamDbFsItemListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemListItemWithAncestorNames
    IUpstreamDbFsItemListItemWithAncestorNames --|> IUpstreamDbFsItemListItem

    class FsInfoCat.Model.IDbFsItem

    class IUpstreamDbFsItem
    IUpstreamDbFsItem --|> FsInfoCat.Model.IDbFsItem
    IUpstreamDbFsItem --|> IUpstreamDbFsItemRow

    class FsInfoCat.Model.IFileRow

    class IUpstreamFileRow
    IUpstreamFileRow --|> FsInfoCat.Model.IFileRow
    IUpstreamFileRow --|> IUpstreamDbFsItemRow

    class FsInfoCat.Model.IFileAncestorName

    class FsInfoCat.Model.IFileListItemWithAncestorNames

    class IUpstreamFileListItemWithAncestorNames
    IUpstreamFileListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemAncestorName
    IUpstreamFileListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemListItemWithAncestorNames
    IUpstreamFileListItemWithAncestorNames --|> FsInfoCat.Model.IFileAncestorName
    IUpstreamFileListItemWithAncestorNames --|> FsInfoCat.Model.IFileListItemWithAncestorNames
    IUpstreamFileListItemWithAncestorNames --|> IUpstreamDbFsItemListItem
    IUpstreamFileListItemWithAncestorNames --|> IUpstreamFileRow

    class FsInfoCat.Model.IFileListItemWithBinaryProperties

    class IUpstreamFileListItemWithBinaryProperties
    IUpstreamFileListItemWithBinaryProperties --|> FsInfoCat.Model.IFileListItemWithBinaryProperties
    IUpstreamFileListItemWithBinaryProperties --|> IUpstreamDbFsItemListItem
    IUpstreamFileListItemWithBinaryProperties --|> IUpstreamFileRow

    class FsInfoCat.Model.IFileListItemWithBinaryPropertiesAndAncestorNames

    class IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames
    IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames --|> FsInfoCat.Model.IFileListItemWithBinaryPropertiesAndAncestorNames
    IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames --|> IUpstreamFileListItemWithAncestorNames

    class FsInfoCat.Model.IFile

    class IUpstreamFile
    IUpstreamFile --|> FsInfoCat.Model.IFile
    IUpstreamFile --|> IUpstreamDbFsItem
    IUpstreamFile --|> IUpstreamFileRow

    class FsInfoCat.Model.ISubdirectoryRow

    class IUpstreamSubdirectoryRow
    IUpstreamSubdirectoryRow --|> FsInfoCat.Model.ISubdirectoryRow
    IUpstreamSubdirectoryRow --|> IUpstreamDbFsItemRow

    class FsInfoCat.Model.ISubdirectoryListItem

    class IUpstreamSubdirectoryListItem
    IUpstreamSubdirectoryListItem --|> FsInfoCat.Model.ISubdirectoryListItem
    IUpstreamSubdirectoryListItem --|> FsInfoCat.Model.IDbFsItemListItem
    IUpstreamSubdirectoryListItem --|> IUpstreamSubdirectoryRow

    class FsInfoCat.Model.ISubdirectoryAncestorName

    class FsInfoCat.Model.ISubdirectoryListItemWithAncestorNames

    class IUpstreamSubdirectoryListItemWithAncestorNames
    IUpstreamSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.ISubdirectoryListItem
    IUpstreamSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.ISubdirectoryAncestorName
    IUpstreamSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemAncestorName
    IUpstreamSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemListItem
    IUpstreamSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.IDbFsItemListItemWithAncestorNames
    IUpstreamSubdirectoryListItemWithAncestorNames --|> FsInfoCat.Model.ISubdirectoryListItemWithAncestorNames
    IUpstreamSubdirectoryListItemWithAncestorNames --|> IUpstreamSubdirectoryRow

    class FsInfoCat.Model.ISubdirectory

    class IUpstreamSubdirectory
    IUpstreamSubdirectory --|> FsInfoCat.Model.ISubdirectory
    IUpstreamSubdirectory --|> FsInfoCat.Model.ISubdirectoryRow
    IUpstreamSubdirectory --|> IUpstreamDbFsItem

    class FsInfoCat.Model.IFileSystemProperties

    class FsInfoCat.Model.IFileSystemRow

    class IUpstreamFileSystemRow
    IUpstreamFileSystemRow --|> FsInfoCat.Model.IFileSystemProperties
    IUpstreamFileSystemRow --|> FsInfoCat.Model.IFileSystemRow
    IUpstreamFileSystemRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamFileSystemRow --|> IUpstreamDbEntity

    class FsInfoCat.Model.IFileSystemListItem

    class IUpstreamFileSystemListItem
    IUpstreamFileSystemListItem --|> FsInfoCat.Model.IFileSystemListItem
    IUpstreamFileSystemListItem --|> IUpstreamFileSystemRow

    class FsInfoCat.Model.IFileSystem

    class IUpstreamFileSystem
    IUpstreamFileSystem --|> FsInfoCat.Model.IFileSystemProperties
    IUpstreamFileSystem --|> FsInfoCat.Model.IFileSystem
    IUpstreamFileSystem --|> FsInfoCat.Model.IFileSystemRow
    IUpstreamFileSystem --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamFileSystem --|> IUpstreamDbEntity

    class FsInfoCat.Model.ISymbolicNameRow

    class IUpstreamSymbolicNameRow
    IUpstreamSymbolicNameRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamSymbolicNameRow --|> FsInfoCat.Model.ISymbolicNameRow
    IUpstreamSymbolicNameRow --|> IUpstreamDbEntity

    class FsInfoCat.Model.ISymbolicNameListItem

    class IUpstreamSymbolicNameListItem
    IUpstreamSymbolicNameListItem --|> FsInfoCat.Model.ISymbolicNameListItem
    IUpstreamSymbolicNameListItem --|> IUpstreamSymbolicNameRow

    class FsInfoCat.Model.ISymbolicName

    class IUpstreamSymbolicName
    IUpstreamSymbolicName --|> FsInfoCat.Model.ISymbolicName
    IUpstreamSymbolicName --|> IUpstreamSymbolicNameRow

    class FsInfoCat.Model.IVolumeRow

    class IUpstreamVolumeRow
    IUpstreamVolumeRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamVolumeRow --|> FsInfoCat.Model.IVolumeRow
    IUpstreamVolumeRow --|> IUpstreamDbEntity

    class FsInfoCat.Model.IVolumeListItem

    class IUpstreamVolumeListItem
    IUpstreamVolumeListItem --|> FsInfoCat.Model.IVolumeListItem
    IUpstreamVolumeListItem --|> IUpstreamVolumeRow

    class FsInfoCat.Model.IVolumeListItemWithFileSystem

    class IUpstreamVolumeListItemWithFileSystem
    IUpstreamVolumeListItemWithFileSystem --|> FsInfoCat.Model.IVolumeListItemWithFileSystem
    IUpstreamVolumeListItemWithFileSystem --|> IUpstreamVolumeListItem

    class FsInfoCat.Model.IVolume

    class IUpstreamVolume
    IUpstreamVolume --|> FsInfoCat.Model.IVolume
    IUpstreamVolume --|> IUpstreamVolumeRow
```

## Action, Membership and Task Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class FsInfoCat.Model.IDbEntity

    class IUpstreamDbEntity
    IUpstreamDbEntity --|> FsInfoCat.Model.IDbEntity

    class IFileAction
    IFileAction --|> IUpstreamDbEntity

    class IGroupMembershipRow
    IGroupMembershipRow --|> IUpstreamDbEntity

    class IGroupMemberListItem
    IGroupMemberListItem --|> IGroupMembershipRow

    class IGroupMemberOfListItem
    IGroupMemberOfListItem --|> IGroupMembershipRow

    class IGroupMembership
    IGroupMembership --|> IGroupMembershipRow

    class IHostDeviceRow
    IHostDeviceRow --|> IUpstreamDbEntity

    class IHostDevice
    IHostDevice --|> IHostDeviceRow

    class IHostDeviceListItem
    IHostDeviceListItem --|> IHostDeviceRow

    class IHostPlatformRow
    IHostPlatformRow --|> IUpstreamDbEntity

    class IHostPlatform
    IHostPlatform --|> IHostPlatformRow

    class IHostPlatformListItem
    IHostPlatformListItem --|> IHostPlatformRow

    class IMitigationTaskRow
    IMitigationTaskRow --|> IUpstreamDbEntity

    class IMitigationTask
    IMitigationTask --|> IMitigationTaskRow

    class IMitigationTaskListItem
    IMitigationTaskListItem --|> IMitigationTaskRow

    class ISubdirectoryActionRow
    ISubdirectoryActionRow --|> IUpstreamDbEntity

    class ISubdirectoryAction
    ISubdirectoryAction --|> ISubdirectoryActionRow

    class IUserGroupRow
    IUserGroupRow --|> IUpstreamDbEntity

    class IUserGroup
    IUserGroup --|> IUserGroupRow

    class IUserGroupListItem
    IUserGroupListItem --|> IUserGroupRow

    class IUserProfileRow
    IUserProfileRow --|> IUpstreamDbEntity

    class IUserProfile
    IUserProfile --|> IUserProfileRow

    class IUserProfileListItem
    IUserProfileListItem --|> IUserProfileRow
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

    class IUpstreamDbEntity
    IUpstreamDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.ICrawlConfigurationRow

    class FsInfoCat.Model.ICrawlSettings

    class IUpstreamCrawlConfigurationRow
    IUpstreamCrawlConfigurationRow --|> FsInfoCat.Model.ICrawlConfigurationRow
    IUpstreamCrawlConfigurationRow --|> FsInfoCat.Model.ICrawlSettings
    IUpstreamCrawlConfigurationRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamCrawlConfigurationRow --|> IUpstreamDbEntity

    class FsInfoCat.Model.ICrawlConfigurationListItem

    class IUpstreamCrawlConfigurationListItem
    IUpstreamCrawlConfigurationListItem --|> FsInfoCat.Model.ICrawlConfigurationListItem
    IUpstreamCrawlConfigurationListItem --|> FsInfoCat.Model.ICrawlConfigurationRow
    IUpstreamCrawlConfigurationListItem --|> FsInfoCat.Model.ICrawlSettings
    IUpstreamCrawlConfigurationListItem --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamCrawlConfigurationListItem --|> IUpstreamDbEntity

    class FsInfoCat.Model.ICrawlConfiguration

    class IUpstreamCrawlConfiguration
    IUpstreamCrawlConfiguration --|> FsInfoCat.Model.ICrawlConfiguration
    IUpstreamCrawlConfiguration --|> FsInfoCat.Model.ICrawlConfigurationRow
    IUpstreamCrawlConfiguration --|> FsInfoCat.Model.ICrawlSettings
    IUpstreamCrawlConfiguration --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamCrawlConfiguration --|> IUpstreamDbEntity

    class FsInfoCat.Model.ICrawlConfigReportItem

    class IUpstreamCrawlConfigReportItem
    IUpstreamCrawlConfigReportItem --|> FsInfoCat.Model.ICrawlConfigReportItem
    IUpstreamCrawlConfigReportItem --|> IUpstreamCrawlConfigurationListItem

    class FsInfoCat.Model.ICrawlJobLogRow

    class IUpstreamCrawlJobLogRow
    IUpstreamCrawlJobLogRow --|> FsInfoCat.Model.ICrawlJobLogRow
    IUpstreamCrawlJobLogRow --|> FsInfoCat.Model.ICrawlSettings
    IUpstreamCrawlJobLogRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamCrawlJobLogRow --|> IUpstreamDbEntity

    class IUpstreamCrawlJobListItem
    IUpstreamCrawlJobListItem --|> FsInfoCat.Model.ICrawlJobListItem
    IUpstreamCrawlJobListItem --|> IUpstreamCrawlJobLogRow

    class FsInfoCat.Model.ICrawlJobLog

    class IUpstreamCrawlJobLog
    IUpstreamCrawlJobLog --|> FsInfoCat.Model.ICrawlJobLog
    IUpstreamCrawlJobLog --|> FsInfoCat.Model.ICrawlJobLogRow
    IUpstreamCrawlJobLog --|> FsInfoCat.Model.ICrawlSettings
    IUpstreamCrawlJobLog --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamCrawlJobLog --|> IUpstreamDbEntity

    class FsInfoCat.Model.ICrawlJobListItem
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

    class IUpstreamDbEntity
    IUpstreamDbEntity --|> FsInfoCat.Model.IDbEntity

    class FsInfoCat.Model.IAccessError

    class FsInfoCat.Model.IHasSimpleIdentifier

    class IUpstreamAccessError
    IUpstreamAccessError --|> FsInfoCat.Model.IAccessError
    IUpstreamAccessError --|> FsInfoCat.Model.IDbEntity
    IUpstreamAccessError --|> FsInfoCat.Model.IHasSimpleIdentifier

    class FsInfoCat.Model.IFileAccessError

    class IUpstreamFileAccessError
    IUpstreamFileAccessError --|> FsInfoCat.Model.IFileAccessError
    IUpstreamFileAccessError --|> IUpstreamAccessError

    class FsInfoCat.Model.ISubdirectoryAccessError

    class IUpstreamSubdirectoryAccessError
    IUpstreamSubdirectoryAccessError --|> FsInfoCat.Model.ISubdirectoryAccessError
    IUpstreamSubdirectoryAccessError --|> IUpstreamAccessError

    class FsInfoCat.Model.IVolumeAccessError

    class IUpstreamVolumeAccessError
    IUpstreamVolumeAccessError --|> FsInfoCat.Model.IVolumeAccessError
    IUpstreamVolumeAccessError --|> IUpstreamAccessError

    class FsInfoCat.Model.IBinaryPropertySet

    class IUpstreamBinaryPropertySet
    IUpstreamBinaryPropertySet --|> FsInfoCat.Model.IBinaryPropertySet
    IUpstreamBinaryPropertySet --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamBinaryPropertySet --|> IUpstreamDbEntity

    class FsInfoCat.Model.IComparison

    class FsInfoCat.Model.IHasCompoundIdentifier

    class FsInfoCat.Model.IHasIdentifierPair

    class FsInfoCat.Model.IHasMembershipKeyReference

    class IUpstreamComparison
    IUpstreamComparison --|> FsInfoCat.Model.IComparison
    IUpstreamComparison --|> FsInfoCat.Model.IHasCompoundIdentifier
    IUpstreamComparison --|> FsInfoCat.Model.IHasIdentifierPair
    IUpstreamComparison --|> FsInfoCat.Model.IHasMembershipKeyReference
    IUpstreamComparison --|> IUpstreamDbEntity

    class FsInfoCat.Model.IRedundancy

    class IUpstreamRedundancy
    IUpstreamRedundancy --|> FsInfoCat.Model.IRedundancy
    IUpstreamRedundancy --|> FsInfoCat.Model.IHasCompoundIdentifier
    IUpstreamRedundancy --|> FsInfoCat.Model.IHasIdentifierPair
    IUpstreamRedundancy --|> FsInfoCat.Model.IHasMembershipKeyReference
    IUpstreamRedundancy --|> IUpstreamDbEntity

    class FsInfoCat.Model.IRedundantSetRow

    class IUpstreamRedundantSetRow
    IUpstreamRedundantSetRow --|> FsInfoCat.Model.IHasSimpleIdentifier
    IUpstreamRedundantSetRow --|> FsInfoCat.Model.IRedundantSetRow
    IUpstreamRedundantSetRow --|> IUpstreamDbEntity

    class FsInfoCat.Model.IRedundantSetListItem

    class IUpstreamRedundantSetListItem
    IUpstreamRedundantSetListItem --|> FsInfoCat.Model.IRedundantSetListItem
    IUpstreamRedundantSetListItem --|> IUpstreamRedundantSetRow

    class FsInfoCat.Model.IRedundantSet

    class IUpstreamRedundantSet
    IUpstreamRedundantSet --|> FsInfoCat.Model.IRedundantSet
    IUpstreamRedundantSet --|> IUpstreamRedundantSetRow
```
