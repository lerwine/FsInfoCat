using FsInfoCat.Local.Model;
using FsInfoCat.Model;
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
    public class LocalSubdirectoryUnitTest
    {
        private static TestContext _testContext;

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

        [TestMethod("Subdirectory Constructor Tests"), Ignore]
        public void SubdirectoryConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            Subdirectory target = new();
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
            Assert.AreEqual(DirectoryCrawlOptions.None, target.Options);
            Assert.AreEqual(DirectoryStatus.Incomplete, target.Status);
            Assert.IsNull(target.CrawlConfiguration);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            Assert.IsNull(target.Parent);
            Assert.IsNull(target.ParentId);
            Assert.IsNotNull(target.PersonalTags);
            Assert.AreEqual(0, target.PersonalTags.Count);
            Assert.IsNotNull(target.SharedTags);
            Assert.AreEqual(0, target.SharedTags.Count);
            Assert.IsNotNull(target.PersonalTags);
            Assert.AreEqual(0, target.PersonalTags.Count);
            Assert.IsNotNull(target.SharedTags);
            Assert.AreEqual(0, target.SharedTags.Count);
            Assert.IsNotNull(target.SubDirectories);
            Assert.AreEqual(0, target.SubDirectories.Count);
            Assert.IsNull(target.Volume);
            Assert.IsNull(target.VolumeId);
        }

        [TestMethod("Subdirectory Add/Remove Tests"), Ignore]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void SubdirectoryAddRemoveTestMethod()
        {
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
            Subdirectory target1 = new() { Volume = volume1 };
            EntityEntry<Subdirectory> entityEntry = dbContext.Entry(target1);
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
            Assert.AreEqual(target1.Status, DirectoryStatus.Incomplete);
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

        [TestMethod("Subdirectory Name Validation Tests"), Ignore]
        [Description("Subdirectory.Name: NVARCHAR(1024) NOT NULL (ParentId IS NULL OR length(trim(Name))>0) COLLATE NOCASE")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void SubdirectoryNameTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expectedName = "";
            FileSystem fileSystem1 = new() { DisplayName = "Subdirectory NameTest FileSystem" };
            dbContext.FileSystems.Add(fileSystem1);
            Volume volume1 = new()
            {
                DisplayName = "Subdirectory NameTest Item",
                VolumeName = "Subdirectory_NameTest_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1
            };
            dbContext.Volumes.Add(volume1);
            Subdirectory parent1 = new() { Volume = volume1 };
            dbContext.Subdirectories.Add(parent1);
            dbContext.SaveChanges();
            Subdirectory target1 = new();
            EntityEntry<Subdirectory> entityEntry = dbContext.Subdirectories.Add(target1);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target1, new ValidationContext(target1), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Subdirectory.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeOrParentRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expectedName, target1.Name);

            target1.Parent = parent1;
            results = new();
            success = Validator.TryValidateObject(target1, new ValidationContext(target1), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Subdirectory.Name), results[0].MemberNames.First());
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
            Assert.AreEqual(nameof(Subdirectory.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameLength, results[0].ErrorMessage);
            dbContext.Subdirectories.Update(target1);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expectedName2, target1.Name);

            target1.Name = expectedName;
            dbContext.SaveChanges();

            Volume volume2 = new()
            {
                DisplayName = "Subdirectory NameTest Item 2",
                VolumeName = "Subdirectory_NameTest_Name 2",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1
            };
            dbContext.Volumes.Add(volume2);
            dbContext.SaveChanges();
            Volume volume3 = new()
            {
                DisplayName = "Subdirectory NameTest Item 3",
                VolumeName = "Subdirectory_NameTest_Name 3",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1
            };
            dbContext.Volumes.Add(volume3);
            dbContext.SaveChanges();

            Subdirectory target2 = new() { Volume = volume2, Name = expectedName };
            Assert.AreEqual(expectedName, target2.Name);
            entityEntry = dbContext.Subdirectories.Add(target2);
            results = new();
            success = Validator.TryValidateObject(target2, new ValidationContext(target2), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();

            Subdirectory target3 = new() { Parent = parent1, Name = expectedName };
            entityEntry = dbContext.Subdirectories.Add(target3);
            results = new();
            success = Validator.TryValidateObject(target3, new ValidationContext(target3), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Subdirectory.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target3.Name = $"{expectedName[1..]}2";
            dbContext.SaveChanges();

            expectedName = $"{expectedName[1..]}3";
            Subdirectory target4 = new() { Volume = volume3, Parent = parent1, Name = expectedName };
            entityEntry = dbContext.Subdirectories.Add(target4);
            results = new();
            success = Validator.TryValidateObject(target4, new ValidationContext(target4), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Subdirectory.Volume), results[0].MemberNames.First());
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
            Assert.AreEqual(nameof(Subdirectory.Volume), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_VolumeHasRoot, results[0].ErrorMessage);
            dbContext.Subdirectories.Update(target4);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expectedName, target4.Name);

            target4.Volume = volume3;
            dbContext.Subdirectories.Update(target4);
            target2.Volume = volume2;
            dbContext.SaveChanges();
        }

        [TestMethod("Subdirectory Options Validation Tests"), Ignore]
        [Description("Subdirectory.Options: TINYINT NOT NULL TINYINT  NOT NULL CHECK(Options>=0 AND Options<64)")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void SubdirectoryOptionsTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem fileSystem = new() { DisplayName = "Subdirectory OptionsTest FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Volume volume = new()
            {
                DisplayName = "Subdirectory OptionsTest Item",
                VolumeName = "Subdirectory_OptionsTest_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem
            };
            dbContext.Volumes.Add(volume);
            Subdirectory parent = new() { Volume = volume };
            dbContext.Subdirectories.Add(parent);
            dbContext.SaveChanges();
            DirectoryCrawlOptions expected = (DirectoryCrawlOptions)(object)(byte)64;
            Subdirectory target = new() { Options = expected, Name = "OptionsTest Dir", Parent = parent };
            EntityEntry<Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Subdirectory.Options), results[0].MemberNames.First());
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

        [TestMethod("Subdirectory CreatedOn Validation Tests"), Ignore]
        [Description("Subdirectory.CreatedOn: CreatedOn<=ModifiedOn")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void SubdirectoryCreatedOnTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem fileSystem = new() { DisplayName = "Subdirectory CreatedOnTest FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Volume volume = new()
            {
                DisplayName = "Subdirectory CreatedOnTest Item",
                VolumeName = "Subdirectory_CreatedOnTest_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem
            };
            dbContext.Volumes.Add(volume);
            Subdirectory parent = new() { Volume = volume };
            dbContext.Subdirectories.Add(parent);
            dbContext.SaveChanges();
            Subdirectory target = new() { Name = "CreatedOnTest Dir", Parent = parent };
            EntityEntry<Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Subdirectory.CreatedOn), results[0].MemberNames.First());
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

        [TestMethod("Subdirectory LastSynchronizedOn Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description,
            "Subdirectory.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        [TestCategory(TestHelper.TestCategory_LocalDb)]
        public void SubdirectoryLastSynchronizedOnTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem fileSystem = new() { DisplayName = "Subdirectory LastSynchronizedOn FileSystem" };
            dbContext.FileSystems.Add(fileSystem);
            Volume volume = new()
            {
                DisplayName = "Subdirectory LastSynchronizedOn Item",
                VolumeName = "Subdirectory_LastSynchronizedOn_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem
            };
            dbContext.Volumes.Add(volume);
            Subdirectory parent = new() { Volume = volume };
            dbContext.Subdirectories.Add(parent);
            dbContext.SaveChanges();
            Subdirectory target = new() { UpstreamId = Guid.NewGuid(), Parent = parent, Name = "LastSynchronizedOn Dir" };
            EntityEntry<Subdirectory> entityEntry = dbContext.Subdirectories.Add(target);
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
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Subdirectories.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }
    }
}
