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
	VALUES ('7920cf04-9e7f-4414-986e-d1bfba011db7', 'G:', 'OS', 'urn:volume:id:9E49-7DE8', 'bedb396b-2212-4149-9cad-7e437c47314c', 255, 3, 3, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId", "MaxNameLength", "Type", "Status", "CreatedOn", "ModifiedOn")
	VALUES ('cdd51583-08a0-4dda-8fa8-ad62b1b2df2c', 'D:', 'HP_TOOLS', 'urn:volume:id:3B51-8D4B', '17f0c19f-5f9e-4699-bf4c-cafd1de5ec54', 255, 2, 3, '2021-06-05 00:58:34', '2021-06-05 00:58:34');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId", "MaxNameLength", "Type", "Status", "CreatedOn", "ModifiedOn")
	VALUES ('47af1d42-49b2-477f-b7d1-d764922e2830', 'E:', 'My Disc', 'urn:volume:id:FD91-BC0C', '88a3cdb9-ed66-4778-a33b-437675a5ae38', 110, 5, 3, '2021-06-05 01:07:19', '2021-06-05 01:07:19');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId", "MaxNameLength", "Type", "Status", "CreatedOn", "ModifiedOn")
	VALUES ('355b32f0-d9c8-4a81-b894-24109fbbda64', 'E:', 'Audio CD', 'urn:volume:id:032B-0EBE', '88a3cdb9-ed66-4778-a33b-437675a5ae38', 221, 5, 3, '2021-06-05 01:09:46', '2021-06-05 01:09:46');
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "FileSystemId", "MaxNameLength", "Type", "Status", "CreatedOn", "ModifiedOn")
	VALUES ('c48c1c92-154c-43cf-a277-53223d5c1510', 'testazureshare (on servicenowdiag479.file.core.windows.net)', '', 'file://servicenowdiag479.file.core.windows.net/testazureshare',
        '0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd', 255, 4, 2, '2021-06-05 00:58:35', '2021-09-24 14:15:09');
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('20a61d4b-80c2-48a3-8df6-522e598aae08', 'G:', '2021-06-05 00:58:34', '2019-03-19 00:37:21', '2021-06-04 13:47:20', NULL, '7920cf04-9e7f-4414-986e-d1bfba011db7', '2021-06-05 00:58:34',
        '2021-06-05 00:58:34');
-- G:\Users
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('b63144ce-91cb-4cb8-a407-8d3490a8c90c', 'Users', '2021-06-05 00:58:34', '2019-12-07 04:03:44', '2021-03-09 23:51:36', '20a61d4b-80c2-48a3-8df6-522e598aae08', NULL, '2021-06-05 00:58:34',
        '2021-06-05 00:58:34');
-- G:\Users\lerwi
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('38a40fde-acf0-4cc5-9302-d37ec2cbb631', 'lerwi', '2021-06-05 00:58:34', '2021-03-09 23:51:36', '2021-06-04 03:09:25', 'b63144ce-91cb-4cb8-a407-8d3490a8c90c', NULL, '2021-06-05 00:58:34',
        '2021-06-05 00:58:34');
-- G:\Users\lerwi\Videos
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a', 'Videos', '2021-06-05 00:58:34', '2019-11-23 20:30:23', '2021-05-16 18:54:50', '38a40fde-acf0-4cc5-9302-d37ec2cbb631', NULL, '2021-06-05 00:58:34',
        '2021-06-05 00:58:34');
-- G:\Users\lerwi\Videos
INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "RootId", "StatusValue", "LastCrawlStart", "LastCrawlEnd", "CreatedOn", "ModifiedOn")
    VALUES ('9c91ba89-6ab5-4d4e-9798-0d926b405f41', 'Lenny''s Laptop Videos', '3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a', 2, '2021-08-01 14:54:22', '2021-08-01 14:57:16', '2021-07-31 15:28:18', '2021-08-01 14:57:16');
-- G:\Users\lerwi\Videos
INSERT INTO "CrawlJobLogs" ("Id", "MaxRecursionDepth", "RootPath", "StatusCode", "CrawlStart", "CrawlEnd", "StatusMessage", "StatusDetail", "FoldersProcessed", "FilesProcessed", "CreatedOn", "ModifiedOn",
        "ConfigurationId")
    VALUES ('563160b2-cb6e-4e3b-855c-89eebefdf8bd', 256, 'G:\Users\lerwi\Downloads', 2, '2021-08-01 14:54:22', '2021-08-01 14:57:16', 'Directory was empty.', '', 0, 0, '2021-08-01 14:57:16', '2021-08-01 14:57:16',
        '9c91ba89-6ab5-4d4e-9798-0d926b405f41');
-- G:\Users\lerwi\Videos\the move down on the bay - YouTube.webm
INSERT INTO "BinaryPropertySets" ("Id", "Length", "CreatedOn", "ModifiedOn")
    VALUES('82d46e21-5eba-4f1b-8c99-78cb94689316', 25057982, '2021-08-22 14:32:22', '2021-08-22 14:32:2s');
-- G:\Users\lerwi\Videos\the move down on the bay - YouTube.webm
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "CreatedOn", "ParentId", "BinaryPropertySetId")
    VALUES ('5f7b7beb-5aae-496a-925c-b3a43666c742', 'the move down on the bay - YouTube.webm', '2020-07-19 00:02:07', '2020-07-19 00:04:35', '2021-08-22 14:32:22', '3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a',
        '82d46e21-5eba-4f1b-8c99-78cb94689316');
-- \\servicenowdiag479.file.core.windows.net\testazureshare
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('b228346f-7023-4ba9-afe3-8e9ff758971f', '\\servicenowdiag479.file.core.windows.net\testazureshare', '2020-05-11 09:31:25', '2021-06-04 13:48:55', '2021-08-22 15:04:12', NULL,
        'c48c1c92-154c-43cf-a277-53223d5c1510', '2021-08-22 15:04:12', '2021-08-22 15:04:12');
-- \\servicenowdiag479.file.core.windows.net\testazureshare\webroot
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "Options", "Notes", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('c7f6d510-6acf-43c0-b5d5-d5f99cca0ce3', 'webroot', '2021-08-22 15:04:12', '2021-01-03 12:30:47', '2021-01-05 10:52:06', 1, 'Only scanning the root files.', 'b228346f-7023-4ba9-afe3-8e9ff758971f', NULL,
        '2021-08-22 15:04:12', '2021-08-22 15:04:12');
-- \\servicenowdiag479.file.core.windows.net\testazureshare\webroot
INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "RootId", "StatusValue", "LastCrawlStart", "LastCrawlEnd", "CreatedOn", "ModifiedOn")
    VALUES ('fa6c52c5-862b-4bf7-a145-ad7d2533a1d2', 'Web Root', 'c7f6d510-6acf-43c0-b5d5-d5f99cca0ce3', 0, '2021-08-22 15:04:17', '2021-08-22 15:04:31', '2021-08-22 15:04:12', '2021-08-22 15:04:32');
-- G:\Users\lerwi\Downloads
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('A85D3A22-C402-43F1-AC82-B2B83B843C0F', 'Downloads', '2021-09-24 07:17:22', '2019-11-23 20:30:23', '2021-09-24 06:16:50', '38a40fde-acf0-4cc5-9302-d37ec2cbb631', NULL, '2021-09-24 07:17:34',
        '2021-09-24 07:17:34');
-- G:\Users\lerwi\Downloads\Local Downloads
INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "RootId", "StatusValue", "CreatedOn", "ModifiedOn")
    VALUES ('5221E107-D03D-4D9D-AB2A-55425AF103E0', 'Local Downloads', 'A85D3A22-C402-43F1-AC82-B2B83B843C0F', 7, '2021-09-24 07:18:24', '2021-09-24 07:18:24');
-- G:\Users\lerwi\OneDrive
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "Options", "Status", "VolumeId", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('C8E9C683-82A2-4BCE-8C59-2E57055FFEA7', 'OneDrive', '2021-09-24 08:00:03', '2019-11-23 20:35:06', '2021-09-24 06:01:39', 0, 1, NULL, '38a40fde-acf0-4cc5-9302-d37ec2cbb631', '2021-09-24 08:00:04',
        '2021-09-24 08:00:04');
