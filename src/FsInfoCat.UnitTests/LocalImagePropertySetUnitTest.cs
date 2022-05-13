using FsInfoCat.Local;
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
    public class LocalImagePropertySetUnitTest
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

        [TestMethod("ImagePropertySet Constructor Tests"), Ignore]
        public void ImagePropertySetConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            ImagePropertySet target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.BitDepth);
            Assert.IsNull(target.ColorSpace);
            Assert.IsNull(target.CompressedBitsPerPixel);
            Assert.IsNull(target.Compression);
            Assert.AreEqual(string.Empty, target.CompressionText);
            Assert.IsNull(target.CompressedBitsPerPixel);
            Assert.IsNull(target.Compression);
            Assert.AreEqual(string.Empty, target.CompressionText);
            Assert.IsNull(target.HorizontalResolution);
            Assert.IsNull(target.HorizontalSize);
            Assert.AreEqual(string.Empty, target.ImageID);
            Assert.IsNull(target.ResolutionUnit);
            Assert.IsNull(target.VerticalResolution);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
        }

        [TestMethod("ImagePropertySet Add/Remove Tests"), Ignore]
        public void ImagePropertySetAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            ImagePropertySet target = new();
            EntityEntry<ImagePropertySet> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.ImagePropertySets.Add(target);
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
            Assert.IsNull(target.BitDepth);
            Assert.IsNull(target.ColorSpace);
            Assert.IsNull(target.CompressedBitsPerPixel);
            Assert.IsNull(target.Compression);
            Assert.IsNull(target.CompressionText);
            Assert.IsNull(target.HorizontalResolution);
            Assert.IsNull(target.HorizontalSize);
            Assert.IsNull(target.ImageID);
            Assert.IsNull(target.ResolutionUnit);
            Assert.IsNull(target.VerticalResolution);
            Assert.IsNull(target.VerticalSize);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("Guid Id"), Ignore]
        public void ImagePropertySetIdTestMethod()
        {
            ImagePropertySet target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("ImagePropertySet BitDepth Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "ImagePropertySet.BitDepth: BIGINT \"BitDepth\" IS NULL OR (\"BitDepth\">=0 AND \"BitDepth\"<4294967296)")]
        public void ImagePropertySetBitDepthTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { BitDepth = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.BitDepth), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.BitDepth);

            expected = default; // DEFERRED: Set valid value
            target.BitDepth = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.BitDepth);

            expected = default; // DEFERRED: Set invalid value
            target.BitDepth = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.BitDepth), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.BitDepth);
        }

        [TestMethod("ImagePropertySet ColorSpace Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "ImagePropertySet.ColorSpace: INT \"ColorSpace\" IS NULL OR (\"ColorSpace\">=0 AND \"ColorSpace\"<65536)")]
        public void ImagePropertySetColorSpaceTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            ushort? expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { ColorSpace = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.ColorSpace), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ColorSpace);

            expected = default; // DEFERRED: Set valid value
            target.ColorSpace = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ColorSpace);

            expected = default; // DEFERRED: Set invalid value
            target.ColorSpace = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.ColorSpace), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ColorSpace);
        }

        [TestMethod("ImagePropertySet CompressedBitsPerPixel Validation Tests"), Ignore]
        [Description("ImagePropertySet.CompressedBitsPerPixel: DOUBLE")]
        public void ImagePropertySetCompressedBitsPerPixelTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            double? expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { CompressedBitsPerPixel = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.CompressedBitsPerPixel), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.CompressedBitsPerPixel);

            expected = default; // DEFERRED: Set valid value
            target.CompressedBitsPerPixel = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.CompressedBitsPerPixel);

            expected = default; // DEFERRED: Set invalid value
            target.CompressedBitsPerPixel = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.CompressedBitsPerPixel), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.CompressedBitsPerPixel);
        }

        [TestMethod("ImagePropertySet Compression Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "ImagePropertySet.Compression: INT \"Compression\" IS NULL OR (\"Compression\">=0 AND \"Compression\"<65536)")]
        public void ImagePropertySetCompressionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            ushort? expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { Compression = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.Compression), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Compression);

            expected = default; // DEFERRED: Set valid value
            target.Compression = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Compression);

            expected = default; // DEFERRED: Set invalid value
            target.Compression = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.Compression), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Compression);
        }

        [TestMethod("ImagePropertySet CompressionText Validation Tests"), Ignore]
        [Description("ImagePropertySet.CompressionText: NVARCHAR(256)")]
        public void ImagePropertySetCompressionTextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { CompressionText = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.CompressionText), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.CompressionText);

            expected = default; // DEFERRED: Set valid value
            target.CompressionText = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.CompressionText);

            expected = default; // DEFERRED: Set invalid value
            target.CompressionText = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.CompressionText), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.CompressionText);
        }

        [TestMethod("ImagePropertySet HorizontalResolution Validation Tests"), Ignore]
        [Description("ImagePropertySet.HorizontalResolution: DOUBLE")]
        public void ImagePropertySetHorizontalResolutionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            double? expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { HorizontalResolution = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.HorizontalResolution), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.HorizontalResolution);

            expected = default; // DEFERRED: Set valid value
            target.HorizontalResolution = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.HorizontalResolution);

            expected = default; // DEFERRED: Set invalid value
            target.HorizontalResolution = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.HorizontalResolution), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.HorizontalResolution);
        }

        [TestMethod("ImagePropertySet HorizontalSize Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "ImagePropertySet.HorizontalSize: BIGINT \"HorizontalSize\" IS NULL OR (\"HorizontalSize\">=0 AND \"HorizontalSize\"<4294967296)")]
        public void ImagePropertySetHorizontalSizeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { HorizontalSize = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.HorizontalSize), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.HorizontalSize);

            expected = default; // DEFERRED: Set valid value
            target.HorizontalSize = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.HorizontalSize);

            expected = default; // DEFERRED: Set invalid value
            target.HorizontalSize = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.HorizontalSize), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.HorizontalSize);
        }

        [TestMethod("ImagePropertySet ImageID Validation Tests"), Ignore]
        [Description("ImagePropertySet.ImageID: NVARCHAR(256)")]
        public void ImagePropertySetImageIDTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { ImageID = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.ImageID), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ImageID);

            expected = default; // DEFERRED: Set valid value
            target.ImageID = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ImageID);

            expected = default; // DEFERRED: Set invalid value
            target.ImageID = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.ImageID), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ImageID);
        }

        [TestMethod("ImagePropertySet ResolutionUnit Validation Tests"), Ignore]
        [Description("ImagePropertySet.ResolutionUnit: TINYINT")]
        public void ImagePropertySetResolutionUnitTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            short? expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { ResolutionUnit = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.ResolutionUnit), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ResolutionUnit);

            expected = default; // DEFERRED: Set valid value
            target.ResolutionUnit = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ResolutionUnit);

            expected = default; // DEFERRED: Set invalid value
            target.ResolutionUnit = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.ResolutionUnit), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ResolutionUnit);
        }

        [TestMethod("ImagePropertySet VerticalResolution Validation Tests"), Ignore]
        [Description("ImagePropertySet.VerticalResolution: DOUBLE")]
        public void ImagePropertySetVerticalResolutionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            double? expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { VerticalResolution = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.VerticalResolution), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.VerticalResolution);

            expected = default; // DEFERRED: Set valid value
            target.VerticalResolution = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.VerticalResolution);

            expected = default; // DEFERRED: Set invalid value
            target.VerticalResolution = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.VerticalResolution), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.VerticalResolution);
        }

        [TestMethod("ImagePropertySet VerticalSize Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "ImagePropertySet.VerticalSize: BIGINT \"VerticalSize\" IS NULL OR (\"VerticalSize\">=0 AND \"VerticalSize\"<4294967296)")]
        public void ImagePropertySetVerticalSizeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            ImagePropertySet target = new() { VerticalSize = expected };
            EntityEntry<ImagePropertySet> entityEntry = dbContext.ImagePropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.VerticalSize), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.VerticalSize);

            expected = default; // DEFERRED: Set valid value
            target.VerticalSize = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.VerticalSize);

            expected = default; // DEFERRED: Set invalid value
            target.VerticalSize = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.ImagePropertySet.VerticalSize), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.ImagePropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.VerticalSize);
        }
    }
}
