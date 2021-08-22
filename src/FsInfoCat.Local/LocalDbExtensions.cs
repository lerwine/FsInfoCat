using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public static class LocalDbExtensions
    {
        public static ISimpleIdentityReference<Subdirectory> GetParentReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.Parent ?? IdentityReference<Subdirectory>.FromId(file.ParentId);

        public static ISimpleIdentityReference<AudioPropertySet> GetAudioPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.AudioProperties ?? file.AudioPropertySetId.ToIdentityReference<AudioPropertySet>();

        public static ISimpleIdentityReference<SummaryPropertySet> GetSummaryPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.SummaryProperties ?? file.SummaryPropertySetId.ToIdentityReference<SummaryPropertySet>();

        public static ISimpleIdentityReference<ImagePropertySet> GetImagePropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.ImageProperties ?? file.ImagePropertySetId.ToIdentityReference<ImagePropertySet>();

        public static ISimpleIdentityReference<MusicPropertySet> GetMusicPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.MusicProperties ?? file.MusicPropertySetId.ToIdentityReference<MusicPropertySet>();

        public static ISimpleIdentityReference<RecordedTVPropertySet> GetRecordedTVPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.RecordedTVProperties ?? file.RecordedTVPropertySetId.ToIdentityReference<RecordedTVPropertySet>();

        public static ISimpleIdentityReference<VideoPropertySet> GetVideoPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.VideoProperties ?? file.VideoPropertySetId.ToIdentityReference<VideoPropertySet>();

        public static ISimpleIdentityReference<PhotoPropertySet> GetPhotoPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.PhotoProperties ?? file.PhotoPropertySetId.ToIdentityReference<PhotoPropertySet>();

        public static ISimpleIdentityReference<MediaPropertySet> GetMediaPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.MediaProperties ?? file.MediaPropertySetId.ToIdentityReference<MediaPropertySet>();

        public static ISimpleIdentityReference<GPSPropertySet> GetGPSPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.GPSProperties ?? file.GPSPropertySetId.ToIdentityReference<GPSPropertySet>();

        public static ISimpleIdentityReference<DRMPropertySet> GetDRMPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.DRMProperties ?? file.DRMPropertySetId.ToIdentityReference<DRMPropertySet>();

        public static ISimpleIdentityReference<DocumentPropertySet> GetDocumentPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.DocumentProperties ?? file.DocumentPropertySetId.ToIdentityReference<DocumentPropertySet>();
    }
}
