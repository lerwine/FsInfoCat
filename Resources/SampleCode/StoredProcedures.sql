CREATE PROCEDURE [dbo].[EnsureAutoUserAccount]
	@SID VARBINARY(85),
	@UserId SMALLINT,
	@LoginName VARCHAR(128)
WITH EXECUTE AS OWNER
AS BEGIN
	DECLARE @N AS INT;
	DECLARE @ID AS UNIQUEIDENTIFIER = NEWID();
	DECLARE @ModifiedOn AS DATETIME = CURRENT_TIMESTAMP;
	SELECT @N = COUNT(Id) FROM UserProfiles WHERE [SID]=@SID;
	IF @N=0 AND @LoginName=ORIGINAL_LOGIN() BEGIN
		SELECT @N = COUNT([principal_id]) FROM sys.database_principals WHERE [principal_id]=@UserId AND [sid]=@SID AND ([type]='C' OR [type]='E' OR [type]='K' OR [type]='S' OR [type]='U') AND [authentication_type]<>0;
		IF @N=0 THROW 50001, N'Invalid operation', 1
		ELSE BEGIN
			ALTER TABLE [dbo].[UserProfiles] NOCHECK CONSTRAINT [FK_CreatedByUserProfile];
			ALTER TABLE [dbo].[UserProfiles] NOCHECK CONSTRAINT [FK_ModifiedByUserProfile];
			IF IS_ROLEMEMBER('db_denydatareader')=1 BEGIN
				INSERT INTO [UserProfiles] ([LoginName], [SID], [DbPrincipalId], [Id], [ExplicitRoles], [IsInactive], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [DisplayName], [FirstName], [LastName], [MI], [Suffix], [Title], [Notes])
					VALUES (@LoginName, @SID, @UserId, @ID, 0, 0, @ModifiedOn, @ID, @ModifiedOn, @ID, @LoginName, N'', @LoginName, NULL, NULL, NULL, N'');
			END
			ELSE IF IS_ROLEMEMBER('db_owner')=1 OR IS_ROLEMEMBER('access_admin')=1 OR IS_ROLEMEMBER('db_securityadmin')=1 BEGIN
				INSERT INTO [UserProfiles] ([LoginName], [SID], [DbPrincipalId], [Id], [ExplicitRoles], [IsInactive], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [DisplayName], [FirstName], [LastName], [MI], [Suffix], [Title], [Notes])
					VALUES (@LoginName, @SID, @UserId, @ID, 16, 0, @ModifiedOn, @ID, @ModifiedOn, @ID, @LoginName, N'', @LoginName, NULL, NULL, NULL, N'');
			END
			ELSE IF IS_ROLEMEMBER('db_datawriter')=1 BEGIN
				INSERT INTO [UserProfiles] ([LoginName], [SID], [DbPrincipalId], [Id], [ExplicitRoles], [IsInactive], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [DisplayName], [FirstName], [LastName], [MI], [Suffix], [Title], [Notes])
					VALUES (@LoginName, @SID, @UserId, @ID, 4, 0, @ModifiedOn, @ID, @ModifiedOn, @ID, @LoginName, N'', @LoginName, NULL, NULL, NULL, N'');
			END
			ELSE BEGIN
				INSERT INTO [UserProfiles] ([LoginName], [SID], [DbPrincipalId], [Id], [ExplicitRoles], [IsInactive], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [DisplayName], [FirstName], [LastName], [MI], [Suffix], [Title], [Notes])
					VALUES (@LoginName, @SID, @UserId, @ID, 1, 0, @ModifiedOn, @ID, @ModifiedOn, @ID, @LoginName, N'', @LoginName, NULL, NULL, NULL, N'');
			END
			ALTER TABLE [dbo].[UserProfiles] CHECK CONSTRAINT [FK_CreatedByUserProfile];
			ALTER TABLE [dbo].[UserProfiles] CHECK CONSTRAINT [FK_ModifiedByUserProfile];
			SELECT @ID as [Id], 0 AS [Code], N'' AS [Message]
		END
	END
	ELSE THROW 50001, N'Invalid operation', 1;
END
GO
CREATE PROCEDURE [dbo].[GetAutoUserAccountId]
AS BEGIN
	DECLARE @N INT;
	DECLARE @UserSid AS VARBINARY(85) = USER_SID();
	DECLARE @ID SMALLINT = USER_ID();
	DECLARE @OriginalLogin VARCHAR = ORIGINAL_LOGIN();
	SELECT @N = COUNT(Id) FROM UserProfiles WHERE [SID]=@UserSid;
	IF @N=0 BEGIN
		SELECT @N = COUNT([principal_id]) FROM sys.database_principals WHERE [sid]=@UserSid AND ([type]='C' OR [type]='E' OR [type]='K' OR [type]='S' OR [type]='U') AND [authentication_type]<>0;
		IF @N=0 SELECT NULL as [Id], 1 AS [Code], N'No matching database principal found' AS [Message]
		ELSE BEGIN
			SELECT @N = COUNT(Id) FROM UserProfiles WHERE [DbPrincipalId]=@ID;
			IF @N=0 BEGIN
				EXEC [dbo].[EnsureAutoUserAccount] @SID = @UserSid, @UserId = @ID, @LoginName = @OriginalLogin
			END
			ELSE SELECT NULL as [Id], 2 AS [Code], N'Duplicate Principal ID found' AS [Message]
		END
	END
	ELSE SELECT [Id], 0 AS [Code], N'' AS [Message] FROM [UserProfiles] WHERE [SID]=@UserSid
END
GO
Create PROCEDURE [dbo].[GetApplicationProperty]
	@Name SYSNAME
WITH EXECUTE AS OWNER
AS
BEGIN
	IF @Name IS NULL BEGIN
		SELECT RIGHT([name], LEN([name]) - 46) AS [name], [value] FROM fn_listextendedproperty(default, N'Schema', 'dbo', default, default, default, default) WHERE [name] LIKE 'urn:uuid:6ba7b811-ffff-3fff-80b4-00c04fd430c8:%';
	END ELSE BEGIN
		DECLARE @PName SYSNAME = 'urn:uuid:6ba7b811-ffff-3fff-80b4-00c04fd430c8:' + @Name;
		SELECT RIGHT([name], LEN([name]) - 46) AS [name], [value] FROM fn_listextendedproperty(default, N'Schema', 'dbo', default, default, default, default) WHERE [name] = @PName;
	END
END
GO
CREATE PROCEDURE [dbo].[SetMaxRecursionDepth]
	@Value INT
WITH EXECUTE AS OWNER
AS
BEGIN
	DECLARE @PName SYSNAME = 'urn:uuid:6ba7b811-ffff-3fff-80b4-00c04fd430c8:MaxRecursionDepth';
	DECLARE @Count INT;
	SELECT @Count = COUNT([objname]) FROM fn_listextendedproperty(default, N'Schema', 'dbo', default, default, default, default) WHERE [name] = @PName;
	IF @Count = 0 BEGIN
		EXEC sp_addextendedproperty @name = @PName, @Value = 32, @level0type = N'Schema', @level0name = 'dbo';
	END ELSE BEGIN
		EXEC sp_updateextendedproperty @name = @PName, @Value = 32, @level0type = N'Schema', @level0name = 'dbo';
	END
END

