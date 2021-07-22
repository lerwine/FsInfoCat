-- Deleting tables

DROP TABLE IF EXISTS "Comparisons";
DROP TABLE IF EXISTS "Redundancies";
DROP TABLE IF EXISTS "FileAccessErrors";
DROP TABLE IF EXISTS "Files";
DROP TABLE IF EXISTS "RedundantSets";
DROP TABLE IF EXISTS "VideoPropertySets";
DROP TABLE IF EXISTS "RecordedTVPropertySets";
DROP TABLE IF EXISTS "PhotoPropertySets";
DROP TABLE IF EXISTS "MusicPropertySets";
DROP TABLE IF EXISTS "MediaPropertySets";
DROP TABLE IF EXISTS "ImagePropertySets";
DROP TABLE IF EXISTS "GPSPropertySets";
DROP TABLE IF EXISTS "DRMPropertySets";
DROP TABLE IF EXISTS "AudioPropertySets";
DROP TABLE IF EXISTS "DocumentPropertySets";
DROP TABLE IF EXISTS "SummaryPropertySets";
DROP TABLE IF EXISTS "BinaryPropertySets";
DROP TABLE IF EXISTS "SubdirectoryAccessErrors";
DROP TABLE IF EXISTS "CrawlJobLogs";
DROP TABLE IF EXISTS "CrawlConfigurations";
DROP TABLE IF EXISTS "Subdirectories";
DROP TABLE IF EXISTS "VolumeAccessErrors";
DROP TABLE IF EXISTS "Volumes";
DROP TABLE IF EXISTS "SymbolicNames";
DROP TABLE IF EXISTS "FileSystems";

-- Creating tables

