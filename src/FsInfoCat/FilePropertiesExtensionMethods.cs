using FsInfoCat.Collections;

namespace FsInfoCat
{
    public static class FilePropertiesExtensionMethods
    {
        public static ISummaryProperties NullIfPropertiesEmpty(this ISummaryProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this ISummaryProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.ApplicationName) &&
            string.IsNullOrWhiteSpace(properties.Comment) && string.IsNullOrWhiteSpace(properties.Subject) &&
            string.IsNullOrWhiteSpace(properties.Title) && string.IsNullOrWhiteSpace(properties.Company) &&
            string.IsNullOrWhiteSpace(properties.ContentType) && string.IsNullOrWhiteSpace(properties.Copyright) &&
            string.IsNullOrWhiteSpace(properties.ParentalRating) && string.IsNullOrWhiteSpace(properties.ItemType) &&
            string.IsNullOrWhiteSpace(properties.MIMEType) && string.IsNullOrWhiteSpace(properties.ItemTypeText) &&
            string.IsNullOrWhiteSpace(properties.ParentalRatingsOrganization) && string.IsNullOrWhiteSpace(properties.ParentalRatingReason) &&
            string.IsNullOrWhiteSpace(properties.SensitivityText) && string.IsNullOrWhiteSpace(properties.Trademarks) && string.IsNullOrWhiteSpace(properties.ProductName) &&
            !(properties.Rating.HasValue || properties.Sensitivity.HasValue || properties.SimpleRating.HasValue) &&
            MultiStringValue.NullOrNotAny(properties.Author) && MultiStringValue.NullOrNotAny(properties.Keywords) &&
            MultiStringValue.NullOrNotAny(properties.ItemAuthors) && MultiStringValue.NullOrNotAny(properties.Kind));