-- G:\Users\lerwi\OneDrive\Music
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "Options", "Status", "VolumeId", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('2AB26D8B-562C-44CB-8C1C-971733F5DC04', 'Music', '2021-09-24 08:00:03', '2019-11-23 20:35:49', '2021-09-20 18:48:49', 0, 1, NULL, 'C8E9C683-82A2-4BCE-8C59-2E57055FFEA7', '2021-09-24 08:00:04',
        '2021-09-24 08:00:04');
-- G:\Users\lerwi\OneDrive\Music\SmashMouth
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "Options", "Status", "VolumeId", "ParentId", "CreatedOn", "ModifiedOn")
    VALUES ('58CB39A1-6080-4F73-A1E7-34274552B47B', 'SmashMouth', '2021-09-24 08:00:03', '2021-07-22 23:06:10', '2021-07-22 23:06:10', 0, 0, NULL, '2AB26D8B-562C-44CB-8C1C-971733F5DC04', '2021-09-24 08:00:04',
        '2021-09-24 08:00:04');
-- G:\Users\lerwi\OneDrive\Music\SmashMouth
INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "MaxRecursionDepth", "RootId", "StatusValue", "CreatedOn", "ModifiedOn")
    VALUES ('2DD01786-78F7-45A3-8C18-7B02E3336768', 'Music folder', 256, '58CB39A1-6080-4F73-A1E7-34274552B47B', 0,	'2021-09-24 08:01:01', '2021-09-24 08:01:01');
INSERT INTO "VolumeAccessErrors" ("Id", "ErrorCode", "Message", "CreatedOn", "ModifiedOn", "TargetId")
    VALUES ('b806e05a-b705-4ef7-b127-a8e477125cfc', 2, 'Network unreachable.', '2021-09-24 14:15:09', '2021-09-24 14:15:09', 'c48c1c92-154c-43cf-a277-53223d5c1510');
-- \\servicenowdiag479.file.core.windows.net\testazureshare\webroot
INSERT INTO "CrawlJobLogs" ("Id", "RootPath", "StatusCode", "CrawlStart", "CrawlEnd", "StatusMessage", "StatusDetail", "FoldersProcessed", "FilesProcessed", "CreatedOn", "ModifiedOn", "ConfigurationId")
    VALUES ('7a337ebd-4dc6-4560-ba21-bc1ed1262d49', '\\servicenowdiag479.file.core.windows.net\testazureshare\webroot', 6, '2021-09-24 14:12:06', '2021-09-24 14:15:09', 'Network unreachable.', '', 0, 0,
        '2021-09-24 14:15:09', '2021-09-24 14:15:09', 'fa6c52c5-862b-4bf7-a145-ad7d2533a1d2');
-- C:\
INSERT INTO "Volumes" ("Id", "DisplayName", "VolumeName", "Identifier", "ReadOnly", "MaxNameLength", "Type", "Status", "Notes", "CreatedOn", "ModifiedOn", "UpstreamId", "LastSynchronizedOn", "FileSystemId")
    VALUES ('dfe10650-2f3a-48ee-ba9c-8777878e7850', 'C:', 'Windows', 'urn:volume:id:07FE-6760', 0, 255, 3, 3, 'New computer system', '2022-05-28 05:18:50', '2022-05-28 22:25:21',
        '14e2ef71-9044-468b-90d5-e5e3f32445a4', '2022-05-28 22:25:21', 'bedb396b-2212-4149-9cad-7e437c47314c');
-- C:\
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('8b42a599-9e72-4fb0-a7a4-836d05da1400', 'C:', '2022-05-28 20:59:01', '2021-06-05 08:01:25', '2022-05-26 19:55:20', NULL, 'dfe10650-2f3a-48ee-ba9c-8777878e7850', '2022-05-28 20:59:01',
        '2022-05-28 20:59:01');
-- C:\Users
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('850b2107-0eb1-4d9c-8834-b829f2cf4ecd', 'Users', '2022-05-28 20:59:02', '2021-06-05 08:01:25', '2022-05-14 15:02:20', '8b42a599-9e72-4fb0-a7a4-836d05da1400', NULL, '2022-05-28 20:59:02',
        '2022-05-28 20:59:02');
-- C:\Users\Lenny
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('8cfb4602-ffd1-488b-99af-78e57aa39dd6', 'Lenny', '2022-05-28 20:59:02', '2022-05-14 14:43:34', '2022-05-28 01:18:59', '850b2107-0eb1-4d9c-8834-b829f2cf4ecd', NULL, '2022-05-28 20:59:02',
        '2022-05-28 20:59:02');
-- C:\Users\Lenny
INSERT INTO "CrawlConfigurations" ("Id", "DisplayName", "MaxRecursionDepth", "RootId", "StatusValue", "CreatedOn", "ModifiedOn")
    VALUES ('73bc1748-12d1-4e36-a493-634f26247080', 'Personal folder', 256, '8cfb4602-ffd1-488b-99af-78e57aa39dd6', 0,  '2022-05-28 20:59:07', '2022-05-28 21:00:30');
-- C:\Users\Lenny
INSERT INTO "CrawlJobLogs" ("Id", "MaxRecursionDepth", "RootPath", "StatusCode", "CrawlStart", "CrawlEnd", "StatusMessage", "StatusDetail", "FoldersProcessed", "FilesProcessed", "CreatedOn", "ModifiedOn",
        "ConfigurationId")
    VALUES ('b14788f5-278f-4e17-9e04-bc3e0eb658be', 256, 'C:\Users\Lenny', 2, '2022-05-28 20:59:15', '2022-05-28 21:00:29', '', '', 22, 18, '2022-05-28 20:59:15', '2022-05-28 21:00:29',
        '73bc1748-12d1-4e36-a493-634f26247080');
-- C:\Users\Lenny\Downloads
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('27dbeea5-a865-4d8d-996d-cb28264f8bf3', 'Downloads', '2022-05-28 20:59:16', '2022-05-14 14:43:34', '2022-05-28 17:51:28', '8cfb4602-ffd1-488b-99af-78e57aa39dd6', NULL, '2022-05-28 20:59:16',
        '2022-05-28 20:59:16');
-- C:\Users\Lenny\Downloads\MyMovie.mp4
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('03b7fba6-99d0-473f-9aab-7e2be5a950c1', 348430388, X'8c9928f7b7a6d340fcc925ea8295425a', '2022-05-28 20:59:16', '2022-05-28 20:59:16');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('c77df27b-6b60-41a2-b3fb-3a15fcbf268e', 'video/mp4', '.mp4', 'MP4VideoFile(VLC)', 'video', 'video/mp4', '2022-05-28 20:59:17', '2022-05-28 20:59:17');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "SampleRate", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('a7abc1fb-cbcf-4acd-8867-a9f44624110b', 185344, '{00001610-0000-0010-8000-00AA00389B71}', 48000, 16, 1, '2022-05-28 20:59:18', '2022-05-28 20:59:18');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('85466d4c-5970-4830-83d6-969d840ff196', '2022-05-21 10:47:04', '2022-05-28 20:59:19', '2022-05-28 20:59:19');
INSERT INTO "DRMPropertySets" ("Id", "IsProtected", "CreatedOn", "ModifiedOn")
    VALUES('78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82', 0, '2022-05-28 20:59:19', '2022-05-28 20:59:19');
INSERT INTO "VideoPropertySets" ("Id", "Compression", "EncodingBitrate", "FrameHeight", "FrameRate", "FrameWidth", "HorizontalAspectRatio", "StreamNumber", "VerticalAspectRatio", "CreatedOn", "ModifiedOn")
    VALUES('21a056a1-3d6e-4481-b774-abf1fd7645a9', '{34363248-0000-0010-8000-00AA00389B71}', 27169320, 2160, 24990, 3840, 1, 2, 1, '2022-05-28 20:59:20', '2022-05-28 20:59:20');
