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
    public class FileComparisonTests
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static TestContext _testContext;
#pragma warning restore IDE0052 // Remove unread private members

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new FileComparison()"), Ignore]
        public void NewFileComparisonTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            FileComparison target = new();

            EntityEntry<FileComparison> entry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.AreEqual(Guid.Empty, target.BaselineId);
            Assert.AreEqual(Guid.Empty, target.CorrelativeId);
            Assert.IsFalse(target.AreEqual);
            Assert.IsNull(target.Baseline);
            Assert.IsNull(target.Correlative);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.ComparedOn);

            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for new FileComparison()

            dbContext.Comparisons.Add(target);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.AreEqual(Guid.Empty, target.BaselineId);
            Assert.AreEqual(Guid.Empty, target.CorrelativeId);
            Assert.IsFalse(target.AreEqual);
            Assert.IsNull(target.Baseline);
            Assert.IsNull(target.Correlative);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.ComparedOn);
        }

        [TestMethod("Guid BaselineId"), Ignore]
        public void BaselineIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid BaselineId

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            Guid expectedValue = default;
            target.BaselineId = default;
            Guid actualValue = target.BaselineId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid CorrelativeId"), Ignore]
        public void CorrelativeIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid CorrelativeId

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            Guid expectedValue = default;
            target.CorrelativeId = default;
            Guid actualValue = target.CorrelativeId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("bool AreEqual"), Ignore]
        public void AreEqualTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool AreEqual

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            bool expectedValue = default;
            target.AreEqual = default;
            bool actualValue = target.AreEqual;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime ComparedOn"), Ignore]
        public void ComparedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime ComparedOn

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            DateTime expectedValue = default;
            target.ComparedOn = default;
            DateTime actualValue = target.ComparedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DbFile Baseline"), Ignore]
        public void BaselineTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DbFile Baseline

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            DbFile expectedValue = default;
            target.Baseline = default;
            DbFile actualValue = target.Baseline;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DbFile Correlative"), Ignore]
        public void CorrelativeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DbFile Correlative

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            DbFile expectedValue = default;
            target.Correlative = default;
            DbFile actualValue = target.Correlative;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId"), Ignore]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? UpstreamId

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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

            FileComparison target = default; // TODO: Create and initialize FileComparison instance
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
            FileComparison target = default; // TODO: Create and initialize FileComparison instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
