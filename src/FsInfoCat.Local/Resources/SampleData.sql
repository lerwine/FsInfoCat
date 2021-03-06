DROP TABLE "Comparisons";
DROP TABLE "Redundancies";
DROP TABLE "Files";
DROP TABLE "RedundantSets";
DROP TABLE "BinaryProperties";
DROP TABLE "ExtendedProperties";
PRAGMA foreign_keys = OFF;
DROP TABLE "Subdirectories";
PRAGMA foreign_keys = ON;
DROP TABLE "Volumes";
DROP TABLE "SymbolicNames";
DROP TABLE "FileSystems";

CREATE TABLE "FileSystems" (
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
CREATE TABLE "SymbolicNames" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Name"	NVARCHAR(256) NOT NULL CHECK(length(trim(Name)) = length(Name) AND length(Name)>0) UNIQUE COLLATE NOCASE,
    "Priority" INT NOT NULL DEFAULT 0,
	"Notes"	TEXT NOT NULL DEFAULT '',
	"IsInactive"	BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"FileSystemId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_SymbolicNameFileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT,
	CONSTRAINT "PK_SymbolicNames" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE "Volumes" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "DisplayName" NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName)) = length(DisplayName) AND length(DisplayName)>0) COLLATE NOCASE,
    "VolumeName" NVARCHAR(128) NOT NULL CHECK(length(trim(VolumeName)) = length(VolumeName)) COLLATE NOCASE,
    "Identifier" NVARCHAR(1024) NOT NULL CHECK(length(trim(Identifier)) = length(Identifier) AND length(Identifier)>0) UNIQUE COLLATE NOCASE,
    "CaseSensitiveSearch" BIT DEFAULT NULL,
    "ReadOnly" BIT DEFAULT NULL,
    "MaxNameLength" INT CHECK(MaxNameLength IS NULL OR MaxNameLength>=1) DEFAULT NULL,
    "Type" TINYINT NOT NULL CHECK(Type>=0 AND Type<7) DEFAULT 0,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Status" TINYINT NOT NULL CHECK(Type>=0 AND Type<6) DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"FileSystemId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_VolumeFileSystem" REFERENCES "FileSystems"("Id") ON DELETE RESTRICT,
	CONSTRAINT "PK_Volumes" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE "Subdirectories" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Name" NVARCHAR(1024) NOT NULL COLLATE NOCASE,
    "Options" TINYINT  NOT NULL CHECK(Options>=0 AND Options<64) DEFAULT 0,
    "LastAccessed" DATETIME  NOT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Deleted" BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ParentId"	UNIQUEIDENTIFIER CONSTRAINT "FK_SubdirectoryParent" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
	"VolumeId"	UNIQUEIDENTIFIER CONSTRAINT "FK_SubdirectoryVolume" REFERENCES "Volumes"("Id") ON DELETE RESTRICT,
	CONSTRAINT "PK_Subdirectories" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND
        ((ParentId IS NULL AND VolumeId IS NOT NULL) OR
        (ParentId IS NOT NULL AND VolumeId IS NULL AND length(trim(Name))>0)))
);
--ALTER TABLE "Volumes" ADD COLUMN "RootDirectoryId" UNIQUEIDENTIFIER CONSTRAINT "FK_VolumeSubDirectory" REFERENCES "Subdirectories"("Id");
CREATE TABLE "Files" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
    "Name" NVARCHAR(1024) NOT NULL CHECK(length(trim(Name))>0) COLLATE NOCASE,
    "Options" TINYINT  NOT NULL CHECK(Options>=0 AND Options<15) DEFAULT 0,
    "LastAccessed" DATETIME  NOT NULL,
    "LastHashCalculation" DATETIME DEFAULT NULL,
    "Notes" TEXT NOT NULL DEFAULT '',
    "Deleted" BIT NOT NULL DEFAULT 0,
    "ExtendedPropertyId" UNIQUEIDENTIFIER DEFAULT NULL CONSTRAINT "FK_FileExtendedProperty" REFERENCES "ExtendedProperties"("Id") ON DELETE RESTRICT,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"BinaryPropertiesId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FileBinaryProperties" REFERENCES "BinaryProperties"("Id") ON DELETE RESTRICT,
	"ParentId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_FileSubdirectory" REFERENCES "Subdirectories"("Id") ON DELETE RESTRICT,
	CONSTRAINT "PK_Files" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE "ExtendedProperties" (
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
CREATE TABLE "BinaryProperties" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Length"	BIGINT NOT NULL CHECK(Length>=0),
	"Hash"	BINARY(16) CHECK(Hash IS NULL OR length(HASH)=16) DEFAULT NULL,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_BinaryProperties" PRIMARY KEY("Id"),
	CONSTRAINT "UK_LengthHash" UNIQUE("Length","Hash"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE "RedundantSets" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"BinaryPropertiesId"	UNIQUEIDENTIFIER NOT NULL CONSTRAINT "FK_RedundantSetBinaryProperties" REFERENCES "BinaryProperties"("Id") ON DELETE RESTRICT,
	"RemediationStatus"	TINYINT NOT NULL DEFAULT 1 CHECK(RemediationStatus>=0 AND RemediationStatus<9),
    "Reference" NVARCHAR(128) NOT NULL DEFAULT '' COLLATE NOCASE,
    "Notes" TEXT NOT NULL DEFAULT '',
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_RedundantSets" PRIMARY KEY("Id"),
    CHECK(CreatedOn<=ModifiedOn AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
);
CREATE TABLE "Redundancies" (
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
CREATE TABLE "Comparisons" (
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
CREATE TRIGGER validate_new_redundancy 
   BEFORE INSERT
   ON Redundancies
   WHEN (SELECT COUNT(f.Id) FROM Files f LEFT JOIN RedundantSets r ON f.BinaryPropertiesId=r.BinaryPropertiesId WHERE f.Id=NEW.FileId AND r.Id=NEW.RedundantSetId)=0
BEGIN
    SELECT RAISE (ABORT,'File does not have same content info as the redundancy set.');
END;
INSERT INTO "FileSystems" ("Id", "DisplayName", "MaxNameLength", "DefaultDriveType", "CreatedOn", "ModifiedOn")
    VALUES ('{bedb396b-2212-4149-9cad-7e437c47314c}', 'New Technology File System)', 255, 3, '2004-08-19 14:51:06', '2004-08-19 14:51:06');
INSERT INTO "FileSystems" ("Id", "DisplayName", "CaseSensitiveSearch", "MaxNameLength", "CreatedOn", "ModifiedOn")
    VALUES ('{02070ea8-a2ba-4240-9596-bb6d355dd366}', 'Ext4 Journaling Filesystem', 1, 255, '2021-05-21 21:12:21', '2021-05-21 21:12:21');
INSERT INTO "FileSystems" ("Id", "DisplayName", "MaxNameLength", "CreatedOn", "ModifiedOn")
    VALUES ('{53a9e9a4-f5f0-4b4c-9f1e-4e3a80a93cfd}', 'VFAT (Virtual File Allocation Table)', 255, '2021-05-21 21:15:54', '2021-05-21 21:15:54');
INSERT INTO "FileSystems" ("Id", "DisplayName", "CreatedOn", "ModifiedOn")
    VALUES ('{bd64e811-2c25-4385-8b99-1494bbb24612}', 'Common Internet Filesystem', '2021-05-21 21:25:23', '2021-05-21 21:25:23');
INSERT INTO "FileSystems" ("Id", "DisplayName", "ReadOnly", "CreatedOn", "ModifiedOn")
    VALUES ('{88a3cdb9-ed66-4778-a33b-437675a5ae38}', 'ISO 9660 optical disc media', 1, '2021-05-21 21:27:27', '2021-05-21 21:27:27');
INSERT INTO "FileSystems" ("Id", "DisplayName", "ReadOnly", "CreatedOn", "ModifiedOn")
    VALUES ('{0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd}', 'MAFS (Multi-volume Archive File System)', 0, '2021-05-21 21:27:27', '2021-05-21 21:27:27');

INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "CreatedOn", "ModifiedOn")
    VALUES ('{74381ccb-d56d-444d-890f-3a8051bc18e6}', 'NTFS', '{bedb396b-2212-4149-9cad-7e437c47314c}', '2021-05-21 21:29:59', '2021-05-21 21:29:59');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "CreatedOn", "ModifiedOn")
    VALUES ('{e41dfef2-d6f1-4e8a-b81f-971a85b9be9b}', 'ext4', '{02070ea8-a2ba-4240-9596-bb6d355dd366}', '2021-05-21 21:30:01', '2021-05-21 21:30:01');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "CreatedOn", "ModifiedOn")
    VALUES ('{bbb162e7-9477-49e3-acce-aee45d58bc34}', 'vfat', '{53a9e9a4-f5f0-4b4c-9f1e-4e3a80a93cfd}', '2021-05-21 21:30:09', '2021-05-21 21:30:09');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "CreatedOn", "ModifiedOn")
    VALUES ('{0f54c5a9-5e48-48a4-8056-b01f68d682a6}', 'cifs', '{bd64e811-2c25-4385-8b99-1494bbb24612}', '2021-05-21 21:36:19', '2021-05-21 21:36:19');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "CreatedOn", "ModifiedOn")
    VALUES ('{0989eb7a-d9db-4cef-9ac9-981fe11876b0}', 'iso9660', '{88a3cdb9-ed66-4778-a33b-437675a5ae38}', '2021-05-21 21:36:23', '2021-05-21 21:36:23');
