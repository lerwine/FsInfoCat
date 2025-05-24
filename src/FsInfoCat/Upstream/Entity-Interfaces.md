# Upstream Entity Interfaces

- [File Properties Interfaces](#file-properties-interfaces)
- [Tag Interfaces](#tag-interfaces)
- [File System Interfaces](#file-system-interfaces)
- [Action, Membership and Task Interfaces](#action-membership-and-task-interfaces)
- [Crawl Interfaces](#crawl-interfaces)
- [Other Interfaces](#other-interfaces)

See Also:

- [Base Entity Interfaces](../Base-Entity-Interfaces.md)
- [Local Entity Interfaces](Local/Entity-Interfaces.md)

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

- **[FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)** : [FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs), [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
  - **[IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
    - **[IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)** : [FsInfoCat.Model.IPropertiesListItem](../Model/IPropertiesListItem.cs)
    - **[IUpstreamPropertySet](Model/IUpstreamPropertySet.cs)** : [FsInfoCat.Model.IPropertySet](../Model/IPropertySet.cs)
- **[FsInfoCat.Model.ISummaryProperties](../Model/ISummaryProperties.cs)**
  - **[FsInfoCat.Model.ISummaryPropertiesRow](../Model/ISummaryPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[IUpstreamSummaryPropertiesRow](Model/IUpstreamSummaryPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamSummaryPropertiesListItem](Model/IUpstreamSummaryPropertiesListItem.cs)** : [FsInfoCat.Model.ISummaryPropertiesListItem](../Model/ISummaryPropertiesListItem.cs), [FsInfoCat.Model.ISummaryPropertiesListItem](../Model/ISummaryPropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamSummaryPropertySet](Model/IUpstreamSummaryPropertySet.cs)** : [FsInfoCat.Model.ISummaryPropertySet](../Model/ISummaryPropertySet.cs), [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.ISummaryPropertySet](../Model/ISummaryPropertySet.cs)
- **[FsInfoCat.Model.IAudioProperties](../Model/IAudioProperties.cs)**
  - **[FsInfoCat.Model.IAudioPropertiesRow](../Model/IAudioPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IAudioPropertiesListItem](../Model/IAudioPropertiesListItem.cs)**
    - **[FsInfoCat.Model.IAudioPropertySet](../Model/IAudioPropertySet.cs)**
    - **[IUpstreamAudioPropertiesRow](Model/IUpstreamAudioPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamAudioPropertiesListItem](Model/IUpstreamAudioPropertiesListItem.cs)** : [FsInfoCat.Model.IAudioPropertiesListItem](../Model/IAudioPropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamAudioPropertySet](Model/IUpstreamAudioPropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IAudioPropertySet](../Model/IAudioPropertySet.cs)
- **[FsInfoCat.Model.IDocumentProperties](../Model/IDocumentProperties.cs)**
  - **[FsInfoCat.Model.IDocumentPropertiesRow](../Model/IDocumentPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IDocumentPropertiesListItem](../Model/IDocumentPropertiesListItem.cs)**
    - **[FsInfoCat.Model.IDocumentPropertySet](../Model/IDocumentPropertySet.cs)**
    - **[IUpstreamDocumentPropertiesRow](Model/IUpstreamDocumentPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamDocumentPropertiesListItem](Model/IUpstreamDocumentPropertiesListItem.cs)** : [FsInfoCat.Model.IDocumentPropertiesListItem](../Model/IDocumentPropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamDocumentPropertySet](Model/IUpstreamDocumentPropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IDocumentPropertySet](../Model/IDocumentPropertySet.cs)
- **[FsInfoCat.Model.IDRMProperties](../Model/IDRMProperties.cs)**
  - **[FsInfoCat.Model.IDRMPropertiesRow](../Model/IDRMPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IDRMPropertiesListItem](../Model/IDRMPropertiesListItem.cs)**
    - **[FsInfoCat.Model.IDRMPropertySet](../Model/IDRMPropertySet.cs)**
    - **[IUpstreamDRMPropertiesRow](Model/IUpstreamDRMPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamDRMPropertiesListItem](Model/IUpstreamDRMPropertiesListItem.cs)** : [FsInfoCat.Model.IDRMPropertiesListItem](../Model/IDRMPropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamDRMPropertySet](Model/IUpstreamDRMPropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IDRMPropertySet](../Model/IDRMPropertySet.cs)
- **[FsInfoCat.Model.IGPSProperties](../Model/IGPSProperties.cs)**
  - **[FsInfoCat.Model.IGPSPropertiesRow](../Model/IGPSPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IGPSPropertiesListItem](../Model/IGPSPropertiesListItem.cs)**
    - **[FsInfoCat.Model.IGPSPropertySet](../Model/IGPSPropertySet.cs)**
    - **[IUpstreamGPSPropertiesRow](Model/IUpstreamGPSPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamGPSPropertiesListItem](Model/IUpstreamGPSPropertiesListItem.cs)** : [FsInfoCat.Model.IGPSPropertiesListItem](../Model/IGPSPropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamGPSPropertySet](Model/IUpstreamGPSPropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IGPSPropertySet](../Model/IGPSPropertySet.cs)
- **[FsInfoCat.Model.IImageProperties](../Model/IImageProperties.cs)**
  - **[FsInfoCat.Model.IImagePropertiesRow](../Model/IImagePropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IImagePropertiesListItem](../Model/IImagePropertiesListItem.cs)**
    - **[FsInfoCat.Model.IImagePropertySet](../Model/IImagePropertySet.cs)**
    - **[IUpstreamImagePropertiesRow](Model/IUpstreamImagePropertiesRow.cs)** :[IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamImagePropertiesListItem](Model/IUpstreamImagePropertiesListItem.cs)** : [FsInfoCat.Model.IImagePropertiesListItem](../Model/IImagePropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamImagePropertySet](Model/IUpstreamImagePropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IImagePropertySet](../Model/IImagePropertySet.cs)
- **[FsInfoCat.Model.IMediaProperties](../Model/IMediaProperties.cs)**
  - **[FsInfoCat.Model.IMediaPropertiesRow](../Model/IMediaPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IMediaPropertiesListItem](../Model/IMediaPropertiesListItem.cs)**
    - **[FsInfoCat.Model.IMediaPropertySet](../Model/IMediaPropertySet.cs)**
    - **[IUpstreamMediaPropertiesRow](Model/IUpstreamMediaPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamMediaPropertiesListItem](Model/IUpstreamMediaPropertiesListItem.cs)** : [FsInfoCat.Model.IMediaPropertiesListItem](../Model/IMediaPropertiesListItem.cs), [IUpstreamMediaPropertiesRow](Model/IUpstreamMediaPropertiesRow.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamMediaPropertySet](Model/IUpstreamMediaPropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IMediaPropertySet](../Model/IMediaPropertySet.cs), [IUpstreamMediaPropertiesRow](Model/IUpstreamMediaPropertiesRow.cs)
- **[FsInfoCat.Model.IMusicProperties](../Model/IMusicProperties.cs)**
  - **[FsInfoCat.Model.IMusicPropertiesRow](../Model/IMusicPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IMusicPropertiesListItem](../Model/IMusicPropertiesListItem.cs)**
    - **[FsInfoCat.Model.IMusicPropertySet](../Model/IMusicPropertySet.cs)**
    - **[IUpstreamMusicPropertiesRow](Model/IUpstreamMusicPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamMusicPropertiesListItem](Model/IUpstreamMusicPropertiesListItem.cs)** : [FsInfoCat.Model.IMusicPropertiesListItem](../Model/IMusicPropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamMusicPropertySet](Model/IUpstreamMusicPropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IMusicPropertySet](../Model/IMusicPropertySet.cs)
- **[FsInfoCat.Model.IPhotoProperties](../Model/IPhotoProperties.cs)**
  - **[FsInfoCat.Model.IPhotoPropertiesRow](../Model/IPhotoPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IPhotoPropertiesListItem](../Model/IPhotoPropertiesListItem.cs)**
    - **[FsInfoCat.Model.IPhotoPropertySet](../Model/IPhotoPropertySet.cs)**
    - **[IUpstreamPhotoPropertiesRow](Model/IUpstreamPhotoPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamPhotoPropertiesListItem](Model/IUpstreamPhotoPropertiesListItem.cs)** : [FsInfoCat.Model.IPhotoProperties](../Model/IPhotoProperties.cs), [FsInfoCat.Model.IPhotoPropertiesListItem](../Model/IPhotoPropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamPhotoPropertySet](Model/IUpstreamPhotoPropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IPhotoPropertySet](../Model/IPhotoPropertySet.cs), [IUpstreamPhotoPropertiesRow](Model/IUpstreamPhotoPropertiesRow.cs)
- **[FsInfoCat.Model.IRecordedTVProperties](../Model/IRecordedTVProperties.cs)**
  - **[FsInfoCat.Model.IRecordedTVPropertiesRow](../Model/IRecordedTVPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IRecordedTVPropertiesListItem](../Model/IRecordedTVPropertiesListItem.cs)**
    - **[FsInfoCat.Model.IRecordedTVPropertySet](../Model/IRecordedTVPropertySet.cs)**
    - **[IUpstreamRecordedTVPropertiesRow](Model/IUpstreamRecordedTVPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamRecordedTVPropertiesListItem](Model/IUpstreamRecordedTVPropertiesListItem.cs)** : [FsInfoCat.Model.IRecordedTVPropertiesListItem](../Model/IRecordedTVPropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamRecordedTVPropertySet](Model/IUpstreamRecordedTVPropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IRecordedTVPropertySet](../Model/IRecordedTVPropertySet.cs)
- **[FsInfoCat.Model.IVideoProperties](../Model/IVideoProperties.cs)**
  - **[FsInfoCat.Model.IVideoPropertiesRow](../Model/IVideoPropertiesRow.cs)** : [FsInfoCat.Model.IPropertiesRow](../Model/IPropertiesRow.cs)
    - **[FsInfoCat.Model.IVideoPropertiesListItem](../Model/IVideoPropertiesListItem.cs)**
    - **[FsInfoCat.Model.IVideoPropertySet](../Model/IVideoPropertySet.cs)**
    - **[IUpstreamVideoPropertiesRow](Model/IUpstreamVideoPropertiesRow.cs)** : [IUpstreamPropertiesRow](Model/IUpstreamPropertiesRow.cs)
      - **[IUpstreamVideoPropertiesListItem](Model/IUpstreamVideoPropertiesListItem.cs)** : [FsInfoCat.Model.IVideoPropertiesListItem](../Model/IVideoPropertiesListItem.cs), [IUpstreamPropertiesListItem](Model/IUpstreamPropertiesListItem.cs)
      - **[IUpstreamVideoPropertySet](Model/IUpstreamVideoPropertySet.cs)** : [IUpstreamPropertySet](Model/IUpstreamPropertySet.cs), [FsInfoCat.Model.IVideoPropertySet](../Model/IVideoPropertySet.cs)

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

- **[FsInfoCat.Model.IItemTagRow](../Model/IItemTagRow.cs)** : [FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs), [FsInfoCat.Model.IHasIdentifierPair](../Model/IHasCompoundIdentifier.cs)
  - **[IUpstreamItemTagRow](Model/IUpstreamItemTagRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
    - **[IUpstreamItemTagListItem](Model/IUpstreamItemTagListItem.cs)** : [FsInfoCat.Model.IItemTagListItem](../Model/IItemTagListItem.cs)
    - **[IUpstreamItemTag](Model/IUpstreamItemTag.cs)** : [FsInfoCat.Model.IItemTag](../Model/IItemTag.cs)
      - **[IUpstreamSharedTag](Model/IUpstreamSharedTag.cs)** : [FsInfoCat.Model.ISharedTag](../Model/ISharedTag.cs)
        - **[IUpstreamSharedFileTag](Model/IUpstreamSharedFileTag.cs)** : [IUpstreamFileTag](Model/IUpstreamFileTag.cs), [FsInfoCat.Model.ISharedFileTag](../Model/ISharedFileTag.cs)
        - **[IUpstreamSharedSubdirectoryTag](Model/IUpstreamSharedSubdirectoryTag.cs)** : [IUpstreamSubdirectoryTag](Model/IUpstreamSubdirectoryTag.cs), [FsInfoCat.Model.ISharedSubdirectoryTag](../Model/ISharedSubdirectoryTag.cs)
        - **[IUpstreamSharedVolumeTag](Model/IUpstreamSharedVolumeTag.cs)** : [IUpstreamVolumeTag](Model/IUpstreamVolumeTag.cs), [FsInfoCat.Model.ISharedVolumeTag](../Model/ISharedVolumeTag.cs)
      - **[IUpstreamPersonalTag](Model/IUpstreamPersonalTag.cs)** : [FsInfoCat.Model.IPersonalTag](../Model/IPersonalTag.cs)
        - **[IUpstreamPersonalFileTag](Model/IUpstreamPersonalFileTag.cs)** : [IUpstreamFileTag](Model/IUpstreamFileTag.cs), [FsInfoCat.Model.IPersonalFileTag](../Model/IPersonalFileTag.cs)
        - **[IUpstreamPersonalSubdirectoryTag](Model/IUpstreamPersonalSubdirectoryTag.cs)** : [IUpstreamSubdirectoryTag](Model/IUpstreamSubdirectoryTag.cs), [FsInfoCat.Model.IPersonalSubdirectoryTag](../Model/IPersonalSubdirectoryTag.cs)
        - **[IUpstreamPersonalVolumeTag](Model/IUpstreamPersonalVolumeTag.cs)** : [IUpstreamVolumeTag](Model/IUpstreamVolumeTag.cs), [FsInfoCat.Model.IPersonalVolumeTag](../Model/IPersonalVolumeTag.cs)
      - **[IUpstreamFileTag](Model/IUpstreamFileTag.cs)** : [FsInfoCat.Model.IFileTag](../Model/IFileTag.cs), [FsInfoCat.Model.IHasMembershipKeyReference](../Model/IHasMembershipKeyReference.cs)
      - **[IUpstreamSubdirectoryTag](Model/IUpstreamSubdirectoryTag.cs)** : [FsInfoCat.Model.ISubdirectoryTag](../Model/ISubdirectoryTag.cs), [FsInfoCat.Model.IHasMembershipKeyReference](../Model/IHasMembershipKeyReference.cs)
      - **[IUpstreamVolumeTag](Model/IUpstreamVolumeTag.cs)** : [FsInfoCat.Model.IVolumeTag](../Model/IVolumeTag.cs), [FsInfoCat.Model.IHasMembershipKeyReference](../Model/IHasMembershipKeyReference.cs)
- **[FsInfoCat.Model.ITagDefinitionRow](../Model/ITagDefinitionRow.cs)** : [FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs), [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
  - **[IUpstreamTagDefinitionRow](Model/IUpstreamTagDefinitionRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
  - **[IUpstreamTagDefinitionListItem](Model/IUpstreamTagDefinitionListItem.cs)** : [FsInfoCat.Model.ITagDefinitionListItem](../Model/ITagDefinitionListItem.cs), [IUpstreamTagDefinitionRow](Model/IUpstreamTagDefinitionRow.cs)
  - **[IUpstreamTagDefinition](Model/IUpstreamTagDefinition.cs)** : [FsInfoCat.Model.ITagDefinition](../Model/ITagDefinition.cs), [IUpstreamTagDefinitionRow](Model/IUpstreamTagDefinitionRow.cs)
    - **[IUpstreamPersonalTagDefinition](Model/IUpstreamPersonalTagDefinition.cs)** : [FsInfoCat.Model.IPersonalTagDefinition](../Model/IPersonalTagDefinition.cs), [IUpstreamTagDefinition](Model/IUpstreamTagDefinition.cs)
    - **[IUpstreamSharedTagDefinition](Model/IUpstreamSharedTagDefinition.cs)** : [FsInfoCat.Model.ISharedTagDefinition](../Model/ISharedTagDefinition.cs), [IUpstreamTagDefinition](Model/IUpstreamTagDefinition.cs)

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

- **[FsInfoCat.Model.IVolumeRow](../Model/IVolumeRow.cs)** : [FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs), [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
  - **[IUpstreamVolumeRow](Model/IUpstreamVolumeRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
    - **[IUpstreamVolumeListItem](Model/IUpstreamVolumeListItem.cs)** : [FsInfoCat.Model.IVolumeListItem](../Model/IVolumeListItem.cs)
      - **[IUpstreamVolumeListItemWithFileSystem](Model/IUpstreamVolumeListItemWithFileSystem.cs)** : [FsInfoCat.Model.IVolumeListItemWithFileSystem](../Model/IVolumeListItemWithFileSystem.cs)
    - **[IUpstreamVolume](Model/IUpstreamVolume.cs)** : [FsInfoCat.Model.IVolume](../Model/IVolume.cs)
- **[FsInfoCat.Model.IFileSystemProperties](../Model/IFileSystemProperties.cs)**
  - **[FsInfoCat.Model.IFileSystemRow](../Model/IFileSystemRow.cs)** : [FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs), [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
    - **[IUpstreamFileSystemRow](Model/IUpstreamFileSystemRow.cs)** : [FsInfoCat.Model.IFileSystemRow](../Model/IFileSystemRow.cs), [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
      - **[IUpstreamFileSystemListItem](Model/IUpstreamFileSystemListItem.cs)** : [FsInfoCat.Model.IFileSystemListItem](../Model/IFileSystemListItem.cs)
      - **[IUpstreamFileSystem](Model/IUpstreamFileSystem.cs)** : [FsInfoCat.Model.IFileSystem](../Model/IFileSystem.cs), [FsInfoCat.Model.IFileSystemRow](../Model/IFileSystemRow.cs), [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
- **[FsInfoCat.Model.ISymbolicNameRow](../Model/ISymbolicNameRow.cs)** :[FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs),  [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
  - **[IUpstreamSymbolicNameRow](Model/IUpstreamSymbolicNameRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
    - **[IUpstreamSymbolicNameListItem](Model/IUpstreamSymbolicNameListItem.cs)** : [FsInfoCat.Model.ISymbolicNameListItem](../Model/ISymbolicNameListItem.cs)
    - **[IUpstreamSymbolicName](Model/IUpstreamSymbolicName.cs)** : [FsInfoCat.Model.ISymbolicName](../Model/ISymbolicName.cs)
- **[FsInfoCat.Model.IDbFsItemRow](../Model/IDbFsItemRow.cs)** : [FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs), [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
  - **[IUpstreamDbFsItemRow](Model/IUpstreamDbFsItemRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
    - **[IUpstreamDbFsItemListItem](Model/IUpstreamDbFsItemListItem.cs)** : [FsInfoCat.Model.IDbFsItemListItem](../Model/IDbFsItemListItem.cs)
      - **[IUpstreamDbFsItemListItemWithAncestorNames](Model/IUpstreamDbFsItemListItemWithAncestorNames.cs)** : [FsInfoCat.Model.IDbFsItemAncestorName](../Model/IDbFsItemAncestorName.cs), [FsInfoCat.Model.IDbFsItemListItemWithAncestorNames](../Model/IDbFsItemListItemWithAncestorNames.cs), [IUpstreamDbFsItemListItem](Model/IUpstreamDbFsItemListItem.cs)
    - **[IUpstreamDbFsItem](Model/IUpstreamDbFsItem.cs)** : [FsInfoCat.Model.IDbFsItem](../Model/IDbFsItem.cs)
    - **[IUpstreamFileRow](Model/IUpstreamFileRow.cs)** : [FsInfoCat.Model.IFileRow](../Model/IFileRow.cs)
      - **[IUpstreamFileListItemWithBinaryProperties](Model/IUpstreamFileListItemWithBinaryProperties.cs)** : [FsInfoCat.Model.IFileListItemWithBinaryProperties](../Model/IFileListItemWithBinaryProperties.cs), [IUpstreamDbFsItemListItem](Model/IUpstreamDbFsItemListItem.cs)
      - **[IUpstreamFileListItemWithAncestorNames](Model/IUpstreamFileListItemWithAncestorNames.cs)** : [FsInfoCat.Model.IDbFsItemAncestorName](../Model/IDbFsItemAncestorName.cs), [FsInfoCat.Model.IDbFsItemListItemWithAncestorNames](../Model/IDbFsItemListItemWithAncestorNames.cs), [FsInfoCat.Model.IFileAncestorName](../Model/IFileAncestorName.cs), [FsInfoCat.Model.IFileListItemWithAncestorNames](../Model/IFileListItemWithAncestorNames.cs), [IUpstreamDbFsItemListItem](Model/IUpstreamDbFsItemListItem.cs)
        - **[IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames](Model/IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames.cs)** : [FsInfoCat.Model.IFileListItemWithBinaryPropertiesAndAncestorNames](../Model/IFileListItemWithBinaryPropertiesAndAncestorNames.cs)
      - **[IUpstreamFile](Model/IUpstreamFile.cs)** : [FsInfoCat.Model.IFile](../Model/IFile.cs), [IUpstreamDbFsItem](Model/IUpstreamDbFsItem.cs)
    - **[IUpstreamSubdirectoryRow](Model/IUpstreamSubdirectoryRow.cs)** : [FsInfoCat.Model.ISubdirectoryRow](../Model/ISubdirectoryRow.cs)
      - **[IUpstreamSubdirectoryListItem](Model/IUpstreamSubdirectoryListItem.cs)** : [FsInfoCat.Model.ISubdirectoryListItem](../Model/ISubdirectoryListItem.cs), [FsInfoCat.Model.IDbFsItemListItem](../Model/IDbFsItemListItem.cs)
        - **[IUpstreamSubdirectoryListItemWithAncestorNames](Model/IUpstreamSubdirectoryListItemWithAncestorNames.cs)** : [FsInfoCat.Model.ISubdirectoryListItem](../Model/ISubdirectoryListItem.cs), [FsInfoCat.Model.ISubdirectoryAncestorName](../Model/ISubdirectoryAncestorName.cs), [FsInfoCat.Model.IDbFsItemAncestorName](../Model/IDbFsItemAncestorName.cs), [FsInfoCat.Model.IDbFsItemListItem](../Model/IDbFsItemListItem.cs), [FsInfoCat.Model.IDbFsItemListItemWithAncestorNames](../Model/IDbFsItemListItemWithAncestorNames.cs), [FsInfoCat.Model.ISubdirectoryListItemWithAncestorNames](../Model/ISubdirectoryListItemWithAncestorNames.cs)
      - **[IUpstreamSubdirectory](Model/IUpstreamSubdirectory.cs)** : [FsInfoCat.Model.ISubdirectory](../Model/ISubdirectory.cs), [FsInfoCat.Model.ISubdirectoryRow](../Model/ISubdirectoryRow.cs), [IUpstreamDbFsItem](Model/IUpstreamDbFsItem.cs)

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

- **[IFileAction](Model/IFileAction.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
- **[IGroupMembershipRow](Model/IGroupMembershipRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
  - **[IGroupMemberListItem](Model/IGroupMemberListItem.cs)** : [IGroupMembershipRow](Model/IGroupMembershipRow.cs)
  - **[IGroupMemberOfListItem](Model/IGroupMemberOfListItem.cs)** : [IGroupMembershipRow](Model/IGroupMembershipRow.cs)
  - **[IGroupMembership](Model/IGroupMembership.cs)** : [IGroupMembershipRow](Model/IGroupMembershipRow.cs)
- **[IHostDeviceRow](Model/IHostDeviceRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
  - **[IHostDeviceListItem](Model/IHostDeviceListItem.cs)** : [IHostDeviceRow](Model/IHostDeviceRow.cs)
  - **[IHostDevice](Model/IHostDevice.cs)** : [IHostDeviceRow](Model/IHostDeviceRow.cs)
- **[IHostPlatformRow](Model/IHostPlatformRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
  - **[IHostPlatformListItem](Model/IHostPlatformListItem.cs)** : [IHostPlatformRow](Model/IHostPlatformRow.cs)
  - **[IHostPlatform](Model/IHostPlatform.cs)** : [IHostPlatformRow](Model/IHostPlatformRow.cs)
- **[IMitigationTaskRow](Model/IMitigationTaskRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
  - **[IMitigationTaskListItem](Model/IMitigationTaskListItem.cs)** : [IMitigationTaskRow](Model/IMitigationTaskRow.cs)
  - **[IMitigationTask](Model/IMitigationTask.cs)** : [IMitigationTaskRow](Model/IMitigationTaskRow.cs)
- **[ISubdirectoryActionRow](Model/ISubdirectoryActionRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
  - **[ISubdirectoryAction](Model/ISubdirectoryAction.cs)** : [ISubdirectoryActionRow](Model/ISubdirectoryActionRow.cs)
- **[IUserGroupRow](Model/IUserGroupRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
  - **[IUserGroupListItem](Model/IUserGroupListItem.cs)** : [IUserGroupRow](Model/IUserGroupRow.cs)
  - **[IUserGroup](Model/IUserGroup.cs)** : [IUserGroupRow](Model/IUserGroupRow.cs)
- **[IUserProfileRow](Model/IUserProfileRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
  - **[IUserProfileListItem](Model/IUserProfileListItem.cs)** : [IUserProfileRow](Model/IUserProfileRow.cs)
  - **[IUserProfile](Model/IUserProfile.cs)** : [IUserProfileRow](Model/IUserProfileRow.cs)

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

- **[FsInfoCat.Model.ICrawlSettings](../Model/ICrawlSettings.cs)**
  - **[FsInfoCat.Model.ICrawlConfigurationRow](../Model/ICrawlConfigurationRow.cs)** : [FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs), [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
    - **[IUpstreamCrawlConfigurationRow](Model/IUpstreamCrawlConfigurationRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
      - **[IUpstreamCrawlConfigurationListItem](Model/IUpstreamCrawlConfigurationListItem.cs)** : [FsInfoCat.Model.ICrawlConfigurationListItem](../Model/ICrawlConfigurationListItem.cs), [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
        - **[IUpstreamCrawlConfigReportItem](Model/IUpstreamCrawlConfigReportItem.cs)** : [FsInfoCat.Model.ICrawlConfigReportItem](../Model/ICrawlConfigReportItem.cs)
      - **[IUpstreamCrawlConfiguration](Model/IUpstreamCrawlConfiguration.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
  - **[FsInfoCat.Model.ICrawlJobLogRow](../Model/ICrawlJobLogRow.cs)** : [FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs), [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
    - **[IUpstreamCrawlJobLogRow](Model/IUpstreamCrawlJobLogRow.cs)** : [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)
      - **[IUpstreamCrawlJobListItem](Model/IUpstreamCrawlJobListItem.cs)** : [FsInfoCat.Model.ICrawlJobListItem](../Model/ICrawlJobListItem.cs)
      - **[IUpstreamCrawlJobLog](Model/IUpstreamCrawlJobLog.cs)** : [FsInfoCat.Model.ICrawlJobLog](../Model/ICrawlJobLog.cs), [IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)

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

- **[FsInfoCat.Model.IDbEntity](../Model/IDbEntity.cs)**
  - **[IUpstreamDbEntity](Model/IUpstreamDbEntity.cs)**
    - **[FsInfoCat.Model.IAccessError](../Model/IAccessError.cs)** : [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
      - **[IUpstreamAccessError](Model/IUpstreamAccessError.cs)**
        - **[IUpstreamFileAccessError](Model/IUpstreamFileAccessError.cs)** : [FsInfoCat.Model.IFileAccessError](../Model/IFileAccessError.cs), [IUpstreamAccessError](Model/IUpstreamAccessError.cs)
        - **[IUpstreamSubdirectoryAccessError](Model/IUpstreamSubdirectoryAccessError.cs)** : [FsInfoCat.Model.ISubdirectoryAccessError](../Model/ISubdirectoryAccessError.cs), [IUpstreamAccessError](Model/IUpstreamAccessError.cs)
        - **[IUpstreamVolumeAccessError](Model/IUpstreamVolumeAccessError.cs)** : [FsInfoCat.Model.IVolumeAccessError](../Model/IVolumeAccessError.cs), [IUpstreamAccessError](Model/IUpstreamAccessError.cs)
    - **[FsInfoCat.Model.IBinaryPropertySet](../Model/IBinaryPropertySet.cs)** : [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
      - **[IUpstreamBinaryPropertySet](Model/IUpstreamBinaryPropertySet.cs)**
    - **[FsInfoCat.Model.IComparison](../Model/IComparison.cs)** : [FsInfoCat.Model.IHasMembershipKeyReference](../Model/IHasCompoundIdentifier.cs)
      - **[IUpstreamComparison](Model/IUpstreamComparison.cs)**
    - **[FsInfoCat.Model.IRedundancy](../Model/IRedundancy.cs)** : [FsInfoCat.Model.IHasMembershipKeyReference](../Model/IHasMembershipKeyReference.cs)
      - **[IUpstreamRedundancy](Model/IUpstreamRedundancy.cs)**
    - **[FsInfoCat.Model.IRedundantSetRow](../Model/IRedundantSetRow.cs)** : [FsInfoCat.Model.IHasSimpleIdentifier](../Model/IHasSimpleIdentifier.cs)
      - **[IUpstreamRedundantSetRow](Model/IUpstreamRedundantSetRow.cs)**
        - **[IUpstreamRedundantSetListItem](Model/IUpstreamRedundantSetListItem.cs)** : [FsInfoCat.Model.IRedundantSetListItem](../Model/IRedundantSetListItem.cs), [IUpstreamRedundantSetRow](Model/IUpstreamRedundantSetRow.cs)
        - **[IUpstreamRedundantSet](Model/IUpstreamRedundantSet.cs)** : [FsInfoCat.Model.IRedundantSet](../Model/IRedundantSet.cs), [IUpstreamRedundantSetRow](Model/IUpstreamRedundantSetRow.cs)
