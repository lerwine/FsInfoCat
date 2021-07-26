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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim("DisplayName"))=length("DisplayName") AND "DisplayName">0) COLLATE NOCASE,
    "CaseSensitiveSearch" BIT NOT NULL DEFAULT 0,
    "ReadOnly" BIT NOT NULL DEFAULT 0,
    "MaxNameLength" UNSIGNED INT NOT NULL DEFAULT 255,
    "DefaultDriveType" UNSIGNED TINYINT CHECK("DefaultDriveType" IS NULL OR ("DefaultDriveType">=0 AND "DefaultDriveType"<7)) DEFAULT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_FileSystems" PRIMARY KEY("Id"),
    CONSTRAINT "UK_FileSystem_DislayName" UNIQUE("DisplayName"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_FileSystem_DisplayName" ON "FileSystems" ("DisplayName" COLLATE NOCASE);

CREATE INDEX "IDX_FileSystem_IsInactive" ON "FileSystems" ("IsInactive");

CREATE TABLE IF NOT EXISTS "SymbolicNames" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Name" NVARCHAR(256) NOT NULL CHECK(length(trim("Name"))=length("Name") AND "Name">0) COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Priority" INT NOT NULL DEFAULT 0,
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "FileSystemId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SymbolicName_FileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SymbolicNames" PRIMARY KEY("Id"),
    CONSTRAINT "UK_FileSystem_Name" UNIQUE("Name"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_SymbolicName_Name" ON "SymbolicNames" ("Name" COLLATE NOCASE);

CREATE INDEX "IDX_SymbolicName_IsInactive" ON "SymbolicNames" ("IsInactive");

CREATE TABLE IF NOT EXISTS "Volumes" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim("DisplayName"))=length("DisplayName") AND "DisplayName">0) COLLATE NOCASE,
    "VolumeName" NVARCHAR(128) NOT NULL CHECK(length(trim("VolumeName"))=length("VolumeName")) DEFAULT '' COLLATE NOCASE,
    "Identifier" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "CaseSensitiveSearch" BIT DEFAULT NULL,
    "ReadOnly" BIT DEFAULT NULL,
    "MaxNameLength" UNSIGNED INT DEFAULT NULL,
    "Type" UNSIGNED TINYINT NOT NULL CHECK("Type">=0 AND "Type"<7) DEFAULT 0, -- Unknown
    "Notes" TEXT NOT NULL DEFAULT '',
    "Status" UNSIGNED TINYINT NOT NULL CHECK("Status"<=6) DEFAULT 0, -- VolumeStatus.Unknown
    "FileSystemId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Volume_FileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_Volumes" PRIMARY KEY("Id"),
    CONSTRAINT "UK_FileSystem_Volume" UNIQUE("Identifier"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_Volume_VolumeName" ON "Volumes" ("VolumeName" COLLATE NOCASE);

CREATE INDEX "IDX_Volume_Identifier" ON "Volumes" ("Identifier" COLLATE NOCASE);

CREATE INDEX "IDX_Volume_Status" ON "Volumes" ("Status");

CREATE TABLE IF NOT EXISTS "VolumeAccessErrors" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ErrorCode" UNSIGNED TINYINT NOT NULL CHECK("ErrorCode"<=5),
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message"))=length("Message") AND "Message">0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
    "TargetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessError_DbEntity" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_VolumeAccessErrors" PRIMARY KEY("Id"),
    CHECK("CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "Subdirectories" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Name" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "LastAccessed" DATETIME NOT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "CreationTime" DATETIME NOT NULL,
    "LastWriteTime" DATETIME NOT NULL,
    "ParentId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_DbFsItem_Subdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "Options" UNSIGNED TINYINT NOT NULL CHECK("Options"<=32) DEFAULT 0, -- DirectoryCrawlOptions.None
    "Status" UNSIGNED TINYINT NOT NULL CHECK("Status"<=3) DEFAULT 0, -- DirectoryStatus.Incomplete
    "VolumeId" UNIQUEIDENTIFIER CONSTRAINT "FK_Subdirectory_Volume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "CrawlConfigurationId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Subdirectory_CrawlConfiguration" REFERENCES "CrawlConfigurations"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_Subdirectories" PRIMARY KEY("Id"),
    CHECK((("Parent" IS NULL AND "Volume" IS NOT NULL) OR ("Parent" IS NOT NULL AND "Volume" IS NULL)) AND (("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_Subdirectory_Status" ON "Subdirectories" ("Status");

CREATE TABLE IF NOT EXISTS "CrawlConfigurations" (
    "MaxRecursionDepth" UNSIGNED SMALLINT NOT NULL DEFAULT 128,
    "MaxTotalItems" UNSIGNED BIGINT DEFAULT NULL,
    "TTL" BIGINT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim("DisplayName"))=length("DisplayName") AND "DisplayName">0) COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "StatusValue" UNSIGNED TINYINT NOT NULL CHECK("StatusValue"<=7) DEFAULT 0, -- CrawlStatus.NotRunning
    "LastCrawlStart" DATETIME DEFAULT NULL,
    "LastCrawlEnd" DATETIME DEFAULT NULL,
    "NextScheduledStart" DATETIME DEFAULT NULL,
    "RescheduleInterval" BIGINT DEFAULT NULL,
    "RescheduleFromJobEnd" BIT NOT NULL DEFAULT 0,
    "RescheduleAfterFail" BIT NOT NULL DEFAULT 0,
    "RootId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_CrawlConfiguration_Subdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_CrawlConfigurations" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_CrawlConfiguration_StatusValue" ON "CrawlConfigurations" ("StatusValue");

CREATE TABLE IF NOT EXISTS "CrawlJobLogs" (
    "MaxRecursionDepth" UNSIGNED SMALLINT NOT NULL DEFAULT 128,
    "MaxTotalItems" UNSIGNED BIGINT DEFAULT NULL,
    "TTL" BIGINT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "RootPath" NVARCHAR(4096) NOT NULL COLLATE NOCASE,
    "StatusCode" UNSIGNED TINYINT NOT NULL CHECK("StatusCode"<=7) DEFAULT 0, -- CrawlStatus.NotRunning
    "CrawlStart" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "CrawlEnd" DATETIME DEFAULT NULL,
    "StatusMessage" NVARCHAR(1024) NOT NULL CHECK(length(trim("StatusMessage"))=length("StatusMessage")) COLLATE NOCASE,
    "StatusDetail" TEXT NOT NULL,
    "ConfigurationId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_CrawlJobLog_CrawlConfiguration" REFERENCES "CrawlConfigurations"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_CrawlJobLogs" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_CrawlJobLog_StatusCode" ON "CrawlJobLogs" ("StatusCode");

CREATE TABLE IF NOT EXISTS "SubdirectoryAccessErrors" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ErrorCode" UNSIGNED TINYINT NOT NULL CHECK("ErrorCode"<=5),
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message"))=length("Message") AND "Message">0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
    "TargetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessError_DbEntity" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_SubdirectoryAccessErrors" PRIMARY KEY("Id"),
    CHECK("CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "BinaryPropertySets" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Length" BIGINT NOT NULL,
    "Hash" BINARY(16) DEFAULT NULL,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_BinaryPropertySets" PRIMARY KEY("Id"),
    CONSTRAINT "UK_BinaryPropertySet_Length_Hash" UNIQUE("Length", "Hash"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE INDEX "IDX_BinaryPropertySet_Length" ON "BinaryPropertySets" ("Length");

CREATE INDEX "IDX_BinaryPropertySet_Hash" ON "BinaryPropertySets" ("Hash");

CREATE TABLE IF NOT EXISTS "SummaryPropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SummaryPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "DocumentPropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_DocumentPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "AudioPropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_AudioPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "DRMPropertySets" (
    "DatePlayExpires" DATETIME DEFAULT NULL,
    "DatePlayStarts" DATETIME DEFAULT NULL,
    "Description" TEXT DEFAULT NULL,
    "IsProtected" BIT DEFAULT NULL,
    "PlayCount" UNSIGNED INT DEFAULT NULL,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_DRMPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "GPSPropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_GPSPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "ImagePropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_ImagePropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "MediaPropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_MediaPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "MusicPropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_MusicPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "PhotoPropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_PhotoPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "RecordedTVPropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_RecordedTVPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "VideoPropertySets" (
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
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_VideoPropertySets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "RedundantSets" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Reference" NVARCHAR(128) NOT NULL CHECK(length(trim("Reference"))=length("Reference")) COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "BinaryPropertySetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundantSet_BinaryPropertySet" REFERENCES "BinaryPropertySets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_RedundantSets" PRIMARY KEY("Id"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "Files" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "Name" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "LastAccessed" DATETIME NOT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "CreationTime" DATETIME NOT NULL,
    "LastWriteTime" DATETIME NOT NULL,
    "ParentId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_DbFsItem_Subdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    "Options" UNSIGNED TINYINT NOT NULL CHECK("Options"<=8) DEFAULT 0, -- FileCrawlOptions.None
    "Status" UNSIGNED TINYINT NOT NULL CHECK("Status"<=9) DEFAULT 0, -- FileCorrelationStatus.Dissociated
    "LastHashCalculation" DATETIME DEFAULT NULL,
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
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Id" UNIQUEIDENTIFIER NOT NULL COLLATE NOCASE,
    "ErrorCode" UNSIGNED TINYINT NOT NULL CHECK("ErrorCode"<=5),
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message"))=length("Message") AND "Message">0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
    "TargetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessError_DbEntity" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    CONSTRAINT "PK_FileAccessErrors" PRIMARY KEY("Id"),
    CHECK("CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "Redundancies" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "Reference" NVARCHAR(128) NOT NULL CHECK(length(trim("Reference"))=length("Reference")) COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "FileId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Redundancy_File" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "RedundantSetId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Redundancy_RedundantSet" REFERENCES "RedundantSets"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_Redundancies" PRIMARY KEY(""FileId", "RedundantSetId""),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "Comparisons" (
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "AreEqual" BIT NOT NULL DEFAULT 0,
    "ComparedOn" DATETIME NOT NULL,
    "BaselineId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Comparison_BaselineFile" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "CorrelativeId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_Comparison_CorrelativeFile" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);
