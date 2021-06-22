CREATE PROCEDURE [dbo].[GetAutoUserAccountId]
AS BEGIN
	DECLARE @N INT;
	DECLARE @UserSid AS VARBINARY(85) = USER_SID();
	DECLARE @ID SMALLINT = USER_ID();
	DECLARE @OriginalLogin VARCHAR = ORIGINAL_LOGIN();
	SELECT @N = COUNT(Id) FROM UserAccounts WHERE [SID]=@UserSid;
	IF @N=0 BEGIN
		SELECT @N = COUNT([principal_id]) FROM sys.database_principals WHERE [sid]=@UserSid AND ([type]='C' OR [type]='E' OR [type]='K' OR [type]='S' OR [type]='U') AND [authentication_type]<>0;
		IF @N=0 SELECT NULL as [Id], 1 AS [Code], N'No matching database principal found' AS [Message]
		ELSE BEGIN
			SELECT @N = COUNT(Id) FROM UserAccounts WHERE [DbPrincipalId]=@ID;
			IF @N=0 BEGIN
				EXEC [dbo].[EnsureAutoUserAccount] @SID = @UserSid, @UserId = @ID, @LoginName = @OriginalLogin
			END
			ELSE SELECT NULL as [Id], 2 AS [Code], N'Duplicate Principal ID found' AS [Message]
		END
	END
	ELSE SELECT [Id], 0 AS [Code], N'' AS [Message] FROM [UserAccounts] WHERE [SID]=@UserSid
END