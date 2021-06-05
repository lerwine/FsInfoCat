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

using System.IO;
namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class VolumeTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new Volume()")]
        public void NewVolumeTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            Volume target = new();
            EntityEntry<Volume> entry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNotNull(target.DisplayName);
            Assert.AreEqual("", target.DisplayName);
            Assert.IsNotNull(target.VolumeName);
            Assert.AreEqual("", target.VolumeName);
            Assert.IsTrue(target.Identifier.IsEmpty());
            Assert.AreEqual(VolumeStatus.Unknown, target.Status);
            Assert.AreEqual(DriveType.Unknown, target.Type);
            Assert.IsNull(target.CaseSensitiveSearch);
            Assert.IsNull(target.ReadOnly);
            Assert.IsNull(target.MaxNameLength);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.AreEqual(Guid.Empty, target.FileSystemId);
            Assert.IsNull(target.FileSystem);
            Assert.IsNull(target.RootDirectory);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new Volume()

            dbContext.Volumes.Add(target);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.IsNotNull(target.DisplayName);
            Assert.AreEqual("", target.DisplayName);
            Assert.IsNotNull(target.VolumeName);
            Assert.AreEqual("", target.VolumeName);
            Assert.IsTrue(target.Identifier.IsEmpty());
            Assert.AreEqual(VolumeStatus.Unknown, target.Status);
            Assert.AreEqual(DriveType.Unknown, target.Type);
            Assert.IsNull(target.CaseSensitiveSearch);
            Assert.IsNull(target.ReadOnly);
            Assert.IsNull(target.MaxNameLength);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.AreEqual(Guid.Empty, target.FileSystemId);
            Assert.IsNull(target.FileSystem);
            Assert.IsNull(target.RootDirectory);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
        }

        [TestMethod("Guid Id")]
        public void IdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid Id

            Volume target = default; // TODO: Create and initialize Volume instance
            Guid expectedValue = default;
            target.Id = default;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string DisplayName")]
        public void DisplayNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string DisplayName

            Volume target = default; // TODO: Create and initialize Volume instance
            string expectedValue = default;
            target.DisplayName = default;
            string actualValue = target.DisplayName;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string VolumeName")]
        public void VolumeNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string VolumeName

            Volume target = default; // TODO: Create and initialize Volume instance
            string expectedValue = default;
            target.VolumeName = default;
            string actualValue = target.VolumeName;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("VolumeIdentifier Identifier")]
        public void IdentifierTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for VolumeIdentifier Identifier

            Volume target = default; // TODO: Create and initialize Volume instance
            VolumeIdentifier expectedValue = default;
            target.Identifier = default;
            VolumeIdentifier actualValue = target.Identifier;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("VolumeStatus Status")]
        public void StatusTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for VolumeStatus Status

            Volume target = default; // TODO: Create and initialize Volume instance
            VolumeStatus expectedValue = default;
            target.Status = default;
            VolumeStatus actualValue = target.Status;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DriveType Type")]
        public void TypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DriveType Type

            Volume target = default; // TODO: Create and initialize Volume instance
            DriveType expectedValue = default;
            target.Type = default;
            DriveType actualValue = target.Type;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("bool? CaseSensitiveSearch")]
        public void CaseSensitiveSearchTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool? CaseSensitiveSearch

            Volume target = default; // TODO: Create and initialize Volume instance
            bool? expectedValue = default;
            target.CaseSensitiveSearch = default;
            bool? actualValue = target.CaseSensitiveSearch;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("bool? ReadOnly")]
        public void ReadOnlyTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool? ReadOnly

            Volume target = default; // TODO: Create and initialize Volume instance
            bool? expectedValue = default;
            target.ReadOnly = default;
            bool? actualValue = target.ReadOnly;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("int? MaxNameLength")]
        public void MaxNameLengthTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int? MaxNameLength

            Volume target = default; // TODO: Create and initialize Volume instance
            int? expectedValue = default;
            target.MaxNameLength = default;
            int? actualValue = target.MaxNameLength;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Notes")]
        public void NotesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Notes

            Volume target = default; // TODO: Create and initialize Volume instance
            string expectedValue = default;
            target.Notes = default;
            string actualValue = target.Notes;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid FileSystemId")]
        public void FileSystemIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid FileSystemId

            Volume target = default; // TODO: Create and initialize Volume instance
            Guid expectedValue = default;
            target.FileSystemId = default;
            Guid actualValue = target.FileSystemId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("FileSystem FileSystem")]
        public void FileSystemTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for FileSystem FileSystem

            Volume target = default; // TODO: Create and initialize Volume instance
            FileSystem expectedValue = default;
            target.FileSystem = default;
            FileSystem actualValue = target.FileSystem;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Subdirectory RootDirectory")]
        public void RootDirectoryTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Subdirectory RootDirectory

            Volume target = default; // TODO: Create and initialize Volume instance
            Subdirectory expectedValue = default;
            target.RootDirectory = default;
            Subdirectory actualValue = target.RootDirectory;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<VolumeAccessError> AccessErrors")]
        public void AccessErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<VolumeAccessError> AccessErrors

            Volume target = default; // TODO: Create and initialize Volume instance
            HashSet<VolumeAccessError> expectedValue = default;
            target.AccessErrors = default;
            HashSet<VolumeAccessError> actualValue = target.AccessErrors;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            Volume target = default; // TODO: Create and initialize Volume instance
            Guid? expectedValue = default;
            target.UpstreamId = default;
            Guid? actualValue = target.UpstreamId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? LastSynchronizedOn")]
        public void LastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime? LastSynchronizedOn

            Volume target = default; // TODO: Create and initialize Volume instance
            DateTime? expectedValue = default;
            target.LastSynchronizedOn = default;
            DateTime? actualValue = target.LastSynchronizedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime CreatedOn

            Volume target = default; // TODO: Create and initialize Volume instance
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

            Volume target = default; // TODO: Create and initialize Volume instance
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
            Volume target = default; // TODO: Create and initialize Volume instance
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
            Volume target = default; // TODO: Create and initialize Volume instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            Volume target = default; // TODO: Create and initialize Volume instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            Volume target = default; // TODO: Create and initialize Volume instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            Volume target = default; // TODO: Create and initialize Volume instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            Volume target = default; // TODO: Create and initialize Volume instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            Volume target = default; // TODO: Create and initialize Volume instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            Volume target = default; // TODO: Create and initialize Volume instance
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
            Volume target = default; // TODO: Create and initialize Volume instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            Volume target = default; // TODO: Create and initialize Volume instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
