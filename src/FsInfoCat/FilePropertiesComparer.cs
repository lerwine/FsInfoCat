using FsInfoCat.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    /// <summary>
    /// Common comparer for comparing extended file properties
    /// </summary>
    /// <seealso cref="Model.ISummaryProperties" />
    /// <seealso cref="Model.IDocumentProperties" />
    /// <seealso cref="Model.IAudioProperties" />
    /// <seealso cref="Model.IDRMProperties" />
    /// <seealso cref="Model.IGPSProperties" />
    /// <seealso cref="Model.IImageProperties" />
    /// <seealso cref="Model.IMediaProperties" />
    /// <seealso cref="Model.IMusicProperties" />
    /// <seealso cref="Model.IPhotoProperties" />
    /// <seealso cref="Model.IRecordedTVProperties" />
    /// <seealso cref="Model.IVideoProperties" />
    // TODO: Document FilePropertiesComparer class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class FilePropertiesComparer : IEqualityComparer<Model.ISummaryProperties>, IEqualityComparer<Model.IDocumentProperties>, IEqualityComparer<Model.IAudioProperties>,
        IEqualityComparer<Model.IDRMProperties>, IEqualityComparer<Model.IGPSProperties>, IEqualityComparer<Model.IImageProperties>, IEqualityComparer<Model.IMediaProperties>,
        IEqualityComparer<Model.IMusicProperties>, IEqualityComparer<Model.IPhotoProperties>, IEqualityComparer<Model.IRecordedTVProperties>, IEqualityComparer<Model.IVideoProperties>
    {
        public static readonly FilePropertiesComparer Default = new();

        public static readonly NullIfWhiteSpaceOrTrimmedStringCoersion StringValueCoersion = new(StringComparer.InvariantCultureIgnoreCase);

        public static readonly NullIfWhiteSpaceOrNormalizedStringCoersion NormalizedStringValueCoersion = new(StringComparer.InvariantCultureIgnoreCase);

        public static bool Equals(Model.ISummaryProperties x, Model.ISummaryProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.Rating == y.Rating && x.Sensitivity == y.Sensitivity && x.SimpleRating == y.SimpleRating &&
                NormalizedStringValueCoersion.Equals(x.ApplicationName, y.ApplicationName) && StringValueCoersion.Equals(x.Comment, y.Comment) &&
                NormalizedStringValueCoersion.Equals(x.Title, y.Title) && NormalizedStringValueCoersion.Equals(x.Subject, y.Subject) &&
                NormalizedStringValueCoersion.Equals(x.Company, y.Company) && NormalizedStringValueCoersion.Equals(x.ContentType, y.ContentType) &&
                NormalizedStringValueCoersion.Equals(x.Copyright, y.Copyright) && NormalizedStringValueCoersion.Equals(x.ParentalRating, y.ParentalRating) &&
                NormalizedStringValueCoersion.Equals(x.ItemType, y.ItemType) && NormalizedStringValueCoersion.Equals(x.MIMEType, y.MIMEType) &&
                StringValueCoersion.Equals(x.ItemTypeText, y.ItemTypeText) &&
                NormalizedStringValueCoersion.Equals(x.ParentalRatingsOrganization, y.ParentalRatingsOrganization) &&
                StringValueCoersion.Equals(x.ParentalRatingReason, y.ParentalRatingReason) &&
                StringValueCoersion.Equals(x.SensitivityText, y.SensitivityText) && StringValueCoersion.Equals(x.Trademarks, y.Trademarks) &&
                NormalizedStringValueCoersion.Equals(x.ProductName, y.ProductName) && MultiStringValue.AreEqual(x.Author, y.Author) &&
                MultiStringValue.AreEqual(x.Keywords, y.Keywords) && MultiStringValue.AreEqual(x.ItemAuthors, y.ItemAuthors) &&
                MultiStringValue.AreEqual(x.Kind, y.Kind)));
        }

        public static bool Equals(Model.IDocumentProperties x, Model.IDocumentProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.DateCreated == y.DateCreated && x.Security == y.Security &&
                NormalizedStringValueCoersion.Equals(x.ClientID, y.ClientID) && NormalizedStringValueCoersion.Equals(x.LastAuthor, y.LastAuthor) &&
                NormalizedStringValueCoersion.Equals(x.RevisionNumber, y.RevisionNumber) && StringValueCoersion.Equals(x.Division, y.Division) &&
                NormalizedStringValueCoersion.Equals(x.DocumentID, y.DocumentID) && NormalizedStringValueCoersion.Equals(x.Manager, y.Manager) &&
                NormalizedStringValueCoersion.Equals(x.PresentationFormat, y.PresentationFormat) && NormalizedStringValueCoersion.Equals(x.Version, y.Version) &&
                MultiStringValue.AreEqual(x.Contributor, y.Contributor)));
        }

        public static bool Equals(Model.IAudioProperties x, Model.IAudioProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.EncodingBitrate == y.EncodingBitrate && x.IsVariableBitrate == y.IsVariableBitrate &&
                x.SampleRate == y.SampleRate && x.SampleSize == y.SampleSize && x.StreamNumber == y.StreamNumber &&
                NormalizedStringValueCoersion.Equals(x.Compression, y.Compression) &&
                NormalizedStringValueCoersion.Equals(x.Format, y.Format) && NormalizedStringValueCoersion.Equals(x.StreamName, y.StreamName)));
        }

        public static bool Equals(Model.IDRMProperties x, Model.IDRMProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.DatePlayExpires == y.DatePlayExpires && x.DatePlayStarts == y.DatePlayStarts &&
                x.IsProtected == y.IsProtected && x.PlayCount == y.PlayCount && StringValueCoersion.Equals(x.Description, y.Description)));
        }

        public static bool Equals(Model.IGPSProperties x, Model.IGPSProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.LatitudeDegrees == y.LatitudeDegrees && x.LatitudeMinutes == y.LatitudeMinutes &&
                x.LatitudeSeconds == y.LatitudeSeconds && x.LongitudeDegrees == y.LongitudeDegrees && x.LongitudeMinutes == y.LongitudeMinutes &&
                x.LongitudeSeconds == y.LongitudeSeconds && NormalizedStringValueCoersion.Equals(x.AreaInformation, y.AreaInformation) &&
                NormalizedStringValueCoersion.Equals(x.LatitudeRef, y.LatitudeRef) && NormalizedStringValueCoersion.Equals(x.LongitudeRef, y.LongitudeRef) &&
                NormalizedStringValueCoersion.Equals(x.MeasureMode, y.MeasureMode) && NormalizedStringValueCoersion.Equals(x.ProcessingMethod, y.ProcessingMethod) &&
                x.VersionID.EmptyIfNull().SequenceEqual(y.VersionID.EmptyIfNull())));
        }

        public static bool Equals(Model.IImageProperties x, Model.IImageProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.BitDepth == y.BitDepth && x.ColorSpace == y.ColorSpace &&
                x.CompressedBitsPerPixel == y.CompressedBitsPerPixel && x.Compression == y.Compression && x.HorizontalResolution == y.HorizontalResolution &&
                x.HorizontalSize == y.HorizontalSize && x.ResolutionUnit == y.ResolutionUnit && x.VerticalResolution == y.VerticalResolution &&
                x.VerticalSize == y.VerticalSize && NormalizedStringValueCoersion.Equals(x.CompressionText, y.CompressionText) &&
                NormalizedStringValueCoersion.Equals(x.ImageID, y.ImageID)));
        }

        public static bool Equals(Model.IMediaProperties x, Model.IMediaProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.Duration == y.Duration && x.FrameCount == y.FrameCount && x.Year == y.Year &&
                NormalizedStringValueCoersion.Equals(x.ContentDistributor, y.ContentDistributor) &&
                NormalizedStringValueCoersion.Equals(x.CreatorApplication, y.CreatorApplication) &&
                NormalizedStringValueCoersion.Equals(x.CreatorApplicationVersion, y.CreatorApplicationVersion) &&
                NormalizedStringValueCoersion.Equals(x.DateReleased, y.DateReleased) && NormalizedStringValueCoersion.Equals(x.DVDID, y.DVDID) &&
                NormalizedStringValueCoersion.Equals(x.ProtectionType, y.ProtectionType) &&
                NormalizedStringValueCoersion.Equals(x.ProviderRating, y.ProviderRating) && NormalizedStringValueCoersion.Equals(x.ProviderStyle, y.ProviderStyle) &&
                NormalizedStringValueCoersion.Equals(x.Publisher, y.Publisher) && NormalizedStringValueCoersion.Equals(x.Subtitle, y.Subtitle) &&
                MultiStringValue.AreEqual(x.Producer, y.Producer) && MultiStringValue.AreEqual(x.Writer, y.Writer)));
        }

        public static bool Equals(Model.IMusicProperties x, Model.IMusicProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.ChannelCount == y.ChannelCount && x.TrackNumber == y.TrackNumber &&
                NormalizedStringValueCoersion.Equals(x.AlbumArtist, y.AlbumArtist) && NormalizedStringValueCoersion.Equals(x.AlbumTitle, y.AlbumTitle) &&
                NormalizedStringValueCoersion.Equals(x.DisplayArtist, y.DisplayArtist) && NormalizedStringValueCoersion.Equals(x.PartOfSet, y.PartOfSet) &&
                NormalizedStringValueCoersion.Equals(x.Period, y.Period) && MultiStringValue.AreEqual(x.Artist, y.Artist) &&
                MultiStringValue.AreEqual(x.Composer, y.Composer) && MultiStringValue.AreEqual(x.Conductor, y.Conductor) &&
                MultiStringValue.AreEqual(x.Genre, y.Genre)));
        }

        public static bool Equals(Model.IPhotoProperties x, Model.IPhotoProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.DateTaken == y.DateTaken && x.Orientation == y.Orientation &&
                NormalizedStringValueCoersion.Equals(x.CameraManufacturer, y.CameraManufacturer) &&
                NormalizedStringValueCoersion.Equals(x.CameraModel, y.CameraModel) &&
                NormalizedStringValueCoersion.Equals(x.EXIFVersion, y.EXIFVersion) && StringValueCoersion.Equals(x.OrientationText, y.OrientationText) &&
                MultiStringValue.AreEqual(x.Event, y.Event) && MultiStringValue.AreEqual(x.PeopleNames, y.PeopleNames)));
        }

        public static bool Equals(Model.IRecordedTVProperties x, Model.IRecordedTVProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.ChannelNumber == y.ChannelNumber && x.IsDTVContent == y.IsDTVContent &&
                x.IsHDContent == y.IsHDContent && x.OriginalBroadcastDate == y.OriginalBroadcastDate &&
                NormalizedStringValueCoersion.Equals(x.EpisodeName, y.EpisodeName) &&
                NormalizedStringValueCoersion.Equals(x.NetworkAffiliation, y.NetworkAffiliation) &&
                StringValueCoersion.Equals(x.ProgramDescription, y.ProgramDescription) &&
                NormalizedStringValueCoersion.Equals(x.StationCallSign, y.StationCallSign) && NormalizedStringValueCoersion.Equals(x.StationName, y.StationName)));
        }

        public static bool Equals(Model.IVideoProperties x, Model.IVideoProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.EncodingBitrate == y.EncodingBitrate && x.FrameHeight == y.FrameHeight && x.FrameRate == y.FrameRate &&
                x.FrameWidth == y.FrameWidth && x.HorizontalAspectRatio == y.HorizontalAspectRatio && x.StreamNumber == y.StreamNumber &&
                x.VerticalAspectRatio == y.VerticalAspectRatio && NormalizedStringValueCoersion.Equals(x.Compression, y.Compression) &&
                NormalizedStringValueCoersion.Equals(x.StreamName, y.StreamName) && MultiStringValue.AreEqual(x.Director, y.Director)));
        }

        bool IEqualityComparer<Model.ISummaryProperties>.Equals(Model.ISummaryProperties x, Model.ISummaryProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IDocumentProperties>.Equals(Model.IDocumentProperties x, Model.IDocumentProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IAudioProperties>.Equals(Model.IAudioProperties x, Model.IAudioProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IDRMProperties>.Equals(Model.IDRMProperties x, Model.IDRMProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IGPSProperties>.Equals(Model.IGPSProperties x, Model.IGPSProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IImageProperties>.Equals(Model.IImageProperties x, Model.IImageProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IMediaProperties>.Equals(Model.IMediaProperties x, Model.IMediaProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IMusicProperties>.Equals(Model.IMusicProperties x, Model.IMusicProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IPhotoProperties>.Equals(Model.IPhotoProperties x, Model.IPhotoProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IRecordedTVProperties>.Equals(Model.IRecordedTVProperties x, Model.IRecordedTVProperties y) => Equals(x, y);
        bool IEqualityComparer<Model.IVideoProperties>.Equals(Model.IVideoProperties x, Model.IVideoProperties y) => Equals(x, y);

        public int GetHashCode([DisallowNull] Model.ISummaryProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.Rating?.GetHashCode(), obj.Rating?.GetHashCode(), obj.Rating?.GetHashCode(), obj.Author?.GetHashCode(), obj.Keywords?.GetHashCode(),
                obj.ItemAuthors?.GetHashCode(), obj.Kind?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.Title, obj.Subject, obj.Company, obj.ContentType, obj.Copyright, obj.ParentalRating, obj.ItemType, obj.MIMEType,
                    obj.ParentalRatingsOrganization, obj.ProductName })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s)))
                .Concat((new string[] { obj.Comment, obj.ItemTypeText, obj.ParentalRatingReason, obj.SensitivityText, obj.Copyright })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s))).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IDocumentProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.DateCreated?.GetHashCode(), obj.Security?.GetHashCode(), obj.Contributor?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.ClientID, obj.LastAuthor, obj.RevisionNumber, obj.DocumentID, obj.Manager, obj.PresentationFormat, obj.Version })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s)))
                .Concat((new int[] { NormalizedStringValueCoersion.GetHashCode(obj.Division) })).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IAudioProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.EncodingBitrate?.GetHashCode(), obj.Format?.GetHashCode(), obj.IsVariableBitrate?.GetHashCode(), obj.SampleRate?.GetHashCode(),
                obj.SampleSize?.GetHashCode(), obj.StreamNumber?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.Compression, obj.StreamName })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s))).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IDRMProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.DatePlayExpires?.GetHashCode(), obj.DatePlayStarts?.GetHashCode(), obj.IsProtected?.GetHashCode(), obj.PlayCount?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new int[] { NormalizedStringValueCoersion.GetHashCode(obj.Description) })).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IGPSProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.LatitudeDegrees?.GetHashCode(), obj.LatitudeMinutes?.GetHashCode(), obj.LatitudeSeconds?.GetHashCode(), obj.LongitudeDegrees?.GetHashCode(),
                obj.LongitudeMinutes?.GetHashCode(), obj.LongitudeSeconds?.GetHashCode(), obj.VersionID?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.AreaInformation, obj.LatitudeRef, obj.LongitudeRef, obj.MeasureMode, obj.ProcessingMethod })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s))).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IImageProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.BitDepth?.GetHashCode(), obj.ColorSpace?.GetHashCode(), obj.CompressedBitsPerPixel?.GetHashCode(), obj.Compression?.GetHashCode(),
                obj.HorizontalResolution?.GetHashCode(), obj.HorizontalSize?.GetHashCode(), obj.ResolutionUnit?.GetHashCode(), obj.VerticalResolution?.GetHashCode(),
                obj.VerticalSize?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.CompressionText, obj.ImageID })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s))).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IMediaProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.Duration?.GetHashCode(), obj.FrameCount?.GetHashCode(), obj.Producer?.GetHashCode(),
                obj.Writer?.GetHashCode(), obj.Year?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.ContentDistributor, obj.CreatorApplication, obj.CreatorApplicationVersion, obj.DateReleased, obj.DVDID, obj.ProtectionType,
                    obj.ProviderRating, obj.ProviderStyle, obj.Publisher, obj.Subtitle })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s))).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IMusicProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.Artist?.GetHashCode(), obj.ChannelCount?.GetHashCode(), obj.Composer?.GetHashCode(), obj.Conductor?.GetHashCode(), obj.Genre?.GetHashCode(),
                obj.TrackNumber?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.AlbumArtist, obj.AlbumTitle, obj.DisplayArtist, obj.PartOfSet, obj.Period })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s))).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IPhotoProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.DateTaken?.GetHashCode(), obj.Event?.GetHashCode(), obj.Orientation?.GetHashCode(), obj.PeopleNames?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.CameraManufacturer, obj.CameraModel, obj.EXIFVersion })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s)))
                .Concat((new int[] { NormalizedStringValueCoersion.GetHashCode(obj.OrientationText) })).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IRecordedTVProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.ChannelNumber?.GetHashCode(), obj.IsDTVContent?.GetHashCode(), obj.IsHDContent?.GetHashCode(), obj.OriginalBroadcastDate?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.EpisodeName, obj.NetworkAffiliation, obj.StationCallSign, obj.StationName })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s)))
                .Concat((new int[] { NormalizedStringValueCoersion.GetHashCode(obj.ProgramDescription) })).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] Model.IVideoProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.Director?.GetHashCode(), obj.EncodingBitrate?.GetHashCode(), obj.FrameHeight?.GetHashCode(), obj.FrameRate?.GetHashCode(),
                obj.FrameWidth?.GetHashCode(), obj.HorizontalAspectRatio?.GetHashCode(), obj.StreamNumber?.GetHashCode(), obj.VerticalAspectRatio?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.Compression, obj.StreamName })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s))).ToAggregateHashCode();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