CREATE TABLE IF NOT EXISTS "FileSystems" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"DisplayName" NVARCHAR(1024) CHECK(length(trim("DisplayName")) = length("DisplayName") AND length("DisplayName")>0) NOT NULL,
	"CaseSensitiveSearch" BIT NOT NULL DEFAULT 0,
	"ReadOnly" BIT NOT NULL DEFAULT 0,
	"MaxNameLength" UNSIGNED INT NOT NULL DEFAULT 255,
	 DEFAULT NULL,
	"Notes" TEXT NOT NULL DEFAULT '',
	"IsInactive" BIT NOT NULL DEFAULT 0,
	CONSTRAINT "PK_FileSystems" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "SymbolicNames" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"Name" NVARCHAR(256) CHECK(length(trim("Name")) = length("Name") AND length("Name")>0) NOT NULL,
	"Notes" TEXT NOT NULL DEFAULT '',
	"Priority" INT NOT NULL DEFAULT 0,
	"IsInactive" BIT NOT NULL DEFAULT 0,
	"FileSystemId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SymbolicNamesFileSystems" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_SymbolicNames" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "Volumes" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"DisplayName" NVARCHAR(1024) CHECK(length(trim("DisplayName")) = length("DisplayName") AND length("DisplayName")>0) NOT NULL,
	"VolumeName" NVARCHAR(128) CHECK(length(trim("VolumeName")) = length("VolumeName")) NOT NULL DEFAULT '',
	"Identifier" NVARCHAR(1024) CHECK(length(trim("Identifier")) = length("Identifier") AND length("Identifier")>0) NOT NULL,
	"CaseSensitiveSearch" BIT DEFAULT NULL,
	"ReadOnly" BIT DEFAULT NULL,
	"MaxNameLength" UNSIGNED INT DEFAULT NULL,
	 NOT NULL DEFAULT 0, -- DriveType.
	"Notes" TEXT NOT NULL DEFAULT '',
	"Status" UNSIGNED TINYINT NOT NULL DEFAULT 0, -- VolumeStatus.Unknown
	"RootDirectoryId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_VolumesSubdirectories" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"FileSystemId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_VolumesFileSystems" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_Volumes" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "VolumeAccessErrors" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"ErrorCode" UNSIGNED TINYINT NOT NULL,
	"Message" NVARCHAR(1024) CHECK(length(trim("Message")) = length("Message") AND length("Message")>0) NOT NULL,
	"Details" TEXT NOT NULL DEFAULT '',
	"TargetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_VolumeAccessErrorsVolumes" REFERENCES "Volumes"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_VolumeAccessErrors" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn)
);
CREATE TABLE IF NOT EXISTS "Subdirectories" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"Name" NVARCHAR(1024) NOT NULL,
	"LastAccessed" DATETIME NOT NULL,
	"Notes" TEXT NOT NULL DEFAULT '',
	"CreationTime" DATETIME NOT NULL,
	"LastWriteTime" DATETIME NOT NULL,
	"ParentId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SubdirectoriesParent" REFERENCES "Parent"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"Options" UNSIGNED TINYINT NOT NULL DEFAULT 0, -- DirectoryCrawlOptions.None
	"Status" UNSIGNED TINYINT NOT NULL DEFAULT 0, -- DirectoryStatus.Incomplete
	"VolumeId" UNIQUEIDENTIFIER CONSTRAINT "FK_SubdirectoriesVolumes" REFERENCES "Volumes"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"CrawlConfigurationId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SubdirectoriesCrawlConfigurations" REFERENCES "CrawlConfigurations"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_Subdirectories" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "CrawlConfigurations" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"MaxRecursionDepth" UNSIGNED SMALLINT NOT NULL DEFAULT 32,
	"MaxTotalItems" UNSIGNED BIGINT DEFAULT NULL,
	"TTL" BIGINT DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"DisplayName" NVARCHAR(1024) CHECK(length(trim("DisplayName")) = length("DisplayName") AND length("DisplayName")>0) NOT NULL,
	"Notes" TEXT NOT NULL DEFAULT '',
	"StatusValue" UNSIGNED TINYINT NOT NULL DEFAULT 0, -- CrawlStatus.NotRunning
	"LastCrawlStart" DATETIME DEFAULT NULL,
	"LastCrawlEnd" DATETIME DEFAULT NULL,
	"NextScheduledStart" DATETIME DEFAULT NULL,
	"RescheduleInterval" BIGINT DEFAULT NULL,
	"RescheduleFromJobEnd" BIT NOT NULL DEFAULT 0,
	"RescheduleAfterFail" BIT NOT NULL DEFAULT 0,
	"RootId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_CrawlConfigurationsSubdirectories" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_CrawlConfigurations" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "CrawlJobLogs" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"MaxRecursionDepth" UNSIGNED SMALLINT NOT NULL DEFAULT 32,
	"MaxTotalItems" UNSIGNED BIGINT DEFAULT NULL,
	"TTL" BIGINT DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"RootPath" NVARCHAR(4096) NOT NULL,
	"StatusCode" UNSIGNED TINYINT NOT NULL DEFAULT 0, -- CrawlStatus.NotRunning
	"CrawlStart" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"CrawlEnd" DATETIME DEFAULT NULL,
	"StatusMessage" NVARCHAR(1024) CHECK(length(trim("StatusMessage")) = length("StatusMessage")) NOT NULL,
	"StatusDetail" TEXT NOT NULL,
	"ConfigurationId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_CrawlJobLogsCrawlConfigurations" REFERENCES "CrawlConfigurations"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_CrawlJobLogs" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "SubdirectoryAccessErrors" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"ErrorCode" UNSIGNED TINYINT NOT NULL,
	"Message" NVARCHAR(1024) CHECK(length(trim("Message")) = length("Message") AND length("Message")>0) NOT NULL,
	"Details" TEXT NOT NULL DEFAULT '',
	"TargetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SubdirectoryAccessErrorsSubdirectories" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_SubdirectoryAccessErrors" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn)
);
CREATE TABLE IF NOT EXISTS "BinaryPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"Length" BIGINT NOT NULL,
	"Hash" BINARY(16) DEFAULT NULL,
	CONSTRAINT "PK_BinaryPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "SummaryPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"ApplicationName" NVARCHAR(1024) CHECK("ApplicationName" IS NULL OR length(trim("ApplicationName")) = length("ApplicationName")) DEFAULT NULL,
	"Author" TEXT DEFAULT NULL,
	"Comment" NVARCHAR(1024) DEFAULT NULL,
	"Keywords" TEXT DEFAULT NULL,
	"Subject" NVARCHAR(1024) CHECK("Subject" IS NULL OR length(trim("Subject")) = length("Subject")) DEFAULT NULL,
	"Title" NVARCHAR(1024) CHECK("Title" IS NULL OR length(trim("Title")) = length("Title")) DEFAULT NULL,
	"Company" NVARCHAR(1024) CHECK("Company" IS NULL OR length(trim("Company")) = length("Company")) DEFAULT NULL,
	"ContentType" NVARCHAR(1024) CHECK("ContentType" IS NULL OR length(trim("ContentType")) = length("ContentType")) DEFAULT NULL,
	"Copyright" NVARCHAR(1024) CHECK("Copyright" IS NULL OR length(trim("Copyright")) = length("Copyright")) DEFAULT NULL,
	"ParentalRating" NVARCHAR(32) CHECK("ParentalRating" IS NULL OR length(trim("ParentalRating")) = length("ParentalRating")) DEFAULT NULL,
	"Rating" UNSIGNED INT DEFAULT NULL,
	"ItemAuthors" TEXT DEFAULT NULL,
	"ItemType" NVARCHAR(32) CHECK("ItemType" IS NULL OR length(trim("ItemType")) = length("ItemType")) DEFAULT NULL,
	"ItemTypeText" NVARCHAR(64) CHECK("ItemTypeText" IS NULL OR length(trim("ItemTypeText")) = length("ItemTypeText")) DEFAULT NULL,
	"Kind" TEXT DEFAULT NULL,
	"MIMEType" NVARCHAR(1024) CHECK("MIMEType" IS NULL OR length(trim("MIMEType")) = length("MIMEType")) DEFAULT NULL,
	"ParentalRatingReason" NVARCHAR(1024) DEFAULT NULL,
	"ParentalRatingsOrganization" NVARCHAR(1024) CHECK("ParentalRatingsOrganization" IS NULL OR length(trim("ParentalRatingsOrganization")) = length("ParentalRatingsOrganization")) DEFAULT NULL,
	"Sensitivity" UNSIGNED SMALLINT DEFAULT NULL,
	"SensitivityText" NVARCHAR(1024) CHECK("SensitivityText" IS NULL OR length(trim("SensitivityText")) = length("SensitivityText")) DEFAULT NULL,
	"SimpleRating" UNSIGNED INT DEFAULT NULL,
	"Trademarks" NVARCHAR(1024) CHECK("Trademarks" IS NULL OR length(trim("Trademarks")) = length("Trademarks")) DEFAULT NULL,
	"ProductName" NVARCHAR(256) CHECK("ProductName" IS NULL OR length(trim("ProductName")) = length("ProductName")) DEFAULT NULL,
	CONSTRAINT "PK_SummaryPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "DocumentPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"ClientID" NVARCHAR(64) CHECK("ClientID" IS NULL OR length(trim("ClientID")) = length("ClientID")) DEFAULT NULL,
	"Contributor" TEXT DEFAULT NULL,
	"DateCreated" DATETIME DEFAULT NULL,
	"LastAuthor" NVARCHAR(1024) CHECK("LastAuthor" IS NULL OR length(trim("LastAuthor")) = length("LastAuthor")) DEFAULT NULL,
	"RevisionNumber" NVARCHAR(64) CHECK("RevisionNumber" IS NULL OR length(trim("RevisionNumber")) = length("RevisionNumber")) DEFAULT NULL,
	"Security" INT DEFAULT NULL,
	"Division" NVARCHAR(256) CHECK("Division" IS NULL OR length(trim("Division")) = length("Division")) DEFAULT NULL,
	"DocumentID" NVARCHAR(64) CHECK("DocumentID" IS NULL OR length(trim("DocumentID")) = length("DocumentID")) DEFAULT NULL,
	"Manager" NVARCHAR(256) CHECK("Manager" IS NULL OR length(trim("Manager")) = length("Manager")) DEFAULT NULL,
	"PresentationFormat" NVARCHAR(256) CHECK("PresentationFormat" IS NULL OR length(trim("PresentationFormat")) = length("PresentationFormat")) DEFAULT NULL,
	"Version" NVARCHAR(64) CHECK("Version" IS NULL OR length(trim("Version")) = length("Version")) DEFAULT NULL,
	CONSTRAINT "PK_DocumentPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "AudioPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"Compression" NVARCHAR(256) CHECK("Compression" IS NULL OR length(trim("Compression")) = length("Compression")) DEFAULT NULL,
	"EncodingBitrate" UNSIGNED INT DEFAULT NULL,
	"Format" NVARCHAR(256) CHECK("Format" IS NULL OR length(trim("Format")) = length("Format")) DEFAULT NULL,
	"IsVariableBitrate" BIT DEFAULT NULL,
	"SampleRate" UNSIGNED INT DEFAULT NULL,
	"SampleSize" UNSIGNED INT DEFAULT NULL,
	"StreamName" NVARCHAR(256) DEFAULT NULL,
	"StreamNumber" UNSIGNED SMALLINT DEFAULT NULL,
	CONSTRAINT "PK_AudioPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "DRMPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"DatePlayExpires" DATETIME DEFAULT NULL,
	"DatePlayStarts" DATETIME DEFAULT NULL,
	"Description" TEXT DEFAULT NULL,
	"IsProtected" BIT DEFAULT NULL,
	"PlayCount" UNSIGNED INT DEFAULT NULL,
	CONSTRAINT "PK_DRMPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "GPSPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"AreaInformation" NVARCHAR(1024) CHECK("AreaInformation" IS NULL OR length(trim("AreaInformation")) = length("AreaInformation")) DEFAULT NULL,
	"LatitudeDegrees" REAL DEFAULT NULL,
	"LatitudeMinutes" REAL DEFAULT NULL,
	"LatitudeSeconds" REAL DEFAULT NULL,
	"LatitudeRef" NVARCHAR(256) CHECK("LatitudeRef" IS NULL OR length(trim("LatitudeRef")) = length("LatitudeRef")) DEFAULT NULL,
	"LongitudeDegrees" REAL DEFAULT NULL,
	"LongitudeMinutes" REAL DEFAULT NULL,
	"LongitudeSeconds" REAL DEFAULT NULL,
	"LongitudeRef" NVARCHAR(256) CHECK("LongitudeRef" IS NULL OR length(trim("LongitudeRef")) = length("LongitudeRef")) DEFAULT NULL,
	"MeasureMode" NVARCHAR(256) CHECK("MeasureMode" IS NULL OR length(trim("MeasureMode")) = length("MeasureMode")) DEFAULT NULL,
	"ProcessingMethod" NVARCHAR(256) CHECK("ProcessingMethod" IS NULL OR length(trim("ProcessingMethod")) = length("ProcessingMethod")) DEFAULT NULL,
	"VersionID" VARBINARY(128) DEFAULT NULL,
	CONSTRAINT "PK_GPSPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "ImagePropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"BitDepth" UNSIGNED INT DEFAULT NULL,
	"ColorSpace" UNSIGNED SMALLINT DEFAULT NULL,
	"CompressedBitsPerPixel" REAL DEFAULT NULL,
	"Compression" UNSIGNED SMALLINT DEFAULT NULL,
	"CompressionText" NVARCHAR(256) CHECK("CompressionText" IS NULL OR length(trim("CompressionText")) = length("CompressionText")) DEFAULT NULL,
	"HorizontalResolution" REAL DEFAULT NULL,
	"HorizontalSize" UNSIGNED INT DEFAULT NULL,
	"ImageID" NVARCHAR(256) CHECK("ImageID" IS NULL OR length(trim("ImageID")) = length("ImageID")) DEFAULT NULL,
	"ResolutionUnit" SMALLINT DEFAULT NULL,
	"VerticalResolution" REAL DEFAULT NULL,
	"VerticalSize" UNSIGNED INT DEFAULT NULL,
	CONSTRAINT "PK_ImagePropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "MediaPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"ContentDistributor" NVARCHAR(256) CHECK("ContentDistributor" IS NULL OR length(trim("ContentDistributor")) = length("ContentDistributor")) DEFAULT NULL,
	"CreatorApplication" NVARCHAR(256) CHECK("CreatorApplication" IS NULL OR length(trim("CreatorApplication")) = length("CreatorApplication")) DEFAULT NULL,
	"CreatorApplicationVersion" NVARCHAR(64) CHECK("CreatorApplicationVersion" IS NULL OR length(trim("CreatorApplicationVersion")) = length("CreatorApplicationVersion")) DEFAULT NULL,
	"DateReleased" NVARCHAR(64) CHECK("DateReleased" IS NULL OR length(trim("DateReleased")) = length("DateReleased")) DEFAULT NULL,
	"Duration" UNSIGNED BIGINT DEFAULT NULL,
	"DVDID" NVARCHAR(64) CHECK("DVDID" IS NULL OR length(trim("DVDID")) = length("DVDID")) DEFAULT NULL,
	"FrameCount" UNSIGNED INT DEFAULT NULL,
	"Producer" TEXT DEFAULT NULL,
	"ProtectionType" NVARCHAR(256) CHECK("ProtectionType" IS NULL OR length(trim("ProtectionType")) = length("ProtectionType")) DEFAULT NULL,
	"ProviderRating" NVARCHAR(256) CHECK("ProviderRating" IS NULL OR length(trim("ProviderRating")) = length("ProviderRating")) DEFAULT NULL,
	"ProviderStyle" NVARCHAR(256) CHECK("ProviderStyle" IS NULL OR length(trim("ProviderStyle")) = length("ProviderStyle")) DEFAULT NULL,
	"Publisher" NVARCHAR(256) CHECK("Publisher" IS NULL OR length(trim("Publisher")) = length("Publisher")) DEFAULT NULL,
	"Subtitle" NVARCHAR(256) CHECK("Subtitle" IS NULL OR length(trim("Subtitle")) = length("Subtitle")) DEFAULT NULL,
	"Writer" TEXT DEFAULT NULL,
	"Year" UNSIGNED INT DEFAULT NULL,
	CONSTRAINT "PK_MediaPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "MusicPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"AlbumArtist" NVARCHAR(1024) CHECK("AlbumArtist" IS NULL OR length(trim("AlbumArtist")) = length("AlbumArtist")) DEFAULT NULL,
	"AlbumTitle" NVARCHAR(1024) CHECK("AlbumTitle" IS NULL OR length(trim("AlbumTitle")) = length("AlbumTitle")) DEFAULT NULL,
	"Artist" TEXT DEFAULT NULL,
	"ChannelCount" UNSIGNED INT DEFAULT NULL,
	"Composer" TEXT DEFAULT NULL,
	"Conductor" TEXT DEFAULT NULL,
	"DisplayArtist" TEXT DEFAULT NULL,
	"Genre" TEXT DEFAULT NULL,
	"PartOfSet" NVARCHAR(64) CHECK("PartOfSet" IS NULL OR length(trim("PartOfSet")) = length("PartOfSet")) DEFAULT NULL,
	"Period" NVARCHAR(64) CHECK("Period" IS NULL OR length(trim("Period")) = length("Period")) DEFAULT NULL,
	"TrackNumber" UNSIGNED INT DEFAULT NULL,
	CONSTRAINT "PK_MusicPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "PhotoPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"CameraManufacturer" NVARCHAR(256) CHECK("CameraManufacturer" IS NULL OR length(trim("CameraManufacturer")) = length("CameraManufacturer")) DEFAULT NULL,
	"CameraModel" NVARCHAR(256) CHECK("CameraModel" IS NULL OR length(trim("CameraModel")) = length("CameraModel")) DEFAULT NULL,
	"DateTaken" DATETIME DEFAULT NULL,
	"Event" TEXT DEFAULT NULL,
	"EXIFVersion" NVARCHAR(256) CHECK("EXIFVersion" IS NULL OR length(trim("EXIFVersion")) = length("EXIFVersion")) DEFAULT NULL,
	"Orientation" UNSIGNED SMALLINT DEFAULT NULL,
	"OrientationText" NVARCHAR(256) CHECK("OrientationText" IS NULL OR length(trim("OrientationText")) = length("OrientationText")) DEFAULT NULL,
	"PeopleNames" TEXT DEFAULT NULL,
	CONSTRAINT "PK_PhotoPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "RecordedTVPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"ChannelNumber" UNSIGNED INT DEFAULT NULL,
	"EpisodeName" NVARCHAR(1024) CHECK("EpisodeName" IS NULL OR length(trim("EpisodeName")) = length("EpisodeName")) DEFAULT NULL,
	"IsDTVContent" BIT DEFAULT NULL,
	"IsHDContent" BIT DEFAULT NULL,
	"NetworkAffiliation" NVARCHAR(256) CHECK("NetworkAffiliation" IS NULL OR length(trim("NetworkAffiliation")) = length("NetworkAffiliation")) DEFAULT NULL,
	"OriginalBroadcastDate" DATETIME DEFAULT NULL,
	"ProgramDescription" TEXT DEFAULT NULL,
	"StationCallSign" NVARCHAR(32) CHECK("StationCallSign" IS NULL OR length(trim("StationCallSign")) = length("StationCallSign")) DEFAULT NULL,
	"StationName" NVARCHAR(256) CHECK("StationName" IS NULL OR length(trim("StationName")) = length("StationName")) DEFAULT NULL,
	CONSTRAINT "PK_RecordedTVPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "VideoPropertySets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"Compression" NVARCHAR(256) CHECK("Compression" IS NULL OR length(trim("Compression")) = length("Compression")) DEFAULT NULL,
	"Director" TEXT DEFAULT NULL,
	"EncodingBitrate" UNSIGNED INT DEFAULT NULL,
	"FrameHeight" UNSIGNED INT DEFAULT NULL,
	"FrameRate" UNSIGNED INT DEFAULT NULL,
	"FrameWidth" UNSIGNED INT DEFAULT NULL,
	"HorizontalAspectRatio" UNSIGNED INT DEFAULT NULL,
	"StreamNumber" UNSIGNED SMALLINT DEFAULT NULL,
	"StreamName" NVARCHAR(256) CHECK("StreamName" IS NULL OR length(trim("StreamName")) = length("StreamName")) DEFAULT NULL,
	"VerticalAspectRatio" UNSIGNED INT DEFAULT NULL,
	CONSTRAINT "PK_VideoPropertySets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "RedundantSets" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"Reference" NVARCHAR(128) CHECK(length(trim("Reference")) = length("Reference")) NOT NULL,
	"Notes" TEXT NOT NULL DEFAULT '',
	"BinaryPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundantSetsBinaryPropertySets" REFERENCES "BinaryPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_RedundantSets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "Files" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"Name" NVARCHAR(1024) NOT NULL,
	"LastAccessed" DATETIME NOT NULL,
	"Notes" TEXT NOT NULL DEFAULT '',
	"CreationTime" DATETIME NOT NULL,
	"LastWriteTime" DATETIME NOT NULL,
	"ParentId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesSubdirectories" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"Options" UNSIGNED TINYINT NOT NULL DEFAULT 0, -- FileCrawlOptions.None
	"Status" UNSIGNED TINYINT NOT NULL DEFAULT 0, -- FileCorrelationStatus.Dissociated
	"LastHashCalculation" DATETIME DEFAULT NULL,
	"BinaryPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesBinaryPropertySets" REFERENCES "BinaryPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"SummaryPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesSummaryPropertySets" REFERENCES "SummaryPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"DocumentPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesDocumentPropertySets" REFERENCES "DocumentPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"AudioPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesAudioPropertySets" REFERENCES "AudioPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"DRMPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesDRMPropertySets" REFERENCES "DRMPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"GPSPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesGPSPropertySets" REFERENCES "GPSPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"ImagePropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesImagePropertySets" REFERENCES "ImagePropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"MediaPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesMediaPropertySets" REFERENCES "MediaPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"MusicPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesMusicPropertySets" REFERENCES "MusicPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"PhotoPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesPhotoPropertySets" REFERENCES "PhotoPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"RecordedTVPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesRecordedTVPropertySets" REFERENCES "RecordedTVPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"VideoPropertiesId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesVideoPropertySets" REFERENCES "VideoPropertySets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"RedundancyId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FilesRedundancies" REFERENCES "Redundancies"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_Files" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "FileAccessErrors" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id" UNIQUEIDENTIFIER NOT NULL,
	"ErrorCode" UNSIGNED TINYINT NOT NULL,
	"Message" NVARCHAR(1024) CHECK(length(trim("Message")) = length("Message") AND length("Message")>0) NOT NULL,
	"Details" TEXT NOT NULL DEFAULT '',
	"TargetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FileAccessErrorsFiles" REFERENCES "Files"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_FileAccessErrors" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn)
);
CREATE TABLE IF NOT EXISTS "Redundancies" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"Reference" NVARCHAR(128) CHECK(length(trim("Reference")) = length("Reference")) NOT NULL,
	"Notes" TEXT NOT NULL DEFAULT '',
	"FileId" UNIQUEIDENTIFIER NOT NULL,
	"FileId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundanciesFiles" REFERENCES "Files"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"RedundantSetId" UNIQUEIDENTIFIER NOT NULL,
	"RedundantSetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundanciesRedundantSets" REFERENCES "RedundantSets"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_Redundancies" PRIMARY KEY("FileId", "RedundantSetId"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE IF NOT EXISTS "Comparisons" (
	"CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"AreEqual" BIT NOT NULL DEFAULT 0,
	"ComparedOn" DATETIME NOT NULL,
	"BaselineId" UNIQUEIDENTIFIER NOT NULL,
	"BaselineId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_ComparisonsFiles" REFERENCES "Files"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	"CorrelativeId" UNIQUEIDENTIFIER NOT NULL,
	"CorrelativeId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_ComparisonsFiles" REFERENCES "Files"("Id") ON DELETE RESTRICT,
	#bug Need some way to override AllowNull for entity references
	CONSTRAINT "PK_Comparisons" PRIMARY KEY(""),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
