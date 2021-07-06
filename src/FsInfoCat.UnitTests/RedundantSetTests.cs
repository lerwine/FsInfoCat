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
    public class RedundantSetTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new RedundantSet()")]
        public void NewRedundantSetTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            RedundantSet target = new();

            EntityEntry<RedundantSet> entry = dbContext.Entry(target);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.IsNotNull(target.Reference);
            Assert.AreEqual("", target.Reference);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertiesId);
            Assert.IsNull(target.BinaryProperties);
            Assert.IsNotNull(target.Redundancies);
            Assert.AreEqual(0, target.Redundancies.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new RedundantSet()

            dbContext.RedundantSets.Add(target);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.IsNotNull(target.Reference);
            Assert.AreEqual("", target.Reference);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertiesId);
            Assert.IsNull(target.BinaryProperties);
            Assert.IsNotNull(target.Redundancies);
            Assert.AreEqual(0, target.Redundancies.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
        }

        [TestMethod("Guid Id")]
        [Ignore]
        public void IdTestMethod()
        {
            RedundantSet target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("string Reference")]
        public void ReferenceTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Reference

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
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
            RedundantSet target = new();
            target.Notes = notes;
            string actualValue = target.Notes;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("Guid BinaryPropertiesId")]
        public void BinaryPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid BinaryPropertiesId

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            Guid expectedValue = default;
            target.BinaryPropertiesId = default;
            Guid actualValue = target.BinaryPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("BinaryProperties BinaryProperties")]
        public void BinaryPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for BinaryProperties BinaryProperties

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            BinaryPropertySet expectedValue = default;
            target.BinaryProperties = default;
            BinaryPropertySet actualValue = target.BinaryProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<Redundancy> Redundancies")]
        public void RedundanciesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<Redundancy> Redundancies

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            HashSet<Redundancy> expectedValue = default;
            target.Redundancies = default;
            HashSet<Redundancy> actualValue = target.Redundancies;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
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

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
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

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
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

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
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
            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
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
            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
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
            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            RedundantSet target = default; // TODO: Create and initialize RedundantSet instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
