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
    public class LocalExtendedPropertiesUnitTest
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("ExtendedProperties Add/Remove Tests")]
        public void ExtendedPropertiesAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ExtendedProperties target = new() { /* TODO: Initialize properties */ };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.ExtendedProperties.Add(target);
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
            // TODO: Validate default values
            Assert.AreEqual(0, target.Height);
            Assert.AreEqual(0, target.Width);
            Assert.IsNull(target.Duration);
            Assert.IsNull(target.FrameCount);
            Assert.IsNull(target.TrackNumber);
            Assert.IsNull(target.Bitrate);
            Assert.IsNull(target.FrameRate);
            Assert.IsNull(target.SamplesPerPixel);
            Assert.IsNull(target.PixelPerUnitX);
            Assert.IsNull(target.PixelPerUnitY);
            Assert.IsNull(target.Compression);
            Assert.IsNull(target.XResNumerator);
            Assert.IsNull(target.XResDenominator);
            Assert.IsNull(target.YResNumerator);
            Assert.IsNull(target.YResDenominator);
            Assert.IsNull(target.ResolutionXUnit);
            Assert.IsNull(target.ResolutionYUnit);
            Assert.IsNull(target.JPEGProc);
            Assert.IsNull(target.JPEGQuality);
            Assert.IsNull(target.DateTime);
            Assert.IsNull(target.Title);
            Assert.IsNull(target.Description);
            Assert.IsNull(target.Copyright);
            Assert.IsNull(target.SoftwareUsed);
            Assert.IsNull(target.Artist);
            Assert.IsNull(target.HostComputer);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("ExtendedProperties Height Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "ExtendedProperties.Height: INT NOT NULL CHECK(Height>=0 AND Height<65536)")]
        public void ExtendedPropertiesHeightTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ExtendedProperties target = new() { Height = 0 };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.ExtendedProperties.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(0, target.Height);
            // TODO: Validate default values

            ushort expected = 12;
            target.Height = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(expected, target.Height);
        }

        [TestMethod("ExtendedProperties Width Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "ExtendedProperties.Width: INT NOT NULL CHECK(Height>=0 AND Height<65536)")]
        public void ExtendedPropertiesWidthTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            ushort expected = default; // TODO: Set invalid value
            Local.ExtendedProperties target = new() { Width = expected };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.ExtendedProperties.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ExtendedProperties.Width), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Width);

            expected = default; // TODO: Set valid value
            target.Width = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Width);

            expected = default; // TODO: Set invalid value
            target.Width = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ExtendedProperties.Width), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ExtendedProperties.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Width);
        }

        [TestMethod("ExtendedProperties CreatedOn Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "ExtendedProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void ExtendedPropertiesCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ExtendedProperties target = new() {  /* TODO: Initialize properties */ };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.ExtendedProperties.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ExtendedProperties.CreatedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_CreatedOnAfterModifiedOn, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.CreatedOn = target.ModifiedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.ExtendedProperties.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("ExtendedProperties LastSynchronizedOn Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description,
            "ExtendedProperties.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void ExtendedPropertiesLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.ExtendedProperties target = new() {  /* TODO: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<Local.ExtendedProperties> entityEntry = dbContext.ExtendedProperties.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();

            target.CreatedOn = target.ModifiedOn.AddDays(-1);
            target.LastSynchronizedOn = target.CreatedOn.AddDays(0.5);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.ExtendedProperties.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.ExtendedProperties.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.ExtendedProperties.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.ExtendedProperties.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }
    }
}
