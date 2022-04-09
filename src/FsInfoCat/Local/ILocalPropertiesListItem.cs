namespace FsInfoCat.Local
{
    public interface ILocalPropertiesListItem : ILocalPropertiesRow, IPropertiesListItem { }

    public interface ILocalSummaryPropertiesListItem : ILocalSummaryPropertiesRow, ILocalPropertiesListItem, ISummaryPropertiesListItem { }

    public interface ILocalDocumentPropertiesListItem : ILocalDocumentPropertiesRow, ILocalPropertiesListItem, IDocumentPropertiesListItem { }

    public interface ILocalAudioPropertiesListItem : ILocalAudioPropertiesRow, ILocalPropertiesListItem, IAudioPropertiesListItem { }

    public interface ILocalDRMPropertiesListItem : ILocalDRMPropertiesRow, ILocalPropertiesListItem, IDRMPropertiesListItem { }

    public interface ILocalGPSPropertiesListItem : ILocalGPSPropertiesRow, ILocalPropertiesListItem, IGPSPropertiesListItem { }

    public interface ILocalImagePropertiesListItem : ILocalImagePropertiesRow, ILocalPropertiesListItem, IImagePropertiesListItem { }

    public interface ILocalMediaPropertiesListItem : ILocalMediaPropertiesRow, ILocalPropertiesListItem, IMediaPropertiesListItem { }

    public interface ILocalMusicPropertiesListItem : ILocalMusicPropertiesRow, ILocalPropertiesListItem, IMusicPropertiesListItem { }

    public interface ILocalPhotoPropertiesListItem : ILocalPhotoPropertiesRow, ILocalPropertiesListItem, IPhotoPropertiesListItem { }

    public interface ILocalRecordedTVPropertiesListItem : ILocalRecordedTVPropertiesRow, ILocalPropertiesListItem, IRecordedTVPropertiesListItem { }

    public interface ILocalVideoPropertiesListItem : ILocalVideoPropertiesRow, ILocalPropertiesListItem, IVideoPropertiesListItem { }
}
