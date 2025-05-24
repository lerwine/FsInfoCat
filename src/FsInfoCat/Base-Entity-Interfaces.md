# Base Entity Interfaces

- [Interface Model Equivalents](#interface-model-equivalents)
- [File Properties Interfaces](#file-properties-interfaces)
- [Tag Interfaces](#tag-interfaces)
- [File System Interfaces](#file-system-interfaces)
- [Crawl Interfaces](#crawl-interfaces)
- [Other Interfaces](#other-interfaces)

See Also:

- [Local Entity Interfaces](./Local/Entity-Interfaces.md)
- [Upstream Entity Interfaces](./Upstream/Entity-Interfaces.md)

## Interface Model Equivalents

| Base                                                                                                                            | Local                                                                                                                                                 | Upstream                                                                                                                                                          |
| ------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [FsInfoCat.Model.IDbEntity](Model/IDbEntity.cs)                                                                                 | [FsInfoCat.Local.Model.ILocalDbEntity](Local/Model/ILocalDbEntity.cs)                                                                                 | [FsInfoCat.Upstream.Model.IUpstreamDbEntity](Upstream/Model/IUpstreamDbEntity.cs)                                                                                 |
| [FsInfoCat.Model.IDbContext](Model/IDbContext.cs)                                                                               | [FsInfoCat.Local.Model.ILocalDbContext](Local/Model/ILocalDbContext.cs)                                                                               | [FsInfoCat.Upstream.Model.IUpstreamDbContext](Upstream/Model/IUpstreamDbContext.cs)                                                                               |
| [FsInfoCat.Model.IVolumeRow](Model/IVolumeRow.cs)                                                                               | [FsInfoCat.Local.Model.ILocalVolumeRow](Local/Model/ILocalVolumeRow.cs)                                                                               | [FsInfoCat.Upstream.Model.IUpstreamVolumeRow](Upstream/Model/IUpstreamVolumeRow.cs)                                                                               |
| [FsInfoCat.Model.IVolumeListItem](Model/IVolumeListItem.cs)                                                                     | [FsInfoCat.Local.Model.ILocalVolumeListItem](Local/Model/ILocalVolumeListItem.cs)                                                                     | [FsInfoCat.Upstream.Model.IUpstreamVolumeListItem](Upstream/Model/IUpstreamVolumeListItem.cs)                                                                     |
| [FsInfoCat.Model.IVolumeListItemWithFileSystem](Model/IVolumeListItemWithFileSystem.cs)                                         | [FsInfoCat.Local.Model.ILocalVolumeListItemWithFileSystem](Local/Model/ILocalVolumeListItemWithFileSystem.cs)                                         | [FsInfoCat.Upstream.Model.IUpstreamVolumeListItemWithFileSystem](Upstream/Model/IUpstreamVolumeListItemWithFileSystem.cs)                                         |
| [FsInfoCat.Model.IVolume](Model/IVolume.cs)                                                                                     | [FsInfoCat.Local.Model.ILocalVolume](Local/Model/ILocalVolume.cs)                                                                                     | [FsInfoCat.Upstream.Model.IUpstreamVolume](Upstream/Model/IUpstreamVolume.cs)                                                                                     |
| [FsInfoCat.Model.IFileSystemRow](Model/IFileSystemRow.cs)                                                                       | [FsInfoCat.Local.Model.ILocalFileSystemRow](Local/Model/ILocalFileSystemRow.cs)                                                                       | [FsInfoCat.Upstream.Model.IUpstreamFileSystemRow](Upstream/Model/IUpstreamFileSystemRow.cs)                                                                       |
| [FsInfoCat.Model.IFileSystemListItem](Model/IFileSystemListItem.cs)                                                             | [FsInfoCat.Local.Model.ILocalFileSystemListItem](Local/Model/ILocalFileSystemListItem.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamFileSystemListItem](Upstream/Model/IUpstreamFileSystemListItem.cs)                                                             |
| [FsInfoCat.Model.IFileSystem](Model/IFileSystem.cs)                                                                             | [FsInfoCat.Local.Model.ILocalFileSystem](Local/Model/ILocalFileSystem.cs)                                                                             | [FsInfoCat.Upstream.Model.IUpstreamFileSystem](Upstream/Model/IUpstreamFileSystem.cs)                                                                             |
| [FsInfoCat.Model.ISymbolicNameRow](Model/ISymbolicNameRow.cs)                                                                   | [FsInfoCat.Local.Model.ILocalSymbolicNameRow](Local/Model/ILocalSymbolicNameRow.cs)                                                                   | [FsInfoCat.Upstream.Model.IUpstreamSymbolicNameRow](Upstream/Model/IUpstreamSymbolicNameRow.cs)                                                                   |
| [FsInfoCat.Model.ISymbolicNameListItem](Model/ISymbolicNameListItem.cs)                                                         | [FsInfoCat.Local.Model.ILocalSymbolicNameListItem](Local/Model/ILocalSymbolicNameListItem.cs)                                                         | [FsInfoCat.Upstream.Model.IUpstreamSymbolicNameListItem](Upstream/Model/IUpstreamSymbolicNameListItem.cs)                                                         |
| [FsInfoCat.Model.ISymbolicName](Model/ISymbolicName.cs)                                                                         | [FsInfoCat.Local.Model.ILocalSymbolicName](Local/Model/ILocalSymbolicName.cs)                                                                         | [FsInfoCat.Upstream.Model.IUpstreamSymbolicName](Upstream/Model/IUpstreamSymbolicName.cs)                                                                         |
| [FsInfoCat.Model.IDbFsItemRow](Model/IDbFsItemRow.cs)                                                                           | [FsInfoCat.Local.Model.ILocalDbFsItemRow](Local/Model/ILocalDbFsItemRow.cs)                                                                           | [FsInfoCat.Upstream.Model.IUpstreamDbFsItemRow](Upstream/Model/IUpstreamDbFsItemRow.cs)                                                                           |
| [FsInfoCat.Model.IDbFsItemListItem](Model/IDbFsItemListItem.cs)                                                                 | [FsInfoCat.Local.Model.ILocalDbFsItemListItem](Local/Model/ILocalDbFsItemListItem.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamDbFsItemListItem](Upstream/Model/IUpstreamDbFsItemListItem.cs)                                                                 |
| [FsInfoCat.Model.IDbFsItemListItemWithAncestorNames](Model/IDbFsItemListItemWithAncestorNames.cs)                               | [FsInfoCat.Local.Model.ILocalDbFsItemListItemWithAncestorNames](Local/Model/ILocalDbFsItemListItemWithAncestorNames.cs)                               | [FsInfoCat.Upstream.Model.IUpstreamDbFsItemListItemWithAncestorNames](Upstream/Model/IUpstreamDbFsItemListItemWithAncestorNames.cs)                               |
| [FsInfoCat.Model.IDbFsItem](Model/IDbFsItem.cs)                                                                                 | [FsInfoCat.Local.Model.ILocalDbFsItem](Local/Model/ILocalDbFsItem.cs)                                                                                 | [FsInfoCat.Upstream.Model.IUpstreamDbFsItem](Upstream/Model/IUpstreamDbFsItem.cs)                                                                                 |
| [FsInfoCat.Model.IFileRow](Model/IFileRow.cs)                                                                                   | [FsInfoCat.Local.Model.ILocalFileRow](Local/Model/ILocalFileRow.cs)                                                                                   | [FsInfoCat.Upstream.Model.IUpstreamFileRow](Upstream/Model/IUpstreamFileRow.cs)                                                                                   |
| [FsInfoCat.Model.IFileListItemWithBinaryProperties](Model/IFileListItemWithBinaryProperties.cs)                                 | [FsInfoCat.Local.Model.ILocalFileListItemWithBinaryProperties](Local/Model/ILocalFileListItemWithBinaryProperties.cs)                                 | [FsInfoCat.Upstream.Model.IUpstreamFileListItemWithBinaryProperties](Upstream/Model/IUpstreamFileListItemWithBinaryProperties.cs)                                 |
| [FsInfoCat.Model.IFileListItemWithAncestorNames](Model/IFileListItemWithAncestorNames.cs)                                       | [FsInfoCat.Local.Model.ILocalFileListItemWithAncestorNames](Local/Model/ILocalFileListItemWithAncestorNames.cs)                                       | [FsInfoCat.Upstream.Model.IUpstreamFileListItemWithAncestorNames](Upstream/Model/IUpstreamFileListItemWithAncestorNames.cs)                                       |
| [FsInfoCat.Model.IFileListItemWithBinaryPropertiesAndAncestorNames](Model/IFileListItemWithBinaryPropertiesAndAncestorNames.cs) | [FsInfoCat.Local.Model.ILocalFileListItemWithBinaryPropertiesAndAncestorNames](Local/Model/ILocalFileListItemWithBinaryPropertiesAndAncestorNames.cs) | [FsInfoCat.Upstream.Model.IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames](Upstream/Model/IUpstreamFileListItemWithBinaryPropertiesAndAncestorNames.cs) |
| [FsInfoCat.Model.IFile](Model/IFile.cs)                                                                                         | [FsInfoCat.Local.Model.ILocalFile](Local/Model/ILocalFile.cs)                                                                                         | [FsInfoCat.Upstream.Model.IUpstreamFile](Upstream/Model/IUpstreamFile.cs)                                                                                         |
| [FsInfoCat.Model.ISubdirectoryRow](Model/ISubdirectoryRow.cs)                                                                   | [FsInfoCat.Local.Model.ILocalSubdirectoryRow](Local/Model/ILocalSubdirectoryRow.cs)                                                                   | [FsInfoCat.Upstream.Model.IUpstreamSubdirectoryRow](Upstream/Model/IUpstreamSubdirectoryRow.cs)                                                                   |
| [FsInfoCat.Model.ISubdirectoryListItem](Model/ISubdirectoryListItem.cs)                                                         | [FsInfoCat.Local.Model.ILocalSubdirectoryListItem](Local/Model/ILocalSubdirectoryListItem.cs)                                                         | [FsInfoCat.Upstream.Model.IUpstreamSubdirectoryListItem](Upstream/Model/IUpstreamSubdirectoryListItem.cs)                                                         |
| [FsInfoCat.Model.ISubdirectoryListItemWithAncestorNames](Model/ISubdirectoryListItemWithAncestorNames.cs)                       | [FsInfoCat.Local.Model.ILocalSubdirectoryListItemWithAncestorNames](Local/Model/ILocalSubdirectoryListItemWithAncestorNames.cs)                       | [FsInfoCat.Upstream.Model.IUpstreamSubdirectoryListItemWithAncestorNames](Upstream/Model/IUpstreamSubdirectoryListItemWithAncestorNames.cs)                       |
| [FsInfoCat.Model.ISubdirectory](Model/ISubdirectory.cs)                                                                         | [FsInfoCat.Local.Model.ILocalSubdirectory](Local/Model/ILocalSubdirectory.cs)                                                                         | [FsInfoCat.Upstream.Model.IUpstreamSubdirectory](Upstream/Model/IUpstreamSubdirectory.cs)                                                                         |
| [FsInfoCat.Model.IPropertiesRow](Model/IPropertiesRow.cs)                                                                       | [FsInfoCat.Local.Model.ILocalPropertiesRow](Local/Model/ILocalPropertiesRow.cs)                                                                       | [FsInfoCat.Upstream.Model.IUpstreamPropertiesRow](Upstream/Model/IUpstreamPropertiesRow.cs)                                                                       |
| [FsInfoCat.Model.IPropertiesListItem](Model/IPropertiesListItem.cs)                                                             | [FsInfoCat.Local.Model.ILocalPropertiesListItem](Local/Model/ILocalPropertiesListItem.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamPropertiesListItem](Upstream/Model/IUpstreamPropertiesListItem.cs)                                                             |
| [FsInfoCat.Model.IPropertySet](Model/IPropertySet.cs)                                                                           | [FsInfoCat.Local.Model.ILocalPropertySet](Local/Model/ILocalPropertySet.cs)                                                                           | [FsInfoCat.Upstream.Model.IUpstreamPropertySet](Upstream/Model/IUpstreamPropertySet.cs)                                                                           |
| [FsInfoCat.Model.ISummaryPropertiesRow](Model/ISummaryPropertiesRow.cs)                                                         | [FsInfoCat.Local.Model.ILocalSummaryPropertiesRow](Local/Model/ILocalSummaryPropertiesRow.cs)                                                         | [FsInfoCat.Upstream.Model.IUpstreamSummaryPropertiesRow](Upstream/Model/IUpstreamSummaryPropertiesRow.cs)                                                         |
| [FsInfoCat.Model.ISummaryPropertiesListItem](Model/ISummaryPropertiesListItem.cs)                                               | [FsInfoCat.Local.Model.ILocalSummaryPropertiesListItem](Local/Model/ILocalSummaryPropertiesListItem.cs)                                               | [FsInfoCat.Upstream.Model.IUpstreamSummaryPropertiesListItem](Upstream/Model/IUpstreamSummaryPropertiesListItem.cs)                                               |
| [FsInfoCat.Model.ISummaryPropertySet](Model/ISummaryPropertySet.cs)                                                             | [FsInfoCat.Local.Model.ILocalSummaryPropertySet](Local/Model/ILocalSummaryPropertySet.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamSummaryPropertySet](Upstream/Model/IUpstreamSummaryPropertySet.cs)                                                             |
| [FsInfoCat.Model.IAudioPropertiesRow](Model/IAudioPropertiesRow.cs)                                                             | [FsInfoCat.Local.Model.ILocalAudioPropertiesRow](Local/Model/ILocalAudioPropertiesRow.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamAudioPropertiesRow](Upstream/Model/IUpstreamAudioPropertiesRow.cs)                                                             |
| [FsInfoCat.Model.IAudioPropertiesListItem](Model/IAudioPropertiesListItem.cs)                                                   | [FsInfoCat.Local.Model.ILocalAudioPropertiesListItem](Local/Model/ILocalAudioPropertiesListItem.cs)                                                   | [FsInfoCat.Upstream.Model.IUpstreamAudioPropertiesListItem](Upstream/Model/IUpstreamAudioPropertiesListItem.cs)                                                   |
| [FsInfoCat.Model.IAudioPropertySet](Model/IAudioPropertySet.cs)                                                                 | [FsInfoCat.Local.Model.ILocalAudioPropertySet](Local/Model/ILocalAudioPropertySet.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamAudioPropertySet](Upstream/Model/IUpstreamAudioPropertySet.cs)                                                                 |
| [FsInfoCat.Model.IDocumentPropertiesRow](Model/IDocumentPropertiesRow.cs)                                                       | [FsInfoCat.Local.Model.ILocalDocumentPropertiesRow](Local/Model/ILocalDocumentPropertiesRow.cs)                                                       | [FsInfoCat.Upstream.Model.IUpstreamDocumentPropertiesRow](Upstream/Model/IUpstreamDocumentPropertiesRow.cs)                                                       |
| [FsInfoCat.Model.IDocumentPropertiesListItem](Model/IDocumentPropertiesListItem.cs)                                             | [FsInfoCat.Local.Model.ILocalDocumentPropertiesListItem](Local/Model/ILocalDocumentPropertiesListItem.cs)                                             | [FsInfoCat.Upstream.Model.IUpstreamDocumentPropertiesListItem](Upstream/Model/IUpstreamDocumentPropertiesListItem.cs)                                             |
| [FsInfoCat.Model.IDocumentPropertySet](Model/IDocumentPropertySet.cs)                                                           | [FsInfoCat.Local.Model.ILocalDocumentPropertySet](Local/Model/ILocalDocumentPropertySet.cs)                                                           | [FsInfoCat.Upstream.Model.IUpstreamDocumentPropertySet](Upstream/Model/IUpstreamDocumentPropertySet.cs)                                                           |
| [FsInfoCat.Model.IDRMPropertiesRow](Model/IDRMPropertiesRow.cs)                                                                 | [FsInfoCat.Local.Model.ILocalDRMPropertiesRow](Local/Model/ILocalDRMPropertiesRow.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamDRMPropertiesRow](Upstream/Model/IUpstreamDRMPropertiesRow.cs)                                                                 |
| [FsInfoCat.Model.IDRMPropertiesListItem](Model/IDRMPropertiesListItem.cs)                                                       | [FsInfoCat.Local.Model.ILocalDRMPropertiesListItem](Local/Model/ILocalDRMPropertiesListItem.cs)                                                       | [FsInfoCat.Upstream.Model.IUpstreamDRMPropertiesListItem](Upstream/Model/IUpstreamDRMPropertiesListItem.cs)                                                       |
| [FsInfoCat.Model.IDRMPropertySet](Model/IDRMPropertySet.cs)                                                                     | [FsInfoCat.Local.Model.ILocalDRMPropertySet](Local/Model/ILocalDRMPropertySet.cs)                                                                     | [FsInfoCat.Upstream.Model.IUpstreamDRMPropertySet](Upstream/Model/IUpstreamDRMPropertySet.cs)                                                                     |
| [FsInfoCat.Model.IGPSPropertiesRow](Model/IGPSPropertiesRow.cs)                                                                 | [FsInfoCat.Local.Model.ILocalGPSPropertiesRow](Local/Model/ILocalGPSPropertiesRow.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamGPSPropertiesRow](Upstream/Model/IUpstreamGPSPropertiesRow.cs)                                                                 |
| [FsInfoCat.Model.IGPSPropertiesListItem](Model/IGPSPropertiesListItem.cs)                                                       | [FsInfoCat.Local.Model.ILocalGPSPropertiesListItem](Local/Model/ILocalGPSPropertiesListItem.cs)                                                       | [FsInfoCat.Upstream.Model.IUpstreamGPSPropertiesListItem](Upstream/Model/IUpstreamGPSPropertiesListItem.cs)                                                       |
| [FsInfoCat.Model.IGPSPropertySet](Model/IGPSPropertySet.cs)                                                                     | [FsInfoCat.Local.Model.ILocalGPSPropertySet](Local/Model/ILocalGPSPropertySet.cs)                                                                     | [FsInfoCat.Upstream.Model.IUpstreamGPSPropertySet](Upstream/Model/IUpstreamGPSPropertySet.cs)                                                                     |
| [FsInfoCat.Model.IImagePropertiesRow](Model/IImagePropertiesRow.cs)                                                             | [FsInfoCat.Local.Model.ILocalImagePropertiesRow](Local/Model/ILocalImagePropertiesRow.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamImagePropertiesRow](Upstream/Model/IUpstreamImagePropertiesRow.cs)                                                             |
| [FsInfoCat.Model.IImagePropertiesListItem](Model/IImagePropertiesListItem.cs)                                                   | [FsInfoCat.Local.Model.ILocalImagePropertiesListItem](Local/Model/ILocalImagePropertiesListItem.cs)                                                   | [FsInfoCat.Upstream.Model.IUpstreamImagePropertiesListItem](Upstream/Model/IUpstreamImagePropertiesListItem.cs)                                                   |
| [FsInfoCat.Model.IImagePropertySet](Model/IImagePropertySet.cs)                                                                 | [FsInfoCat.Local.Model.ILocalImagePropertySet](Local/Model/ILocalImagePropertySet.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamImagePropertySet](Upstream/Model/IUpstreamImagePropertySet.cs)                                                                 |
| [FsInfoCat.Model.IMediaPropertiesRow](Model/IMediaPropertiesRow.cs)                                                             | [FsInfoCat.Local.Model.ILocalMediaPropertiesRow](Local/Model/ILocalMediaPropertiesRow.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamMediaPropertiesRow](Upstream/Model/IUpstreamMediaPropertiesRow.cs)                                                             |
| [FsInfoCat.Model.IMediaPropertiesListItem](Model/IMediaPropertiesListItem.cs)                                                   | [FsInfoCat.Local.Model.ILocalMediaPropertiesListItem](Local/Model/ILocalMediaPropertiesListItem.cs)                                                   | [FsInfoCat.Upstream.Model.IUpstreamMediaPropertiesListItem](Upstream/Model/IUpstreamMediaPropertiesListItem.cs)                                                   |
| [FsInfoCat.Model.IMediaPropertySet](Model/IMediaPropertySet.cs)                                                                 | [FsInfoCat.Local.Model.ILocalMediaPropertySet](Local/Model/ILocalMediaPropertySet.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamMediaPropertySet](Upstream/Model/IUpstreamMediaPropertySet.cs)                                                                 |
| [FsInfoCat.Model.IMusicPropertiesRow](Model/IMusicPropertiesRow.cs)                                                             | [FsInfoCat.Local.Model.ILocalMusicPropertiesRow](Local/Model/ILocalMusicPropertiesRow.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamMusicPropertiesRow](Upstream/Model/IUpstreamMusicPropertiesRow.cs)                                                             |
| [FsInfoCat.Model.IMusicPropertiesListItem](Model/IMusicPropertiesListItem.cs)                                                   | [FsInfoCat.Local.Model.ILocalMusicPropertiesListItem](Local/Model/ILocalMusicPropertiesListItem.cs)                                                   | [FsInfoCat.Upstream.Model.IUpstreamMusicPropertiesListItem](Upstream/Model/IUpstreamMusicPropertiesListItem.cs)                                                   |
| [FsInfoCat.Model.IMusicPropertySet](Model/IMusicPropertySet.cs)                                                                 | [FsInfoCat.Local.Model.ILocalMusicPropertySet](Local/Model/ILocalMusicPropertySet.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamMusicPropertySet](Upstream/Model/IUpstreamMusicPropertySet.cs)                                                                 |
| [FsInfoCat.Model.IPhotoPropertiesRow](Model/IPhotoPropertiesRow.cs)                                                             | [FsInfoCat.Local.Model.ILocalPhotoPropertiesRow](Local/Model/ILocalPhotoPropertiesRow.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamPhotoPropertiesRow](Upstream/Model/IUpstreamPhotoPropertiesRow.cs)                                                             |
| [FsInfoCat.Model.IPhotoPropertiesListItem](Model/IPhotoPropertiesListItem.cs)                                                   | [FsInfoCat.Local.Model.ILocalPhotoPropertiesListItem](Local/Model/ILocalPhotoPropertiesListItem.cs)                                                   | [FsInfoCat.Upstream.Model.IUpstreamPhotoPropertiesListItem](Upstream/Model/IUpstreamPhotoPropertiesListItem.cs)                                                   |
| [FsInfoCat.Model.IPhotoPropertySet](Model/IPhotoPropertySet.cs)                                                                 | [FsInfoCat.Local.Model.ILocalPhotoPropertySet](Local/Model/ILocalPhotoPropertySet.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamPhotoPropertySet](Upstream/Model/IUpstreamPhotoPropertySet.cs)                                                                 |
| [FsInfoCat.Model.IRecordedTVPropertiesRow](Model/IRecordedTVPropertiesRow.cs)                                                   | [FsInfoCat.Local.Model.ILocalRecordedTVPropertiesRow](Local/Model/ILocalRecordedTVPropertiesRow.cs)                                                   | [FsInfoCat.Upstream.Model.IUpstreamRecordedTVPropertiesRow](Upstream/Model/IUpstreamRecordedTVPropertiesRow.cs)                                                   |
| [FsInfoCat.Model.IRecordedTVPropertiesListItem](Model/IRecordedTVPropertiesListItem.cs)                                         | [FsInfoCat.Local.Model.ILocalRecordedTVPropertiesListItem](Local/Model/ILocalRecordedTVPropertiesListItem.cs)                                         | [FsInfoCat.Upstream.Model.IUpstreamRecordedTVPropertiesListItem](Upstream/Model/IUpstreamRecordedTVPropertiesListItem.cs)                                         |
| [FsInfoCat.Model.IRecordedTVPropertySet](Model/IRecordedTVPropertySet.cs)                                                       | [FsInfoCat.Local.Model.ILocalRecordedTVPropertySet](Local/Model/ILocalRecordedTVPropertySet.cs)                                                       | [FsInfoCat.Upstream.Model.IUpstreamRecordedTVPropertySet](Upstream/Model/IUpstreamRecordedTVPropertySet.cs)                                                       |
| [FsInfoCat.Model.IVideoPropertiesRow](Model/IVideoPropertiesRow.cs)                                                             | [FsInfoCat.Local.Model.ILocalVideoPropertiesRow](Local/Model/ILocalVideoPropertiesRow.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamVideoPropertiesRow](Upstream/Model/IUpstreamVideoPropertiesRow.cs)                                                             |
| [FsInfoCat.Model.IVideoPropertiesListItem](Model/IVideoPropertiesListItem.cs)                                                   | [FsInfoCat.Local.Model.ILocalVideoPropertiesListItem](Local/Model/ILocalVideoPropertiesListItem.cs)                                                   | [FsInfoCat.Upstream.Model.IUpstreamVideoPropertiesListItem](Upstream/Model/IUpstreamVideoPropertiesListItem.cs)                                                   |
| [FsInfoCat.Model.IVideoPropertySet](Model/IVideoPropertySet.cs)                                                                 | [FsInfoCat.Local.Model.ILocalVideoPropertySet](Local/Model/ILocalVideoPropertySet.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamVideoPropertySet](Upstream/Model/IUpstreamVideoPropertySet.cs)                                                                 |
| [FsInfoCat.Model.IAccessError](Model/IAccessError.cs)                                                                           | [FsInfoCat.Local.Model.ILocalAccessError](Local/Model/ILocalAccessError.cs)                                                                           | [FsInfoCat.Upstream.Model.IUpstreamAccessError](Upstream/Model/IUpstreamAccessError.cs)                                                                           |
| [FsInfoCat.Model.IFileAccessError](Model/IFileAccessError.cs)                                                                   | [FsInfoCat.Local.Model.ILocalFileAccessError](Local/Model/ILocalFileAccessError.cs)                                                                   | [FsInfoCat.Upstream.Model.IUpstreamFileAccessError](Upstream/Model/IUpstreamFileAccessError.cs)                                                                   |
| [FsInfoCat.Model.ISubdirectoryAccessError](Model/ISubdirectoryAccessError.cs)                                                   | [FsInfoCat.Local.Model.ILocalSubdirectoryAccessError](Local/Model/ILocalSubdirectoryAccessError.cs)                                                   | [FsInfoCat.Upstream.Model.IUpstreamSubdirectoryAccessError](Upstream/Model/IUpstreamSubdirectoryAccessError.cs)                                                   |
| [FsInfoCat.Model.IVolumeAccessError](Model/IVolumeAccessError.cs)                                                               | [FsInfoCat.Local.Model.ILocalVolumeAccessError](Local/Model/ILocalVolumeAccessError.cs)                                                               | [FsInfoCat.Upstream.Model.IUpstreamVolumeAccessError](Upstream/Model/IUpstreamVolumeAccessError.cs)                                                               |
| [FsInfoCat.Model.ITagDefinitionRow](Model/ITagDefinitionRow.cs)                                                                 | [FsInfoCat.Local.Model.ILocalTagDefinitionRow](Local/Model/ILocalTagDefinitionRow.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamTagDefinitionRow](Upstream/Model/IUpstreamTagDefinitionRow.cs)                                                                 |
| [FsInfoCat.Model.ITagDefinitionListItem](Model/ITagDefinitionListItem.cs)                                                       | [FsInfoCat.Local.Model.ILocalTagDefinitionListItem](Local/Model/ILocalTagDefinitionListItem.cs)                                                       | [FsInfoCat.Upstream.Model.IUpstreamTagDefinitionListItem](Upstream/Model/IUpstreamTagDefinitionListItem.cs)                                                       |
| [FsInfoCat.Model.ITagDefinition](Model/ITagDefinition.cs)                                                                       | [FsInfoCat.Local.Model.ILocalTagDefinition](Local/Model/ILocalTagDefinition.cs)                                                                       | [FsInfoCat.Upstream.Model.IUpstreamTagDefinition](Upstream/Model/IUpstreamTagDefinition.cs)                                                                       |
| [FsInfoCat.Model.ISharedTagDefinition](Model/ISharedTagDefinition.cs)                                                           | [FsInfoCat.Local.Model.ILocalSharedTagDefinition](Local/Model/ILocalSharedTagDefinition.cs)                                                           | [FsInfoCat.Upstream.Model.IUpstreamSharedTagDefinition](Upstream/Model/IUpstreamSharedTagDefinition.cs)                                                           |
| [FsInfoCat.Model.IPersonalTagDefinition](Model/IPersonalTagDefinition.cs)                                                       | [FsInfoCat.Local.Model.ILocalPersonalTagDefinition](Local/Model/ILocalPersonalTagDefinition.cs)                                                       | [FsInfoCat.Upstream.Model.IUpstreamPersonalTagDefinition](Upstream/Model/IUpstreamPersonalTagDefinition.cs)                                                       |
| [FsInfoCat.Model.IItemTagRow](Model/IItemTagRow.cs)                                                                             | [FsInfoCat.Local.Model.ILocalItemTagRow](Local/Model/ILocalItemTagRow.cs)                                                                             | [FsInfoCat.Upstream.Model.IUpstreamItemTagRow](Upstream/Model/IUpstreamItemTagRow.cs)                                                                             |
| [FsInfoCat.Model.IItemTagListItem](Model/IItemTagListItem.cs)                                                                   | [FsInfoCat.Local.Model.ILocalItemTagListItem](Local/Model/ILocalItemTagListItem.cs)                                                                   | [FsInfoCat.Upstream.Model.IUpstreamItemTagListItem](Upstream/Model/IUpstreamItemTagListItem.cs)                                                                   |
| [FsInfoCat.Model.IItemTag](Model/IItemTag.cs)                                                                                   | [FsInfoCat.Local.Model.ILocalItemTag](Local/Model/ILocalItemTag.cs)                                                                                   | [FsInfoCat.Upstream.Model.IUpstreamItemTag](Upstream/Model/IUpstreamItemTag.cs)                                                                                   |
| [FsInfoCat.Model.ISharedTag](Model/ISharedTag.cs)                                                                               | [FsInfoCat.Local.Model.ILocalSharedTag](Local/Model/ILocalSharedTag.cs)                                                                               | [FsInfoCat.Upstream.Model.IUpstreamSharedTag](Upstream/Model/IUpstreamSharedTag.cs)                                                                               |
| [FsInfoCat.Model.IPersonalTag](Model/IPersonalTag.cs)                                                                           | [FsInfoCat.Local.Model.ILocalPersonalTag](Local/Model/ILocalPersonalTag.cs)                                                                           | [FsInfoCat.Upstream.Model.IUpstreamPersonalTag](Upstream/Model/IUpstreamPersonalTag.cs)                                                                           |
| [FsInfoCat.Model.IFileTag](Model/IFileTag.cs)                                                                                   | [FsInfoCat.Local.Model.ILocalFileTag](Local/Model/ILocalFileTag.cs)                                                                                   | [FsInfoCat.Upstream.Model.IUpstreamFileTag](Upstream/Model/IUpstreamFileTag.cs)                                                                                   |
| [FsInfoCat.Model.ISubdirectoryTag](Model/ISubdirectoryTag.cs)                                                                   | [FsInfoCat.Local.Model.ILocalSubdirectoryTag](Local/Model/ILocalSubdirectoryTag.cs)                                                                   | [FsInfoCat.Upstream.Model.IUpstreamSubdirectoryTag](Upstream/Model/IUpstreamSubdirectoryTag.cs)                                                                   |
| [FsInfoCat.Model.IVolumeTag](Model/IVolumeTag.cs)                                                                               | [FsInfoCat.Local.Model.ILocalVolumeTag](Local/Model/ILocalVolumeTag.cs)                                                                               | [FsInfoCat.Upstream.Model.IUpstreamVolumeTag](Upstream/Model/IUpstreamVolumeTag.cs)                                                                               |
| [FsInfoCat.Model.ISharedFileTag](Model/ISharedFileTag.cs)                                                                       | [FsInfoCat.Local.Model.ILocalSharedFileTag](Local/Model/ILocalSharedFileTag.cs)                                                                       | [FsInfoCat.Upstream.Model.IUpstreamSharedFileTag](Upstream/Model/IUpstreamSharedFileTag.cs)                                                                       |
| [FsInfoCat.Model.ISharedSubdirectoryTag](Model/ISharedSubdirectoryTag.cs)                                                       | [FsInfoCat.Local.Model.ILocalSharedSubdirectoryTag](Local/Model/ILocalSharedSubdirectoryTag.cs)                                                       | [FsInfoCat.Upstream.Model.IUpstreamSharedSubdirectoryTag](Upstream/Model/IUpstreamSharedSubdirectoryTag.cs)                                                       |
| [FsInfoCat.Model.ISharedVolumeTag](Model/ISharedVolumeTag.cs)                                                                   | [FsInfoCat.Local.Model.ILocalSharedVolumeTag](Local/Model/ILocalSharedVolumeTag.cs)                                                                   | [FsInfoCat.Upstream.Model.IUpstreamSharedVolumeTag](Upstream/Model/IUpstreamSharedVolumeTag.cs)                                                                   |
| [FsInfoCat.Model.IPersonalFileTag](Model/IPersonalFileTag.cs)                                                                   | [FsInfoCat.Local.Model.ILocalPersonalFileTag](Local/Model/ILocalPersonalFileTag.cs)                                                                   | [FsInfoCat.Upstream.Model.IUpstreamPersonalFileTag](Upstream/Model/IUpstreamPersonalFileTag.cs)                                                                   |
| [FsInfoCat.Model.IPersonalSubdirectoryTag](Model/IPersonalSubdirectoryTag.cs)                                                   | [FsInfoCat.Local.Model.ILocalPersonalSubdirectoryTag](Local/Model/ILocalPersonalSubdirectoryTag.cs)                                                   | [FsInfoCat.Upstream.Model.IUpstreamPersonalSubdirectoryTag](Upstream/Model/IUpstreamPersonalSubdirectoryTag.cs)                                                   |
| [FsInfoCat.Model.IPersonalVolumeTag](Model/IPersonalVolumeTag.cs)                                                               | [FsInfoCat.Local.Model.ILocalPersonalVolumeTag](Local/Model/ILocalPersonalVolumeTag.cs)                                                               | [FsInfoCat.Upstream.Model.IUpstreamPersonalVolumeTag](Upstream/Model/IUpstreamPersonalVolumeTag.cs)                                                               |
| [FsInfoCat.Model.ICrawlConfigurationRow](Model/ICrawlConfigurationRow.cs)                                                       | [FsInfoCat.Local.Model.ILocalCrawlConfigurationRow](Local/Model/ILocalCrawlConfigurationRow.cs)                                                       | [FsInfoCat.Upstream.Model.IUpstreamCrawlConfigurationRow](Upstream/Model/IUpstreamCrawlConfigurationRow.cs)                                                       |
| [FsInfoCat.Model.ICrawlConfigurationListItem](Model/ICrawlConfigurationListItem.cs)                                             | [FsInfoCat.Local.Model.ILocalCrawlConfigurationListItem](Local/Model/ILocalCrawlConfigurationListItem.cs)                                             | [FsInfoCat.Upstream.Model.IUpstreamCrawlConfigurationListItem](Upstream/Model/IUpstreamCrawlConfigurationListItem.cs)                                             |
| [FsInfoCat.Model.ICrawlConfigReportItem](Model/ICrawlConfigReportItem.cs)                                                       | [FsInfoCat.Local.Model.ILocalCrawlConfigReportItem](Local/Model/ILocalCrawlConfigReportItem.cs)                                                       | [FsInfoCat.Upstream.Model.IUpstreamCrawlConfigReportItem](Upstream/Model/IUpstreamCrawlConfigReportItem.cs)                                                       |
| [FsInfoCat.Model.ICrawlConfiguration](Model/ICrawlConfiguration.cs)                                                             | [FsInfoCat.Local.Model.ILocalCrawlConfiguration](Local/Model/ILocalCrawlConfiguration.cs)                                                             | [FsInfoCat.Upstream.Model.IUpstreamCrawlConfiguration](Upstream/Model/IUpstreamCrawlConfiguration.cs)                                                             |
| [FsInfoCat.Model.ICrawlJobLogRow](Model/ICrawlJobLogRow.cs)                                                                     | [FsInfoCat.Local.Model.ILocalCrawlJobLogRow](Local/Model/ILocalCrawlJobLogRow.cs)                                                                     | [FsInfoCat.Upstream.Model.IUpstreamCrawlJobLogRow](Upstream/Model/IUpstreamCrawlJobLogRow.cs)                                                                     |
| [FsInfoCat.Model.ICrawlJobListItem](Model/ICrawlJobListItem.cs)                                                                 | [FsInfoCat.Local.Model.ILocalCrawlJobListItem](Local/Model/ILocalCrawlJobListItem.cs)                                                                 | [FsInfoCat.Upstream.Model.IUpstreamCrawlJobListItem](Upstream/Model/IUpstreamCrawlJobListItem.cs)                                                                 |
| [FsInfoCat.Model.ICrawlJobLog](Model/ICrawlJobLog.cs)                                                                           | [FsInfoCat.Local.Model.ILocalCrawlJobLog](Local/Model/ILocalCrawlJobLog.cs)                                                                           | [FsInfoCat.Upstream.Model.IUpstreamCrawlJobLog](Upstream/Model/IUpstreamCrawlJobLog.cs)                                                                           |
| [FsInfoCat.Model.IBinaryPropertySet](Model/IBinaryPropertySet.cs)                                                               | [FsInfoCat.Local.Model.ILocalBinaryPropertySet](Local/Model/ILocalBinaryPropertySet.cs)                                                               | [FsInfoCat.Upstream.Model.IUpstreamBinaryPropertySet](Upstream/Model/IUpstreamBinaryPropertySet.cs)                                                               |
| [FsInfoCat.Model.IComparison](Model/IComparison.cs)                                                                             | [FsInfoCat.Local.Model.ILocalComparison](Local/Model/ILocalComparison.cs)                                                                             | [FsInfoCat.Upstream.Model.IUpstreamComparison](Upstream/Model/IUpstreamComparison.cs)                                                                             |
| [FsInfoCat.Model.IRedundancy](Model/IRedundancy.cs)                                                                             | [FsInfoCat.Local.Model.ILocalRedundancy](Local/Model/ILocalRedundancy.cs)                                                                             | [FsInfoCat.Upstream.Model.IUpstreamRedundancy](Upstream/Model/IUpstreamRedundancy.cs)                                                                             |
| [FsInfoCat.Model.IRedundantSetRow](Model/IRedundantSetRow.cs)                                                                   | [FsInfoCat.Local.Model.ILocalRedundantSetRow](Local/Model/ILocalRedundantSetRow.cs)                                                                   | [FsInfoCat.Upstream.Model.IUpstreamRedundantSetRow](Upstream/Model/IUpstreamRedundantSetRow.cs)                                                                   |
| [FsInfoCat.Model.IRedundantSetListItem](Model/IRedundantSetListItem.cs)                                                         | [FsInfoCat.Local.Model.ILocalRedundantSetListItem](Local/Model/ILocalRedundantSetListItem.cs)                                                         | [FsInfoCat.Upstream.Model.IUpstreamRedundantSetListItem](Upstream/Model/IUpstreamRedundantSetListItem.cs)                                                         |
| [FsInfoCat.Model.IRedundantSet](Model/IRedundantSet.cs)                                                                         | [FsInfoCat.Local.Model.ILocalRedundantSet](Local/Model/ILocalRedundantSet.cs)                                                                         | [FsInfoCat.Upstream.Model.IUpstreamRedundantSet](Upstream/Model/IUpstreamRedundantSet.cs)                                                                         |

## File Properties Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class IDbEntity

    class IPropertiesRow
    IPropertiesRow --|> IDbEntity

    class IPropertiesListItem
    IPropertiesListItem --|> IPropertiesRow

    class IPropertySet
    IPropertySet --|> IPropertiesRow

    class ISummaryProperties

    class ISummaryPropertiesRow
    ISummaryPropertiesRow --|> ISummaryProperties
    ISummaryPropertiesRow --|> IPropertiesRow

    class ISummaryPropertiesListItem
    ISummaryPropertiesListItem --|> IPropertiesListItem
    ISummaryPropertiesListItem --|> ISummaryPropertiesRow

    class ISummaryPropertySet
    ISummaryPropertySet --|> IPropertySet
    ISummaryPropertySet --|> ISummaryPropertiesRow

    class IAudioProperties

    class IAudioPropertiesRow
    IAudioPropertiesRow --|> IAudioProperties
    IAudioPropertiesRow --|> IPropertiesRow

    class IAudioPropertiesListItem
    IAudioPropertiesListItem --|> IPropertiesListItem
    IAudioPropertiesListItem --|> IAudioPropertiesRow

    class IAudioPropertySet
    IAudioPropertySet --|> IPropertySet
    IAudioPropertySet --|> IAudioPropertiesRow

    class IDocumentProperties

    class IDocumentPropertiesRow
    IDocumentPropertiesRow --|> IDocumentProperties
    IDocumentPropertiesRow --|> IPropertiesRow

    class IDocumentPropertiesListItem
    IDocumentPropertiesListItem --|> IPropertiesListItem
    IDocumentPropertiesListItem --|> IDocumentPropertiesRow

    class IDocumentPropertySet
    IDocumentPropertySet --|> IPropertySet
    IDocumentPropertySet --|> IDocumentPropertiesRow

    class IDRMProperties

    class IDRMPropertiesRow
    IDRMPropertiesRow --|> IDRMProperties
    IDRMPropertiesRow --|> IPropertiesRow

    class IDRMPropertiesListItem
    IDRMPropertiesListItem --|> IPropertiesListItem
    IDRMPropertiesListItem --|> IDRMPropertiesRow

    class IDRMPropertySet
    IDRMPropertySet --|> IPropertySet
    IDRMPropertySet --|> IDRMPropertiesRow

    class IGPSProperties

    class IGPSPropertiesRow
    IGPSPropertiesRow --|> IGPSProperties
    IGPSPropertiesRow --|> IPropertiesRow

    class IGPSPropertiesListItem
    IGPSPropertiesListItem --|> IPropertiesListItem
    IGPSPropertiesListItem --|> IGPSPropertiesRow

    class IGPSPropertySet
    IGPSPropertySet --|> IPropertySet
    IGPSPropertySet --|> IGPSPropertiesRow

    class IImageProperties

    class IImagePropertiesRow
    IImagePropertiesRow --|> IImageProperties
    IImagePropertiesRow --|> IPropertiesRow

    class IImagePropertiesListItem
    IImagePropertiesListItem --|> IPropertiesListItem
    IImagePropertiesListItem --|> IImagePropertiesRow

    class IImagePropertySet
    IImagePropertySet --|> IPropertySet
    IImagePropertySet --|> IImagePropertiesRow

    class IMediaProperties

    class IMediaPropertiesRow
    IMediaPropertiesRow --|> IMediaProperties
    IMediaPropertiesRow --|> IPropertiesRow

    class IMediaPropertiesListItem
    IMediaPropertiesListItem --|> IPropertiesListItem
    IMediaPropertiesListItem --|> IMediaPropertiesRow

    class IMediaPropertySet
    IMediaPropertySet --|> IPropertySet
    IMediaPropertySet --|> IMediaPropertiesRow

    class IMusicProperties

    class IMusicPropertiesRow
    IMusicPropertiesRow --|> IMusicProperties
    IMusicPropertiesRow --|> IPropertiesRow

    class IMusicPropertiesListItem
    IMusicPropertiesListItem --|> IPropertiesListItem
    IMusicPropertiesListItem --|> IMusicPropertiesRow

    class IMusicPropertySet
    IMusicPropertySet --|> IPropertySet
    IMusicPropertySet --|> IMusicPropertiesRow

    class IPhotoProperties

    class IPhotoPropertiesRow
    IPhotoPropertiesRow --|> IPhotoProperties
    IPhotoPropertiesRow --|> IPropertiesRow

    class IPhotoPropertiesListItem
    IPhotoPropertiesListItem --|> IPropertiesListItem
    IPhotoPropertiesListItem --|> IPhotoPropertiesRow

    class IPhotoPropertySet
    IPhotoPropertySet --|> IPropertySet
    IPhotoPropertySet --|> IPhotoPropertiesRow

    class IRecordedTVProperties

    class IRecordedTVPropertiesRow
    IRecordedTVPropertiesRow --|> IRecordedTVProperties
    IRecordedTVPropertiesRow --|> IPropertiesRow

    class IRecordedTVPropertiesListItem
    IRecordedTVPropertiesListItem --|> IPropertiesListItem
    IRecordedTVPropertiesListItem --|> IRecordedTVPropertiesRow

    class IRecordedTVPropertySet
    IRecordedTVPropertySet --|> IPropertySet
    IRecordedTVPropertySet --|> IRecordedTVPropertiesRow

    class IVideoProperties

    class IVideoPropertiesRow
    IVideoPropertiesRow --|> IVideoProperties
    IVideoPropertiesRow --|> IPropertiesRow

    class IVideoPropertiesListItem
    IVideoPropertiesListItem --|> IPropertiesListItem
    IVideoPropertiesListItem --|> IVideoPropertiesRow

    class IVideoPropertySet
    IVideoPropertySet --|> IPropertySet
    IVideoPropertySet --|> IVideoPropertiesRow
```

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
erDiagram
    IFile }o--o| IAudioPropertySet : Has
    IFile }o--o| IDRMPropertySet : Has
    IFile }o--o| IGPSPropertySet : Has
    IFile }o--o| IImagePropertySet : Has
    IFile }o--o| IMediaPropertySet : Has
    IFile }o--o| IMusicPropertySet : Has
    IFile }o--o| IPhotoPropertySet : Has
    IFile }o--o| IRecordedTVPropertySet : Has
    IFile }o--o| ISummaryPropertySet : Has
    IFile }o--o| IVideoPropertySet : Has
```

| Base Interface                                          | Row Interface                                                 | List Item Interface                                                     | Record Interface                                          |
| ------------------------------------------------------- | ------------------------------------------------------------- | ----------------------------------------------------------------------- | --------------------------------------------------------- |
| [IDbEntity](Model/IDbEntity.cs)                         | [IPropertiesRow](Model/IPropertiesRow.cs)                     | [IPropertiesListItem](Model/IPropertiesListItem.cs)                     | [IPropertySet](Model/IPropertySet.cs)                     |
| [ISummaryProperties](Model/ISummaryProperties.cs)       | [ISummaryPropertiesRow](Model/ISummaryPropertiesRow.cs)       | [ISummaryPropertiesListItem](Model/ISummaryPropertiesListItem.cs)       | [ISummaryPropertySet](Model/ISummaryPropertySet.cs)       |
| [IAudioProperties](Model/IAudioProperties.cs)           | [IAudioPropertiesRow](Model/IAudioPropertiesRow.cs)           | [IAudioPropertiesListItem](Model/IAudioPropertiesListItem.cs)           | [IAudioPropertySet](Model/IAudioPropertySet.cs)           |
| [IDRMProperties](Model/IDRMProperties.cs)               | [IDRMPropertiesRow](Model/IDRMPropertiesRow.cs)               | [IDRMPropertiesListItem](Model/IDRMPropertiesListItem.cs)               | [IDRMPropertySet](Model/IDRMPropertySet.cs)               |
| [IGPSProperties](Model/IGPSProperties.cs)               | [IGPSPropertiesRow](Model/IGPSPropertiesRow.cs)               | [IGPSPropertiesListItem](Model/IGPSPropertiesListItem.cs)               | [IGPSPropertySet](Model/IGPSPropertySet.cs)               |
| [IImageProperties](Model/IImageProperties.cs)           | [IImagePropertiesRow](Model/IImagePropertiesRow.cs)           | [IImagePropertiesListItem](Model/IImagePropertiesListItem.cs)           | [IImagePropertySet](Model/IImagePropertySet.cs)           |
| [IMediaProperties](Model/IMediaProperties.cs)           | [IMediaPropertiesRow](Model/IMediaPropertiesRow.cs)           | [IMediaPropertiesListItem](Model/IMediaPropertiesListItem.cs)           | [IMediaPropertySet](Model/IMediaPropertySet.cs)           |
| [IMusicProperties](Model/IMusicProperties.cs)           | [IMusicPropertiesRow](Model/IMusicPropertiesRow.cs)           | [IMusicPropertiesListItem](Model/IMusicPropertiesListItem.cs)           | [IMusicPropertySet](Model/IMusicPropertySet.cs)           |
| [IPhotoProperties](Model/IPhotoProperties.cs)           | [IPhotoPropertiesRow](Model/IPhotoPropertiesRow.cs)           | [IPhotoPropertiesListItem](Model/IPhotoPropertiesListItem.cs)           | [IPhotoPropertySet](Model/IPhotoPropertySet.cs)           |
| [IRecordedTVProperties](Model/IRecordedTVProperties.cs) | [IRecordedTVPropertiesRow](Model/IRecordedTVPropertiesRow.cs) | [IRecordedTVPropertiesListItem](Model/IRecordedTVPropertiesListItem.cs) | [IRecordedTVPropertySet](Model/IRecordedTVPropertySet.cs) |
| [IVideoProperties](Model/IVideoProperties.cs)           | [IVideoPropertiesRow](Model/IVideoPropertiesRow.cs)           | [IVideoPropertiesListItem](Model/IVideoPropertiesListItem.cs)           | [IVideoPropertySet](Model/IVideoPropertySet.cs)           |

- **[IPropertiesRow](Model/IPropertiesRow.cs)** : [IDbEntity](Model/IDbEntity.cs), [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
  - **[IPropertiesListItem](Model/IPropertiesListItem.cs)**
  - **[IPropertySet](Model/IPropertySet.cs)**
- **[ISummaryProperties](Model/ISummaryProperties.cs)**
  - **[ISummaryPropertiesRow](Model/ISummaryPropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[ISummaryPropertiesListItem](Model/ISummaryPropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[ISummaryPropertySet](Model/ISummaryPropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)
- **[IAudioProperties](Model/IAudioProperties.cs)**
  - **[IAudioPropertiesRow](Model/IAudioPropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[IAudioPropertiesListItem](Model/IAudioPropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[IAudioPropertySet](Model/IAudioPropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)
- **[IDRMProperties](Model/IDRMProperties.cs)**
  - **[IDRMPropertiesRow](Model/IDRMPropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[IDRMPropertiesListItem](Model/IDRMPropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[IDRMPropertySet](Model/IDRMPropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)
- **[IGPSProperties](Model/IGPSProperties.cs)**
  - **[IGPSPropertiesRow](Model/IGPSPropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[IGPSPropertiesListItem](Model/IGPSPropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[IGPSPropertySet](Model/IGPSPropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)
- **[IImageProperties](Model/IImageProperties.cs)**
  - **[IImagePropertiesRow](Model/IImagePropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[IImagePropertiesListItem](Model/IImagePropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[IImagePropertySet](Model/IImagePropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)
- **[IMediaProperties](Model/IMediaProperties.cs)**
  - **[IMediaPropertiesRow](Model/IMediaPropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[IMediaPropertiesListItem](Model/IMediaPropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[IMediaPropertySet](Model/IMediaPropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)
- **[IMusicProperties](Model/IMusicProperties.cs)**
  - **[IMusicPropertiesRow](Model/IMusicPropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[IMusicPropertiesListItem](Model/IMusicPropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[IMusicPropertySet](Model/IMusicPropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)
- **[IPhotoProperties](Model/IPhotoProperties.cs)**
  - **[IPhotoPropertiesRow](Model/IPhotoPropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[IPhotoPropertiesListItem](Model/IPhotoPropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[IPhotoPropertySet](Model/IPhotoPropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)
- **[IRecordedTVProperties](Model/IRecordedTVProperties.cs)**
  - **[IRecordedTVPropertiesRow](Model/IRecordedTVPropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[IRecordedTVPropertiesListItem](Model/IRecordedTVPropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[IRecordedTVPropertySet](Model/IRecordedTVPropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)
- **[IVideoProperties](Model/IVideoProperties.cs)**
  - **[IVideoPropertiesRow](Model/IVideoPropertiesRow.cs)** : [IPropertiesRow](Model/IPropertiesRow.cs)
    - **[IVideoPropertiesListItem](Model/IVideoPropertiesListItem.cs)** : [IPropertiesListItem](Model/IPropertiesListItem.cs)
    - **[IVideoPropertySet](Model/IVideoPropertySet.cs)** : [IPropertySet](Model/IPropertySet.cs)

## Tag Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class IDbEntity

    class IItemTagRow
    IItemTagRow --|> IDbEntity

    class IItemTagListItem
    IItemTagListItem --|> IItemTagRow

    class IItemTag
    IItemTag --|> IItemTagRow

    class ISharedTag
    ISharedTag --|> IItemTag

    class IPersonalTag
    IPersonalTag --|> IItemTag

    class IFileTag
    IFileTag --|> IItemTag

    class ISubdirectoryTag
    ISubdirectoryTag --|> IItemTag

    class IVolumeTag
    IVolumeTag --|> IItemTag

    class ISharedFileTag
    ISharedFileTag --|> ISharedTag
    ISharedFileTag --|> IFileTag

    class ISharedSubdirectoryTag
    ISharedSubdirectoryTag --|> ISharedTag
    ISharedSubdirectoryTag --|> ISubdirectoryTag

    class ISharedVolumeTag
    ISharedVolumeTag --|> ISharedTag
    ISharedVolumeTag --|> IVolumeTag

    class IPersonalFileTag
    IPersonalFileTag --|> IPersonalTag
    IPersonalFileTag --|> IFileTag

    class IPersonalSubdirectoryTag
    IPersonalSubdirectoryTag --|> IPersonalTag
    IPersonalSubdirectoryTag --|> ISubdirectoryTag

    class IPersonalVolumeTag
    IPersonalVolumeTag --|> IPersonalTag
    IPersonalVolumeTag --|> IVolumeTag

    class ITagDefinitionRow
    ITagDefinitionRow --|> IDbEntity

    class ITagDefinitionListItem
    ITagDefinitionListItem --|> ITagDefinitionRow

    class ITagDefinition
    ITagDefinition --|> ITagDefinitionRow

    class IPersonalTagDefinition
    IPersonalTagDefinition --|> ITagDefinition

    class ISharedTagDefinition
    ISharedTagDefinition --|> ITagDefinition
```

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
erDiagram
  direction RL
    ISharedFileTag }o--o| IFile : Tags
    ISharedFileTag }o--o| ISharedTagDefinition : Is
    
    ISharedSubdirectoryTag }o--o| IFile : Tags
    ISharedSubdirectoryTag }o--o| ISharedTagDefinition : Is
    
    ISharedVolumeTag }o--o| IFile : Tags
    ISharedVolumeTag }o--o| ISharedTagDefinition : Is
    
    IPersonalFileTag }o--o| IFile : Tags
    IPersonalFileTag }o--o| IPersonalTagDefinition : Is
    
    IPersonalSubdirectoryTag }o--o| IFile : Tags
    IPersonalSubdirectoryTag }o--o| IPersonalTagDefinition : Is
    
    IPersonalVolumeTag }o--o| IFile : Tags
    IPersonalVolumeTag }o--o| IPersonalTagDefinition : Is
```

| Row Interface                                   | List Item Interface                                       | Base Record Interface                     | Shared Record Interface                               | Personal Record Interface                                 |
| ----------------------------------------------- | --------------------------------------------------------- | ----------------------------------------- | ----------------------------------------------------- | --------------------------------------------------------- |
| [ITagDefinitionRow](Model/ITagDefinitionRow.cs) | [ITagDefinitionListItem](Model/ITagDefinitionListItem.cs) | [ITagDefinition](Model/ITagDefinition.cs) | [ISharedTagDefinition](Model/ISharedTagDefinition.cs) | [IPersonalTagDefinition](Model/IPersonalTagDefinition.cs) |
| [IItemTagRow](Model/IItemTagRow.cs)             | [IItemTagListItem](Model/IItemTagListItem.cs)             | [IItemTag](Model/IItemTag.cs)             | [ISharedTag](Model/ISharedTag.cs)                     | [IPersonalTag](Model/IPersonalTag.cs)                     |
| [IFileTag](Model/IFileTag.cs)                   |                                                           |                                           | [ISharedFileTag](Model/ISharedFileTag.cs)             |                                                           |

| Base Interface                                | Shared Tag Interface                                      | Personal Tag Interface                                        |
| --------------------------------------------- | --------------------------------------------------------- | ------------------------------------------------------------- |
| [IItemTag](Model/IItemTag.cs)                 | [ISharedTag](Model/ISharedTag.cs)                         | [IPersonalTag](Model/IPersonalTag.cs)                         |
| [IFileTag](Model/IFileTag.cs)                 | [ISharedFileTag](Model/ISharedFileTag.cs)                 | [IPersonalFileTag](Model/IPersonalFileTag.cs)                 |
| [ISubdirectoryTag](Model/ISubdirectoryTag.cs) | [ISharedSubdirectoryTag](Model/ISharedSubdirectoryTag.cs) | [IPersonalSubdirectoryTag](Model/IPersonalSubdirectoryTag.cs) |
| [IVolumeTag](Model/IVolumeTag.cs)             | [ISharedVolumeTag](Model/ISharedVolumeTag.cs)             | [IPersonalVolumeTag](Model/IPersonalVolumeTag.cs)             |

- **[IItemTagRow](Model/IItemTagRow.cs)** : [IDbEntity](Model/IDbEntity.cs), [IHasIdentifierPair](Model/IHasIdentifierPair.cs)
  - **[IItemTagListItem](Model/IItemTagListItem.cs)**
  - **[IItemTag](Model/IItemTag.cs)**
    - **[ISharedTag](Model/ISharedTag.cs)**
      - **[ISharedFileTag](Model/ISharedFileTag.cs)** : [IFileTag](Model/IFileTag.cs), [IHasMembershipKeyReference](Model/IHasMembershipKeyReference.cs)
      - **[ISharedSubdirectoryTag](Model/ISharedSubdirectoryTag.cs)** : [ISubdirectoryTag](Model/ISubdirectoryTag.cs), [IHasMembershipKeyReference](Model/IHasMembershipKeyReference.cs)
      - **[ISharedVolumeTag](Model/ISharedVolumeTag.cs)** : [IVolumeTag](Model/IVolumeTag.cs), [IHasMembershipKeyReference](Model/IHasMembershipKeyReference.cs)
    - **[IPersonalTag](Model/IPersonalTag.cs)**
      - **[IPersonalFileTag](Model/IPersonalFileTag.cs)** : [IFileTag](Model/IFileTag.cs), [IHasMembershipKeyReference](Model/IHasMembershipKeyReference.cs)
      - **[IPersonalSubdirectoryTag](Model/IPersonalSubdirectoryTag.cs)** : [ISubdirectoryTag](Model/ISubdirectoryTag.cs), [IHasMembershipKeyReference](Model/IHasMembershipKeyReference.cs)
      - **[IPersonalVolumeTag](Model/IPersonalVolumeTag.cs)** : [IVolumeTag](Model/IVolumeTag.cs), [IHasMembershipKeyReference](Model/IHasMembershipKeyReference.cs)
    - **[IFileTag](Model/IFileTag.cs)**
    - **[ISubdirectoryTag](Model/ISubdirectoryTag.cs)**
    - **[IVolumeTag](Model/IVolumeTag.cs)**
- **[ITagDefinitionRow](Model/ITagDefinitionRow.cs)** : [IDbEntity](Model/IDbEntity.cs), [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
  - **[ITagDefinitionListItem](Model/ITagDefinitionListItem.cs)**
  - **[ITagDefinition](Model/ITagDefinition.cs)**
    - **[IPersonalTagDefinition](Model/IPersonalTagDefinition.cs)**
    - **[ISharedTagDefinition](Model/ISharedTagDefinition.cs)**

## File System Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class IDbEntity

    class IVolumeRow
    IVolumeRow --|> IDbEntity

    class IVolumeListItem
    IVolumeListItem --|> IVolumeRow

    class IVolumeListItemWithFileSystem
    IVolumeListItemWithFileSystem --|> IVolumeListItem

    class IVolume
    IVolume --|> IVolumeRow

    class IFileSystemProperties

    class IFileSystemRow
    IFileSystemRow --|> IDbEntity
    IFileSystemRow --|> IFileSystemProperties

    class IFileSystemListItem
    IFileSystemListItem --|> IFileSystemRow

    class IFileSystem
    IFileSystem --|> IFileSystemRow

    class ISymbolicNameRow
    ISymbolicNameRow --|> IDbEntity

    class ISymbolicName
    ISymbolicName --|> ISymbolicNameRow

    class ISymbolicNameListItem
    ISymbolicNameListItem --|> ISymbolicNameRow

    class IDbFsItemRow
    IDbFsItemRow --|> IDbEntity

    class IDbFsItemListItem
    IDbFsItemListItem --|> IDbFsItemRow

    class IDbFsItemAncestorName

    class IDbFsItemListItemWithAncestorNames
    IDbFsItemListItemWithAncestorNames --|> IDbFsItemAncestorName
    IDbFsItemListItemWithAncestorNames --|> IDbFsItemListItem

    class IDbFsItem
    IDbFsItem --|> IDbFsItemRow

    class IFileRow
    IFileRow --|> IDbFsItemRow

    class IFileAncestorName
    IFileAncestorName --|> IDbFsItemAncestorName

    class IFileListItemWithAncestorNames
    IFileListItemWithAncestorNames --|> IDbFsItemListItemWithAncestorNames
    IFileListItemWithAncestorNames --|> IFileRow
    IFileListItemWithAncestorNames --|> IFileAncestorName

    class IFileListItemWithBinaryProperties
    IFileListItemWithBinaryProperties --|> IDbFsItemListItem
    IFileListItemWithBinaryProperties --|> IFileRow

    class IFileListItemWithBinaryPropertiesAndAncestorNames
    IFileListItemWithBinaryPropertiesAndAncestorNames --|> IFileListItemWithAncestorNames

    class IFile
    IFile --|> IDbFsItem
    IFile --|> IFileRow

    class ISubdirectoryRow
    ISubdirectoryRow --|> IDbFsItemRow

    class ISubdirectoryAncestorName
    ISubdirectoryAncestorName --|> IDbFsItemAncestorName

    class ISubdirectoryListItem
    ISubdirectoryListItem --|> IDbFsItemListItem
    ISubdirectoryListItem --|> ISubdirectoryRow

    class ISubdirectoryListItemWithAncestorNames
    ISubdirectoryListItemWithAncestorNames --|> ISubdirectoryAncestorName
    ISubdirectoryListItemWithAncestorNames --|> ISubdirectoryListItem
    ISubdirectoryListItemWithAncestorNames --|> IDbFsItemListItemWithAncestorNames

    class ISubdirectory
    ISubdirectory --|> IDbFsItem
    ISubdirectory --|> ISubdirectoryRow
```

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
erDiagram
  direction RL
    IVolume ||--o| ISubdirectory : Has
    IVolume ||--o{ IFileSystem : Uses
    IVolume ||--o{ IVolumeAccessError : Has
    IVolume ||--o{ IPersonalVolumeTag : Has
    IVolume ||--o{ ISharedVolumeTag : Has
    ISymbolicName }o--|| IFileSystem : Is
    IFile ||--o{ IComparison : Has
    IFile ||--o{ IFileAccessError : Has
    IFile ||--o{ IPersonalFileTag : Has
    IFile ||--o{ ISharedFileTag : Has
    IFile }o--o| ISubdirectory : Contained By
    ISubdirectory |o--o{ IFile : Contains
    ISubdirectory |o--o{ ISubdirectory : Contains
    ISubdirectory ||--o| ICrawlConfiguration : Has
    ISubdirectory ||--o{ IPersonalSubdirectoryTag : Has
    ISubdirectory ||--o{ ISharedSubdirectoryTag : Has
```

| Base Interface                                          | Row Interface                                 | List Item Interface(s)                                                                                               | Record Interface                        |
| ------------------------------------------------------- | --------------------------------------------- | -------------------------------------------------------------------------------------------------------------------- | --------------------------------------- |
| [IFileSystemProperties](Model/IFileSystemProperties.cs) | [IFileSystemRow](Model/IFileSystemRow.cs)     | [IFileSystemListItem](Model/IFileSystemListItem.cs)                                                                  | [IFileSystem](Model/IFileSystem.cs)     |
|                                                         | [ISymbolicNameRow](Model/ISymbolicNameRow.cs) | [ISymbolicNameListItem](Model/ISymbolicNameListItem.cs)                                                              | [ISymbolicName](Model/ISymbolicName.cs) |
|                                                         | [IVolumeRow](Model/IVolumeRow.cs)             | [IVolumeListItem](Model/IVolumeListItem.cs), [IVolumeListItemWithFileSystem](Model/IVolumeListItemWithFileSystem.cs) | [IVolume](Model/IVolume.cs)             |

| Row Interface                                 | List Item Interface(s)                                                                                                                                                                                                                                                      | Record Interface                        |
| --------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------- |
| [IDbFsItemRow](Model/IDbFsItemRow.cs)         | [IDbFsItemListItem](Model/IDbFsItemListItem.cs)                                                                                                                                                                                                                             | [IDbFsItem](Model/IDbFsItem.cs)         |
| [IFileRow](Model/IFileRow.cs)                 | [IFileListItemWithAncestorNames](Model/IFileListItemWithAncestorNames.cs), [IFileListItemWithBinaryProperties](Model/IFileListItemWithBinaryProperties.cs), [IFileListItemWithBinaryPropertiesAndAncestorNames](Model/IFileListItemWithBinaryPropertiesAndAncestorNames.cs) | [IFile](Model/IFile.cs)                 |
| [ISubdirectoryRow](Model/ISubdirectoryRow.cs) | [ISubdirectoryListItem](Model/ISubdirectoryListItem.cs), [ISubdirectoryListItemWithAncestorNames](Model/ISubdirectoryListItemWithAncestorNames.cs)                                                                                                                          | [ISubdirectory](Model/ISubdirectory.cs) |

| Row / Base Interface                                    | File Interface                                                                  | Subdirectory Interface                                                                    |
| ------------------------------------------------------- | ------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| [IDbFsItemRow](Model/IDbFsItemRow.cs)                   | [IFileRow](Model/IFileRow.cs)                                                   | [ISubdirectoryRow](Model/ISubdirectoryRow.cs)                                             |
| [IDbFsItemListItem](Model/IDbFsItemListItem.cs)         | [IFileListItemWithBinaryProperties](Model/IFileListItemWithBinaryProperties.cs) | [ISubdirectoryListItem](Model/ISubdirectoryListItem.cs)                                   |
| [IDbFsItemAncestorName](Model/IDbFsItemAncestorName.cs) | [IFileListItemWithAncestorNames](Model/IFileListItemWithAncestorNames.cs)       | [ISubdirectoryListItemWithAncestorNames](Model/ISubdirectoryListItemWithAncestorNames.cs) |
| [IDbFsItem](Model/IDbFsItem.cs)                         | [IFile](Model/IFile.cs)                                                         | [ISubdirectory](Model/ISubdirectory.cs)                                                   |

- **[IVolumeRow](Model/IVolumeRow.cs)** : [IDbEntity](Model/IDbEntity.cs), [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
  - **[IVolumeListItem](Model/IVolumeListItem.cs)**
    - **[IVolumeListItemWithFileSystem](Model/IVolumeListItemWithFileSystem.cs)**
  - **[IVolume](Model/IVolume.cs)**
- **[IFileSystemProperties](Model/IFileSystemProperties.cs)**
  - **[IFileSystemRow](Model/IFileSystemRow.cs)** : [IDbEntity](Model/IDbEntity.cs), [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
    - **[IFileSystemListItem](Model/IFileSystemListItem.cs)**
  - **[IFileSystem](Model/IFileSystem.cs)**
- **[ISymbolicNameRow](Model/ISymbolicNameRow.cs)** : [IDbEntity](Model/IDbEntity.cs), [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
  - **[ISymbolicNameListItem](Model/ISymbolicNameListItem.cs)**
  - **[ISymbolicName](Model/ISymbolicName.cs)**
- **[IDbFsItemAncestorName](Model/IDbFsItemAncestorName.cs)** : [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
  - **[IFileAncestorName](Model/IFileAncestorName.cs)**
  - **[ISubdirectoryAncestorName](Model/ISubdirectoryAncestorName.cs)**
- **[IDbFsItemRow](Model/IDbFsItemRow.cs)** : [IDbEntity](Model/IDbEntity.cs), [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
  - **[IDbFsItemListItem](Model/IDbFsItemListItem.cs)**
    - **[IDbFsItemListItemWithAncestorNames](Model/IDbFsItemListItemWithAncestorNames.cs)** : [IDbFsItemAncestorName](Model/IDbFsItemAncestorName.cs)
  - **[IDbFsItem](Model/IDbFsItem.cs)**
  - **[IFileRow](Model/IFileRow.cs)**
    - **[IFileListItemWithAncestorNames](Model/IFileListItemWithAncestorNames.cs)** : [IDbFsItemListItemWithAncestorNames](Model/IDbFsItemListItemWithAncestorNames.cs), [IFileAncestorName](Model/IFileAncestorName.cs)
      - **[IFileListItemWithBinaryPropertiesAndAncestorNames](Model/IFileListItemWithBinaryPropertiesAndAncestorNames.cs)**
    - **[IFileListItemWithBinaryProperties](Model/IFileListItemWithBinaryProperties.cs)** : [IDbFsItemListItem](Model/IDbFsItemListItem.cs)
    - **[IFile](Model/IFile.cs) : [IDbFsItem](Model/IDbFsItem.cs)**
  - **[ISubdirectoryRow](Model/ISubdirectoryRow.cs)**
    - **[ISubdirectoryListItem](Model/ISubdirectoryListItem.cs)** : [IDbFsItemListItem](Model/IDbFsItemListItem.cs)
      - **[ISubdirectoryListItemWithAncestorNames](Model/ISubdirectoryListItemWithAncestorNames.cs)** : [IDbFsItemListItemWithAncestorNames](Model/IDbFsItemListItemWithAncestorNames.cs), [ISubdirectoryAncestorName](Model/ISubdirectoryAncestorName.cs)
    - **[ISubdirectory](Model/ISubdirectory.cs)** : [IDbFsItem](Model/IDbFsItem.cs)

## Crawl Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class IDbEntity

    class ICrawlSettings

    class ICrawlConfigurationRow
    ICrawlConfigurationRow --|> IDbEntity
    ICrawlConfigurationRow --|> ICrawlSettings

    class ICrawlConfigurationListItem
    ICrawlConfigurationListItem --|> ICrawlConfigurationRow

    class ICrawlConfigReportItem
    ICrawlConfigReportItem --|> ICrawlConfigurationListItem

    class ICrawlConfiguration
    ICrawlConfiguration --|> ICrawlConfigurationRow

    class ICrawlJobLogRow
    ICrawlJobLogRow --|> IDbEntity
    ICrawlJobLogRow --|> ICrawlSettings

    class ICrawlJobListItem
    ICrawlJobListItem --|> ICrawlJobLogRow

    class ICrawlJobLog
    ICrawlJobLog --|> ICrawlJobLogRow
```

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
erDiagram
  direction RL
    ICrawlConfiguration ||--o| ISubdirectory : Has
    ICrawlConfiguration ||--o{ ICrawlJobLog : Contains
```

| Base Interface                            | Row Interface                                             | List Item Interface(s)                                              | Record Interface                                    |
| ----------------------------------------- | --------------------------------------------------------- | ------------------------------------------------------------------- | --------------------------------------------------- |
| [ICrawlSettings](Model/ICrawlSettings.cs) | [ICrawlConfigurationRow](Model/ICrawlConfigurationRow.cs) | [ICrawlConfigurationListItem](Model/ICrawlConfigurationListItem.cs) | [ICrawlConfiguration](Model/ICrawlConfiguration.cs) |
| [ICrawlSettings](Model/ICrawlSettings.cs) | [ICrawlJobLogRow](Model/ICrawlJobLogRow.cs)               | [ICrawlJobListItem](Model/ICrawlJobListItem.cs)                     | [ICrawlJobLog](Model/ICrawlJobLog.cs)               |

- **[ICrawlSettings](Model/ICrawlSettings.cs)**
  - **[ICrawlConfigurationRow](Model/ICrawlConfigurationRow.cs)** : [IDbEntity](Model/IDbEntity.cs), [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
    - **[ICrawlConfigurationListItem](Model/ICrawlConfigurationListItem.cs)**
      - **[ICrawlConfigReportItem](Model/ICrawlConfigReportItem.cs)**
    - **[ICrawlConfiguration](Model/ICrawlConfiguration.cs)**
  - **[ICrawlJobLogRow](Model/ICrawlJobLogRow.cs)** : [IDbEntity](Model/IDbEntity.cs), [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
    - **[ICrawlJobListItem](Model/ICrawlJobListItem.cs)**
    - **[ICrawlJobLog](Model/ICrawlJobLog.cs)**

## Other Interfaces

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
classDiagram
  direction RL
    class IDbEntity

    class IComparison
    IComparison --|> IDbEntity

    class IBinaryPropertySet
    IBinaryPropertySet --|> IDbEntity

    class IAccessError
    IAccessError --|> IDbEntity

    class IFileAccessError
    IFileAccessError --|> IAccessError

    class ISubdirectoryAccessError
    ISubdirectoryAccessError --|> IAccessError

    class IVolumeAccessError
    IVolumeAccessError --|> IAccessError

    class IRedundancy
    IRedundancy --|> IDbEntity

    class IRedundantSetRow
    IRedundantSetRow --|> IDbEntity

    class IRedundantSet
    IRedundantSet --|> IRedundantSetRow

    class IRedundantSetListItem
    IRedundantSetListItem --|> IRedundantSetRow
```

```mermaid
---
  config:
    class:
      hideEmptyMembersBox: true
---
erDiagram
    IComparison ||--o| IFiles : Has Comparer
    IComparison ||--o| IFiles : Has Comparand
    IBinaryPropertySet ||--o{ IFile : Describes
    IBinaryPropertySet ||--o{ IRedundantSet : Defines
    IFileAccessError ||--o{ ICrawIFileJobLog : Annotates
    ISubdirectoryAccessError ||--o{ ISubdirectory : Annotates
    IVolumeAccessError ||--o{ IVolume : Annotates
    IRedundancy ||--o{ IFile : Links
    IRedundancy ||--o{ IRedundantSet : Links
```

- **[IDbEntity](Model/IDbEntity.cs)**
  - **[IComparison](Model/IComparison.cs)** : [IHasMembershipKeyReference](Model/IHasMembershipKeyReference.cs)
  - **[IBinaryPropertySet](Model/IBinaryPropertySet.cs)** : [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
  - **[IAccessError](Model/IAccessError.cs)** : [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
    - **[IFileAccessError](Model/IFileAccessError.cs)**
    - **[ISubdirectoryAccessError](Model/ISubdirectoryAccessError.cs)**
    - **[IVolumeAccessError](Model/IVolumeAccessError.cs)**
  - **[IRedundancy](Model/IRedundancy.cs)** : [IHasMembershipKeyReference](Model/IHasMembershipKeyReference.cs)
  - **[IRedundantSetRow](Model/IRedundantSetRow.cs)** : [IHasSimpleIdentifier](Model/IHasSimpleIdentifier.cs)
    - **[IRedundantSetListItem](Model/IRedundantSetListItem.cs)**
    - **[IRedundantSet](Model/IRedundantSet.cs)**
- **[IHasCompoundIdentifier](Model/IHasCompoundIdentifier.cs)**
  - **[IHasIdentifierPair](Model/IHasIdentifierPair.cs)**
    - **[IHasMembershipKeyReference](Model/IHasMembershipKeyReference.cs)**

```regex
\)[\r\n\s]+\w+ --\|> (\w+)(?= |$)
), [$1](./Model/$1.cs)

\)[\r\n\s]+\w+ --\|> FsInfoCat\.Model\.(\w+)
), [FsInfoCat.Model.$1](../Model/$1.cs)
```