-- C:\Users\Lenny\Downloads\MyMovie.mp4
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "VideoPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('d19de127-22a2-4e38-bc7e-0ce1eaa20f67', 'MyMovie.mp4', '2022-05-21 10:47:02', '2022-05-21 12:41:34', '27dbeea5-a865-4d8d-996d-cb28264f8bf3', '03b7fba6-99d0-473f-9aab-7e2be5a950c1',
        '2022-05-28 20:59:16', 'c77df27b-6b60-41a2-b3fb-3a15fcbf268e', 'a7abc1fb-cbcf-4acd-8867-a9f44624110b', '85466d4c-5970-4830-83d6-969d840ff196', '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82',
        '21a056a1-3d6e-4481-b774-abf1fd7645a9', '2022-05-28 20:59:20', '2022-05-28 20:59:20');
-- C:\Users\Lenny\Downloads\Audition.mp4
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('67f8f53d-b5f7-4b95-b666-045f071f5a85', 979963869, X'f214d52f52ba6c633bc906bd1de500e0', '2022-05-28 20:59:23', '2022-05-28 20:59:23');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('b6528e8f-8427-48e9-a087-e34cb4064fb1', 'video/mp4', '.mp4', 'MP4VideoFile(VLC)', 'video', 'video/mp4', '2022-05-28 20:59:24', '2022-05-28 20:59:24');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "SampleRate", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('6e9ddac9-700e-4e27-bbe9-6470553bc498', 317392, '{00001610-0000-0010-8000-00AA00389B71}', 48000, 16, 1, '2022-05-28 20:59:24', '2022-05-28 20:59:24');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('6263d0e5-2a32-4433-a707-9dd1b405aa10', '2022-05-08 09:30:06', '2022-05-28 20:59:24', '2022-05-28 20:59:24');
INSERT INTO "VideoPropertySets" ("Id", "Compression", "EncodingBitrate", "FrameHeight", "FrameRate", "FrameWidth", "HorizontalAspectRatio", "StreamNumber", "VerticalAspectRatio", "CreatedOn", "ModifiedOn")
    VALUES('6adeeee8-dcf9-42d3-a555-0eefbc957921', '{34363248-0000-0010-8000-00AA00389B71}', 6382824, 1080, 25000, 1920, 1, 2, 1, '2022-05-28 20:59:25', '2022-05-28 20:59:25');
-- C:\Users\Lenny\Downloads\Audition.mp4
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "VideoPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('f3154918-2062-4421-af2d-638270f1675b', 'Audition.mp4', '2022-05-19 22:47:22', '2022-05-08 09:30:06', '27dbeea5-a865-4d8d-996d-cb28264f8bf3', '67f8f53d-b5f7-4b95-b666-045f071f5a85',
        '2022-05-28 20:59:23', 'b6528e8f-8427-48e9-a087-e34cb4064fb1', '6e9ddac9-700e-4e27-bbe9-6470553bc498', '6263d0e5-2a32-4433-a707-9dd1b405aa10', '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82',
        '6adeeee8-dcf9-42d3-a555-0eefbc957921', '2022-05-28 20:59:25', '2022-05-28 20:59:25');
-- C:\Users\Lenny\Downloads\ClientTransactionDetails.xls
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('f161f02b-af05-4d9f-a8d0-5fbf4fcef390', 77659, X'a82451786a21690111db589d1c8e548c', '2022-05-28 20:59:26', '2022-05-28 20:59:26');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "Kind", "CreatedOn", "ModifiedOn")
    VALUES('4b95a568-a7fd-4f63-aaa3-c1e4c6d62478', 'application/vnd.ms-excel', '.xls', 'MicrosoftExcel97-2003Worksheet', 'document', '2022-05-28 20:59:27', '2022-05-28 20:59:27');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('05ea8f5a-79cd-4476-a8fb-ba746bde196a', '2014-12-21 18:05:32', '2022-05-28 20:59:27', '2022-05-28 20:59:27');
-- C:\Users\Lenny\Downloads\ClientTransactionDetails.xls
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "DocumentPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('4c21d8c4-1f21-44b9-a9af-a65ee72133ee', 'ClientTransactionDetails.xls', '2022-05-27 19:25:28', '2014-12-21 18:05:32', '27dbeea5-a865-4d8d-996d-cb28264f8bf3', 'f161f02b-af05-4d9f-a8d0-5fbf4fcef390',
        '2022-05-28 20:59:26', '4b95a568-a7fd-4f63-aaa3-c1e4c6d62478', '05ea8f5a-79cd-4476-a8fb-ba746bde196a', '2022-05-28 20:59:28', '2022-05-28 20:59:28');
-- C:\Users\Lenny\Downloads\VdhCoAppSetup-1.6.3.exe
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('775d1fa4-92f6-46cc-8742-3fb169ad0995', 44612640, X'9be9b671c7dd8ad50413975352782a6a', '2022-05-28 20:59:29', '2022-05-28 20:59:29');
INSERT INTO "SummaryPropertySets" ("Id", "FileDescription", "FileVersion", "Company", "ContentType", "ItemType", "ItemTypeText", "Kind", "CreatedOn", "ModifiedOn")
    VALUES('fa4e65d0-dad9-4656-8688-1651e819fea3', 'VdhCoAppSetup', '0.0.0.0', 'DownloadHelper', 'application/x-msdownload', '.exe', 'Application', 'program', '2022-05-28 20:59:29', '2022-05-28 20:59:29');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('9dbb96da-8a54-46f1-8144-11444fef6a06', '2022-05-15 06:08:30', '2022-05-28 20:59:30', '2022-05-28 20:59:30');
-- C:\Users\Lenny\Downloads\VdhCoAppSetup-1.6.3.exe
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "DocumentPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('efbd1eaf-6abd-4bbb-a7be-f56607acb705', 'VdhCoAppSetup-1.6.3.exe', '2022-05-15 06:08:29', '2022-05-15 06:08:32', '27dbeea5-a865-4d8d-996d-cb28264f8bf3', '775d1fa4-92f6-46cc-8742-3fb169ad0995',
        '2022-05-28 20:59:29', 'fa4e65d0-dad9-4656-8688-1651e819fea3', '9dbb96da-8a54-46f1-8144-11444fef6a06', '2022-05-28 20:59:31', '2022-05-28 20:59:31');
-- C:\Users\Lenny\Dropbox
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('c9e1e725-5e4c-43d9-98ad-dd0e2e26a1f6', 'Dropbox', '2022-05-28 20:59:31', '2022-05-14 19:03:59', '2022-05-15 12:39:08', '8cfb4602-ffd1-488b-99af-78e57aa39dd6', NULL, '2022-05-28 20:59:31',
        '2022-05-28 20:59:31');
-- C:\Users\Lenny\Dropbox\Cruise2019
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('d6f0112e-c909-4570-a89d-d5829035e98f', 'Cruise2019', '2022-05-28 20:59:32', '2022-05-14 19:04:01', '2022-05-14 19:04:02', 'c9e1e725-5e4c-43d9-98ad-dd0e2e26a1f6', NULL, '2022-05-28 20:59:32',
        '2022-05-28 20:59:32');
-- C:\Users\Lenny\Dropbox\Cruise2019\05-11-2019
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('a1951abb-2dfb-47fe-8842-e5d5f1a3484f', '05-11-2019', '2022-05-28 20:59:32', '2022-05-14 19:04:01', '2022-05-14 19:04:02', 'd6f0112e-c909-4570-a89d-d5829035e98f', NULL, '2022-05-28 20:59:32',
        '2022-05-28 20:59:32');
-- C:\Users\Lenny\Dropbox\Cruise2019\05-11-2019\iCloud Photos
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('422dc1f3-cfd3-4c10-9c5a-6c61da5df62a', 'iCloud Photos', '2022-05-28 20:59:33', '2022-05-14 19:04:02', '2022-05-14 19:04:13', 'a1951abb-2dfb-47fe-8842-e5d5f1a3484f', NULL, '2022-05-28 20:59:33',
        '2022-05-28 20:59:33');
