using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalDbFileUnitTest
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static TestContext _testContext;
#pragma warning restore IDE0052 // Remove unread private members

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestInitialize]
        public void OnTestInitialize()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            TestHelper.UndoChanges(dbContext);
        }

        [TestMethod("DbFile Constructor Tests")]
        [Ignore]
        public void DbFileConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            DbFile target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.CreationTime);
            Assert.AreEqual(target.CreatedOn, target.LastWriteTime);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.AreEqual(string.Empty, target.Name);
            Assert.AreEqual(string.Empty, target.Notes);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.AreEqual(FileCorrelationStatus.Dissociated, target.Status);
            Assert.IsNull(target.Parent);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNotNull(target.BaselineComparisons);
            Assert.AreEqual(0, target.BaselineComparisons.Count);
            Assert.IsNotNull(target.CorrelativeComparisons);
            Assert.AreEqual(0, target.CorrelativeComparisons.Count);
            Assert.IsNotNull(target.PersonalTags);
            Assert.AreEqual(0, target.PersonalTags.Count);
            Assert.IsNotNull(target.SharedTags);
            Assert.AreEqual(0, target.SharedTags.Count);
            Assert.IsNull(target.AudioProperties);
            Assert.IsNull(target.AudioPropertySetId);
            Assert.IsNull(target.BinaryProperties);
            Assert.IsNull(target.BinaryPropertySetId);
            Assert.IsNull(target.DocumentProperties);
            Assert.IsNull(target.DocumentPropertySetId);
            Assert.IsNull(target.DRMProperties);
            Assert.IsNull(target.DRMPropertySetId);
            Assert.IsNull(target.GPSProperties);
            Assert.IsNull(target.GPSPropertySetId);
            Assert.IsNull(target.ImageProperties);
            Assert.IsNull(target.ImagePropertySetId);
            Assert.IsNull(target.LastHashCalculation);
            Assert.IsNull(target.MediaProperties);
            Assert.IsNull(target.MediaPropertySetId);
            Assert.IsNull(target.MusicProperties);
            Assert.IsNull(target.MusicPropertySetId);
            Assert.IsNull(target.PhotoProperties);
            Assert.IsNull(target.PhotoPropertySetId);
            Assert.IsNull(target.RecordedTVProperties);
            Assert.IsNull(target.RecordedTVPropertySetId);
            Assert.IsNull(target.Redundancy);
            Assert.IsNull(target.SummaryProperties);
            Assert.IsNull(target.SummaryPropertySetId);
            Assert.IsNull(target.VideoProperties);
            Assert.IsNull(target.VideoPropertySetId);
        }

        [TestMethod("DbFile Add/Remove Tests")]
        [Ignore]
        public void DbFileAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem fileSystem1 = new() { DisplayName = "Subdirectory Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem1);
            Volume volume1 = new()
            {
                DisplayName = "Subdirectory Add/Remove Item",
                VolumeName = "Subdirectory_Add_Remove_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1
            };
            dbContext.Volumes.Add(volume1);
            string expectedName = "";
            Subdirectory parent1 = new() { Volume = volume1 };
            DbFile target = new() { /* DEFERRED: Initialize properties */ };
            EntityEntry<DbFile> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.Files.Add(target);
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            entityEntry.Reload();

            // DEFERRED: Validate default values
            Assert.IsNull(target.LastAccessed);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            Assert.IsNull(target.LastHashCalculation);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.AreEqual("", target.Notes);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("DbFile Content Validation Tests")]
        [Description("DbFile.Content: UNIQUEIDENTIFIER FOREIGN REFERENCES BinaryProperties")]
        [Ignore]
        public void DbFileBinaryPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            BinaryPropertySet expected = default; // DEFERRED: Set invalid value
            DbFile target = new() { BinaryProperties = expected };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.BinaryProperties), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_BinaryPropertiesRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.BinaryProperties);

            expected = default; // DEFERRED: Set valid value
            target.BinaryProperties = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.BinaryProperties);

            expected = default; // DEFERRED: Set invalid value
            target.BinaryProperties = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.BinaryProperties), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_BinaryPropertiesRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.BinaryProperties);
            dbContext.Files.Remove(target);
        }

        [TestMethod("DbFile Name Validation Tests")]
        [Description("DbFile.Name: NVARCHAR(1024) NOT NULL CHECK(length(trim(Name))>0) COLLATE NOCASE")]
        [Ignore]
        public void DbFileNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            DbFile target = new() { Name = expected };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Name);

            expected = default; // DEFERRED: Set valid value
            target.Name = expected;
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            expected = default; // DEFERRED: Set invalid value
            target.Name = expected;
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Name);
            dbContext.Files.Remove(target);
        }

        [TestMethod("DbFile Parent Validation Tests")]
        [Description("DbFile.Parent: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES Subdirectories")]
        [Ignore]
        public void DbFileParentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Subdirectory expected = default; // DEFERRED: Set invalid value
            DbFile target = new() { Parent = expected };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Parent);

            expected = default; // DEFERRED: Set valid value
            target.Parent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Parent);

            expected = default; // DEFERRED: Set invalid value
            target.Parent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Parent);
            dbContext.Files.Remove(target);
        }

        [TestMethod("DbFile Options Validation Tests")]
        [Description("DbFile.Options: TINYINT  NOT NULL CHECK(Options>=0 AND Options<15)")]
        [Ignore]
        public void DbFileOptionsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileCrawlOptions expected = default; // DEFERRED: Set invalid value
            DbFile target = new() { Options = expected };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Options), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileCrawlOption, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Options);

            expected = default; // DEFERRED: Set valid value
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Options);

            expected = default; // DEFERRED: Set invalid value
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Options), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileCrawlOption, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Options);
        }

        [TestMethod("DbFile CreatedOn Validation Tests")]
        [Description("DbFile.CreatedOn: CreatedOn<=ModifiedOn")]
        [Ignore]
        public void DbFileCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new() {  /* DEFERRED: Initialize properties */ };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Files.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("DbFile LastSynchronizedOn Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description,
            "DbFile.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        [Ignore]
        public void DbFileLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new() {  /* DEFERRED: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Files.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Files.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }
    }
}
