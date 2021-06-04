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
        private const string TestProperty_Description = "Description";
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("DbFile Add/Remove Tests")]
        public void DbFileAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem1 = new() { DisplayName = "Subdirectory Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem1);
            Local.Volume volume1 = new()
            {
                DisplayName = "Subdirectory Add/Remove Item",
                VolumeName = "Subdirectory_Add_Remove_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1
            };
            dbContext.Volumes.Add(volume1);
            string expectedName = "";
            Local.Subdirectory parent1 = new() { Volume = volume1 };
            Local.DbFile target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.DbFile> entityEntry = dbContext.Entry(target);
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

            // TODO: Validate default values
            Assert.IsNull(target.LastAccessed);
            Assert.IsNull(target.Deleted);
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
        [TestProperty(TestProperty_Description, "DbFile.Content: UNIQUEIDENTIFIER FOREIGN REFERENCES ContentInfos")]
        public void DbFileContentInfoTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ContentInfo expected = default; // TODO: Set invalid value
            Local.DbFile target = new() { Content = expected };
            EntityEntry<Local.DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Content), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ContentInfoRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Content);

            expected = default; // TODO: Set valid value
            target.Content = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Content);

            expected = default; // TODO: Set invalid value
            target.Content = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Content), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ContentInfoRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Content);
            dbContext.Files.Remove(target);
        }

        [TestMethod("DbFile Name Validation Tests")]
        [TestProperty(TestProperty_Description, "DbFile.Name: NVARCHAR(1024) NOT NULL CHECK(length(trim(Name))>0) COLLATE NOCASE")]
        public void DbFileNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // TODO: Set invalid value
            Local.DbFile target = new() { Name = expected };
            EntityEntry<Local.DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Name);

            expected = default; // TODO: Set valid value
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

            expected = default; // TODO: Set invalid value
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
        [TestProperty(TestProperty_Description, "DbFile.Parent: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES Subdirectories")]
        public void DbFileParentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.Subdirectory expected = default; // TODO: Set invalid value
            Local.DbFile target = new() { Parent = expected };
            EntityEntry<Local.DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Parent);

            expected = default; // TODO: Set valid value
            target.Parent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Parent);

            expected = default; // TODO: Set invalid value
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
        [TestProperty(TestProperty_Description, "DbFile.Options: TINYINT  NOT NULL CHECK(Options>=0 AND Options<15)")]
        public void DbFileOptionsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            FileCrawlOptions expected = default; // TODO: Set invalid value
            Local.DbFile target = new() { Options = expected };
            EntityEntry<Local.DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Options), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileCrawlOption, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Options);

            expected = default; // TODO: Set valid value
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Options);

            expected = default; // TODO: Set invalid value
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
        [TestProperty(TestProperty_Description, "DbFile.CreatedOn: CreatedOn<=ModifiedOn")]
        public void DbFileCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.DbFile target = new() {  /* TODO: Initialize properties */ };
            EntityEntry<Local.DbFile> entityEntry = dbContext.Files.Add(target);
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
        [TestProperty(TestProperty_Description,
            "DbFile.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void DbFileLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.DbFile target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.DbFile> entityEntry = dbContext.Files.Add(target);
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