-- C:\Users\Lenny\Dropbox\Cruise2019\05-11-2019\iCloud Photos\57926728061__71DEA427-4227-4B5F-97C9-3026A51827D5.JPG
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('7b34654b-b1e5-46fb-af53-5fb01f1b717b', 2479913, X'64b9bcaf3f1274c3e4d20777e52e0d49', '2022-05-28 20:59:33', '2022-05-28 20:59:33');
INSERT INTO "SummaryPropertySets" ("Id", "ApplicationName", "ContentType", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('1622a6ed-be3b-4a76-9b37-3e3c25e3c497', '12.2', 'image/jpeg', '.JPG', 'JPGFile', 'picture', 'image/jpeg', '2022-05-28 20:59:33', '2022-05-28 20:59:33');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('49239a95-21a5-4012-8da0-3406bd5a630f', '2019-05-11 20:34:54', '2022-05-28 20:59:34', '2022-05-28 20:59:34');
INSERT INTO "ImagePropertySets" ("Id", "BitDepth", "ColorSpace", "HorizontalResolution", "HorizontalSize", "ResolutionUnit", "VerticalResolution", "VerticalSize", "CreatedOn", "ModifiedOn")
    VALUES('eab6a416-3294-4909-82f6-d28450f115aa', 24, 1, 72, 3024, 2, 72, 4032, '2022-05-28 20:59:34', '2022-05-28 20:59:34');
INSERT INTO "PhotoPropertySets" ("Id", "CameraManufacturer", "CameraModel", "EXIFVersion", "Orientation", "OrientationText", "CreatedOn", "ModifiedOn")
    VALUES('68e9ffa8-cd3e-417b-8bec-2ff8126dca32', 'Apple', 'iPhone6s', '0221', 6, 'Rotate270degrees', '2022-05-28 20:59:35', '2022-05-28 20:59:35');
-- C:\Users\Lenny\Dropbox\Cruise2019\05-11-2019\iCloud Photos\57926728061__71DEA427-4227-4B5F-97C9-3026A51827D5.JPG
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "DocumentPropertySetId", "ImagePropertySetId",
        "PhotoPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('e8a4c6f9-bd9a-4a56-a7c9-44a780e111d9', '57926728061__71DEA427-4227-4B5F-97C9-3026A51827D5.JPG', '2022-05-14 19:04:03', '2019-05-11 20:34:54', '422dc1f3-cfd3-4c10-9c5a-6c61da5df62a',
        '7b34654b-b1e5-46fb-af53-5fb01f1b717b', '2022-05-28 20:59:33', '1622a6ed-be3b-4a76-9b37-3e3c25e3c497', '49239a95-21a5-4012-8da0-3406bd5a630f', 'eab6a416-3294-4909-82f6-d28450f115aa',
        '68e9ffa8-cd3e-417b-8bec-2ff8126dca32', '2022-05-28 20:59:36', '2022-05-28 20:59:36');
-- C:\Users\Lenny\Music
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('9224512c-1d30-43d8-abfe-2ba6c17c53e6', 'Music', '2022-05-28 20:59:37', '2022-05-14 14:43:34', '2022-05-27 19:16:19', '8cfb4602-ffd1-488b-99af-78e57aa39dd6', NULL, '2022-05-28 20:59:37',
        '2022-05-28 20:59:37');
-- C:\Users\Lenny\Music\Eldorado
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('ef7637ec-fc3c-47aa-baac-ff49a801fbfe', 'Eldorado', '2022-05-28 20:59:38', '2022-05-27 19:16:19', '2022-05-27 19:16:23', '9224512c-1d30-43d8-abfe-2ba6c17c53e6', NULL, '2022-05-28 20:59:38',
        '2022-05-28 20:59:38');
-- C:\Users\Lenny\Music\Eldorado\08. ILLUSIONS_IN_G_MAJOR.WAV
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('3094e4c0-b4cd-423c-a111-ee448bd002a5', 27751004, X'c9dc7a54b193e18697b57544596cae5a', '2022-05-28 20:59:38', '2022-05-28 20:59:38');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('43b292a6-9f5d-465b-bc4a-fdfc8c01a659', 'audio/wav', '.WAV', 'WAVFile', 'music', 'audio/wav', '2022-05-28 20:59:39', '2022-05-28 20:59:39');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "SampleRate", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('47ef548a-4b84-41b5-a431-b30f46d68b61', 1411200, '{00000001-0000-0010-8000-00AA00389B71}', 44100, 16, 1, '2022-05-28 20:59:39', '2022-05-28 20:59:39');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('5cf71589-6130-45f7-bfa1-111e88cb9a86', '2004-04-24 15:06:36', '2022-05-28 20:59:40', '2022-05-28 20:59:40');
-- C:\Users\Lenny\Music\Eldorado\08. ILLUSIONS_IN_G_MAJOR.WAV
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('c9204845-965a-430c-aac7-0b355b2502eb', '08. ILLUSIONS_IN_G_MAJOR.WAV', '2022-05-27 19:16:22', '2004-04-24 15:06:36', 'ef7637ec-fc3c-47aa-baac-ff49a801fbfe', '3094e4c0-b4cd-423c-a111-ee448bd002a5',
        '2022-05-28 20:59:38', '43b292a6-9f5d-465b-bc4a-fdfc8c01a659', '47ef548a-4b84-41b5-a431-b30f46d68b61', '5cf71589-6130-45f7-bfa1-111e88cb9a86', '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82', '2022-05-28 20:59:42',
        '2022-05-28 20:59:42');
-- C:\Users\Lenny\Music\Eldorado\Jeff Lynne''s ELO - Alone In The Universe [2015] [MP3-VBR] [H4CKUS] [GloDLS]
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('8bfb57be-10c7-4da6-9364-62a8d17ab1ae', 'Jeff Lynne''s ELO - Alone In The Universe [2015] [MP3-VBR] [H4CKUS] [GloDLS]', '2022-05-28 20:59:43', '2022-05-27 19:15:25', '2022-05-27 19:15:25',
        '9224512c-1d30-43d8-abfe-2ba6c17c53e6', NULL, '2022-05-28 20:59:43', '2022-05-28 20:59:43');
-- C:\Users\Lenny\Music\Eldorado\Jeff Lynne's ELO - Alone In The Universe [2015] [MP3-VBR] [H4CKUS] [GloDLS]\01-jeff_lynnes_elo-when_i_was_a_boy.mp3
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('f91afd66-cdfb-42cf-ade6-89bdf77ff30d', 5976319, X'12e06a64d394f68573fa1290e161affc', '2022-05-28 20:59:43', '2022-05-28 20:59:43');
INSERT INTO "SummaryPropertySets" ("Id", "Author", "Title", "ContentType", "ItemAuthors", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('11f8790e-74e3-40fd-a230-fefa7575a6f7', 'JeffLynne''sELO', 'WhenIWasABoy', 'audio/mpeg', 'JeffLynne''sELO', '.mp3', 'MP3File', 'music', 'audio/mpeg', '2022-05-28 20:59:44', '2022-05-28 20:59:44');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "IsVariableBitrate", "SampleRate", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('da5e143f-5e6f-47c9-be26-a8567bcbe02b', 247584, '{00000055-0000-0010-8000-00AA00389B71}', 0, 44100, 16, 0, '2022-05-28 20:59:44', '2022-05-28 20:59:44');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('417a8c71-037e-4826-a690-9fe4814a1b49', '2017-07-14 08:35:56', '2022-05-28 20:59:45', '2022-05-28 20:59:45');
INSERT INTO "MediaPropertySets" ("Id", "Publisher", "Year", "CreatedOn", "ModifiedOn")
    VALUES('6276edee-b98a-4cfd-9a39-b0ff68146e85', 'ColumbiaRecords', 2015, '2022-05-28 20:59:47', '2022-05-28 20:59:47');
INSERT INTO "MusicPropertySets" ("Id", "AlbumArtist", "AlbumTitle", "Artist", "Composer", "DisplayArtist", "Genre", "TrackNumber", "CreatedOn", "ModifiedOn")
    VALUES('8e7102df-9c73-49fb-b884-c95a86d9f029', 'JeffLynne''sELO', 'AloneInTheUniverse', 'JeffLynne''sELO', 'JeffLynne''sELO', 'JeffLynne''sELO', 'Rock', 1, '2022-05-28 20:59:47', '2022-05-28 20:59:47');
-- C:\Users\Lenny\Music\Eldorado\Jeff Lynne's ELO - Alone In The Universe [2015] [MP3-VBR] [H4CKUS] [GloDLS]\01-jeff_lynnes_elo-when_i_was_a_boy.mp3
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "MediaPropertySetId", "MusicPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('b0cd4fe4-9dc5-4a86-94e6-efdceab20dfd', '01-jeff_lynnes_elo-when_i_was_a_boy.mp3', '2022-05-27 19:15:25', '2017-07-14 08:35:56', '8bfb57be-10c7-4da6-9364-62a8d17ab1ae',
        'f91afd66-cdfb-42cf-ade6-89bdf77ff30d', '2022-05-28 20:59:43', '11f8790e-74e3-40fd-a230-fefa7575a6f7', 'da5e143f-5e6f-47c9-be26-a8567bcbe02b', '417a8c71-037e-4826-a690-9fe4814a1b49',
        '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82', '6276edee-b98a-4cfd-9a39-b0ff68146e85', '8e7102df-9c73-49fb-b884-c95a86d9f029', '2022-05-28 20:59:47', '2022-05-28 20:59:47');
