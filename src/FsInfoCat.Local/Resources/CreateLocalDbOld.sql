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

DROP TABLE IF EXISTS "CrawlConfigurations";
DROP TABLE IF EXISTS "Subdirectories";
DROP TABLE IF EXISTS "VolumeAccessErrors";
DROP TABLE IF EXISTS "Volumes";
DROP TABLE IF EXISTS "SymbolicNames";
DROP TABLE IF EXISTS "FileSystems";

-- Creating tables

CREATE TABLE IF NOT EXISTS "FileSystems" (
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
	"DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim("DisplayName")) = length("DisplayName") AND length("DisplayName")>0) UNIQUE COLLATE NOCASE,
	"CaseSensitiveSearch" BIT NOT NULL DEFAULT 0,
	"ReadOnly" BIT NOT NULL DEFAULT 0,
	"MaxNameLength" UNSIGNED INT NOT NULL DEFAULT 255,
	"DefaultDriveType" UNSIGNED TINYINT CHECK("DefaultDriveType" IS NULL OR ("DefaultDriveType">=0 AND DefaultDriveType<7)) DEFAULT NULL,
	"Notes" TEXT NOT NULL DEFAULT '',
	"IsInactive" BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	CONSTRAINT "PK_FileSystems" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE INDEX "IDX_FileSystem_DisplayName" ON "FileSystems" ("DisplayName" COLLATE NOCASE);

CREATE TABLE IF NOT EXISTS "SymbolicNames" (
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Name"	NVARCHAR(256) NOT NULL CHECK(length(trim(Name)) = length(Name) AND length(Name)>0) UNIQUE COLLATE NOCASE,
    "Priority" INT NOT NULL DEFAULT 0,
	"Notes"	TEXT NOT NULL DEFAULT '',
	"IsInactive"	BIT NOT NULL DEFAULT 0,
	"FileSystemId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SymbolicNameFileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	CONSTRAINT "PK_SymbolicNames" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE INDEX "IDX_SymbolicName_Name" ON "SymbolicNames" ("Name" COLLATE NOCASE);

CREATE INDEX "IDX_SymbolicName_IsInactive" ON "SymbolicNames" ("IsInactive");

CREATE TABLE IF NOT EXISTS "Volumes" (
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName)) = length(DisplayName) AND length(DisplayName)>0) COLLATE NOCASE,
    "VolumeName" NVARCHAR(128) NOT NULL CHECK(length(trim(VolumeName)) = length(VolumeName)) COLLATE NOCASE,
    "Identifier" NVARCHAR(1024) NOT NULL CHECK(length(trim(Identifier)) = length(Identifier) AND length(Identifier)>0) UNIQUE COLLATE NOCASE,
    "CaseSensitiveSearch" BIT DEFAULT NULL,
    "ReadOnly" BIT DEFAULT NULL,
    "MaxNameLength" INT CHECK(MaxNameLength IS NULL OR ("MaxNameLength">0 AND "MaxNameLength"<4294967296)) DEFAULT NULL,
    "Type" TINYINT NOT NULL CHECK(Type>=0 AND Type<7) DEFAULT 0,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Status" TINYINT NOT NULL CHECK(Type>=0 AND Type<7) DEFAULT 0,
	"FileSystemId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_VolumeFileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	CONSTRAINT "PK_Volumes" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE INDEX "IDX_Volume_Identifier" ON "Volumes" ("Identifier" COLLATE NOCASE);

CREATE INDEX "IDX_Volume_Status" ON "Volumes" ("Status");

CREATE TABLE IF NOT EXISTS "VolumeAccessErrors" (
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "ErrorCode" INT NOT NULL,
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message")) = length("Message") AND length("Message")>0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
	"TargetId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessErrorVolume" REFERENCES "Volume"("Id") ON DELETE RESTRICT,
	CONSTRAINT "PK_VolumeAccessErrors" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn)
);

