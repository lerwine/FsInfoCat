using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalDbUnitTest
    {
        private const string TestProperty_Description = "Description";
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
            if (Services.ServiceProvider is null)
            {
                string path = Services.GetAppDataPath(typeof(LocalDbUnitTest).Assembly, AppDataPathLevel.Application);
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                Services.Initialize(services => Local.LocalDbContext.ConfigureServices(services, typeof(LocalDbUnitTest).Assembly, null));
            }
        }

        [ClassCleanup]
        public static void OnClassCleanup()
        {
            string path = Services.GetAppDataPath(typeof(LocalDbUnitTest).Assembly, AppDataPathLevel.Application);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        [TestInitialize]
        public void OnTestInitialize()
        {
            //XDocument document = XDocument.Parse(Local.Properties.Resources.DbCommands);
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            dbContext.ChangeTracker.Clear();
            //using var transaction = dbContext.Database.BeginTransaction();
            //try
            //{
            //    foreach (XElement element in document.Root.Elements("DropTables").Elements("Text"))
            //        dbContext.Database.ExecuteSqlRaw(element.Value);
            //    transaction.Commit();
            //}
            //catch
            //{
            //    transaction.Rollback();
            //}
        }

        #region FileSystem Tests

        [TestMethod("FileSystem Add/Remove Tests")]
        public void FileSystemAddRemoveTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "FileSystem Add/Remove Item";
            Local.FileSystem target = new() { DisplayName = expected };
            EntityEntry<Local.FileSystem> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.FileSystems.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
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
            Assert.AreEqual(entityEntry.State, EntityState.Deleted);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
        }

        [TestMethod("FileSystem DefaultDriveType Validation Tests")]
        [TestProperty(TestProperty_Description, "FileSystem.DefaultDriveType: CHECK(DefaultDriveType IS NULL OR (DefaultDriveType>=0 AND DefaultDriveType<7))")]
        public void FileSystemDefaultDriveTypeTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            DriveType expected = (DriveType)(object)-1;
            Local.FileSystem target = new() { DefaultDriveType = expected, DisplayName = "FileSystem DefaultDriveType Item" };
            EntityEntry<Local.FileSystem> entityEntry = dbContext.FileSystems.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DefaultDriveType), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.DefaultDriveType);

            expected = DriveType.Fixed;
            target.DefaultDriveType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DefaultDriveType);

            expected = (DriveType)(object)7;
            target.DefaultDriveType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DefaultDriveType), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.DefaultDriveType);

            target.DefaultDriveType = DriveType.Fixed;
            dbContext.SaveChanges();
        }

        [TestMethod("FileSystem DisplayName Validation Tests")]
        [TestProperty(TestProperty_Description, "FileSystem: DisplayName NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName)) = length(DisplayName) AND length(DisplayName)>0) UNIQUE COLLATE NOCASE")]
        public void FileSystemDisplayNameTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "";
            Local.FileSystem target = new() { DisplayName = null };
            Assert.AreEqual(expected, target.DisplayName);
            EntityEntry<Local.FileSystem> entityEntry = dbContext.FileSystems.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.DisplayName);

            expected = "FileSystem DisplayName Item";
            target.DisplayName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);

            target.DisplayName = $" {expected} ";
            Assert.AreEqual(expected, target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);

            expected = $"{expected} {new string('X', 1023 - expected.Length)}";
            target.DisplayName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);

            string expected2 = $"{expected}X";
            target.DisplayName = expected2;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DisplayNameLength, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected2, target.DisplayName);

            target.DisplayName = new string(' ', 1025);
            Assert.AreEqual("", target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual("", target.DisplayName);

            target.DisplayName = "FileSystem DisplayName Item";

            target = new() { DisplayName = expected };
            entityEntry = dbContext.FileSystems.Add(target);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DuplicateDisplayName, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Assert.AreEqual(expected, target.DisplayName);

            target.DisplayName = "FileSystem DisplayName Item 2";
            dbContext.SaveChanges();
        }

        [TestMethod("FileSystem MaxNameLength Validation Tests")]
        [TestProperty(TestProperty_Description, "FileSystem: MaxNameLength CHECK(MaxNameLength IS NULL OR MaxNameLength>=0)")]
        public void FileSystemMaxNameLengthTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            int expected = 0;
            Local.FileSystem target = new() { DisplayName = "FileSystem MaxNameLength Item", MaxNameLength = expected };
            EntityEntry <Local.FileSystem> entityEntry = dbContext.FileSystems.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.MaxNameLength), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_MaxNameLengthInvalid, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.MaxNameLength);

            expected = 1;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.MaxNameLength);

            expected = int.MaxValue;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.MaxNameLength);

            expected = -1;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.MaxNameLength), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_MaxNameLengthInvalid, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.MaxNameLength);

            target.MaxNameLength = 255;
            dbContext.SaveChanges();
        }

        [TestMethod("FileSystem CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "FileSystem.CreatedOn: CreatedOn<=ModifiedOn")]
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.FileSystems.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("FileSystem LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "FileSystem.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void FileSystemLastSynchronizedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem target = new() { DisplayName = "FileSystem LastSynchronizedOn FileSystem", UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.FileSystem> entityEntry = dbContext.FileSystems.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.FileSystems.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.FileSystems.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.FileSystems.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

        #region SymbolicName Tests

        [TestMethod("SymbolicName Add/Remove Tests")]
        public void SymbolicNameAddRemoveTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "SymbolicNameAddRemove";
            Local.FileSystem fileSystem = new() { DisplayName = "SymbolicName Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.SymbolicName target = new() { Name = expected, FileSystem = fileSystem };
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.SymbolicNames.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
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
            Assert.AreEqual(entityEntry.State, EntityState.Deleted);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
        }

        [TestMethod("SymbolicName Name Validation Tests")]
        [TestProperty(TestProperty_Description, "SymbolicName.Name: NVARCHAR(256) NOT NULL CHECK(length(trim(Name)) = length(Name) AND length(Name)>0) UNIQUE COLLATE NOCASE")]
        public void SymbolicNameNameTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "";
            Local.FileSystem fileSystem = new() { DisplayName = "SymbolicName Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.SymbolicName target = new() { Name = null, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.Name);
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.SymbolicNames.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.Name), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Name);

            expected = "SymbolicNameNameTest";
            target.Name = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            target.Name = $" {expected} ";
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            expected = $"{expected} {new string('X', 255 - expected.Length)}";
            target.Name = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            string expected2 = $"{expected}X";
            target.Name = expected2;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.Name), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_NameLength, results[0].ErrorMessage);
            entityEntry = dbContext.SymbolicNames.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected2, target.Name);

            target.Name = new string(' ', 257);
            Assert.AreEqual("", target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.Name), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.SymbolicNames.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual("", target.Name);

            target.Name = "SymbolicNameNameTest";
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.FileSystem), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_FileSystemRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.FileSystem);

            expected = new() { DisplayName = "SymbolicName Add/Remove FileSystem" };
            target.FileSystem = expected;
            dbContext.FileSystems.Add(expected);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.FileSystem);

            target.FileSystemId = expected.Id;
            dbContext.FileSystems.Remove(expected);
            target.FileSystem = null;
            Assert.AreEqual(Guid.Empty, target.FileSystemId);
            target.FileSystemId = expected.Id;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.FileSystem), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_FileSystemRequired, results[0].ErrorMessage);
            entityEntry = dbContext.SymbolicNames.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.IsNull(target.FileSystem);
            Assert.AreEqual(target.FileSystemId, expected.Id);

            expected = new() { DisplayName = "SymbolicName Add/Remove FileSystem" };
            target.FileSystem = expected;
            dbContext.SaveChanges();
        }

        [TestMethod("SymbolicName CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "SymbolicName.CreatedOn: CreatedOn<=ModifiedOn")]
        public void SymbolicNameCreatedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = new() { DisplayName = "SymbolicName Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.SymbolicName target = new() {  Name = "SymbolicName CreatedOn Item", FileSystem = fileSystem };
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.SymbolicNames.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
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
            Local.SymbolicName target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.SymbolicNames.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.SymbolicNames.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.SymbolicNames.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.SymbolicNames.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.SymbolicName.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.SymbolicNames.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

        #region Volume Tests

        [TestMethod("Volume Add/Remove Tests")]
        public void VolumeAddRemoveTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string displayName = "Volume Add/Remove Item", volumeName = "Volume_Add_Remove_Name", identifier = "VolumeAddRemoveIdentifier";
            Local.FileSystem fileSystem = new() { DisplayName = "Volume Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            EntityEntry<Local.Volume> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.Volumes.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            entityEntry.Reload();
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);
            Assert.AreEqual(fileSystem.Id, target.FileSystemId);
            Assert.IsNotNull(target.FileSystem);
            Assert.AreEqual(fileSystem.Id, target.FileSystem.Id);
            Assert.AreEqual(DriveType.Unknown, target.Type);
            Assert.IsNull(target.CaseSensitiveSearch);
            Assert.IsNull(target.ReadOnly);
            Assert.AreEqual(VolumeStatus.Unknown, target.Status);
            Assert.IsNull(target.MaxNameLength);
            Assert.AreEqual("", target.Notes);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(entityEntry.State, EntityState.Deleted);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
        }

        [TestMethod("Volume Identifier Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.Identifier: NVARCHAR(1024) NOT NULL CHECK(length(trim(Identifier))>0) UNIQUE COLLATE NOCASE")]
        public void VolumeIdentifierTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string displayName = "Volume Identifier Item", volumeName = "Volume_Identifier_Test";
            string expected = "";
            Local.FileSystem fileSystem = new() { DisplayName = "Volume Identifier FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = null, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.Identifier);
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_IdentifierRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);

            expected = "VolumeIdentifierTest";
            target.Identifier = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Identifier);

            target.Identifier = $" {expected} ";
            Assert.AreEqual(expected, target.Identifier);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);

            expected = $"{expected} {new string('X', 1023 - expected.Length)}";
            target.Identifier = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);

            string expected2 = $"{expected}X";
            target.Identifier = expected2;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_IdentifierLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected2, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);

            target.Identifier = new string(' ', 1025);
            Assert.AreEqual("", target.Identifier);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_IdentifierLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual("", target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            dbContext.Volumes.Remove(target);

            fileSystem = new() { DisplayName = "Volume Identifier FileSystem 2" };
            dbContext.FileSystems.Add(fileSystem);
            target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = expected, FileSystem = fileSystem };
            entityEntry = dbContext.Volumes.Add(target);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DuplicateIdentifierName, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Assert.AreEqual(expected, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
        }

        [TestMethod("Volume VolumeName Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.VolumeName: NVARCHAR(128) NOT NULL CHECK(length(trim(VolumeName))>0) COLLATE NOCASE")]
        public void VolumeVolumeNameTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "";
            string displayName = "Volume VolumeName Item", identifier = "VolumeVolumeNameIdentifier";
            Local.FileSystem fileSystem = new() { DisplayName = "Volume Name FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume target = new() { DisplayName = displayName, VolumeName = null, Identifier = identifier, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.VolumeName);
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.VolumeName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_VolumeNameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            expected = "Volume_VolumeName";
            target.VolumeName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            target.VolumeName = $" {expected} ";
            Assert.AreEqual(expected, target.VolumeName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            expected = $"{expected}_{new string('X', 127 - expected.Length)}";
            target.VolumeName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            string expected2 = $"{expected}X";
            target.VolumeName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.VolumeName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_VolumeNameLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected2, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            target.VolumeName = new string(' ', 129);
            Assert.AreEqual("", target.VolumeName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.VolumeName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_VolumeNameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual("", target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            fileSystem = new() { DisplayName = "Volume Name FileSystem 2" };
            dbContext.FileSystems.Add(fileSystem);
            target = new() { DisplayName = displayName, VolumeName = expected, Identifier = $"{identifier}2", FileSystem = fileSystem };
            entityEntry = dbContext.Volumes.Add(target);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);
        }

        [TestMethod("Volume DisplayName Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.DisplayName: NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName))>0) COLLATE NOCASE")]
        public void VolumeDisplayNameTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = "";
            string volumeName = "Volume_DisplayName", identifier = "VolumeDisplayNameIdentifier";
            Local.FileSystem fileSystem = new() { DisplayName = "Volume DisplayName FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume target = new() { DisplayName = null, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.DisplayName);
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            expected = "Volume DisplayName Item";
            target.DisplayName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            target.DisplayName = $" {expected} ";
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            expected = $"{expected} {new string('X', 1023 - expected.Length)}";
            target.DisplayName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            string expected2 = $"{expected}X";
            target.DisplayName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DisplayNameLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            target.DisplayName = new string(' ', 1025);
            Assert.AreEqual("", target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual("", target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            fileSystem = new() { DisplayName = "Volume DisplayName FileSystem 2" };
            dbContext.FileSystems.Add(fileSystem);
            target = new() { DisplayName = null, VolumeName = volumeName, Identifier = $"{identifier}2", FileSystem = fileSystem };
            Assert.AreEqual(expected, target.DisplayName);
            entityEntry = dbContext.Volumes.Add(target);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);
        }

        [TestMethod("Volume Type Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.Type: TINYINT NOT NULL CHECK(Type>=0 AND Type<7)")]
        public void VolumeTypeTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            DriveType expected = (DriveType)(object)-1;
            Local.FileSystem fileSystem = new() { DisplayName = "Volume Type FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume Type Item", volumeName = "VolumeType", identifier = "VolumeTypeIdentifier";
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, Type = expected };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Type), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Type);

            expected = DriveType.Fixed;
            target.Type = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Type);

            expected = (DriveType)(object)7;
            target.Type = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Type), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Type);
        }

        [TestMethod("Volume FileSystem Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.FileSystem: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES FileSystems")]
        public void VolumeFileSystemTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem expected = null;
            string displayName = "Volume FileSystem Item", volumeName = "Volume_FileSystem_Name", identifier = "VolumeFileSystemIdentifier";
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = expected };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.FileSystem), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_FileSystemRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.IsNull(target.FileSystem);

            expected = new Local.FileSystem { DisplayName = "Volume FileSystem" };
            dbContext.FileSystems.Add(expected);
            target.FileSystem = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.IsNotNull(target.FileSystem);
            Assert.AreEqual(expected.Id, target.FileSystemId);
            Assert.AreEqual(expected.Id, target.FileSystem.Id);

            Local.FileSystem fs = new Local.FileSystem { DisplayName = "Volume FileSystem 2" };
            dbContext.FileSystems.Add(fs);
            dbContext.SaveChanges();

            target.FileSystem = fs;
            target.FileSystemId = fs.Id;
            target.FileSystem = null;
            Assert.AreEqual(Guid.Empty, target.FileSystemId);

            dbContext.Remove(expected);
            dbContext.SaveChanges();

            target.FileSystemId = fs.Id;

            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.FileSystem), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_FileSystemRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.FileSystem);

            dbContext.Remove(expected);
            Assert.ThrowsException<DbUpdateException>(() => dbContext.SaveChanges());

            dbContext.Remove(target);
            dbContext.SaveChanges();
        }

        [TestMethod("Volume Status Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.Status: TINYINT NOT NULL CHECK(Type>=0 AND Type<7)")]
        public void VolumeStatusTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            VolumeStatus expected = (VolumeStatus)(object)-1;
            Local.FileSystem fileSystem = new() { DisplayName = "Volume Status FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume Status Item", volumeName = "Volume_Status_Name", identifier = "VolumeStatusIdentifier";
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, Status = expected };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Status), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidVolumeStatus, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Status);

            expected = VolumeStatus.Controlled;
            target.Status = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Status);

            expected = (VolumeStatus)(object)6;
            target.Status = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Status), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidVolumeStatus, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Status);
        }

        [TestMethod("Volume MaxNameLength Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.MaxNameLength: CHECK(MaxNameLength IS NULL OR MaxNameLength>=1)")]
        public void VolumeMaxNameLengthTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            int expected = 0;
            Local.FileSystem fileSystem = new() { DisplayName = "Volume MaxNameLength FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume MaxNameLength Item", volumeName = "Volume_MaxNameLength_Name", identifier = "VolumeMaxNameLengthIdentifier";
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, MaxNameLength = expected };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.MaxNameLength), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_MaxNameLengthInvalid, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.IsTrue(target.MaxNameLength.HasValue);
            Assert.AreEqual(expected, target.MaxNameLength.Value);

            expected = 1;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.IsTrue(target.MaxNameLength.HasValue);
            Assert.AreEqual(expected, target.MaxNameLength.Value);

            expected = int.MaxValue;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.IsTrue(target.MaxNameLength.HasValue);
            Assert.AreEqual(expected, target.MaxNameLength.Value);

            expected = -1;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.MaxNameLength), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_MaxNameLengthInvalid, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.MaxNameLength);
        }

        [TestMethod("Volume CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.CreatedOn: CreatedOn<=ModifiedOn")]
        public void VolumeCreatedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = new() { DisplayName = "Volume CreatedOn FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume CreatedOn Item", volumeName = "Volume_CreatedOn_Name", identifier = "VolumeCreatedOnIdentifier";
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Volumes.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("Volume LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "Volume.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void VolumeLastSynchronizedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = new() { DisplayName = "Volume LastSynchronizedOn FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume LastSynchronizedOn Item", volumeName = "Volume_LastSynchronizedOn_Name", identifier = "VolumeLastSynchronizedOnIdentifier";
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Volumes.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Volumes.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

        #region Subdirectory Tests

        [TestMethod("Subdirectory Add/Remove Tests")]
        public void SubdirectoryAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.Subdirectory target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.Subdirectories.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            entityEntry.Reload();

            // TODO: Validate default values
            Assert.IsNull(target.LastAccessed);
            Assert.IsNull(target.Deleted);
            Assert.AreEqual("DirectoryCrawlOptions.None", target.Options);
            Assert.AreEqual("", target.Notes);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(entityEntry.State, EntityState.Deleted);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
        }

        [TestMethod("Subdirectory Name Validation Tests")]
        [TestProperty(TestProperty_Description, "Subdirectory.Name: NVARCHAR(1024) NOT NULL (ParentId IS NULL OR length(trim(Name))>0) COLLATE NOCASE")]
        public void SubdirectoryNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // TODO: Set invalid value
            Local.Subdirectory target = new() { Name = expected };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Name), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Name);

            expected = default; // TODO: Set valid value
            target.Name = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            expected = default; // TODO: Set invalid value
            target.Name = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Name), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Name);
            dbContext.Subdirectories.Remove(target);
        }

        [TestMethod("Subdirectory Volume Validation Tests")]
        [TestProperty(TestProperty_Description, "Subdirectory.Volume: UNIQUEIDENTIFIER FOREIGN REFERENCES Volume")]
        public void SubdirectoryVolumeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.Volume expected = default; // TODO: Set invalid value
            Local.Subdirectory target = new() { Volume = expected };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Volume), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_VolumeOrParentRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Volume);

            expected = default; // TODO: Set valid value
            target.Volume = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Volume);

            expected = default; // TODO: Set invalid value
            target.Volume = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Volume), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_VolumeOrParentRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Volume);
            dbContext.Subdirectories.Remove(target);
        }

        [TestMethod("Subdirectory Parent Validation Tests")]
        [TestProperty(TestProperty_Description, "Subdirectory.Parent: UNIQUEIDENTIFIER ((ParentId IS NULL AND VolumeId IS NOT NULL) OR (ParentId IS NOT NULL AND VolumeId IS NULL)) FOREIGN REFERENCES Subdirectories")]
        public void SubdirectoryParentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.Subdirectory expected = default; // TODO: Set invalid value
            Local.Subdirectory target = new() { Parent = expected };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Parent), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_VolumeOrParentRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Parent);

            expected = default; // TODO: Set valid value
            target.Parent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Parent);

            expected = default; // TODO: Set invalid value
            target.Parent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Parent), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_VolumeOrParentRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Parent);
            dbContext.Subdirectories.Remove(target);
        }

        [TestMethod("Subdirectory Options Validation Tests")]
        [TestProperty(TestProperty_Description, "Subdirectory.Options: TINYINT NOT NULL TINYINT  NOT NULL CHECK(Options>=0 AND Options<64)")]
        public void SubdirectoryOptionsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            DirectoryCrawlOptions expected = default; // TODO: Set invalid value
            Local.Subdirectory target = new() { Options = expected };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Options), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidDirectoryCrawlOption, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Options);

            expected = default; // TODO: Set valid value
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Options);

            expected = default; // TODO: Set invalid value
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Options), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidDirectoryCrawlOption, results[0].ErrorMessage);
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Options);
            dbContext.Subdirectories.Remove(target);
        }

        [TestMethod("Subdirectory CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "Subdirectory.CreatedOn: CreatedOn<=ModifiedOn")]
        public void SubdirectoryCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.Subdirectory target = new() {  /* TODO: Initialize properties */ };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Subdirectories.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("Subdirectory LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "Subdirectory.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void SubdirectoryLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.Subdirectory target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Subdirectories.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Subdirectories.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            dbContext.Subdirectories.Remove(target);

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

        #region DbFile Tests

        [TestMethod("DbFile Add/Remove Tests")]
        public void DbFileAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.DbFile target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.DbFile> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.Files.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
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
            Assert.AreEqual(entityEntry.State, EntityState.Deleted);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Content), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ContentInfoRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Content);

            expected = default; // TODO: Set valid value
            target.Content = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Content);

            expected = default; // TODO: Set invalid value
            target.Content = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Content), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ContentInfoRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Name);

            expected = default; // TODO: Set valid value
            target.Name = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            expected = default; // TODO: Set invalid value
            target.Name = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Parent);

            expected = default; // TODO: Set valid value
            target.Parent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Parent);

            expected = default; // TODO: Set invalid value
            target.Parent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Options), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidFileCrawlOption, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Options);

            expected = default; // TODO: Set valid value
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Options);

            expected = default; // TODO: Set invalid value
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.Options), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidFileCrawlOption, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Files.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Files.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.DbFile.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

        #region ContentInfo Tests

        [TestMethod("ContentInfo Add/Remove Tests")]
        public void ContentInfoAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ContentInfo target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.ContentInfo> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.ContentInfos.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            entityEntry.Reload();
            // TODO: Validate default values
            Assert.AreEqual(0L, target.Length);
            Assert.IsNull(target.Hash);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(entityEntry.State, EntityState.Deleted);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
        }

        [TestMethod("ContentInfo Hash Validation Tests")]
        [TestProperty(TestProperty_Description, "ContentInfo.Hash: BINARY(16) CHECK(Hash IS NULL OR length(HASH)=16) DEFAULT NULL")]
        public void ContentInfoHashTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            byte[] expected = default; // TODO: Set invalid value
            Local.ContentInfo target = new() { Hash = expected };
            EntityEntry<Local.ContentInfo> entityEntry = dbContext.ContentInfos.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.Hash), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidHashLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Hash);

            // TODO: Validate default values
            target.Hash = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Hash);

            expected = default; // TODO: Set invalid value
            target.Hash = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.Hash), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidHashLength, results[0].ErrorMessage);
            entityEntry = dbContext.ContentInfos.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Hash);
        }

        [TestMethod("ContentInfo Length Validation Tests")]
        [TestProperty(TestProperty_Description, "ContentInfo.Length: BIGINT NOT NULL CHECK(Length>=0) UNIQUE")]
        public void ContentInfoLengthTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            long expected = default; // TODO: Set invalid value
            Local.ContentInfo target = new() { Length = expected };
            EntityEntry<Local.ContentInfo> entityEntry = dbContext.ContentInfos.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.Length), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Length);

            expected = default; // TODO: Set valid value
            target.Length = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Length);

            expected = default; // TODO: Set invalid value
            target.Length = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.Length), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ContentInfos.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Length);
        }

        [TestMethod("ContentInfo CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "ContentInfo.CreatedOn: CreatedOn<=ModifiedOn")]
        public void ContentInfoCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ContentInfo target = new() {  /* TODO: Initialize properties */ };
            EntityEntry<Local.ContentInfo> entityEntry = dbContext.ContentInfos.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.ContentInfos.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("ContentInfo LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "ContentInfo.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void ContentInfoLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ContentInfo target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.ContentInfo> entityEntry = dbContext.ContentInfos.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.ContentInfos.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.ContentInfos.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.ContentInfos.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.ContentInfos.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

        #region RedundantSet Tests

        [TestMethod("RedundantSet Add/Remove Tests")]
        public void RedundantSetAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.RedundantSet target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.RedundantSet> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.RedundantSets.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            entityEntry.Reload();

            // TODO: Validate default values
            Assert.AreEqual("", target.Reference);
            Assert.AreEqual("", target.Notes);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(entityEntry.State, EntityState.Deleted);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
        }

        [TestMethod("RedundantSet ContentInfo Validation Tests")]
        [TestProperty(TestProperty_Description, "RedundantSet.ContentInfo: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES ContentInfos")]
        public void RedundantSetContentInfoTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ContentInfo expected = default; // TODO: Set invalid value
            Local.RedundantSet target = new() { ContentInfo = expected };
            EntityEntry<Local.RedundantSet> entityEntry = dbContext.RedundantSets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.ContentInfo), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ContentInfoRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ContentInfo);

            expected = default; // TODO: Set valid value
            target.ContentInfo = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ContentInfo);

            expected = default; // TODO: Set invalid value
            target.ContentInfo = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.ContentInfo), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ContentInfoRequired, results[0].ErrorMessage);
            entityEntry = dbContext.RedundantSets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.ContentInfo);
        }

        [TestMethod("RedundantSet Reference Validation Tests")]
        [TestProperty(TestProperty_Description, "RedundantSet.Reference: NVARCHAR(128) NOT NULL COLLATE NOCASE")]
        public void RedundantSetReferenceTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // TODO: Set invalid value
            Local.RedundantSet target = new() { Reference = expected };
            EntityEntry<Local.RedundantSet> entityEntry = dbContext.RedundantSets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.Reference), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set valid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set invalid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.Reference), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            entityEntry = dbContext.RedundantSets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Reference);
        }

        [TestMethod("RedundantSet CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "RedundantSet.CreatedOn: CreatedOn<=ModifiedOn")]
        public void RedundantSetCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.RedundantSet target = new() {  /* TODO: Initialize properties */ };
            EntityEntry<Local.RedundantSet> entityEntry = dbContext.RedundantSets.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.RedundantSets.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("RedundantSet LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "RedundantSet.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void RedundantSetLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.RedundantSet target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.RedundantSet> entityEntry = dbContext.RedundantSets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.RedundantSets.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.RedundantSets.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.RedundantSets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.RedundantSets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

        #region Redundancy Tests

        [TestMethod("Redundancy Add/Remove Tests")]
        public void RedundancyAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.Redundancy target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.Redundancy> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.Redundancies.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();

            // TODO: Validate default values
            Assert.AreEqual("", target.Reference);
            Assert.AreEqual(FileRedundancyStatus.NotRedundant, target.Status);
            Assert.AreEqual("", target.Notes);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(entityEntry.State, EntityState.Deleted);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
        }

        [TestMethod("Redundancy Reference Validation Tests")]
        [TestProperty(TestProperty_Description, "Redundancy.Reference: NVARCHAR(128) NOT NULL COLLATE NOCASE")]
        public void RedundancyReferenceTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // TODO: Set invalid value
            Local.Redundancy target = new() { Reference = expected };
            EntityEntry<Local.Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.Reference), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set valid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set invalid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.Reference), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Reference);
        }

        [TestMethod("Redundancy Status Validation Tests")]
        [TestProperty(TestProperty_Description, "Redundancy.Status: TINYINT NOT NULL DEFAULT 0 CHECK(Status>=0 AND Status < 9)")]
        public void RedundancyStatusTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            FileRedundancyStatus expected = default; // TODO: Set invalid value
            Local.Redundancy target = new() { Status = expected };
            EntityEntry<Local.Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.Status), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidFileRedundancyStatus, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Status);

            expected = default; // TODO: Set valid value
            target.Status = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Status);

            expected = default; // TODO: Set invalid value
            target.Status = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.Status), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_InvalidFileRedundancyStatus, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.Status);
        }

        [TestMethod("Redundancy RedundantSet Validation Tests")]
        [TestProperty(TestProperty_Description, "Redundancy.RedundantSet: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES RedundantSets")]
        public void RedundancyRedundantSetTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.RedundantSet expected = default; // TODO: Set invalid value
            Local.Redundancy target = new() { RedundantSet = expected };
            EntityEntry<Local.Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.RedundantSet), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_RedundantSetRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.RedundantSet);

            expected = default; // TODO: Set valid value
            target.RedundantSet = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.RedundantSet);

            expected = default; // TODO: Set invalid value
            target.RedundantSet = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.RedundantSet), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_RedundantSetRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.RedundantSet);
        }

        [TestMethod("Redundancy File Validation Tests")]
        [TestProperty(TestProperty_Description, "Redundancy.File: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES DbFiles")]
        public void RedundancyFileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.DbFile expected = default; // TODO: Set invalid value
            Local.Redundancy target = new() { File = expected };
            EntityEntry<Local.Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.File), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_FileRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.File);

            expected = default; // TODO: Set valid value
            target.File = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.File);

            expected = default; // TODO: Set invalid value
            target.File = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.File), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_FileRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.File);
        }

        [TestMethod("Redundancy CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "Redundancy.CreatedOn: CreatedOn<=ModifiedOn")]
        public void RedundancyCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.Redundancy target = new() {  /* TODO: Initialize properties */ };
            EntityEntry<Local.Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Redundancies.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("Redundancy LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "Redundancy.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void RedundancyLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.Redundancy target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.Redundancy> entityEntry = dbContext.Redundancies.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Redundancies.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Redundancies.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

        #region FileComparison Tests

        [TestMethod("FileComparison Add/Remove Tests")]
        public void FileComparisonAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileComparison target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.FileComparison> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.Comparisons.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();

            // TODO: Validate default values
            Assert.IsFalse(target.AreEqual);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(entityEntry.State, EntityState.Deleted);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
        }

        [TestMethod("FileComparison SourceFile Validation Tests")]
        [TestProperty(TestProperty_Description, "FileComparison.SourceFile: UNIQUEIDENTIFIER NOT NULL (SourceFileId<>TargetFileId) FOREIGN REFERENCES DbFiles")]
        public void FileComparisonSourceFileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.DbFile expected = default; // TODO: Set invalid value
            Local.FileComparison target = new() { SourceFile = expected };
            EntityEntry<Local.FileComparison> entityEntry = dbContext.Comparisons.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.SourceFile), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_SourceFileRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.SourceFile);

            expected = default; // TODO: Set valid value
            target.SourceFile = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.SourceFile);

            expected = default; // TODO: Set invalid value
            target.SourceFile = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.SourceFile), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_SourceFileRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Comparisons.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.SourceFile);
        }

        [TestMethod("FileComparison TargetFile Validation Tests")]
        [TestProperty(TestProperty_Description, "FileComparison.TargetFile: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES DbFiles")]
        public void FileComparisonTargetFileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.DbFile expected = default; // TODO: Set invalid value
            Local.FileComparison target = new() { TargetFile = expected };
            EntityEntry<Local.FileComparison> entityEntry = dbContext.Comparisons.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.TargetFile), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_TargetFileRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.TargetFile);

            expected = default; // TODO: Set valid value
            target.TargetFile = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.TargetFile);

            expected = default; // TODO: Set invalid value
            target.TargetFile = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.TargetFile), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_TargetFileRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Comparisons.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            Assert.AreEqual(expected, target.TargetFile);
        }

        [TestMethod("FileComparison CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "FileComparison.CreatedOn: CreatedOn<=ModifiedOn")]
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Comparisons.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("FileComparison LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "FileComparison.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void FileComparisonLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileComparison target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.FileComparison> entityEntry = dbContext.Comparisons.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Comparisons.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Comparisons.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Comparisons.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileComparison.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Comparisons.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

    }
}
