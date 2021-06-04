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
    public class LocalSymbolicNameUnitTest
    {
        private const string TestProperty_Description = "Description";
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("SymbolicName Add/Remove Tests")]
        public void SymbolicNameAddRemoveTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "SymbolicNameAddRemove";
            Local.FileSystem fileSystem = GetVFatFileSystem(dbContext);
            Local.SymbolicName target = new() { Name = expected, FileSystem = fileSystem };
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.SymbolicNames.Add(target);
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
            Assert.AreEqual(expected, target.Name);
            Assert.IsNotNull(target.FileSystem);
            Assert.AreEqual(fileSystem.Id, target.FileSystemId);
            Assert.AreEqual(fileSystem.Id, target.FileSystem.Id);
            Assert.IsTrue(fileSystem.SymbolicNames.Contains(target));
            Assert.AreEqual(0, target.Priority);
            Assert.IsFalse(target.IsInactive);
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

        private static Local.FileSystem GetVFatFileSystem(Local.LocalDbContext dbContext)
        {
            Guid id = Guid.Parse("{53a9e9a4-f5f0-4b4c-9f1e-4e3a80a93cfd}");
            return dbContext.FileSystems.ToList().FirstOrDefault(fs => fs.Id.Equals(id));
        }

        [TestMethod("SymbolicName Name Validation Tests")]
        [TestProperty(TestProperty_Description, "SymbolicName.Name: NVARCHAR(256) NOT NULL CHECK(length(trim(Name)) = length(Name) AND length(Name)>0) UNIQUE COLLATE NOCASE")]
        public void SymbolicNameNameTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "";
            Local.FileSystem fileSystem = GetVFatFileSystem(dbContext);
            Local.SymbolicName target = new() { Name = null, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.Name);
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.SymbolicNames.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Name);

            expected = "SymbolicNameNameTest";
            target.Name = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            target.Name = $" {expected} ";
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            expected = $"{expected} {new string('X', 255 - expected.Length)}";
            target.Name = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            string expected2 = $"{expected}X";
            target.Name = expected2;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameLength, results[0].ErrorMessage);
            entityEntry = dbContext.SymbolicNames.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected2, target.Name);

            target.Name = new string(' ', 257);
            Assert.AreEqual("", target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.SymbolicNames.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual("", target.Name);

            target.Name = "SymbolicNameNameTest2";
            dbContext.SaveChanges();
        }

        [TestMethod("SymbolicName FileSystem Validation Tests")]
        [TestProperty(TestProperty_Description, "SymbolicName.FileSystem: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES FileSystems")]
        public void SymbolicNameFileSystemTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem expected = null;
            Local.SymbolicName target = new() { Name = "SymbolicNameFileSystemTest", FileSystem = expected };
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.SymbolicNames.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.FileSystem), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.FileSystem);

            expected = GetVFatFileSystem(dbContext);
            expected = new() { DisplayName = "SymbolicName Name FileSystem" };
            target.FileSystem = expected;
            dbContext.FileSystems.Add(expected);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.FileSystem);
        }

        [TestMethod("SymbolicName CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "SymbolicName.CreatedOn: CreatedOn<=ModifiedOn")]
        public void SymbolicNameCreatedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = GetVFatFileSystem(dbContext);
            Local.SymbolicName target = new() { Name = "SymbolicName CreatedOn Item", FileSystem = fileSystem };
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.SymbolicNames.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.SymbolicNames.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("SymbolicName LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "SymbolicName.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void SymbolicNameLastSynchronizedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = GetVFatFileSystem(dbContext);
            Local.SymbolicName target = new() { Name = "SymbolicName LastSynchronizedOn Item", FileSystem = fileSystem, UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.SymbolicNames.Add(target);
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
            entityEntry = dbContext.SymbolicNames.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.SymbolicNames.Update(target);
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
            entityEntry = dbContext.SymbolicNames.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.SymbolicNames.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }
    }
}