-- C:\Users\Lenny\Music\Eldorado\Jeff Lynne's ELO - Alone In The Universe [2015] [MP3-VBR] [H4CKUS] [GloDLS]\06-jeff_lynnes_elo-aint_it_a_drag.mp3
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('fdfdc4db-646f-4ff4-b2ef-8d01c3ee125a', 5163295, X'1ee6f518e5cd115a9730ba00ef472979', '2022-05-28 20:59:47', '2022-05-28 20:59:47');
INSERT INTO "SummaryPropertySets" ("Id", "Author", "Title", "ContentType", "ItemAuthors", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('2da8eab2-e647-440f-be0c-0bac9d064e06', 'JeffLynne''sELO', 'Ain''tItADrag', 'audio/mpeg', 'JeffLynne''sELO', '.mp3', 'MP3File', 'music', 'audio/mpeg', '2022-05-28 20:59:48', '2022-05-28 20:59:48');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "IsVariableBitrate", "SampleRate", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('d771b085-9f4c-40b9-8543-3934a69a239d', 264376, '{00000055-0000-0010-8000-00AA00389B71}', 0, 44100, 16, 0, '2022-05-28 20:59:49', '2022-05-28 20:59:49');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('df45690e-f340-41ea-ae17-bc7db3c99217', '2017-09-27 10:41:36', '2022-05-28 20:59:49', '2022-05-28 20:59:49');
INSERT INTO "MediaPropertySets" ("Id", "Publisher", "Year", "CreatedOn", "ModifiedOn")
    VALUES('9c378c32-6c2b-46aa-a99a-ef81f537e014', 'ColumbiaRecords', 2015, '2022-05-28 20:59:51', '2022-05-28 20:59:51');
INSERT INTO "MusicPropertySets" ("Id", "AlbumArtist", "AlbumTitle", "Artist", "Composer", "DisplayArtist", "Genre", "TrackNumber", "CreatedOn", "ModifiedOn")
    VALUES('8fc99aaf-1bfc-49d0-878c-e68639cf4622', 'JeffLynne''sELO', 'AloneInTheUniverse', 'JeffLynne''sELO', 'JeffLynne''sELO', 'JeffLynne''sELO', 'Rock', 6, '2022-05-28 20:59:51', '2022-05-28 20:59:51');
-- C:\Users\Lenny\Music\Eldorado\Jeff Lynne's ELO - Alone In The Universe [2015] [MP3-VBR] [H4CKUS] [GloDLS]\06-jeff_lynnes_elo-aint_it_a_drag.mp3
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "MediaPropertySetId", "MusicPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('3c819f79-d4df-4ef9-9266-4e144a425ed4', '06-jeff_lynnes_elo-aint_it_a_drag.mp3', '2022-05-27 19:15:25', '2017-09-27 10:41:36', '8bfb57be-10c7-4da6-9364-62a8d17ab1ae',
        'fdfdc4db-646f-4ff4-b2ef-8d01c3ee125a', '2022-05-28 20:59:47', '2da8eab2-e647-440f-be0c-0bac9d064e06', 'd771b085-9f4c-40b9-8543-3934a69a239d', 'df45690e-f340-41ea-ae17-bc7db3c99217',
        '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82', '9c378c32-6c2b-46aa-a99a-ef81f537e014', '8fc99aaf-1bfc-49d0-878c-e68639cf4622', '2022-05-28 20:59:52', '2022-05-28 20:59:52');
-- C:\Users\Lenny\OneDrive
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('2d138032-8062-4520-b310-42192e893fb5', 'OneDrive', '2022-05-28 20:59:54', '2022-05-14 14:46:43', '2022-05-28 01:21:00', '8cfb4602-ffd1-488b-99af-78e57aa39dd6', NULL, '2022-05-28 20:59:54',
        '2022-05-28 20:59:54');
-- C:\Users\Lenny\OneDrive\Documents
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('21c2d910-d933-4299-b212-2a7162d1becd', 'Documents', '2022-05-28 20:59:54', '2022-05-15 12:26:46', '2022-05-26 19:48:18', '2d138032-8062-4520-b310-42192e893fb5', NULL, '2022-05-28 20:59:54',
        '2022-05-28 20:59:54');
-- C:\Users\Lenny\OneDrive\Documents\Custom Office Templates
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('98fa6b88-d04b-42aa-9765-c8d0c7822727', 'Custom Office Templates', '2022-05-28 20:59:54', '2022-05-15 12:27:14', '2022-05-15 12:27:14', '21c2d910-d933-4299-b212-2a7162d1becd', NULL,
        '2022-05-28 20:59:54', '2022-05-28 20:59:54');
-- C:\Users\Lenny\OneDrive\Documents\Custom Office Templates\APA Essay.dotm
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('3526d6a6-e780-4b57-bc47-ce620b50881c', 28528, X'8b2dee7a7083b2086779e77f92f30b4c', '2022-05-28 20:59:54', '2022-05-28 20:59:54');
INSERT INTO "SummaryPropertySets" ("Id", "ApplicationName", "Author", "Company", "ContentType", "ItemAuthors", "ItemType", "ItemTypeText", "Kind", "CreatedOn", "ModifiedOn")
    VALUES('095334bd-87d7-4b8e-ba29-ccce3447ca3b', 'MicrosoftOfficeWord', 'LeonardT.Erwine', 'WesternGovernorsUniversity', 'application/vnd.ms-word.template.macroEnabled.12', 'LeonardT.Erwine', '.dotm',
        'MicrosoftWordMacro-EnabledTemplate', 'document', '2022-05-28 20:59:55', '2022-05-28 20:59:55');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "LastAuthor", "RevisionNumber", "Security", "CreatedOn", "ModifiedOn")
    VALUES('3401add0-a630-4ff7-a158-c9d6ba2bb368', '2018-07-21 02:47:00', 'LeonardErwine', '3', 0, '2022-05-28 20:59:55', '2022-05-28 20:59:55');
-- C:\Users\Lenny\OneDrive\Documents\Custom Office Templates\APA Essay.dotm
INSERT INTO "Files" ("Id", "Name", "LastAccessed", "Options", "Status", "LastHashCalculation", "CreationTime", "LastWriteTime", "Notes", "CreatedOn", "ModifiedOn", "UpstreamId", "LastSynchronizedOn", "ParentId",
        "BinaryPropertySetId", "SummaryPropertySetId", "DocumentPropertySetId")
    VALUES ('08d6a180-b23b-4cc6-a2ed-27cad48bde67', 'APA Essay.dotm', '2022-05-28 21:11:59', '0', '0', '2022-05-28 20:59:54', '2022-05-15 12:27:14', '2019-10-17 23:06:23',
        'Manually synchronized document properties', '2022-05-28 20:59:56', '2022-05-28 22:25:26', '8a3747d1-ba93-4300-acc3-50269e984194', '2022-05-28 22:25:26', '98fa6b88-d04b-42aa-9765-c8d0c7822727',
        '3526d6a6-e780-4b57-bc47-ce620b50881c', '095334bd-87d7-4b8e-ba29-ccce3447ca3b', '3401add0-a630-4ff7-a158-c9d6ba2bb368');
