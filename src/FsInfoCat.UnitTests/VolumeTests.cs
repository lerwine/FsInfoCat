using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [TestMethod("new Volume()"), Priority(20)]
        public void NewVolumeTestMethod()
        {
            Volume target = new();
            //Assert.IsFalse(target.IsChanged());
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNotNull(target.DisplayName);
            Assert.AreEqual("", target.DisplayName);
            Assert.IsNotNull(target.VolumeName);
            Assert.AreEqual("", target.VolumeName);
            Assert.IsTrue(target.Identifier.IsEmpty());
            Assert.AreEqual(VolumeStatus.Unknown, target.Status);
            Assert.AreEqual(DriveType.Unknown, target.Type);
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

            string expectedDisplayName = "Example Volume";
            target.DisplayName = expectedDisplayName;
            Guid expectedFileSystemId = Guid.Parse("88a3cdb9-ed66-4778-a33b-437675a5ae38");
            target.FileSystemId = expectedFileSystemId;
            VolumeIdentifier expectedIdentifier = new VolumeIdentifier(0xFb987a58u);
            target.Identifier = expectedIdentifier;
            string expectedVolumeName = "Test_Vol";
            target.VolumeName = expectedVolumeName;
            target.Status = VolumeStatus.Controlled;
            target.Type = DriveType.CDRom;
            target.ReadOnly = true;
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod("Guid Id"), Priority(20)]
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

        public static IEnumerable<object[]> GetDisplayNameTestData() =>
            XDocument.Parse(Properties.Resources.DisplayName1024).Root.Elements("Row").Select(row =>
            {
                XElement a = row.Element("Argument");
                XElement e = row.Element("Expected");
                XAttribute m = row.Attribute("ErrorMessage");
                return new object[] { a.IsEmpty ? null : a.Value, e.Value, (m is null) ? "" : m.Value };
            });

        [DataTestMethod, Priority(20)]
        [DynamicData(nameof(GetDisplayNameTestData), DynamicDataSourceType.Method)]
        [Description("Volume.DisplayName: NVARCHAR(1024) NOT NULL CHECK(length(trim(DisplayName))>0) COLLATE NOCASE")]
        public void DisplayNameTestMethod(string displayName, string expected, string errorMessage)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = dbContext.Database.BeginTransaction();
            Volume target = new() { DisplayName = displayName };
            Assert.IsTrue(target.IsChanged());
            target.FileSystem = dbContext.FileSystems.Find(Guid.Parse("0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd"));
            target.Identifier = new VolumeIdentifier(Guid.NewGuid());
            string actualValue = target.DisplayName;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            dbContext.Volumes.Add(target);
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                Assert.IsTrue(isValid);
                dbContext.SaveChanges();
            }
            else
            {
                Assert.IsFalse(isValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.AreEqual(validationResults[0].ErrorMessage, errorMessage);
                string actualMemberName = validationResults[0].MemberNames.FirstOrDefault();
                Assert.IsNotNull(actualMemberName);
                Assert.IsFalse(validationResults[0].MemberNames.Skip(1).Any());
                Assert.AreEqual(nameof(Volume.DisplayName), actualMemberName);
                Assert.ThrowsException<AggregateException>(() => dbContext.SaveChanges());
            }
        }

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

        [DataTestMethod, Priority(20)]
        [DynamicData(nameof(GetVolumeNameTestData), DynamicDataSourceType.Method)]
        [Description("Volume.VolumeName: NVARCHAR(128) NOT NULL CHECK(length(trim(VolumeName))>0) COLLATE NOCASE")]
        public void VolumeNameTestMethod(string volumeName, string expected, string errorMessage)
        {
            Volume target = new() { VolumeName = volumeName };
            Assert.IsTrue(target.IsChanged());
            target.DisplayName = "Test";
            target.FileSystem = new();
            target.Identifier = new VolumeIdentifier(Guid.NewGuid());
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

        [DataTestMethod, Priority(20)]
        [DynamicData(nameof(GetIdentifierTestData), DynamicDataSourceType.Method)]
        [Description("Volume.Identifier: NVARCHAR(1024) NOT NULL CHECK(length(trim(Identifier))>0) UNIQUE COLLATE NOCASE")]
        public void IdentifierTestMethod(string identifier, string expected, string errorMessage)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = dbContext.Database.BeginTransaction();
            Volume target = new() { Identifier = string.IsNullOrWhiteSpace(identifier) ? default : VolumeIdentifier.Parse(identifier) };
            Assert.IsTrue(target.IsChanged());
            target.DisplayName = "Test";
            target.FileSystem = dbContext.FileSystems.Find(Guid.Parse("0af7fe3e-3bc2-41ac-b6b1-310ad5fc46cd"));
            VolumeIdentifier expectedValue = string.IsNullOrWhiteSpace(expected) ? default : VolumeIdentifier.Parse(expected);
            VolumeIdentifier actualValue = target.Identifier;
            Assert.AreEqual(expectedValue, actualValue);
            Collection<ValidationResult> validationResults = new();
            bool isValid = Validator.TryValidateObject(target, new ValidationContext(target), validationResults, true);
            dbContext.Volumes.Add(target);
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                Assert.IsTrue(isValid);
                dbContext.SaveChanges();
            }
            else
            {
                Assert.IsFalse(isValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.AreEqual(validationResults[0].ErrorMessage, errorMessage);
                string actualMemberName = validationResults[0].MemberNames.FirstOrDefault();
                Assert.IsNotNull(actualMemberName);
                Assert.IsFalse(validationResults[0].MemberNames.Skip(1).Any());
                Assert.AreEqual(nameof(Volume.Identifier), actualMemberName);
                Assert.ThrowsException<AggregateException>(() => dbContext.SaveChanges());
            }
        }

        [DataTestMethod, Priority(20)]
        [DataRow(VolumeStatus.Unknown, null, DisplayName = "VolumeStatus Status = Unknown")]
        [DataRow(VolumeStatus.Controlled, null, DisplayName = "VolumeStatus Status = Controlled")]
        [DataRow(VolumeStatus.Uncontrolled, null, DisplayName = "VolumeStatus Status = Uncontrolled")]
        [DataRow(VolumeStatus.Offline, null, DisplayName = "VolumeStatus Status = Offline")]
        [DataRow(VolumeStatus.Relinquished, null, DisplayName = "VolumeStatus Status = Relinquished")]
        [DataRow(VolumeStatus.Destroyed, null, DisplayName = "VolumeStatus Status = Destroyed")]
        [DataRow(7, "Volume Status is invalid.", DisplayName = "VolumeStatus Status = 6")]
        [Description("Volume.Type: TINYINT NOT NULL CHECK(Status>=0 AND Status<6)")]
        public void StatusTestMethod(object sourceValue, string errorMessage)
        {
            VolumeStatus value = (VolumeStatus)Enum.ToObject(typeof(VolumeStatus), sourceValue);
            Volume target = new() { Status = value };
            Assert.IsTrue(target.IsChanged());
            target.DisplayName = "Test";
            target.FileSystem = new();
            target.Identifier = new VolumeIdentifier(Guid.NewGuid());
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

        [DataTestMethod, Priority(20)]
        [DataRow(DriveType.Unknown, null, DisplayName = "DriveType Type = Unknown")]
        [DataRow(DriveType.NoRootDirectory, null, DisplayName = "DriveType Type = NoRootDirectory")]
        [DataRow(DriveType.Removable, null, DisplayName = "DriveType Type = Removable")]
        [DataRow(DriveType.Fixed, null, DisplayName = "DriveType Type = Fixed")]
        [DataRow(DriveType.Network, null, DisplayName = "DriveType Type = Network")]
        [DataRow(DriveType.CDRom, null, DisplayName = "DriveType Type = CDRom")]
        [DataRow(DriveType.Ram, null, DisplayName = "DriveType Type = Ram")]
        [DataRow(7, "Drive Type is invalid.", DisplayName = "DriveType Type = 7")]
        [Description("Volume.Type: TINYINT NOT NULL CHECK(Type>=0 AND Type<7)")]
        public void TypeTestMethod(DriveType value, string errorMessage)
        {
            Volume target = new() { Type = value };
            Assert.IsTrue(target.IsChanged());
            target.DisplayName = "Test";
            target.FileSystem = new();
            target.Identifier = new VolumeIdentifier(Guid.NewGuid());
            target.DisplayName = "Test";
            target.FileSystem = new();
            target.Identifier = new VolumeIdentifier(Guid.NewGuid());
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

        [DataTestMethod, Priority(20)]
        [DataRow(null, DisplayName = "bool? ReadOnly = null")]
        [DataRow(true, DisplayName = "bool? ReadOnly = true")]
        [DataRow(false, DisplayName = "bool? ReadOnly = false")]
        public void ReadOnlyTestMethod(bool? value)
        {
            Volume target = new() { ReadOnly = value };
            Assert.IsTrue(target.IsChanged());
            bool? actualValue = target.ReadOnly;
            Assert.AreEqual(value, actualValue);
        }

        [DataTestMethod, Priority(20)]
        [DataRow(null, null, DisplayName = "uint? MaxNameLength = null")]
        [DataRow(1u, null, DisplayName = "uint? MaxNameLength = 1")]
        [DataRow((uint)int.MaxValue, null, DisplayName = "int? MaxNameLength = int.MaxValue")]
        [DataRow(0u, "Maximum Name Length must be greater than zero.", DisplayName = "uint? MaxNameLength = 0")]
        [Description("Volume.MaxNameLength: CHECK(MaxNameLength IS NULL OR MaxNameLength>=1)")]
        public void MaxNameLengthTestMethod(uint? value, string errorMessage)
        {
            Volume target = new() { MaxNameLength = value };
            Assert.IsTrue(target.IsChanged());
            target.DisplayName = "Test";
            target.FileSystem = new();
            target.Identifier = new VolumeIdentifier(Guid.NewGuid());
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

        [DataTestMethod, Priority(20)]
        [DataRow(null, "", DisplayName = "string Notes = null")]
        [DataRow("", "", DisplayName = "string Notes = \"\"")]
        [DataRow("\n\r", "", DisplayName = "string Notes = \"\\n\\r\"")]
        [DataRow("Test", "Test", DisplayName = "string Notes = \"Test\"")]
        [DataRow("\n Test \r", "\n Test \r", DisplayName = "string Notes = \"\\n Test \\r\"")]
        public void NotesTestMethod(string notes, string expected)
        {
            Volume target = new() { Notes = notes };
            Assert.IsTrue(target.IsChanged());
            string actualValue = target.Notes;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("FileSystem FileSystem"), Priority(20)]
        [Description("Volume.FileSystem: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES FileSystems")]
        public void FileSystemTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = dbContext.Database.BeginTransaction();
            Guid expectedFileSystemId = Guid.Parse("88a3cdb9-ed66-4778-a33b-437675a5ae38");
            Volume target = new() { FileSystemId = expectedFileSystemId };
            Assert.IsTrue(target.IsChanged());
            Assert.IsNull(target.FileSystem);
            target.DisplayName = "Test";
            target.Identifier = new VolumeIdentifier(Guid.NewGuid());
            Guid actualId = target.FileSystemId;
            Assert.AreEqual(expectedFileSystemId, actualId);
            dbContext.Volumes.Add(target);
            dbContext.SaveChanges();
            expectedFileSystemId = Guid.Parse("bd64e811-2c25-4385-8b99-1494bbb24612");
            FileSystem expectedValue = dbContext.FileSystems.Find(expectedFileSystemId);
            target.FileSystem = expectedValue;
            actualId = target.FileSystemId;
            FileSystem actualValue = target.FileSystem;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expectedFileSystemId, actualId);
            Assert.AreSame(expectedValue, actualValue);
            dbContext.Volumes.Update(target);
            dbContext.SaveChanges();
            target.FileSystem = null;
            dbContext.Volumes.Update(target);
            Assert.ThrowsException<DbUpdateException>(() => dbContext.SaveChanges());
        }

        [DataTestMethod, Priority(20)]
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
            DateTime? expectedDateTime = string.IsNullOrWhiteSpace(lastSynchronizedOn) ? null : DateTime.Parse(lastSynchronizedOn);
            Volume target = new() { LastSynchronizedOn = expectedDateTime };
            Assert.IsTrue(target.IsChanged());
            target.DisplayName = "Test"; target.FileSystem = new();
            target.Identifier = new VolumeIdentifier(Guid.NewGuid());
            target.CreatedOn = DateTime.Parse(createdOn);
            target.ModifiedOn = DateTime.Parse(modifiedOn);
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

        [TestMethod("DateTime CreatedOn"), Priority(20)]
        [Description("BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            DateTime expectedValue = DateTime.Now.AddDays(-1);
            Volume target = new() { CreatedOn = expectedValue };
            Assert.IsTrue(target.IsChanged());
            DateTime actualValue = target.CreatedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime ModifiedOn"), Priority(20)]
        public void ModifiedOnTestMethod()
        {
            DateTime expectedValue = DateTime.Now.AddDays(-1);
            Volume target = new() { ModifiedOn = expectedValue };
            Assert.IsTrue(target.IsChanged());
            DateTime actualValue = target.ModifiedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Item[string]"), Priority(20), Ignore]
        public void ItemColumnNameTestMethod()
        {
            // DEFERRED: Implement test for string Item[string]
            string columnNameIndex = default;
            Volume target = default; // TODO: Create and initialize Volume instance
            string expectedValue = default;
            string actualValue = target[columnNameIndex];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("IEnumerable<ValidationResult> Validate(ValidationContext)"), Priority(20), Ignore]
        public void ValidateValidationContextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for IEnumerable<ValidationResult> Validate(ValidationContext)

            ValidationContext validationContextArg = default;
            Volume target = default; // TODO: Create and initialize Volume instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()"), Priority(20), Ignore]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool HasErrors()

            Volume target = default; // TODO: Create and initialize Volume instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()"), Priority(20), Ignore]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for void AcceptChanges()

            Volume target = default; // TODO: Create and initialize Volume instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()"), Priority(20), Ignore]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool IsChanged()

            Volume target = default; // TODO: Create and initialize Volume instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()"), Priority(20), Ignore]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for void RejectChanges()

            Volume target = default; // TODO: Create and initialize Volume instance
            target.RejectChanges();
        }

        [TestMethod("DbContext.Volumes.Add(Volume)"), Priority(20)]
        public void DbInsertTestMethod()
        {
            string expectedDisplayName = "Example Volume";
            Guid expectedFileSystemId = Guid.Parse("88a3cdb9-ed66-4778-a33b-437675a5ae38");
            VolumeIdentifier expectedIdentifier = new VolumeIdentifier(0xFb987a58u);
            string expectedVolumeName = "Test_Vol";
            VolumeStatus expectedStatus = VolumeStatus.Controlled;
            DriveType expectdType = DriveType.CDRom;
            bool? expectedReadOnly = true;
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = dbContext.Database.BeginTransaction();
            Volume target = new()
            {
                DisplayName = expectedDisplayName,
                FileSystemId = expectedFileSystemId,
                Identifier = expectedIdentifier,
                VolumeName = expectedVolumeName,
                Status = expectedStatus,
                Type = expectdType,
                ReadOnly = expectedReadOnly
            };
            Assert.IsTrue(target.IsChanged());
            EntityEntry<Volume> entry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entry.State);
            dbContext.Volumes.Add(target);
            Assert.AreEqual(EntityState.Added, entry.State);
            Guid id = target.Id;
            Assert.AreNotEqual(Guid.Empty, id);
            Assert.AreEqual(expectedDisplayName, target.DisplayName);
            Assert.AreEqual(expectedVolumeName, target.VolumeName);
            Assert.AreEqual(expectedIdentifier, target.Identifier);
            Assert.AreEqual(VolumeStatus.Controlled, target.Status);
            Assert.AreEqual(DriveType.CDRom, target.Type);
            Assert.IsTrue(target.ReadOnly);
            Assert.IsTrue(target.IsChanged());
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entry.State);
            Assert.AreEqual(id, target.Id);
            Assert.AreEqual(expectedDisplayName, target.DisplayName);
            Assert.AreEqual(expectedVolumeName, target.VolumeName);
            Assert.AreEqual(expectedIdentifier, target.Identifier);
            Assert.AreEqual(VolumeStatus.Controlled, target.Status);
            Assert.AreEqual(DriveType.CDRom, target.Type);
            Assert.IsTrue(target.ReadOnly);
            Assert.IsFalse(target.IsChanged());
            target = new()
            {
                DisplayName = expectedDisplayName,
                FileSystemId = expectedFileSystemId,
                Identifier = new(Guid.NewGuid()),
                VolumeName = expectedVolumeName,
                Status = expectedStatus,
                Type = expectdType,
                ReadOnly = expectedReadOnly
            };
            dbContext.Volumes.Add(target);
            dbContext.SaveChanges();
            target = new()
            {
                DisplayName = expectedDisplayName,
                FileSystemId = expectedFileSystemId,
                Identifier = expectedIdentifier,
                VolumeName = expectedVolumeName,
                Status = expectedStatus,
                Type = expectdType,
                ReadOnly = expectedReadOnly
            };
            dbContext.Volumes.Add(target);
            Assert.ThrowsException<DbUpdateException>(() => dbContext.SaveChanges());
        }

        [TestMethod("DbContext.Volumes.Add(Volume)"), Priority(20)]
        public void DbUpdateTestMethod()
        {
            Guid targetId = Guid.Parse("355b32f0-d9c8-4a81-b894-24109fbbda64");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = dbContext.Database.BeginTransaction();
            Volume target = dbContext.Volumes.Find(targetId);
            if (target is null)
                Assert.Inconclusive($"Could not find Volume with ID of {targetId}");
            VolumeIdentifier originalIdentifier = target.Identifier;
            //Assert.IsFalse(target.IsChanged());
            EntityEntry<Volume> entry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Unchanged, entry.State);
            VolumeIdentifier expectedIdentifier = new(Guid.NewGuid());
            target.Identifier = expectedIdentifier;
            dbContext.Volumes.Update(target);
            Assert.AreEqual(EntityState.Modified, entry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entry.State);
            VolumeIdentifier actualIdentifier = target.Identifier;
            Assert.AreEqual(expectedIdentifier, actualIdentifier);
            target.Identifier = originalIdentifier;
            dbContext.Volumes.Update(target);
            Assert.AreEqual(EntityState.Modified, entry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entry.State);
            actualIdentifier = target.Identifier;
            Assert.AreEqual(originalIdentifier, actualIdentifier);
            target.Identifier = new(0xFD91BC0Cu);
            dbContext.Volumes.Update(target);
            Assert.AreEqual(EntityState.Modified, entry.State);
            Assert.ThrowsException<AggregateException>(() => dbContext.SaveChanges());
        }
    }
}
