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
    public class SymbolicNameTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new SymbolicName()"), Ignore]
        public void NewSymbolicNameTestMethod()
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            SymbolicName target = new();

            EntityEntry<SymbolicName> entry = dbContext.Entry(target);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(0, target.Priority);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsFalse(target.IsInactive);
            Assert.AreEqual(Guid.Empty, target.FileSystemId);
            Assert.IsNull(target.FileSystem);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for new SymbolicName()

            dbContext.SymbolicNames.Add(target);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(0, target.Priority);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsFalse(target.IsInactive);
            Assert.AreEqual(Guid.Empty, target.FileSystemId);
            Assert.IsNull(target.FileSystem);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
        }

        [TestMethod("Guid Id"), Ignore]
        public void IdTestMethod()
        {
            SymbolicName target = new();
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
        [DataRow(null, "", DisplayName = "string Name = null")]
        [DataRow("", "", DisplayName = "string Name = \"\"")]
        [DataRow("\n\r", "", DisplayName = "string Name = \"\\n\\r\"")]
        [DataRow("Test", "Test", DisplayName = "string Name = \"Test\"")]
        [DataRow("\n Test \r", "Test", DisplayName = "string Name = \"\\n Test \\r\"")]
        public void NameTestMethod(string name, string expected)
        {
            SymbolicName target = new();
            target.Name = name;
            string actualValue = target.Name;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [DataTestMethod]
        [DataRow(null, "", DisplayName = "string Notes = null")]
        [DataRow("", "", DisplayName = "string Notes = \"\"")]
        [DataRow("\n\r", "", DisplayName = "string Notes = \"\\n\\r\"")]
        [DataRow("Test", "Test", DisplayName = "string Notes = \"Test\"")]
        [DataRow("\n Test \r", "\n Test \r", DisplayName = "string Notes = \"\\n Test \\r\"")]
        public void NotesTestMethod(string notes, string expected)
        {
            SymbolicName target = new();
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

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            bool expectedValue = default;
            target.IsInactive = default;
            bool actualValue = target.IsInactive;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("int Priority"), Ignore]
        public void PriorityTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for int Priority

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            int expectedValue = default;
            target.Priority = default;
            int actualValue = target.Priority;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid FileSystemId"), Ignore]
        public void FileSystemIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid FileSystemId

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            Guid expectedValue = default;
            target.FileSystemId = default;
            Guid actualValue = target.FileSystemId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("FileSystem FileSystem"), Ignore]
        public void FileSystemTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for FileSystem FileSystem

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            FileSystem expectedValue = default;
            target.FileSystem = default;
            FileSystem actualValue = target.FileSystem;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId"), Ignore]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? UpstreamId

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
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

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            DateTime? expectedValue = default;
            target.LastSynchronizedOn = default;
            DateTime? actualValue = target.LastSynchronizedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime CreatedOn

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
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

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
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
            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
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
            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()"), Ignore]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool HasErrors()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()"), Ignore]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for void AcceptChanges()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()"), Ignore]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool IsChanged()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()"), Ignore]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for void RejectChanges()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()"), Ignore]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Type GetType()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()"), Ignore]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for string ToString()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
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
            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()"), Ignore]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for int GetHashCode()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
