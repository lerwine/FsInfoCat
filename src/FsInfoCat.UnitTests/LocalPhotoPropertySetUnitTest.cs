using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalPhotoPropertySetUnitTest
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
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            dbContext.RejectChanges();
        }

        [TestMethod("PhotoPropertySet Add/Remove Tests")]
        [Ignore]
        public void PhotoPropertySetAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.PhotoPropertySet target = new();
            EntityEntry<Local.PhotoPropertySet> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.PhotoPropertySets.Add(target);
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            entityEntry.Reload();
            // DEFERRED: Validate default values
            Assert.IsNull(target.CameraManufacturer);
            Assert.IsNull(target.CameraModel);
            Assert.IsNull(target.DateTaken);
            Assert.IsNull(target.Event);
            Assert.IsNull(target.EXIFVersion);
            Assert.IsNull(target.Orientation);
            Assert.IsNull(target.OrientationText);
            Assert.IsNull(target.PeopleNames);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("Guid Id")]
        [Ignore]
        public void PhotoPropertySetIdTestMethod()
        {
            Local.PhotoPropertySet target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("PhotoPropertySet CameraManufacturer Validation Tests")]
        [Description("PhotoPropertySet.CameraManufacturer: NVARCHAR(256)")]
        [Ignore]
        public void PhotoPropertySetCameraManufacturerTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.PhotoPropertySet target = new() { CameraManufacturer = expected };
            EntityEntry<Local.PhotoPropertySet> entityEntry = dbContext.PhotoPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.CameraManufacturer), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.CameraManufacturer);

            expected = default; // DEFERRED: Set valid value
            target.CameraManufacturer = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.CameraManufacturer);

            expected = default; // DEFERRED: Set invalid value
            target.CameraManufacturer = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.CameraManufacturer), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.PhotoPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.CameraManufacturer);
        }

        [TestMethod("PhotoPropertySet CameraModel Validation Tests")]
        [Description("PhotoPropertySet.CameraModel: NVARCHAR(256)")]
        [Ignore]
        public void PhotoPropertySetCameraModelTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.PhotoPropertySet target = new() { CameraModel = expected };
            EntityEntry<Local.PhotoPropertySet> entityEntry = dbContext.PhotoPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.CameraModel), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.CameraModel);

            expected = default; // DEFERRED: Set valid value
            target.CameraModel = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.CameraModel);

            expected = default; // DEFERRED: Set invalid value
            target.CameraModel = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.CameraModel), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.PhotoPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.CameraModel);
        }

        [TestMethod("PhotoPropertySet DateTaken Validation Tests")]
        [Description("PhotoPropertySet.DateTaken: DATETIME")]
        [Ignore]
        public void PhotoPropertySetDateTakenTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            DateTime? expected = default; // DEFERRED: Set invalid value
            Local.PhotoPropertySet target = new() { DateTaken = expected };
            EntityEntry<Local.PhotoPropertySet> entityEntry = dbContext.PhotoPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.DateTaken), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.DateTaken);

            expected = default; // DEFERRED: Set valid value
            target.DateTaken = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DateTaken);

            expected = default; // DEFERRED: Set invalid value
            target.DateTaken = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.DateTaken), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.PhotoPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.DateTaken);
        }

        [TestMethod("PhotoPropertySet Event Validation Tests")]
        [Description("PhotoPropertySet.Event: TEXT")]
        [Ignore]
        public void PhotoPropertySetEventTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            MultiStringValue expected = default; // DEFERRED: Set invalid value
            Local.PhotoPropertySet target = new() { Event = expected };
            EntityEntry<Local.PhotoPropertySet> entityEntry = dbContext.PhotoPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.Event), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Event);

            expected = default; // DEFERRED: Set valid value
            target.Event = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Event);

            expected = default; // DEFERRED: Set invalid value
            target.Event = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.Event), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.PhotoPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Event);
        }

        [TestMethod("PhotoPropertySet EXIFVersion Validation Tests")]
        [Description("PhotoPropertySet.EXIFVersion: NVARCHAR(256)")]
        [Ignore]
        public void PhotoPropertySetEXIFVersionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.PhotoPropertySet target = new() { EXIFVersion = expected };
            EntityEntry<Local.PhotoPropertySet> entityEntry = dbContext.PhotoPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.EXIFVersion), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.EXIFVersion);

            expected = default; // DEFERRED: Set valid value
            target.EXIFVersion = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.EXIFVersion);

            expected = default; // DEFERRED: Set invalid value
            target.EXIFVersion = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.EXIFVersion), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.PhotoPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.EXIFVersion);
        }

        [TestMethod("PhotoPropertySet Orientation Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "PhotoPropertySet.Orientation: INT \"Orientation\" IS NULL OR (\"Orientation\">=0 AND \"Orientation\"<65536)")]
        [Ignore]
        public void PhotoPropertySetOrientationTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            ushort? expected = default; // DEFERRED: Set invalid value
            Local.PhotoPropertySet target = new() { Orientation = expected };
            EntityEntry<Local.PhotoPropertySet> entityEntry = dbContext.PhotoPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.Orientation), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Orientation);

            expected = default; // DEFERRED: Set valid value
            target.Orientation = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Orientation);

            expected = default; // DEFERRED: Set invalid value
            target.Orientation = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.Orientation), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.PhotoPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Orientation);
        }

        [TestMethod("PhotoPropertySet OrientationText Validation Tests")]
        [Description("PhotoPropertySet.OrientationText: NVARCHAR")]
        [Ignore]
        public void PhotoPropertySetOrientationTextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.PhotoPropertySet target = new() { OrientationText = expected };
            EntityEntry<Local.PhotoPropertySet> entityEntry = dbContext.PhotoPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.OrientationText), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.OrientationText);

            expected = default; // DEFERRED: Set valid value
            target.OrientationText = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.OrientationText);

            expected = default; // DEFERRED: Set invalid value
            target.OrientationText = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.OrientationText), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.PhotoPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.OrientationText);
        }

        [TestMethod("PhotoPropertySet PeopleNames Validation Tests")]
        [Description("PhotoPropertySet.PeopleNames: TEXT")]
        [Ignore]
        public void PhotoPropertySetPeopleNamesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            MultiStringValue expected = default; // DEFERRED: Set invalid value
            Local.PhotoPropertySet target = new() { PeopleNames = expected };
            EntityEntry<Local.PhotoPropertySet> entityEntry = dbContext.PhotoPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.PeopleNames), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.PeopleNames);

            expected = default; // DEFERRED: Set valid value
            target.PeopleNames = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.PeopleNames);

            expected = default; // DEFERRED: Set invalid value
            target.PeopleNames = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.PhotoPropertySet.PeopleNames), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.PhotoPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.PeopleNames);
        }
    }
}