        public static IAudioProperties NullIfPropertiesEmpty(this IAudioProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IAudioProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.Compression) &&
            string.IsNullOrWhiteSpace(properties.Format) && string.IsNullOrWhiteSpace(properties.StreamName) && !(properties.EncodingBitrate.HasValue ||
            properties.IsVariableBitrate.HasValue || properties.SampleRate.HasValue || properties.SampleSize.HasValue || properties.StreamNumber.HasValue));

        public static IDocumentProperties NullIfPropertiesEmpty(this IDocumentProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IDocumentProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.ClientID) &&
            string.IsNullOrWhiteSpace(properties.LastAuthor) && string.IsNullOrWhiteSpace(properties.RevisionNumber) &&
            string.IsNullOrWhiteSpace(properties.Division) && string.IsNullOrWhiteSpace(properties.DocumentID) && string.IsNullOrWhiteSpace(properties.Manager) &&
            string.IsNullOrWhiteSpace(properties.PresentationFormat) && string.IsNullOrWhiteSpace(properties.Version) && !(properties.DateCreated.HasValue ||
            properties.Security.HasValue) && MultiStringValue.NullOrNotAny(properties.Contributor));

        public static IDRMProperties NullIfPropertiesEmpty(this IDRMProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IDRMProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.Description) &&
            !(properties.DatePlayExpires.HasValue || properties.DatePlayStarts.HasValue || properties.IsProtected.HasValue || properties.PlayCount.HasValue));

        public static IGPSProperties NullIfPropertiesEmpty(this IGPSProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IGPSProperties properties)
        {
            if (properties is null)
                return true;
            if (properties.LatitudeDegrees.HasValue || properties.LatitudeMinutes.HasValue || properties.LatitudeSeconds.HasValue || properties.LongitudeDegrees.HasValue || properties.LongitudeMinutes.HasValue || properties.LongitudeSeconds.HasValue)
                return false;
            if (string.IsNullOrWhiteSpace(properties.AreaInformation) && string.IsNullOrWhiteSpace(properties.LatitudeRef) && string.IsNullOrWhiteSpace(properties.LongitudeRef) &&
                string.IsNullOrWhiteSpace(properties.MeasureMode) && string.IsNullOrWhiteSpace(properties.ProcessingMethod))
                return properties.VersionID is null || properties.VersionID.Count == 0;
            return false;
        }

        public static IImageProperties NullIfPropertiesEmpty(this IImageProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IImageProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.CompressionText) &&
            string.IsNullOrWhiteSpace(properties.ImageID) && !(properties.BitDepth.HasValue || properties.ColorSpace.HasValue ||
            properties.CompressedBitsPerPixel.HasValue || properties.Compression.HasValue || properties.HorizontalResolution.HasValue ||
            properties.HorizontalSize.HasValue || properties.ResolutionUnit.HasValue || properties.VerticalResolution.HasValue || properties.VerticalSize.HasValue));

        public static IMediaProperties NullIfPropertiesEmpty(this IMediaProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IMediaProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.ContentDistributor) &&
            string.IsNullOrWhiteSpace(properties.CreatorApplication) && string.IsNullOrWhiteSpace(properties.CreatorApplicationVersion) &&
            string.IsNullOrWhiteSpace(properties.DateReleased) && string.IsNullOrWhiteSpace(properties.DVDID) && string.IsNullOrWhiteSpace(properties.ProtectionType) &&
            string.IsNullOrWhiteSpace(properties.ProviderRating) && string.IsNullOrWhiteSpace(properties.ProviderStyle) &&
            string.IsNullOrWhiteSpace(properties.Publisher) && string.IsNullOrWhiteSpace(properties.Subtitle) && !(properties.Duration.HasValue ||
            properties.FrameCount.HasValue || properties.Year.HasValue) && MultiStringValue.NullOrNotAny(properties.Producer) &&
            MultiStringValue.NullOrNotAny(properties.Writer));

        public static IMusicProperties NullIfPropertiesEmpty(this IMusicProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IMusicProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.AlbumArtist) &&
            string.IsNullOrWhiteSpace(properties.AlbumTitle) && string.IsNullOrWhiteSpace(properties.DisplayArtist) && string.IsNullOrWhiteSpace(properties.PartOfSet) &&
            string.IsNullOrWhiteSpace(properties.Period) && !properties.TrackNumber.HasValue && MultiStringValue.NullOrNotAny(properties.Artist) &&
            MultiStringValue.NullOrNotAny(properties.Composer) && MultiStringValue.NullOrNotAny(properties.Conductor) &&
            MultiStringValue.NullOrNotAny(properties.Genre));

        public static IPhotoProperties NullIfPropertiesEmpty(this IPhotoProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IPhotoProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.CameraManufacturer) &&
            string.IsNullOrWhiteSpace(properties.CameraModel) && string.IsNullOrWhiteSpace(properties.EXIFVersion) &&
            string.IsNullOrWhiteSpace(properties.OrientationText) && !(properties.DateTaken.HasValue || properties.Orientation.HasValue) &&
            MultiStringValue.NullOrNotAny(properties.Event) && MultiStringValue.NullOrNotAny(properties.PeopleNames));

        public static IRecordedTVProperties NullIfPropertiesEmpty(this IRecordedTVProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IRecordedTVProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.EpisodeName) &&
            string.IsNullOrWhiteSpace(properties.NetworkAffiliation) && string.IsNullOrWhiteSpace(properties.ProgramDescription) &&
            string.IsNullOrWhiteSpace(properties.StationCallSign) && string.IsNullOrWhiteSpace(properties.StationName) &&
            !(properties.ChannelNumber.HasValue || properties.IsDTVContent.HasValue || properties.IsHDContent.HasValue || properties.OriginalBroadcastDate.HasValue));

        public static IVideoProperties NullIfPropertiesEmpty(this IVideoProperties properties) => properties.IsNullOrAllPropertiesEmpty() ? null : properties;

        public static bool IsNullOrAllPropertiesEmpty(this IVideoProperties properties) => properties is null || (string.IsNullOrWhiteSpace(properties.Compression) &&
            string.IsNullOrWhiteSpace(properties.StreamName) && !(properties.EncodingBitrate.HasValue || properties.FrameHeight.HasValue ||
            properties.FrameRate.HasValue || properties.FrameWidth.HasValue || properties.HorizontalAspectRatio.HasValue || properties.StreamNumber.HasValue ||
            properties.VerticalAspectRatio.HasValue) && MultiStringValue.NullOrNotAny(properties.Director));
    }
}
