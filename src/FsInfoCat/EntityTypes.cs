using FsInfoCat.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat
{
    /// <summary>
    /// Base interface for all database entity objects which track the creation and modification dates as well as implementing the
    /// <see cref="IValidatableObject" /> and <see cref="IRevertibleChangeTracking" /> interfaces.
    /// </summary>
    /// <seealso cref="IValidatableObject"></seealso>
    /// <seealso cref="IRevertibleChangeTracking"></seealso>
    public interface IDbEntityTest : IValidatableObject, IRevertibleChangeTracking
    {
        /// <summary>
        /// Base interface for all database entity objects which track the creation and modification dates as well as implementing the
        /// <see cref="IValidatableObject" /> and <see cref="IRevertibleChangeTracking" /> interfaces.
        /// </summary>
        DateTime CreatedOn { get; }

        /// <summary>
        /// Base interface for all database entity objects which track the creation and modification dates as well as implementing the
        /// <see cref="IValidatableObject" /> and <see cref="IRevertibleChangeTracking" /> interfaces.
        /// </summary>
        DateTime ModifiedOn { get; }
    }

    /// <summary>Generic interface for access error entities.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface IAccessErrorTest : IDbEntity
    {
        /// <summary>Generic interface for access error entities.</summary>
        Guid Id { get; }

        /// <summary>Generic interface for access error entities.</summary>
        Guid TargetId { get; }

        /// <summary>Generic interface for access error entities.</summary>
        object ErrorCode { get; }

        /// <summary>Generic interface for access error entities.</summary>
        string Message { get; }

        /// <summary>Generic interface for access error entities.</summary>
        string Details { get; }
    }

    /// <summary>Interface for entities which represent a specific file system type.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface IFileSystemTest : IDbEntity
    {
        /// <summary>Interface for entities which represent a specific file system type.</summary>
        Guid Id { get; }

        /// <summary>Interface for entities which represent a specific file system type.</summary>
        string DisplayName { get; }

        /// <summary>Interface for entities which represent a specific file system type.</summary>
        bool CaseSensitiveSearch { get; }

        /// <summary>Interface for entities which represent a specific file system type.</summary>
        bool ReadOnly { get; }

        /// <summary>Interface for entities which represent a specific file system type.</summary>
        uint MaxNameLength { get; }

        /// <summary>Interface for entities which represent a specific file system type.</summary>
        DriveType DefaultDriveType { get; }

        /// <summary>Interface for entities which represent a specific file system type.</summary>
        string Notes { get; }

        /// <summary>Interface for entities which represent a specific file system type.</summary>
        bool IsInactive { get; }

        /// <summary>Interface for entities which represent a specific file system type.</summary>
        object Volumes { get; }

        /// <summary>Interface for entities which represent a specific file system type.</summary>
        object SymbolicNames { get; }
    }

    /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface ISymbolicNameTest : IDbEntity
    {
        /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
        Guid Id { get; }

        /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
        Guid FileSystemId { get; }

        /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
        string Name { get; }

        /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
        string Notes { get; }

        /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
        int Priority { get; }

        /// <summary>Interface for entities that represent a symbolic name for a file system type.</summary>
        bool IsInactive { get; }
    }

    /// <summary>Interface for entities which represent a logical file system volume.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface IVolumeTest : IDbEntity
    {
        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        Guid Id { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        Guid FileSystemId { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        string DisplayName { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        string VolumeName { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        VolumeIdentifier Identifier { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        bool CaseSensitiveSearch { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        bool ReadOnly { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        uint? MaxNameLength { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        DriveType Type { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        string Notes { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        object Status { get; }

        /// <summary>Interface for entities which represent a logical file system volume.</summary>
        object AccessErrors { get; }
    }

    /// <summary>Configuration of a file system crawl instance.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface ICrawlConfigurationTest : IDbEntity
    {
        /// <summary>Configuration of a file system crawl instance.</summary>
        Guid Id { get; }

        /// <summary>Configuration of a file system crawl instance.</summary>
        string DisplayName { get; }

        /// <summary>Configuration of a file system crawl instance.</summary>
        string Notes { get; }

        /// <summary>Configuration of a file system crawl instance.</summary>
        ushort MaxRecursionDepth { get; }

        /// <summary>Configuration of a file system crawl instance.</summary>
        ulong MaxTotalItems { get; }

        /// <summary>Configuration of a file system crawl instance.</summary>
        long? TTL { get; }

        /// <summary>Configuration of a file system crawl instance.</summary>
        bool IsInactive { get; }
    }

    /// <summary>Base interface for a database entity that represents a file system node.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface IDbFsItemTest : IDbEntity
    {
        /// <summary>Base interface for a database entity that represents a file system node.</summary>
        Guid Id { get; }

        /// <summary>Base interface for a database entity that represents a file system node.</summary>
        string Name { get; }

        /// <summary>Base interface for a database entity that represents a file system node.</summary>
        DateTime LastAccessed { get; }

        /// <summary>Base interface for a database entity that represents a file system node.</summary>
        string Notes { get; }

        /// <summary>Base interface for a database entity that represents a file system node.</summary>
        DateTime CreationTime { get; }

        /// <summary>Base interface for a database entity that represents a file system node.</summary>
        DateTime LastWriteTime { get; }
    }

    /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
    /// <seealso cref="IDbFsItem"></seealso>
    public interface ISubdirectoryTest : IDbFsItem
    {
        /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
        object Options { get; }

        /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
        object Status { get; }

        /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
        Guid? ParentId { get; }

        /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
        Guid? VolumeId { get; }

        /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
        Guid? CrawlConfigurationId { get; }

        /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
        object Files { get; }

        /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
        object SubDirectories { get; }

        /// <summary>Interface for entities that represent a subdirectory node within a file system.</summary>
        object AccessErrors { get; }
    }

    /// <summary>Represents a set of files that have the same file size and cryptographic hash.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface IBinaryPropertySetTest : IDbEntity
    {
        /// <summary>Represents a set of files that have the same file size and cryptographic hash.</summary>
        Guid Id { get; }

        /// <summary>Represents a set of files that have the same file size and cryptographic hash.</summary>
        long Length { get; }

        /// <summary>Represents a set of files that have the same file size and cryptographic hash.</summary>
        MD5Hash Hash { get; }

        /// <summary>Represents a set of files that have the same file size and cryptographic hash.</summary>
        object Files { get; }

        /// <summary>Represents a set of files that have the same file size and cryptographic hash.</summary>
        object RedundantSets { get; }
    }

    /// <summary>Represents extended file summary properties.</summary>
    public interface ISummaryPropertiesTest
    {
        /// <summary>Represents extended file summary properties.</summary>
        string ApplicationName { get; }

        /// <summary>Represents extended file summary properties.</summary>
        MultiStringValue Author { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string Comment { get; }

        /// <summary>Represents extended file summary properties.</summary>
        MultiStringValue Keywords { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string Subject { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string Title { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string Company { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string ContentType { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string Copyright { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string ParentalRating { get; }

        /// <summary>Represents extended file summary properties.</summary>
        uint Rating { get; }

        /// <summary>Represents extended file summary properties.</summary>
        MultiStringValue ItemAuthors { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string ItemType { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string ItemTypeText { get; }

        /// <summary>Represents extended file summary properties.</summary>
        MultiStringValue Kind { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string MIMEType { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string ParentalRatingReason { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string ParentalRatingsOrganization { get; }

        /// <summary>Represents extended file summary properties.</summary>
        ushort Sensitivity { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string SensitivityText { get; }

        /// <summary>Represents extended file summary properties.</summary>
        uint SimpleRating { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string Trademarks { get; }

        /// <summary>Represents extended file summary properties.</summary>
        string ProductName { get; }
    }

    /// <summary>Represents extended file properties for document files.</summary>
    public interface IDocumentPropertiesTest
    {
        /// <summary>Represents extended file properties for document files.</summary>
        string ClientID { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        MultiStringValue Contributor { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        DateTime DateCreated { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        string LastAuthor { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        string RevisionNumber { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        int Security { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        string Division { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        string DocumentID { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        string Manager { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        string PresentationFormat { get; }

        /// <summary>Represents extended file properties for document files.</summary>
        string Version { get; }
    }

    /// <summary>Represents extended file properties for audio files.</summary>
    public interface IAudioPropertiesTest
    {
        /// <summary>Represents extended file properties for audio files.</summary>
        string Compression { get; }

        /// <summary>Represents extended file properties for audio files.</summary>
        uint EncodingBitrate { get; }

        /// <summary>Represents extended file properties for audio files.</summary>
        string Format { get; }

        /// <summary>Represents extended file properties for audio files.</summary>
        bool IsVariableBitrate { get; }

        /// <summary>Represents extended file properties for audio files.</summary>
        uint SampleRate { get; }

        /// <summary>Represents extended file properties for audio files.</summary>
        uint SampleSize { get; }

        /// <summary>Represents extended file properties for audio files.</summary>
        string StreamName { get; }

        /// <summary>Represents extended file properties for audio files.</summary>
        ushort StreamNumber { get; }
    }

    /// <summary>Represents extended file properties for DRM information.</summary>
    public interface IDRMPropertiesTest
    {
        /// <summary>Represents extended file properties for DRM information.</summary>
        DateTime DatePlayExpires { get; }

        /// <summary>Represents extended file properties for DRM information.</summary>
        DateTime DatePlayStarts { get; }

        /// <summary>Represents extended file properties for DRM information.</summary>
        string Description { get; }

        /// <summary>Represents extended file properties for DRM information.</summary>
        bool IsProtected { get; }

        /// <summary>Represents extended file properties for DRM information.</summary>
        uint PlayCount { get; }
    }

    /// <summary>Represents extended file properties for GPS information.</summary>
    public interface IGPSPropertiesTest
    {
        /// <summary>Represents extended file properties for GPS information.</summary>
        string AreaInformation { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        double LatitudeDegrees { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        double LatitudeMinutes { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        double LatitudeSeconds { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        string LatitudeRef { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        double LongitudeDegrees { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        double LongitudeMinutes { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        double LongitudeSeconds { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        string LongitudeRef { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        string MeasureMode { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        string ProcessingMethod { get; }

        /// <summary>Represents extended file properties for GPS information.</summary>
        byte[] VersionID { get; }
    }

    /// <summary>Represents extended file properties for image files.</summary>
    public interface IImagePropertiesTest
    {
        /// <summary>Represents extended file properties for image files.</summary>
        uint BitDepth { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        ushort ColorSpace { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        double CompressedBitsPerPixel { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        ushort Compression { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        string CompressionText { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        double HorizontalResolution { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        uint HorizontalSize { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        string ImageID { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        short ResolutionUnit { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        double VerticalResolution { get; }

        /// <summary>Represents extended file properties for image files.</summary>
        uint VerticalSize { get; }
    }

    /// <summary>Represents extended file properties for media files.</summary>
    public interface IMediaPropertiesTest
    {
        /// <summary>Represents extended file properties for media files.</summary>
        string ContentDistributor { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        string CreatorApplication { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        string CreatorApplicationVersion { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        string DateReleased { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        ulong Duration { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        string DVDID { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        uint FrameCount { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        MultiStringValue Producer { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        string ProtectionType { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        string ProviderRating { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        string ProviderStyle { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        string Publisher { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        string Subtitle { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        MultiStringValue Writer { get; }

        /// <summary>Represents extended file properties for media files.</summary>
        uint Year { get; }
    }

    /// <summary>Represents extended file properties for music files.</summary>
    public interface IMusicPropertiesTest
    {
        /// <summary>Represents extended file properties for music files.</summary>
        string AlbumArtist { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        string AlbumTitle { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        MultiStringValue Artist { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        uint ChannelCount { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        MultiStringValue Composer { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        MultiStringValue Conductor { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        string DisplayArtist { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        MultiStringValue Genre { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        string PartOfSet { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        string Period { get; }

        /// <summary>Represents extended file properties for music files.</summary>
        uint TrackNumber { get; }
    }

    /// <summary>Represents extended file properties for photo files.</summary>
    public interface IPhotoPropertiesTest
    {
        /// <summary>Represents extended file properties for photo files.</summary>
        string CameraManufacturer { get; }

        /// <summary>Represents extended file properties for photo files.</summary>
        string CameraModel { get; }

        /// <summary>Represents extended file properties for photo files.</summary>
        DateTime DateTaken { get; }

        /// <summary>Represents extended file properties for photo files.</summary>
        MultiStringValue Event { get; }

        /// <summary>Represents extended file properties for photo files.</summary>
        string EXIFVersion { get; }

        /// <summary>Represents extended file properties for photo files.</summary>
        ushort Orientation { get; }

        /// <summary>Represents extended file properties for photo files.</summary>
        string OrientationText { get; }

        /// <summary>Represents extended file properties for photo files.</summary>
        MultiStringValue PeopleNames { get; }
    }

    /// <summary>Represents extended file properties for recorded TV files.</summary>
    public interface IRecordedTVPropertiesTest
    {
        /// <summary>Represents extended file properties for recorded TV files.</summary>
        uint ChannelNumber { get; }

        /// <summary>Represents extended file properties for recorded TV files.</summary>
        string EpisodeName { get; }

        /// <summary>Represents extended file properties for recorded TV files.</summary>
        bool IsDTVContent { get; }

        /// <summary>Represents extended file properties for recorded TV files.</summary>
        bool IsHDContent { get; }

        /// <summary>Represents extended file properties for recorded TV files.</summary>
        string NetworkAffiliation { get; }

        /// <summary>Represents extended file properties for recorded TV files.</summary>
        DateTime OriginalBroadcastDate { get; }

        /// <summary>Represents extended file properties for recorded TV files.</summary>
        string ProgramDescription { get; }

        /// <summary>Represents extended file properties for recorded TV files.</summary>
        string StationCallSign { get; }

        /// <summary>Represents extended file properties for recorded TV files.</summary>
        string StationName { get; }
    }

    /// <summary>Represents extended file properties for video files.</summary>
    public interface IVideoPropertiesTest
    {
        /// <summary>Represents extended file properties for video files.</summary>
        string Compression { get; }

        /// <summary>Represents extended file properties for video files.</summary>
        MultiStringValue Director { get; }

        /// <summary>Represents extended file properties for video files.</summary>
        uint EncodingBitrate { get; }

        /// <summary>Represents extended file properties for video files.</summary>
        uint FrameHeight { get; }

        /// <summary>Represents extended file properties for video files.</summary>
        uint FrameRate { get; }

        /// <summary>Represents extended file properties for video files.</summary>
        uint FrameWidth { get; }

        /// <summary>Represents extended file properties for video files.</summary>
        uint HorizontalAspectRatio { get; }

        /// <summary>Represents extended file properties for video files.</summary>
        ushort StreamNumber { get; }

        /// <summary>Represents extended file properties for video files.</summary>
        string StreamName { get; }

        /// <summary>Represents extended file properties for video files.</summary>
        uint VerticalAspectRatio { get; }
    }

    /// <summary>Base interface for entities that represent a grouping of extended file properties.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface IPropertySetTest : IDbEntity
    {
        /// <summary>Base interface for entities that represent a grouping of extended file properties.</summary>
        Guid Id { get; }

        /// <summary>Base interface for entities that represent a grouping of extended file properties.</summary>
        object Files { get; }
    }

    /// <summary>Interface for database objects that contain extended file summary property values.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="ISummaryProperties"></seealso>
    public interface ISummaryPropertySetTest : IPropertySet, ISummaryProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of document files.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IDocumentProperties"></seealso>
    public interface IDocumentPropertySetTest : IPropertySet, IDocumentProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of audio files.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IAudioProperties"></seealso>
    public interface IAudioPropertySetTest : IPropertySet, IAudioProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file DRM property values.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IDRMProperties"></seealso>
    public interface IDRMPropertySetTest : IPropertySet, IDRMProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file GPS property values.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IGPSProperties"></seealso>
    public interface IGPSPropertySetTest : IPropertySet, IGPSProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of image files.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IImageProperties"></seealso>
    public interface IImagePropertySetTest : IPropertySet, IImageProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of media files.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IMediaProperties"></seealso>
    public interface IMediaPropertySetTest : IPropertySet, IMediaProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of music files.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IMusicProperties"></seealso>
    public interface IMusicPropertySetTest : IPropertySet, IMusicProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of photo files.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IPhotoProperties"></seealso>
    public interface IPhotoPropertySetTest : IPropertySet, IPhotoProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of recorded TV files.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IRecordedTVProperties"></seealso>
    public interface IRecordedTVPropertySetTest : IPropertySet, IRecordedTVProperties
    {
    }

    /// <summary>Interface for database objects that contain extended file property values of video files.</summary>
    /// <seealso cref="IPropertySet"></seealso>
    /// <seealso cref="IVideoProperties"></seealso>
    public interface IVideoPropertySetTest : IPropertySet, IVideoProperties
    {
    }

    /// <summary>Represents a set of files that have the same size, Hash and remediation status.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface IRedundantSetTest : IDbEntity
    {
        /// <summary>Represents a set of files that have the same size, Hash and remediation status.</summary>
        Guid Id { get; }

        /// <summary>Represents a set of files that have the same size, Hash and remediation status.</summary>
        string Reference { get; }

        /// <summary>Represents a set of files that have the same size, Hash and remediation status.</summary>
        string Notes { get; }

        /// <summary>Represents a set of files that have the same size, Hash and remediation status.</summary>
        Guid BinaryPropertiesId { get; }

        /// <summary>Represents a set of files that have the same size, Hash and remediation status.</summary>
        object Redundancies { get; }
    }

    /// <summary>Represents a structural instance of file.</summary>
    /// <seealso cref="IDbFsItem"></seealso>
    public interface IFileTest : IDbFsItem
    {
        /// <summary>Represents a structural instance of file.</summary>
        object Options { get; }

        /// <summary>Represents a structural instance of file.</summary>
        object Status { get; }

        /// <summary>Represents a structural instance of file.</summary>
        DateTime LastHashCalculation { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid ParentId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid BinaryPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid SummaryPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid DocumentPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid AudioPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid DRMPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid GPSPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid ImagePropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid MediaPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid MusicPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid PhotoPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid RecordedTVPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid VideoPropertiesId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        Guid RedundancyId { get; }

        /// <summary>Represents a structural instance of file.</summary>
        object BaselineComparisons { get; }

        /// <summary>Represents a structural instance of file.</summary>
        object CorrelativeComparisons { get; }

        /// <summary>Represents a structural instance of file.</summary>
        object AccessErrors { get; }
    }

    /// <summary></summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface IRedundancyTest : IDbEntity
    {
        /// <summary></summary>
        Guid FileId { get; }

        /// <summary></summary>
        Guid RedundantSetId { get; }

        /// <summary></summary>
        string Reference { get; }

        /// <summary></summary>
        string Notes { get; }
    }

    /// <summary>The results of a byte-for-byte comparison of 2 files.</summary>
    /// <seealso cref="IDbEntity"></seealso>
    public interface IComparisonTest : IDbEntity
    {
        /// <summary>The results of a byte-for-byte comparison of 2 files.</summary>
        Guid BaselineId { get; }

        /// <summary>The results of a byte-for-byte comparison of 2 files.</summary>
        Guid CorrelativeId { get; }

        /// <summary>The results of a byte-for-byte comparison of 2 files.</summary>
        bool AreEqual { get; }

        /// <summary>The results of a byte-for-byte comparison of 2 files.</summary>
        DateTime ComparedOn { get; }
    }

    /// <summary>Generic interface for file access error entities.</summary>
    /// <seealso cref="IAccessError"></seealso>
    /// <seealso cref="IAccessError&lt;IFile&gt;"></seealso>
    public interface IFileAccessErrorTest : IAccessError, IAccessError<IFile>
    {
        /// <summary>Generic interface for file access error entities.</summary>
        Guid TargetId { get; }
    }

    /// <summary>Generic interface for subdirectory access error entities.</summary>
    /// <seealso cref="IAccessError"></seealso>
    /// <seealso cref="IAccessError&lt;ISubdirectory&gt;"></seealso>
    public interface ISubdirectoryAccessErrorTest : IAccessError, IAccessError<ISubdirectory>
    {
        /// <summary>Generic interface for subdirectory access error entities.</summary>
        Guid TargetId { get; }
    }

    /// <summary>Generic interface for volume access error entities.</summary>
    /// <seealso cref="IAccessError"></seealso>
    /// <seealso cref="IAccessError&lt;IVolume&gt;"></seealso>
    public interface IVolumeAccessErrorTest : IAccessError, IAccessError<IVolume>
    {
        /// <summary>Generic interface for volume access error entities.</summary>
        Guid TargetId { get; }
    }
}