-- C:\Users\Lenny\OneDrive\Music
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('2e17a3c5-b85e-4b4c-918e-d84dd6c02e29', 'Music', '2022-05-28 20:59:57', '2022-05-15 12:26:44', '2022-05-15 12:26:52', '2d138032-8062-4520-b310-42192e893fb5', NULL, '2022-05-28 20:59:57',
        '2022-05-28 20:59:57');
-- C:\Users\Lenny\OneDrive\Music\Help! [UK]
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('c1b48a43-ea1e-4511-80fa-55513ed970ea', 'Help! [UK]', '2022-05-28 20:59:58', '2022-05-15 12:26:45', '2022-05-15 12:26:45', '2e17a3c5-b85e-4b4c-918e-d84dd6c02e29', NULL, '2022-05-28 20:59:58',
        '2022-05-28 20:59:58');
-- C:\Users\Lenny\OneDrive\Music\Help! [UK]\The Beatles - I've Just Seen a Face.mp3
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('141f716b-3ef7-4060-9b07-1113518fa09e', 2044032, X'b70278d5ea2d79fd00d581b013b95618', '2022-05-28 20:59:58', '2022-05-28 20:59:58');
INSERT INTO "SummaryPropertySets" ("Id", "Author", "Title", "ContentType", "ItemAuthors", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('4fe69351-7d09-42a4-8dab-2fb9c9d28e5b', 'TheBeatles', 'I''veJustSeenaFace', 'audio/mpeg', 'TheBeatles', '.mp3', 'MP3File', 'music', 'audio/mpeg', '2022-05-28 20:59:59', '2022-05-28 20:59:59');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "IsVariableBitrate", "SampleRate", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('d65e7907-c8b2-4a2a-8aa0-9e3a6f978702', 128000, '{00000055-0000-0010-8000-00AA00389B71}', 0, 44100, 16, 0, '2022-05-28 21:00:00', '2022-05-28 21:00:00');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('fd93db99-04b8-4cba-85cc-ccac09e8900d', '2010-09-03 10:47:04', '2022-05-28 21:00:01', '2022-05-28 21:00:01');
INSERT INTO "MediaPropertySets" ("Id", "Publisher", "Year", "CreatedOn", "ModifiedOn")
    VALUES('f5791723-a9d1-4af6-9d45-268e8a30bd5e', 'Toshiba', 1965, '2022-05-28 21:00:02', '2022-05-28 21:00:02');
INSERT INTO "MusicPropertySets" ("Id", "AlbumArtist", "AlbumTitle", "Artist", "Composer", "DisplayArtist", "Genre", "TrackNumber", "CreatedOn", "ModifiedOn")
    VALUES('49cf85de-52e6-48da-bf27-c29fde85cbfd', 'TheBeatles', 'Help![UK]', 'TheBeatles', 'JohnLennon`zPaulMcCartney', 'TheBeatles', 'Rock/Pop', 12, '2022-05-28 21:00:03', '2022-05-28 21:00:03');
-- C:\Users\Lenny\OneDrive\Music\Help! [UK]\The Beatles - I''ve Just Seen a Face.mp3
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "MediaPropertySetId", "MusicPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('94c659d6-446d-4cb0-bb53-e42bea7bc643', 'The Beatles - I''ve Just Seen a Face.mp3', '2022-05-15 12:26:45', '2010-09-03 10:47:04', 'c1b48a43-ea1e-4511-80fa-55513ed970ea',
        '141f716b-3ef7-4060-9b07-1113518fa09e', '2022-05-28 20:59:58', '4fe69351-7d09-42a4-8dab-2fb9c9d28e5b', 'd65e7907-c8b2-4a2a-8aa0-9e3a6f978702', 'fd93db99-04b8-4cba-85cc-ccac09e8900d',
        '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82', 'f5791723-a9d1-4af6-9d45-268e8a30bd5e', '49cf85de-52e6-48da-bf27-c29fde85cbfd', '2022-05-28 21:00:03', '2022-05-28 21:00:03');
-- C:\Users\Lenny\OneDrive\Music\John Denver
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('774f9487-38cd-4f15-b719-de00102d38a2', 'John Denver', '2022-05-28 21:00:05', '2022-05-15 12:26:45', '2022-05-15 12:26:45', '2e17a3c5-b85e-4b4c-918e-d84dd6c02e29', NULL, '2022-05-28 21:00:05',
        '2022-05-28 21:00:05');
-- C:\Users\Lenny\OneDrive\Music\John Denver\Rocky Mountain High.mp3
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('47385b1b-f0b9-454a-a0af-e8d03c4d2036', 4530009, X'43e15aaa35940378886d9d20fdfcff6c', '2022-05-28 21:00:05', '2022-05-28 21:00:05');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('a34eb424-819c-4332-a835-1db3f2dc1abb', 'audio/mpeg', '.mp3', 'MP3File', 'music', 'audio/mpeg', '2022-05-28 21:00:06', '2022-05-28 21:00:06');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "IsVariableBitrate", "SampleRate", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('70bf5f23-f855-4b68-9cd3-f978f05c31d5', 128000, '{00000055-0000-0010-8000-00AA00389B71}', 0, 44100, 16, 0, '2022-05-28 21:00:06', '2022-05-28 21:00:06');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('7ecedd6b-45aa-4059-97e0-ed28f4fa99d6', '2022-02-08 11:25:55', '2022-05-28 21:00:07', '2022-05-28 21:00:07');
-- C:\Users\Lenny\OneDrive\Music\John Denver\Rocky Mountain High.mp3
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('9495baa2-a19c-4909-ba47-b4b43c690353', 'Rocky Mountain High.mp3', '2022-05-15 12:26:46', '2022-02-08 11:25:55', '774f9487-38cd-4f15-b719-de00102d38a2', '47385b1b-f0b9-454a-a0af-e8d03c4d2036',
        '2022-05-28 21:00:05', 'a34eb424-819c-4332-a835-1db3f2dc1abb', '70bf5f23-f855-4b68-9cd3-f978f05c31d5', '7ecedd6b-45aa-4059-97e0-ed28f4fa99d6', '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82', '2022-05-28 21:00:07',
        '2022-05-28 21:00:07');
-- C:\Users\Lenny\OneDrive\Music\Smashmouth
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('83e11fba-7e09-444f-a7f4-92f32b7c7988', 'Smashmouth', '2022-05-28 21:00:09', '2022-05-15 12:26:46', '2022-05-15 12:26:46', '2e17a3c5-b85e-4b4c-918e-d84dd6c02e29', NULL, '2022-05-28 21:00:09',
        '2022-05-28 21:00:09');
-- C:\Users\Lenny\OneDrive\Music\Smashmouth\Smash Mouth - Walkin- On The Sun.mp3
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('b192c999-0cad-4d63-a5a5-c55dbf87ad13', 3283179, X'a0f5938479c4b9ac0a6b9f659f6170ca', '2022-05-28 21:00:09', '2022-05-28 21:00:09');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('3fa6b5c8-a77e-4531-89e5-2710d3ae224f', 'audio/mpeg', '.mp3', 'MP3File', 'music', 'audio/mpeg', '2022-05-28 21:00:09', '2022-05-28 21:00:09');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "IsVariableBitrate", "SampleRate", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('eb2bbbfa-6688-466b-98c8-739eec150a03', 128000, '{00000055-0000-0010-8000-00AA00389B71}', 0, 44100, 16, 0, '2022-05-28 21:00:10', '2022-05-28 21:00:10');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('f91ca294-043a-41a1-a6b8-675ed8b9a179', '2022-02-08 11:27:18', '2022-05-28 21:00:10', '2022-05-28 21:00:10');
-- C:\Users\Lenny\OneDrive\Music\Smashmouth\Smash Mouth - Walkin- On The Sun.mp3
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('3114452f-5f88-45bd-9bdc-c6b62252987a', 'Smash Mouth - Walkin- On The Sun.mp3', '2022-05-15 12:26:46', '2022-02-08 11:27:18', '83e11fba-7e09-444f-a7f4-92f32b7c7988',
        'b192c999-0cad-4d63-a5a5-c55dbf87ad13', '2022-05-28 21:00:09', '3fa6b5c8-a77e-4531-89e5-2710d3ae224f', 'eb2bbbfa-6688-466b-98c8-739eec150a03', 'f91ca294-043a-41a1-a6b8-675ed8b9a179',
        '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82', '2022-05-28 21:00:11', '2022-05-28 21:00:11');
