using FsInfoCat.Local.Model;
using FsInfoCat.Model;
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
    public class LocalFileAccessErrorTests
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

        [TestMethod("FileAccessError Constructor Tests"), Ignore]
        public void FileAccessErrorConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            FileAccessError target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(string.Empty, target.Details);
            Assert.AreEqual(ErrorCode.Unexpected, target.ErrorCode);
            Assert.AreEqual(string.Empty, target.Message);
            Assert.IsNull(target.Target);
            Assert.AreEqual(Guid.Empty, target.TargetId);
        }

        [TestMethod("Guid Id"), Ignore]
        public void IdTestMethod()
        {
            FileAccessError target = new();
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
        [DataRow(null, "", DisplayName = "string Message = null")]
        [DataRow("", "", DisplayName = "string Message = \"\"")]
        [DataRow("\n\r", "", DisplayName = "string Message = \"\\n\\r\"")]
        [DataRow("Test", "Test", DisplayName = "string Message = \"Test\"")]
        [DataRow("\n Test \r", "Test", DisplayName = "string Message = \"\\n Test \\r\"")]
        public void MessageTestMethod(string message, string expected)
        {
            FileAccessError target = new();
            target.Message = message;
            string actualValue = target.Message;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [DataTestMethod]
        [DataRow(null, "", DisplayName = "string Details = null")]
        [DataRow("", "", DisplayName = "string Details = \"\"")]
        [DataRow("\n\r", "", DisplayName = "string Details = \"\\n\\r\"")]
        [DataRow("Test", "Test", DisplayName = "string Details = \"Test\"")]
        [DataRow("\n Test \r", "\n Test \r", DisplayName = "string Details = \"\\n Test \\r\"")]
        public void DetailsTestMethod(string details, string expected)
        {
            FileAccessError target = new();
            target.Details = details;
            string actualValue = target.Details;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("ErrorCode ErrorCode"), Ignore]
        public void ErrorCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for ErrorCode ErrorCode

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            ErrorCode expectedValue = default;
            target.ErrorCode = default;
            ErrorCode actualValue = target.ErrorCode;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid TargetId"), Ignore]
        public void TargetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid TargetId

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            Guid expectedValue = default;
            target.TargetId = default;
            Guid actualValue = target.TargetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DbFile Target"), Ignore]
        public void TargetTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DbFile Target

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            DbFile expectedValue = default;
            target.Target = default;
            DbFile actualValue = target.Target;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn"), Ignore]
        [Description("BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime CreatedOn

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
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

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
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
            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
