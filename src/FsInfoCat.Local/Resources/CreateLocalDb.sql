-- Deleting tables

DROP TABLE IF EXISTS "Comparisons";
DROP TABLE IF EXISTS "SharedVolumeTags";
DROP TABLE IF EXISTS "SharedSubdirectoryTags";
DROP TABLE IF EXISTS "SharedFileTags";
DROP TABLE IF EXISTS "SharedTagDefinitions";
DROP TABLE IF EXISTS "PersonalVolumeTags";
DROP TABLE IF EXISTS "PersonalSubdirectoryTags";
DROP TABLE IF EXISTS "PersonalFileTags";
DROP TABLE IF EXISTS "PersonalTagDefinitions";
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
PRAGMA foreign_keys=off;
DROP TABLE IF EXISTS "Subdirectories";
PRAGMA foreign_keys=on;
DROP TABLE IF EXISTS "VolumeAccessErrors";
DROP TABLE IF EXISTS "Volumes";
DROP TABLE IF EXISTS "SymbolicNames";
DROP TABLE IF EXISTS "FileSystems";

-- Creating tables

CREATE TABLE IF NOT EXISTS "FileSystems" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim("DisplayName"))=length("DisplayName") AND length("DisplayName")>0) COLLATE NOCASE,
    "ReadOnly" BIT NOT NULL DEFAULT 0,
    "MaxNameLength" UNSIGNED INT NOT NULL DEFAULT 255,
    "DefaultDriveType" UNSIGNED TINYINT CHECK("DefaultDriveType" IS NULL OR ("DefaultDriveType">=0 AND "DefaultDriveType"<7)) DEFAULT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_FileSystems" PRIMARY KEY("Id"),
    CONSTRAINT "UK_FileSystem_DislayName" UNIQUE("DisplayName"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_FileSystem_DisplayName" ON "FileSystems" ("DisplayName" COLLATE NOCASE);

CREATE INDEX "IDX_FileSystem_IsInactive" ON "FileSystems" ("IsInactive");

CREATE TABLE IF NOT EXISTS "SymbolicNames" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Name" NVARCHAR(256) NOT NULL CHECK(length(trim("Name"))=length("Name") AND length("Name")>0) COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Priority" INT NOT NULL DEFAULT 0,
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "FileSystemId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SymbolicName_FileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_SymbolicNames" PRIMARY KEY("Id"),
    CONSTRAINT "UK_FileSystem_Name" UNIQUE("Name"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_SymbolicName_Name" ON "SymbolicNames" ("Name" COLLATE NOCASE);

CREATE INDEX "IDX_SymbolicName_IsInactive" ON "SymbolicNames" ("IsInactive");

CREATE TABLE IF NOT EXISTS "Volumes" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim("DisplayName"))=length("DisplayName") AND length("DisplayName")>0) COLLATE NOCASE,
    "VolumeName" NVARCHAR(128) NOT NULL CHECK(length(trim("VolumeName"))=length("VolumeName")) DEFAULT '' COLLATE NOCASE,
    "Identifier" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "ReadOnly" BIT DEFAULT NULL,
    "MaxNameLength" UNSIGNED INT DEFAULT NULL,
    "Type" UNSIGNED TINYINT NOT NULL CHECK("Type">=0 AND "Type"<7) DEFAULT 0, -- Unknown
    "Status" UNSIGNED TINYINT NOT NULL CHECK("Status"<=6) DEFAULT 0, -- VolumeStatus.Unknown
    "Notes" TEXT NOT NULL DEFAULT '',
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "FileSystemId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Volume_FileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_Volumes" PRIMARY KEY("Id"),
    CONSTRAINT "UK_FileSystem_Volume" UNIQUE("Identifier"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_Volume_VolumeName" ON "Volumes" ("VolumeName" COLLATE NOCASE);

CREATE INDEX "IDX_Volume_Identifier" ON "Volumes" ("Identifier" COLLATE NOCASE);

CREATE INDEX "IDX_Volume_Status" ON "Volumes" ("Status");

CREATE TABLE IF NOT EXISTS "VolumeAccessErrors" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ErrorCode" UNSIGNED TINYINT NOT NULL CHECK("ErrorCode"<=5),
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message"))=length("Message") AND length("Message")>0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "TargetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessError_Volume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_VolumeAccessErrors" PRIMARY KEY("Id"),
    CHECK("CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "Subdirectories" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Name" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "LastAccessed" DATETIME NOT NULL,
    "CreationTime" DATETIME NOT NULL,
    "LastWriteTime" DATETIME NOT NULL,
    "Options" UNSIGNED TINYINT NOT NULL CHECK("Options"<=32) DEFAULT 0, -- DirectoryCrawlOptions.None
    "Status" UNSIGNED TINYINT NOT NULL CHECK("Status"<=3) DEFAULT 0, -- DirectoryStatus.Incomplete
    "Notes" TEXT NOT NULL DEFAULT '',
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "ParentId" UNIQUEIDENTIFIER CONSTRAINT "FK_Subdirectory_Parent" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "VolumeId" UNIQUEIDENTIFIER CONSTRAINT "FK_Subdirectory_Volume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_Subdirectories" PRIMARY KEY("Id"),
    CHECK((("ParentId" IS NULL AND "VolumeId" IS NOT NULL) OR ("ParentId" IS NOT NULL AND "VolumeId" IS NULL)) AND (("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_Subdirectory_Status" ON "Subdirectories" ("Status");

CREATE TABLE IF NOT EXISTS "CrawlConfigurations" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim("DisplayName"))=length("DisplayName") AND length("DisplayName")>0) COLLATE NOCASE,
    "MaxRecursionDepth" UNSIGNED SMALLINT NOT NULL DEFAULT 128,
    "MaxTotalItems" UNSIGNED BIGINT CHECK("MaxTotalItems" IS NULL OR "MaxTotalItems">0) DEFAULT NULL,
    "TTL" BIGINT CHECK("TTL" IS NULL OR "TTL">299) DEFAULT NULL,
    "StatusValue" UNSIGNED TINYINT NOT NULL CHECK("StatusValue"<8) DEFAULT 0, -- CrawlStatus.NotRunning
    "LastCrawlStart" DATETIME DEFAULT NULL,
    "LastCrawlEnd" DATETIME DEFAULT NULL,
    "NextScheduledStart" DATETIME DEFAULT NULL,
    "RescheduleInterval" BIGINT CHECK("RescheduleInterval" IS NULL OR "RescheduleInterval">899) DEFAULT NULL,
    "RescheduleFromJobEnd" BIT NOT NULL DEFAULT 0,
    "RescheduleAfterFail" BIT NOT NULL DEFAULT 0,
    "Notes" TEXT NOT NULL DEFAULT '',
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "RootId" UNIQUEIDENTIFIER CONSTRAINT "FK_CrawlConfiguration_Root" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_CrawlConfigurations" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_CrawlConfiguration_StatusValue" ON "CrawlConfigurations" ("StatusValue");

CREATE TABLE IF NOT EXISTS "CrawlJobLogs" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "MaxRecursionDepth" UNSIGNED SMALLINT NOT NULL DEFAULT 128,
    "MaxTotalItems" UNSIGNED BIGINT DEFAULT NULL,
    "TTL" BIGINT DEFAULT NULL,
    "RootPath" NVARCHAR(4096) NOT NULL COLLATE NOCASE,
    "StatusCode" UNSIGNED TINYINT NOT NULL CHECK("StatusCode"<=7) DEFAULT 0, -- CrawlStatus.NotRunning
    "CrawlStart" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "CrawlEnd" DATETIME DEFAULT NULL,
    "StatusMessage" NVARCHAR(1024) NOT NULL CHECK(length(trim("StatusMessage"))=length("StatusMessage")) COLLATE NOCASE,
    "StatusDetail" TEXT NOT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "ConfigurationId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_CrawlJobLog_CrawlConfiguration" REFERENCES "CrawlConfigurations"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_CrawlJobLogs" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_CrawlJobLog_StatusCode" ON "CrawlJobLogs" ("StatusCode");

CREATE TABLE IF NOT EXISTS "SubdirectoryAccessErrors" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ErrorCode" UNSIGNED TINYINT NOT NULL CHECK("ErrorCode"<=5),
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message"))=length("Message") AND length("Message")>0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "TargetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessError_Subdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_SubdirectoryAccessErrors" PRIMARY KEY("Id"),
    CHECK("CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "BinaryPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Length" BIGINT NOT NULL,
    "Hash" BINARY(16) DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_BinaryPropertySets" PRIMARY KEY("Id"),
    CONSTRAINT "UK_BinaryPropertySet_Length_Hash" UNIQUE("Length", "Hash"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_BinaryPropertySet_Length" ON "BinaryPropertySets" ("Length");

CREATE INDEX "IDX_BinaryPropertySet_Hash" ON "BinaryPropertySets" ("Hash");

CREATE TABLE IF NOT EXISTS "SummaryPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ApplicationName" NVARCHAR(1024) CHECK("ApplicationName" IS NULL OR length(trim("ApplicationName"))=length("ApplicationName")) DEFAULT NULL COLLATE NOCASE,
    "Author" TEXT DEFAULT NULL,
    "Comment" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
    "Keywords" TEXT DEFAULT NULL,
    "Subject" NVARCHAR(1024) CHECK("Subject" IS NULL OR length(trim("Subject"))=length("Subject")) DEFAULT NULL COLLATE NOCASE,
    "Title" NVARCHAR(1024) CHECK("Title" IS NULL OR length(trim("Title"))=length("Title")) DEFAULT NULL COLLATE NOCASE,
    "Company" NVARCHAR(1024) CHECK("Company" IS NULL OR length(trim("Company"))=length("Company")) DEFAULT NULL COLLATE NOCASE,
    "ContentType" NVARCHAR(1024) CHECK("ContentType" IS NULL OR length(trim("ContentType"))=length("ContentType")) DEFAULT NULL COLLATE NOCASE,
    "Copyright" NVARCHAR(1024) CHECK("Copyright" IS NULL OR length(trim("Copyright"))=length("Copyright")) DEFAULT NULL COLLATE NOCASE,
    "ParentalRating" NVARCHAR(32) CHECK("ParentalRating" IS NULL OR length(trim("ParentalRating"))=length("ParentalRating")) DEFAULT NULL COLLATE NOCASE,
    "Rating" UNSIGNED INT DEFAULT NULL,
    "ItemAuthors" TEXT DEFAULT NULL,
    "ItemType" NVARCHAR(32) CHECK("ItemType" IS NULL OR length(trim("ItemType"))=length("ItemType")) DEFAULT NULL COLLATE NOCASE,
    "ItemTypeText" NVARCHAR(64) CHECK("ItemTypeText" IS NULL OR length(trim("ItemTypeText"))=length("ItemTypeText")) DEFAULT NULL COLLATE NOCASE,
    "Kind" TEXT DEFAULT NULL,
    "MIMEType" NVARCHAR(1024) CHECK("MIMEType" IS NULL OR length(trim("MIMEType"))=length("MIMEType")) DEFAULT NULL COLLATE NOCASE,
    "ParentalRatingReason" NVARCHAR(1024) DEFAULT NULL COLLATE NOCASE,
    "ParentalRatingsOrganization" NVARCHAR(1024) CHECK("ParentalRatingsOrganization" IS NULL OR length(trim("ParentalRatingsOrganization"))=length("ParentalRatingsOrganization")) DEFAULT NULL COLLATE NOCASE,
    "Sensitivity" UNSIGNED SMALLINT DEFAULT NULL,
    "SensitivityText" NVARCHAR(1024) CHECK("SensitivityText" IS NULL OR length(trim("SensitivityText"))=length("SensitivityText")) DEFAULT NULL COLLATE NOCASE,
    "SimpleRating" UNSIGNED INT DEFAULT NULL,
    "Trademarks" NVARCHAR(1024) CHECK("Trademarks" IS NULL OR length(trim("Trademarks"))=length("Trademarks")) DEFAULT NULL COLLATE NOCASE,
    "ProductName" NVARCHAR(256) CHECK("ProductName" IS NULL OR length(trim("ProductName"))=length("ProductName")) DEFAULT NULL COLLATE NOCASE,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SummaryPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "DocumentPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ClientID" NVARCHAR(64) CHECK("ClientID" IS NULL OR length(trim("ClientID"))=length("ClientID")) DEFAULT NULL COLLATE NOCASE,
    "Contributor" TEXT DEFAULT NULL,
    "DateCreated" DATETIME DEFAULT NULL,
    "LastAuthor" NVARCHAR(1024) CHECK("LastAuthor" IS NULL OR length(trim("LastAuthor"))=length("LastAuthor")) DEFAULT NULL COLLATE NOCASE,
    "RevisionNumber" NVARCHAR(64) CHECK("RevisionNumber" IS NULL OR length(trim("RevisionNumber"))=length("RevisionNumber")) DEFAULT NULL COLLATE NOCASE,
    "Security" INT DEFAULT NULL,
    "Division" NVARCHAR(256) CHECK("Division" IS NULL OR length(trim("Division"))=length("Division")) DEFAULT NULL COLLATE NOCASE,
    "DocumentID" NVARCHAR(64) CHECK("DocumentID" IS NULL OR length(trim("DocumentID"))=length("DocumentID")) DEFAULT NULL COLLATE NOCASE,
    "Manager" NVARCHAR(256) CHECK("Manager" IS NULL OR length(trim("Manager"))=length("Manager")) DEFAULT NULL COLLATE NOCASE,
    "PresentationFormat" NVARCHAR(256) CHECK("PresentationFormat" IS NULL OR length(trim("PresentationFormat"))=length("PresentationFormat")) DEFAULT NULL COLLATE NOCASE,
    "Version" NVARCHAR(64) CHECK("Version" IS NULL OR length(trim("Version"))=length("Version")) DEFAULT NULL COLLATE NOCASE,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_DocumentPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "AudioPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Compression" NVARCHAR(256) CHECK("Compression" IS NULL OR length(trim("Compression"))=length("Compression")) DEFAULT NULL COLLATE NOCASE,
    "EncodingBitrate" UNSIGNED INT DEFAULT NULL,
    "Format" NVARCHAR(256) CHECK("Format" IS NULL OR length(trim("Format"))=length("Format")) DEFAULT NULL COLLATE NOCASE,
    "IsVariableBitrate" BIT DEFAULT NULL,
    "SampleRate" UNSIGNED INT DEFAULT NULL,
    "SampleSize" UNSIGNED INT DEFAULT NULL,
    "StreamName" NVARCHAR(256) DEFAULT NULL COLLATE NOCASE,
    "StreamNumber" UNSIGNED SMALLINT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_AudioPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "DRMPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "DatePlayExpires" DATETIME DEFAULT NULL,
    "DatePlayStarts" DATETIME DEFAULT NULL,
    "Description" TEXT DEFAULT NULL,
    "IsProtected" BIT DEFAULT NULL,
    "PlayCount" UNSIGNED INT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_DRMPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "GPSPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "AreaInformation" NVARCHAR(1024) CHECK("AreaInformation" IS NULL OR length(trim("AreaInformation"))=length("AreaInformation")) DEFAULT NULL COLLATE NOCASE,
    "LatitudeDegrees" REAL DEFAULT NULL,
    "LatitudeMinutes" REAL DEFAULT NULL,
    "LatitudeSeconds" REAL DEFAULT NULL,
    "LatitudeRef" NVARCHAR(256) CHECK("LatitudeRef" IS NULL OR length(trim("LatitudeRef"))=length("LatitudeRef")) DEFAULT NULL COLLATE NOCASE,
    "LongitudeDegrees" REAL DEFAULT NULL,
    "LongitudeMinutes" REAL DEFAULT NULL,
    "LongitudeSeconds" REAL DEFAULT NULL,
    "LongitudeRef" NVARCHAR(256) CHECK("LongitudeRef" IS NULL OR length(trim("LongitudeRef"))=length("LongitudeRef")) DEFAULT NULL COLLATE NOCASE,
    "MeasureMode" NVARCHAR(256) CHECK("MeasureMode" IS NULL OR length(trim("MeasureMode"))=length("MeasureMode")) DEFAULT NULL COLLATE NOCASE,
    "ProcessingMethod" NVARCHAR(256) CHECK("ProcessingMethod" IS NULL OR length(trim("ProcessingMethod"))=length("ProcessingMethod")) DEFAULT NULL COLLATE NOCASE,
    "VersionID" VARBINARY(128) CHECK("VersionID" IS NULL OR length("VersionID")<129) DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_GPSPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "ImagePropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "BitDepth" UNSIGNED INT DEFAULT NULL,
    "ColorSpace" UNSIGNED SMALLINT DEFAULT NULL,
    "CompressedBitsPerPixel" REAL DEFAULT NULL,
    "Compression" UNSIGNED SMALLINT DEFAULT NULL,
    "CompressionText" NVARCHAR(256) CHECK("CompressionText" IS NULL OR length(trim("CompressionText"))=length("CompressionText")) DEFAULT NULL COLLATE NOCASE,
    "HorizontalResolution" REAL DEFAULT NULL,
    "HorizontalSize" UNSIGNED INT DEFAULT NULL,
    "ImageID" NVARCHAR(256) CHECK("ImageID" IS NULL OR length(trim("ImageID"))=length("ImageID")) DEFAULT NULL COLLATE NOCASE,
    "ResolutionUnit" SMALLINT DEFAULT NULL,
    "VerticalResolution" REAL DEFAULT NULL,
    "VerticalSize" UNSIGNED INT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_ImagePropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "MediaPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ContentDistributor" NVARCHAR(256) CHECK("ContentDistributor" IS NULL OR length(trim("ContentDistributor"))=length("ContentDistributor")) DEFAULT NULL COLLATE NOCASE,
    "CreatorApplication" NVARCHAR(256) CHECK("CreatorApplication" IS NULL OR length(trim("CreatorApplication"))=length("CreatorApplication")) DEFAULT NULL COLLATE NOCASE,
    "CreatorApplicationVersion" NVARCHAR(64) CHECK("CreatorApplicationVersion" IS NULL OR length(trim("CreatorApplicationVersion"))=length("CreatorApplicationVersion")) DEFAULT NULL COLLATE NOCASE,
    "DateReleased" NVARCHAR(64) CHECK("DateReleased" IS NULL OR length(trim("DateReleased"))=length("DateReleased")) DEFAULT NULL COLLATE NOCASE,
    "Duration" UNSIGNED BIGINT DEFAULT NULL,
    "DVDID" NVARCHAR(64) CHECK("DVDID" IS NULL OR length(trim("DVDID"))=length("DVDID")) DEFAULT NULL COLLATE NOCASE,
    "FrameCount" UNSIGNED INT DEFAULT NULL,
    "Producer" TEXT DEFAULT NULL,
    "ProtectionType" NVARCHAR(256) CHECK("ProtectionType" IS NULL OR length(trim("ProtectionType"))=length("ProtectionType")) DEFAULT NULL COLLATE NOCASE,
    "ProviderRating" NVARCHAR(256) CHECK("ProviderRating" IS NULL OR length(trim("ProviderRating"))=length("ProviderRating")) DEFAULT NULL COLLATE NOCASE,
    "ProviderStyle" NVARCHAR(256) CHECK("ProviderStyle" IS NULL OR length(trim("ProviderStyle"))=length("ProviderStyle")) DEFAULT NULL COLLATE NOCASE,
    "Publisher" NVARCHAR(256) CHECK("Publisher" IS NULL OR length(trim("Publisher"))=length("Publisher")) DEFAULT NULL COLLATE NOCASE,
    "Subtitle" NVARCHAR(256) CHECK("Subtitle" IS NULL OR length(trim("Subtitle"))=length("Subtitle")) DEFAULT NULL COLLATE NOCASE,
    "Writer" TEXT DEFAULT NULL,
    "Year" UNSIGNED INT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_MediaPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "MusicPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "AlbumArtist" NVARCHAR(1024) CHECK("AlbumArtist" IS NULL OR length(trim("AlbumArtist"))=length("AlbumArtist")) DEFAULT NULL COLLATE NOCASE,
    "AlbumTitle" NVARCHAR(1024) CHECK("AlbumTitle" IS NULL OR length(trim("AlbumTitle"))=length("AlbumTitle")) DEFAULT NULL COLLATE NOCASE,
    "Artist" TEXT DEFAULT NULL,
    "ChannelCount" UNSIGNED INT DEFAULT NULL,
    "Composer" TEXT DEFAULT NULL,
    "Conductor" TEXT DEFAULT NULL,
    "DisplayArtist" TEXT DEFAULT NULL,
    "Genre" TEXT DEFAULT NULL,
    "PartOfSet" NVARCHAR(64) CHECK("PartOfSet" IS NULL OR length(trim("PartOfSet"))=length("PartOfSet")) DEFAULT NULL COLLATE NOCASE,
    "Period" NVARCHAR(64) CHECK("Period" IS NULL OR length(trim("Period"))=length("Period")) DEFAULT NULL COLLATE NOCASE,
    "TrackNumber" UNSIGNED INT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_MusicPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "PhotoPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "CameraManufacturer" NVARCHAR(256) CHECK("CameraManufacturer" IS NULL OR length(trim("CameraManufacturer"))=length("CameraManufacturer")) DEFAULT NULL COLLATE NOCASE,
    "CameraModel" NVARCHAR(256) CHECK("CameraModel" IS NULL OR length(trim("CameraModel"))=length("CameraModel")) DEFAULT NULL COLLATE NOCASE,
    "DateTaken" DATETIME DEFAULT NULL,
    "Event" TEXT DEFAULT NULL,
    "EXIFVersion" NVARCHAR(256) CHECK("EXIFVersion" IS NULL OR length(trim("EXIFVersion"))=length("EXIFVersion")) DEFAULT NULL COLLATE NOCASE,
    "Orientation" UNSIGNED SMALLINT DEFAULT NULL,
    "OrientationText" NVARCHAR(256) CHECK("OrientationText" IS NULL OR length(trim("OrientationText"))=length("OrientationText")) DEFAULT NULL COLLATE NOCASE,
    "PeopleNames" TEXT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_PhotoPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "RecordedTVPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ChannelNumber" UNSIGNED INT DEFAULT NULL,
    "EpisodeName" NVARCHAR(1024) CHECK("EpisodeName" IS NULL OR length(trim("EpisodeName"))=length("EpisodeName")) DEFAULT NULL COLLATE NOCASE,
    "IsDTVContent" BIT DEFAULT NULL,
    "IsHDContent" BIT DEFAULT NULL,
    "NetworkAffiliation" NVARCHAR(256) CHECK("NetworkAffiliation" IS NULL OR length(trim("NetworkAffiliation"))=length("NetworkAffiliation")) DEFAULT NULL COLLATE NOCASE,
    "OriginalBroadcastDate" DATETIME DEFAULT NULL,
    "ProgramDescription" TEXT DEFAULT NULL,
    "StationCallSign" NVARCHAR(32) CHECK("StationCallSign" IS NULL OR length(trim("StationCallSign"))=length("StationCallSign")) DEFAULT NULL COLLATE NOCASE,
    "StationName" NVARCHAR(256) CHECK("StationName" IS NULL OR length(trim("StationName"))=length("StationName")) DEFAULT NULL COLLATE NOCASE,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_RecordedTVPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "VideoPropertySets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Compression" NVARCHAR(256) CHECK("Compression" IS NULL OR length(trim("Compression"))=length("Compression")) DEFAULT NULL COLLATE NOCASE,
    "Director" TEXT DEFAULT NULL,
    "EncodingBitrate" UNSIGNED INT DEFAULT NULL,
    "FrameHeight" UNSIGNED INT DEFAULT NULL,
    "FrameRate" UNSIGNED INT DEFAULT NULL,
    "FrameWidth" UNSIGNED INT DEFAULT NULL,
    "HorizontalAspectRatio" UNSIGNED INT DEFAULT NULL,
    "StreamNumber" UNSIGNED SMALLINT DEFAULT NULL,
    "StreamName" NVARCHAR(256) CHECK("StreamName" IS NULL OR length(trim("StreamName"))=length("StreamName")) DEFAULT NULL COLLATE NOCASE,
    "VerticalAspectRatio" UNSIGNED INT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_VideoPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "RedundantSets" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Reference" NVARCHAR(128) NOT NULL CHECK(length(trim("Reference"))=length("Reference")) COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "BinaryPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundantSet_BinaryPropertySet" REFERENCES "BinaryPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_RedundantSets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "Files" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Name" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "LastAccessed" DATETIME NOT NULL,
    "Options" UNSIGNED TINYINT NOT NULL CHECK("Options"<=8) DEFAULT 0, -- FileCrawlOptions.None
    "Status" UNSIGNED TINYINT NOT NULL CHECK("Status"<=9) DEFAULT 0, -- FileCorrelationStatus.Dissociated
    "LastHashCalculation" DATETIME DEFAULT NULL,
    "CreationTime" DATETIME NOT NULL,
    "LastWriteTime" DATETIME NOT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "ParentId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_Parent" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "BinaryPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_BinaryPropertySet" REFERENCES "BinaryPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "SummaryPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_SummaryPropertySet" REFERENCES "SummaryPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "DocumentPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_DocumentPropertySet" REFERENCES "DocumentPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "AudioPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_AudioPropertySet" REFERENCES "AudioPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "DRMPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_DRMPropertySet" REFERENCES "DRMPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "GPSPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_GPSPropertySet" REFERENCES "GPSPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "ImagePropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_ImagePropertySet" REFERENCES "ImagePropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "MediaPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_MediaPropertySet" REFERENCES "MediaPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "MusicPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_MusicPropertySet" REFERENCES "MusicPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "PhotoPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_PhotoPropertySet" REFERENCES "PhotoPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "RecordedTVPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_RecordedTVPropertySet" REFERENCES "RecordedTVPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "VideoPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_File_VideoPropertySet" REFERENCES "VideoPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_Files" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_File_Status" ON "Files" ("Status");

CREATE TABLE IF NOT EXISTS "FileAccessErrors" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ErrorCode" UNSIGNED TINYINT NOT NULL CHECK("ErrorCode"<=5),
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message"))=length("Message") AND length("Message")>0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "TargetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessError_File" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_FileAccessErrors" PRIMARY KEY("Id"),
    CHECK("CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "PersonalTagDefinitions" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Name" NVARCHAR(256) NOT NULL CHECK(length(trim("Name"))=length("Name")) COLLATE NOCASE,
    "Description" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_PersonalTagDefinitions" PRIMARY KEY("Id"),
    CONSTRAINT "UK_PersonalTagDefinition_Name" UNIQUE("Name"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_PersonalTagDefinition_Name" ON "PersonalTagDefinitions" ("Name");

CREATE TABLE IF NOT EXISTS "PersonalFileTags" (
    "FileId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalFileTag_File" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "PersonalTagDefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalFileTag_TagDefinition" REFERENCES "PersonalTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_PersonalFileTags" PRIMARY KEY("FileId", "PersonalTagDefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "PersonalSubdirectoryTags" (
    "SubdirectoryId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalSubdirectoryTag_Subdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "PersonalTagDefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalSubdirectoryTag_TagDefinition" REFERENCES "PersonalTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_PersonalSubdirectoryTags" PRIMARY KEY("SubdirectoryId", "PersonalTagDefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "PersonalVolumeTags" (
    "VolumeId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalVolumeTag_Volume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "PersonalTagDefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalVolumeTag_TagDefinition" REFERENCES "PersonalTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_PersonalVolumeTags" PRIMARY KEY("VolumeId", "PersonalTagDefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "SharedTagDefinitions" (
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Name" NVARCHAR(256) NOT NULL CHECK(length(trim("Name"))=length("Name")) COLLATE NOCASE,
    "Description" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SharedTagDefinitions" PRIMARY KEY("Id"),
    CONSTRAINT "UK_SharedTagDefinition_Name" UNIQUE("Name"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_SharedTagDefinition_Name" ON "SharedTagDefinitions" ("Name");

CREATE TABLE IF NOT EXISTS "SharedFileTags" (
    "FileId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedFileTag_File" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "SharedTagDefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedFileTag_TagDefinition" REFERENCES "SharedTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SharedFileTags" PRIMARY KEY("FileId", "SharedTagDefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "SharedSubdirectoryTags" (
    "SubdirectoryId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedSubdirectoryTag_Subdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "SharedTagDefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedSubdirectoryTag_TagDefinition" REFERENCES "SharedTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SharedSubdirectoryTags" PRIMARY KEY("SubdirectoryId", "SharedTagDefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "SharedVolumeTags" (
    "VolumeId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedVolumeTag_Volume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "SharedTagDefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedVolumeTag_TagDefinition" REFERENCES "SharedTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SharedVolumeTags" PRIMARY KEY("VolumeId", "SharedTagDefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "Redundancies" (
    "FileId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Redundancy_File" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "RedundantSetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Redundancy_RedundantSet" REFERENCES "RedundantSets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Reference" NVARCHAR(128) NOT NULL CHECK(length(trim("Reference"))=length("Reference")) COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_Redundancies" PRIMARY KEY("FileId", "RedundantSetId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "Comparisons" (
    "BaselineId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Comparison_BaselineFile" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "CorrelativeId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Comparison_CorrelativeFile" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "AreEqual" BIT NOT NULL DEFAULT 0,
    "ComparedOn" DATETIME NOT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_Comparisons" PRIMARY KEY("BaselineId", "CorrelativeId"),
    CHECK("BaselineId"<>"CorrelativeId" AND (("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TRIGGER IF NOT EXISTS validate_new_file_name
   BEFORE INSERT
   ON "Files"
   WHEN (SELECT COUNT(Id) FROM "Files" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0 OR (SELECT COUNT(Id) FROM "Subdirectories" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0
BEGIN
    SELECT RAISE (ABORT,'Duplicate file/directory names are not allowed within the same subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_file_name_change
   BEFORE UPDATE
   ON "Files"
   WHEN OLD."Name"<>NEW."Name" AND ((SELECT COUNT(Id) FROM "Files" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0 OR (SELECT COUNT(Id) FROM "Subdirectories" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0)
BEGIN
    SELECT RAISE (ABORT,'Duplicate file/directory names are not allowed within the same subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_new_subdirectory_name
   BEFORE INSERT
   ON "Subdirectories"
   WHEN NEW."ParentId" IS NOT NULL AND (SELECT COUNT(Id) FROM "Subdirectories" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0
BEGIN
    SELECT RAISE (ABORT,'Duplicate file/directory names are not allowed within the same subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_subdirectory_name_change
   BEFORE UPDATE
   ON "Subdirectories"
   WHEN NEW."ParentId" IS NOT NULL AND OLD."Name"<>NEW."Name" AND (SELECT COUNT(Id) FROM "Files" WHERE "Name"=NEW."Name" AND "ParentId"=NEW."ParentId" COLLATE BINARY)>0
BEGIN
    SELECT RAISE (ABORT,'Duplicate file/directory names are not allowed within the same subdirectory.');
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
    SELECT RAISE (ABORT,'Volume already has a root subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_root_subdirectory_change
   BEFORE UPDATE
   ON "Subdirectories"
   WHEN OLD."VolumeId"<>NEW."VolumeId" AND (SELECT COUNT(Id) FROM "Subdirectories" WHERE "Id"=NEW."VolumeId")>0
BEGIN
    SELECT RAISE (ABORT,'Volume already has a root subdirectory.');
END;

CREATE TRIGGER IF NOT EXISTS validate_new_crawlconfiguration
   BEFORE INSERT
   ON "CrawlConfigurations"
   WHEN (SELECT COUNT(Id) FROM "CrawlConfigurations" WHERE "RootId"=NEW."RootId")>0
BEGIN
    SELECT RAISE (ABORT,'Subdirectory already has a crawl configuration.');
END;

CREATE TRIGGER IF NOT EXISTS validate_crawlconfiguration_root_change
   BEFORE UPDATE
   ON "CrawlConfigurations"
   WHEN OLD."RootId"<>NEW."RootId" AND (SELECT COUNT(Id) FROM "CrawlConfigurations" WHERE "RootId"=NEW."RootId")>0
BEGIN
    SELECT RAISE (ABORT,'Subdirectory already has a crawl configuration.');
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
INSERT INTO "FileSystems" ("Id", "DisplayName", "CreatedOn", "ModifiedOn")
	VALUES ('bedb396b-2212-4149-9cad-7e437c47314c', 'New Technology File System', '2004-08-19 14:51:06', '2004-08-19 14:51:06');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "Priority", "CreatedOn", "ModifiedOn")
	VALUES ('74381ccb-d56d-444d-890f-3a8051bc18e6', 'NTFS', 'bedb396b-2212-4149-9cad-7e437c47314c', 0, '2021-05-21 21:29:59', '2021-05-21 21:29:59');
INSERT INTO "FileSystems" ("Id", "DisplayName", "CreatedOn", "ModifiedOn")
	VALUES ('02070ea8-a2ba-4240-9596-bb6d355dd366', 'Ext4 Journaling Filesystem', '2021-05-21 21:12:21', '2021-05-21 21:12:21');
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
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('20a61d4b-80c2-48a3-8df6-522e598aae08', 'C:\', '2021-06-05 00:58:34', '2019-03-19 00:37:21', '2021-06-04 13:47:20', NULL, '7920cf04-9e7f-4414-986e-d1bfba011db7', '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('b63144ce-91cb-4cb8-a407-8d3490a8c90c', 'Users', '2021-06-05 00:58:34', '2019-12-07 04:03:44', '2021-03-09 23:51:36', '20a61d4b-80c2-48a3-8df6-522e598aae08', NULL, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('38a40fde-acf0-4cc5-9302-d37ec2cbb631', 'lerwi', '2021-06-05 00:58:34', '2021-03-09 23:51:36', '2021-06-04 03:09:25', 'b63144ce-91cb-4cb8-a407-8d3490a8c90c', NULL, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a', 'Videos', '2021-06-05 00:58:34', '2019-11-23 20:30:23', '2021-05-16 18:54:50', '38a40fde-acf0-4cc5-9302-d37ec2cbb631', NULL, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "main"."CrawlConfigurations" ("Id", "DisplayName", "RootId", "CreatedOn", "ModifiedOn")
    VALUES ('9c91ba89-6ab5-4d4e-9798-0d926b405f41', 'Lenny''s Laptop Videos', '3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a', '2021-07-31 15:28:18', '2021-07-31 15:28:18');