-- C:\Users\Lenny\OneDrive\Documents\My Shapes
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('2c66ec48-6f48-4de2-b82b-f486e1c0dfdc', 'My Shapes', '2022-05-28 21:00:12', '2022-05-15 12:27:09', '2022-05-28 20:04:51', '21c2d910-d933-4299-b212-2a7162d1becd', NULL, '2022-05-28 21:00:12',
        '2022-05-28 21:00:12');
-- C:\Users\Lenny\OneDrive\Documents\My Shapes\Favorites.vssx
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('6b7ff08f-40bb-4543-89c4-9fd1c2591a1e', 0, X'd41d8cd98f00b204e9800998ecf8427e', '2022-05-28 21:00:12', '2022-05-28 21:00:12');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "CreatedOn", "ModifiedOn")
    VALUES('f0b5038e-4584-47ef-9a17-072dd0dd9309', 'application/vnd.ms-visio.viewer', '.vssx', 'MicrosoftVisioDocument', '2022-05-28 21:00:13', '2022-05-28 21:00:13');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('eb602642-4980-4576-9c19-958d8d1ad4ad', '2020-05-19 18:59:16', '2022-05-28 21:00:13', '2022-05-28 21:00:13');
-- C:\Users\Lenny\OneDrive\Documents\My Shapes\Favorites.vssx
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "DocumentPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('1610babf-82a1-471e-b9ce-fe94dffeff93', 'Favorites.vssx', '2022-05-15 12:27:09', '2020-05-19 18:59:16', '2c66ec48-6f48-4de2-b82b-f486e1c0dfdc', '6b7ff08f-40bb-4543-89c4-9fd1c2591a1e',
        '2022-05-28 21:00:12', 'f0b5038e-4584-47ef-9a17-072dd0dd9309', 'eb602642-4980-4576-9c19-958d8d1ad4ad', '2022-05-28 21:00:14', '2022-05-28 21:00:14');
-- C:\Users\Lenny\OneDrive\Documents\Work
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('ffb9a36a-44f0-479e-97db-b3260c3e693f', 'Work', '2022-05-28 21:00:14', '2022-05-15 12:26:47', '2022-05-15 12:27:14', '21c2d910-d933-4299-b212-2a7162d1becd', NULL, '2022-05-28 21:00:14',
        '2022-05-28 21:00:14');
-- C:\Users\Lenny\OneDrive\Documents\Work\Company_Letterhead-Main.docx
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('15ddf854-97c4-4d8f-aebd-50c9ae79fab0', 61868, X'126233e5be4489930e0f749a0b6b35af', '2022-05-28 21:00:14', '2022-05-28 21:00:14');
INSERT INTO "SummaryPropertySets" ("Id", "ApplicationName", "Author", "Title", "ContentType", "ItemAuthors", "ItemType", "ItemTypeText", "Kind", "CreatedOn", "ModifiedOn")
    VALUES('78831c78-37cf-4ddd-8862-e4bfcad36d38', 'MicrosoftOfficeWord', 'Quesnell,BethA', 'Requesttocancelservice', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'Quesnell,BethA',
        '.docx', 'MicrosoftWordDocument', 'document', '2022-05-28 21:00:15', '2022-05-28 21:00:15');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "LastAuthor", "RevisionNumber", "Security", "CreatedOn", "ModifiedOn")
    VALUES('d81671a6-ef1f-4bfe-8d7d-59e3d20c046e', '2015-08-04 12:51:00', 'LeonardErwine', '2', 0, '2022-05-28 21:00:16', '2022-05-28 21:00:16');
-- C:\Users\Lenny\OneDrive\Documents\Work\Company_Letterhead-Main.docx
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "DocumentPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('762e086b-c570-4825-a26a-4aec5a75ba40', 'Company_Letterhead-Main.docx', '2022-05-15 12:27:14', '2015-08-04 12:51:34', 'ffb9a36a-44f0-479e-97db-b3260c3e693f',
        '15ddf854-97c4-4d8f-aebd-50c9ae79fab0', '2022-05-28 21:00:14', '78831c78-37cf-4ddd-8862-e4bfcad36d38', 'd81671a6-ef1f-4bfe-8d7d-59e3d20c046e', '2022-05-28 21:00:16', '2022-05-28 21:00:16');
-- C:\Users\Lenny\OneDrive\Documents\Work\Old contacts.csv
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('c185a25b-40e3-4850-85c7-b557421e57e5', 3923, X'29d9c2ce7ee88e6ceac261d3a6a08272', '2022-05-28 21:00:16', '2022-05-28 21:00:16');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "Kind", "CreatedOn", "ModifiedOn")
    VALUES('7f4def8b-6abf-4188-ab35-75f64058e0a0', 'application/vnd.ms-excel', '.csv', 'MicrosoftExcelCommaSeparatedValuesFile', 'document', '2022-05-28 21:00:17', '2022-05-28 21:00:17');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('24d868aa-148c-487d-973b-a76624ace68c', '2022-04-26 10:24:12', '2022-05-28 21:00:18', '2022-05-28 21:00:18');
-- C:\Users\Lenny\OneDrive\Documents\Work\Old contacts.csv
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "DocumentPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('6a93f5d9-9933-41d7-bdf5-f72a5bb160e3', 'Old contacts.csv', '2022-05-15 12:26:47', '2022-04-26 10:24:12', 'ffb9a36a-44f0-479e-97db-b3260c3e693f', 'c185a25b-40e3-4850-85c7-b557421e57e5',
        '2022-05-28 21:00:16', '7f4def8b-6abf-4188-ab35-75f64058e0a0', '24d868aa-148c-487d-973b-a76624ace68c', '2022-05-28 21:00:19', '2022-05-28 21:00:19');
-- C:\Users\Lenny\OneDrive\Documents\Work\P2SChildCert.pfx
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('9561bdbf-fe65-44bb-abf1-633383297af3', 3414, X'3a51ede323c5fff6728c03201820378a', '2022-05-28 21:00:19', '2022-05-28 21:00:19');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "CreatedOn", "ModifiedOn")
    VALUES('3235fe5e-c7c5-47ea-8f0c-872049bd393c', 'application/x-pkcs12', '.pfx', 'PersonalInformationExchange', '2022-05-28 21:00:19', '2022-05-28 21:00:19');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('a88691da-49e0-44b3-8a98-76d03f1163ba', '2020-12-29 07:41:01', '2022-05-28 21:00:20', '2022-05-28 21:00:20');
-- C:\Users\Lenny\OneDrive\Documents\Work\P2SChildCert.pfx
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "DocumentPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('4d863270-dbfa-4567-8db5-7e31fb3a9d9b', 'P2SChildCert.pfx', '2022-05-15 12:26:53', '2020-12-29 07:41:01', 'ffb9a36a-44f0-479e-97db-b3260c3e693f', '9561bdbf-fe65-44bb-abf1-633383297af3',
        '2022-05-28 21:00:19', '3235fe5e-c7c5-47ea-8f0c-872049bd393c', 'a88691da-49e0-44b3-8a98-76d03f1163ba', '2022-05-28 21:00:21', '2022-05-28 21:00:21');
-- C:\Users\Lenny\Videos
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('c18cdd08-ccda-4f14-8215-34a1125bbf0a', 'Videos', '2022-05-28 21:00:22', '2022-05-14 14:43:34', '2022-05-27 19:17:25', '8cfb4602-ffd1-488b-99af-78e57aa39dd6', NULL, '2022-05-28 21:00:22',
        '2022-05-28 21:00:22');
-- C:\Users\Lenny\Videos\2015-01
INSERT INTO "Subdirectories" ("Id", "Name", "LastAccessed", "CreationTime", "LastWriteTime", "ParentId", "VolumeId", "CreatedOn", "ModifiedOn")
    VALUES ('d422a6fa-bf2a-4ee1-abeb-f928f7cd2f1a', '2015-01', '2022-05-28 21:00:22', '2022-05-27 19:17:25', '2022-05-27 19:17:25', 'c18cdd08-ccda-4f14-8215-34a1125bbf0a', NULL, '2022-05-28 21:00:22',
        '2022-05-28 21:00:22');
