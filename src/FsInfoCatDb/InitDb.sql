ALTER TABLE [dbo].[UserAccounts] NOCHECK CONSTRAINT [FK_CreatedByUserAccount];
ALTER TABLE [dbo].[UserAccounts] NOCHECK CONSTRAINT [FK_ModifiedByUserAccount];
DECLARE @CreatedOn DATETIME = GETDATE();
DECLARE @Id UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';
INSERT INTO [dbo].[UserAccounts] (Id, Title, LastName, FirstName, MI, Suffix, DisplayName, ExplicitRoles, IsInactive, Notes, CreatedById, CreatedOn, ModifiedById, ModifiedOn)
    VALUES (@Id, NULL, N'Account', N'System', NULL, NULL, N'System Account', 16, 0, '', @Id, @CreatedOn, @Id, @CreatedOn);
ALTER TABLE [dbo].[UserAccounts] CHECK CONSTRAINT [FK_CreatedByUserAccount];
ALTER TABLE [dbo].[UserAccounts] CHECK CONSTRAINT [FK_ModifiedByUserAccount];
-- Sets password to 'P@ssw0rd123!@#'
INSERT INTO [dbo].[BasicLogins] (Id, UserId, LoginName, PwHash, FailCount, IsInactive, LockedOut, CreatedById, CreatedOn, ModifiedById, ModifiedOn)
    VALUES (@Id, @Id, 'admin', N'pBV6wduugMHi0kTLi4MQ0JNd7N2hJKd9Q4YxA7jKkfprFfbWnWGf8Dm+O6Zlyfct8d4Ar7aiIw+58EDewfgZu9/kuOzJRme4', 0, 0, 0, @Id, @CreatedOn, @Id, @CreatedOn);
