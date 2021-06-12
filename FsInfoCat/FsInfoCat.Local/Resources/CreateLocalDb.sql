-- Deleting tables

DROP TABLE IF EXISTS "VolumeAccessErrors";
DROP TABLE IF EXISTS "SubdirectoryAccessErrors";
DROP TABLE IF EXISTS "FileAccessErrors";
DROP TABLE IF EXISTS "Comparisons";
DROP TABLE IF EXISTS "Redundancies";
DROP TABLE IF EXISTS "Files";
DROP TABLE IF EXISTS "RedundantSets";
DROP TABLE IF EXISTS "ContentInfos";
DROP TABLE IF EXISTS "ExtendedProperties";
DROP TABLE IF EXISTS "CrawlConfigurations";
PRAGMA foreign_keys = OFF;
DROP TABLE IF EXISTS "Subdirectories";
PRAGMA foreign_keys = ON;
DROP TABLE IF EXISTS "Volumes";
DROP TABLE IF EXISTS "SymbolicNames";
DROP TABLE IF EXISTS "FileSystems";

-- Creating tables

CREATE TABLE IF NOT EXISTS "FileSystems" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"DisplayName"	NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName)) = length(DisplayName) AND length(DisplayName)>0) UNIQUE COLLATE NOCASE,
	"CaseSensitiveSearch"	BIT NOT NULL DEFAULT 0,
	"ReadOnly"	BIT NOT NULL DEFAULT 0,
	"MaxNameLength"	INT NOT NULL CHECK(MaxNameLength>=1) DEFAULT 255,
	"DefaultDriveType"	TINYINT CHECK(DefaultDriveType IS NULL OR (DefaultDriveType>=0 AND DefaultDriveType<7)),
	"Notes"	TEXT NOT NULL DEFAULT '',
	"IsInactive"	BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_FileSystems" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "SymbolicNames" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Name"	NVARCHAR(256) NOT NULL CHECK(length(trim(Name)) = length(Name) AND length(Name)>0) UNIQUE COLLATE NOCASE,
    "Priority" INT NOT NULL DEFAULT 0,
	"Notes"	TEXT NOT NULL DEFAULT '',
	"IsInactive"	BIT NOT NULL DEFAULT 0,
	"FileSystemId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SymbolicNameFileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_SymbolicNames" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "Volumes" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName)) = length(DisplayName) AND length(DisplayName)>0) COLLATE NOCASE,
    "VolumeName" NVARCHAR(128) NOT NULL CHECK(length(trim(VolumeName)) = length(VolumeName)) COLLATE NOCASE,
    "Identifier" NVARCHAR(1024) NOT NULL CHECK(length(trim(Identifier)) = length(Identifier) AND length(Identifier)>0) UNIQUE COLLATE NOCASE,
    "CaseSensitiveSearch" BIT DEFAULT NULL,
    "ReadOnly" BIT DEFAULT NULL,
    "MaxNameLength" INT CHECK(MaxNameLength IS NULL OR MaxNameLength>=1) DEFAULT NULL,
    "Type" TINYINT NOT NULL CHECK(Type>=0 AND Type<7) DEFAULT 0,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Status" TINYINT NOT NULL CHECK(Type>=0 AND Type<7) DEFAULT 0,
	"FileSystemId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_VolumeFileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_Volumes" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "VolumeAccessErrors" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message")) = length("Message") AND length("Message")>0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
    "ErrorCode" INT NOT NULL,
	"TargetId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessErrorVolume" REFERENCES "Volume"("Id") ON DELETE RESTRICT,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_VolumeAccessErrors" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn)
);

CREATE TABLE IF NOT EXISTS "CrawlConfigurations" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim("DisplayName")) = length("DisplayName") AND length("DisplayName")>0) COLLATE NOCASE,
	"IsInactive"	BIT NOT NULL DEFAULT 0,
    "MaxRecursionDepth" INT NOT NULL CHECK("MaxRecursionDepth">=0 AND "MaxRecursionDepth"<65536) DEFAULT 256,
    "MaxTotalItems" BIGINT NOT NULL CHECK("MaxTotalItems">0),
    "Notes" TEXT NOT NULL DEFAULT '',
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_CrawlConfigurations" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "Subdirectories" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Name" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "Options" TINYINT  NOT NULL CHECK(Options>=0 AND Options<64) DEFAULT 0,
    "LastAccessed" DATETIME  NOT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Status" TINYINT  NOT NULL CHECK(Options>=0 AND Options<4) DEFAULT 0,
	"CreationTime"	DATETIME NOT NULL,
	"LastWriteTime"	DATETIME NOT NULL,
	"ParentId"	UNIQUEIDENTIFIER CONSTRAINT "FK_SubdirectoryParent" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
	"VolumeId"	UNIQUEIDENTIFIER CONSTRAINT "FK_SubdirectoryVolume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT,
    "CrawlConfigurationId"	UNIQUEIDENTIFIER CONSTRAINT "FK_SubdirectoryCrawlConfiguration" REFERENCES CrawlConfigurations("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_Subdirectories" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND
        ((ParentId IS NULL AND VolumeId IS NOT NULL) OR
        (ParentId IS NOT NULL AND VolumeId IS NULL AND length(trim(Name))>0)))
);

CREATE TABLE IF NOT EXISTS "SubdirectoryAccessErrors" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Message" NVARCHAR(1024) NOT NULL CHECK(length(trim("Message")) = length("Message") AND length("Message")>0) COLLATE NOCASE,
    "Details" TEXT NOT NULL DEFAULT '',
    "ErrorCode" INT NOT NULL,
	"TargetId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_AccessErrorSubdirectory" REFERENCES "Subdirectory"("Id") ON DELETE RESTRICT,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_SubdirectoryccessErrors" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn)
);

CREATE TABLE IF NOT EXISTS "Files" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Name" NVARCHAR(1024) NOT NULL CHECK(length(trim(Name))>0) COLLATE NOCASE,
    "Options" TINYINT  NOT NULL CHECK(Options>=0 AND Options<15) DEFAULT 0,
    "LastAccessed" DATETIME  NOT NULL,
    "LastHashCalculation" DATETIME DEFAULT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Deleted" BIT NOT NULL DEFAULT 0,
	"CreationTime"	DATETIME NOT NULL,
	"LastWriteTime"	DATETIME NOT NULL,
    "ExtendedPropertyId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileExtendedProperty" REFERENCES "ExtendedProperties"("Id") ON DELETE RESTRICT,
	"ContentId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FileContentInfo" REFERENCES "ContentInfos"("Id") ON DELETE RESTRICT,
	"ParentId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FileSubdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_Files" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

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

CREATE TABLE IF NOT EXISTS "ExtendedProperties" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Width"	INT NOT NULL CHECK(Width>=0 AND Width<65536),
    "Height" INT NOT NULL CHECK(Height>=0 AND Height<65536),
    "Duration" BIGINT CHECK(Duration>=0) DEFAULT NULL,
    "FrameCount" BIGINT CHECK(PixelPerUnitX>=0 AND PixelPerUnitX < 4294967296) DEFAULT NULL,
    "TrackNumber" BIGINT CHECK(PixelPerUnitX>=0 AND PixelPerUnitX < 4294967296) DEFAULT NULL,
    "Bitrate" BIGINT CHECK(PixelPerUnitX>=0 AND PixelPerUnitX < 4294967296) DEFAULT NULL,
    "FrameRate" BIGINT CHECK(PixelPerUnitX>=0 AND PixelPerUnitX < 4294967296) DEFAULT NULL,
    "SamplesPerPixel" INT CHECK(SamplesPerPixel>=0 AND SamplesPerPixel<65536) DEFAULT NULL,
    "PixelPerUnitX" BIGINT CHECK(PixelPerUnitX>=0 AND PixelPerUnitX < 4294967296) DEFAULT NULL,
    "PixelPerUnitY" BIGINT CHECK(PixelPerUnitY>=0 AND PixelPerUnitY < 4294967296) DEFAULT NULL,
    "Compression" INT CHECK(Compression>=0 AND Compression<65536)  DEFAULT NULL,
    "XResNumerator" BIGINT CHECK(PixelPerUnitX>=0 AND PixelPerUnitX < 4294967296) DEFAULT NULL,
    "XResDenominator" BIGINT CHECK(PixelPerUnitX>=0 AND PixelPerUnitX < 4294967296) DEFAULT NULL,
    "YResNumerator" BIGINT CHECK(PixelPerUnitX>=0 AND PixelPerUnitX < 4294967296) DEFAULT NULL,
    "YResDenominator" BIGINT CHECK(PixelPerUnitX>=0 AND PixelPerUnitX < 4294967296) DEFAULT NULL,
    "ResolutionXUnit" INT CHECK(ResolutionXUnit>=0 AND ResolutionXUnit<65536) DEFAULT NULL,
    "ResolutionYUnit" INT CHECK(ResolutionYUnit>=0 AND ResolutionYUnit<65536) DEFAULT NULL,
    "JPEGProc" INT CHECK(JPEGProc>=0 AND JPEGProc<65536) DEFAULT NULL,
    "JPEGQuality" INT CHECK(JPEGQuality>=0 AND JPEGQuality<65536) DEFAULT NULL,
    "DateTime" DateTime DEFAULT NULL,
    "Title" NVARCHAR(1024) DEFAULT NULL,
    "Description" TEXT DEFAULT NULL,
    "Copyright" NVARCHAR(1024) DEFAULT NULL,
    "SoftwareUsed" NVARCHAR(1024) DEFAULT NULL,
    "Artist" NVARCHAR(1024) DEFAULT NULL,
    "HostComputer" NVARCHAR(1024) DEFAULT NULL,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_ExtendedProperties" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND
        (XResNumerator IS NULL) = (XResDenominator IS NULL) AND (YResNumerator IS NULL) = (YResDenominator IS NULL))
);

