using FsInfoCat.Local.Model;
using FsInfoCat.Model;
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
    public class LocalVolumeUnitTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void OnTestInitialize()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            TestHelper.UndoChanges(dbContext);
        }

        [TestMethod("Volume Constructor Test"), Ignore]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            Volume target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(string.Empty, target.DisplayName);
            Assert.AreEqual(string.Empty, target.VolumeName);
            Assert.AreEqual(VolumeIdentifier.Empty, target.Identifier);
            Assert.AreEqual(Guid.Empty, target.FileSystemId);
            Assert.IsNull(target.FileSystem);
            Assert.AreEqual(DriveType.Unknown, target.Type);
            Assert.IsNull(target.ReadOnly);
            Assert.AreEqual(VolumeStatus.Unknown, target.Status);
            Assert.IsNull(target.MaxNameLength);
            Assert.AreEqual(string.Empty, target.Notes);
        }

        [TestMethod("Volume Add/Remove Tests"), Ignore]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeAddRemoveTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string displayName = "Volume Add/Remove Item", volumeName = "Volume_Add_Remove_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            FileSystem fileSystem = new() { DisplayName = "Volume Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            EntityEntry<Volume> entityEntry = dbContext.Entry(target);
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

        [TestMethod("Volume Identifier Validation Tests"), Ignore]
        [Description("Volume.Identifier: NVARCHAR(1024) NOT NULL CHECK(length(trim(Identifier))>0) UNIQUE COLLATE NOCASE")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeIdentifierTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string displayName = "Volume Identifier Item", volumeName = "Volume_Identifier_Test";
            VolumeIdentifier expected = default;
            FileSystem fileSystem = new() { DisplayName = "Volume Identifier FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            dbContext.SaveChanges();
            Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = default, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.Identifier);
            EntityEntry<Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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
            Assert.AreEqual(nameof(Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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
            Assert.AreEqual(nameof(Volume.Identifier), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateVolumeIdentifier, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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

        [TestMethod("Volume VolumeName Validation Tests"), Ignore]
        [Description("Volume.VolumeName: NVARCHAR(128) NOT NULL CHECK(length(trim(VolumeName))>0) COLLATE NOCASE")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeVolumeNameTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = "";
            string displayName = "Volume VolumeName Item";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            FileSystem fileSystem = new() { DisplayName = "Volume Name FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Volume target = new() { DisplayName = displayName, VolumeName = null, Identifier = identifier, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.VolumeName);
            EntityEntry<Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Volume.VolumeName), results[0].MemberNames.First());
            //Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameRequired, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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
            Assert.AreEqual(nameof(Volume.VolumeName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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
            Assert.AreEqual(nameof(Volume.VolumeName), results[0].MemberNames.First());
            //Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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

        [TestMethod("Volume DisplayName Validation Tests"), Ignore]
        [Description("Volume.DisplayName: NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName))>0) COLLATE NOCASE")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeDisplayNameTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = "";
            string volumeName = "Volume_DisplayName";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            FileSystem fileSystem = new() { DisplayName = "Volume DisplayName FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Volume target = new() { DisplayName = null, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            Assert.AreEqual(expected, target.DisplayName);
            EntityEntry<Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Volume.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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
            Assert.AreEqual(nameof(Volume.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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
            Assert.AreEqual(nameof(Volume.DisplayName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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

        [TestMethod("Volume Type Validation Tests"), Ignore]
        [Description("Volume.Type: TINYINT NOT NULL CHECK(Type>=0 AND Type<7)")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeTypeTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DriveType expected = (DriveType)(object)-1;
            FileSystem fileSystem = new() { DisplayName = "Volume Type FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume Type Item", volumeName = "VolumeType";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, Type = expected };
            EntityEntry<Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Volume.Type), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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
            Assert.AreEqual(nameof(Volume.Type), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Type);
        }

        [TestMethod("Volume FileSystem Validation Tests"), Ignore]
        [Description("Volume.FileSystem: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES FileSystems")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeFileSystemTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem expected = null;
            string displayName = "Volume FileSystem Item", volumeName = "Volume_FileSystem_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = expected };
            EntityEntry<Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Volume.FileSystem), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.IsNull(target.FileSystem);

            expected = new FileSystem { DisplayName = "Volume FileSystem" };
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

            FileSystem fs = new() { DisplayName = "Volume FileSystem 2" };
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
            Assert.ThrowsExactly<InvalidOperationException>(() => dbContext.Remove(expected));

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
            Assert.AreEqual(nameof(Volume.FileSystem), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.IsNull(target.FileSystem);

            target.FileSystem = fs;
            dbContext.SaveChanges();
        }

        [TestMethod("Volume Status Validation Tests"), Ignore]
        [Description("Volume.Status: TINYINT NOT NULL CHECK(Status>=0 AND Status<6)")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeStatusTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            VolumeStatus expected = (VolumeStatus)(object)(byte)255;
            FileSystem fileSystem = new() { DisplayName = "Volume Status FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume Status Item", volumeName = "Volume_Status_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, Status = expected };
            EntityEntry<Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Volume.Status), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidVolumeStatus, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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
            Assert.AreEqual(nameof(Volume.Status), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidVolumeStatus, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Status);
        }

        [TestMethod("Volume MaxNameLength Validation Tests"), Ignore]
        [Description("Volume.MaxNameLength: CHECK(MaxNameLength IS NULL OR MaxNameLength>=1)")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeMaxNameLengthTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            uint expected = 0;
            FileSystem fileSystem = new() { DisplayName = "Volume MaxNameLength FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume MaxNameLength Item", volumeName = "Volume_MaxNameLength_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, MaxNameLength = expected };
            EntityEntry<Volume> entityEntry = dbContext.Volumes.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Volume.MaxNameLength), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
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

            expected = uint.MaxValue;
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

            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.MaxNameLength);
        }

        [TestMethod("Volume CreatedOn Validation Tests"), Ignore]
        [Description("Volume.CreatedOn: CreatedOn<=ModifiedOn")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeCreatedOnTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem fileSystem = new() { DisplayName = "Volume CreatedOn FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume CreatedOn Item", volumeName = "Volume_CreatedOn_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem };
            EntityEntry<Volume> entityEntry = dbContext.Volumes.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Volume.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.Volumes.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("Volume LastSynchronizedOn Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description,
            "Volume.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void VolumeLastSynchronizedOnTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem fileSystem = new() { DisplayName = "Volume LastSynchronizedOn FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            string displayName = "Volume LastSynchronizedOn Item", volumeName = "Volume_LastSynchronizedOn_Name";
            VolumeIdentifier identifier = new(Guid.NewGuid());
            Volume target = new() { DisplayName = displayName, VolumeName = volumeName, Identifier = identifier, FileSystem = fileSystem, UpstreamId = Guid.NewGuid() };
            EntityEntry<Volume> entityEntry = dbContext.Volumes.Add(target);
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
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Volumes.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }
    }
}
