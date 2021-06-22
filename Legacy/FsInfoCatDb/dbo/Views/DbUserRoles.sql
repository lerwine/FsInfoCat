CREATE VIEW [dbo].[DbUserRoles] AS
SELECT [r].[name] AS [RoleName], [u].[name] AS [UserName], [u].[sid] AS [SID], [u].[principal_id] AS PrincipalId
	FROM sys.database_role_members AS [m]
	RIGHT OUTER JOIN sys.database_principals AS [u] ON [m].[member_principal_id] = [u].[principal_id]
	LEFT OUTER JOIN sys.database_principals AS [r] ON [m].[role_principal_id] = [r].[principal_id]
WHERE [r].[type] = 'R' AND ([u].[type]='C' OR [u].[type]='E' OR [u].[type]='K' OR [u].[type]='S' OR [u].[type]='U') AND [u].[authentication_type]<>0;
--	AS SELECT [name], [principal_id], [sid] FROM sys.database_principals WHERE ([type]='C' OR [type]='E' OR [type]='K' OR [type]='S' OR [type]='U') AND [authentication_type]<>0