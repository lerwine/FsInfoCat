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
    public class DbFileTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new DbFile()")]
        public void NewDbFileTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            DbFile target = new();

            EntityEntry<DbFile> entry = dbContext.Entry(target);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.IsNull(target.LastHashCalculation);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsFalse(target.Deleted);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            Assert.AreEqual(Guid.Empty, target.ContentId);
            Assert.IsNull(target.ExtendedPropertiesId);
            Assert.IsNull(target.Content);
            Assert.IsNull(target.Parent);
            Assert.IsNull(target.Redundancy);
            Assert.IsNull(target.ExtendedProperties);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNotNull(target.ComparisonSources);
            Assert.AreEqual(0, target.ComparisonSources.Count);
            Assert.IsNotNull(target.ComparisonTargets);
            Assert.AreEqual(0, target.ComparisonTargets.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new DbFile()

            dbContext.Files.Add(target);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.IsNull(target.LastHashCalculation);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsFalse(target.Deleted);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            Assert.AreEqual(Guid.Empty, target.ContentId);
            Assert.IsNull(target.ExtendedPropertiesId);
            Assert.IsNull(target.Content);
            Assert.IsNull(target.Parent);
            Assert.IsNull(target.Redundancy);
            Assert.IsNull(target.ExtendedProperties);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNotNull(target.ComparisonSources);
            Assert.AreEqual(0, target.ComparisonSources.Count);
            Assert.IsNotNull(target.ComparisonTargets);
            Assert.AreEqual(0, target.ComparisonTargets.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);
        }

        [TestMethod("Guid Id")]
        public void IdTestMethod()
        {
            DbFile target = new();
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

            DbFile target = default; // TODO: Create and initialize DbFile instance
            string expectedValue = default;
            target.Name = default;
            string actualValue = target.Name;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("FileCrawlOptions Options")]
        public void OptionsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for FileCrawlOptions Options

            DbFile target = default; // TODO: Create and initialize DbFile instance
            FileCrawlOptions expectedValue = default;
            target.Options = default;
            FileCrawlOptions actualValue = target.Options;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime LastAccessed")]
        public void LastAccessedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime LastAccessed

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime expectedValue = default;
            target.LastAccessed = default;
            DateTime actualValue = target.LastAccessed;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? LastHashCalculation")]
        public void LastHashCalculationTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime? LastHashCalculation

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime? expectedValue = default;
            target.LastHashCalculation = default;
            DateTime? actualValue = target.LastHashCalculation;
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
            DbFile target = new();
            target.Notes = notes;
            string actualValue = target.Notes;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("bool Deleted")]
        public void DeletedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool Deleted

            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedValue = default;
            target.Deleted = default;
            bool actualValue = target.Deleted;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid ParentId")]
        public void ParentIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid ParentId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid expectedValue = default;
            target.ParentId = default;
            Guid actualValue = target.ParentId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid ContentId")]
        public void ContentIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid ContentId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid expectedValue = default;
            target.ContentId = default;
            Guid actualValue = target.ContentId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? ExtendedPropertiesId")]
        public void ExtendedPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? ExtendedPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.ExtendedPropertiesId = default;
            Guid? actualValue = target.ExtendedPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ContentInfo Content")]
        public void ContentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ContentInfo Content

            DbFile target = default; // TODO: Create and initialize DbFile instance
            ContentInfo expectedValue = default;
            target.Content = default;
            ContentInfo actualValue = target.Content;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Subdirectory Parent")]
        public void ParentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Subdirectory Parent

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Subdirectory expectedValue = default;
            target.Parent = default;
            Subdirectory actualValue = target.Parent;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Redundancy Redundancy")]
        public void RedundancyTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Redundancy Redundancy

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Redundancy expectedValue = default;
            target.Redundancy = default;
            Redundancy actualValue = target.Redundancy;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ExtendedProperties ExtendedProperties")]
        public void ExtendedPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ExtendedProperties ExtendedProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            ExtendedProperties expectedValue = default;
            target.ExtendedProperties = default;
            ExtendedProperties actualValue = target.ExtendedProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<FileAccessError> AccessErrors")]
        public void AccessErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<FileAccessError> AccessErrors

            DbFile target = default; // TODO: Create and initialize DbFile instance
            HashSet<FileAccessError> expectedValue = default;
            target.AccessErrors = default;
            HashSet<FileAccessError> actualValue = target.AccessErrors;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<FileComparison> ComparisonSources")]
        public void ComparisonSourcesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<FileComparison> ComparisonSources

            DbFile target = default; // TODO: Create and initialize DbFile instance
            HashSet<FileComparison> expectedValue = default;
            target.ComparisonSources = default;
            HashSet<FileComparison> actualValue = target.ComparisonSources;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<FileComparison> ComparisonTargets")]
        public void ComparisonTargetsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<FileComparison> ComparisonTargets

            DbFile target = default; // TODO: Create and initialize DbFile instance
            HashSet<FileComparison> expectedValue = default;
            target.ComparisonTargets = default;
            HashSet<FileComparison> actualValue = target.ComparisonTargets;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            DbFile target = default; // TODO: Create and initialize DbFile instance
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

            DbFile target = default; // TODO: Create and initialize DbFile instance
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

            DbFile target = default; // TODO: Create and initialize DbFile instance
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

            DbFile target = default; // TODO: Create and initialize DbFile instance
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
            DbFile target = default; // TODO: Create and initialize DbFile instance
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
            DbFile target = default; // TODO: Create and initialize DbFile instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            DbFile target = default; // TODO: Create and initialize DbFile instance
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
            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
