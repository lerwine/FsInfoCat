using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    /// <summary>
    /// Common comparer for comparing extended file properties
    /// </summary>
    /// <seealso cref="ISummaryProperties" />
    /// <seealso cref="IDocumentProperties" />
    /// <seealso cref="IAudioProperties" />
    /// <seealso cref="IDRMProperties" />
    /// <seealso cref="IGPSProperties" />
    /// <seealso cref="IImageProperties" />
    /// <seealso cref="IMediaProperties" />
    /// <seealso cref="IMusicProperties" />
    /// <seealso cref="IPhotoProperties" />
    /// <seealso cref="IRecordedTVProperties" />
    /// <seealso cref="IVideoProperties" />
    public class FilePropertiesComparer : IEqualityComparer<ISummaryProperties>, IEqualityComparer<IDocumentProperties>, IEqualityComparer<IAudioProperties>,
        IEqualityComparer<IDRMProperties>, IEqualityComparer<IGPSProperties>, IEqualityComparer<IImageProperties>, IEqualityComparer<IMediaProperties>,
        IEqualityComparer<IMusicProperties>, IEqualityComparer<IPhotoProperties>, IEqualityComparer<IRecordedTVProperties>, IEqualityComparer<IVideoProperties>
    {
        public static readonly FilePropertiesComparer Default = new();

        // BUG: This is not using case-insensitive matching
        public static bool Equals(ISummaryProperties x, ISummaryProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.Rating == y.Rating && x.Sensitivity == y.Sensitivity && x.SimpleRating == y.SimpleRating &&
                x.ApplicationName.NullIfWhiteSpace() == y.ApplicationName.NullIfWhiteSpace() && x.Comment.NullIfWhiteSpace() == y.Comment.NullIfWhiteSpace() &&
                x.Title.NullIfWhiteSpace() == y.Title.NullIfWhiteSpace() && x.Subject.NullIfWhiteSpace() == y.Subject.NullIfWhiteSpace() &&
                x.Company.NullIfWhiteSpace() == y.Company.NullIfWhiteSpace() && x.ContentType.NullIfWhiteSpace() == y.ContentType.NullIfWhiteSpace() &&
                x.Copyright.NullIfWhiteSpace() == y.Copyright.NullIfWhiteSpace() && x.ParentalRating.NullIfWhiteSpace() == y.ParentalRating.NullIfWhiteSpace() &&
                x.ItemType.NullIfWhiteSpace() == y.ItemType.NullIfWhiteSpace() && x.MIMEType.NullIfWhiteSpace() == y.MIMEType.NullIfWhiteSpace() &&
                x.ItemTypeText.NullIfWhiteSpace() == y.ItemTypeText.NullIfWhiteSpace() &&
                x.ParentalRatingsOrganization.NullIfWhiteSpace() == y.ParentalRatingsOrganization.NullIfWhiteSpace() &&
                x.ParentalRatingReason.NullIfWhiteSpace() == y.ParentalRatingReason.NullIfWhiteSpace() &&
                x.SensitivityText.NullIfWhiteSpace() == y.SensitivityText.NullIfWhiteSpace() && x.Trademarks.NullIfWhiteSpace() == y.Trademarks.NullIfWhiteSpace() &&
                x.ProductName.NullIfWhiteSpace() == y.ProductName.NullIfWhiteSpace() &&
                x.Author.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Author.EmptyIfNull().ElementsNotNullOrWhiteSpace()) &&
                x.Keywords.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Keywords.EmptyIfNull().ElementsNotNullOrWhiteSpace()) &&
                x.ItemAuthors.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.ItemAuthors.EmptyIfNull().ElementsNotNullOrWhiteSpace()) &&
                x.Kind.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Kind.EmptyIfNull().ElementsNotNullOrWhiteSpace())));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IDocumentProperties x, IDocumentProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.DateCreated == y.DateCreated && x.Security == y.Security &&
                x.ClientID.NullIfWhiteSpace() == y.ClientID.NullIfWhiteSpace() && x.LastAuthor.NullIfWhiteSpace() == y.LastAuthor.NullIfWhiteSpace() &&
                x.RevisionNumber.NullIfWhiteSpace() == y.RevisionNumber.NullIfWhiteSpace() && x.Division.NullIfWhiteSpace() == y.Division.NullIfWhiteSpace() &&
                x.DocumentID.NullIfWhiteSpace() == y.DocumentID.NullIfWhiteSpace() && x.Manager.NullIfWhiteSpace() == y.Manager.NullIfWhiteSpace() &&
                x.PresentationFormat.NullIfWhiteSpace() == y.PresentationFormat.NullIfWhiteSpace() && x.Version.NullIfWhiteSpace() == y.Version.NullIfWhiteSpace() &&
                x.Contributor.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Contributor.EmptyIfNull().ElementsNotNullOrWhiteSpace())));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IAudioProperties x, IAudioProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.EncodingBitrate == y.EncodingBitrate && x.IsVariableBitrate == y.IsVariableBitrate &&
                x.SampleRate == y.SampleRate && x.SampleSize == y.SampleSize && x.StreamNumber == y.StreamNumber &&
                x.Compression.NullIfWhiteSpace() == y.Compression.NullIfWhiteSpace() &&
                x.Format.NullIfWhiteSpace() == y.Format.NullIfWhiteSpace() && x.StreamName.NullIfWhiteSpace() == y.StreamName.NullIfWhiteSpace()));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IDRMProperties x, IDRMProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.DatePlayExpires == y.DatePlayExpires && x.DatePlayStarts == y.DatePlayStarts &&
                x.IsProtected == y.IsProtected && x.PlayCount == y.PlayCount && x.Description.NullIfWhiteSpace() == y.Description.NullIfWhiteSpace()));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IGPSProperties x, IGPSProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.LatitudeDegrees == y.LatitudeDegrees && x.LatitudeMinutes == y.LatitudeMinutes &&
                x.LatitudeSeconds == y.LatitudeSeconds && x.LongitudeDegrees == y.LongitudeDegrees && x.LongitudeMinutes == y.LongitudeMinutes &&
                x.LongitudeSeconds == y.LongitudeSeconds && x.AreaInformation.NullIfWhiteSpace() == y.AreaInformation.NullIfWhiteSpace() &&
                x.LatitudeRef.NullIfWhiteSpace() == y.LatitudeRef.NullIfWhiteSpace() && x.LongitudeRef.NullIfWhiteSpace() == y.LongitudeRef.NullIfWhiteSpace() &&
                x.MeasureMode.NullIfWhiteSpace() == y.MeasureMode.NullIfWhiteSpace() && x.ProcessingMethod.NullIfWhiteSpace() == y.ProcessingMethod.NullIfWhiteSpace() &&
                x.VersionID.EmptyIfNull().SequenceEqual(y.VersionID.EmptyIfNull())));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IImageProperties x, IImageProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.BitDepth == y.BitDepth && x.ColorSpace == y.ColorSpace &&
                x.CompressedBitsPerPixel == y.CompressedBitsPerPixel && x.Compression == y.Compression && x.HorizontalResolution == y.HorizontalResolution &&
                x.HorizontalSize == y.HorizontalSize && x.ResolutionUnit == y.ResolutionUnit && x.VerticalResolution == y.VerticalResolution &&
                x.VerticalSize == y.VerticalSize && x.CompressionText.NullIfWhiteSpace() == y.CompressionText.NullIfWhiteSpace() &&
                x.ImageID.NullIfWhiteSpace() == y.ImageID.NullIfWhiteSpace()));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IMediaProperties x, IMediaProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.Duration == y.Duration && x.FrameCount == y.FrameCount && x.Year == y.Year &&
                x.ContentDistributor.NullIfWhiteSpace() == y.ContentDistributor.NullIfWhiteSpace() &&
                x.CreatorApplication.NullIfWhiteSpace() == y.CreatorApplication.NullIfWhiteSpace() &&
                x.CreatorApplicationVersion.NullIfWhiteSpace() == y.CreatorApplicationVersion.NullIfWhiteSpace() &&
                x.DateReleased.NullIfWhiteSpace() == y.DateReleased.NullIfWhiteSpace() && x.DVDID.NullIfWhiteSpace() == y.DVDID.NullIfWhiteSpace() &&
                x.ProtectionType.NullIfWhiteSpace() == y.ProtectionType.NullIfWhiteSpace() &&
                x.ProviderRating.NullIfWhiteSpace() == y.ProviderRating.NullIfWhiteSpace() && x.ProviderStyle.NullIfWhiteSpace() == y.ProviderStyle.NullIfWhiteSpace() &&
                x.Publisher.NullIfWhiteSpace() == y.Publisher.NullIfWhiteSpace() && x.Subtitle.NullIfWhiteSpace() == y.Subtitle.NullIfWhiteSpace() &&
                x.Producer.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Producer.EmptyIfNull().ElementsNotNullOrWhiteSpace()) &&
                x.Writer.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Writer.EmptyIfNull().ElementsNotNullOrWhiteSpace())));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IMusicProperties x, IMusicProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.ChannelCount == y.ChannelCount && x.TrackNumber == y.TrackNumber &&
                x.AlbumArtist.NullIfWhiteSpace() == y.AlbumArtist.NullIfWhiteSpace() && x.AlbumTitle.NullIfWhiteSpace() == y.AlbumTitle.NullIfWhiteSpace() &&
                x.DisplayArtist.NullIfWhiteSpace() == y.DisplayArtist.NullIfWhiteSpace() && x.PartOfSet.NullIfWhiteSpace() == y.PartOfSet.NullIfWhiteSpace() &&
                x.Period.NullIfWhiteSpace() == y.Period.NullIfWhiteSpace() &&
                x.Artist.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Artist.EmptyIfNull().ElementsNotNullOrWhiteSpace()) &&
                x.Composer.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Composer.EmptyIfNull().ElementsNotNullOrWhiteSpace()) &&
                x.Conductor.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Conductor.EmptyIfNull().ElementsNotNullOrWhiteSpace()) &&
                x.Genre.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Genre.EmptyIfNull().ElementsNotNullOrWhiteSpace())));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IPhotoProperties x, IPhotoProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.DateTaken == y.DateTaken && x.Orientation == y.Orientation &&
                x.CameraManufacturer.NullIfWhiteSpace() == y.CameraManufacturer.NullIfWhiteSpace() &&
                x.CameraModel.NullIfWhiteSpace() == y.CameraModel.NullIfWhiteSpace() &&
                x.EXIFVersion.NullIfWhiteSpace() == y.EXIFVersion.NullIfWhiteSpace() && x.OrientationText.NullIfWhiteSpace() == y.OrientationText.NullIfWhiteSpace() &&
                x.Event.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Event.EmptyIfNull().ElementsNotNullOrWhiteSpace()) &&
                x.PeopleNames.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.PeopleNames.EmptyIfNull().ElementsNotNullOrWhiteSpace())));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IRecordedTVProperties x, IRecordedTVProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.ChannelNumber == y.ChannelNumber && x.IsDTVContent == y.IsDTVContent &&
                x.IsHDContent == y.IsHDContent && x.OriginalBroadcastDate == y.OriginalBroadcastDate &&
                x.EpisodeName.NullIfWhiteSpace() == y.EpisodeName.NullIfWhiteSpace() &&
                x.NetworkAffiliation.NullIfWhiteSpace() == y.NetworkAffiliation.NullIfWhiteSpace() &&
                x.ProgramDescription.NullIfWhiteSpace() == y.ProgramDescription.NullIfWhiteSpace() &&
                x.StationCallSign.NullIfWhiteSpace() == y.StationCallSign.NullIfWhiteSpace() && x.StationName.NullIfWhiteSpace() == y.StationName.NullIfWhiteSpace()));
        }

        // BUG: This is not using case-insensitive matching
        public static bool Equals(IVideoProperties x, IVideoProperties y)
        {
            if (x.ArePropertiesNullOrEmpty())
                return y.ArePropertiesNullOrEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.EncodingBitrate == y.EncodingBitrate && x.FrameHeight == y.FrameHeight && x.FrameRate == y.FrameRate &&
                x.FrameWidth == y.FrameWidth && x.HorizontalAspectRatio == y.HorizontalAspectRatio && x.StreamNumber == y.StreamNumber &&
                x.VerticalAspectRatio == y.VerticalAspectRatio && x.Compression.NullIfWhiteSpace() == y.Compression.NullIfWhiteSpace() &&
                x.StreamName.NullIfWhiteSpace() == y.StreamName.NullIfWhiteSpace() &&
                x.Director.EmptyIfNull().ElementsNotNullOrWhiteSpace().SequenceEqual(y.Director.EmptyIfNull().ElementsNotNullOrWhiteSpace())));
        }

        bool IEqualityComparer<ISummaryProperties>.Equals(ISummaryProperties x, ISummaryProperties y) => Equals(x, y);
        bool IEqualityComparer<IDocumentProperties>.Equals(IDocumentProperties x, IDocumentProperties y) => Equals(x, y);
        bool IEqualityComparer<IAudioProperties>.Equals(IAudioProperties x, IAudioProperties y) => Equals(x, y);
        bool IEqualityComparer<IDRMProperties>.Equals(IDRMProperties x, IDRMProperties y) => Equals(x, y);
        bool IEqualityComparer<IGPSProperties>.Equals(IGPSProperties x, IGPSProperties y) => Equals(x, y);
        bool IEqualityComparer<IImageProperties>.Equals(IImageProperties x, IImageProperties y) => Equals(x, y);
        bool IEqualityComparer<IMediaProperties>.Equals(IMediaProperties x, IMediaProperties y) => Equals(x, y);
        bool IEqualityComparer<IMusicProperties>.Equals(IMusicProperties x, IMusicProperties y) => Equals(x, y);
        bool IEqualityComparer<IPhotoProperties>.Equals(IPhotoProperties x, IPhotoProperties y) => Equals(x, y);
        bool IEqualityComparer<IRecordedTVProperties>.Equals(IRecordedTVProperties x, IRecordedTVProperties y) => Equals(x, y);
        bool IEqualityComparer<IVideoProperties>.Equals(IVideoProperties x, IVideoProperties y) => Equals(x, y);

        public int GetHashCode([DisallowNull] ISummaryProperties obj)
        {
            // TODO: Implement GetHashCode(ISummaryProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IDocumentProperties obj)
        {
            // TODO: Implement GetHashCode(IDocumentProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IAudioProperties obj)
        {
            // TODO: Implement GetHashCode(IAudioProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IDRMProperties obj)
        {
            // TODO: Implement GetHashCode(IDRMProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IGPSProperties obj)
        {
            // TODO: Implement GetHashCode(IGPSProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IImageProperties obj)
        {
            // TODO: Implement GetHashCode(IImageProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IMediaProperties obj)
        {
            // TODO: Implement GetHashCode(IMediaProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IMusicProperties obj)
        {
            // TODO: Implement GetHashCode(IMusicProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IPhotoProperties obj)
        {
            // TODO: Implement GetHashCode(IPhotoProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IRecordedTVProperties obj)
        {
            // TODO: Implement GetHashCode(IRecordedTVProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IVideoProperties obj)
        {
            // TODO: Implement GetHashCode(IVideoProperties);
            throw new NotImplementedException();
        }
    }
}
