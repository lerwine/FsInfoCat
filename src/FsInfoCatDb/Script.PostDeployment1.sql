/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
ALTER TABLE [dbo].[UserAccounts] NOCHECK CONSTRAINT [FK_CreatedByUserAccount];
ALTER TABLE [dbo].[UserAccounts] NOCHECK CONSTRAINT [FK_ModifiedByUserAccount];
:setvar CreatedOn GETDATE();
:setvar Id '00000000-0000-0000-0000-000000000000';
INSERT INTO [dbo].[UserAccounts] (Id, Title, LastName, FirstName, MI, Suffix, DisplayName, ExplicitRoles, IsInactive, Notes, CreatedById, CreatedOn, ModifiedById, ModifiedOn)
    VALUES ('$(Id)', NULL, N'Account', N'System', NULL, NULL, N'System Account', 16, 0, '', '$(Id)', $(CreatedOn), '$(Id)', $(CreatedOn));
ALTER TABLE [dbo].[UserAccounts] CHECK CONSTRAINT [FK_CreatedByUserAccount];
ALTER TABLE [dbo].[UserAccounts] CHECK CONSTRAINT [FK_ModifiedByUserAccount];
INSERT INTO [dbo].[BasicLogins] (Id, UserId, LoginName, PwHash, FailCount, IsInactive, LockedOut, CreatedById, CreatedOn, ModifiedById, ModifiedOn)
    VALUES ('$(Id)', '$(Id)', 'admin', N'pBV6wduugMHi0kTLi4MQ0JNd7N2hJKd9Q4YxA7jKkfprFfbWnWGf8Dm+O6Zlyfct8d4Ar7aiIw+58EDewfgZu9/kuOzJRme4', 0, 0, 0, '$(Id)', $(CreatedOn), '$(Id)', $(CreatedOn));
