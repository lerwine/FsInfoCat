-- Deleting tables

DROP VIEW IF EXISTS "vPersonalVolumeTagListing";
DROP VIEW IF EXISTS "vSharedVolumeTagListing";
DROP VIEW IF EXISTS "vPersonalSubdirectoryTagListing";
DROP VIEW IF EXISTS "vSharedSubdirectoryTagListing";
DROP VIEW IF EXISTS "vPersonalFileTagListing";
DROP VIEW IF EXISTS "vSharedFileTagListing";
DROP VIEW IF EXISTS "vFileListingWithAncestorNames";
DROP VIEW IF EXISTS "vFileListingWithBinaryProperties";
DROP VIEW IF EXISTS "vFileListingWithBinaryPropertiesAndAncestorNames";
DROP VIEW IF EXISTS "vSubdirectoryListing";
DROP VIEW IF EXISTS "vSubdirectoryAncestorNames";
DROP VIEW IF EXISTS "vSubdirectoryListingWithAncestorNames";
DROP VIEW IF EXISTS "vVolumeListingWithFileSystem";
DROP VIEW IF EXISTS "vVolumeListing";
DROP VIEW IF EXISTS "vRedundantSetListing";
DROP VIEW IF EXISTS "vSharedTagDefinitionListing";
DROP VIEW IF EXISTS "vPersonalTagDefinitionListing";
DROP VIEW IF EXISTS "vFileSystemListing";
DROP VIEW IF EXISTS "vSymbolicNameListing";
DROP VIEW IF EXISTS "vSummaryPropertiesListing";
DROP VIEW IF EXISTS "vDocumentPropertiesListing";
DROP VIEW IF EXISTS "vAudioPropertiesListing";
DROP VIEW IF EXISTS "vDRMPropertiesListing";
DROP VIEW IF EXISTS "vGPSPropertiesListing";
DROP VIEW IF EXISTS "vImagePropertiesListing";
DROP VIEW IF EXISTS "vMediaPropertiesListing";
DROP VIEW IF EXISTS "vMusicPropertiesListing";
DROP VIEW IF EXISTS "vPhotoPropertiesListing";
DROP VIEW IF EXISTS "vRecordedTVPropertiesListing";
DROP VIEW IF EXISTS "vVideoPropertiesListing";
DROP VIEW IF EXISTS "vCrawlConfigListing";
DROP VIEW IF EXISTS "vCrawlConfigReport";
DROP VIEW IF EXISTS "vCrawlJobListing";
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
    "Identifier" NVARCHAR(1024) NOT NULL CHECK(length(trim("Identifier"))=length("Identifier") AND length("Identifier")>9) COLLATE NOCASE,
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
    "FoldersProcessed" BIGINT NOT NULL DEFAULT 0,
    "FilesProcessed" BIGINT NOT NULL DEFAULT 0,
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
    "Status" UNSIGNED TINYINT NOT NULL CHECK("Status"<9) DEFAULT 1,
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
    "LastAccessed" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
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
    "SummaryPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_SummaryPropertySet" REFERENCES "SummaryPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "DocumentPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_DocumentPropertySet" REFERENCES "DocumentPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "AudioPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_AudioPropertySet" REFERENCES "AudioPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "DRMPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_DRMPropertySet" REFERENCES "DRMPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "GPSPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_GPSPropertySet" REFERENCES "GPSPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "ImagePropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_ImagePropertySet" REFERENCES "ImagePropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "MediaPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_MediaPropertySet" REFERENCES "MediaPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "MusicPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_MusicPropertySet" REFERENCES "MusicPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "PhotoPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_PhotoPropertySet" REFERENCES "PhotoPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "RecordedTVPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_RecordedTVPropertySet" REFERENCES "RecordedTVPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
    "VideoPropertySetId" UNIQUEIDENTIFIER CONSTRAINT "FK_File_VideoPropertySet" REFERENCES "VideoPropertySets"("Id") ON DELETE RESTRICT DEFAULT NULL COLLATE NOCASE,
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
    "TaggedId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalFileTag_File" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "DefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalFileTag_TagDefinition" REFERENCES "PersonalTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_PersonalFileTags" PRIMARY KEY("TaggedId", "DefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "PersonalSubdirectoryTags" (
    "TaggedId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalSubdirectoryTag_Subdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "DefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalSubdirectoryTag_TagDefinition" REFERENCES "PersonalTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_PersonalSubdirectoryTags" PRIMARY KEY("TaggedId", "DefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "PersonalVolumeTags" (
    "TaggedId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalVolumeTag_Volume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "DefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_PersonalVolumeTag_TagDefinition" REFERENCES "PersonalTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_PersonalVolumeTags" PRIMARY KEY("TaggedId", "DefinitionId"),
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
    "TaggedId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedFileTag_File" REFERENCES "Files"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "DefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedFileTag_TagDefinition" REFERENCES "SharedTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SharedFileTags" PRIMARY KEY("TaggedId", "DefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "SharedSubdirectoryTags" (
    "TaggedId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedSubdirectoryTag_Subdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "DefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedSubdirectoryTag_TagDefinition" REFERENCES "SharedTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SharedSubdirectoryTags" PRIMARY KEY("TaggedId", "DefinitionId"),
    CHECK((("UpstreamId" IS NULL AND "LastSynchronizedOn" IS NULL) OR ("UpstreamId" IS NOT NULL AND "LastSynchronizedOn" IS NOT NULL)) AND "CreatedOn"<="ModifiedOn")
);

CREATE TABLE IF NOT EXISTS "SharedVolumeTags" (
    "TaggedId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedVolumeTag_Volume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "DefinitionId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SharedVolumeTag_TagDefinition" REFERENCES "SharedTagDefinitions"("Id") ON DELETE RESTRICT COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsInactive" BIT NOT NULL DEFAULT 0,
    "CreatedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "ModifiedOn" DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL COLLATE NOCASE,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
    CONSTRAINT "PK_SharedVolumeTags" PRIMARY KEY("TaggedId", "DefinitionId"),
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

CREATE VIEW IF NOT EXISTS "vSymbolicNameListing" AS SELECT "SymbolicNames".*, "FileSystems"."DisplayName" AS "FileSystemDisplayName" FROM "SymbolicNames"
	LEFT JOIN "FileSystems" ON "SymbolicNames"."FileSystemId"="FileSystems"."Id";

CREATE VIEW IF NOT EXISTS "vFileSystemListing" AS SELECT "FileSystems".*, "SymbolicNames"."Name" AS "PrimarySymbolicName",
	(SELECT "SymbolicNames"."Id" FROM "SymbolicNames" WHERE "SymbolicNames"."FileSystemId"="FileSystems"."Id" AND "SymbolicNames"."IsInactive"=0 ORDER BY "SymbolicNames"."Priority", "SymbolicNames"."CreatedOn" LIMIT 1) AS "PrimarySymbolicNameId",
	(SELECT count("SymbolicNames"."Id") FROM "SymbolicNames" WHERE "SymbolicNames"."FileSystemId"="FileSystems"."Id") AS "SymbolicNameCount",
	(SELECT count("Volumes"."Id") FROM "Volumes" WHERE "Volumes"."FileSystemId"="FileSystems"."Id") AS "VolumeCount"
	FROM "FileSystems"
	LEFT JOIN "SymbolicNames" ON "PrimarySymbolicNameId"="SymbolicNames"."Id";

CREATE VIEW IF NOT EXISTS "vPersonalTagDefinitionListing" AS SELECT "PersonalTagDefinitions".*,
	(SELECT count("PersonalVolumeTags"."TaggedId") FROM "PersonalVolumeTags" WHERE "PersonalVolumeTags"."DefinitionId"="PersonalTagDefinitions"."Id") AS "VolumeTagCount",
	(SELECT count("PersonalSubdirectoryTags"."TaggedId") FROM "PersonalSubdirectoryTags" WHERE "PersonalSubdirectoryTags"."DefinitionId"="PersonalTagDefinitions"."Id") AS "SubdirectoryTagCount",
	(SELECT count("PersonalFileTags"."TaggedId") FROM "PersonalFileTags" WHERE "PersonalFileTags"."DefinitionId"="PersonalTagDefinitions"."Id") AS "FileTagCount"
	FROM "PersonalTagDefinitions";

CREATE VIEW IF NOT EXISTS "vSharedTagDefinitionListing" AS SELECT "SharedTagDefinitions".*,
	(SELECT count("SharedVolumeTags"."TaggedId") FROM "SharedVolumeTags" WHERE "SharedVolumeTags"."DefinitionId"="SharedTagDefinitions"."Id") AS "VolumeTagCount",
	(SELECT count("SharedSubdirectoryTags"."TaggedId") FROM "SharedSubdirectoryTags" WHERE "SharedSubdirectoryTags"."DefinitionId"="SharedTagDefinitions"."Id") AS "SubdirectoryTagCount",
	(SELECT count("SharedFileTags"."TaggedId") FROM "SharedFileTags" WHERE "SharedFileTags"."DefinitionId"="SharedTagDefinitions"."Id") AS "FileTagCount"
	FROM "SharedTagDefinitions";

CREATE VIEW IF NOT EXISTS "vRedundantSetListing" AS SELECT "RedundantSets".*, "BinaryPropertySets"."Length", "BinaryPropertySets"."Hash",
	(SELECT count("Redundancies"."FileId") FROM "Redundancies" WHERE "Redundancies"."RedundantSetId"="RedundantSets"."Id") AS "RedundancyCount"
	FROM "RedundantSets"
	LEFT JOIN "BinaryPropertySets" ON "RedundantSets"."BinaryPropertySetId"="BinaryPropertySets"."Id";

CREATE VIEW IF NOT EXISTS "vVolumeListing" AS SELECT "Volumes".*, "r"."Name" AS "RootPath",
	(SELECT count("s"."Id") FROM "Subdirectories" "s" WHERE "s"."ParentId"="r"."Id") AS "RootSubdirectoryCount",
	(SELECT count("Files"."Id") FROM "Files" WHERE "Files"."ParentId"="r"."Id") AS "RootFileCount",
	(SELECT count("VolumeAccessErrors"."Id") FROM "VolumeAccessErrors" WHERE "VolumeAccessErrors"."TargetId"="Volumes"."Id") AS "AccessErrorCount",
	(SELECT count("SharedVolumeTags"."DefinitionId") FROM "SharedVolumeTags" WHERE "SharedVolumeTags"."TaggedId"="Volumes"."Id") AS "SharedTagCount",
	(SELECT count("PersonalVolumeTags"."DefinitionId") FROM "PersonalVolumeTags" WHERE "PersonalVolumeTags"."TaggedId"="Volumes"."Id") AS "PersonalTagCount"
	FROM "Volumes"
	LEFT JOIN "Subdirectories" "r" ON "Volumes"."Id"="r"."VolumeId";

CREATE VIEW IF NOT EXISTS "vVolumeListingWithFileSystem" AS SELECT "Volumes".*, "r"."Name" AS "RootPath",
	"FileSystems"."DisplayName" AS "FileSystemDisplayName", ifnull("Volumes"."ReadOnly", "FileSystems"."ReadOnly") AS "EffectiveReadOnly",
	ifnull("Volumes"."MaxNameLength", "FileSystems"."MaxNameLength") AS "EffectiveMaxNameLength",
	(SELECT count("s"."Id") FROM "Subdirectories" "s" WHERE "s"."ParentId"="r"."Id") AS "RootSubdirectoryCount",
	(SELECT count("Files"."Id") FROM "Files" WHERE "Files"."ParentId"="r"."Id") AS "RootFileCount",
	(SELECT count("VolumeAccessErrors"."Id") FROM "VolumeAccessErrors" WHERE "VolumeAccessErrors"."TargetId"="Volumes"."Id") AS "AccessErrorCount",
	(SELECT count("SharedVolumeTags"."DefinitionId") FROM "SharedVolumeTags" WHERE "SharedVolumeTags"."TaggedId"="Volumes"."Id") AS "SharedTagCount",
	(SELECT count("PersonalVolumeTags"."DefinitionId") FROM "PersonalVolumeTags" WHERE "PersonalVolumeTags"."TaggedId"="Volumes"."Id") AS "PersonalTagCount"
	FROM "Volumes"
	LEFT JOIN "Subdirectories" "r" ON "Volumes"."Id"="r"."VolumeId"
	LEFT JOIN "FileSystems" ON "Volumes"."FileSystemId"="FileSystems"."Id";

CREATE VIEW IF NOT EXISTS "vSubdirectoryListing" AS SELECT "Subdirectories".*, "CrawlConfigurations"."DisplayName" AS "CrawlConfigDisplayName",
    (SELECT count("s"."Id") FROM "Subdirectories" "s" WHERE "s"."ParentId"="Subdirectories"."Id") AS "SubdirectoryCount",
	(SELECT count("Files"."Id") FROM "Files" WHERE "Files"."ParentId"="Subdirectories"."Id") AS "FileCount",
	(SELECT count("SubdirectoryAccessErrors"."Id") FROM "SubdirectoryAccessErrors" WHERE "SubdirectoryAccessErrors"."TargetId"="Subdirectories"."Id") AS "AccessErrorCount",
	(SELECT count("SharedSubdirectoryTags"."DefinitionId") FROM "SharedSubdirectoryTags" WHERE "SharedSubdirectoryTags"."TaggedId"="Subdirectories"."Id") AS "SharedTagCount",
	(SELECT count("PersonalSubdirectoryTags"."DefinitionId") FROM "PersonalSubdirectoryTags" WHERE "PersonalSubdirectoryTags"."TaggedId"="Subdirectories"."Id") AS "PersonalTagCount"
    FROM "Subdirectories"
    LEFT JOIN "CrawlConfigurations" ON "Subdirectories"."Id"="CrawlConfigurations"."RootId";
    
CREATE VIEW IF NOT EXISTS "vSubdirectoryListingWithAncestorNames" AS SELECT "Subdirectories".*, "CrawlConfigurations"."DisplayName" AS "CrawlConfigDisplayName",
    (SELECT count("s"."Id") FROM "Subdirectories" "s" WHERE "s"."ParentId"="Subdirectories"."Id") AS "SubdirectoryCount",
	(SELECT count("Files"."Id") FROM "Files" WHERE "Files"."ParentId"="Subdirectories"."Id") AS "FileCount",
	(SELECT count("SubdirectoryAccessErrors"."Id") FROM "SubdirectoryAccessErrors" WHERE "SubdirectoryAccessErrors"."TargetId"="Subdirectories"."Id") AS "AccessErrorCount",
	(SELECT count("SharedSubdirectoryTags"."DefinitionId") FROM "SharedSubdirectoryTags" WHERE "SharedSubdirectoryTags"."TaggedId"="Subdirectories"."Id") AS "SharedTagCount",
	(SELECT count("PersonalSubdirectoryTags"."DefinitionId") FROM "PersonalSubdirectoryTags" WHERE "PersonalSubdirectoryTags"."TaggedId"="Subdirectories"."Id") AS "PersonalTagCount",
    (WITH RECURSIVE
        directlyContains("ChildId", "ParentId") AS (SELECT "Id", "ParentId" FROM "Subdirectories"),
        containerOf("ChildId") AS (SELECT "ParentId" FROM directlyContains WHERE "ChildId"="Subdirectories"."Id" UNION ALL SELECT "ParentId" FROM directlyContains JOIN containerOf USING("ChildId"))
        SELECT group_concat("ParentSubdir"."Name", '/') FROM containerOf, "Subdirectories" AS "ParentSubdir" WHERE containerOf."ChildId"="ParentSubdir"."Id"
    ) AS "AncestorNames",
    ifnull("Subdirectories"."VolumeId", (WITH RECURSIVE
        directlyContains("ChildId", "ParentId") AS (SELECT "Id", "ParentId" FROM "Subdirectories"),
        containerOf("ChildId") AS (SELECT "ParentId" FROM directlyContains WHERE "ChildId"="Subdirectories"."Id" UNION ALL SELECT "ParentId" FROM directlyContains JOIN containerOf USING("ChildId"))
        SELECT "ParentSubdir"."VolumeId" FROM containerOf, "Subdirectories" AS "ParentSubdir" WHERE containerOf."ChildId"="ParentSubdir"."Id" AND "ParentSubdir"."VolumeId" IS NOT NULL
    )) AS "EffectiveVolumeId", "Volumes"."DisplayName" AS "VolumeDisplayName", "Volumes"."VolumeName", "Volumes"."Identifier" AS "VolumeIdentifier",
	"vFileSystemListing"."DisplayName" AS "FileSystemDisplayName", "vFileSystemListing"."PrimarySymbolicName" AS "FileSystemSymbolicName" FROM "Subdirectories"
    LEFT JOIN "CrawlConfigurations" ON "Subdirectories"."Id"="CrawlConfigurations"."RootId"
	LEFT JOIN "Volumes" ON "EffectiveVolumeId"="Volumes"."Id"
	LEFT JOIN "vFileSystemListing" ON "Volumes"."FileSystemId"="vFileSystemListing"."Id";

CREATE VIEW IF NOT EXISTS "vSubdirectoryAncestorNames" AS SELECT "Subdirectories"."Id", "Subdirectories"."ParentId", "Subdirectories"."Name",(WITH RECURSIVE
    directlyContains("ChildId", "ParentId") AS (SELECT "Id", "ParentId" FROM "Subdirectories"),
    containerOf("ChildId") AS (SELECT "ParentId" FROM directlyContains WHERE "ChildId"="Subdirectories"."Id" UNION ALL SELECT "ParentId" FROM directlyContains JOIN containerOf USING("ChildId"))
    SELECT group_concat("ParentSubdir"."Name", '/') FROM containerOf, "Subdirectories" AS "ParentSubdir" WHERE containerOf."ChildId"="ParentSubdir"."Id") AS "AncestorNames" FROM "Subdirectories";
    
CREATE VIEW IF NOT EXISTS "vCrawlConfigListing" AS SELECT "CrawlConfigurations".*,
	iif("vSubdirectoryListingWithAncestorNames"."AncestorNames" IS NULL,
		"vSubdirectoryListingWithAncestorNames"."Name",
		printf('%s/%s', "vSubdirectoryListingWithAncestorNames"."Name", "vSubdirectoryListingWithAncestorNames"."AncestorNames")
	) AS "AncestorNames",
    "vSubdirectoryListingWithAncestorNames"."EffectiveVolumeId" AS "VolumeId", "vSubdirectoryListingWithAncestorNames"."VolumeDisplayName",
	"vSubdirectoryListingWithAncestorNames"."VolumeName", "vSubdirectoryListingWithAncestorNames"."VolumeIdentifier",
	"vSubdirectoryListingWithAncestorNames"."FileSystemDisplayName", "vSubdirectoryListingWithAncestorNames"."FileSystemSymbolicName" FROM "CrawlConfigurations"
	LEFT JOIN "vSubdirectoryListingWithAncestorNames" ON "CrawlConfigurations"."RootId"="vSubdirectoryListingWithAncestorNames"."Id";
    
CREATE VIEW IF NOT EXISTS "vFileListingWithAncestorNames" AS SELECT "Files".*,
	iif("vSubdirectoryListingWithAncestorNames"."AncestorNames" IS NULL,
		"vSubdirectoryListingWithAncestorNames"."Name",
		printf('%s/%s', "vSubdirectoryListingWithAncestorNames"."Name", "vSubdirectoryListingWithAncestorNames"."AncestorNames")
	) AS "AncestorNames",
    "vSubdirectoryListingWithAncestorNames"."EffectiveVolumeId", "vSubdirectoryListingWithAncestorNames"."VolumeDisplayName",
	"vSubdirectoryListingWithAncestorNames"."VolumeName", "vSubdirectoryListingWithAncestorNames"."VolumeIdentifier",
	"vSubdirectoryListingWithAncestorNames"."FileSystemDisplayName", "vSubdirectoryListingWithAncestorNames"."FileSystemSymbolicName"
	FROM "Files"
	LEFT JOIN "vSubdirectoryListingWithAncestorNames" ON "Files"."ParentId"="vSubdirectoryListingWithAncestorNames"."Id";
    
CREATE VIEW IF NOT EXISTS "vFileListingWithBinaryProperties" AS SELECT "Files".*, "BinaryPropertySets"."Length", "BinaryPropertySets"."Hash",
	(SELECT count("Redundancies"."RedundantSetId") FROM "Redundancies" WHERE "Redundancies"."FileId"="Files"."Id") AS "RedundancyCount",
	(SELECT count("Comparisons"."AreEqual") FROM "Comparisons" WHERE "Comparisons"."BaselineId"="Files"."Id" OR "Comparisons"."CorrelativeId"="Files"."Id") AS "ComparisonCount",
	(SELECT count("FileAccessErrors"."Id") FROM "FileAccessErrors" WHERE "FileAccessErrors"."TargetId"="Files"."Id") AS "AccessErrorCount",
	(SELECT count("SharedFileTags"."DefinitionId") FROM "SharedFileTags" WHERE "SharedFileTags"."TaggedId"="Files"."Id") AS "SharedTagCount",
	(SELECT count("PersonalFileTags"."DefinitionId") FROM "PersonalFileTags" WHERE "PersonalFileTags"."TaggedId"="Files"."Id") AS "PersonalTagCount" FROM "Files"
	LEFT JOIN "BinaryPropertySets" ON "Files"."BinaryPropertySetId"="BinaryPropertySets"."Id";

CREATE VIEW IF NOT EXISTS "vFileListingWithBinaryPropertiesAndAncestorNames" AS SELECT "Files".*, "BinaryPropertySets"."Length", "BinaryPropertySets"."Hash",
	iif("vSubdirectoryListingWithAncestorNames"."AncestorNames" IS NULL,
		"vSubdirectoryListingWithAncestorNames"."Name",
		printf('%s/%s', "vSubdirectoryListingWithAncestorNames"."Name", "vSubdirectoryListingWithAncestorNames"."AncestorNames")
	) AS "AncestorNames",
    "vSubdirectoryListingWithAncestorNames"."EffectiveVolumeId", "vSubdirectoryListingWithAncestorNames"."VolumeDisplayName",
	"vSubdirectoryListingWithAncestorNames"."VolumeName", "vSubdirectoryListingWithAncestorNames"."VolumeIdentifier",
	"vSubdirectoryListingWithAncestorNames"."FileSystemDisplayName", "vSubdirectoryListingWithAncestorNames"."FileSystemSymbolicName"
	FROM "Files"
	LEFT JOIN "BinaryPropertySets" ON "Files"."BinaryPropertySetId"="BinaryPropertySets"."Id"
	LEFT JOIN "vSubdirectoryListingWithAncestorNames" ON "Files"."ParentId"="vSubdirectoryListingWithAncestorNames"."Id";
    
CREATE VIEW IF NOT EXISTS "vCrawlJobListing" AS SELECT "CrawlJobLogs".*, "CrawlConfigurations"."DisplayName" AS "ConfigurationDisplayName" FROM "CrawlJobLogs"
	LEFT JOIN "CrawlConfigurations" ON "CrawlJobLogs"."ConfigurationId" = "CrawlConfigurations"."Id";
    
CREATE VIEW IF NOT EXISTS "vSummaryPropertiesListing" AS SELECT "SummaryPropertySets".*, Count("Files"."Id") FROM "SummaryPropertySets"
    LEFT JOIN "Files" ON "SummaryPropertySets"."Id" = "Files"."SummaryPropertySetId";

CREATE VIEW IF NOT EXISTS "vDocumentPropertiesListing" AS SELECT "DocumentPropertySets".*, Count("Files"."Id") FROM "DocumentPropertySets"
    LEFT JOIN "Files" ON "DocumentPropertySets"."Id" = "Files"."DocumentPropertySetId";

CREATE VIEW IF NOT EXISTS "vAudioPropertiesListing" AS SELECT "AudioPropertySets".*, Count("Files"."Id") FROM "AudioPropertySets"
    LEFT JOIN "Files" ON "AudioPropertySets"."Id" = "Files"."AudioPropertySetId";

CREATE VIEW IF NOT EXISTS "vDRMPropertiesListing" AS SELECT "DRMPropertySets".*, Count("Files"."Id") FROM "DRMPropertySets"
    LEFT JOIN "Files" ON "DRMPropertySets"."Id" = "Files"."DRMPropertySetId";

CREATE VIEW IF NOT EXISTS "vGPSPropertiesListing" AS SELECT "GPSPropertySets".*, Count("Files"."Id") FROM "GPSPropertySets"
    LEFT JOIN "Files" ON "GPSPropertySets"."Id" = "Files"."GPSPropertySetId";

CREATE VIEW IF NOT EXISTS "vImagePropertiesListing" AS SELECT "ImagePropertySets".*, Count("Files"."Id") FROM "ImagePropertySets"
    LEFT JOIN "Files" ON "ImagePropertySets"."Id" = "Files"."ImagePropertySetId";

CREATE VIEW IF NOT EXISTS "vMediaPropertiesListing" AS SELECT "MediaPropertySets".*, Count("Files"."Id") FROM "MediaPropertySets"
    LEFT JOIN "Files" ON "MediaPropertySets"."Id" = "Files"."MediaPropertySetId";

CREATE VIEW IF NOT EXISTS "vMusicPropertiesListing" AS SELECT "MusicPropertySets".*, Count("Files"."Id") FROM "MusicPropertySets"
    LEFT JOIN "Files" ON "MusicPropertySets"."Id" = "Files"."MusicPropertySetId";

CREATE VIEW IF NOT EXISTS "vPhotoPropertiesListing" AS SELECT "PhotoPropertySets".*, Count("Files"."Id") FROM "PhotoPropertySets"
    LEFT JOIN "Files" ON "PhotoPropertySets"."Id" = "Files"."PhotoPropertySetId";

CREATE VIEW IF NOT EXISTS "vRecordedTVPropertiesListing" AS SELECT "RecordedTVPropertySets".*, Count("Files"."Id") FROM "RecordedTVPropertySets"
    LEFT JOIN "Files" ON "RecordedTVPropertySets"."Id" = "Files"."RecordedTVPropertySetId";

CREATE VIEW IF NOT EXISTS "vVideoPropertiesListing" AS SELECT "VideoPropertySets".*, Count("Files"."Id") FROM "VideoPropertySets"
    LEFT JOIN "Files" ON "VideoPropertySets"."Id" = "Files"."VideoPropertySetId";

CREATE VIEW IF NOT EXISTS "vPersonalVolumeTagListing" AS SELECT "PersonalVolumeTags".*, "PersonalTagDefinitions"."Name", "PersonalTagDefinitions"."Description"
	FROM "PersonalVolumeTags"
	LEFT JOIN "PersonalTagDefinitions" ON "PersonalVolumeTags"."DefinitionId"="PersonalTagDefinitions"."Id";

CREATE VIEW IF NOT EXISTS "vSharedVolumeTagListing" AS SELECT "SharedVolumeTags".*, "SharedTagDefinitions"."Name", "SharedTagDefinitions"."Description"
	FROM "SharedVolumeTags"
	LEFT JOIN "SharedTagDefinitions" ON "SharedVolumeTags"."DefinitionId"="SharedTagDefinitions"."Id";

CREATE VIEW IF NOT EXISTS "vPersonalSubdirectoryTagListing" AS SELECT "PersonalSubdirectoryTags".*, "PersonalTagDefinitions"."Name", "PersonalTagDefinitions"."Description"
	FROM "PersonalSubdirectoryTags"
	LEFT JOIN "PersonalTagDefinitions" ON "PersonalSubdirectoryTags"."DefinitionId"="PersonalTagDefinitions"."Id";

CREATE VIEW IF NOT EXISTS "vSharedSubdirectoryTagListing" AS SELECT "SharedSubdirectoryTags".*, "SharedTagDefinitions"."Name", "SharedTagDefinitions"."Description"
	FROM "SharedSubdirectoryTags"
	LEFT JOIN "SharedTagDefinitions" ON "SharedSubdirectoryTags"."DefinitionId"="SharedTagDefinitions"."Id";

CREATE VIEW IF NOT EXISTS "vPersonalFileTagListing" AS SELECT "PersonalFileTags".*, "PersonalTagDefinitions"."Name", "PersonalTagDefinitions"."Description"
	FROM "PersonalFileTags"
	LEFT JOIN "PersonalTagDefinitions" ON "PersonalFileTags"."DefinitionId"="PersonalTagDefinitions"."Id";

CREATE VIEW IF NOT EXISTS "vSharedFileTagListing" AS SELECT "SharedFileTags".*, "SharedTagDefinitions"."Name", "SharedTagDefinitions"."Description"
	FROM "SharedFileTags"
	LEFT JOIN "SharedTagDefinitions" ON "SharedFileTags"."DefinitionId"="SharedTagDefinitions"."Id";

CREATE VIEW IF NOT EXISTS "vSharedFileTagListing" AS SELECT "SharedFileTags".*, "SharedTagDefinitions"."Name", "SharedTagDefinitions"."Description"
	FROM "SharedFileTags"
	LEFT JOIN "SharedTagDefinitions" ON "SharedFileTags"."DefinitionId"="SharedTagDefinitions"."Id";

CREATE VIEW IF NOT EXISTS "vCrawlConfigReport" AS SELECT "CrawlConfigurations".*,
	iif("vSubdirectoryListingWithAncestorNames"."AncestorNames" IS NULL,
		"vSubdirectoryListingWithAncestorNames"."Name",
		printf('%s/%s', "vSubdirectoryListingWithAncestorNames"."Name", "vSubdirectoryListingWithAncestorNames"."AncestorNames")
	) AS "AncestorNames",
    "vSubdirectoryListingWithAncestorNames"."EffectiveVolumeId" AS "VolumeId", "vSubdirectoryListingWithAncestorNames"."VolumeDisplayName",
	"vSubdirectoryListingWithAncestorNames"."VolumeName", "vSubdirectoryListingWithAncestorNames"."VolumeIdentifier",
	"vSubdirectoryListingWithAncestorNames"."FileSystemDisplayName", "vSubdirectoryListingWithAncestorNames"."FileSystemSymbolicName",
	(SELECT COUNT("Id") FROM "CrawlJobLogs" WHERE "ConfigurationId"="CrawlConfigurations"."Id" AND "StatusCode"=2) AS "SucceededCount",
	(SELECT COUNT("Id") FROM "CrawlJobLogs" WHERE "ConfigurationId"="CrawlConfigurations"."Id" AND "StatusCode"=3) AS "TimedOutCount",
	(SELECT COUNT("Id") FROM "CrawlJobLogs" WHERE "ConfigurationId"="CrawlConfigurations"."Id" AND "StatusCode"=4) AS "ItemLimitReachedCount",
	(SELECT COUNT("Id") FROM "CrawlJobLogs" WHERE "ConfigurationId"="CrawlConfigurations"."Id" AND "StatusCode"=5) AS "CanceledCount",
	(SELECT COUNT("Id") FROM "CrawlJobLogs" WHERE "ConfigurationId"="CrawlConfigurations"."Id" AND "StatusCode"=6) AS "FailedCount",
	(SELECT AVG(ROUND((JULIANDAY("CrawlEnd") - JULIANDAY("CrawlStart")) * 86400)) FROM "CrawlJobLogs" WHERE "ConfigurationId"="CrawlConfigurations"."Id" AND "CrawlEnd" IS NOT NULL) AS "AverageDuration",
	(SELECT MAX(ROUND((JULIANDAY("CrawlEnd") - JULIANDAY("CrawlStart")) * 86400)) FROM "CrawlJobLogs" WHERE "ConfigurationId"="CrawlConfigurations"."Id" AND "CrawlEnd" IS NOT NULL) AS "MaxDuration"
	FROM "CrawlConfigurations"
	LEFT JOIN "vSubdirectoryListingWithAncestorNames" ON "CrawlConfigurations"."RootId"="vSubdirectoryListingWithAncestorNames"."Id";

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
	VALUES ('c48c1c92-154c-43cf-a277-53223d5c1510', 'testazureshare (on servicenowdiag479.file.core.windows.net)', '', 'file://servicenowdiag479.file.core.windows.net/testazureshare', '0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd', 255, 4, 2, '2021-06-05 00:58:35', '2021-09-24 14:15:09');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('20a61d4b-80c2-48a3-8df6-522e598aae08', 'C:\', '2021-06-05 00:58:34', '2019-03-19 00:37:21', '2021-06-04 13:47:20', NULL, '7920cf04-9e7f-4414-986e-d1bfba011db7', '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('b63144ce-91cb-4cb8-a407-8d3490a8c90c', 'Users', '2021-06-05 00:58:34', '2019-12-07 04:03:44', '2021-03-09 23:51:36', '20a61d4b-80c2-48a3-8df6-522e598aae08', NULL, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('38a40fde-acf0-4cc5-9302-d37ec2cbb631', 'lerwi', '2021-06-05 00:58:34', '2021-03-09 23:51:36', '2021-06-04 03:09:25', 'b63144ce-91cb-4cb8-a407-8d3490a8c90c', NULL, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a', 'Videos', '2021-06-05 00:58:34', '2019-11-23 20:30:23', '2021-05-16 18:54:50', '38a40fde-acf0-4cc5-9302-d37ec2cbb631', NULL, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "RootId", "StatusValue", "LastCrawlStart", "LastCrawlEnd", "CreatedOn", "ModifiedOn")
    VALUES ('9c91ba89-6ab5-4d4e-9798-0d926b405f41', 'Lenny''s Laptop Videos', '3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a', 2, '2021-08-01 14:54:22', '2021-08-01 14:57:16', '2021-07-31 15:28:18', '2021-08-01 14:57:16');
INSERT INTO "CrawlJobLogs" ("Id", "MaxRecursionDepth", "RootPath", "StatusCode", "CrawlStart", "CrawlEnd", "StatusMessage", "StatusDetail", "FoldersProcessed", "FilesProcessed", "CreatedOn", "ModifiedOn", "ConfigurationId")
    VALUES ('563160b2-cb6e-4e3b-855c-89eebefdf8bd', 256, 'C:\Users\lerwi\Downloads', 2, '2021-08-01 14:54:22', '2021-08-01 14:57:16', 'Directory was empty.', '', 0, 0, '2021-08-01 14:57:16', '2021-08-01 14:57:16', '9c91ba89-6ab5-4d4e-9798-0d926b405f41');
INSERT INTO "BinaryPropertySets" ("Id", "Length", "CreatedOn", "ModifiedOn")
    VALUES('82d46e21-5eba-4f1b-8c99-78cb94689316', 25057982, '2021-08-22 14:32:22', '2021-08-22 14:32:2s');
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "CreatedOn", "ParentId", "BinaryPropertySetId")
    VALUES ('5f7b7beb-5aae-496a-925c-b3a43666c742', 'the move down on the bay - YouTube.webm', '2020-07-19 00:02:07', '2020-07-19 00:04:35', '2021-08-22 14:32:22', '3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a', '82d46e21-5eba-4f1b-8c99-78cb94689316');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('b228346f-7023-4ba9-afe3-8e9ff758971f', '\\servicenowdiag479.file.core.windows.net\testazureshare', '2020-05-11 09:31:25', '2021-06-04 13:48:55', '2021-08-22 15:04:12', NULL, 'c48c1c92-154c-43cf-a277-53223d5c1510', '2021-08-22 15:04:12', '2021-08-22 15:04:12');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "Options", "Notes", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('c7f6d510-6acf-43c0-b5d5-d5f99cca0ce3', 'webroot', '2021-08-22 15:04:12', '2021-01-03 12:30:47', '2021-01-05 10:52:06', 1, 'Only scanning the root files.', 'b228346f-7023-4ba9-afe3-8e9ff758971f', NULL, '2021-08-22 15:04:12', '2021-08-22 15:04:12');
INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "RootId", "StatusValue", "LastCrawlStart", "LastCrawlEnd", "CreatedOn", "ModifiedOn")
    VALUES ('fa6c52c5-862b-4bf7-a145-ad7d2533a1d2', 'Web Root', 'c7f6d510-6acf-43c0-b5d5-d5f99cca0ce3', 0, '2021-08-22 15:04:17', '2021-08-22 15:04:31', '2021-08-22 15:04:12', '2021-08-22 15:04:32');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('A85D3A22-C402-43F1-AC82-B2B83B843C0F', 'Downloads', '2021-09-24 07:17:22', '2019-11-23 20:30:23', '2021-09-24 06:16:50', '38a40fde-acf0-4cc5-9302-d37ec2cbb631', NULL, '2021-09-24 07:17:34', '2021-09-24 07:17:34');
INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "RootId", "StatusValue", "CreatedOn", "ModifiedOn")
    VALUES ('5221E107-D03D-4D9D-AB2A-55425AF103E0', 'Local Downloads', 'A85D3A22-C402-43F1-AC82-B2B83B843C0F', 7, '2021-09-24 07:18:24', '2021-09-24 07:18:24');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "Options", "Status", "VolumeId", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('C8E9C683-82A2-4BCE-8C59-2E57055FFEA7', 'OneDrive', '2021-09-24 08:00:03', '2019-11-23 20:35:06', '2021-09-24 06:01:39', 0, 1, NULL, '38a40fde-acf0-4cc5-9302-d37ec2cbb631', '2021-09-24 08:00:04', '2021-09-24 08:00:04');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "Options", "Status", "VolumeId", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('2AB26D8B-562C-44CB-8C1C-971733F5DC04', 'Music', '2021-09-24 08:00:03', '2019-11-23 20:35:49', '2021-09-20 18:48:49', 0, 1, NULL, 'C8E9C683-82A2-4BCE-8C59-2E57055FFEA7', '2021-09-24 08:00:04', '2021-09-24 08:00:04');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "Options", "Status", "VolumeId", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('58CB39A1-6080-4F73-A1E7-34274552B47B', 'SmashMouth', '2021-09-24 08:00:03', '2021-07-22 23:06:10', '2021-07-22 23:06:10', 0, 0, NULL, '2AB26D8B-562C-44CB-8C1C-971733F5DC04', '2021-09-24 08:00:04', '2021-09-24 08:00:04');
INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "MaxRecursionDepth", "RootId", "StatusValue", "CreatedOn", "ModifiedOn")
    VALUES ('2DD01786-78F7-45A3-8C18-7B02E3336768', 'Music folder', 256, '58CB39A1-6080-4F73-A1E7-34274552B47B', 0,	'2021-09-24 08:01:01', '2021-09-24 08:01:01');
INSERT INTO "VolumeAccessErrors" ("Id", "ErrorCode", "Message", "CreatedOn", "ModifiedOn", "TargetId")
    VALUES ('b806e05a-b705-4ef7-b127-a8e477125cfc', 2, 'Network unreachable.', '2021-09-24 14:15:09', '2021-09-24 14:15:09', 'c48c1c92-154c-43cf-a277-53223d5c1510');
INSERT INTO "CrawlJobLogs" ("Id", "RootPath", "StatusCode", "CrawlStart", "CrawlEnd", "StatusMessage", "StatusDetail", "FoldersProcessed", "FilesProcessed", "CreatedOn", "ModifiedOn", "ConfigurationId")
    VALUES ('7a337ebd-4dc6-4560-ba21-bc1ed1262d49', '\\servicenowdiag479.file.core.windows.net\testazureshare\webroot', 6, '2021-09-24 14:12:06', '2021-09-24 14:15:09', 'Network unreachable.', '', 0, 0, '2021-09-24 14:15:09', '2021-09-24 14:15:09', 'fa6c52c5-862b-4bf7-a145-ad7d2533a1d2');