CREATE TABLE IF NOT EXISTS "ContentInfos" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Length"	BIGINT NOT NULL CHECK(Length>=0),
	"Hash"	BINARY(16) CHECK(Hash IS NULL OR length(HASH)=16),
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_ContentInfo" PRIMARY KEY("Id"),
	CONSTRAINT "UK_LengthHash" UNIQUE("Length","Hash"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TABLE IF NOT EXISTS "RedundantSets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"RemediationStatus"	TINYINT NOT NULL DEFAULT 1 CHECK(RemediationStatus>=0 AND RemediationStatus<9),
    "Reference" NVARCHAR(128) NOT NULL DEFAULT '' COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
	"ContentInfoId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundantSeContentInfo" REFERENCES "ContentInfos"("Id") ON DELETE RESTRICT,
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
    "SourceFileId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_ComparisonSourceFile" REFERENCES "Files"("Id") ON DELETE RESTRICT,
    "TargetFileId" UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_ComparisonTargetFile" REFERENCES "Files"("Id") ON DELETE RESTRICT,
    "AreEqual" BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_Comparisons" PRIMARY KEY("SourceFileId","TargetFileId"),
    CHECK(CreatedOn<=ModifiedOn AND SourceFileId<>TargetFileId AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);

CREATE TRIGGER IF NOT EXISTS validate_new_redundancy 
   BEFORE INSERT
   ON Redundancies
   WHEN (SELECT COUNT(f.Id) FROM Files f LEFT JOIN RedundantSets r ON f.ContentInfoId=r.ContentInfoId WHERE f.Id=NEW.FileId AND r.Id=NEW.RedundantSetId)=0
BEGIN
    SELECT RAISE (ABORT,'File does not have content info as the redundancy set.');
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
INSERT INTO "Subdirectories" ("Id", "Name", "VolumeId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('6743e340-0ffc-41f6-81bc-fe01d3db944d', 'C:\', '7920cf04-9e7f-4414-986e-d1bfba011db7', '2019-03-19 00:37:21', '2021-05-27 17:24:13', '2021-06-05 00:58:36', '2021-06-05 00:58:36', '2021-06-05 00:58:36');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('37cd633c-a544-46fb-80c0-8832944a47a0', 'Users', '6743e340-0ffc-41f6-81bc-fe01d3db944d', '2019-12-07 04:03:44', '2021-03-09 23:51:36', '2021-06-05 00:58:37', '2021-06-05 00:58:37', '2021-06-05 00:58:37');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('513fe108-9777-4ff1-84bb-1836ceebc958', 'lerwi', '37cd633c-a544-46fb-80c0-8832944a47a0', '2021-03-09 23:51:36', '2021-06-02 22:49:19', '2021-06-05 00:58:39', '2021-06-05 00:58:39', '2021-06-05 00:58:39');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('941fcb97-9ea7-4c95-be4f-4b1b21ff1ead', 'OneDrive', '513fe108-9777-4ff1-84bb-1836ceebc958', '2019-11-23 20:35:06', '2021-06-09 18:49:25', '2021-06-05 00:58:42', '2021-06-05 00:58:42', '2021-06-05 00:58:42');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('dfc1b888-4bfc-4ad8-bfef-e46ba491fbc3', 'Music', '941fcb97-9ea7-4c95-be4f-4b1b21ff1ead', '2019-11-23 20:35:49', '2021-05-27 16:12:42', '2021-06-05 00:58:44', '2021-06-05 00:58:44', '2021-06-05 00:58:44');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('957378c1-b275-49e5-b6d0-d2dd3df1eb19', 'Discovery', 'dfc1b888-4bfc-4ad8-bfef-e46ba491fbc3', '2021-05-26 15:33:27', '2021-05-26 15:33:28', '2021-06-05 00:58:45', '2021-06-05 00:58:45', '2021-06-05 00:58:45');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('107a63e4-368a-43c1-b735-b5ef81d0ad11', 'Archive', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:28', '2021-05-26 15:33:28', '2021-06-05 00:58:48', '2021-06-05 00:58:48', '2021-06-05 00:58:48');
INSERT INTO "ContentInfos" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn") VALUES ('f88cd035-18e5-4146-ac1e-b8382e89f2c9', 3967853, X'16c43f4f7e5922486395276be56d20a8', '2021-06-10 01:48:08', '2021-06-10 01:48:08');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('d51373d8-20a5-421a-9a63-d136b2244b80', '09-Don''t Bring Me Down.wma', 'f88cd035-18e5-4146-ac1e-b8382e89f2c9', '107a63e4-368a-43c1-b735-b5ef81d0ad11', '2021-05-26 15:33:28', '2014-08-17 22:01:29', '2021-06-10 01:48:08', '2021-06-05 00:58:51', '2021-06-10 01:48:08');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('24e2f71d-8233-4722-8776-328b83566d6e', 1543837, '2021-06-05 00:58:54', '2021-06-05 00:58:54');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('c9798f77-72d5-4803-a8f0-231235a8202f', '10-On The run.mp3', '24e2f71d-8233-4722-8776-328b83566d6e', '107a63e4-368a-43c1-b735-b5ef81d0ad11', '2021-05-26 15:33:28', '2014-08-17 22:17:36', '2021-06-05 00:58:57', '2021-06-05 00:58:57', '2021-06-05 00:58:57');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('2926322b-af17-4a7a-8f36-51ca74e4b6d5', 950862, '2021-06-05 00:58:59', '2021-06-05 00:58:59');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('bb915b7b-dc7d-464b-8f28-939eb787420f', '11-Second Time Around.mp3', '2926322b-af17-4a7a-8f36-51ca74e4b6d5', '107a63e4-368a-43c1-b735-b5ef81d0ad11', '2021-05-26 15:33:28', '2014-08-17 22:17:33', '2021-06-05 00:59:01', '2021-06-05 00:59:01', '2021-06-05 00:59:01');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('4b78d8ef-f057-4628-830d-b04f57350ae4', 3970642, '2021-06-05 00:59:03', '2021-06-05 00:59:03');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('6833b8e4-34a2-4542-8232-4c974ebfd926', '12-Little Town Flirt.mp3', '4b78d8ef-f057-4628-830d-b04f57350ae4', '107a63e4-368a-43c1-b735-b5ef81d0ad11', '2021-05-26 15:33:28', '2014-08-17 22:17:31', '2021-06-05 00:59:07', '2021-06-05 00:59:07', '2021-06-05 00:59:07');
INSERT INTO "ContentInfos" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn") VALUES ('cbd9f5f3-1613-4634-ab4a-c2b0b816ced3', 3967853, X'c245690aa1a5ef5306932d7a03d8ac7d', '2021-06-10 01:48:08', '2021-06-10 01:48:08');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1442b2ad-921b-4495-977a-b55c60f66df7', 'Don''t Bring Me Down.wma', 'cbd9f5f3-1613-4634-ab4a-c2b0b816ced3', '107a63e4-368a-43c1-b735-b5ef81d0ad11', '2021-05-26 15:33:28', '2014-08-17 22:22:52', '2021-06-10 01:48:08', '2021-06-05 00:59:08', '2021-06-10 01:48:08');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('e82f41ee-1cfc-4ae3-8d8d-e97707378e6b', 4428007, '2021-06-05 00:59:10', '2021-06-05 00:59:10');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('c48a47ae-1366-4d50-a2fa-4a6f5a17ad05', 'Last Train to London.wma', 'e82f41ee-1cfc-4ae3-8d8d-e97707378e6b', '107a63e4-368a-43c1-b735-b5ef81d0ad11', '2021-05-26 15:33:28', '2014-08-17 22:22:08', '2021-06-05 00:59:10', '2021-06-05 00:59:10', '2021-06-05 00:59:10');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('7f120709-7f07-4943-80e4-ec04f497b892', 4607285, '2021-06-05 00:59:12', '2021-06-05 00:59:12');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('f5aee463-dffa-4d22-bb96-c9738339a8d3', '01 Shine a Little Love.wma', '7f120709-7f07-4943-80e4-ec04f497b892', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:27', '2010-09-03 10:42:26', '2021-06-05 00:59:16', '2021-06-05 00:59:16', '2021-06-05 00:59:16');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('91114580-13c5-42c1-8be4-d5d7094be21e', 3627201, '2021-06-05 00:59:16', '2021-06-05 00:59:16');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ae465db1-0f5f-442d-94aa-3d625b6c520f', '02 Confusion.wma', '91114580-13c5-42c1-8be4-d5d7094be21e', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:27', '2010-09-03 10:42:26', '2021-06-05 00:59:16', '2021-06-05 00:59:16', '2021-06-05 00:59:16');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('1a31dbc2-236d-4dfc-98be-343415134548', 5067425, '2021-06-05 00:59:19', '2021-06-05 00:59:19');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('3374a447-905c-448f-ae75-96b94980d443', '03 Need Her Love.wma', '1a31dbc2-236d-4dfc-98be-343415134548', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:27', '2010-09-03 10:42:26', '2021-06-05 00:59:21', '2021-06-05 00:59:21', '2021-06-05 00:59:21');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('ba0e933b-b0dc-4091-bd11-c0bea75940d9', 4218855, '2021-06-05 00:59:23', '2021-06-05 00:59:23');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('17c192eb-a243-47cd-b8ee-dede2002f9d6', '04 The Diary of Horace Wimp.wma', 'ba0e933b-b0dc-4091-bd11-c0bea75940d9', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:27', '2010-09-03 10:42:26', '2021-06-05 00:59:25', '2021-06-05 00:59:25', '2021-06-05 00:59:25');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('2de8e4d6-7eb8-4589-a93c-67686bd51a51', '05 Last Train to London.wma', 'e82f41ee-1cfc-4ae3-8d8d-e97707378e6b', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:27', '2010-09-03 10:42:28', '2021-06-05 00:59:26', '2021-06-05 00:59:26', '2021-06-05 00:59:26');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('7cb13d4f-49f0-4f5d-be09-bd4da023703e', 4212857, '2021-06-05 00:59:29', '2021-06-05 00:59:29');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('c9d9e47f-f543-4dc3-a3b2-8c941094a627', '06 Midnight Blue.wma', '7cb13d4f-49f0-4f5d-be09-bd4da023703e', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:27', '2010-09-03 10:42:28', '2021-06-05 00:59:29', '2021-06-05 00:59:29', '2021-06-05 00:59:29');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('8dda9ba6-3fad-44cb-bd2d-052802cd654b', 3842339, '2021-06-05 00:59:33', '2021-06-05 00:59:33');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('311771f8-510a-41ef-bfeb-fcce9090523b', '07 On the Run.wma', '8dda9ba6-3fad-44cb-bd2d-052802cd654b', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:28', '2010-09-03 10:42:28', '2021-06-05 00:59:34', '2021-06-05 00:59:34', '2021-06-05 00:59:34');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('7ecda162-749d-4efb-af88-2d48d0541a9e', 4123205, '2021-06-05 00:59:35', '2021-06-05 00:59:35');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('56e88df1-b0df-4804-a5ad-68c3d2d71365', '08 Wishing.wma', '7ecda162-749d-4efb-af88-2d48d0541a9e', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:28', '2010-09-03 10:42:28', '2021-06-05 00:59:38', '2021-06-05 00:59:38', '2021-06-05 00:59:38');
INSERT INTO "ContentInfos" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn") VALUES ('d43fefcc-94be-4595-91a6-9721638bfdff', 3967853, X'8cad6db17bd3a24ae7ed6e697eb611a8', '2021-06-10 01:45:47', '2021-06-10 01:45:47');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('6d29a7ee-40b1-43ab-8d22-faf058a88e70', '09 Don''t Bring Me Down.wma', 'd43fefcc-94be-4595-91a6-9721638bfdff', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:28', '2012-02-01 19:36:44', '2021-06-10 01:45:47', '2021-06-05 00:59:42', '2021-06-10 01:45:47');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('ad0caef3-e824-4fbf-b7e6-6b9b6a2d1161', 1676470, '2021-06-05 00:59:42', '2021-06-05 00:59:42');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('7f1eddaa-817d-4b43-9d27-bd1dbbd80224', '10-On The Run [Home Demo, Bonus Track].mp3', 'ad0caef3-e824-4fbf-b7e6-6b9b6a2d1161', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:28', '2010-12-29 19:49:36', '2021-06-05 00:59:44', '2021-06-05 00:59:44', '2021-06-05 00:59:44');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('7c36cc4c-4792-4e5d-a856-927d93ce8e4d', 1083391, '2021-06-05 00:59:45', '2021-06-05 00:59:45');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('3d95b1f6-3fa5-499a-ae07-c9109831f540', '11-Second Time Around [Home Demo, Bonus Track].mp3', '7c36cc4c-4792-4e5d-a856-927d93ce8e4d', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:28', '2010-12-29 19:48:41', '2021-06-05 00:59:48', '2021-06-05 00:59:48', '2021-06-05 00:59:48');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('085a2f5c-3c1e-41b1-bc97-e394ac4acb26', 4857611, '2021-06-05 00:59:48', '2021-06-05 00:59:48');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('78e1657a-2ae2-4ae2-92a0-fea29fbea49c', '12-Little Town Flirt [Del Shannon cover, Bonus Track].mp3', '085a2f5c-3c1e-41b1-bc97-e394ac4acb26', '957378c1-b275-49e5-b6d0-d2dd3df1eb19', '2021-05-26 15:33:28', '2010-12-29 20:21:04', '2021-06-05 00:59:51', '2021-06-05 00:59:51', '2021-06-05 00:59:51');
INSERT INTO "Subdirectories" ("Id", "Name", "VolumeId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('a1a5880e-393c-4599-9fbf-7e59a0a7235c', 'D:\', 'cdd51583-08a0-4dda-8fa8-ad62b1b2df2c', '1979-12-31 23:00:00', '1979-12-31 23:00:00', '2021-06-05 00:59:53', '2021-06-05 00:59:53', '2021-06-05 00:59:53');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('61f83298-8641-48f0-8b43-aedf0bc945b2', 'Music', 'a1a5880e-393c-4599-9fbf-7e59a0a7235c', '2020-05-18 12:23:07', '2020-05-18 12:23:10', '2021-06-05 00:59:55', '2021-06-05 00:59:55', '2021-06-05 00:59:55');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('128196f8-ebe8-4b21-acab-abd62f3c35d7', 'Mozart', '61f83298-8641-48f0-8b43-aedf0bc945b2', '2020-05-18 12:33:09', '2017-05-13 13:16:00', '2021-06-05 00:59:57', '2021-06-05 00:59:57', '2021-06-05 00:59:57');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('8afa1dbb-3e7c-4f39-bf1f-c766debf27ae', 3095491, '2021-06-05 00:59:59', '2021-06-05 00:59:59');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('c672fde0-36d8-41c4-a7b9-f6af234e220f', '01 Serenade Nº 3  in D major KV 203- Andante maestoso.wma', '8afa1dbb-3e7c-4f39-bf1f-c766debf27ae', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:09', '2010-09-03 10:45:16', '2021-06-05 00:59:59', '2021-06-05 00:59:59', '2021-06-05 00:59:59');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('6aa4afc4-8e28-49df-a629-4ec09606b73f', 6268729, '2021-06-05 01:00:01', '2021-06-05 01:00:01');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1795aeeb-5c35-4fc5-ae06-bf7f2820f5f4', '02 Serenade Nº 3  in D major KV 203- Andante.wma', '6aa4afc4-8e28-49df-a629-4ec09606b73f', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:12', '2010-09-03 10:45:16', '2021-06-05 01:00:03', '2021-06-05 01:00:03', '2021-06-05 01:00:03');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('3d5b15df-72ef-4371-99ed-49582a5edfe4', 5683083, '2021-06-05 01:00:06', '2021-06-05 01:00:06');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1a116523-19f9-4f28-a324-d74586158347', '03 Serenade Nº 3  in D major KV 203- Menuetto.wma', '3d5b15df-72ef-4371-99ed-49582a5edfe4', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:15', '2010-09-03 10:45:16', '2021-06-05 01:00:10', '2021-06-05 01:00:10', '2021-06-05 01:00:10');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('1ac95de2-a9de-45b8-9fcd-72df06b28104', 8694985, '2021-06-05 01:00:10', '2021-06-05 01:00:10');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('490376b8-6537-49bf-938f-5920ee8150d1', '04 Serenade Nº 3  in D major KV 203- Allegro.wma', '1ac95de2-a9de-45b8-9fcd-72df06b28104', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:16', '2010-09-03 10:45:16', '2021-06-05 01:00:13', '2021-06-05 01:00:13', '2021-06-05 01:00:13');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('2fab86e3-5a6e-4c14-9dc3-d7b6ed95f49d', 3675147, '2021-06-05 01:00:14', '2021-06-05 01:00:14');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5b01ed05-1fe5-4c25-8698-b3f99555e1cf', '05 Serenade Nº 3  in D major KV 203- Menuetto.wma', '2fab86e3-5a6e-4c14-9dc3-d7b6ed95f49d', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:18', '2010-09-03 10:45:16', '2021-06-05 01:00:15', '2021-06-05 01:00:15', '2021-06-05 01:00:15');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('b154c87d-2e29-4772-ac28-e47f74141433', 5569537, '2021-06-05 01:00:17', '2021-06-05 01:00:17');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('646386c4-067f-49dc-8b87-ab66c4689812', '06 Serenade Nº 3  in D major KV 203- Andante.wma', 'b154c87d-2e29-4772-ac28-e47f74141433', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:18', '2010-09-03 10:45:16', '2021-06-05 01:00:18', '2021-06-05 01:00:18', '2021-06-05 01:00:18');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('557becb6-bd43-4fb2-b835-f65b96936d8a', 4595451, '2021-06-05 01:00:20', '2021-06-05 01:00:20');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('2c80c272-357c-4eec-a414-c8644a12e28b', '07 Serenade Nº 3  in D major KV 203- Menuetto.wma', '557becb6-bd43-4fb2-b835-f65b96936d8a', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:20', '2010-09-03 10:45:16', '2021-06-05 01:00:22', '2021-06-05 01:00:22', '2021-06-05 01:00:22');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('cfececdc-3b6c-41de-b319-3dbeb39ed91d', 5025729, '2021-06-05 01:00:25', '2021-06-05 01:00:25');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('f4a77f9f-d250-4d88-8c65-8d079c1df045', '08 Serenade Nº 3  in D major KV 203- Prestissimo.wma', 'cfececdc-3b6c-41de-b319-3dbeb39ed91d', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:21', '2010-09-03 10:45:16', '2021-06-05 01:00:26', '2021-06-05 01:00:26', '2021-06-05 01:00:26');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('6e0c5dde-9dcf-4a0d-912e-112bca41b859', 4242921, '2021-06-05 01:00:26', '2021-06-05 01:00:26');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('4a06e5c5-085f-4883-8d77-5c54e402ad21', '09 Serenade Nº 6 in D major KV 239 &quot;Serenata Notturna&quot;- Marcia.wma', '6e0c5dde-9dcf-4a0d-912e-112bca41b859', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:22', '2010-09-03 10:45:18', '2021-06-05 01:00:29', '2021-06-05 01:00:29', '2021-06-05 01:00:29');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('d661f104-17df-46dc-84ff-e137779d1aaf', 3968029, '2021-06-05 01:00:33', '2021-06-05 01:00:33');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('afa59924-8e7b-499e-823f-76dfaf6990cb', '10 Serenade Nº 6 in D major KV 239 &quot;Serenata Notturna&quot;- Menuetto.wma', 'd661f104-17df-46dc-84ff-e137779d1aaf', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:23', '2010-09-03 10:45:18', '2021-06-05 01:00:34', '2021-06-05 01:00:34', '2021-06-05 01:00:34');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('67a6a4fc-c0e0-439f-a66f-a6fd2ec92d0f', 4649311, '2021-06-05 01:00:36', '2021-06-05 01:00:36');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('20953fba-b327-4f25-b848-cdbe3c49c098', '11 Serenade Nº 6 in D major KV 239 &quot;Serenata Notturna&quot;- Rondo- Allegretto.wma', '67a6a4fc-c0e0-439f-a66f-a6fd2ec92d0f', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:24', '2010-09-03 10:45:18', '2021-06-05 01:00:38', '2021-06-05 01:00:38', '2021-06-05 01:00:38');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('eacca8d1-8599-49ae-99e3-ff848e13eb80', 6304655, '2021-06-05 01:00:39', '2021-06-05 01:00:39');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1bf160e9-a5a4-46d0-93f7-cff71d9a70de', '12 Serenade Nº 13 in G major KV 525 &quot;Eine kleine Nachtmusik&quot;- Allegro.wma', 'eacca8d1-8599-49ae-99e3-ff848e13eb80', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:25', '2010-09-03 10:45:18', '2021-06-05 01:00:41', '2021-06-05 01:00:41', '2021-06-05 01:00:41');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('34583b80-1c85-4fe3-be97-479226d0ae3a', 6531743, '2021-06-05 01:00:44', '2021-06-05 01:00:44');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ed3febf6-c360-413a-85ae-03706ad78a21', '13 Serenade Nº 13 in G major KV 525 &quot;Eine kleine Nachtmusik&quot;- Romance.wma', '34583b80-1c85-4fe3-be97-479226d0ae3a', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:27', '2010-09-03 10:45:20', '2021-06-05 01:00:46', '2021-06-05 01:00:46', '2021-06-05 01:00:46');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('462d9407-93e3-40f2-9890-655a6c786fe2', 2246971, '2021-06-05 01:00:48', '2021-06-05 01:00:48');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1f446135-c229-4dba-a4a3-251477a033a6', '14 Serenade Nº 13 in G major KV 525 &quot;Eine kleine Nachtmusik&quot;- Menuetto- Allegro.wma', '462d9407-93e3-40f2-9890-655a6c786fe2', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:30', '2010-09-03 10:45:20', '2021-06-05 01:00:48', '2021-06-05 01:00:48', '2021-06-05 01:00:48');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('d28265ad-8fbd-41af-b196-4a56d10eafd0', 3274821, '2021-06-05 01:00:51', '2021-06-05 01:00:51');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1313af66-7f5a-4956-a3b3-ee091f388076', '15 Serenade Nº 13 in G major KV 525 &quot;Eine kleine Nachtmusik&quot;- Rondo-.wma', 'd28265ad-8fbd-41af-b196-4a56d10eafd0', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:30', '2010-09-03 10:45:20', '2021-06-05 01:00:51', '2021-06-05 01:00:51', '2021-06-05 01:00:51');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('513ba28b-8a45-47b8-a63e-5f4a1eb7a0c2', 4900143, '2021-06-05 01:00:53', '2021-06-05 01:00:53');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('c7fcde80-732c-45f0-be7e-dfec2bdc8c30', '17 allegro assai.wma', '513ba28b-8a45-47b8-a63e-5f4a1eb7a0c2', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:31', '2010-09-03 10:44:52', '2021-06-05 01:00:54', '2021-06-05 01:00:54', '2021-06-05 01:00:54');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('b3d254c7-29c0-4e06-9dac-7ba591f4f6c3', 4553549, '2021-06-05 01:00:57', '2021-06-05 01:00:57');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('03c7f734-1b1b-4e89-9d63-fbc4cca8377c', '18 tema com cariazianoni.wma', 'b3d254c7-29c0-4e06-9dac-7ba591f4f6c3', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:32', '2010-09-03 10:44:52', '2021-06-05 01:00:59', '2021-06-05 01:00:59', '2021-06-05 01:00:59');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('a3adcda9-3f00-49dc-8917-dbb85045ddaa', 4183027, '2021-06-05 01:00:59', '2021-06-05 01:00:59');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('426337d6-5f56-4ad2-9e32-57530024fcf0', '19 allegro di molto.wma', 'a3adcda9-3f00-49dc-8917-dbb85045ddaa', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:33', '2010-09-03 10:44:52', '2021-06-05 01:01:00', '2021-06-05 01:01:00', '2021-06-05 01:01:00');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('b0387a41-e30b-4515-9d24-ffd9df34bd34', 2300615, '2021-06-05 01:01:01', '2021-06-05 01:01:01');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('6fa41a21-196d-42a8-b9ad-ec8ec862a2a4', '20 tempo de menuetto poco andante.wma', 'b0387a41-e30b-4515-9d24-ffd9df34bd34', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:34', '2010-09-03 10:44:52', '2021-06-05 01:01:04', '2021-06-05 01:01:04', '2021-06-05 01:01:04');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('0b33b1f0-8e01-430b-bc4c-ca3a69fa0418', 2396183, '2021-06-05 01:01:06', '2021-06-05 01:01:06');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('176a5917-a969-4466-85ed-476454a9fcdd', '21 presto.wma', '0b33b1f0-8e01-430b-bc4c-ca3a69fa0418', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:35', '2010-09-03 10:44:52', '2021-06-05 01:01:09', '2021-06-05 01:01:09', '2021-06-05 01:01:09');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('a80e4623-f8c2-465d-a88f-a80515f56638', 4344363, '2021-06-05 01:01:09', '2021-06-05 01:01:09');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('7ecf69d8-55da-4153-8960-d0b4e39a5a2b', '22 moderato.wma', 'a80e4623-f8c2-465d-a88f-a80515f56638', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:35', '2010-09-03 10:44:52', '2021-06-05 01:01:10', '2021-06-05 01:01:10', '2021-06-05 01:01:10');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('284d5bb9-4f1a-4088-b45a-9bdbf550ccd7', 2079471, '2021-06-05 01:01:10', '2021-06-05 01:01:10');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5dba97a9-a142-4aa8-86a7-909fee2dc546', '23 un poco adagio.wma', '284d5bb9-4f1a-4088-b45a-9bdbf550ccd7', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:36', '2010-09-03 10:44:52', '2021-06-05 01:01:13', '2021-06-05 01:01:13', '2021-06-05 01:01:13');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('72225c9d-98ca-4377-9f8e-5e0418d9b044', 2282647, '2021-06-05 01:01:15', '2021-06-05 01:01:15');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('a58469c4-cc25-4960-989e-b9009500f5d8', '24 allegretto.wma', '72225c9d-98ca-4377-9f8e-5e0418d9b044', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:37', '2010-09-03 10:44:54', '2021-06-05 01:01:18', '2021-06-05 01:01:18', '2021-06-05 01:01:18');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('08b59300-636d-44ab-9fb1-711bd5bae76c', 11546402, '2021-06-05 01:01:20', '2021-06-05 01:01:20');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ee380c4e-ea47-4968-a6ce-8b679091f686', 'Mozart concerto 20 in d, K.466 - 1. Allegro (1of2) Gulda.mp3', '08b59300-636d-44ab-9fb1-711bd5bae76c', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:37', '2017-04-30 22:30:06', '2021-06-05 01:01:23', '2021-06-05 01:01:23', '2021-06-05 01:01:23');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('ec0972ad-35a4-4776-ac21-c403eb5e972a', 10079151, '2021-06-05 01:01:24', '2021-06-05 01:01:24');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5a4fe455-35dc-47bc-b1be-3ca8894ad24b', 'Mozart concerto 20 in d, K.466 - 1. Allegro (2of2) Gulda.mp3', 'ec0972ad-35a4-4776-ac21-c403eb5e972a', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:41', '2017-04-30 22:37:44', '2021-06-05 01:01:26', '2021-06-05 01:01:26', '2021-06-05 01:01:26');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('7a582ce0-3bfa-44fc-91a0-513d22cdc08a', 13601921, '2021-06-05 01:01:26', '2021-06-05 01:01:26');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('8c3d5f5d-9dfe-4bf4-833f-5677d9f8e3a8', 'Mozart concerto 20 in d, K.466 - 2. Romance Gulda.mp3', '7a582ce0-3bfa-44fc-91a0-513d22cdc08a', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:43', '2017-04-30 04:06:18', '2021-06-05 01:01:29', '2021-06-05 01:01:29', '2021-06-05 01:01:29');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('1b533b50-8fed-4296-9e0a-55b45c3ad7e3', 14210101, '2021-06-05 01:01:32', '2021-06-05 01:01:32');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('7d5f73ed-fd6f-4d6f-80c1-bfde477d77a2', 'Mozart concerto 20 in d, K.466 - 3. Rondo ; Gulda.mp3', '1b533b50-8fed-4296-9e0a-55b45c3ad7e3', '128196f8-ebe8-4b21-acab-abd62f3c35d7', '2020-05-18 12:33:47', '2017-04-30 22:44:40', '2021-06-05 01:01:33', '2021-06-05 01:01:33', '2021-06-05 01:01:33');
INSERT INTO "Subdirectories" ("Id", "Name", "VolumeId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('a0a3ac94-eb7c-46fb-b493-3be94110a081', 'Z:\', 'c48c1c92-154c-43cf-a277-53223d5c1510', '2021-02-01 13:51:40', '2021-02-01 13:51:40', '2021-06-05 01:01:36', '2021-06-05 01:01:36', '2021-06-05 01:01:36');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('a375bac9-14d8-431a-a93e-2e4d01289d8b', 9224036, '2021-06-05 01:01:36', '2021-06-05 01:01:36');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1c097e1c-986c-4f4c-b6c1-18325a0c30df', '08___DIRE_STRAITS___SULTANS.MP3', 'a375bac9-14d8-431a-a93e-2e4d01289d8b', 'a0a3ac94-eb7c-46fb-b493-3be94110a081', '2021-05-28 00:28:41', '2013-12-01 11:33:34', '2021-06-05 01:01:39', '2021-06-05 01:01:39', '2021-06-05 01:01:39');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('6459283e-5b6e-47b6-b2dd-f79a07ec9455', 'barba', '37cd633c-a544-46fb-80c0-8832944a47a0', '2020-08-07 09:50:50', '2021-06-09 21:15:16', '2021-06-05 01:07:26', '2021-06-05 01:07:26', '2021-06-05 01:07:26');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('bc1db013-7b38-421c-80dd-1d90e9b8bcfc', 'iCloudDrive', '6459283e-5b6e-47b6-b2dd-f79a07ec9455', '2020-08-06 16:03:18', '2021-06-09 21:30:02', '2021-06-05 01:07:29', '2021-06-05 01:07:29', '2021-06-05 01:07:29');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('acdf7b49-5e63-4a6b-87db-20fac7984a10', 'Downloads', 'bc1db013-7b38-421c-80dd-1d90e9b8bcfc', '2020-03-06 16:05:12', '2021-02-04 19:53:54', '2021-06-05 01:07:29', '2021-06-05 01:07:29', '2021-06-05 01:07:29');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('e75d961f-336f-4cf0-810c-1f3323603899', 332771, '2021-06-05 01:07:30', '2021-06-05 01:07:30');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('d0ab8bf4-22e2-48bb-bdfa-a009e751f2cb', 'walmart near me - Google Search.pdf', 'e75d961f-336f-4cf0-810c-1f3323603899', 'acdf7b49-5e63-4a6b-87db-20fac7984a10', '2021-02-26 16:35:39', '2021-02-26 16:35:39', '2021-06-05 01:07:31', '2021-06-05 01:07:31', '2021-06-05 01:07:31');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('9fb3a2e0-307a-4a99-8761-34bb3504e28d', 'Orders', 'a1a5880e-393c-4599-9fbf-7e59a0a7235c', '2021-06-10 01:05:00', '2021-06-10 01:05:02', '2021-06-05 01:07:35', '2021-06-05 01:07:35', '2021-06-05 01:07:35');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('db6e844d-85f7-4eb4-9895-e493a56d8895', 107402, '2021-06-05 01:07:39', '2021-06-05 01:07:39');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1fb0228c-df56-4807-90e6-9f9c9821992d', 'FalconsTicketOrder.PNG', 'db6e844d-85f7-4eb4-9895-e493a56d8895', '9fb3a2e0-307a-4a99-8761-34bb3504e28d', '2021-06-10 01:05:40', '2018-11-04 08:23:00', '2021-06-05 01:07:42', '2021-06-05 01:07:42', '2021-06-05 01:07:42');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('5075d9cd-1e02-400d-b961-ee8784ced3ce', 79229, '2021-06-05 01:07:45', '2021-06-05 01:07:45');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('71c35e30-1175-4102-ad6a-d48b59087571', 'FalconsTicketOrder2.PNG', '5075d9cd-1e02-400d-b961-ee8784ced3ce', '9fb3a2e0-307a-4a99-8761-34bb3504e28d', '2021-06-10 01:05:40', '2018-11-04 08:24:34', '2021-06-05 01:07:45', '2021-06-05 01:07:45', '2021-06-05 01:07:45');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('d15b2464-550b-48bf-bb0b-f648bfb4a763', 40430, '2021-06-05 01:07:45', '2021-06-05 01:07:45');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('2ac28181-5d0c-4e65-95b7-5e0ff0cb50c6', 'FalconsTicketOrder3.PNG', 'd15b2464-550b-48bf-bb0b-f648bfb4a763', '9fb3a2e0-307a-4a99-8761-34bb3504e28d', '2021-06-10 01:05:40', '2018-11-04 08:36:50', '2021-06-05 01:07:49', '2021-06-05 01:07:49', '2021-06-05 01:07:49');
INSERT INTO "Subdirectories" ("Id", "Name", "VolumeId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('15c26b84-7303-48a6-8ba1-4abdeb0170fd', 'E:\', '47af1d42-49b2-477f-b7d1-d764922e2830', '2008-06-23 00:02:00', '2008-06-23 00:02:00', '2021-06-05 01:07:52', '2021-06-05 01:07:52', '2021-06-05 01:07:52');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('2f3afd8d-e2cb-44a6-8708-5115e21b59d8', 'Easter2006', '15c26b84-7303-48a6-8ba1-4abdeb0170fd', '2008-06-21 16:30:20', '2008-06-21 16:30:20', '2021-06-05 01:07:53', '2021-06-05 01:07:53', '2021-06-05 01:07:53');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('2e849c9a-ea27-495f-a488-0a77e30dc064', 2077110, '2021-06-05 01:07:56', '2021-06-05 01:07:56');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('77885636-0cc7-4368-a8e5-4d86e9795fee', 'Copy of HPIM0528.JPG', '2e849c9a-ea27-495f-a488-0a77e30dc064', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-08-10 15:54:48', '2006-08-10 15:54:48', '2021-06-05 01:07:58', '2021-06-05 01:07:58', '2021-06-05 01:07:58');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('0a0c5bac-fa90-4e71-a495-478b386b93d8', 1791946, '2021-06-05 01:08:00', '2021-06-05 01:08:00');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('aa3fa8fa-a702-4097-b976-dc7227bbfcc5', 'Copy of HPIM0529.JPG', '0a0c5bac-fa90-4e71-a495-478b386b93d8', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-08-10 15:54:42', '2006-08-10 15:54:42', '2021-06-05 01:08:02', '2021-06-05 01:08:02', '2021-06-05 01:08:02');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('f1f7a737-7574-4e9f-995a-b3e9ef921a46', 2076818, '2021-06-05 01:08:03', '2021-06-05 01:08:03');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('d0ce88d9-996b-400a-b3b9-a2cd0f83bc98', 'Copy of HPIM0530.JPG', 'f1f7a737-7574-4e9f-995a-b3e9ef921a46', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-08-10 15:54:38', '2006-08-10 15:54:38', '2021-06-05 01:08:04', '2021-06-05 01:08:04', '2021-06-05 01:08:04');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('c2bd16b4-181f-488e-ac0d-11146b9dc19e', 1947986, '2021-06-05 01:08:06', '2021-06-05 01:08:06');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('eb0a865a-262b-4a88-a40f-a2c5bc15a50c', 'Copy of HPIM0532.JPG', 'c2bd16b4-181f-488e-ac0d-11146b9dc19e', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-08-10 15:54:32', '2006-08-10 15:54:32', '2021-06-05 01:08:07', '2021-06-05 01:08:07', '2021-06-05 01:08:07');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('f66653f1-9881-4ae9-9488-d85b572e307b', 2059998, '2021-06-05 01:08:08', '2021-06-05 01:08:08');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1a8f9c6f-77b0-472a-a098-ea59f12775dc', 'Copy of HPIM0535.JPG', 'f66653f1-9881-4ae9-9488-d85b572e307b', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-08-10 15:54:26', '2006-08-10 15:54:26', '2021-06-05 01:08:10', '2021-06-05 01:08:10', '2021-06-05 01:08:10');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('2645eec6-0697-4985-8a56-f0da8ece1ba8', 1639174, '2021-06-05 01:08:13', '2021-06-05 01:08:13');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('801f6b09-c11e-46f0-b5f3-c819afa45ef4', 'Copy of HPIM0543.JPG', '2645eec6-0697-4985-8a56-f0da8ece1ba8', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-08-10 15:54:20', '2006-08-10 15:54:20', '2021-06-05 01:08:14', '2021-06-05 01:08:14', '2021-06-05 01:08:14');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('5831217c-c761-43a1-8ba7-af991b8b6003', 860, '2021-06-05 01:08:16', '2021-06-05 01:08:16');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('3376ee2b-ec09-4182-8ca3-cdd7a9c28062', 'DEVICEID.XML', '5831217c-c761-43a1-8ba7-af991b8b6003', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:29:28', '2006-04-16 15:29:28', '2021-06-05 01:08:19', '2021-06-05 01:08:19', '2021-06-05 01:08:19');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('02ad2914-4d00-46de-abce-f086951f53fa', 1697975, '2021-06-05 01:08:20', '2021-06-05 01:08:20');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('81946331-289f-477f-8428-248b76c434da', 'HPIM0509.JPG', '02ad2914-4d00-46de-abce-f086951f53fa', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:40:20', '2006-04-16 09:40:20', '2021-06-05 01:08:21', '2021-06-05 01:08:21', '2021-06-05 01:08:21');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('c8e5e374-ed2d-40da-b7ee-331ad2a16ecd', 1017534, '2021-06-05 01:08:21', '2021-06-05 01:08:21');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('9fc708d3-d15e-4c89-8e58-b1d6f4233fa4', 'HPIM0510.JPG', 'c8e5e374-ed2d-40da-b7ee-331ad2a16ecd', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:40:38', '2006-04-16 09:40:38', '2021-06-05 01:08:23', '2021-06-05 01:08:23', '2021-06-05 01:08:23');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('715dc746-0cd2-43b6-8d83-5aa06a25b1a0', 998751, '2021-06-05 01:08:25', '2021-06-05 01:08:25');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('caa0e712-b537-422a-ba63-c856fb396b41', 'HPIM0511.JPG', '715dc746-0cd2-43b6-8d83-5aa06a25b1a0', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:40:46', '2006-04-16 09:40:46', '2021-06-05 01:08:28', '2021-06-05 01:08:28', '2021-06-05 01:08:28');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('3ab9d281-11f6-4e0e-a74d-e3c178d74d93', 1206506, '2021-06-05 01:08:31', '2021-06-05 01:08:31');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('e147335d-3860-4adc-a8b3-75f0d88f0269', 'HPIM0512.JPG', '3ab9d281-11f6-4e0e-a74d-e3c178d74d93', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:41:02', '2006-04-16 09:41:02', '2021-06-05 01:08:33', '2021-06-05 01:08:33', '2021-06-05 01:08:33');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('04011d24-beaf-4984-b0c2-d3f4ec3e7753', 1312099, '2021-06-05 01:08:34', '2021-06-05 01:08:34');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('91247c96-6663-4b7e-a635-13de0766e2fe', 'HPIM0513.JPG', '04011d24-beaf-4984-b0c2-d3f4ec3e7753', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:41:12', '2006-04-16 09:41:12', '2021-06-05 01:08:36', '2021-06-05 01:08:36', '2021-06-05 01:08:36');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('191f60c4-234a-40d1-95d3-c36aa166dc5c', 1294954, '2021-06-05 01:08:39', '2021-06-05 01:08:39');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('e8ec29d5-3570-402e-b808-b6b1690391e7', 'HPIM0514.JPG', '191f60c4-234a-40d1-95d3-c36aa166dc5c', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:41:26', '2006-04-16 09:41:26', '2021-06-05 01:08:41', '2021-06-05 01:08:41', '2021-06-05 01:08:41');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('47e7fee1-bdb9-4e37-9bc8-935d357c5a36', 1314626, '2021-06-05 01:08:43', '2021-06-05 01:08:43');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5d7fdec5-c246-439c-8e8d-a1a89e36c89d', 'HPIM0515.JPG', '47e7fee1-bdb9-4e37-9bc8-935d357c5a36', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:41:46', '2006-04-16 09:41:46', '2021-06-05 01:08:44', '2021-06-05 01:08:44', '2021-06-05 01:08:44');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('025c8624-9996-4078-b258-e73a81765ee5', 1731066, '2021-06-05 01:08:46', '2021-06-05 01:08:46');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('fb9bb704-b88e-4d18-9637-1665ec437399', 'HPIM0516.JPG', '025c8624-9996-4078-b258-e73a81765ee5', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:53:40', '2006-04-16 09:53:40', '2021-06-05 01:08:48', '2021-06-05 01:08:48', '2021-06-05 01:08:48');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('65494a80-2f76-4443-9026-61788140c6bb', 1293138, '2021-06-05 01:08:50', '2021-06-05 01:08:50');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('935f6465-57e3-4078-91a1-1edf0bead0de', 'HPIM0517.JPG', '65494a80-2f76-4443-9026-61788140c6bb', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:53:48', '2006-04-16 09:53:48', '2021-06-05 01:08:53', '2021-06-05 01:08:53', '2021-06-05 01:08:53');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('eb97142c-96f1-4e24-91d1-b9510bb218fc', 1648427, '2021-06-05 01:08:54', '2021-06-05 01:08:54');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('00dfe75d-91bc-4b4b-a27d-957634e5f310', 'HPIM0518.JPG', 'eb97142c-96f1-4e24-91d1-b9510bb218fc', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:53:58', '2006-04-16 09:53:58', '2021-06-05 01:08:56', '2021-06-05 01:08:56', '2021-06-05 01:08:56');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('bb5a0d6e-550e-47d1-bbba-5c337cde7d9e', 1350406, '2021-06-05 01:08:57', '2021-06-05 01:08:57');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('527b085d-22ac-4c53-b1c0-ed103f9f7923', 'HPIM0519.JPG', 'bb5a0d6e-550e-47d1-bbba-5c337cde7d9e', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:54:30', '2006-04-16 09:54:30', '2021-06-05 01:09:00', '2021-06-05 01:09:00', '2021-06-05 01:09:00');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('2155ce7a-cc24-478b-86fb-d7b090c12ec7', 1435734, '2021-06-05 01:09:01', '2021-06-05 01:09:01');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('671adf48-63eb-4a17-b41b-e405a3700844', 'HPIM0520.JPG', '2155ce7a-cc24-478b-86fb-d7b090c12ec7', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:54:44', '2006-04-16 09:54:44', '2021-06-05 01:09:02', '2021-06-05 01:09:02', '2021-06-05 01:09:02');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('0a5c3a05-8d3c-4584-9ee5-0e6503d5286b', 1405878, '2021-06-05 01:09:04', '2021-06-05 01:09:04');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('0ba6d741-a880-4732-a3e7-f2360e2d170f', 'HPIM0521.JPG', '0a5c3a05-8d3c-4584-9ee5-0e6503d5286b', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:54:54', '2006-04-16 09:54:54', '2021-06-05 01:09:06', '2021-06-05 01:09:06', '2021-06-05 01:09:06');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('2417f3e7-2958-4a49-8791-212dd1fca506', 1480999, '2021-06-05 01:09:09', '2021-06-05 01:09:09');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('b8e2090a-3181-4ef0-b55c-af6c53b80e15', 'HPIM0522.JPG', '2417f3e7-2958-4a49-8791-212dd1fca506', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:55:44', '2006-04-16 09:55:44', '2021-06-05 01:09:11', '2021-06-05 01:09:11', '2021-06-05 01:09:11');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('6731fe75-f3b9-4e29-86b0-bea6302d0f42', 1448879, '2021-06-05 01:09:13', '2021-06-05 01:09:13');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('503fe934-9548-467b-9b62-d9cbd4475a51', 'HPIM0523.JPG', '6731fe75-f3b9-4e29-86b0-bea6302d0f42', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:57:56', '2006-04-16 09:57:56', '2021-06-05 01:09:15', '2021-06-05 01:09:15', '2021-06-05 01:09:15');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('71d16c20-1238-4ba7-8f73-792e0ff061eb', 1626746, '2021-06-05 01:09:17', '2021-06-05 01:09:17');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('f17dc8f4-883c-4caf-8e79-391558f49b8d', 'HPIM0524.JPG', '71d16c20-1238-4ba7-8f73-792e0ff061eb', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:58:50', '2006-04-16 09:58:50', '2021-06-05 01:09:19', '2021-06-05 01:09:19', '2021-06-05 01:09:19');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('4e1ddfc5-2f64-42f9-9862-68d17b77ef92', 1523877, '2021-06-05 01:09:22', '2021-06-05 01:09:22');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('2f84dd01-2111-4d92-b95b-5710278b6d9e', 'HPIM0525.JPG', '4e1ddfc5-2f64-42f9-9862-68d17b77ef92', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:59:30', '2006-04-16 09:59:30', '2021-06-05 01:09:22', '2021-06-05 01:09:22', '2021-06-05 01:09:22');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('c54692ad-65fc-40a9-9072-e756fd20319f', 1527942, '2021-06-05 01:09:24', '2021-06-05 01:09:24');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('0dca4884-236c-4077-922a-7b4f28d937cd', 'HPIM0526.JPG', 'c54692ad-65fc-40a9-9072-e756fd20319f', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 09:59:40', '2006-04-16 09:59:40', '2021-06-05 01:09:27', '2021-06-05 01:09:27', '2021-06-05 01:09:27');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('31c454a1-f381-4560-bb24-0f8d63358374', 2036962, '2021-06-05 01:09:27', '2021-06-05 01:09:27');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('fbbd31b9-aaad-4625-bd9f-41025295e3e9', 'HPIM0527.JPG', '31c454a1-f381-4560-bb24-0f8d63358374', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 10:03:20', '2006-04-16 10:03:20', '2021-06-05 01:09:29', '2021-06-05 01:09:29', '2021-06-05 01:09:29');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('9551a5dc-7708-4f4c-9757-e9fa4e3d6f43', 'HPIM0528.JPG', '2e849c9a-ea27-495f-a488-0a77e30dc064', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 10:03:34', '2006-04-16 10:03:34', '2021-06-05 01:09:31', '2021-06-05 01:09:31', '2021-06-05 01:09:31');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('9f1d2bda-3345-4d55-81f7-a3ce25fe9c17', 'HPIM0529.JPG', '0a0c5bac-fa90-4e71-a495-478b386b93d8', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 10:03:46', '2006-04-16 10:03:46', '2021-06-05 01:09:34', '2021-06-05 01:09:34', '2021-06-05 01:09:34');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('3fec3afb-2608-431f-96d5-d509b2578972', 'HPIM0530.JPG', 'f1f7a737-7574-4e9f-995a-b3e9ef921a46', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:04:26', '2006-04-16 15:04:26', '2021-06-05 01:09:36', '2021-06-05 01:09:36', '2021-06-05 01:09:36');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('79bb865f-3b45-4c4d-93a4-9ed3b73b1b8f', 1961366, '2021-06-05 01:09:37', '2021-06-05 01:09:37');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('0cd77ae0-2a86-495d-b107-46e6ce03c8c9', 'HPIM0531.JPG', '79bb865f-3b45-4c4d-93a4-9ed3b73b1b8f', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:04:36', '2006-04-16 15:04:36', '2021-06-05 01:09:41', '2021-06-05 01:09:41', '2021-06-05 01:09:41');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('b803502e-bf04-4a66-b769-5df014d3e567', 'HPIM0532.JPG', 'c2bd16b4-181f-488e-ac0d-11146b9dc19e', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:05:08', '2006-04-16 15:05:08', '2021-06-05 01:09:42', '2021-06-05 01:09:42', '2021-06-05 01:09:42');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('dd48bf05-3533-4bfe-bb50-ba0323158380', 1788853, '2021-06-05 01:09:43', '2021-06-05 01:09:43');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('87db0b67-972b-4c0b-a7a3-0828959edf37', 'HPIM0533.JPG', 'dd48bf05-3533-4bfe-bb50-ba0323158380', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:05:22', '2006-04-16 15:05:22', '2021-06-05 01:09:46', '2021-06-05 01:09:46', '2021-06-05 01:09:46');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('69dd7203-a744-4f2a-8cc6-4ef5e0896da6', 2000334, '2021-06-05 01:09:49', '2021-06-05 01:09:49');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('faa6f7b9-8211-44e6-8e54-99861420091f', 'HPIM0534.JPG', '69dd7203-a744-4f2a-8cc6-4ef5e0896da6', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:05:34', '2006-04-16 15:05:34', '2021-06-05 01:09:50', '2021-06-05 01:09:50', '2021-06-05 01:09:50');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('8aa64564-fbca-47e4-a46a-0d5c35e0a7e3', 'HPIM0535.JPG', 'f66653f1-9881-4ae9-9488-d85b572e307b', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:05:48', '2006-04-16 15:05:48', '2021-06-05 01:09:52', '2021-06-05 01:09:52', '2021-06-05 01:09:52');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('b89a837c-47a9-481c-bb6d-3918322f40a3', 1891907, '2021-06-05 01:09:52', '2021-06-05 01:09:52');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('f118e4e3-9f0d-48e5-8a0b-6cd3389c998c', 'HPIM0536.JPG', 'b89a837c-47a9-481c-bb6d-3918322f40a3', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:06:02', '2006-04-16 15:06:02', '2021-06-05 01:09:55', '2021-06-05 01:09:55', '2021-06-05 01:09:55');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('644925ce-dffb-458c-91fe-df662aa782bc', 2049418, '2021-06-05 01:09:59', '2021-06-05 01:09:59');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('dece12a6-2561-4bd7-8c3a-b797967c931d', 'HPIM0537.JPG', '644925ce-dffb-458c-91fe-df662aa782bc', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:06:16', '2006-04-16 15:06:16', '2021-06-05 01:10:02', '2021-06-05 01:10:02', '2021-06-05 01:10:02');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('f696fa55-3255-4965-83eb-1f7bf3aaf31a', 2007253, '2021-06-05 01:10:03', '2021-06-05 01:10:03');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('7f0baf2a-e492-47ea-b3c4-457fa0161d13', 'HPIM0538.JPG', 'f696fa55-3255-4965-83eb-1f7bf3aaf31a', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:06:30', '2006-04-16 15:06:30', '2021-06-05 01:10:04', '2021-06-05 01:10:04', '2021-06-05 01:10:04');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('d6d50718-cb6f-4ccd-8c4e-6c35c9ae3056', 1703002, '2021-06-05 01:10:07', '2021-06-05 01:10:07');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('e44d249c-aa8a-4b2b-8238-40e818f967f1', 'HPIM0539.JPG', 'd6d50718-cb6f-4ccd-8c4e-6c35c9ae3056', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:06:42', '2006-04-16 15:06:42', '2021-06-05 01:10:09', '2021-06-05 01:10:09', '2021-06-05 01:10:09');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('0ac4c738-f245-4c31-b500-d952bcf50f85', 1911007, '2021-06-05 01:10:13', '2021-06-05 01:10:13');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('636293fc-80db-4364-b519-6dab966036df', 'HPIM0540.JPG', '0ac4c738-f245-4c31-b500-d952bcf50f85', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:06:56', '2006-04-16 15:06:56', '2021-06-05 01:10:16', '2021-06-05 01:10:16', '2021-06-05 01:10:16');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('b2288ec4-ca6a-4bc1-bc29-6a134e0ea8d2', 1610546, '2021-06-05 01:10:19', '2021-06-05 01:10:19');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ec5468b0-60f5-4332-84ac-78416bb52ee2', 'HPIM0541.JPG', 'b2288ec4-ca6a-4bc1-bc29-6a134e0ea8d2', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:07:10', '2006-04-16 15:07:10', '2021-06-05 01:10:20', '2021-06-05 01:10:20', '2021-06-05 01:10:20');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('58ed3586-e4d7-4c8b-8505-8db6a02c7f2a', 1657659, '2021-06-05 01:10:21', '2021-06-05 01:10:21');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('fc9c5013-de5d-4c56-9779-427455ab9b7d', 'HPIM0542.JPG', '58ed3586-e4d7-4c8b-8505-8db6a02c7f2a', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:07:24', '2006-04-16 15:07:24', '2021-06-05 01:10:24', '2021-06-05 01:10:24', '2021-06-05 01:10:24');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('08893f7f-583e-46c0-8783-470d1db389c5', 'HPIM0543.JPG', '2645eec6-0697-4985-8a56-f0da8ece1ba8', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:07:38', '2006-04-16 15:07:38', '2021-06-05 01:10:27', '2021-06-05 01:10:27', '2021-06-05 01:10:27');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('3c5df9d7-662b-4f2f-9c2b-2cbf429ab1c4', 1762618, '2021-06-05 01:10:30', '2021-06-05 01:10:30');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('6bc0e37e-3f07-4cb2-92d5-6dac2ecdd267', 'HPIM0544.JPG', '3c5df9d7-662b-4f2f-9c2b-2cbf429ab1c4', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:07:52', '2006-04-16 15:07:52', '2021-06-05 01:10:31', '2021-06-05 01:10:31', '2021-06-05 01:10:31');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('94c1e939-4ea5-41ab-bda5-c1c330788f29', 1955345, '2021-06-05 01:10:34', '2021-06-05 01:10:34');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('0faefd64-0a2b-423d-ae21-089d5a11ba0d', 'HPIM0545.JPG', '94c1e939-4ea5-41ab-bda5-c1c330788f29', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:08:04', '2006-04-16 15:08:04', '2021-06-05 01:10:36', '2021-06-05 01:10:36', '2021-06-05 01:10:36');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('9f0081d9-5625-47d2-9dd1-4df3924a1092', 1888834, '2021-06-05 01:10:38', '2021-06-05 01:10:38');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('f8ba6fa1-13c9-43b9-b5e5-5499571476a5', 'HPIM0546.JPG', '9f0081d9-5625-47d2-9dd1-4df3924a1092', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:08:16', '2006-04-16 15:08:16', '2021-06-05 01:10:42', '2021-06-05 01:10:42', '2021-06-05 01:10:42');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('98a42190-9ce5-4631-9880-b9ffd77a9890', 1875470, '2021-06-05 01:10:43', '2021-06-05 01:10:43');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('3d76a07e-ddd0-4e05-89b7-5e58382f22c0', 'HPIM0547.JPG', '98a42190-9ce5-4631-9880-b9ffd77a9890', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:08:28', '2006-04-16 15:08:28', '2021-06-05 01:10:44', '2021-06-05 01:10:44', '2021-06-05 01:10:44');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('2df1097f-19d5-4cbe-a72e-0b1bac598053', 1921698, '2021-06-05 01:10:45', '2021-06-05 01:10:45');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5fbf4a8f-7280-48da-bcb0-7ed0efbd813f', 'HPIM0548.JPG', '2df1097f-19d5-4cbe-a72e-0b1bac598053', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:08:42', '2006-04-16 15:08:42', '2021-06-05 01:10:46', '2021-06-05 01:10:46', '2021-06-05 01:10:46');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('2fb2b82b-1bac-4c62-9c8b-57c9afb45548', 27, '2021-06-05 01:10:49', '2021-06-05 01:10:49');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('3ae8ff74-99ec-4dac-af52-e60932cc2187', 'SHAREOLD.XML', '2fb2b82b-1bac-4c62-9c8b-57c9afb45548', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2006-04-16 15:29:28', '2006-04-16 15:29:28', '2021-06-05 01:10:49', '2021-06-05 01:10:49', '2021-06-05 01:10:49');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('c50032a1-683d-4c17-ba60-ace90f88ace6', 8704, '2021-06-05 01:10:52', '2021-06-05 01:10:52');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('70b087d9-6902-4d91-8acd-57f734e8e01b', 'Thumbs.db', 'c50032a1-683d-4c17-ba60-ace90f88ace6', '2f3afd8d-e2cb-44a6-8708-5115e21b59d8', '2008-06-22 08:18:19', '2008-06-22 08:18:19', '2021-06-05 01:10:53', '2021-06-05 01:10:53', '2021-06-05 01:10:53');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('55e7b1b0-3b88-432e-b752-b7e7687e5bbc', 'Pictures', '941fcb97-9ea7-4c95-be4f-4b1b21ff1ead', '2015-12-23 19:44:15', '2021-04-07 16:07:32', '2021-06-05 01:09:53', '2021-06-05 01:09:53', '2021-06-05 01:09:53');
INSERT INTO "Subdirectories" ("Id", "Name", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('357d97a3-1866-4ab3-b589-3bf8df9ed5b1', 'Saved pictures', '55e7b1b0-3b88-432e-b752-b7e7687e5bbc', '2015-12-23 19:44:15', '2017-09-22 11:25:42', '2021-06-05 01:09:56', '2021-06-05 01:09:56', '2021-06-05 01:09:56');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('b198a638-71ba-44c1-a31e-7b9b0b9994ef', 12296, '2021-06-05 01:09:58', '2021-06-05 01:09:58');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('91c56582-9ca4-4ad8-8e83-d2a7d1cdd693', 'C__Data_Users_DefApps_AppData_INTERNETEXPLORER_Temp_Saved Images_images 1.jpg', 'b198a638-71ba-44c1-a31e-7b9b0b9994ef', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-06-20 15:46:01', '2017-06-19 22:58:35', '2021-06-05 01:09:59', '2021-06-05 01:09:59', '2021-06-05 01:09:59');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('8263dd47-8acc-4707-9503-645c9c1ccfbc', 4945, '2021-06-05 01:10:01', '2021-06-05 01:10:01');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('4ec25028-ff9c-475e-b775-00f03283a1f4', 'C__Data_Users_DefApps_AppData_INTERNETEXPLORER_Temp_Saved Images_images(1).jpg', '8263dd47-8acc-4707-9503-645c9c1ccfbc', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-06-01 15:38:43', '2017-05-31 23:05:52', '2021-06-05 01:10:04', '2021-06-05 01:10:04', '2021-06-05 01:10:04');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('c9203ba9-b506-4d68-a4f7-d29ac9139ff5', 3593, '2021-06-05 01:10:08', '2021-06-05 01:10:08');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('449dedc8-864e-4e67-b4f0-3c26449b0a88', 'C__Data_Users_DefApps_AppData_INTERNETEXPLORER_Temp_Saved Images_images(2).jpg', 'c9203ba9-b506-4d68-a4f7-d29ac9139ff5', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-06-01 18:57:13', '2017-06-01 18:57:06', '2021-06-05 01:10:09', '2021-06-05 01:10:09', '2021-06-05 01:10:09');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('f530fba8-ae0b-467c-a76b-df03860c71ac', 5917, '2021-06-05 01:10:12', '2021-06-05 01:10:12');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('e97b702f-9607-4d63-93af-2c8bc68f277a', 'C__Data_Users_DefApps_AppData_INTERNETEXPLORER_Temp_Saved Images_images(3).jpg', 'f530fba8-ae0b-467c-a76b-df03860c71ac', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-06-20 15:45:59', '2017-06-19 22:58:37', '2021-06-05 01:10:13', '2021-06-05 01:10:13', '2021-06-05 01:10:13');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('58271a25-3127-4c2a-bf5b-d70cd51c46b2', 6590, '2021-06-05 01:10:15', '2021-06-05 01:10:15');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1b661af3-1fae-4d09-973c-134f453bb624', 'C__Data_Users_DefApps_AppData_INTERNETEXPLORER_Temp_Saved Images_images.jpg', '58271a25-3127-4c2a-bf5b-d70cd51c46b2', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-06-01 15:38:43', '2017-05-31 23:05:49', '2021-06-05 01:10:15', '2021-06-05 01:10:15', '2021-06-05 01:10:15');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('93e7dfa5-bc4e-4c17-b3c2-76b3e48e015c', 190, '2021-06-05 01:10:18', '2021-06-05 01:10:18');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5c75f83e-526d-47bf-bf76-8c94fb44da8c', 'desktop.ini', '93e7dfa5-bc4e-4c17-b3c2-76b3e48e015c', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-24 10:23:18', '2015-12-24 10:23:18', '2021-06-05 01:10:19', '2021-06-05 01:10:19', '2021-06-05 01:10:19');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('d6255ce3-f072-4bb2-a32a-6ec6ef9052f2', 420026, '2021-06-05 01:10:19', '2021-06-05 01:10:19');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('7815148d-f643-45f8-ad3a-25d66ce9fd71', 'FOT18C9.JPG', 'd6255ce3-f072-4bb2-a32a-6ec6ef9052f2', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-06-26 15:29:42', '2017-06-25 19:48:13', '2021-06-05 01:10:20', '2021-06-05 01:10:20', '2021-06-05 01:10:20');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('879ffcaf-ded8-4d4f-b8d5-d8690a600ac3', 558568, '2021-06-05 01:10:23', '2021-06-05 01:10:23');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('0b630bc6-cc8c-438a-adf1-b7b448f1bb8c', 'FOT3695.JPG', '879ffcaf-ded8-4d4f-b8d5-d8690a600ac3', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-06-26 15:29:39', '2017-06-25 19:42:14', '2021-06-05 01:10:24', '2021-06-05 01:10:24', '2021-06-05 01:10:24');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('4da36157-b42b-41b3-b7b1-445a55c2d3cc', 187832, '2021-06-05 01:10:27', '2021-06-05 01:10:27');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('7c7c1b88-d709-4c11-985a-43a3826ab2d4', 'V__1BE8 1.jpg', '4da36157-b42b-41b3-b7b1-445a55c2d3cc', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:32', '2016-03-20 09:48:09', '2021-06-05 01:10:30', '2021-06-05 01:10:30', '2021-06-05 01:10:30');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('f84b475a-e495-4a67-af65-80db9fb41981', 115870, '2021-06-05 01:10:31', '2021-06-05 01:10:31');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('51aaec66-b949-476c-842e-13eeebe70596', 'V__1BE8.jpg', 'f84b475a-e495-4a67-af65-80db9fb41981', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:03', '2015-07-10 20:15:38', '2021-06-05 01:10:33', '2021-06-05 01:10:33', '2021-06-05 01:10:33');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('161ffabc-631a-4dae-9a83-75059f040bdf', 51854, '2021-06-05 01:10:35', '2021-06-05 01:10:35');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('d5f8dbf6-d584-4d96-ae50-ffbe82d2d10b', 'V__2BE5.png', '161ffabc-631a-4dae-9a83-75059f040bdf', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-05-30 14:47:27', '2017-05-29 21:50:48', '2021-06-05 01:10:37', '2021-06-05 01:10:37', '2021-06-05 01:10:37');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('500b901c-7aae-48fe-bd41-56f227513511', 177977, '2021-06-05 01:10:37', '2021-06-05 01:10:37');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ec21ab07-59fc-4107-acbc-adaacb37d7b1', 'V__3239(1).jpg', '500b901c-7aae-48fe-bd41-56f227513511', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:03', '2015-06-24 17:11:11', '2021-06-05 01:10:40', '2021-06-05 01:10:40', '2021-06-05 01:10:40');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('80209b6d-120c-4fae-95c3-2a414fd8c51e', 'V__3239.jpg', '500b901c-7aae-48fe-bd41-56f227513511', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:02', '2015-06-24 17:11:10', '2021-06-05 01:10:41', '2021-06-05 01:10:41', '2021-06-05 01:10:41');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('fa260af2-5092-496b-9c8c-9ab66057dfdf', 193838, '2021-06-05 01:10:42', '2021-06-05 01:10:42');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1b0a9efc-3e97-4d19-899b-90b2ea826221', 'V__34B2.jpg', 'fa260af2-5092-496b-9c8c-9ab66057dfdf', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-09-22 11:25:20', '2017-09-19 23:05:52', '2021-06-05 01:10:44', '2021-06-05 01:10:44', '2021-06-05 01:10:44');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('48299d7b-a5d9-47ca-9cdd-591b4f4e1410', 108729, '2021-06-05 01:10:47', '2021-06-05 01:10:47');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('b4d3bc02-ad81-4987-80f0-ccc303327468', 'V__39F5.jpg', '48299d7b-a5d9-47ca-9cdd-591b4f4e1410', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:04', '2015-07-10 20:15:57', '2021-06-05 01:10:50', '2021-06-05 01:10:50', '2021-06-05 01:10:50');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('4da2cadb-4212-4018-a1c2-2d16494732f1', 409182, '2021-06-05 01:10:51', '2021-06-05 01:10:51');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('0926b914-d050-4870-abcb-77bad996764c', 'V__4F2B 1.JPG', '4da2cadb-4212-4018-a1c2-2d16494732f1', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:07', '2015-12-23 10:49:09', '2021-06-05 01:10:54', '2021-06-05 01:10:54', '2021-06-05 01:10:54');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('375dabc1-f692-4556-bc57-73afd9afe13f', 408207, '2021-06-05 01:10:56', '2021-06-05 01:10:56');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('30591f3c-8468-4f73-8d40-5e96c07e61c6', 'V__4F2B(1).JPG', '375dabc1-f692-4556-bc57-73afd9afe13f', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:43', '2015-12-23 10:49:15', '2021-06-05 01:10:58', '2021-06-05 01:10:58', '2021-06-05 01:10:58');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('b2d44e98-7f9c-4a3b-af80-d44ea7d41355', 'V__4F2B.JPG', '4da2cadb-4212-4018-a1c2-2d16494732f1', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:07', '2015-12-23 10:49:06', '2021-06-05 01:11:00', '2021-06-05 01:11:00', '2021-06-05 01:11:00');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('1d5d976a-4ba4-448e-aeb0-992d43ff4c51', 295217, '2021-06-05 01:11:03', '2021-06-05 01:11:03');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('fc8c945b-79f6-482a-b3a3-713f11ec0e55', 'V__5171 1.jpg', '1d5d976a-4ba4-448e-aeb0-992d43ff4c51', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:35', '2016-03-20 09:48:39', '2021-06-05 01:11:04', '2021-06-05 01:11:04', '2021-06-05 01:11:04');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('67b77d04-3e65-42a3-ba1e-729f1628dc82', 'V__5171.jpg', '1d5d976a-4ba4-448e-aeb0-992d43ff4c51', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:30', '2016-03-20 09:47:43', '2021-06-05 01:11:07', '2021-06-05 01:11:07', '2021-06-05 01:11:07');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('f0b53029-c2e0-4159-bb3f-4298045e1085', 527884, '2021-06-05 01:11:10', '2021-06-05 01:11:10');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('10b46b42-625a-4116-b9bd-736051fd995f', 'V__5516(1).jpg', 'f0b53029-c2e0-4159-bb3f-4298045e1085', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:19', '2016-03-20 09:48:29', '2021-06-05 01:11:11', '2021-06-05 01:11:11', '2021-06-05 01:11:11');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('8a475739-18d9-4b1c-906f-789cd0f96ff7', 'V__5516(2).jpg', 'f0b53029-c2e0-4159-bb3f-4298045e1085', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:19', '2016-03-20 09:48:36', '2021-06-05 01:11:14', '2021-06-05 01:11:14', '2021-06-05 01:11:14');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('b245bf9e-ff15-4e3f-814a-37c1b8b8a352', 139943, '2021-06-05 01:11:17', '2021-06-05 01:11:17');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('9b7265f5-8ba1-4048-b429-62ff37e4db51', 'V__562E.jpg', 'b245bf9e-ff15-4e3f-814a-37c1b8b8a352', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:03', '2015-07-10 20:15:48', '2021-06-05 01:11:20', '2021-06-05 01:11:20', '2021-06-05 01:11:20');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('0abcf6f2-5597-42c0-aca3-4a8968d25bd5', 301996, '2021-06-05 01:11:22', '2021-06-05 01:11:22');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('e39490cb-2b96-4d7f-9f40-13c1b00f81e1', 'V__5775.jpg', '0abcf6f2-5597-42c0-aca3-4a8968d25bd5', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:06', '2015-10-20 14:41:23', '2021-06-05 01:11:22', '2021-06-05 01:11:22', '2021-06-05 01:11:22');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('1c85f1ab-e916-48d9-aeec-58959f6f42aa', 150634, '2021-06-05 01:11:25', '2021-06-05 01:11:25');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('f3fbdb6a-31d4-4606-bcce-7743f171507c', 'V__5AEB.jpg', '1c85f1ab-e916-48d9-aeec-58959f6f42aa', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:48', '2015-05-05 12:53:11', '2021-06-05 01:11:28', '2021-06-05 01:11:28', '2021-06-05 01:11:28');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('1333e507-4c60-4eaf-816f-739148664b97', 705425, '2021-06-05 01:11:31', '2021-06-05 01:11:31');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('bb2ed27e-6946-404e-9c76-968f2c7e0a6b', 'V__66BA 1.JPG', '1333e507-4c60-4eaf-816f-739148664b97', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:35', '2016-03-20 09:48:49', '2021-06-05 01:11:32', '2021-06-05 01:11:32', '2021-06-05 01:11:32');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('768ce59d-ed4d-472e-8542-08e5aada5654', 569756, '2021-06-05 01:11:32', '2021-06-05 01:11:32');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('76634112-b1a6-4cad-b6b6-49ce85334ead', 'V__66BA.JPG', '768ce59d-ed4d-472e-8542-08e5aada5654', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-02-16 12:57:18', '2016-02-15 16:52:15', '2021-06-05 01:11:36', '2021-06-05 01:11:36', '2021-06-05 01:11:36');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('0f111e03-bc50-43f1-85ce-f740acc87018', 191464, '2021-06-05 01:11:39', '2021-06-05 01:11:39');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('bb468301-51c7-4850-ab0d-1098c8864c8d', 'V__6E2B(1).jpg', '0f111e03-bc50-43f1-85ce-f740acc87018', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:02', '2015-06-11 16:59:58', '2021-06-05 01:11:40', '2021-06-05 01:11:40', '2021-06-05 01:11:40');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('b92b3f32-7368-494d-946f-00fa75f53ce5', 'V__6E2B(2).jpg', '0f111e03-bc50-43f1-85ce-f740acc87018', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:27', '2015-06-11 17:00:04', '2021-06-05 01:11:43', '2021-06-05 01:11:43', '2021-06-05 01:11:43');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('c8193df6-51ab-4f95-95a0-4b1707a8ef28', 'V__6E2B(3).jpg', '0f111e03-bc50-43f1-85ce-f740acc87018', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:02', '2015-06-11 17:00:08', '2021-06-05 01:11:44', '2021-06-05 01:11:44', '2021-06-05 01:11:44');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('bac8b997-0ed4-4306-ae78-27afb5ccc7ee', 'V__6E2B.jpg', '0f111e03-bc50-43f1-85ce-f740acc87018', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:02', '2015-06-11 16:59:53', '2021-06-05 01:11:45', '2021-06-05 01:11:45', '2021-06-05 01:11:45');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('38349182-b2bc-41e5-9d54-8671809338ac', 746152, '2021-06-05 01:11:49', '2021-06-05 01:11:49');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('25413b03-11af-4d9b-8e73-cb6ce5a95c0e', 'V__703F 1.png', '38349182-b2bc-41e5-9d54-8671809338ac', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:20', '2016-03-20 09:47:53', '2021-06-05 01:11:49', '2021-06-05 01:11:49', '2021-06-05 01:11:49');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('eaf10a8b-fcc7-486e-862f-3a3c46b71c9f', 588071, '2021-06-05 01:11:53', '2021-06-05 01:11:53');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1e05108a-c177-4c1a-907e-8fae46669d9a', 'V__703F.png', 'eaf10a8b-fcc7-486e-862f-3a3c46b71c9f', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:06', '2015-12-03 07:01:54', '2021-06-05 01:11:55', '2021-06-05 01:11:55', '2021-06-05 01:11:55');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('8d136d52-0fca-4f28-97a7-718a40116170', 64673, '2021-06-05 01:11:57', '2021-06-05 01:11:57');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('c417669c-07a5-401e-ac1c-b3b48641e856', 'V__8225(1).jpg', '8d136d52-0fca-4f28-97a7-718a40116170', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:02', '2015-05-19 20:37:09', '2021-06-05 01:12:00', '2021-06-05 01:12:00', '2021-06-05 01:12:00');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ec318c15-7fb6-4e20-be49-aa687e4b815e', 'V__8225.jpg', '8d136d52-0fca-4f28-97a7-718a40116170', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:02', '2015-05-19 20:37:07', '2021-06-05 01:12:03', '2021-06-05 01:12:03', '2021-06-05 01:12:03');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('0d497ff1-9749-42ec-ba2e-9afe8bc17f09', 304654, '2021-06-05 01:12:06', '2021-06-05 01:12:06');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('05c09791-85b9-4d34-b5ed-3ea6cedb677a', 'V__8560.jpg', '0d497ff1-9749-42ec-ba2e-9afe8bc17f09', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:04', '2015-08-19 16:55:31', '2021-06-05 01:12:07', '2021-06-05 01:12:07', '2021-06-05 01:12:07');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('dcf410db-04b9-42ba-986d-ed51b9e733b1', 254377, '2021-06-05 01:12:07', '2021-06-05 01:12:07');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('70cd2493-72ae-4b9c-8f88-6e55b34dc127', 'V__8C0E.jpg', 'dcf410db-04b9-42ba-986d-ed51b9e733b1', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:29', '2016-03-20 09:47:36', '2021-06-05 01:12:08', '2021-06-05 01:12:08', '2021-06-05 01:12:08');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('0cc16f20-5b46-4757-950d-c5354f3b1c11', 'V__9699.jpg', '0d497ff1-9749-42ec-ba2e-9afe8bc17f09', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:05', '2015-08-19 16:55:34', '2021-06-05 01:12:10', '2021-06-05 01:12:10', '2021-06-05 01:12:10');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('9d3e09c7-b9f9-417d-a877-8b8f034205da', 225807, '2021-06-05 01:12:12', '2021-06-05 01:12:12');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('c9250cb3-f7ba-45c1-8801-04be2c01be26', 'V__971F.jpg', '9d3e09c7-b9f9-417d-a877-8b8f034205da', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:34', '2016-03-20 09:48:16', '2021-06-05 01:12:14', '2021-06-05 01:12:14', '2021-06-05 01:12:14');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('6db1f7ac-3e04-4f8c-83b8-f2816b2cb7ba', 450484, '2021-06-05 01:12:16', '2021-06-05 01:12:16');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('d196589d-49fc-4cc1-8e4e-ad8c7c4ab735', 'V__9908.jpg', '6db1f7ac-3e04-4f8c-83b8-f2816b2cb7ba', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2017-09-18 10:30:14', '2017-09-17 22:37:46', '2021-06-05 01:12:17', '2021-06-05 01:12:17', '2021-06-05 01:12:17');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('025dcfc1-b8ad-488b-9455-ff51bc2a0878', 295995, '2021-06-05 01:12:21', '2021-06-05 01:12:21');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('dbdab187-1477-4f3f-a6a7-265837c52280', 'V__9DC1.jpg', '025dcfc1-b8ad-488b-9455-ff51bc2a0878', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:19', '2015-07-12 16:18:17', '2021-06-05 01:12:23', '2021-06-05 01:12:23', '2021-06-05 01:12:23');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('a07210b8-0d92-4bce-b40f-14ccd03a5d9e', 209437, '2021-06-05 01:12:25', '2021-06-05 01:12:25');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('35d221b0-ecf8-4bed-8887-0718459a2de2', 'V__9DCD.jpg', 'a07210b8-0d92-4bce-b40f-14ccd03a5d9e', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:05', '2015-09-09 14:18:25', '2021-06-05 01:12:27', '2021-06-05 01:12:27', '2021-06-05 01:12:27');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('afdb76fc-ddc2-45bc-84b7-13931ba6f740', 'V__A454.jpg', 'a07210b8-0d92-4bce-b40f-14ccd03a5d9e', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:05', '2015-09-09 14:18:28', '2021-06-05 01:12:30', '2021-06-05 01:12:30', '2021-06-05 01:12:30');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('e359177e-8c12-49bb-a36b-7d456a347ec9', 19648, '2021-06-05 01:12:34', '2021-06-05 01:12:34');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('61ae6ff2-59ed-4387-b327-d36c9ce59491', 'V__AD22.jpg', 'e359177e-8c12-49bb-a36b-7d456a347ec9', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:06', '2015-10-29 17:14:39', '2021-06-05 01:12:37', '2021-06-05 01:12:37', '2021-06-05 01:12:37');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('95eff807-5c9b-49e7-af01-df9e129a0d9d', 146614, '2021-06-05 01:12:40', '2021-06-05 01:12:40');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('508e1fcf-9a03-45b6-82c6-dc67c7ab906d', 'V__B4FC 1.jpg', '95eff807-5c9b-49e7-af01-df9e129a0d9d', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:34', '2016-03-20 09:48:19', '2021-06-05 01:12:42', '2021-06-05 01:12:42', '2021-06-05 01:12:42');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('34b2a519-6a8c-4fa8-9e1b-07144003bc92', 86761, '2021-06-05 01:12:43', '2021-06-05 01:12:43');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('04ab7291-844e-4400-b6a3-098099ff98da', 'V__B4FC.jpg', '34b2a519-6a8c-4fa8-9e1b-07144003bc92', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:24', '2015-08-25 20:40:28', '2021-06-05 01:12:44', '2021-06-05 01:12:44', '2021-06-05 01:12:44');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('8d7d88cc-569e-4d7d-91fa-9992d7475c0a', 17088, '2021-06-05 01:12:47', '2021-06-05 01:12:47');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('44a811ff-29a5-43e2-9c1f-0be3d56b0666', 'V__C206.jpg', '8d7d88cc-569e-4d7d-91fa-9992d7475c0a', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:22', '2015-10-29 17:14:29', '2021-06-05 01:12:51', '2021-06-05 01:12:51', '2021-06-05 01:12:51');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('13a0abe3-0216-4fe7-9c5a-b48a4735230b', 625503, '2021-06-05 01:12:52', '2021-06-05 01:12:52');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('cbc41f1b-c12f-474a-8e84-ad807a739a6f', 'V__C8FF.jpg', '13a0abe3-0216-4fe7-9c5a-b48a4735230b', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:31', '2016-03-20 09:48:04', '2021-06-05 01:12:54', '2021-06-05 01:12:54', '2021-06-05 01:12:54');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('34eab756-6c53-4a1b-8e73-847e4aff1778', 424341, '2021-06-05 01:12:56', '2021-06-05 01:12:56');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('bda5f813-fc21-41a4-88b1-54c66f676708', 'V__CAE4 1.jpg', '34eab756-6c53-4a1b-8e73-847e4aff1778', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-04-15 15:51:05', '2016-04-14 07:15:54', '2021-06-05 01:12:57', '2021-06-05 01:12:57', '2021-06-05 01:12:57');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('39fa71f5-30a1-4362-97cc-705d38937e4e', 414033, '2021-06-05 01:12:58', '2021-06-05 01:12:58');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('9f19357f-a030-47e1-9b01-79167a042f02', 'V__CAE4.jpg', '39fa71f5-30a1-4362-97cc-705d38937e4e', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-04-15 15:51:05', '2016-04-14 07:15:47', '2021-06-05 01:13:01', '2021-06-05 01:13:01', '2021-06-05 01:13:01');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('7fe2b7e4-5e44-4fc1-9253-c9b9876ce884', 438437, '2021-06-05 01:13:02', '2021-06-05 01:13:02');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('b5114776-ace9-49e6-a3ce-ce6d45c169d4', 'V__CCB5.JPG', '7fe2b7e4-5e44-4fc1-9253-c9b9876ce884', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:04', '2015-08-06 21:46:06', '2021-06-05 01:13:05', '2021-06-05 01:13:05', '2021-06-05 01:13:05');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('4bc9bfee-fca3-4bdc-99b7-7294765d61e7', 'V__CFF0.jpg', 'a07210b8-0d92-4bce-b40f-14ccd03a5d9e', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:06', '2015-09-09 15:56:26', '2021-06-05 01:13:06', '2021-06-05 01:13:06', '2021-06-05 01:13:06');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('05d1f9d3-009d-4d9e-879b-e586ff1c3c62', 500480, '2021-06-05 01:13:08', '2021-06-05 01:13:08');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('2b879a15-7458-49e4-88fc-1fce180828e9', 'V__D3CE.JPG', '05d1f9d3-009d-4d9e-879b-e586ff1c3c62', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:04', '2015-08-18 11:34:47', '2021-06-05 01:13:11', '2021-06-05 01:13:11', '2021-06-05 01:13:11');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ac08ff25-2144-460e-b9b7-2c1fa580d99f', 'V__D3DA.jpg', '0d497ff1-9749-42ec-ba2e-9afe8bc17f09', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:23', '2015-08-19 16:55:36', '2021-06-05 01:13:12', '2021-06-05 01:13:12', '2021-06-05 01:13:12');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('953a06da-14e9-4ad3-a715-cdbd36ba7551', 174432, '2021-06-05 01:13:12', '2021-06-05 01:13:12');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('e2dea7df-7549-4d49-ac36-5e77b0157ac2', 'V__D6DA.jpg', '953a06da-14e9-4ad3-a715-cdbd36ba7551', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:32', '2016-03-20 09:48:06', '2021-06-05 01:13:13', '2021-06-05 01:13:13', '2021-06-05 01:13:13');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('ed77add8-9763-4892-8e80-93bc493a2029', 124344, '2021-06-05 01:13:13', '2021-06-05 01:13:13');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('a8709231-6b0a-4080-85a6-1db0d9cdb491', 'V__D8ED.jpg', 'ed77add8-9763-4892-8e80-93bc493a2029', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:04', '2015-08-13 23:42:51', '2021-06-05 01:13:15', '2021-06-05 01:13:15', '2021-06-05 01:13:15');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('3a9f5741-e94f-4124-ba6b-5ffff1b64e08', 185035, '2021-06-05 01:13:15', '2021-06-05 01:13:15');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('0fe028db-6a44-4554-9bf2-609d4e5a10b9', 'V__E260.jpg', '3a9f5741-e94f-4124-ba6b-5ffff1b64e08', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:03', '2015-06-28 17:07:58', '2021-06-05 01:13:17', '2021-06-05 01:13:17', '2021-06-05 01:13:17');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('e5641725-0a4d-4702-b0d2-29cecad70798', 545472, '2021-06-05 01:13:20', '2021-06-05 01:13:20');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ccabf6bc-b14e-498e-9b7c-dad7885d7c4f', 'V__E30B(1).JPG', 'e5641725-0a4d-4702-b0d2-29cecad70798', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:05', '2015-08-28 23:12:04', '2021-06-05 01:13:22', '2021-06-05 01:13:22', '2021-06-05 01:13:22');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('e29309a9-f47c-4f15-84ed-d49374193122', 'V__E30B(2).JPG', 'e5641725-0a4d-4702-b0d2-29cecad70798', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:05', '2015-08-28 23:13:08', '2021-06-05 01:13:23', '2021-06-05 01:13:23', '2021-06-05 01:13:23');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('849af002-ec92-4f6c-8209-ecf35c4f38ea', 'V__E30B.JPG', 'e5641725-0a4d-4702-b0d2-29cecad70798', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:46', '2015-08-28 23:11:53', '2021-06-05 01:13:27', '2021-06-05 01:13:27', '2021-06-05 01:13:27');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('bfb17738-b401-452a-8e17-60b99bb3ba2b', 170823, '2021-06-05 01:13:30', '2021-06-05 01:13:30');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('61f67a69-31a0-40d9-a359-6cf691ffb500', 'V__E60F.jpg', 'bfb17738-b401-452a-8e17-60b99bb3ba2b', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:01', '2015-05-05 12:52:50', '2021-06-05 01:13:31', '2021-06-05 01:13:31', '2021-06-05 01:13:31');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('9627eab7-b81f-4b8a-b5d9-0fef22e102ba', 191373, '2021-06-05 01:13:34', '2021-06-05 01:13:34');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5f004308-593c-403b-97c4-24e5b8d87d5c', 'V__E9DC 1.jpg', '9627eab7-b81f-4b8a-b5d9-0fef22e102ba', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-03-28 14:11:31', '2016-03-20 09:47:57', '2021-06-05 01:13:34', '2021-06-05 01:13:34', '2021-06-05 01:13:34');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('a53583d3-1716-4b3c-8e3c-245b3db0cb38', 115361, '2021-06-05 01:13:34', '2021-06-05 01:13:34');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('f93757fe-f0fa-44d2-9c8e-4fcb452c88cf', 'V__E9DC.jpg', 'a53583d3-1716-4b3c-8e3c-245b3db0cb38', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:06', '2015-11-16 20:03:46', '2021-06-05 01:13:36', '2021-06-05 01:13:36', '2021-06-05 01:13:36');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('0d031821-de52-45a4-8b72-cfc0259cfc0b', 443291, '2021-06-05 01:13:39', '2021-06-05 01:13:39');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('a9b57967-c886-45ae-8ff3-ab804b2911a4', 'V__EFA7.JPG', '0d031821-de52-45a4-8b72-cfc0259cfc0b', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:50', '2015-08-06 21:46:17', '2021-06-05 01:13:40', '2021-06-05 01:13:40', '2021-06-05 01:13:40');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('aaa4f9a2-795a-4c67-9dfe-e44fb755b9d6', 459873, '2021-06-05 01:13:41', '2021-06-05 01:13:41');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('8ae8f760-4ab2-4d22-86d5-f66a272bd793', 'V__F447.jpg', 'aaa4f9a2-795a-4c67-9dfe-e44fb755b9d6', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2016-02-20 09:48:26', '2016-02-19 19:24:10', '2021-06-05 01:13:43', '2021-06-05 01:13:43', '2021-06-05 01:13:43');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('d3000f39-0340-46b5-a88f-392679644eeb', 112045, '2021-06-05 01:13:46', '2021-06-05 01:13:46');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('9c2df69c-61d1-46a7-8f2a-8bcd61b61a42', 'V__F53F(1).jpg', 'd3000f39-0340-46b5-a88f-392679644eeb', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:45:04', '2015-07-10 20:20:48', '2021-06-05 01:13:49', '2021-06-05 01:13:49', '2021-06-05 01:13:49');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('91e12d1c-9d42-4849-8121-b592d165dace', 'V__F53F.jpg', 'd3000f39-0340-46b5-a88f-392679644eeb', '357d97a3-1866-4ab3-b589-3bf8df9ed5b1', '2015-12-23 19:44:25', '2015-07-10 20:20:44', '2021-06-05 01:13:52', '2021-06-05 01:13:52', '2021-06-05 01:13:52');
INSERT INTO "Subdirectories" ("Id", "Name", "VolumeId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('6334fc35-67a4-44e4-b278-6f9aa7c55a8d', 'E:\', '355b32f0-d9c8-4a81-b894-24109fbbda64', '1600-12-31 19:00:00', '1600-12-31 19:00:00', '2021-06-05 01:13:54', '2021-06-05 01:13:54', '2021-06-05 01:13:54');
INSERT INTO "ContentInfos" ("Id", "Length", "CreatedOn", "ModifiedOn")
	VALUES ('53c0bb72-8819-439f-b3a8-0fe6523c1c09', 44, '2021-06-05 01:13:56', '2021-06-05 01:13:56');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('3d3213a9-91a2-4895-ad43-68ed53300030', 'Track01.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:13:58', '2021-06-05 01:13:58', '2021-06-05 01:13:58');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('53fe5460-cd4b-48b3-8549-584288c1cdd9', 'Track02.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:13:59', '2021-06-05 01:13:59', '2021-06-05 01:13:59');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('d4c1fe58-c133-43cb-87ff-afa29a51fbab', 'Track03.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:01', '2021-06-05 01:14:01', '2021-06-05 01:14:01');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('60fcbf18-bedf-4941-832a-0c89db2d00c5', 'Track04.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:02', '2021-06-05 01:14:02', '2021-06-05 01:14:02');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('65244f9e-96bc-44d9-8944-05230ca4a976', 'Track05.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:04', '2021-06-05 01:14:04', '2021-06-05 01:14:04');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('332114b8-2b1e-4f3b-aaef-051b6aa12a93', 'Track06.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:07', '2021-06-05 01:14:07', '2021-06-05 01:14:07');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('b7e36207-5c9f-4793-9c02-7128e942d245', 'Track07.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:10', '2021-06-05 01:14:10', '2021-06-05 01:14:10');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5c3b6b20-4879-444e-9f91-164c5720d27f', 'Track08.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:12', '2021-06-05 01:14:12', '2021-06-05 01:14:12');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('b065017e-365f-4f4a-a268-867d5ca7647a', 'Track09.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:14', '2021-06-05 01:14:14', '2021-06-05 01:14:14');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('4a208354-c163-450c-943f-080739ef6323', 'Track10.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:17', '2021-06-05 01:14:17', '2021-06-05 01:14:17');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ec1e5e8d-0bad-4b23-a923-ed6c492169a5', 'Track11.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:20', '2021-06-05 01:14:20', '2021-06-05 01:14:20');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('619fa488-31e0-42a4-b754-b3bc436024d3', 'Track12.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:22', '2021-06-05 01:14:22', '2021-06-05 01:14:22');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('ef83b99e-7200-4289-b61a-a83162b7700e', 'Track13.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:24', '2021-06-05 01:14:24', '2021-06-05 01:14:24');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('dece9d31-8561-4eab-b515-c20539c3a99d', 'Track14.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:26', '2021-06-05 01:14:26', '2021-06-05 01:14:26');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('c1759cb8-bcce-4311-b598-1e23e8fa8a1b', 'Track15.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:29', '2021-06-05 01:14:29', '2021-06-05 01:14:29');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('72e7b58e-fac9-4689-8189-94193cb611ee', 'Track16.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:31', '2021-06-05 01:14:31', '2021-06-05 01:14:31');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('1ca4b65c-ba20-43f4-ba48-3fcd83562976', 'Track17.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:32', '2021-06-05 01:14:32', '2021-06-05 01:14:32');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('6f92b9cc-3f0d-4c82-a204-de7870232130', 'Track18.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:33', '2021-06-05 01:14:33', '2021-06-05 01:14:33');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('05a46960-5131-4717-be55-4a21839bc5ca', 'Track19.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:35', '2021-06-05 01:14:35', '2021-06-05 01:14:35');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('2aa5a556-6adb-4ea3-bea5-380fd77e5b44', 'Track20.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:38', '2021-06-05 01:14:38', '2021-06-05 01:14:38');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5f17a58f-a5aa-47d6-80d2-2806f389d85d', 'Track21.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:39', '2021-06-05 01:14:39', '2021-06-05 01:14:39');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('a059fa60-e4d5-4d81-8bf9-868b7d66a14d', 'Track22.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:41', '2021-06-05 01:14:41', '2021-06-05 01:14:41');
INSERT INTO "Files" ("Id", "Name", "ContentId", "ParentId", "CreationTime", "LastWriteTime", "LastAccessed", "CreatedOn", "ModifiedOn")
	VALUES ('5eade208-6c44-4312-8a1e-b6a59a417af4', 'Track23.cda', '53c0bb72-8819-439f-b3a8-0fe6523c1c09', '6334fc35-67a4-44e4-b278-6f9aa7c55a8d', '1994-12-31 19:00:00', '1994-12-31 19:00:00', '2021-06-05 01:14:44', '2021-06-05 01:14:44', '2021-06-05 01:14:44');
