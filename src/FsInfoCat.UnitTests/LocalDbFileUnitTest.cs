using FsInfoCat.Collections;
using FsInfoCat.Local.Model;
using FsInfoCat.Model;
using FsInfoCat.UnitTests.TestData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalDbFileUnitTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void OnTestInitialize()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            TestHelper.UndoChanges(dbContext);
        }

        [TestMethod("DbFile Constructor Tests")]
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
            bool hasId = target.TryGetId(out _);
            Assert.IsFalse(hasId);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.AreEqual(string.Empty, target.Name);
            Assert.AreEqual(string.Empty, target.Notes);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.AreEqual(FileCorrelationStatus.Dissociated, target.Status);
            Assert.IsNull(target.Parent);
            hasId = target.TryGetParentId(out _);
            Assert.IsFalse(hasId);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            Assert.IsNull(target.BinaryProperties);
            hasId = target.TryGetBinaryPropertySetId(out _);
            Assert.IsFalse(hasId);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertySetId);
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

        [TestMethod]
        public void EqualsTestMethod1()
        {
            DbFile target = new();
            DbFile other = null;
            Assert.IsFalse(target.Equals(other));
            other = new();
            Assert.AreEqual(target.CreatedOn.Equals(other.CreatedOn), target.Equals(other));
        }

        private static IEnumerable<object[]> GetEqualsTestData() => TestFileData.GetEqualsTestData().Select((tuple, index) => new object[] { tuple.target, tuple.other, tuple.expectedResult, index });

        [TestMethod]
        [DynamicData(nameof(GetEqualsTestData), DynamicDataSourceType.Method)]
        public void EqualsTestMethod2(DbFile target, DbFile other, bool expectedResult, int index)
        {
            bool actualResult = target.Equals(other);
            Assert.AreEqual(expectedResult, actualResult, $"Failed test at index {index}");
        }

        [TestMethod("DbFile Name Validation Tests")]
        [Description("DbFile.Name: NVARCHAR(1024) NOT NULL CHECK(length(trim(Name))>0) COLLATE NOCASE")]
        public void DbFileNameTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new()
            {
                Name = null,
                CreationTime = TestFileData.Item1.CreationTime,
                LastWriteTime = TestFileData.Item1.LastWriteTime,
                LastAccessed = TestFileData.Item1.LastAccessed,
                CreatedOn = TestFileData.Item1.CreatedOn,
                ModifiedOn = TestFileData.Item1.ModifiedOn,
                Parent = dbContext.Subdirectories.Find(new Guid("3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a")),
                BinaryProperties = dbContext.BinaryPropertySets.Find(new Guid("82d46e21-5eba-4f1b-8c99-78cb94689316"))
            };
            if (target.Parent is null) Assert.Inconclusive("Could not find parent subdirectory");
            if (target.BinaryProperties is null) Assert.Inconclusive("Could not find binary property set");
            string expected = "";
            Assert.AreEqual(expected, target.Name);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);

            target.Name = " \n ";
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);

            expected = "Test";
            target.Name = expected;
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            target.Name = " Test\nName\t";
            expected = "Test Name";
            target.Name = expected;
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            target.Name = "";
            expected = "";
            target.Name = expected;
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
        }

        [TestMethod("DbFile Name Validation Tests")]
        [Description("DbFile.Name: NVARCHAR(1024) NOT NULL CHECK(length(trim(Name))>0) COLLATE NOCASE")]
        public void DbFileCreationTimeTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DateTime expectedCreationTime = TestFileData.Item1.CreationTime;
            DateTime expectedLastWriteTime = TestFileData.Item1.LastWriteTime;
            DbFile target = new()
            {
                Name = TestFileData.Item1.Name,
                CreationTime = expectedCreationTime,
                LastWriteTime = expectedLastWriteTime,
                LastAccessed = TestFileData.Item1.LastAccessed,
                CreatedOn = TestFileData.Item1.CreatedOn,
                ModifiedOn = TestFileData.Item1.ModifiedOn,
                Parent = dbContext.Subdirectories.Find(new Guid("3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a")),
                BinaryProperties = dbContext.BinaryPropertySets.Find(new Guid("82d46e21-5eba-4f1b-8c99-78cb94689316"))
            };
            if (target.Parent is null) Assert.Inconclusive("Could not find parent subdirectory");
            if (target.BinaryProperties is null) Assert.Inconclusive("Could not find binary property set");
            Assert.AreEqual(expectedCreationTime, target.CreationTime);
            Assert.AreEqual(expectedLastWriteTime, target.LastWriteTime);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.CreationTime), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
        }

        [TestMethod("DbFile Name Validation Tests")]
        [Description("DbFile.Name: NVARCHAR(1024) NOT NULL CHECK(length(trim(Name))>0) COLLATE NOCASE")]
        public void DbFileLastHashCalculationTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DateTime expected = TestFileData.Item1.LastHashCalculation;
            DbFile target = new()
            {
                Name = TestFileData.Item1.Name,
                LastHashCalculation = expected,
                CreationTime = TestFileData.Item1.CreationTime,
                LastWriteTime = TestFileData.Item1.LastWriteTime,
                LastAccessed = TestFileData.Item1.LastAccessed,
                CreatedOn = TestFileData.Item1.CreatedOn,
                ModifiedOn = TestFileData.Item1.ModifiedOn,
                Parent = dbContext.Subdirectories.Find(new Guid("3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a")),
                BinaryProperties = dbContext.BinaryPropertySets.Find(new Guid("82d46e21-5eba-4f1b-8c99-78cb94689316"))
            };
            if (target.Parent is null) Assert.Inconclusive("Could not find parent subdirectory");
            if (target.BinaryProperties is null) Assert.Inconclusive("Could not find binary property set");
            Assert.AreEqual(expected, target.LastHashCalculation);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.CreationTime), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
        }

        [TestMethod("DbFile Add/Remove Tests"), Ignore]
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
        public void DbFileBinaryPropertiesTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new()
            {
                Name = TestFileData.Item1.Name,
                CreationTime = TestFileData.Item1.CreationTime,
                LastWriteTime = TestFileData.Item1.LastWriteTime,
                LastAccessed = TestFileData.Item1.LastAccessed,
                CreatedOn = TestFileData.Item1.CreatedOn,
                ModifiedOn = TestFileData.Item1.ModifiedOn,
                Parent = dbContext.Subdirectories.Find(new Guid("3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a"))
            };
            if (target.Parent is null) Assert.Inconclusive("Could not find parent subdirectory");

            bool success = target.TryGetBinaryPropertySetId(out Guid actualId);
            Assert.IsFalse(success);
            Collection<ValidationResult> results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_BinaryPropertiesRequired, results[0].ErrorMessage);

            #region  Set navigation entity when no foreign entity association

            Guid expectedId = new("82d46e21-5eba-4f1b-8c99-78cb94689316");
            BinaryPropertySet expectedNav = dbContext.BinaryPropertySets.Find(expectedId);
            if (expectedNav is null) Assert.Inconclusive("Could not find binary property set");
            target.BinaryProperties = expectedNav;
            Assert.IsNotNull(target.BinaryProperties);
            Assert.AreSame(expectedNav, target.BinaryProperties);
            Assert.AreEqual(expectedId, target.BinaryPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity to equal entity object

            expectedNav = TestBinaryPropertySetData.CreateClone(expectedNav);
            target.BinaryProperties = expectedNav;
            Assert.IsNotNull(target.BinaryProperties);
            Assert.AreSame(expectedNav, target.BinaryProperties);
            Assert.AreEqual(expectedId, target.BinaryPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation identifier when navigation entity has same identifier

            target.BinaryPropertySetId = expectedId;
            Assert.IsNotNull(target.BinaryProperties);
            Assert.AreSame(expectedNav, target.BinaryProperties);
            Assert.AreEqual(expectedId, target.BinaryPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity to different entity object

            expectedId = new Guid("7c9565e8-11c5-4275-8b43-043af0ac1151");
            expectedNav = dbContext.BinaryPropertySets.Find(expectedId);
            if (expectedNav is null) Assert.Inconclusive("Could not find binary property set");
            target.BinaryProperties = expectedNav;
            Assert.IsNotNull(target.BinaryProperties);
            Assert.AreSame(expectedNav, target.BinaryProperties);
            Assert.AreEqual(expectedId, target.BinaryPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity null when navigation entity is not null

            target.BinaryProperties = null;
            Assert.IsNull(target.BinaryProperties);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertySetId);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsFalse(success);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.BinaryProperties), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_BinaryPropertiesRequired, results[0].ErrorMessage);

            #endregion
            #region Set navigation identifier when no foreign entity association

            expectedId = new("82d46e21-5eba-4f1b-8c99-78cb94689316");
            target.BinaryPropertySetId = expectedId;
            Assert.AreEqual(expectedId, target.BinaryPropertySetId);
            Assert.IsNull(target.BinaryProperties);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity when foreign entity is associated by equal Guid-only

            expectedNav = dbContext.BinaryPropertySets.Find(expectedId);
            if (expectedNav is null) Assert.Inconclusive("Could not find binary property set");
            target.BinaryProperties = expectedNav;
            Assert.IsNotNull(target.BinaryProperties);
            Assert.AreSame(expectedNav, target.BinaryProperties);
            Assert.AreEqual(expectedId, target.BinaryPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation identifier when navigation entity has different identifier

            expectedId = Guid.NewGuid();
            target.BinaryPropertySetId = expectedId;
            Assert.AreEqual(expectedId, target.BinaryPropertySetId);
            Assert.IsNull(target.BinaryProperties);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity when foreign entity is associated by other Guid-only

            expectedId = expectedNav.Id;
            target.BinaryProperties = expectedNav;
            Assert.IsNotNull(target.BinaryProperties);
            Assert.AreSame(expectedNav, target.BinaryProperties);
            Assert.AreEqual(expectedId, target.BinaryPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation identifier when foreign entity is associated by other Guid-only

            target.BinaryPropertySetId = Guid.NewGuid();
            target.BinaryPropertySetId = expectedId;
            Assert.AreEqual(expectedId, target.BinaryPropertySetId);
            Assert.IsNull(target.BinaryProperties);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity null when foreign entity is associated by Guid-only

            target.BinaryProperties = null;
            Assert.IsNull(target.BinaryProperties);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertySetId);
            success = target.TryGetBinaryPropertySetId(out actualId);
            Assert.IsFalse(success);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.BinaryProperties), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_BinaryPropertiesRequired, results[0].ErrorMessage);

            #endregion
        }

        [TestMethod("DbFile Parent Validation Tests")]
        [Description("DbFile.Parent: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES Subdirectories")]
        public void DbFileParentTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new()
            {
                Name = TestFileData.Item1.Name,
                CreationTime = TestFileData.Item1.CreationTime,
                LastWriteTime = TestFileData.Item1.LastWriteTime,
                LastAccessed = TestFileData.Item1.LastAccessed,
                CreatedOn = TestFileData.Item1.CreatedOn,
                ModifiedOn = TestFileData.Item1.ModifiedOn,
                BinaryProperties = dbContext.BinaryPropertySets.Find(new Guid("82d46e21-5eba-4f1b-8c99-78cb94689316"))
            };
            if (target.BinaryProperties is null) Assert.Inconclusive("Could not find binary property set");

            bool success = target.TryGetParentId(out Guid actualId);
            Assert.IsFalse(success);
            Collection<ValidationResult> results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);

            #region Set navigation entity when no foreign entity association

            Guid expectedId = new("3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a");
            Subdirectory expectedNav = dbContext.Subdirectories.Find(expectedId);
            if (expectedNav is null) Assert.Inconclusive("Could not find parent subdirectory");
            target.Parent = expectedNav;
            Assert.IsNotNull(target.Parent);
            Assert.AreSame(expectedNav, target.Parent);
            Assert.AreEqual(expectedId, target.ParentId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetParentId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity to equal entity object

            expectedNav = TestSubdirectoryData.CreateClone(expectedNav);
            target.Parent = expectedNav;
            Assert.IsNotNull(target.Parent);
            Assert.AreSame(expectedNav, target.Parent);
            Assert.AreEqual(expectedId, target.ParentId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetParentId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation identifier when navigation entity has same identifier

            target.ParentId = expectedId;
            Assert.IsNotNull(target.Parent);
            Assert.AreSame(expectedNav, target.Parent);
            Assert.AreEqual(expectedId, target.ParentId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetParentId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity to different entity object

            expectedId = new Guid("20a61d4b-80c2-48a3-8df6-522e598aae08");
            expectedNav = dbContext.Subdirectories.Find(expectedId);
            if (expectedNav is null) Assert.Inconclusive("Could not find parent subdirectory");
            target.Parent = expectedNav;
            Assert.IsNotNull(target.Parent);
            Assert.AreSame(expectedNav, target.Parent);
            Assert.AreEqual(expectedId, target.ParentId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetParentId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity null when navigation entity is not null

            target.Parent = null;
            Assert.IsNull(target.Parent);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            success = target.TryGetParentId(out actualId);
            Assert.IsFalse(success);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);

            #endregion
            #region Set navigation identifier when no foreign entity association

            expectedId = new("3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a");
            target.ParentId = expectedId;
            Assert.AreEqual(expectedId, target.ParentId);
            Assert.IsNull(target.Parent);
            success = target.TryGetParentId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity when foreign entity is associated by equal Guid-only

            expectedNav = dbContext.Subdirectories.Find(expectedId);
            if (expectedNav is null) Assert.Inconclusive("Could not find parent subdirectory");
            target.Parent = expectedNav;
            Assert.IsNotNull(target.Parent);
            Assert.AreSame(expectedNav, target.Parent);
            Assert.AreEqual(expectedId, target.ParentId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetParentId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation identifier when navigation entity has different identifier

            expectedId = Guid.NewGuid();
            target.ParentId = expectedId;
            Assert.AreEqual(expectedId, target.ParentId);
            Assert.IsNull(target.Parent);
            success = target.TryGetParentId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity when foreign entity is associated by other Guid-only

            expectedId = expectedNav.Id;
            target.Parent = expectedNav;
            Assert.IsNotNull(target.Parent);
            Assert.AreSame(expectedNav, target.Parent);
            Assert.AreEqual(expectedId, target.ParentId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            success = target.TryGetParentId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation identifier when foreign entity is associated by other Guid-only

            target.ParentId = Guid.NewGuid();
            target.ParentId = expectedId;
            Assert.AreEqual(expectedId, target.ParentId);
            Assert.IsNull(target.Parent);
            success = target.TryGetParentId(out actualId);
            Assert.IsTrue(success);
            Assert.AreEqual(expectedId, actualId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity null when foreign entity is associated by Guid-only

            target.Parent = null;
            Assert.IsNull(target.Parent);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            success = target.TryGetParentId(out actualId);
            Assert.IsFalse(success);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);

            #endregion
        }

        [TestMethod("DbFile Options Validation Tests")]
        [Description("DbFile.Options: TINYINT  NOT NULL CHECK(Options>=0 AND Options<15)")]
        public void DbFileOptionsTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new()
            {
                Name = null,
                CreationTime = TestFileData.Item1.CreationTime,
                LastWriteTime = TestFileData.Item1.LastWriteTime,
                LastAccessed = TestFileData.Item1.LastAccessed,
                CreatedOn = TestFileData.Item1.CreatedOn,
                ModifiedOn = TestFileData.Item1.ModifiedOn,
                Parent = dbContext.Subdirectories.Find(new Guid("3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a")),
                BinaryProperties = dbContext.BinaryPropertySets.Find(new Guid("82d46e21-5eba-4f1b-8c99-78cb94689316"))
            };
            if (target.Parent is null) Assert.Inconclusive("Could not find parent subdirectory");
            if (target.BinaryProperties is null) Assert.Inconclusive("Could not find binary property set");
            FileCrawlOptions expected = FileCrawlOptions.None;
            Assert.AreEqual(expected, target.Options);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            expected = FileCrawlOptions.DoNotCompare;
            target.Options = expected;
            Assert.AreEqual(expected, target.Options);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            expected = FileCrawlOptions.Ignore | FileCrawlOptions.FlaggedForRescan;
            target.Options = expected;
            Assert.AreEqual(expected, target.Options);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            expected = FileCrawlOptions.None;
            target.Options = expected;
            Assert.AreEqual(expected, target.Options);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod("DbFile CreatedOn Validation Tests")]
        [Description("DbFile.CreatedOn: CreatedOn<=ModifiedOn")]
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
            Assert.AreEqual(nameof(DbFile.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.Files.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("DbFile LastSynchronizedOn Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description,
            "DbFile.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void DbFileLastSynchronizedOnTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new()
            {
                Name = null,
                CreationTime = TestFileData.Item1.CreationTime,
                LastWriteTime = TestFileData.Item1.LastWriteTime,
                LastAccessed = TestFileData.Item1.LastAccessed,
                CreatedOn = TestFileData.Item1.CreatedOn,
                ModifiedOn = TestFileData.Item1.ModifiedOn,
                Parent = dbContext.Subdirectories.Find(new Guid("3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a")),
                BinaryProperties = dbContext.BinaryPropertySets.Find(new Guid("82d46e21-5eba-4f1b-8c99-78cb94689316"))
            };
            if (target.Parent is null) Assert.Inconclusive("Could not find parent subdirectory");
            if (target.BinaryProperties is null) Assert.Inconclusive("Could not find binary property set");
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

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
        }

        [TestMethod("DbFile AudioProperties Validation Tests")]
        [Description("DbFile.AudioProperties: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES AudioPropertySets")]
        public void DbFileAudioPropertiesTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new()
            {
                Name = TestFileData.Item1.Name,
                CreationTime = TestFileData.Item1.CreationTime,
                LastWriteTime = TestFileData.Item1.LastWriteTime,
                LastAccessed = TestFileData.Item1.LastAccessed,
                CreatedOn = TestFileData.Item1.CreatedOn,
                ModifiedOn = TestFileData.Item1.ModifiedOn,
                Parent = dbContext.Subdirectories.Find(new Guid("3dfc92c9-8af0-4ab6-bcc3-9104fdcdc35a")),
                BinaryProperties = dbContext.BinaryPropertySets.Find(new Guid("82d46e21-5eba-4f1b-8c99-78cb94689316"))
            };
            if (target.Parent is null) Assert.Inconclusive("Could not find parent subdirectory");
            if (target.BinaryProperties is null) Assert.Inconclusive("Could not find binary property set");

            Assert.IsNull(target.AudioProperties);
            Assert.IsNull(target.AudioPropertySetId);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #region Set navigation entity when no foreign entity association

            Guid expectedId = new("0cb85868-c17c-4216-8deb-89b179e69a04");
            AudioPropertySet expectedNav = dbContext.AudioPropertySets.Find(expectedId);
            if (expectedNav is null) Assert.Inconclusive("Could not find parent subdirectory");
            target.AudioProperties = expectedNav;
            Assert.IsNotNull(target.AudioProperties);
            Assert.AreSame(expectedNav, target.AudioProperties);
            Assert.IsNotNull(target.AudioPropertySetId);
            Assert.AreEqual(expectedId, target.AudioPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity to equal entity object

            expectedNav = TestAudioPropertySetData.CreateClone(expectedNav);
            target.AudioProperties = expectedNav;
            Assert.IsNotNull(target.AudioProperties);
            Assert.AreSame(expectedNav, target.AudioProperties);
            Assert.IsNotNull(target.AudioPropertySetId);
            Assert.AreEqual(expectedId, target.AudioPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation identifier when navigation entity has same identifier

            target.AudioPropertySetId = expectedId;
            Assert.IsNotNull(target.AudioProperties);
            Assert.AreSame(expectedNav, target.AudioProperties);
            Assert.IsNotNull(target.AudioPropertySetId);
            Assert.AreEqual(expectedId, target.AudioPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity to different entity object

            expectedId = new Guid("dbc833c1-7b61-424c-9034-ba421123101d");
            expectedNav = dbContext.AudioPropertySets.Find(expectedId);
            if (expectedNav is null) Assert.Inconclusive("Could not find parent subdirectory");
            target.AudioProperties = expectedNav;
            Assert.IsNotNull(target.AudioProperties);
            Assert.AreSame(expectedNav, target.AudioProperties);
            Assert.IsNotNull(target.AudioPropertySetId);
            Assert.AreEqual(expectedId, target.AudioPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity null when navigation entity is not null

            target.AudioProperties = null;
            Assert.IsNull(target.AudioProperties);
            Assert.IsNull(target.AudioPropertySetId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);

            #endregion
            #region Set navigation identifier when no foreign entity association

            expectedId = new("0cb85868-c17c-4216-8deb-89b179e69a04");
            target.AudioPropertySetId = expectedId;
            Assert.IsNotNull(target.AudioPropertySetId);
            Assert.AreEqual(expectedId, target.AudioPropertySetId);
            Assert.IsNull(target.AudioProperties);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity when foreign entity is associated by equal Guid-only

            expectedNav = dbContext.AudioPropertySets.Find(expectedId);
            if (expectedNav is null) Assert.Inconclusive("Could not find parent subdirectory");
            target.AudioProperties = expectedNav;
            Assert.IsNotNull(target.AudioProperties);
            Assert.AreSame(expectedNav, target.AudioProperties);
            Assert.IsNotNull(target.AudioPropertySetId);
            Assert.AreEqual(expectedId, target.AudioPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation identifier when navigation entity has different identifier

            expectedId = Guid.NewGuid();
            target.AudioPropertySetId = expectedId;
            Assert.IsNotNull(target.AudioPropertySetId);
            Assert.AreEqual(expectedId, target.AudioPropertySetId);
            Assert.IsNull(target.AudioProperties);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity when foreign entity is associated by other Guid-only

            expectedId = expectedNav.Id;
            target.AudioProperties = expectedNav;
            Assert.IsNotNull(target.AudioProperties);
            Assert.AreSame(expectedNav, target.AudioProperties);
            Assert.IsNotNull(target.AudioPropertySetId);
            Assert.AreEqual(expectedId, target.AudioPropertySetId);
            Assert.AreEqual(expectedId, expectedNav.Id);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation identifier when foreign entity is associated by other Guid-only

            target.AudioPropertySetId = Guid.NewGuid();
            target.AudioPropertySetId = expectedId;
            Assert.IsNotNull(target.AudioPropertySetId);
            Assert.AreEqual(expectedId, target.AudioPropertySetId);
            Assert.IsNull(target.AudioProperties);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
            #region Set navigation entity null when foreign entity is associated by Guid-only

            target.AudioProperties = null;
            Assert.IsNull(target.AudioProperties);
            Assert.IsNull(target.AudioPropertySetId);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);

            #endregion
        }
    }
}