CREATE TABLE IF NOT EXISTS "Subdirectories" (
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Name" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "LastAccessed" DATETIME  NOT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
	"CreationTime"	DATETIME NOT NULL,
	"LastWriteTime"	DATETIME NOT NULL,
	"ParentId"	UNIQUEIDENTIFIER CONSTRAINT "FK_SubdirectoryParent" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "Options" TINYINT  NOT NULL CHECK(Options>=0 AND Options<64) DEFAULT 0,
    "Status" TINYINT  NOT NULL CHECK(Options>=0 AND Options<4) DEFAULT 0,
	"VolumeId"	UNIQUEIDENTIFIER CONSTRAINT "FK_SubdirectoryVolume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT,
    "CrawlConfigurationId"	UNIQUEIDENTIFIER CONSTRAINT "FK_SubdirectoryCrawlConfiguration" REFERENCES CrawlConfigurations("Id") ON DELETE RESTRICT,
	CONSTRAINT "PK_Subdirectories" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND LastAccessed >= CreatedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND ((ParentId IS NULL AND VolumeId IS NOT NULL) OR (ParentId IS NOT NULL AND VolumeId IS NULL AND length(trim(Name))>0)))
);

CREATE INDEX "IDX_Subdirectory_Status" ON "Subdirectories" ("Status");

CREATE TABLE IF NOT EXISTS "CrawlConfigurations" (
    "MaxRecursionDepth" INT NOT NULL CHECK("MaxRecursionDepth">=0 AND "MaxRecursionDepth"<65536) DEFAULT 256,
    "MaxTotalItems" BIGINT NOT NULL CHECK("MaxTotalItems">0) DEFAULT 18446744073709551615,
    "TTL" BIGINT CHECK("TTL" IS NULL OR ("TTL">0 AND "TTL"<9223372036854775808)) DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim("DisplayName")) = length("DisplayName") AND length("DisplayName")>0) COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
	"IsInactive"	BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	CONSTRAINT "PK_CrawlConfigurations" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE INDEX "IDX_CrawlConfiguration_IsInactive" ON "CrawlConfigurations" ("IsInactive");

CREATE TABLE IF NOT EXISTS "SubdirectoryAccessErrors" (
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "ErrorCode" INT NOT NULL,
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message")) = length("Message") AND length("Message")>0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
	"TargetId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessErrorSubdirectory" REFERENCES "Subdirectory"("Id") ON DELETE RESTRICT,
	CONSTRAINT "PK_SubdirectoryccessErrors" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn)
);

