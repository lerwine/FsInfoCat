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
    public class RedundancyTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new Redundancy()")]
        public void NewRedundancyTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            Redundancy target = new();

            EntityEntry<Redundancy> entry = dbContext.Entry(target);
            Assert.AreEqual(Guid.Empty, target.FileId);
            Assert.AreEqual(Guid.Empty, target.RedundantSetId);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.IsNotNull(target.Reference);
            Assert.AreEqual("", target.Reference);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsNull(target.File);
            Assert.IsNull(target.RedundantSet);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new Redundancy()

            dbContext.Redundancies.Add(target);
            Assert.AreEqual(Guid.Empty, target.FileId);
            Assert.AreEqual(Guid.Empty, target.RedundantSetId);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.IsNotNull(target.Reference);
            Assert.AreEqual("", target.Reference);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsNull(target.File);
            Assert.IsNull(target.RedundantSet);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
        }

        [TestMethod("Guid FileId")]
        public void FileIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid FileId

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            Guid expectedValue = default;
            target.FileId = default;
            Guid actualValue = target.FileId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid RedundantSetId")]
        public void RedundantSetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid RedundantSetId

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            Guid expectedValue = default;
            target.RedundantSetId = default;
            Guid actualValue = target.RedundantSetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Reference")]
        public void ReferenceTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Reference

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            string expectedValue = default;
            target.Reference = default;
            string actualValue = target.Reference;
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
            Redundancy target = new();
            target.Notes = notes;
            string actualValue = target.Notes;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("DbFile File")]
        public void FileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DbFile File

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            DbFile expectedValue = default;
            target.File = default;
            DbFile actualValue = target.File;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("RedundantSet RedundantSet")]
        public void RedundantSetTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for RedundantSet RedundantSet

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            RedundantSet expectedValue = default;
            target.RedundantSet = default;
            RedundantSet actualValue = target.RedundantSet;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
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

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            DateTime? expectedValue = default;
            target.LastSynchronizedOn = default;
            DateTime? actualValue = target.LastSynchronizedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn")]
        [TestProperty(TestHelper.TestProperty_Description, "ContentInfo.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime CreatedOn

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
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

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
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
            Redundancy target = default; // TODO: Create and initialize Redundancy instance
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
            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
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
            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
