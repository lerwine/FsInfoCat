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
    public class LocalFileComparisonUnitTest
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("FileComparison Add/Remove Tests")]
        public void FileComparisonAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileComparison target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.FileComparison> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.Comparisons.Add(target);
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();

            // TODO: Validate default values
            Assert.IsFalse(target.AreEqual);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("FileComparison SourceFile Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "FileComparison.SourceFile: UNIQUEIDENTIFIER NOT NULL (SourceFileId<>TargetFileId) FOREIGN REFERENCES DbFiles")]
        public void FileComparisonSourceFileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.DbFile expected = default; // TODO: Set invalid value
            Local.FileComparison target = new() { SourceFile = expected };
            EntityEntry<Local.FileComparison> entityEntry = dbContext.Comparisons.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.SourceFile), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_SourceFileRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.SourceFile);

            expected = default; // TODO: Set valid value
            target.SourceFile = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.SourceFile);

            expected = default; // TODO: Set invalid value
            target.SourceFile = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.SourceFile), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_SourceFileRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Comparisons.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.SourceFile);
        }

        [TestMethod("FileComparison TargetFile Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "FileComparison.TargetFile: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES DbFiles")]
        public void FileComparisonTargetFileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.DbFile expected = default; // TODO: Set invalid value
            Local.FileComparison target = new() { TargetFile = expected };
            EntityEntry<Local.FileComparison> entityEntry = dbContext.Comparisons.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.TargetFile), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_TargetFileRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.TargetFile);

            expected = default; // TODO: Set valid value
            target.TargetFile = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.TargetFile);

            expected = default; // TODO: Set invalid value
            target.TargetFile = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.TargetFile), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_TargetFileRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Comparisons.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.TargetFile);
        }

        [TestMethod("FileComparison CreatedOn Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "FileComparison.CreatedOn: CreatedOn<=ModifiedOn")]
        public void FileComparisonCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileComparison target = new() {  /* TODO: Initialize properties */ };
            EntityEntry<Local.FileComparison> entityEntry = dbContext.Comparisons.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.Comparisons.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("FileComparison LastSynchronizedOn Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description,
            "FileComparison.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void FileComparisonLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileComparison target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.FileComparison> entityEntry = dbContext.Comparisons.Add(target);
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
            entityEntry = dbContext.Comparisons.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Comparisons.Update(target);
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
            entityEntry = dbContext.Comparisons.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Comparisons.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }
    }
}
