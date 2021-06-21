using FsInfoCat;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class SubdirectoryTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new Subdirectory()")]
        public void NewSubdirectoryTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            Subdirectory target = new();

            EntityEntry<Subdirectory> entry = dbContext.Entry(target);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(DirectoryCrawlOptions.None, target.Options);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.AreEqual(target.Status, DirectoryStatus.Incomplete);
            Assert.IsNull(target.ParentId);
            Assert.IsNull(target.VolumeId);
            Assert.IsNull(target.Parent);
            Assert.IsNull(target.Volume);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            Assert.IsNotNull(target.SubDirectories);
            Assert.AreEqual(0, target.SubDirectories.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new Subdirectory()

            dbContext.Subdirectories.Add(target);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(DirectoryCrawlOptions.None, target.Options);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.AreEqual(target.Status, DirectoryStatus.Incomplete);
            Assert.IsNull(target.ParentId);
            Assert.IsNull(target.VolumeId);
            Assert.IsNull(target.Parent);
            Assert.IsNull(target.Volume);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            Assert.IsNotNull(target.SubDirectories);
            Assert.AreEqual(0, target.SubDirectories.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);
        }

        [TestMethod("Guid Id")]
        [Ignore]
        public void IdTestMethod()
        {
            Subdirectory target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("string Name")]
        public void NameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Name

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            string expectedValue = default;
            target.Name = default;
            string actualValue = target.Name;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DirectoryCrawlOptions Options")]
        public void OptionsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DirectoryCrawlOptions Options

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            DirectoryCrawlOptions expectedValue = default;
            target.Options = default;
            DirectoryCrawlOptions actualValue = target.Options;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime LastAccessed")]
        public void LastAccessedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime LastAccessed

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            DateTime expectedValue = default;
            target.LastAccessed = default;
            DateTime actualValue = target.LastAccessed;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [DataTestMethod]
        [DataRow(null, "", DisplayName = "string Notes = null")]
        [DataRow("", "", DisplayName = "string Notes = \"\"")]
        [DataRow("\n\r", "", DisplayName = "string Notes = \"\\n\\r\"")]
        [DataRow("Test", "Test", DisplayName = "string Notes = \"Test\"")]
        [DataRow("\n Test \r", "\n Test \r", DisplayName = "string Notes = \"\\n Test \\r\"")]
        public void NotesTestMethod(string notes, string expected)
        {
            Subdirectory target = new();
            target.Notes = notes;
            string actualValue = target.Notes;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("DirectoryStatus Status")]
        public void StatusTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool Deleted

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            DirectoryStatus expectedValue = default;
            target.Status = default;
            DirectoryStatus actualValue = target.Status;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? ParentId")]
        public void ParentIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? ParentId

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            Guid? expectedValue = default;
            target.ParentId = default;
            Guid? actualValue = target.ParentId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? VolumeId")]
        public void VolumeIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? VolumeId

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            Guid? expectedValue = default;
            target.VolumeId = default;
            Guid? actualValue = target.VolumeId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Subdirectory Parent")]
        public void ParentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Subdirectory Parent

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            Subdirectory expectedValue = default;
            target.Parent = default;
            Subdirectory actualValue = target.Parent;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Volume Volume")]
        public void VolumeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Volume Volume

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            Volume expectedValue = default;
            target.Volume = default;
            Volume actualValue = target.Volume;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<DbFile> Files")]
        public void FilesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<DbFile> Files

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            HashSet<DbFile> expectedValue = default;
            target.Files = default;
            HashSet<DbFile> actualValue = target.Files;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<Subdirectory> SubDirectories")]
        public void SubDirectoriesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<Subdirectory> SubDirectories

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            HashSet<Subdirectory> expectedValue = default;
            target.SubDirectories = default;
            HashSet<Subdirectory> actualValue = target.SubDirectories;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<SubdirectoryAccessError> AccessErrors")]
        public void AccessErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<SubdirectoryAccessError> AccessErrors

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            HashSet<SubdirectoryAccessError> expectedValue = default;
            target.AccessErrors = default;
            HashSet<SubdirectoryAccessError> actualValue = target.AccessErrors;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            Guid? expectedValue = default;
            target.UpstreamId = default;
            Guid? actualValue = target.UpstreamId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? LastSynchronizedOn")]
        [TestProperty(TestHelper.TestProperty_Description,
            "Volume.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void LastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime? LastSynchronizedOn

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            DateTime? expectedValue = default;
            target.LastSynchronizedOn = default;
            DateTime? actualValue = target.LastSynchronizedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn")]
        [TestProperty(TestHelper.TestProperty_Description, "BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime CreatedOn

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            DateTime expectedValue = default;
            target.CreatedOn = default;
            DateTime actualValue = target.CreatedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime ModifiedOn")]
        public void ModifiedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime ModifiedOn

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            DateTime expectedValue = default;
            target.ModifiedOn = default;
            DateTime actualValue = target.ModifiedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Item[string]")]
        public void ItemColumnNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Item[string]

            string columnNameIndex = default;
            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            string expectedValue = default;
            string actualValue = target[columnNameIndex];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("IEnumerable<ValidationResult> Validate(ValidationContext)")]
        public void ValidateValidationContextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for IEnumerable<ValidationResult> Validate(ValidationContext)

            ValidationContext validationContextArg = default;
            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            string expectedReturnValue = default;
            string actualReturnValue = target.ToString();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool Equals(object)")]
        public void EqualsObjectTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool Equals(object)

            object objArg = default;
            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            Subdirectory target = default; // TODO: Create and initialize Subdirectory instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
