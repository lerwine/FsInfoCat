using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalBinaryPropertiesTests
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static TestContext _testContext;
#pragma warning restore IDE0052 // Remove unread private members

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestInitialize]
        public void OnTestInitialize()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            TestHelper.UndoChanges(dbContext);
        }

        [TestMethod("BinaryPropertySet Constructor Tests")]
        [Ignore]
        public void BinaryPropertiesConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            BinaryPropertySet target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.Hash);
            Assert.AreEqual(0L, target.Length);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            Assert.IsNotNull(target.RedundantSets);
            Assert.AreEqual(0, target.RedundantSets.Count);
        }

        [TestMethod("Guid Id"), Ignore]
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

        [TestMethod("long Length"), Ignore]
        [Description("BinaryProperties.Length: BIGINT NOT NULL CHECK(Length>=0) UNIQUE")]
        public void LengthTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for long Length

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            long expectedValue = default;
            target.Length = default;
            long actualValue = target.Length;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("MD5Hash? Hash"), Ignore]
        [Description("BinaryProperties.Hash: BINARY(16) CHECK(Hash IS NULL OR length(HASH)=16) DEFAULT NULL")]
        public void HashTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for MD5Hash? Hash

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            MD5Hash? expectedValue = default;
            target.Hash = default;
            MD5Hash? actualValue = target.Hash;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<DbFile> Files"), Ignore]
        public void FilesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for HashSet<DbFile> Files

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            HashSet<DbFile> expectedValue = default;
            target.Files = default;
            HashSet<DbFile> actualValue = target.Files;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<RedundantSet> RedundantSets"), Ignore]
        public void RedundantSetsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for HashSet<RedundantSet> RedundantSets

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            HashSet<RedundantSet> expectedValue = default;
            target.RedundantSets = default;
            HashSet<RedundantSet> actualValue = target.RedundantSets;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId"), Ignore]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? UpstreamId

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
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

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            DateTime? expectedValue = default;
            target.LastSynchronizedOn = default;
            DateTime? actualValue = target.LastSynchronizedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn"), Ignore]
        [Description("BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime CreatedOn

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
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

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            DateTime expectedValue = default;
            target.ModifiedOn = default;
            DateTime actualValue = target.ModifiedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("IEnumerable<ValidationResult> Validate(ValidationContext)"), Ignore]
        public void ValidateValidationContextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for IEnumerable<ValidationResult> Validate(ValidationContext)

            ValidationContext validationContextArg = default;
            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
