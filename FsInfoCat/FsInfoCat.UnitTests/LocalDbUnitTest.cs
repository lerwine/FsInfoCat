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
                using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
                foreach (var element in XDocument.Parse(Local.Properties.Resources.DbCommands).Root.Elements("ExampleData").Elements("Text"))
                    dbContext.Database.ExecuteSqlRaw(element.Value);
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
            XDocument document = XDocument.Parse(Local.Properties.Resources.DbCommands);
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            dbContext.ChangeTracker.Clear();
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                foreach (XElement element in document.Root.Elements("ExampleData").Elements("Text"))
                    dbContext.Database.ExecuteSqlRaw(element.Value);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }

        [TestMethod]
        public void LocalDbContextTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Assert.IsNotNull(dbContext.FileSystems);
            Assert.IsNotNull(dbContext.SymbolicNames);
            Assert.IsNotNull(dbContext.Volumes);
            Assert.IsNotNull(dbContext.Subdirectories);
            Assert.IsNotNull(dbContext.Files);
            Assert.IsNotNull(dbContext.ExtendedProperties);
            Assert.IsNotNull(dbContext.ContentInfos);
            Assert.IsNotNull(dbContext.Comparisons);
            Assert.IsNotNull(dbContext.RedundantSets);
            Assert.IsNotNull(dbContext.Redundancies);
        }

        #region FileSystem Tests

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
        [TestProperty(TestProperty_Description, "FileSystem.DefaultDriveType: CHECK(DefaultDriveType IS NULL OR (DefaultDriveType>=0 AND DefaultDriveType<7))")]
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
        [TestProperty(TestProperty_Description, "FileSystem: DisplayName NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName)) = length(DisplayName) AND length(DisplayName)>0) UNIQUE COLLATE NOCASE")]
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
        [TestProperty(TestProperty_Description, "FileSystem: MaxNameLength CHECK(MaxNameLength IS NULL OR MaxNameLength>=0)")]
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
        [TestProperty(TestProperty_Description,
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

        #endregion

        #region SymbolicName Tests

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

        private Local.FileSystem GetVFatFileSystem(Local.LocalDbContext dbContext)
        {
            Guid id = Guid.Parse("53a9e9a4-f5f0-4b4c-9f1e-4e3a80a93cfd");
            return (from fs in dbContext.FileSystems where fs.Id == id select fs).FirstOrDefault();
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

        #endregion

        #region Volume Tests

        [TestMethod("Volume Add/Remove Tests")]
        public void VolumeAddRemoveTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string displayName = "Volume Add/Remove Item", volumeName = "Volume_Add_Remove_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Local.FileSystem fileSystem = new() { DisplayName = "Volume Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            EntityEntry<Local.Volume> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.Volumes.Add(target);
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
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("Volume Identifier Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.Identifier: NVARCHAR(1024) NOT NULL CHECK(length(trim(Identifier))>0) UNIQUE COLLATE NOCASE")]
        public void VolumeIdentifierTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string displayName = "Volume Identifier Item", volumeName = "Volume_Identifier_Test";
            VolumeIdentifier expected = default;
            Local.FileSystem fileSystem = new() { DisplayName = "Volume Identifier FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            dbContext.SaveChanges();
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = default, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.Identifier);
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);

            expected = new VolumeIdentifier(0xB04D955Du);
            target.Identifier = expected;
            Assert.AreEqual(expected, target.Identifier);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Identifier);

            target.Identifier = new VolumeIdentifier(new Uri("urn:volume:id:B04D-955D", UriKind.Absolute));
            Assert.AreEqual(expected, target.Identifier);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Identifier);

            Guid id = Guid.NewGuid();
            expected = new VolumeIdentifier(id);
            target.Identifier = expected;
            Assert.AreEqual(expected, target.Identifier);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Identifier);

            target.Identifier = new VolumeIdentifier(new Uri($"urn:uuid:{id:d}", UriKind.Absolute));
            Assert.AreEqual(expected, target.Identifier);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);

            expected = new VolumeIdentifier(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare"));
            target.Identifier = expected;
            Assert.AreEqual(expected, target.Identifier);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);


            expected = new VolumeIdentifier(new Uri($"{expected} {new string('X', 1023 - expected.ToString().Length)}"));
            target.Identifier = expected;
            Assert.AreEqual(expected, target.Identifier);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);

            VolumeIdentifier expected2 = new(new Uri($"{expected}X"));
            target.Identifier = expected2;
            Assert.AreEqual(expected2, target.Identifier);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected2, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);

            target.Identifier = expected;
            dbContext.SaveChanges();

            fileSystem = new() { DisplayName = "Volume Identifier FileSystem 2" };
            dbContext.FileSystems.Add(fileSystem);
            target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = expected, FileSystem = fileSystem };
            entityEntry = dbContext.Volumes.Add(target);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateVolumeIdentifier, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            Assert.AreEqual(expected, target.Identifier);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);

            expected = new VolumeIdentifier(Guid.Empty);
            target.Identifier = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
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
            string displayName = "Volume VolumeName Item";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Local.FileSystem fileSystem = new() { DisplayName = "Volume Name FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume target = new() { DisplayName = displayName, VolumeName = null, Identifier = identifier, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.VolumeName);
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.VolumeName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            expected = "Volume_VolumeName";
            target.VolumeName = expected;
            Assert.AreEqual(expected, target.VolumeName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            target.VolumeName = $" {expected} ";
            Assert.AreEqual(expected, target.VolumeName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            expected = $"{expected}_{new string('X', 127 - expected.Length)}";
            target.VolumeName = expected;
            Assert.AreEqual(expected, target.VolumeName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            string expected2 = $"{expected}X";
            target.VolumeName = expected2;
            Assert.AreEqual(expected2, target.VolumeName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.VolumeName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected2, target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            target.VolumeName = new string(' ', 129);
            Assert.AreEqual("", target.VolumeName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.VolumeName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual("", target.VolumeName);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(identifier, target.Identifier);

            target.VolumeName = expected;
            fileSystem = new() { DisplayName = "Volume Name FileSystem 2" };
            dbContext.FileSystems.Add(fileSystem);
            identifier = new(Guid.NewGuid());
            target = new() { DisplayName = displayName, VolumeName = expected, Identifier = identifier, FileSystem = fileSystem };
            entityEntry = dbContext.Volumes.Add(target);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
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
            string volumeName = "Volume_DisplayName";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Local.FileSystem fileSystem = new() { DisplayName = "Volume DisplayName FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume target = new() { DisplayName = null, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.DisplayName);
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            expected = "Volume DisplayName Item";
            target.DisplayName = expected;
            Assert.AreEqual(expected, target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

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
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            expected = $"{expected} {new string('X', 1023 - expected.Length)}";
            target.DisplayName = expected;
            Assert.AreEqual(expected, target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            string expected2 = $"{expected}X";
            target.DisplayName = expected2;
            Assert.AreEqual(expected2, target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected2, target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            target.DisplayName = new string(' ', 1025);
            Assert.AreEqual("", target.DisplayName);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual("", target.DisplayName);
            Assert.AreEqual(volumeName, target.VolumeName);
            Assert.AreEqual(identifier, target.Identifier);

            target.DisplayName = expected;
            fileSystem = new() { DisplayName = "Volume DisplayName FileSystem 2" };
            dbContext.FileSystems.Add(fileSystem);
            identifier = new VolumeIdentifier(Guid.NewGuid());
            target = new() { DisplayName = expected, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.DisplayName);
            entityEntry = dbContext.Volumes.Add(target);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
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
            string displayName = "Volume Type Item", volumeName = "VolumeType";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, Type = expected };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Type), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Type);

            expected = DriveType.Fixed;
            target.Type = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Type);

            expected = (DriveType)(object)7;
            target.Type = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Type), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Type);
        }

        [TestMethod("Volume FileSystem Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.FileSystem: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES FileSystems")]
        public void VolumeFileSystemTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem expected = null;
            string displayName = "Volume FileSystem Item", volumeName = "Volume_FileSystem_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = expected };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.FileSystem), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.IsNull(target.FileSystem);

            expected = new Local.FileSystem { DisplayName = "Volume FileSystem" };
            dbContext.FileSystems.Add(expected);
            target.FileSystem = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.IsNotNull(target.FileSystem);
            Assert.AreEqual(expected.Id, target.FileSystemId);
            Assert.AreEqual(expected.Id, target.FileSystem.Id);

            Local.FileSystem fs = new() { DisplayName = "Volume FileSystem 2" };
            dbContext.FileSystems.Add(fs);
            dbContext.SaveChanges();

            Guid fileSystemId = expected.Id;
            dbContext.ChangeTracker.Clear();
            //dbContext.SaveChanges();
            //var vEntities = from v in dbContext.Volumes where v.Id == id select v;
            //var fsEntities = from f in dbContext.FileSystems where f.Id == fileSystemId select f;
            //Assert.IsTrue(vEntities.Any());
            //Assert.IsTrue(fsEntities.Any());
            //dbContext.Database.GetConnectionString()
            Assert.ThrowsException<InvalidOperationException>(() => dbContext.Remove(expected));

            target.FileSystem = fs;
            Assert.AreEqual(fs.Id, target.FileSystemId);

            dbContext.Remove(expected);
            dbContext.SaveChanges();

            target.FileSystem = null;
            Assert.AreEqual(Guid.Empty, target.FileSystemId);

            target.FileSystemId = fileSystemId;

            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.FileSystem), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.IsNull(target.FileSystem);

            target.FileSystem = fs;
            dbContext.SaveChanges();
        }

        [TestMethod("Volume Status Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.Status: TINYINT NOT NULL CHECK(Type>=0 AND Type<7)")]
        public void VolumeStatusTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            VolumeStatus expected = (VolumeStatus)(object)(byte)255;
            Local.FileSystem fileSystem = new() { DisplayName = "Volume Status FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume Status Item", volumeName = "Volume_Status_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, Status = expected };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Status), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidVolumeStatus, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Status);

            expected = VolumeStatus.Controlled;
            target.Status = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Status);

            expected = (VolumeStatus)(object)(byte)6;
            target.Status = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.Status), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidVolumeStatus, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
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
            string displayName = "Volume MaxNameLength Item", volumeName = "Volume_MaxNameLength_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, MaxNameLength = expected };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.MaxNameLength), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.IsTrue(target.MaxNameLength.HasValue);
            Assert.AreEqual(expected, target.MaxNameLength.Value);

            expected = 1;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.IsTrue(target.MaxNameLength.HasValue);
            Assert.AreEqual(expected, target.MaxNameLength.Value);

            expected = int.MaxValue;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.IsTrue(target.MaxNameLength.HasValue);
            Assert.AreEqual(expected, target.MaxNameLength.Value);

            expected = -1;
            target.MaxNameLength = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.MaxNameLength), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.MaxNameLength);
        }

        [TestMethod("Volume CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "Volume.CreatedOn: CreatedOn<=ModifiedOn")]
        public void VolumeCreatedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = new() { DisplayName = "Volume CreatedOn FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume CreatedOn Item", volumeName = "Volume_CreatedOn_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Volume.CreatedOn), results[0].MemberNames.First());
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
            string displayName = "Volume LastSynchronizedOn Item", volumeName = "Volume_LastSynchronizedOn_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Local.Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.Volume> entityEntry = dbContext.Volumes.Add(target);
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
            entityEntry = dbContext.Volumes.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Volumes.Update(target);
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
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
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
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem1 = new() { DisplayName = "Subdirectory Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem1);
            Local.Volume volume1 = new() { DisplayName = "Subdirectory Add/Remove Item", VolumeName = "Subdirectory_Add_Remove_Name", Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1 };
            dbContext.Volumes.Add(volume1);
            string expectedName = "";
            Local.Subdirectory target1 = new() { Volume = volume1 };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Entry(target1);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.Subdirectories.Add(target1);
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target1, new ValidationContext(target1), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            Assert.AreNotEqual(Guid.Empty, target1.Id);
            entityEntry.Reload();
            Assert.AreEqual(expectedName, target1.Name);
            Assert.IsFalse(target1.Deleted);
            Assert.AreEqual("", target1.Notes);
            Assert.AreEqual(DirectoryCrawlOptions.None, target1.Options);
            Assert.IsNull(target1.Parent);
            Assert.IsNotNull(target1.Volume);
            Assert.AreEqual(volume1.Id, target1.VolumeId);
            Assert.AreEqual(volume1.Id, target1.Volume.Id);
            Assert.IsNull(target1.LastSynchronizedOn);
            Assert.IsNull(target1.UpstreamId);
            Assert.IsTrue(target1.CreatedOn >= now);
            Assert.AreEqual(target1.CreatedOn, target1.ModifiedOn);

            entityEntry = dbContext.Remove(target1);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("Subdirectory Name Validation Tests")]
        [TestProperty(TestProperty_Description, "Subdirectory.Name: NVARCHAR(1024) NOT NULL (ParentId IS NULL OR length(trim(Name))>0) COLLATE NOCASE")]
        public void SubdirectoryNameTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            string expectedName = "";
            Local.FileSystem fileSystem1 = new() { DisplayName = "Subdirectory NameTest FileSystem" };
            dbContext.FileSystems.Add(fileSystem1);
            Local.Volume volume1 = new()
            {
                DisplayName = "Subdirectory NameTest Item",
                VolumeName = "Subdirectory_NameTest_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1
            };
            dbContext.Volumes.Add(volume1);
            Local.Subdirectory parent1 = new() { Volume = volume1 };
            dbContext.Subdirectories.Add(parent1);
            dbContext.SaveChanges();
            Local.Subdirectory target1 = new();
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target1);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target1, new ValidationContext(target1), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeOrParentRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expectedName, target1.Name);

            target1.Parent = parent1;
            results = new();
            success = Validator.TryValidateObject(target1, new ValidationContext(target1), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expectedName, target1.Name);

            expectedName = "Subdirectory NameTest Subdir";
            target1.Name = expectedName;
            Assert.AreEqual(expectedName, target1.Name);
            results = new();
            success = Validator.TryValidateObject(target1, new ValidationContext(target1), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expectedName, target1.Name);

            expectedName = $"{expectedName} {new string('X', 1023 - expectedName.Length)}";
            target1.Name = expectedName;
            Assert.AreEqual(expectedName, target1.Name);
            results = new();
            success = Validator.TryValidateObject(target1, new ValidationContext(target1), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.Subdirectories.Update(target1);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expectedName, target1.Name);

            string expectedName2 = $"{expectedName}X";
            target1.Name = expectedName2;
            Assert.AreEqual(expectedName2, target1.Name);
            results = new();
            success = Validator.TryValidateObject(target1, new ValidationContext(target1), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameLength, results[0].ErrorMessage);
            dbContext.Subdirectories.Update(target1);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expectedName2, target1.Name);

            target1.Name = expectedName;
            dbContext.SaveChanges();

            Local.Volume volume2 = new()
            {
                DisplayName = "Subdirectory NameTest Item 2",
                VolumeName = "Subdirectory_NameTest_Name 2",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1
            };
            dbContext.Volumes.Add(volume2);
            dbContext.SaveChanges();
            Local.Volume volume3 = new()
            {
                DisplayName = "Subdirectory NameTest Item 3",
                VolumeName = "Subdirectory_NameTest_Name 3",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1
            };
            dbContext.Volumes.Add(volume3);
            dbContext.SaveChanges();

            Local.Subdirectory target2 = new() { Volume = volume2, Name = expectedName };
            Assert.AreEqual(expectedName, target2.Name);
            entityEntry = dbContext.Subdirectories.Add(target2);
            results = new();
            success = Validator.TryValidateObject(target2, new ValidationContext(target2), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();

            Local.Subdirectory target3 = new() { Parent = parent1, Name = expectedName };
            entityEntry = dbContext.Subdirectories.Add(target3);
            results = new();
            success = Validator.TryValidateObject(target3, new ValidationContext(target3), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target3.Name = $"{expectedName[1..]}2";
            dbContext.SaveChanges();

            expectedName = $"{expectedName[1..]}3";
            Local.Subdirectory target4 = new() { Volume = volume3, Parent = parent1, Name = expectedName };
            entityEntry = dbContext.Subdirectories.Add(target4);
            results = new();
            success = Validator.TryValidateObject(target4, new ValidationContext(target4), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Volume), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeAndParent, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expectedName, target4.Name);

            target4.Volume = null;
            target4.Parent = parent1;
            dbContext.SaveChanges();

            target4.Parent = null;
            target4.Volume = volume2;
            results = new();
            success = Validator.TryValidateObject(target4, new ValidationContext(target4), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Volume), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeHasRoot, results[0].ErrorMessage);
            dbContext.Subdirectories.Update(target4);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expectedName, target4.Name);

            target4.Volume = volume3;
            dbContext.Subdirectories.Update(target4);
            target2.Volume = volume2;
            dbContext.SaveChanges();
        }

        [TestMethod("Subdirectory Options Validation Tests")]
        [TestProperty(TestProperty_Description, "Subdirectory.Options: TINYINT NOT NULL TINYINT  NOT NULL CHECK(Options>=0 AND Options<64)")]
        public void SubdirectoryOptionsTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = new() { DisplayName = "Subdirectory OptionsTest FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume volume = new()
            {
                DisplayName = "Subdirectory OptionsTest Item",
                VolumeName = "Subdirectory_OptionsTest_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem
            };
            dbContext.Volumes.Add(volume);
            Local.Subdirectory parent = new() { Volume = volume };
            dbContext.Subdirectories.Add(parent);
            dbContext.SaveChanges();
            DirectoryCrawlOptions expected = (DirectoryCrawlOptions)(object)(byte)64;
            Local.Subdirectory target = new() { Options = expected, Name = "OptionsTest Dir", Parent = parent };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.Options), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidDirectoryCrawlOption, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Options);

            expected = DirectoryCrawlOptions.DoNotCompareFiles;
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Options);
        }

        [TestMethod("Subdirectory CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "Subdirectory.CreatedOn: CreatedOn<=ModifiedOn")]
        public void SubdirectoryCreatedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = new() { DisplayName = "Subdirectory CreatedOnTest FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume volume = new()
            {
                DisplayName = "Subdirectory CreatedOnTest Item",
                VolumeName = "Subdirectory_CreatedOnTest_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem
            };
            dbContext.Volumes.Add(volume);
            Local.Subdirectory parent = new() { Volume = volume };
            dbContext.Subdirectories.Add(parent);
            dbContext.SaveChanges();
            Local.Subdirectory target = new() { Name = "CreatedOnTest Dir", Parent = parent };
            EntityEntry<Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Subdirectory.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.Subdirectories.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("Subdirectory LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "Subdirectory.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void SubdirectoryLastSynchronizedOnTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = new() { DisplayName = "Subdirectory LastSynchronizedOn FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Local.Volume volume = new()
            {
                DisplayName = "Subdirectory LastSynchronizedOn Item",
                VolumeName = "Subdirectory_LastSynchronizedOn_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem
            };
            dbContext.Volumes.Add(volume);
            Local.Subdirectory parent = new() { Volume = volume };
            dbContext.Subdirectories.Add(parent);
            dbContext.SaveChanges();
            Local.Subdirectory target = new() { UpstreamId = Guid.NewGuid(), Parent = parent, Name = "LastSynchronizedOn Dir" };
            EntityEntry <Local.Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
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
            entityEntry = dbContext.Subdirectories.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Subdirectories.Update(target);
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
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }

        #endregion

        #region DbFile Tests

        [TestMethod("DbFile Add/Remove Tests")]
        public void DbFileAddRemoveTestMethod()
        {
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

        #endregion

        #region ExtendedProperties Tests

        [TestMethod("ExtendedProperties Add/Remove Tests")]
        public void ExtendedPropertiesAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ExtendedProperties target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.ExtendedProperties.Add(target);
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
            Assert.AreEqual(0, target.Height);
            Assert.AreEqual(0, target.Width);
            Assert.IsNull(target.Duration);
            Assert.IsNull(target.FrameCount);
            Assert.IsNull(target.TrackNumber);
            Assert.IsNull(target.Bitrate);
            Assert.IsNull(target.FrameRate);
            Assert.IsNull(target.SamplesPerPixel);
            Assert.IsNull(target.PixelPerUnitX);
            Assert.IsNull(target.PixelPerUnitY);
            Assert.IsNull(target.Compression);
            Assert.IsNull(target.XResNumerator);
            Assert.IsNull(target.XResDenominator);
            Assert.IsNull(target.YResNumerator);
            Assert.IsNull(target.YResDenominator);
            Assert.IsNull(target.ResolutionXUnit);
            Assert.IsNull(target.ResolutionYUnit);
            Assert.IsNull(target.JPEGProc);
            Assert.IsNull(target.JPEGQuality);
            Assert.IsNull(target.DateTime);
            Assert.IsNull(target.Title);
            Assert.IsNull(target.Description);
            Assert.IsNull(target.Copyright);
            Assert.IsNull(target.SoftwareUsed);
            Assert.IsNull(target.Artist);
            Assert.IsNull(target.HostComputer);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("ExtendedProperties Height Validation Tests")]
        [TestProperty(TestProperty_Description, "ExtendedProperties.Height: INT NOT NULL CHECK(Height>=0 AND Height<65536)")]
        public void ExtendedPropertiesHeightTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ExtendedProperties target = new() { Height = 0 };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.ExtendedProperties.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(0, target.Height);
            // TODO: Validate default values

            ushort expected = 12;
            target.Height = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(expected, target.Height);
        }

        [TestMethod("ExtendedProperties Width Validation Tests")]
        [TestProperty(TestProperty_Description, "ExtendedProperties.Width: INT NOT NULL CHECK(Height>=0 AND Height<65536)")]
        public void ExtendedPropertiesWidthTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            ushort expected = default; // TODO: Set invalid value
            Local.ExtendedProperties target = new() { Width = expected };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.ExtendedProperties.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ExtendedProperties.Width), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Width);

            expected = default; // TODO: Set valid value
            target.Width = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Width);

            expected = default; // TODO: Set invalid value
            target.Width = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ExtendedProperties.Width), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ExtendedProperties.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Width);
        }

        [TestMethod("ExtendedProperties CreatedOn Validation Tests")]
        [TestProperty(TestProperty_Description, "ExtendedProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void ExtendedPropertiesCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ExtendedProperties target = new() {  /* TODO: Initialize properties */ };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.ExtendedProperties.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ExtendedProperties.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.ExtendedProperties.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("ExtendedProperties LastSynchronizedOn Validation Tests")]
        [TestProperty(TestProperty_Description,
            "ExtendedProperties.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void ExtendedPropertiesLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ExtendedProperties target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.ExtendedProperties.Add(target);
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
            entityEntry = dbContext.ExtendedProperties.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.ExtendedProperties.Update(target);
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
            entityEntry = dbContext.ExtendedProperties.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.ExtendedProperties.Update(target);
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
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.ContentInfos.Add(target);
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
            Assert.AreEqual(0L, target.Length);
            Assert.IsNull(target.Hash);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("ContentInfo Hash Validation Tests")]
        [TestProperty(TestProperty_Description, "ContentInfo.Hash: BINARY(16) CHECK(Hash IS NULL OR length(HASH)=16) DEFAULT NULL")]
        public void ContentInfoHashTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ContentInfo target = new() { Hash = null };
            EntityEntry<Local.ContentInfo> entityEntry = dbContext.ContentInfos.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.IsNull(target.Hash);
            // TODO: Validate default values

            MD5Hash? expected = new MD5Hash(Guid.NewGuid().ToByteArray());
            target.Hash = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.Length), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Length);

            expected = default; // TODO: Set valid value
            target.Length = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Length);

            expected = default; // TODO: Set invalid value
            target.Length = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.Length), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ContentInfos.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ContentInfo.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.ContentInfos.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.ContentInfos.Update(target);
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
            entityEntry = dbContext.ContentInfos.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
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
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.RedundantSets.Add(target);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.ContentInfo), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ContentInfoRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ContentInfo);

            expected = default; // TODO: Set valid value
            target.ContentInfo = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ContentInfo);

            expected = default; // TODO: Set invalid value
            target.ContentInfo = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.ContentInfo), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ContentInfoRequired, results[0].ErrorMessage);
            entityEntry = dbContext.RedundantSets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ContentInfo);
        }

        [TestMethod("RedundantSet Reference Validation Tests")]
        [TestProperty(TestProperty_Description, "RedundantSet.Reference: NVARCHAR(128) NOT NULL COLLATE NOCASE")]
        public void RedundantSetRemediationStatusTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            RedundancyRemediationStatus expected = default; // TODO: Set invalid value
            Local.RedundantSet target = new() { RemediationStatus = expected };
            EntityEntry<Local.RedundantSet> entityEntry = dbContext.RedundantSets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.Reference), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set valid value
            target.RemediationStatus = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set invalid value
            target.RemediationStatus = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.Reference), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            entityEntry = dbContext.RedundantSets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Reference);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.Reference), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set valid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set invalid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.Reference), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            entityEntry = dbContext.RedundantSets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.RedundantSet.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.RedundantSets.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.RedundantSets.Update(target);
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
            entityEntry = dbContext.RedundantSets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
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

            // TODO: Validate default values
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.Reference), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set valid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Reference);

            expected = default; // TODO: Set invalid value
            target.Reference = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.Reference), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ReferenceLength, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Reference);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.RedundantSet), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_RedundantSetRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.RedundantSet);

            expected = default; // TODO: Set valid value
            target.RedundantSet = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.RedundantSet);

            expected = default; // TODO: Set invalid value
            target.RedundantSet = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.RedundantSet), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_RedundantSetRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.File), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.File);

            expected = default; // TODO: Set valid value
            target.File = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.File);

            expected = default; // TODO: Set invalid value
            target.File = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.File), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
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
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.Redundancy.CreatedOn), results[0].MemberNames.First());
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
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Redundancies.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
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
        [TestProperty(TestProperty_Description, "FileComparison.SourceFile: UNIQUEIDENTIFIER NOT NULL (SourceFileId<>TargetFileId) FOREIGN REFERENCES DbFiles")]
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
        [TestProperty(TestProperty_Description, "FileComparison.TargetFile: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES DbFiles")]
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
        [TestProperty(TestProperty_Description,
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

        #endregion

    }
}
