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
    public class RedundancyTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new Redundancy()"), Ignore]
        public void NewRedundancyTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
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
            // DEFERRED: Implement test for new Redundancy()

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

        [TestMethod("Guid FileId"), Ignore]
        public void FileIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid FileId

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            Guid expectedValue = default;
            target.FileId = default;
            Guid actualValue = target.FileId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid RedundantSetId"), Ignore]
        public void RedundantSetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid RedundantSetId

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            Guid expectedValue = default;
            target.RedundantSetId = default;
            Guid actualValue = target.RedundantSetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Reference"), Ignore]
        public void ReferenceTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for string Reference

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

        [TestMethod("DbFile File"), Ignore]
        public void FileTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DbFile File

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            DbFile expectedValue = default;
            target.File = default;
            DbFile actualValue = target.File;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("RedundantSet RedundantSet"), Ignore]
        public void RedundantSetTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for RedundantSet RedundantSet

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            RedundantSet expectedValue = default;
            target.RedundantSet = default;
            RedundantSet actualValue = target.RedundantSet;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId"), Ignore]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? UpstreamId

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
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

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
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

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
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

            Redundancy target = default; // TODO: Create and initialize Redundancy instance
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
            Redundancy target = default; // TODO: Create and initialize Redundancy instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
