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
    public class FileAccessErrorTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new FileAccessError()")]
        public void NewFileAccessErrorTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            FileAccessError target = new();

            EntityEntry<FileAccessError> entry = dbContext.Entry(target);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.IsNotNull(target.Message);
            Assert.AreEqual("", target.Message);
            Assert.IsNotNull(target.Details);
            Assert.AreEqual("", target.Details);
            Assert.AreEqual(AccessErrorCode.OpenError, target.ErrorCode);
            Assert.AreEqual(Guid.Empty, target.TargetId);
            Assert.IsNull(target.Target);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new FileAccessError()

            dbContext.FileAccessErrors.Add(target);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Added, entry.State);
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
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid Id

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            Guid expectedValue = default;
            target.Id = default;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Message")]
        public void MessageTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Message

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            string expectedValue = default;
            target.Message = default;
            string actualValue = target.Message;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Details")]
        public void DetailsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Details

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            string expectedValue = default;
            target.Details = default;
            string actualValue = target.Details;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("AccessErrorCode ErrorCode")]
        public void ErrorCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for AccessErrorCode ErrorCode

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
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

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            Guid expectedValue = default;
            target.TargetId = default;
            Guid actualValue = target.TargetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DbFile Target")]
        public void TargetTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DbFile Target

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            DbFile expectedValue = default;
            target.Target = default;
            DbFile actualValue = target.Target;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime CreatedOn

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
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

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
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
            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
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
            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
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
            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            FileAccessError target = default; // TODO: Create and initialize FileAccessError instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
