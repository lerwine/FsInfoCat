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
    public class VolumeAccessErrorTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new VolumeAccessError()")]
        public void NewVolumeAccessErrorTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            VolumeAccessError target = new();

            EntityEntry<VolumeAccessError> entry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNotNull(target.Message);
            Assert.AreEqual("", target.Message);
            Assert.IsNotNull(target.Details);
            Assert.AreEqual("", target.Details);
            Assert.AreEqual(AccessErrorCode.OpenError, target.ErrorCode);
            Assert.AreEqual(Guid.Empty, target.TargetId);
            Assert.IsNull(target.Target);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new VolumeAccessError()

            dbContext.VolumeAccessErrors.Add(target);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.IsNotNull(target.Message);
            Assert.AreEqual("", target.Message);
            Assert.IsNotNull(target.Details);
            Assert.AreEqual("", target.Details);
            Assert.AreEqual(AccessErrorCode.OpenError, target.ErrorCode);
            Assert.AreEqual(Guid.Empty, target.TargetId);
            Assert.IsNull(target.Target);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
        }

        [TestMethod("Guid Id")]
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

        [TestMethod("AccessErrorCode ErrorCode")]
        public void ErrorCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for AccessErrorCode ErrorCode

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            AccessErrorCode expectedValue = default;
            target.ErrorCode = default;
            AccessErrorCode actualValue = target.ErrorCode;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid TargetId")]
        public void TargetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid TargetId

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            Guid expectedValue = default;
            target.TargetId = default;
            Guid actualValue = target.TargetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Volume Target")]
        public void TargetTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Volume Target

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            Volume expectedValue = default;
            target.Target = default;
            Volume actualValue = target.Target;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn")]
        [TestProperty(TestHelper.TestProperty_Description, "ContentInfo.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime CreatedOn

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
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

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
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
            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
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
            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
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
            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            VolumeAccessError target = default; // TODO: Create and initialize VolumeAccessError instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