-- C:\Users\Lenny\Videos\2015-01\2301-175155.3gp
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('bad2da5d-6da9-4315-b69f-94f2af159022', 9042101, X'e23b9ef0f0809bb816e4e3ab8e0a1fd4', '2022-05-28 21:00:22', '2022-05-28 21:00:22');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('4cb2e817-ee74-439b-a9c2-eea11443e56d', 'video/3gpp', '.3gp', '3GPFile', 'video', 'video/3gpp', '2022-05-28 21:00:23', '2022-05-28 21:00:23');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('174e042e-8753-47c8-b17a-0978a04608eb', 12840, '{73616D72-767A-494D-B478-F29D25DC9037}', 16, 1, '2022-05-28 21:00:23', '2022-05-28 21:00:23');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('1a5ec501-b5dc-4a2d-a3c4-f8ab33b186a4', '2015-01-23 16:51:54', '2022-05-28 21:00:23', '2022-05-28 21:00:23');
INSERT INTO "VideoPropertySets" ("Id", "Compression", "EncodingBitrate", "FrameHeight", "FrameRate", "FrameWidth", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('51c8ea82-4c7d-4ad2-8172-7826ef219db6', '{33363248-0000-0010-8000-00AA00389B71}', 383712, 144, 14222, 176, 2, '2022-05-28 21:00:24', '2022-05-28 21:00:24');
-- C:\Users\Lenny\Videos\2015-01\2301-175155.3gp
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "VideoPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('e8812e77-f690-4289-a0b3-ad679ce51a7a', '2301-175155.3gp', '2022-05-27 19:17:25', '2015-01-23 16:51:54', 'd422a6fa-bf2a-4ee1-abeb-f928f7cd2f1a', 'bad2da5d-6da9-4315-b69f-94f2af159022',
        '2022-05-28 21:00:22', '4cb2e817-ee74-439b-a9c2-eea11443e56d', '174e042e-8753-47c8-b17a-0978a04608eb', '1a5ec501-b5dc-4a2d-a3c4-f8ab33b186a4', '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82',
        '51c8ea82-4c7d-4ad2-8172-7826ef219db6', '2022-05-28 21:00:25', '2022-05-28 21:00:25');
-- C:\Users\Lenny\Videos\2015-01\2301-180239.3gp
INSERT INTO "BinaryPropertySets" ("Id", "Length", "Hash", "CreatedOn", "ModifiedOn")
    VALUES('733afada-5424-4e47-9576-ec8b756acb95', 14354029, X'2672b88f75431dfc3dc87e18e070d4a6', '2022-05-28 21:00:25', '2022-05-28 21:00:25');
INSERT INTO "SummaryPropertySets" ("Id", "ContentType", "ItemType", "ItemTypeText", "Kind", "MIMEType", "CreatedOn", "ModifiedOn")
    VALUES('a2d7dc39-9c57-4d4d-9ec8-ee9cafe42ae8', 'video/3gpp', '.3gp', '3GPFile', 'video', 'video/3gpp', '2022-05-28 21:00:26', '2022-05-28 21:00:26');
INSERT INTO "AudioPropertySets" ("Id", "EncodingBitrate", "Format", "SampleSize", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('88f13e30-9e0d-4615-be1b-c84cd9cceefd', 12792, '{73616D72-767A-494D-B478-F29D25DC9037}', 16, 1, '2022-05-28 21:00:27', '2022-05-28 21:00:27');
INSERT INTO "DocumentPropertySets" ("Id", "DateCreated", "CreatedOn", "ModifiedOn")
    VALUES('ceee2550-7d15-4601-8017-a688269567ea', '2015-01-23 17:02:38', '2022-05-28 21:00:27', '2022-05-28 21:00:27');
INSERT INTO "VideoPropertySets" ("Id", "Compression", "EncodingBitrate", "FrameHeight", "FrameRate", "FrameWidth", "StreamNumber", "CreatedOn", "ModifiedOn")
    VALUES('44c4ce04-b077-443a-846e-efa3f741745a', '{33363248-0000-0010-8000-00AA00389B71}', 382992, 144, 14221, 176, 2, '2022-05-28 21:00:28', '2022-05-28 21:00:28');
-- C:\Users\Lenny\Videos\2015-01\2301-180239.3gp
INSERT INTO "Files" ("Id", "Name", "CreationTime", "LastWriteTime", "ParentId", "BinaryPropertySetId", "LastHashCalculation", "SummaryPropertySetId", "AudioPropertySetId", "DocumentPropertySetId",
        "DRMPropertySetId", "VideoPropertySetId", "CreatedOn", "ModifiedOn")
    VALUES('2e61b521-4b34-4acd-b813-72032e91343b', '2301-180239.3gp', '2022-05-27 19:17:25', '2015-01-23 17:02:38', 'd422a6fa-bf2a-4ee1-abeb-f928f7cd2f1a', '733afada-5424-4e47-9576-ec8b756acb95',
        '2022-05-28 21:00:25', 'a2d7dc39-9c57-4d4d-9ec8-ee9cafe42ae8', '88f13e30-9e0d-4615-be1b-c84cd9cceefd', 'ceee2550-7d15-4601-8017-a688269567ea', '78c2b1f1-6c9d-47ea-ae99-dbe7e5239b82',
        '44c4ce04-b077-443a-846e-efa3f741745a', '2022-05-28 21:00:29', '2022-05-28 21:00:29');

UPDATE FileSystems SET UpstreamId='88c66219-14a8-45a7-9e8e-d501750fc5d5', LastSynchronizedOn='2022-05-27 22:36:5'1, ModifiedOn='2022-05-27 22:36:51' WHERE Id='bedb396b-2212-4149-9cad-7e437c47314c';
UPDATE Subdirectories SET UpstreamId='b96f5954-086b-4bc3-aa1d-b8e6a8e0f40e', LastSynchronizedOn='2022-05-28 22:25:21', ModifiedOn='2022-05-28 22:25:21' WHERE Id='8b42a599-9e72-4fb0-a7a4-836d05da1400';
UPDATE Subdirectories SET UpstreamId='b82acfa3-f509-4481-9bb9-d987b80155ae', LastSynchronizedOn='2022-05-28 22:25:22', ModifiedOn='2022-05-28 22:25:22' WHERE Id='850b2107-0eb1-4d9c-8834-b829f2cf4ecd';
UPDATE Subdirectories SET UpstreamId='5cb8bbf3-46c7-4334-8dc3-ecd0cb1ad71b', LastSynchronizedOn='2022-05-28 22:25:22', ModifiedOn='2022-05-28 22:25:22' WHERE Id='8cfb4602-ffd1-488b-99af-78e57aa39dd6';
UPDATE Subdirectories SET UpstreamId='8b2aae45-6a84-4d9c-9011-25e7b16f0cc7', LastSynchronizedOn='2022-05-28 22:25:22', ModifiedOn='2022-05-28 22:25:22' WHERE Id='2d138032-8062-4520-b310-42192e893fb5';
UPDATE Subdirectories SET UpstreamId='1be2c8b3-28c4-44e5-82c9-b00d60e1d98c', LastSynchronizedOn='2022-05-28 22:25:24', ModifiedOn='2022-05-28 22:25:24' WHERE Id='21c2d910-d933-4299-b212-2a7162d1becd';
UPDATE Subdirectories SET UpstreamId='71de72ba-f5e1-4731-8a55-da1a4940530d', LastSynchronizedOn='2022-05-28 22:25:25', ModifiedOn='2022-05-28 22:25:25' WHERE Id='98fa6b88-d04b-42aa-9765-c8d0c7822727';
UPDATE BinaryPropertySets SET UpstreamId='751219d4-b1c5-4d00-99aa-e97037d95a61', LastSynchronizedOn='2022-05-28 22:25:26', ModifiedOn='2022-05-28 22:25:26' WHERE Id='3526d6a6-e780-4b57-bc47-ce620b50881c';