INSERT INTO "SymbolicNames" ("Id", "Name", "FileSystemId", "CreatedOn", "ModifiedOn")
    VALUES ('{e9717552-4286-4eeb-bea5-6a5267a2f223}', 'MAFS', '{0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd}', '2021-05-21 21:36:25', '2021-05-21 21:36:25');

INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "Type", "FileSystemId", "CreatedOn", "ModifiedOn")
    VALUES ('{fb962360-518b-40f6-b6ae-afb67f2e2543}', 'C:', 'OS', 'urn:volume:id:9E49-7DE8', 3, '{bedb396b-2212-4149-9cad-7e437c47314c}', '2021-05-21 21:37:16', '2021-05-21 21:37:16');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "Type", "FileSystemId", "CreatedOn", "ModifiedOn")
    VALUES ('{2340260f-6984-4e32-98f5-66dac5100507}', 'testazureshare (on servicenowdiag479.file.core.windows.net)', '', 'file://servicenowdiag479.file.core.windows.net/testazureshare', 4, '{0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd}', '2021-05-21 21:37:18', '2021-05-21 21:37:18');
    
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('{a951b559-a895-4669-9bfb-ea547c4c24d4}', '//servicenowdiag479.file.core.windows.net/testazureshare/', '2021-05-21 21:37:18', '{2340260f-6984-4e32-98f5-66dac5100507}', '2021-05-21 21:37:18', '2021-05-21 21:37:18');

INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('{baaa89ec-d047-4f82-b8b8-be8f00ea80f4}', 'C:/', '2021-05-21 21:44:29', '{fb962360-518b-40f6-b6ae-afb67f2e2543}', '2021-05-21 21:44:29', '2021-05-21 21:44:29');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{3c2684e7-e242-4a68-a06a-26b1cbde333d}', 'Users', '2021-05-21 21:44:38', '{baaa89ec-d047-4f82-b8b8-be8f00ea80f4}', '2021-05-21 21:44:38', '2021-05-21 21:44:38');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{6cb0fe90-f1e4-44c5-8571-d285079b3840}', 'lerwi', '2021-05-21 21:45:02', '{3c2684e7-e242-4a68-a06a-26b1cbde333d}', '2021-05-21 21:45:02', '2021-05-21 21:45:02');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{b9e6fcc4-0ae5-47e2-beb6-c5813b4970fc}', 'Git', '2021-05-21 21:45:17', '{6cb0fe90-f1e4-44c5-8571-d285079b3840}', '2021-05-21 21:45:17', '2021-05-21 21:45:17');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{7a90f629-9ad0-44b3-96d7-c60f51db8f7e}', 'FsInfoCat', '2021-05-21 21:45:27', '{b9e6fcc4-0ae5-47e2-beb6-c5813b4970fc}', '2021-05-21 21:45:27', '2021-05-21 21:45:27');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{7f4e1340-2066-424b-a499-978f82ee0e50}', 'FsInfoCat', '2021-05-21 21:45:35', '{7a90f629-9ad0-44b3-96d7-c60f51db8f7e}', '2021-05-21 21:45:35', '2021-05-21 21:45:35');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{04863253-dc35-48ba-9662-c0c02556ae84}', 'FsInfoCat.Local', '2021-05-21 21:46:53', '{7f4e1340-2066-424b-a499-978f82ee0e50}', '2021-05-21 21:46:53', '2021-05-21 21:46:53');
    
