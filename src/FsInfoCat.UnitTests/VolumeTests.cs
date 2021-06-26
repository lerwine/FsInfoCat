using FsInfoCat;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using System.IO;
using System.Linq;
using System.Xml.Linq;

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
        [Ignore]
        public void IdTestMethod()
        {
            Volume target = new();
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
        [DynamicData(nameof(GetDisplayNameTestData), DynamicDataSourceType.Method)]
        [TestProperty(TestHelper.TestProperty_Description, "Volume.DisplayName: NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName))>0) COLLATE NOCASE")]
        public void DisplayNameTestMethod(string displayName, string expected, string errorMessage)
        {
            Volume target = new() { FileSystem = new(), Identifier = new VolumeIdentifier(Guid.NewGuid()) };
            target.DisplayName = displayName;
            string actualValue = target.DisplayName;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            if (string.IsNullOrWhiteSpace(errorMessage))
                Assert.IsTrue(isValid);
            else
            {
                Assert.IsFalse(isValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.AreEqual(validationResults[0].ErrorMessage, errorMessage);
                string actualMemberName = validationResults[0].MemberNames.FirstOrDefault();
                Assert.IsNotNull(actualMemberName);
                Assert.IsFalse(validationResults[0].MemberNames.Skip(1).Any());
                Assert.AreEqual(nameof(Volume.DisplayName), actualMemberName);
            }
        }

        public static IEnumerable<object[]> GetDisplayNameTestData() =>
            XDocument.Parse(Properties.Resources.DisplayName1024).Root.Elements("Row").Select(row =>
            {
                XElement a = row.Element("Argument");
                XElement e = row.Element("Expected");
                XAttribute m = row.Attribute("ErrorMessage");
                return new object[] { a.IsEmpty ? null : a.Value, e.Value, (m is null) ? "" : m.Value };
            });

        public static IEnumerable<object[]> GetVolumeNameTestData() =>
            XDocument.Parse(Properties.Resources.VolumeName128).Root.Elements("Row").Select(row =>
            {
                XElement a = row.Element("Argument");
                XElement e = row.Element("Expected");
                XAttribute m = row.Attribute("ErrorMessage");
                return new object[] { a.IsEmpty ? null : a.Value, e.Value, (m is null) ? "" : m.Value };
            });

        public static IEnumerable<object[]> GetIdentifierTestData() =>
            XDocument.Parse(Properties.Resources.VolumeIdentifier1024).Root.Elements("Row").Select(row =>
            {
                XElement a = row.Element("Argument");
                XElement e = row.Element("Expected");
                XAttribute m = row.Attribute("ErrorMessage");
                return new object[] { a.IsEmpty ? null : a.Value, e.Value, (m is null) ? "" : m.Value };
            });

        [DataTestMethod]
        [DynamicData(nameof(GetVolumeNameTestData), DynamicDataSourceType.Method)]
        [TestProperty(TestHelper.TestProperty_Description, "Volume.VolumeName: NVARCHAR(128) NOT NULL CHECK(length(trim(VolumeName))>0) COLLATE NOCASE")]
        public void VolumeNameTestMethod(string volumeName, string expected, string errorMessage)
        {
            Volume target = new() { DisplayName = "Test", FileSystem = new(), Identifier = new VolumeIdentifier(Guid.NewGuid()) };
            target.VolumeName = volumeName;
            string actualValue = target.VolumeName;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            if (string.IsNullOrWhiteSpace(errorMessage))
                Assert.IsTrue(isValid);
            else
            {
                Assert.IsFalse(isValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.AreEqual(validationResults[0].ErrorMessage, errorMessage);
                string actualMemberName = validationResults[0].MemberNames.FirstOrDefault();
                Assert.IsNotNull(actualMemberName);
                Assert.IsFalse(validationResults[0].MemberNames.Skip(1).Any());
                Assert.AreEqual(nameof(Volume.VolumeName), actualMemberName);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetIdentifierTestData), DynamicDataSourceType.Method)]
        [TestProperty(TestHelper.TestProperty_Description, "Volume.Identifier: NVARCHAR(1024) NOT NULL CHECK(length(trim(Identifier))>0) UNIQUE COLLATE NOCASE")]
        public void IdentifierTestMethod(string identifier, string expected, string errorMessage)
        {
            Volume target = new() { DisplayName = "Test", FileSystem = new() };
            VolumeIdentifier expectedValue = string.IsNullOrWhiteSpace(expected) ? default : VolumeIdentifier.Parse(expected);
            target.Identifier = string.IsNullOrWhiteSpace(identifier) ? default : VolumeIdentifier.Parse(identifier);
            VolumeIdentifier actualValue = target.Identifier;
            Assert.AreEqual(expectedValue, actualValue);
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            if (string.IsNullOrWhiteSpace(errorMessage))
                Assert.IsTrue(isValid);
            else
            {
                Assert.IsFalse(isValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.AreEqual(validationResults[0].ErrorMessage, errorMessage);
                string actualMemberName = validationResults[0].MemberNames.FirstOrDefault();
                Assert.IsNotNull(actualMemberName);
                Assert.IsFalse(validationResults[0].MemberNames.Skip(1).Any());
                Assert.AreEqual(nameof(Volume.Identifier), actualMemberName);
            }
        }

        [DataTestMethod]
        [DataRow(VolumeStatus.Unknown, null, DisplayName = "VolumeStatus Status = Unknown")]
        [DataRow(VolumeStatus.Controlled, null, DisplayName = "VolumeStatus Status = Controlled")]
        [DataRow(VolumeStatus.Uncontrolled, null, DisplayName = "VolumeStatus Status = Uncontrolled")]
        [DataRow(VolumeStatus.Offline, null, DisplayName = "VolumeStatus Status = Offline")]
        [DataRow(VolumeStatus.Relinquished, null, DisplayName = "VolumeStatus Status = Relinquished")]
        [DataRow(VolumeStatus.Destroyed, null, DisplayName = "VolumeStatus Status = Destroyed")]
        [DataRow(7, "Volume Status is invalid.", DisplayName = "VolumeStatus Status = 6")]
        [TestProperty(TestHelper.TestProperty_Description, "Volume.Type: TINYINT NOT NULL CHECK(Status>=0 AND Status<6)")]
        public void StatusTestMethod(object sourceValue, string errorMessage)
        {
            VolumeStatus value = (VolumeStatus)Enum.ToObject(typeof(VolumeStatus), sourceValue);
            Volume target = new() { DisplayName = "Test", FileSystem = new(), Identifier = new VolumeIdentifier(Guid.NewGuid()) };
            target.Status = value;
            VolumeStatus actualValue = target.Status;
            Assert.AreEqual(value, actualValue);
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            if (string.IsNullOrWhiteSpace(errorMessage))
                Assert.IsTrue(isValid);
            else
            {
                Assert.IsFalse(isValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.AreEqual(validationResults[0].ErrorMessage, errorMessage);
                string actualMemberName = validationResults[0].MemberNames.FirstOrDefault();
                Assert.IsNotNull(actualMemberName);
                Assert.IsFalse(validationResults[0].MemberNames.Skip(1).Any());
                Assert.AreEqual(nameof(Volume.Status), actualMemberName);
            }
        }

        [DataTestMethod]
        [DataRow(DriveType.Unknown, null, DisplayName = "DriveType Type = Unknown")]
        [DataRow(DriveType.NoRootDirectory, null, DisplayName = "DriveType Type = NoRootDirectory")]
        [DataRow(DriveType.Removable, null, DisplayName = "DriveType Type = Removable")]
        [DataRow(DriveType.Fixed, null, DisplayName = "DriveType Type = Fixed")]
        [DataRow(DriveType.Network, null, DisplayName = "DriveType Type = Network")]
        [DataRow(DriveType.CDRom, null, DisplayName = "DriveType Type = CDRom")]
        [DataRow(DriveType.Ram, null, DisplayName = "DriveType Type = Ram")]
        [DataRow(7, "Drive Type is invalid.", DisplayName = "DriveType Type = 7")]
        [TestProperty(TestHelper.TestProperty_Description, "Volume.Type: TINYINT NOT NULL CHECK(Type>=0 AND Type<7)")]
        public void TypeTestMethod(DriveType value, string errorMessage)
        {
            Volume target = new() { DisplayName = "Test", FileSystem = new(), Identifier = new VolumeIdentifier(Guid.NewGuid()) };
            target.Type = value;
            DriveType actualValue = target.Type;
            Assert.AreEqual(value, actualValue);
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            if (string.IsNullOrWhiteSpace(errorMessage))
                Assert.IsTrue(isValid);
            else
            {
                Assert.IsFalse(isValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.AreEqual(validationResults[0].ErrorMessage, errorMessage);
                string actualMemberName = validationResults[0].MemberNames.FirstOrDefault();
                Assert.IsNotNull(actualMemberName);
                Assert.IsFalse(validationResults[0].MemberNames.Skip(1).Any());
                Assert.AreEqual(nameof(Volume.Type), actualMemberName);
            }
        }

        [DataTestMethod]
        [DataRow(null, DisplayName = "bool? CaseSensitiveSearch = null")]
        [DataRow(true, DisplayName = "bool? CaseSensitiveSearch = true")]
        [DataRow(false, DisplayName = "bool? CaseSensitiveSearch = false")]
        public void CaseSensitiveSearchTestMethod(bool? value)
        {
            Volume target = new() { DisplayName = "Test", FileSystem = new(), Identifier = new VolumeIdentifier(Guid.NewGuid()) };
            target.CaseSensitiveSearch = value;
            bool? actualValue = target.CaseSensitiveSearch;
            Assert.AreEqual(value, actualValue);
        }

        [DataTestMethod]
        [DataRow(null, DisplayName = "bool? ReadOnly = null")]
        [DataRow(true, DisplayName = "bool? ReadOnly = true")]
        [DataRow(false, DisplayName = "bool? ReadOnly = false")]
        public void ReadOnlyTestMethod(bool? value)
        {
            Volume target = new() { DisplayName = "Test", FileSystem = new(), Identifier = new VolumeIdentifier(Guid.NewGuid()) };
            target.ReadOnly = value;
            bool? actualValue = target.ReadOnly;
            Assert.AreEqual(value, actualValue);
        }

        [DataTestMethod]
        [DataRow(null, null, DisplayName = "uint? MaxNameLength = null")]
        [DataRow(1u, null, DisplayName = "uint? MaxNameLength = 1")]
        [DataRow(int.MaxValue, null, DisplayName = "int? MaxNameLength = int.MaxValue")]
        [DataRow(0u, "Maximum Name Length must be greater than zero.", DisplayName = "uint? MaxNameLength = 0")]
        [TestMethod("uint? MaxNameLength")]
        [TestProperty(TestHelper.TestProperty_Description, "Volume.MaxNameLength: CHECK(MaxNameLength IS NULL OR MaxNameLength>=1)")]
        public void MaxNameLengthTestMethod(uint? value, string errorMessage)
        {
            Volume target = new() { DisplayName = "Test", FileSystem = new(), Identifier = new VolumeIdentifier(Guid.NewGuid()) };
            target.MaxNameLength = value;
            uint? actualValue = target.MaxNameLength;
            Assert.AreEqual(value, actualValue);
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            if (string.IsNullOrWhiteSpace(errorMessage))
                Assert.IsTrue(isValid);
            else
            {
                Assert.IsFalse(isValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.AreEqual(validationResults[0].ErrorMessage, errorMessage);
                string actualMemberName = validationResults[0].MemberNames.FirstOrDefault();
                Assert.IsNotNull(actualMemberName);
                Assert.IsFalse(validationResults[0].MemberNames.Skip(1).Any());
                Assert.AreEqual(nameof(Volume.MaxNameLength), actualMemberName);
            }
        }

        [DataTestMethod]
        [DataRow(null, "", DisplayName = "string Notes = null")]
        [DataRow("", "", DisplayName = "string Notes = \"\"")]
        [DataRow("\n\r", "", DisplayName = "string Notes = \"\\n\\r\"")]
        [DataRow("Test", "Test", DisplayName = "string Notes = \"Test\"")]
        [DataRow("\n Test \r", "\n Test \r", DisplayName = "string Notes = \"\\n Test \\r\"")]
        public void NotesTestMethod(string notes, string expected)
        {
            Volume target = new() { DisplayName = "Test", FileSystem = new(), Identifier = new VolumeIdentifier(Guid.NewGuid()) };
            target.Notes = notes;
            string actualValue = target.Notes;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("FileSystem FileSystem")]
        [TestProperty(TestHelper.TestProperty_Description, "Volume.FileSystem: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES FileSystems")]
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

        [DataTestMethod]
        [DataRow(
            null, // lastSynchronizedOn
            null, // upstreamId
            "6/5/2021 11:48:58 PM", // createdOn
            "6/8/2021 1:12:34 PM", // modifiedOn
            null, // errorMessage
            DisplayName = "string UpstreamId = null; LastSynchronizedOn = null")]
        [DataRow(
            null, // dateTime
            "81217ede-7cb8-43b8-ace5-8ae10e861303", // upstreamId
            "6/5/2021 11:48:58 PM", // createdOn
            "6/8/2021 1:12:34 PM", // modifiedOn
            "\"LastSynchronizedOn\" date is required when \"Upstream Id\" is specified.", // errorMessage
            DisplayName = "string UpstreamId = \"81217ede-7cb8-43b8-ace5-8ae10e861303\"; LastSynchronizedOn = null")]
        [DataRow(
            "6/8/2021 1:12:34 PM", // lastSynchronizedOn
            null, // upstreamId
            "6/5/2021 11:48:58 PM", // createdOn
            "6/8/2021 1:12:34 PM", // modifiedOn
            null, // errorMessage
            DisplayName = "string UpstreamId = null; LastSynchronizedOn = \"6/5/2021 11:48:58 PM\"")]
        [DataRow(
            "6/8/2021 1:12:34 PM", // lastSynchronizedOn
            "81217ede-7cb8-43b8-ace5-8ae10e861303", // upstreamId
            "6/5/2021 11:48:58 PM", // createdOn
            "6/8/2021 1:12:34 PM", // modifiedOn
            null, // errorMessage
            DisplayName = "string UpstreamId = \"81217ede-7cb8-43b8-ace5-8ae10e861303\"; LastSynchronizedOn = \"6/5/2021 11:48:58 PM\"")]
        // TODO: Add tests for LastSynchronizedOn out of range
        [TestProperty(TestHelper.TestProperty_Description,
            "Volume.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void LastSynchronizedOnTestMethod(string lastSynchronizedOn, string upstreamId, string createdOn, string modifiedOn, string errorMessage)
        {
            Volume target = new()
            {
                DisplayName = "Test",
                FileSystem = new(),
                Identifier = new VolumeIdentifier(Guid.NewGuid()),
                CreatedOn = DateTime.Parse(createdOn),
                ModifiedOn = DateTime.Parse(modifiedOn)
            };
            DateTime? expectedDateTime = string.IsNullOrWhiteSpace(lastSynchronizedOn) ? null : DateTime.Parse(lastSynchronizedOn);
            Guid? expectedGuid = string.IsNullOrWhiteSpace(upstreamId) ? null : Guid.Parse(upstreamId);
            target.UpstreamId = expectedGuid;
            target.LastSynchronizedOn = expectedDateTime;
            DateTime? actualDateTime = target.LastSynchronizedOn;
            Assert.AreEqual(expectedDateTime, actualDateTime);
            Guid? actualGuid = target.UpstreamId;
            Assert.AreEqual(expectedGuid, actualGuid);
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            if (string.IsNullOrWhiteSpace(errorMessage))
                Assert.IsTrue(isValid);
            else
            {
                Assert.IsFalse(isValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.AreEqual(validationResults[0].ErrorMessage, errorMessage);
                string actualMemberName = validationResults[0].MemberNames.FirstOrDefault();
                Assert.IsNotNull(actualMemberName);
                Assert.IsFalse(validationResults[0].MemberNames.Skip(1).Any());
                Assert.AreEqual(nameof(Volume.LastSynchronizedOn), actualMemberName);
            }
        }

        [TestMethod("DateTime CreatedOn")]
        [TestProperty(TestHelper.TestProperty_Description, "BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Volume target = new() { DisplayName = "Test", FileSystem = new(), Identifier = new VolumeIdentifier(Guid.NewGuid()) };
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
