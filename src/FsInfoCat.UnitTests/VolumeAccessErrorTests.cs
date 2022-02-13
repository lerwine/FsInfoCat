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
    public class VolumeAccessErrorTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new VolumeAccessError()"), Ignore]
        public void NewVolumeAccessErrorTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            VolumeAccessError target = new();

            EntityEntry<VolumeAccessError> entry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNotNull(target.Message);
            Assert.AreEqual("", target.Message);
            Assert.IsNotNull(target.Details);
            Assert.AreEqual("", target.Details);
            Assert.AreEqual(ErrorCode.Unexpected, target.ErrorCode);
            Assert.AreEqual(Guid.Empty, target.TargetId);
            Assert.IsNull(target.Target);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for new VolumeAccessError()

            dbContext.VolumeAccessErrors.Add(target);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.IsNotNull(target.Message);
            Assert.AreEqual("", target.Message);
            Assert.IsNotNull(target.Details);
            Assert.AreEqual("", target.Details);
            Assert.AreEqual(ErrorCode.Unexpected, target.ErrorCode);
            Assert.AreEqual(Guid.Empty, target.TargetId);
            Assert.IsNull(target.Target);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
        }

        [TestMethod("Guid Id"), Ignore]
        public void IdTestMethod()
        {
            VolumeAccessError target = new();
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
            VolumeAccessError target = new();
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
            VolumeAccessError target = new();
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

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
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

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            Guid expectedValue = default;
            target.TargetId = default;
            Guid actualValue = target.TargetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Volume Target"), Ignore]
        public void TargetTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Volume Target

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            Volume expectedValue = default;
            target.Target = default;
            Volume actualValue = target.Target;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn"), Ignore]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime CreatedOn

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
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

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
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
            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
