using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalFileSystemUnitTest
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("FileSystem Add/Remove Tests")]
        public void FileSystemAddRemoveTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "FileSystem Add/Remove Item";
            Local.FileSystem target = new() { DisplayName = expected };
            EntityEntry<Local.FileSystem> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.FileSystems.Add(target);
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
            Assert.AreEqual(expected, target.DisplayName);
            Assert.IsNull(target.DefaultDriveType);
            Assert.AreEqual(255, target.MaxNameLength);
            Assert.IsFalse(target.CaseSensitiveSearch);
            Assert.IsFalse(target.IsInactive);
            Assert.IsFalse(target.ReadOnly);
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

        [TestMethod("FileSystem DefaultDriveType Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "FileSystem.DefaultDriveType: CHECK(DefaultDriveType IS NULL OR (DefaultDriveType>=0 AND DefaultDriveType<7))")]
        public void FileSystemDefaultDriveTypeTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            DriveType expected = (DriveType)(object)-1;
            Local.FileSystem target = new() { DefaultDriveType = expected, DisplayName = "FileSystem DefaultDriveType Item" };
            EntityEntry<Local.FileSystem> entityEntry = dbContext.FileSystems.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DefaultDriveType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.DefaultDriveType);

            expected = DriveType.Fixed;
            target.DefaultDriveType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DefaultDriveType);

            expected = (DriveType)(object)7;
            target.DefaultDriveType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DefaultDriveType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.DefaultDriveType);

            target.DefaultDriveType = DriveType.Fixed;
            dbContext.SaveChanges();
        }

        [TestMethod("FileSystem DisplayName Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "FileSystem: DisplayName NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName)) = length(DisplayName) AND length(DisplayName)>0) UNIQUE COLLATE NOCASE")]
        public void FileSystemDisplayNameTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "";
            Local.FileSystem target = new() { DisplayName = null };
            Assert.AreEqual(expected, target.DisplayName);
            EntityEntry<Local.FileSystem> entityEntry = dbContext.FileSystems.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.DisplayName);

            expected = "FileSystem DisplayName Item";
            target.DisplayName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);

            target.DisplayName = $" {expected} ";
            Assert.AreEqual(expected, target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);

            expected = $"{expected} {new string('X', 1023 - expected.Length)}";
            target.DisplayName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);

            string expected2 = $"{expected}X";
            target.DisplayName = expected2;
            Assert.AreEqual(expected2, target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected2, target.DisplayName);

            target.DisplayName = new string(' ', 1025);
            Assert.AreEqual("", target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual("", target.DisplayName);

            target.DisplayName = expected = "FileSystem DisplayName Item";
            dbContext.SaveChanges();

            target = new() { DisplayName = expected };
            entityEntry = dbContext.FileSystems.Add(target);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateDisplayName, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            Assert.AreEqual(expected, target.DisplayName);

            target.DisplayName = $"{expected} 2";
            dbContext.SaveChanges();
        }

        [TestMethod("FileSystem MaxNameLength Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "FileSystem: MaxNameLength CHECK(MaxNameLength IS NULL OR MaxNameLength>=0)")]
        public void FileSystemMaxNameLengthTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            int expected = 0;
            Local.FileSystem target = new() { DisplayName = "FileSystem MaxNameLength Item", MaxNameLength = expected };
            EntityEntry<Local.FileSystem> entityEntry = dbContext.FileSystems.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.MaxNameLength), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.MaxNameLength);

            expected = 1;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.MaxNameLength);

            expected = int.MaxValue;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.MaxNameLength);

            expected = -1;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.MaxNameLength), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.MaxNameLength);

            target.MaxNameLength = 255;
            dbContext.SaveChanges();
        }

        [TestMethod("FileSystem CreatedOn Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "FileSystem.CreatedOn: CreatedOn<=ModifiedOn")]
        public void FileSystemCreatedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem target = new() { DisplayName = "FileSystem CreatedOn FileSystem" };
            EntityEntry<Local.FileSystem> entityEntry = dbContext.FileSystems.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.FileSystems.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("FileSystem LastSynchronizedOn Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description,
            "FileSystem.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void FileSystemLastSynchronizedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem target = new() { DisplayName = "FileSystem LastSynchronizedOn FileSystem", UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.FileSystem> entityEntry = dbContext.FileSystems.Add(target);
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
            entityEntry = dbContext.FileSystems.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.FileSystems.Update(target);
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
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }
    }
}
