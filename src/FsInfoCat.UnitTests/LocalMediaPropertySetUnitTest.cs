using FsInfoCat.Local.Model;
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
    public class LocalMediaPropertySetUnitTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void OnTestInitialize()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            TestHelper.UndoChanges(dbContext);
        }

        [TestMethod("MediaPropertySet Constructor Tests"), Ignore]
        public void MediaPropertySetConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            MediaPropertySet target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.AreEqual(string.Empty, target.ContentDistributor);
            Assert.AreEqual(string.Empty, target.CreatorApplication);
            Assert.AreEqual(string.Empty, target.CreatorApplicationVersion);
            Assert.AreEqual(string.Empty, target.DateReleased);
            Assert.IsNull(target.Duration);
            Assert.AreEqual(string.Empty, target.DVDID);
            Assert.IsNull(target.FrameCount);
            Assert.IsNull(target.Producer);
            Assert.AreEqual(string.Empty, target.ProtectionType);
            Assert.AreEqual(string.Empty, target.ProviderRating);
            Assert.AreEqual(string.Empty, target.ProviderStyle);
            Assert.AreEqual(string.Empty, target.Publisher);
            Assert.AreEqual(string.Empty, target.Subtitle);
            Assert.IsNull(target.Writer);
            Assert.IsNull(target.Year);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
        }

        [TestMethod("MediaPropertySet Add/Remove Tests"), Ignore]
        public void MediaPropertySetAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            MediaPropertySet target = new();
            EntityEntry<MediaPropertySet> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.MediaPropertySets.Add(target);
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
            Assert.IsNull(target.ContentDistributor);
            Assert.IsNull(target.CreatorApplication);
            Assert.IsNull(target.CreatorApplicationVersion);
            Assert.IsNull(target.DateReleased);
            Assert.IsNull(target.Duration);
            Assert.IsNull(target.DVDID);
            Assert.IsNull(target.FrameCount);
            Assert.IsNull(target.Producer);
            Assert.IsNull(target.ProtectionType);
            Assert.IsNull(target.ProviderRating);
            Assert.IsNull(target.ProviderStyle);
            Assert.IsNull(target.Publisher);
            Assert.IsNull(target.Subtitle);
            Assert.IsNull(target.Writer);
            Assert.IsNull(target.Year);
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
        public void MediaPropertySetIdTestMethod()
        {
            MediaPropertySet target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsExactly<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("MediaPropertySet ContentDistributor Validation Tests"), Ignore]
        [Description("MediaPropertySet.ContentDistributor: NVARCHAR(256)")]
        public void MediaPropertySetContentDistributorTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { ContentDistributor = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.ContentDistributor), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ContentDistributor);

            expected = default; // DEFERRED: Set valid value
            target.ContentDistributor = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ContentDistributor);

            expected = default; // DEFERRED: Set invalid value
            target.ContentDistributor = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.ContentDistributor), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ContentDistributor);
        }

        [TestMethod("MediaPropertySet CreatorApplication Validation Tests"), Ignore]
        [Description("MediaPropertySet.CreatorApplication: NVARCHAR(256)")]
        public void MediaPropertySetCreatorApplicationTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { CreatorApplication = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.CreatorApplication), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.CreatorApplication);

            expected = default; // DEFERRED: Set valid value
            target.CreatorApplication = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.CreatorApplication);

            expected = default; // DEFERRED: Set invalid value
            target.CreatorApplication = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.CreatorApplication), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.CreatorApplication);
        }

        [TestMethod("MediaPropertySet CreatorApplicationVersion Validation Tests"), Ignore]
        [Description("MediaPropertySet.CreatorApplicationVersion: NVARCHAR(64)")]
        public void MediaPropertySetCreatorApplicationVersionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { CreatorApplicationVersion = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.CreatorApplicationVersion), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.CreatorApplicationVersion);

            expected = default; // DEFERRED: Set valid value
            target.CreatorApplicationVersion = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.CreatorApplicationVersion);

            expected = default; // DEFERRED: Set invalid value
            target.CreatorApplicationVersion = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.CreatorApplicationVersion), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.CreatorApplicationVersion);
        }

        [TestMethod("MediaPropertySet DateReleased Validation Tests"), Ignore]
        [Description("MediaPropertySet.DateReleased: NVARCHAR(64)")]
        public void MediaPropertySetDateReleasedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { DateReleased = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.DateReleased), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.DateReleased);

            expected = default; // DEFERRED: Set valid value
            target.DateReleased = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DateReleased);

            expected = default; // DEFERRED: Set invalid value
            target.DateReleased = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.DateReleased), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.DateReleased);
        }

        [TestMethod("MediaPropertySet Duration Validation Tests"), Ignore]
        [Description("MediaPropertySet.Duration: BIGINT")]
        public void MediaPropertySetDurationTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            ulong? expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { Duration = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Duration), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Duration);

            expected = default; // DEFERRED: Set valid value
            target.Duration = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Duration);

            expected = default; // DEFERRED: Set invalid value
            target.Duration = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Duration), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Duration);
        }

        [TestMethod("MediaPropertySet DVDID Validation Tests"), Ignore]
        [Description("MediaPropertySet.DVDID: NVARCHAR(64)")]
        public void MediaPropertySetDVDIDTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { DVDID = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.DVDID), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.DVDID);

            expected = default; // DEFERRED: Set valid value
            target.DVDID = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.DVDID);

            expected = default; // DEFERRED: Set invalid value
            target.DVDID = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.DVDID), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.DVDID);
        }

        [TestMethod("MediaPropertySet FrameCount Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "MediaPropertySet.FrameCount: BIGINT \"FrameCount\" IS NULL OR (\"FrameCount\">=0 AND \"FrameCount\"<4294967296)")]
        public void MediaPropertySetFrameCountTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { FrameCount = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.FrameCount), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.FrameCount);

            expected = default; // DEFERRED: Set valid value
            target.FrameCount = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.FrameCount);

            expected = default; // DEFERRED: Set invalid value
            target.FrameCount = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.FrameCount), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.FrameCount);
        }

        [TestMethod("MediaPropertySet Producer Validation Tests"), Ignore]
        [Description("MediaPropertySet.Producer: TEXT")]
        public void MediaPropertySetProducerTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string[] expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { Producer = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Producer), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual<Collections.MultiStringValue>(expected, target.Producer);

            expected = default; // DEFERRED: Set valid value
            target.Producer = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual<Collections.MultiStringValue>(expected, target.Producer);

            expected = default; // DEFERRED: Set invalid value
            target.Producer = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Producer), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual<Collections.MultiStringValue>(expected, target.Producer);
        }

        [TestMethod("MediaPropertySet ProtectionType Validation Tests"), Ignore]
        [Description("MediaPropertySet.ProtectionType: NVARCHAR(256)")]
        public void MediaPropertySetProtectionTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { ProtectionType = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.ProtectionType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ProtectionType);

            expected = default; // DEFERRED: Set valid value
            target.ProtectionType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ProtectionType);

            expected = default; // DEFERRED: Set invalid value
            target.ProtectionType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.ProtectionType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ProtectionType);
        }

        [TestMethod("MediaPropertySet ProviderRating Validation Tests"), Ignore]
        [Description("MediaPropertySet.ProviderRating: NVARCHAR(256)")]
        public void MediaPropertySetProviderRatingTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { ProviderRating = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.ProviderRating), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ProviderRating);

            expected = default; // DEFERRED: Set valid value
            target.ProviderRating = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ProviderRating);

            expected = default; // DEFERRED: Set invalid value
            target.ProviderRating = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.ProviderRating), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ProviderRating);
        }

        [TestMethod("MediaPropertySet ProviderStyle Validation Tests"), Ignore]
        [Description("MediaPropertySet.ProviderStyle: NVARCHAR(256)")]
        public void MediaPropertySetProviderStyleTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { ProviderStyle = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.ProviderStyle), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ProviderStyle);

            expected = default; // DEFERRED: Set valid value
            target.ProviderStyle = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ProviderStyle);

            expected = default; // DEFERRED: Set invalid value
            target.ProviderStyle = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.ProviderStyle), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ProviderStyle);
        }

        [TestMethod("MediaPropertySet Publisher Validation Tests"), Ignore]
        [Description("MediaPropertySet.Publisher: NVARCHAR(256)")]
        public void MediaPropertySetPublisherTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { Publisher = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Publisher), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Publisher);

            expected = default; // DEFERRED: Set valid value
            target.Publisher = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Publisher);

            expected = default; // DEFERRED: Set invalid value
            target.Publisher = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Publisher), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Publisher);
        }

        [TestMethod("MediaPropertySet Subtitle Validation Tests"), Ignore]
        [Description("MediaPropertySet.Subtitle: NVARCHAR(256)")]
        public void MediaPropertySetSubtitleTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { Subtitle = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Subtitle), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Subtitle);

            expected = default; // DEFERRED: Set valid value
            target.Subtitle = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Subtitle);

            expected = default; // DEFERRED: Set invalid value
            target.Subtitle = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Subtitle), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Subtitle);
        }

        [TestMethod("MediaPropertySet Writer Validation Tests"), Ignore]
        [Description("MediaPropertySet.Writer: TEXT")]
        public void MediaPropertySetWriterTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string[] expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { Writer = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Writer), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual<Collections.MultiStringValue>(expected, target.Writer);

            expected = default; // DEFERRED: Set valid value
            target.Writer = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual<Collections.MultiStringValue>(expected, target.Writer);

            expected = default; // DEFERRED: Set invalid value
            target.Writer = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Writer), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual<Collections.MultiStringValue>(expected, target.Writer);
        }

        [TestMethod("MediaPropertySet Year Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "MediaPropertySet.Year: BIGINT \"Year\" IS NULL OR (\"Year\">=0 AND \"Year\"<4294967296)")]
        public void MediaPropertySetYearTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            MediaPropertySet target = new() { Year = expected };
            EntityEntry<MediaPropertySet> entityEntry = dbContext.MediaPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Year), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Year);

            expected = default; // DEFERRED: Set valid value
            target.Year = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Year);

            expected = default; // DEFERRED: Set invalid value
            target.Year = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(MediaPropertySet.Year), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.MediaPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Year);
        }
    }
}
