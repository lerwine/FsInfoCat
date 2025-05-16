using FsInfoCat.Local.Model;
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
    public class LocalRedundancyUnitTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void OnTestInitialize()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            TestHelper.UndoChanges(dbContext);
        }

        [TestMethod("Redundancy Constructor Tests"), Ignore]
        public void RedundancyConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            Redundancy target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.File);
            Assert.AreEqual(Guid.Empty, target.FileId);
            Assert.AreEqual(string.Empty, target.Notes);
            Assert.IsNull(target.RedundantSet);
            Assert.AreEqual(Guid.Empty, target.RedundantSetId);
            Assert.AreEqual(string.Empty, target.Reference);
        }

        [TestMethod("Redundancy Add/Remove Tests"), Ignore]
        public void RedundancyAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Redundancy target = new() { /* DEFERRED: Initialize properties */ };
            EntityEntry<Redundancy> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.Redundancies.Add(target);
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();

            // DEFERRED: Validate default values
            Assert.AreEqual("", target.Reference);
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

        [TestMethod("Redundancy Reference Validation Tests"), Ignore]
        [Description("Redundancy.Reference: NVARCHAR(128) NOT NULL COLLATE NOCASE")]
        public void RedundancyReferenceTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Redundancy target = new() { Reference = expected };
            EntityEntry<Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Redundancy.Reference), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Reference);

            expected = default; // DEFERRED: Set valid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Reference);

            expected = default; // DEFERRED: Set invalid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Redundancy.Reference), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Reference);
        }

        [TestMethod("Redundancy RedundantSet Validation Tests"), Ignore]
        [Description("Redundancy.RedundantSet: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES RedundantSets")]
        public void RedundancyRedundantSetTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            RedundantSet expected = default; // DEFERRED: Set invalid value
            Redundancy target = new() { RedundantSet = expected };
            EntityEntry<Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Redundancy.RedundantSet), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_RedundantSetRequired, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.RedundantSet);

            expected = default; // DEFERRED: Set valid value
            target.RedundantSet = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.RedundantSet);

            expected = default; // DEFERRED: Set invalid value
            target.RedundantSet = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Redundancy.RedundantSet), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_RedundantSetRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.RedundantSet);
        }

        [TestMethod("Redundancy File Validation Tests"), Ignore]
        [Description("Redundancy.File: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES DbFiles")]
        public void RedundancyFileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile expected = default; // DEFERRED: Set invalid value
            Redundancy target = new() { File = expected };
            EntityEntry<Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Redundancy.File), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.File);

            expected = default; // DEFERRED: Set valid value
            target.File = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.File);

            expected = default; // DEFERRED: Set invalid value
            target.File = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Redundancy.File), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.File);
        }

        [TestMethod("Redundancy CreatedOn Validation Tests"), Ignore]
        [Description("Redundancy.CreatedOn: CreatedOn<=ModifiedOn")]
        public void RedundancyCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Redundancy target = new() {  /* DEFERRED: Initialize properties */ };
            EntityEntry<Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Redundancy.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());

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
            entityEntry = dbContext.Redundancies.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("Redundancy LastSynchronizedOn Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description,
            "Redundancy.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void RedundancyLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Redundancy target = new() {  /* DEFERRED: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Redundancy> entityEntry = dbContext.Redundancies.Add(target);
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
            entityEntry = dbContext.Redundancies.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Redundancies.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }
    }
}
