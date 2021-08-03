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
    public class BinaryPropertiesTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new BinaryProperties()")]
        public void NewBinaryPropertiesTestMethod()
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            BinaryPropertySet target = new();

            EntityEntry<BinaryPropertySet> entry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(0L, target.Length);
            Assert.IsNull(target.Hash);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            Assert.IsNotNull(target.RedundantSets);
            Assert.AreEqual(0, target.RedundantSets.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new BinaryProperties()

            dbContext.BinaryPropertySets.Add(target);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.AreEqual(0L, target.Length);
            Assert.IsNull(target.Hash);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            Assert.IsNotNull(target.RedundantSets);
            Assert.AreEqual(0, target.RedundantSets.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
        }

        [TestMethod("Guid Id")]
        [Ignore]
        public void IdTestMethod()
        {
            BinaryPropertySet target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("long Length")]
        [TestProperty(TestHelper.TestProperty_Description, "BinaryProperties.Length: BIGINT NOT NULL CHECK(Length>=0) UNIQUE")]
        public void LengthTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for long Length

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            long expectedValue = default;
            target.Length = default;
            long actualValue = target.Length;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("MD5Hash? Hash")]
        [TestProperty(TestHelper.TestProperty_Description, "BinaryProperties.Hash: BINARY(16) CHECK(Hash IS NULL OR length(HASH)=16) DEFAULT NULL")]
        public void HashTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for MD5Hash? Hash

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            MD5Hash? expectedValue = default;
            target.Hash = default;
            MD5Hash? actualValue = target.Hash;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<DbFile> Files")]
        public void FilesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<DbFile> Files

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            HashSet<DbFile> expectedValue = default;
            target.Files = default;
            HashSet<DbFile> actualValue = target.Files;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<RedundantSet> RedundantSets")]
        public void RedundantSetsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<RedundantSet> RedundantSets

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            HashSet<RedundantSet> expectedValue = default;
            target.RedundantSets = default;
            HashSet<RedundantSet> actualValue = target.RedundantSets;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
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

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
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

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
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

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
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
            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
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
            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
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
            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
