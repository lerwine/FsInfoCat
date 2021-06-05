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

        [TestMethod("new SymbolicName()")]
        public void NewSymbolicNameTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
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
            // TODO: Implement test for new SymbolicName()

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

        [TestMethod("Guid Id")]
        public void IdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid Id

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            Guid expectedValue = default;
            target.Id = default;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Name")]
        public void NameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Name

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            string expectedValue = default;
            target.Name = default;
            string actualValue = target.Name;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Notes")]
        public void NotesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Notes

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            string expectedValue = default;
            target.Notes = default;
            string actualValue = target.Notes;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("bool IsInactive")]
        public void IsInactiveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsInactive

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            bool expectedValue = default;
            target.IsInactive = default;
            bool actualValue = target.IsInactive;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("int Priority")]
        public void PriorityTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int Priority

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            int expectedValue = default;
            target.Priority = default;
            int actualValue = target.Priority;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid FileSystemId")]
        public void FileSystemIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid FileSystemId

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            Guid expectedValue = default;
            target.FileSystemId = default;
            Guid actualValue = target.FileSystemId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("FileSystem FileSystem")]
        public void FileSystemTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for FileSystem FileSystem

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            FileSystem expectedValue = default;
            target.FileSystem = default;
            FileSystem actualValue = target.FileSystem;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            Guid? expectedValue = default;
            target.UpstreamId = default;
            Guid? actualValue = target.UpstreamId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? LastSynchronizedOn")]
        public void LastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime? LastSynchronizedOn

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            DateTime? expectedValue = default;
            target.LastSynchronizedOn = default;
            DateTime? actualValue = target.LastSynchronizedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime CreatedOn

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
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

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
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
            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
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
            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
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
            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            SymbolicName target = default; // TODO: Create and initialize SymbolicName instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
