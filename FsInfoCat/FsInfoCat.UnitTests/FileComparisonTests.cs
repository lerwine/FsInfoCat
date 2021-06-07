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
    public class FileComparisonTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new FileComparison()")]
        public void NewFileComparisonTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            FileComparison target = new();

            EntityEntry<FileComparison> entry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.AreEqual(Guid.Empty, target.SourceFileId);
            Assert.AreEqual(Guid.Empty, target.TargetFileId);
            Assert.IsFalse(target.AreEqual);
            Assert.IsNull(target.SourceFile);
            Assert.IsNull(target.TargetFile);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.ComparedOn);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new FileComparison()

            dbContext.Comparisons.Add(target);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.AreEqual(Guid.Empty, target.SourceFileId);
            Assert.AreEqual(Guid.Empty, target.TargetFileId);
            Assert.IsFalse(target.AreEqual);
            Assert.IsNull(target.SourceFile);
            Assert.IsNull(target.TargetFile);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.ComparedOn);
        }

        [TestMethod("Guid SourceFileId")]
        public void SourceFileIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid SourceFileId

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            Guid expectedValue = default;
            target.SourceFileId = default;
            Guid actualValue = target.SourceFileId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid TargetFileId")]
        public void TargetFileIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid TargetFileId

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            Guid expectedValue = default;
            target.TargetFileId = default;
            Guid actualValue = target.TargetFileId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("bool AreEqual")]
        public void AreEqualTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool AreEqual

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            bool expectedValue = default;
            target.AreEqual = default;
            bool actualValue = target.AreEqual;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime ComparedOn")]
        public void ComparedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime ComparedOn

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            DateTime expectedValue = default;
            target.ComparedOn = default;
            DateTime actualValue = target.ComparedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DbFile SourceFile")]
        public void SourceFileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DbFile SourceFile

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            DbFile expectedValue = default;
            target.SourceFile = default;
            DbFile actualValue = target.SourceFile;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DbFile TargetFile")]
        public void TargetFileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DbFile TargetFile

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            DbFile expectedValue = default;
            target.TargetFile = default;
            DbFile actualValue = target.TargetFile;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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
            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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
            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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
            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
