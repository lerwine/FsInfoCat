namespace FsInfoCat.Local
{
    public interface ILocalPropertiesRow : ILocalDbEntity, IPropertiesRow { }

    public interface ILocalSummaryPropertiesRow : ILocalPropertiesRow, ISummaryPropertiesRow { }

    public interface ILocalDocumentPropertiesRow : ILocalPropertiesRow, IDocumentPropertiesRow { }

    public interface ILocalAudioPropertiesRow : ILocalPropertiesRow, IAudioPropertiesRow { }

    public interface ILocalDRMPropertiesRow : ILocalPropertiesRow, IDRMPropertiesRow { }

    public interface ILocalGPSPropertiesRow : ILocalPropertiesRow, IGPSPropertiesRow { }

    public interface ILocalImagePropertiesRow : ILocalPropertiesRow, IImagePropertiesRow { }

    public interface ILocalMediaPropertiesRow : ILocalPropertiesRow, IMediaPropertiesRow { }

    public interface ILocalMusicPropertiesRow : ILocalPropertiesRow, IMusicPropertiesRow { }

    public interface ILocalPhotoPropertiesRow : ILocalPropertiesRow, IPhotoPropertiesRow { }

    public interface ILocalRecordedTVPropertiesRow : ILocalPropertiesRow, IRecordedTVPropertiesRow { }

    public interface ILocalVideoPropertiesRow : ILocalPropertiesRow, IVideoPropertiesRow { }
}
