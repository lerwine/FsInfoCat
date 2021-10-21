using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class FileSystemTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new FileSystem()"), Ignore]
        public void NewFileSystemTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem target = new();

            EntityEntry<FileSystem> entry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNotNull(target.DisplayName);
            Assert.AreEqual("", target.DisplayName);
            Assert.IsFalse(target.ReadOnly);
            Assert.AreEqual(255u, target.MaxNameLength);
            Assert.IsNull(target.DefaultDriveType);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsFalse(target.IsInactive);
            Assert.IsNull(target.DefaultDriveType);
            Assert.IsNotNull(target.Notes);
            Assert.IsNull(target.DefaultDriveType);
            Assert.IsNotNull(target.Volumes);
            Assert.AreEqual(0, target.Volumes.Count);
            Assert.IsNotNull(target.SymbolicNames);
            Assert.AreEqual(0, target.SymbolicNames.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for new FileSystem()

            dbContext.FileSystems.Add(target);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.IsNotNull(target.DisplayName);
            Assert.AreEqual("", target.DisplayName);
            Assert.IsFalse(target.ReadOnly);
            Assert.AreEqual(255, target.MaxNameLength);
            Assert.IsNull(target.DefaultDriveType);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsFalse(target.IsInactive);
            Assert.IsNull(target.DefaultDriveType);
            Assert.IsNotNull(target.Notes);
            Assert.IsNull(target.DefaultDriveType);
            Assert.IsNotNull(target.Volumes);
            Assert.AreEqual(0, target.Volumes.Count);
            Assert.IsNotNull(target.SymbolicNames);
            Assert.AreEqual(0, target.SymbolicNames.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
        }

        [TestMethod("Guid Id"), Ignore]
        public void IdTestMethod()
        {
            FileSystem target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [DataTestMethod]
        [DataRow(null, "", DisplayName = "string DisplayName = null")]
        [DataRow("", "", DisplayName = "string DisplayName = \"\"")]
        [DataRow("\n\r", "", DisplayName = "string DisplayName = \"\\n\\r\"")]
        [DataRow("Test", "Test", DisplayName = "string DisplayName = \"Test\"")]
        [DataRow("\n Test \r", "Test", DisplayName = "string DisplayName = \"\\n Test \\r\"")]
        public void DisplayNameTestMethod(string displayName, string expected)
        {
            FileSystem target = new();
            target.DisplayName = displayName;
            string actualValue = target.DisplayName;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("bool ReadOnly"), Ignore]
        public void ReadOnlyTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool ReadOnly

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            bool expectedValue = default;
            target.ReadOnly = default;
            bool actualValue = target.ReadOnly;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("int MaxNameLength"), Ignore]
        public void MaxNameLengthTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for int MaxNameLength

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            uint expectedValue = default;
            target.MaxNameLength = default;
            uint actualValue = target.MaxNameLength;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DriveType? DefaultDriveType"), Ignore]
        public void DefaultDriveTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DriveType? DefaultDriveType

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            DriveType? expectedValue = default;
            target.DefaultDriveType = default;
            DriveType? actualValue = target.DefaultDriveType;
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
            FileSystem target = new();
            target.Notes = notes;
            string actualValue = target.Notes;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("bool IsInactive"), Ignore]
        public void IsInactiveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool IsInactive

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            bool expectedValue = default;
            target.IsInactive = default;
            bool actualValue = target.IsInactive;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<Volume> Volumes"), Ignore]
        public void VolumesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for HashSet<Volume> Volumes

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            HashSet<Volume> expectedValue = default;
            target.Volumes = default;
            HashSet<Volume> actualValue = target.Volumes;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<SymbolicName> SymbolicNames"), Ignore]
        public void SymbolicNamesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for HashSet<SymbolicName> SymbolicNames

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            HashSet<SymbolicName> expectedValue = default;
            target.SymbolicNames = default;
            HashSet<SymbolicName> actualValue = target.SymbolicNames;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId"), Ignore]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? UpstreamId

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            Guid? expectedValue = default;
            target.UpstreamId = default;
            Guid? actualValue = target.UpstreamId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? LastSynchronizedOn"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description,
            "Volume.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void LastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime? LastSynchronizedOn

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            DateTime? expectedValue = default;
            target.LastSynchronizedOn = default;
            DateTime? actualValue = target.LastSynchronizedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn"), Ignore]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime CreatedOn

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            DateTime expectedValue = default;
            target.CreatedOn = default;
            DateTime actualValue = target.CreatedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime ModifiedOn"), Ignore]
        public void ModifiedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime ModifiedOn

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            DateTime expectedValue = default;
            target.ModifiedOn = default;
            DateTime actualValue = target.ModifiedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Item[string]"), Ignore]
        public void ItemColumnNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for string Item[string]

            string columnNameIndex = default;
            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            string expectedValue = default;
            string actualValue = target[columnNameIndex];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("IEnumerable<ValidationResult> Validate(ValidationContext)"), Ignore]
        public void ValidateValidationContextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for IEnumerable<ValidationResult> Validate(ValidationContext)

            ValidationContext validationContextArg = default;
            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()"), Ignore]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool HasErrors()

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()"), Ignore]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for void AcceptChanges()

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()"), Ignore]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool IsChanged()

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()"), Ignore]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for void RejectChanges()

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()"), Ignore]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Type GetType()

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()"), Ignore]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for string ToString()

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            string expectedReturnValue = default;
            string actualReturnValue = target.ToString();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool Equals(object)"), Ignore]
        public void EqualsObjectTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool Equals(object)

            object objArg = default;
            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()"), Ignore]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for int GetHashCode()

            FileSystem target = default; // TODO: Create and initialize FileSystem instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
