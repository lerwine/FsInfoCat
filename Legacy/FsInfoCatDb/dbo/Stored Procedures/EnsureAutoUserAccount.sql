CREATE PROCEDURE [dbo].[EnsureAutoUserAccount]
	@SID VARBINARY(85),
	@UserId SMALLINT,
	@LoginName VARCHAR(128)
WITH EXECUTE AS OWNER
AS BEGIN
	DECLARE @N AS INT;
	DECLARE @ID AS UNIQUEIDENTIFIER = NEWID();
	DECLARE @ModifiedOn AS DATETIME = CURRENT_TIMESTAMP;
	SELECT @N = COUNT(Id) FROM UserAccounts WHERE [SID]=@SID;
	IF @N=0 AND @LoginName=ORIGINAL_LOGIN() BEGIN
		SELECT @N = COUNT([principal_id]) FROM sys.database_principals WHERE [principal_id]=@UserId AND [sid]=@SID AND ([type]='C' OR [type]='E' OR [type]='K' OR [type]='S' OR [type]='U') AND [authentication_type]<>0;
		IF @N=0 THROW 50001, N'Invalid operation', 1
		ELSE BEGIN
			ALTER TABLE [dbo].[UserAccounts] NOCHECK CONSTRAINT [FK_CreatedByUserAccount];
			ALTER TABLE [dbo].[UserAccounts] NOCHECK CONSTRAINT [FK_ModifiedByUserAccount];
			IF IS_ROLEMEMBER('db_denydatareader')=1 BEGIN
				INSERT INTO [UserAccounts] ([LoginName], [SID], [DbPrincipalId], [Id], [ExplicitRoles], [IsInactive], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [DisplayName], [FirstName], [LastName], [MI], [Suffix], [Title], [Notes])
					VALUES (@LoginName, @SID, @UserId, @ID, 0, 0, @ModifiedOn, @ID, @ModifiedOn, @ID, @LoginName, N'', @LoginName, NULL, NULL, NULL, N'');
			END
			ELSE IF IS_ROLEMEMBER('db_owner')=1 OR IS_ROLEMEMBER('access_admin')=1 OR IS_ROLEMEMBER('db_securityadmin')=1 BEGIN
				INSERT INTO [UserAccounts] ([LoginName], [SID], [DbPrincipalId], [Id], [ExplicitRoles], [IsInactive], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [DisplayName], [FirstName], [LastName], [MI], [Suffix], [Title], [Notes])
					VALUES (@LoginName, @SID, @UserId, @ID, 16, 0, @ModifiedOn, @ID, @ModifiedOn, @ID, @LoginName, N'', @LoginName, NULL, NULL, NULL, N'');
			END
			ELSE IF IS_ROLEMEMBER('db_datawriter')=1 BEGIN
				INSERT INTO [UserAccounts] ([LoginName], [SID], [DbPrincipalId], [Id], [ExplicitRoles], [IsInactive], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [DisplayName], [FirstName], [LastName], [MI], [Suffix], [Title], [Notes])
					VALUES (@LoginName, @SID, @UserId, @ID, 4, 0, @ModifiedOn, @ID, @ModifiedOn, @ID, @LoginName, N'', @LoginName, NULL, NULL, NULL, N'');
			END
			ELSE BEGIN
				INSERT INTO [UserAccounts] ([LoginName], [SID], [DbPrincipalId], [Id], [ExplicitRoles], [IsInactive], [CreatedOn], [CreatedById], [ModifiedOn], [ModifiedById], [DisplayName], [FirstName], [LastName], [MI], [Suffix], [Title], [Notes])
					VALUES (@LoginName, @SID, @UserId, @ID, 1, 0, @ModifiedOn, @ID, @ModifiedOn, @ID, @LoginName, N'', @LoginName, NULL, NULL, NULL, N'');
			END
			ALTER TABLE [dbo].[UserAccounts] CHECK CONSTRAINT [FK_CreatedByUserAccount];
			ALTER TABLE [dbo].[UserAccounts] CHECK CONSTRAINT [FK_ModifiedByUserAccount];
			SELECT @ID as [Id], 0 AS [Code], N'' AS [Message]
		END
	END
	ELSE THROW 50001, N'Invalid operation', 1;
END