INSERT INTO "BinaryProperties" ("Id", "Length", "CreatedOn", "ModifiedOn")
    VALUES ('{6696e337-c4ad-4e03-b954-ee585270958d}', 77824, '2021-05-21 21:49:59', '2021-05-21 21:49:59');
INSERT INTO "BinaryProperties" ("Id", "Length", "CreatedOn", "ModifiedOn")
    VALUES ('{dc508120-8617-4d61-ba38-480ac35fcfe5}', 0, '2021-05-21 21:49:59', '2021-05-21 21:49:59');

INSERT INTO "Files" ("Id", "Name", "LastAccessed", "BinaryPropertiesId", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{04863253-dc35-48ba-9662-c0c02556ae84}', 'Example.db', '2021-05-21 21:52:08', '{6696e337-c4ad-4e03-b954-ee585270958d}', '{04863253-dc35-48ba-9662-c0c02556ae84}',
    '2021-05-21 21:52:08', '2021-05-21 21:52:08');

INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{e91871f5-76af-46b0-9381-9e411acd2fba}', 'AppData', '2021-05-21 21:45:17', '{6cb0fe90-f1e4-44c5-8571-d285079b3840}', '2021-05-21 21:45:17', '2021-05-21 21:45:17');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{5d2f0713-1857-4253-a69a-eb5e245ed553}', 'Local','2021-05-21 21:45:27', '{e91871f5-76af-46b0-9381-9e411acd2fba}', '2021-05-21 21:45:27', '2021-05-21 21:45:27');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{1b1ba34d-a703-4e65-a7bf-b37a13818c61}', 'FsInfoCat.Desktop', '2021-05-21 21:45:35', '{5d2f0713-1857-4253-a69a-eb5e245ed553}', '2021-05-21 21:45:35', '2021-05-21 21:45:35');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{9659ea19-ca72-419f-9b35-3b59e6cc89e0}', '1.0.0.0', '2021-05-21 21:46:53', '{1b1ba34d-a703-4e65-a7bf-b37a13818c61}', '2021-05-21 21:46:53', '2021-05-21 21:46:53');
    
INSERT INTO "Files" ("Id", "Name", "LastAccessed", "BinaryPropertiesId", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{6219ddc4-5979-491e-8b76-5050b391f4d3}', 'Test.db', '2021-05-21 21:52:08', '{6696e337-c4ad-4e03-b954-ee585270958d}', '{9659ea19-ca72-419f-9b35-3b59e6cc89e0}',
    '2021-05-21 21:52:08', '2021-05-21 21:52:08');

INSERT INTO "Files" ("Id", "Name", "LastAccessed", "BinaryPropertiesId", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('{a3e8dab2-98fd-4059-8756-27db5fb80932}', 'Test.sdl', '2021-05-21 21:52:08', '{6696e337-c4ad-4e03-b954-ee585270958d}', '{9659ea19-ca72-419f-9b35-3b59e6cc89e0}',
    '2021-05-21 21:52:08', '2021-05-21 21:52:08');

INSERT INTO "RedundantSets" ("Id", "BinaryPropertiesId", "CreatedOn", "ModifiedOn")
	VALUES ('{1ba4dd35-7a45-400b-8f20-57546c94afef}', '{6696e337-c4ad-4e03-b954-ee585270958d}', '2021-05-21 22:23:32', '2021-05-21 22:23:32');
    
INSERT INTO "Redundancies" ("FileId", "RedundantSetId", "CreatedOn", "ModifiedOn")
	VALUES ('{04863253-dc35-48ba-9662-c0c02556ae84}', '{1ba4dd35-7a45-400b-8f20-57546c94afef}', '2021-05-21 22:23:32', '2021-05-21 22:23:32');
    
INSERT INTO "Redundancies" ("FileId", "RedundantSetId", "CreatedOn", "ModifiedOn")
	VALUES ('{6219ddc4-5979-491e-8b76-5050b391f4d3}', '{1ba4dd35-7a45-400b-8f20-57546c94afef}', '2021-05-21 22:23:32', '2021-05-21 22:23:32');

INSERT INTO "Redundancies" ("FileId", "RedundantSetId", "CreatedOn", "ModifiedOn")
	VALUES ('{a3e8dab2-98fd-4059-8756-27db5fb80932}', '{1ba4dd35-7a45-400b-8f20-57546c94afef}', '2021-05-21 22:23:32', '2021-05-21 22:23:32');