CREATE TABLE IF NOT EXISTS "BinaryPropertySets" (
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Length"	BIGINT NOT NULL CHECK(Length>=0),
	"Hash"	BINARY(16) CHECK(Hash IS NULL OR length(HASH)=16),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	CONSTRAINT "PK_BinaryPropertySet" PRIMARY KEY("Id"),
	CONSTRAINT "UK_LengthHash" UNIQUE("Length","Hash"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE INDEX "IDX_BinaryPropertySet_Length" ON "BinaryPropertySets" ("Length");

CREATE INDEX "IDX_BinaryPropertySet_Hash" ON "BinaryPropertySets" ("Hash");

CREATE TABLE IF NOT EXISTS "SummaryPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"ApplicationName" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"Author" TEXT DEFAULT NULL COLLATE NOCASE,
	"Comment" TEXT DEFAULT NULL COLLATE NOCASE,
	"Keywords" TEXT DEFAULT NULL COLLATE NOCASE,
	"Subject" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"Title" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"Category" TEXT DEFAULT NULL COLLATE NOCASE,
	"Company" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"ContentType" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"Copyright" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"ParentalRating" NVARCHAR(32) DEFAULT NULL COLLATE NOCASE,
	"Rating" TINYINT CHECK("Rating" IS NULL OR ("Rating">0 AND "Rating"<100)) DEFAULT NULL,
	"ItemAuthors" TEXT DEFAULT NULL COLLATE NOCASE,
	"ItemType" NVARCHAR(32) DEFAULT NULL COLLATE NOCASE,
	"ItemTypeText" NVARCHAR(64) DEFAULT NULL COLLATE NOCASE,
	"Kind" TEXT DEFAULT NULL COLLATE NOCASE,
	"MIMEType" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"ParentalRatingReason" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"ParentalRatingsOrganization" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"Sensitivity" INT CHECK("Sensitivity" IS NULL OR ("Sensitivity">=0 AND "Sensitivity"<65536)) DEFAULT NULL,
	"SensitivityText" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"SimpleRating" TinyInt CHECK("SimpleRating" IS NULL OR ("SimpleRating">=0 AND "SimpleRating"<6)) DEFAULT NULL,
	"Trademarks" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"ProductName" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_SummaryPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "DocumentPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"ClientID" NVARCHAR(64) DEFAULT NULL COLLATE NOCASE,
	"Contributor" TEXT DEFAULT NULL COLLATE NOCASE,
	"DateCreated" DATETIME DEFAULT NULL,
	"LastAuthor" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"RevisionNumber" NVARCHAR(64) DEFAULT NULL COLLATE NOCASE,
	"Security" INT DEFAULT NULL,
	"Division" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"DocumentID" NVARCHAR(64) DEFAULT NULL COLLATE NOCASE,
	"Manager" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"PresentationFormat" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"Version" NVARCHAR(64) DEFAULT NULL COLLATE NOCASE,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_DocumentPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "AudioPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Compression" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"EncodingBitrate" BIGINT CHECK("EncodingBitrate" IS NULL OR ("EncodingBitrate">=0 AND "EncodingBitrate"<4294967296)) DEFAULT NULL,
	"Format" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"IsVariableBitrate" BIT DEFAULT NULL,
	"SampleRate" BIGINT CHECK("SampleRate" IS NULL OR ("SampleRate">=0 AND "SampleRate"<4294967296)) DEFAULT NULL,
	"SampleSize" BIGINT CHECK("SampleSize" IS NULL OR ("SampleSize">=0 AND "SampleSize"<4294967296)) DEFAULT NULL,
	"StreamName" NVARCHAR(256) DEFAULT NULL,
	"StreamNumber" INT CHECK("StreamNumber" IS NULL OR ("StreamNumber">=0 AND "StreamNumber"<65536)) DEFAULT NULL,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_AudioPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "DRMPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"DatePlayExpires" DATETIME DEFAULT NULL,
	"DatePlayStarts" DATETIME DEFAULT NULL,
	"Description" TEXT DEFAULT NULL COLLATE NOCASE,
	"IsProtected" BIT DEFAULT NULL,
	"PlayCount" BIGINT CHECK("PlayCount" IS NULL OR ("PlayCount">=0 AND "PlayCount"<4294967296)) DEFAULT NULL,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_DRMPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "GPSPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"AreaInformation" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"LatitudeDegrees" DOUBLE DEFAULT NULL,
	"LatitudeMinutes" DOUBLE DEFAULT NULL,
	"Latitude" DOUBLE DEFAULT NULL,
	"LatitudeRef" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"LongitudeDegrees" DOUBLE DEFAULT NULL,
	"LongitudeMinutes" DOUBLE DEFAULT NULL,
	"LongitudeSeconds" DOUBLE DEFAULT NULL,
	"LongitudeRef" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"MeasureMode" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"ProcessingMethod" NVARCHAR(256) DEFAULT NULL,
	"VersionID" NVARCHAR(128) DEFAULT NULL COLLATE NOCASE,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_GPSPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "ImagePropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"BitDepth" BIGINT CHECK("BitDepth" IS NULL OR ("BitDepth">=0 AND "BitDepth"<4294967296)) DEFAULT NULL,
	"ColorSpace" INT CHECK("ColorSpace" IS NULL OR ("ColorSpace">=0 AND "ColorSpace"<65536)) DEFAULT NULL,
	"CompressedBitsPerPixel" DOUBLE DEFAULT NULL,
	"Compression" INT CHECK("Compression" IS NULL OR ("Compression">=0 AND "Compression"<65536)) DEFAULT NULL,
	"CompressionText" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"HorizontalResolution" DOUBLE DEFAULT NULL,
	"HorizontalSize" BIGINT CHECK("HorizontalSize" IS NULL OR ("HorizontalSize">=0 AND "HorizontalSize"<4294967296)) DEFAULT NULL,
	"ImageID" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"ResolutionUnit" TINYINT DEFAULT NULL,
	"VerticalResolution" DOUBLE DEFAULT NULL,
	"VerticalSize" BIGINT CHECK("VerticalSize" IS NULL OR ("VerticalSize">=0 AND "VerticalSize"<4294967296)) DEFAULT NULL,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_ImagePropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "MediaPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"ContentDistributor" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"CreatorApplication" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"CreatorApplicationVersion" NVARCHAR(64) DEFAULT NULL COLLATE NOCASE,
	"DateReleased" NVARCHAR(64) DEFAULT NULL,
	"Duration" BIGINT DEFAULT NULL,
	"DVDID" NVARCHAR(64) DEFAULT NULL COLLATE NOCASE,
	"FrameCount" BIGINT CHECK("FrameCount" IS NULL OR ("FrameCount">=0 AND "FrameCount"<4294967296)) DEFAULT NULL,
	"Producer" TEXT DEFAULT NULL COLLATE NOCASE,
	"ProtectionType" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"ProviderRating" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"ProviderStyle" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"Publisher" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"Subtitle" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"Writer" TEXT DEFAULT NULL COLLATE NOCASE,
	"Year" BIGINT CHECK("Year" IS NULL OR ("Year">=0 AND "Year"<4294967296)) DEFAULT NULL,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_MediaPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "MusicPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"AlbumArtist" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"AlbumTitle" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"Artist" TEXT DEFAULT NULL COLLATE NOCASE,
	"Composer" TEXT DEFAULT NULL COLLATE NOCASE,
	"Conductor" TEXT DEFAULT NULL COLLATE NOCASE,
	"DisplayArtist" TEXT DEFAULT NULL COLLATE NOCASE,
	"Genre" TEXT DEFAULT NULL COLLATE NOCASE,
	"PartOfSet" NVARCHAR(64) DEFAULT NULL COLLATE NOCASE,
	"Period" NVARCHAR(64) DEFAULT NULL COLLATE NOCASE,
	"TrackNumber" BIGINT CHECK("TrackNumber" IS NULL OR ("TrackNumber">=0 AND "TrackNumber"<4294967296)) DEFAULT NULL,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_MusicPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "PhotoPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"CameraManufacturer" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"CameraModel" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"DateTaken" DATETIME DEFAULT NULL,
	"Event" TEXT DEFAULT NULL COLLATE NOCASE,
	"EXIFVersion" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"Orientation" INT CHECK("Orientation" IS NULL OR ("Orientation">=0 AND "Orientation"<65536)) DEFAULT NULL,
	"OrientationText" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"PeopleNames" TEXT DEFAULT NULL COLLATE NOCASE,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_PhotoPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "RecordedTVPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"ChannelNumber" BIGINT CHECK("ChannelNumber" IS NULL OR ("ChannelNumber">=0 AND "ChannelNumber"<4294967296)) DEFAULT NULL,
	"EpisodeName" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
	"IsDTVContent" BIT DEFAULT NULL,
	"IsHDContent" BIT DEFAULT NULL,
	"NetworkAffiliation" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"OriginalBroadcastDate" DATETIME DEFAULT NULL,
	"ProgramDescription" TEXT DEFAULT NULL,
	"StationCallSign" NVARCHAR(32) DEFAULT NULL COLLATE NOCASE,
	"StationName" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_RecordedTVPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "VideoPropertySets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Compression" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"Director" TEXT DEFAULT NULL COLLATE NOCASE,
	"EncodingBitrate" BIGINT CHECK("EncodingBitrate" IS NULL OR ("EncodingBitrate">=0 AND "EncodingBitrate"<4294967296)) DEFAULT NULL,
	"FrameHeight" BIGINT CHECK("FrameHeight" IS NULL OR ("FrameHeight">=0 AND "FrameHeight"<4294967296)) DEFAULT NULL,
	"FrameRate" BIGINT CHECK("FrameRate" IS NULL OR ("FrameRate">=0 AND "FrameRate"<4294967296)) DEFAULT NULL,
	"FrameWidth" BIGINT CHECK("FrameWidth" IS NULL OR ("FrameWidth">=0 AND "FrameWidth"<4294967296)) DEFAULT NULL,
	"HorizontalAspectRatio" BIGINT CHECK("HorizontalAspectRatio" IS NULL OR ("HorizontalAspectRatio">=0 AND "HorizontalAspectRatio"<4294967296)) DEFAULT NULL,
	"StreamName" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
	"StreamNumber" INT CHECK("StreamNumber" IS NULL OR ("StreamNumber">=0 AND "StreamNumber"<65536)) DEFAULT NULL,
	"VerticalAspectRatio" BIGINT CHECK("VerticalAspectRatio" IS NULL OR ("VerticalAspectRatio">=0 AND "VerticalAspectRatio"<4294967296)) DEFAULT NULL,
	"UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_VideoPropertySet" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "Files" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Name" NVARCHAR(1024) NOT NULL CHECK(length(trim("Name"))>0) COLLATE NOCASE,
    "Options" TINYINT  NOT NULL CHECK("Options">=0 AND "Options"<15) DEFAULT 0,
    "Status" TINYINT  NOT NULL CHECK("Status">=0 AND "Status"<10) DEFAULT 0,
    "LastAccessed" DATETIME  NOT NULL,
    "LastHashCalculation" DATETIME DEFAULT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
	"CreationTime"	DATETIME NOT NULL,
	"LastWriteTime"	DATETIME NOT NULL,
    "SummaryPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileSummaryPropertySet" REFERENCES "SummaryPropertySets"("Id") ON DELETE RESTRICT,
    "DocumentPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileDocumentPropertySet" REFERENCES "DocumentPropertySets"("Id") ON DELETE RESTRICT,
    "AudioPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileAudioPropertySet" REFERENCES "AudioPropertySets"("Id") ON DELETE RESTRICT,
    "DRMPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileDRMPropertySet" REFERENCES "DRMPropertySets"("Id") ON DELETE RESTRICT,
    "GPSPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileGPSPropertySet" REFERENCES "GPSPropertySets"("Id") ON DELETE RESTRICT,
    "ImagePropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileImagePropertySet" REFERENCES "ImagePropertySets"("Id") ON DELETE RESTRICT,
    "MediaPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileMediaPropertySet" REFERENCES "MediaPropertySets"("Id") ON DELETE RESTRICT,
    "MusicPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileMusicPropertySet" REFERENCES "MusicPropertySets"("Id") ON DELETE RESTRICT,
    "PhotoPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FilePhotoPropertySet" REFERENCES "PhotoPropertySets"("Id") ON DELETE RESTRICT,
    "RecordedTVPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileRecordedTVPropertySet" REFERENCES "RecordedTVPropertySets"("Id") ON DELETE RESTRICT,
    "VideoPropertySetId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileVideoPropertySet" REFERENCES "VideoPropertySets"("Id") ON DELETE RESTRICT,
	"BinaryPropertySetId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FileBinaryPropertySet" REFERENCES "BinaryPropertySets"("Id") ON DELETE RESTRICT,
	"ParentId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FileSubdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_Files" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND LastAccessed >= CreatedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE INDEX "IDX_File_Status" ON "Files" ("Status");

CREATE TABLE IF NOT EXISTS "FileAccessErrors" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message")) = length("Message") AND length("Message")>0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
    "ErrorCode" INT NOT NULL,
	"TargetId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessErrorFile" REFERENCES "Files"("Id") ON DELETE RESTRICT,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_FileccessErrors" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn)
);

CREATE TABLE IF NOT EXISTS "RedundantSets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Reference" NVARCHAR(128) NOT NULL DEFAULT '' COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
	"BinaryPropertySetId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundantSetBinaryPropertySet" REFERENCES "BinaryPropertySets"("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_RedundantSets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "Redundancies" (
	"FileId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundancyFile" REFERENCES "Files"("Id") ON DELETE RESTRICT,
	"RedundantSetId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundancyRedundantSet" REFERENCES "RedundantSets"("Id") ON DELETE RESTRICT,
    "Reference" NVARCHAR(128) NOT NULL DEFAULT '' COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_Redundancies" PRIMARY KEY("FileId","RedundantSetId"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "Comparisons" (
    "BaselineId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_ComparisonBaseline" REFERENCES "Files"("Id") ON DELETE RESTRICT,
    "CorrelativeId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_ComparisonCorrelative" REFERENCES "Files"("Id") ON DELETE RESTRICT,
    "AreEqual" BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_Comparisons" PRIMARY KEY("BaselineId","CorrelativeId"),
    CHECK(CreatedOn<=ModifiedOn AND BaselineId<>CorrelativeId AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TRIGGER IF NOT EXISTS validate_new_file_name
   BEFORE INSERT
   ON "Files"
   WHEN (SELECT COUNT(Id) FROM "Files" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0 OR (SELECT COUNT(Id) FROM "Subdirectories" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0
BEGIN
    SELECT RAISE (ABORT,'Duplicate file names are not allowed within the same subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_file_name_change
   BEFORE UPDATE
   ON "Files"
   WHEN OLD."Name"<>NEW."Name" AND ((SELECT COUNT(Id) FROM "Files" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0 OR (SELECT COUNT(Id) FROM "Subdirectories" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0)
BEGIN
    SELECT RAISE (ABORT,'Duplicate file names are not allowed within the same subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_new_subdirectory_name
   BEFORE INSERT
   ON "Subdirectories"
   WHEN NEW."ParentId" IS NOT NULL AND (SELECT COUNT(Id) FROM "Subdirectories" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0
BEGIN
    SELECT RAISE (ABORT,'Duplicate file names are not allowed within the same subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_subdirectory_name_change
   BEFORE UPDATE
   ON "Subdirectories"
   WHEN NEW."ParentId" IS NOT NULL AND OLD."Name"<>NEW."Name" AND (SELECT COUNT(Id) FROM "Files" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0
BEGIN
    SELECT RAISE (ABORT,'Duplicate file names are not allowed within the same subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_new_subdirectory_parent
   BEFORE INSERT
   ON "Subdirectories"
   WHEN (NEW."ParentId" IS NULL) = (NEW."VolumeId" IS NULL)
BEGIN
    SELECT RAISE (ABORT,'ParentId and VolumeId are exclusive and cannot simultaneously be null or have a value.');
END;

CREATE TRIGGER IF NOT EXISTS validate_subdirectory_parent_change
   BEFORE UPDATE
   ON "Subdirectories"
   WHEN (OLD."ParentId"<>NEW."ParentId" OR OLD."VolumeId"<>NEW."VolumeId") AND ("ParentId" IS NULL) = ("VolumeId" IS NULL)
BEGIN
    SELECT RAISE (ABORT,'ParentId and VolumeId are exclusive and cannot simultaneously be null or have a value.');
END;

CREATE TRIGGER IF NOT EXISTS validate_new_root_subdirectory
   BEFORE INSERT
   ON "Subdirectories"
   WHEN NEW."VolumeId" IS NOT NULL AND (SELECT COUNT(Id) FROM "Subdirectories" WHERE "Id"=NEW."VolumeId")>0
BEGIN
    SELECT RAISE (ABORT,'Volume already as a root subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_root_subdirectory_change
   BEFORE UPDATE
   ON "Subdirectories"
   WHEN OLD."VolumeId"<>NEW."VolumeId" AND (SELECT COUNT(Id) FROM "Subdirectories" WHERE "Id"=NEW."VolumeId")>0
BEGIN
    SELECT RAISE (ABORT,'Volume already as a root subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_new_redundancy 
   BEFORE INSERT
   ON Redundancies
   WHEN (SELECT COUNT(f.Id) FROM Files f LEFT JOIN RedundantSets r ON f.BinaryPropertySetId=r.BinaryPropertySetId WHERE f.Id=NEW.FileId AND r.Id=NEW.RedundantSetId)=0
BEGIN
    SELECT RAISE (ABORT,'File does not have the same binary property set as the redundancy set.');
END;

CREATE TRIGGER IF NOT EXISTS validate_redundancy_change
   BEFORE UPDATE
   ON Redundancies
   WHEN (NEW.FileId<>OLD.FileID OR NEW.RedundantSetId<>OLD.RedundantSetId) AND (SELECT COUNT(f.Id) FROM Files f LEFT JOIN RedundantSets r ON f.BinaryPropertySetId=r.BinaryPropertySetId WHERE f.Id=NEW.FileId AND r.Id=NEW.RedundantSetId)=0
BEGIN
    SELECT RAISE (ABORT,'File does not have the same binary property set as the redundancy set.');
END;

CREATE TRIGGER IF NOT EXISTS validate_new_crawl_config
   BEFORE INSERT
   ON Subdirectories
   WHEN NEW.CrawlConfigurationId IS NOT NULL AND (SELECT COUNT(Id) FROM Subdirectories WHERE CrawlConfigurationId=NEW.CrawlConfigurationId)<>0
BEGIN
    SELECT RAISE (ABORT,'Crawl configuration can only be applied to one subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_crawl_config_change
   BEFORE UPDATE
   ON Subdirectories
   WHEN NEW.CrawlConfigurationId IS NOT NULL AND NEW.CrawlConfigurationId<>OLD.CrawlConfigurationId AND (SELECT COUNT(Id) FROM Subdirectories WHERE Id<>NEW.Id AND CrawlConfigurationId=NEW.CrawlConfigurationId)<>0
BEGIN
    SELECT RAISE (ABORT,'Crawl configuration can only be applied to one subdirectory.');
END;

INSERT INTO "FileSystems" ("Id", "DisplayName", "DefaultDriveType", "CreatedOn", "ModifiedOn")
	VALUES ('bedb396b-2212-4149-9cad-7e437c47314c', 'New Technology File System', 3, '2004-08-19 14:51:06', '2004-08-19 14:51:06');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "Priority", "CreatedOn", "ModifiedOn")
	VALUES ('74381ccb-d56d-444d-890f-3a8051bc18e6', 'NTFS', 'bedb396b-2212-4149-9cad-7e437c47314c', 0, '2021-05-21 21:29:59', '2021-05-21 21:29:59');
INSERT INTO "FileSystems" ("Id", "DisplayName", "CaseSensitiveSearch", "CreatedOn", "ModifiedOn")
	VALUES ('02070ea8-a2ba-4240-9596-bb6d355dd366', 'Ext4 Journaling Filesystem', 1, '2021-05-21 21:12:21', '2021-05-21 21:12:21');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "Priority", "CreatedOn", "ModifiedOn")
	VALUES ('e41dfef2-d6f1-4e8a-b81f-971a85b9be9b', 'ext4', '02070ea8-a2ba-4240-9596-bb6d355dd366', 0, '2021-05-21 21:30:01', '2021-05-21 21:30:01');
INSERT INTO "FileSystems" ("Id", "DisplayName", "CreatedOn", "ModifiedOn")
	VALUES ('53a9e9a4-f5f0-4b4c-9f1e-4e3a80a93cfd', 'Virtual File Allocation Table', '2021-05-21 21:15:54', '2021-05-21 21:15:54');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "Priority", "CreatedOn", "ModifiedOn")
	VALUES ('bbb162e7-9477-49e3-acce-aee45d58bc34', 'vfat', '53a9e9a4-f5f0-4b4c-9f1e-4e3a80a93cfd', 0, '2021-05-21 21:30:09', '2021-05-21 21:30:09');
INSERT INTO "FileSystems" ("Id", "DisplayName", "CreatedOn", "ModifiedOn")
	VALUES ('17f0c19f-5f9e-4699-bf4c-cafd1de5ec54', 'File Allocation Table', '2021-05-21 21:15:54', '2021-05-21 21:15:54');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "Priority", "CreatedOn", "ModifiedOn")
	VALUES ('cd6e27c8-7b28-4731-a3f0-358c75752702', 'FAT32', '17f0c19f-5f9e-4699-bf4c-cafd1de5ec54', 0, '2021-05-21 21:30:09', '2021-05-21 21:30:09');
INSERT INTO "FileSystems" ("Id", "DisplayName", "CreatedOn", "ModifiedOn")
	VALUES ('bd64e811-2c25-4385-8b99-1494bbb24612', 'Common Internet Filesystem', '2021-05-21 21:25:23', '2021-05-21 21:25:23');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "Priority", "CreatedOn", "ModifiedOn")
	VALUES ('0f54c5a9-5e48-48a4-8056-b01f68d682a6', 'cifs', 'bd64e811-2c25-4385-8b99-1494bbb24612', 0, '2021-05-21 21:36:19', '2021-05-21 21:36:19');
INSERT INTO "FileSystems" ("Id", "DisplayName", "ReadOnly", "CreatedOn", "ModifiedOn")
	VALUES ('88a3cdb9-ed66-4778-a33b-437675a5ae38', 'ISO 9660 optical disc media', 1, '2021-05-21 21:27:27', '2021-05-21 21:27:27');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "Priority", "CreatedOn", "ModifiedOn")
	VALUES ('0989eb7a-d9db-4cef-9ac9-981fe11876b0', 'CDFS', '88a3cdb9-ed66-4778-a33b-437675a5ae38', 0, '2021-05-21 21:36:23', '2021-05-21 21:36:23');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "Priority", "CreatedOn", "ModifiedOn")
	VALUES ('556bab7a-0605-4862-a757-26943d175471', 'iso9660', '88a3cdb9-ed66-4778-a33b-437675a5ae38', 1, '2021-05-21 21:36:24', '2021-05-21 21:36:24');
INSERT INTO "FileSystems" ("Id", "DisplayName", "CreatedOn", "ModifiedOn")
	VALUES ('0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd', 'Multi-volume Archive File System', '2021-05-21 21:27:27', '2021-05-21 21:27:27');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "Priority", "CreatedOn", "ModifiedOn")
	VALUES ('e9717552-4286-4eeb-bea5-6a5267a2f223', 'MAFS', '0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd', 0, '2021-05-21 21:36:25', '2021-05-21 21:36:25');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId", "MaxNameLength", "Type", "Status", "CreatedOn", "ModifiedOn")
	VALUES ('7920cf04-9e7f-4414-986e-d1bfba011db7', 'C:', 'OS', 'urn:volume:id:9E49-7DE8', 'bedb396b-2212-4149-9cad-7e437c47314c', 255, 3, 3, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId", "MaxNameLength", "Type", "Status", "CreatedOn", "ModifiedOn")
	VALUES ('cdd51583-08a0-4dda-8fa8-ad62b1b2df2c', 'D:', 'HP_TOOLS', 'urn:volume:id:3B51-8D4B', '17f0c19f-5f9e-4699-bf4c-cafd1de5ec54', 255, 2, 3, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId", "MaxNameLength", "Type", "Status", "CreatedOn", "ModifiedOn")
	VALUES ('47af1d42-49b2-477f-b7d1-d764922e2830', 'E:', 'My Disc', 'urn:volume:id:FD91-BC0C', '88a3cdb9-ed66-4778-a33b-437675a5ae38', 110, 5, 3, '2021-06-05 01:07:19', '2021-06-05 01:07:19');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId", "MaxNameLength", "Type", "Status", "CreatedOn", "ModifiedOn")
	VALUES ('355b32f0-d9c8-4a81-b894-24109fbbda64', 'E:', 'Audio CD', 'urn:volume:id:032B-0EBE', '88a3cdb9-ed66-4778-a33b-437675a5ae38', 221, 5, 3, '2021-06-05 01:09:46', '2021-06-05 01:09:46');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId", "MaxNameLength", "Type", "Status", "CreatedOn", "ModifiedOn")
	VALUES ('c48c1c92-154c-43cf-a277-53223d5c1510', 'testazureshare (on servicenowdiag479.file.core.windows.net)', '', 'file://servicenowdiag479.file.core.windows.net/testazureshare', '0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd', 255, 4, 3, '2021-06-05 00:58:35', '2021-06-05 00:58:35');
