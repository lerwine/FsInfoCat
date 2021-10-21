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
    public class LocalAudioPropertySetUnitTest
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

        [TestMethod("AudioPropertySet Add/Remove Tests")]
        [Ignore]
        public void AudioPropertySetAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.AudioPropertySet target = new();
            EntityEntry<Local.AudioPropertySet> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.AudioPropertySets.Add(target);
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
            Assert.IsNull(target.Compression);
            Assert.IsNull(target.EncodingBitrate);
            Assert.IsNull(target.Format);
            Assert.IsNull(target.IsVariableBitrate);
            Assert.IsNull(target.SampleRate);
            Assert.IsNull(target.SampleSize);
            Assert.IsNull(target.StreamName);
            Assert.IsNull(target.StreamNumber);
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
        public void AudioPropertySetIdTestMethod()
        {
            Local.AudioPropertySet target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("AudioPropertySet Compression Validation Tests")]
        [Description("AudioPropertySet.Compression: NVARCHAR(256)")]
        [Ignore]
        public void AudioPropertySetCompressionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.AudioPropertySet target = new() { Compression = expected };
            EntityEntry<Local.AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.Compression), results[0].MemberNames.First());
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
            Assert.AreEqual(nameof(Local.AudioPropertySet.Compression), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.AudioPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Compression);
        }

        [TestMethod("AudioPropertySet EncodingBitrate Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "AudioPropertySet.EncodingBitrate: BIGINT \"EncodingBitrate\" IS NULL OR (\"EncodingBitrate\">=0 AND \"EncodingBitrate\"<4294967296)")]
        [Ignore]
        public void AudioPropertySetEncodingBitrateTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            Local.AudioPropertySet target = new() { EncodingBitrate = expected };
            EntityEntry<Local.AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.EncodingBitrate), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.EncodingBitrate);

            expected = default; // DEFERRED: Set valid value
            target.EncodingBitrate = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.EncodingBitrate);

            expected = default; // DEFERRED: Set invalid value
            target.EncodingBitrate = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.EncodingBitrate), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.AudioPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.EncodingBitrate);
        }

        [TestMethod("AudioPropertySet Format Validation Tests")]
        [Description("AudioPropertySet.Format: NVARCHAR(256)")]
        [Ignore]
        public void AudioPropertySetFormatTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.AudioPropertySet target = new() { Format = expected };
            EntityEntry<Local.AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.Format), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Format);

            expected = default; // DEFERRED: Set valid value
            target.Format = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Format);

            expected = default; // DEFERRED: Set invalid value
            target.Format = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.Format), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.AudioPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Format);
        }

        [TestMethod("AudioPropertySet IsVariableBitrate Validation Tests")]
        [Description("AudioPropertySet.IsVariableBitrate: BIT")]
        [Ignore]
        public void AudioPropertySetIsVariableBitrateTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            bool? expected = default; // DEFERRED: Set invalid value
            Local.AudioPropertySet target = new() { IsVariableBitrate = expected };
            EntityEntry<Local.AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.IsVariableBitrate), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.IsVariableBitrate);

            expected = default; // DEFERRED: Set valid value
            target.IsVariableBitrate = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.IsVariableBitrate);

            expected = default; // DEFERRED: Set invalid value
            target.IsVariableBitrate = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.IsVariableBitrate), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.AudioPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.IsVariableBitrate);
        }

        [TestMethod("AudioPropertySet SampleRate Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "AudioPropertySet.SampleRate: BIGINT \"SampleRate\" IS NULL OR (\"SampleRate\">=0 AND \"SampleRate\"<4294967296)")]
        [Ignore]
        public void AudioPropertySetSampleRateTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            Local.AudioPropertySet target = new() { SampleRate = expected };
            EntityEntry<Local.AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.SampleRate), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.SampleRate);

            expected = default; // DEFERRED: Set valid value
            target.SampleRate = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.SampleRate);

            expected = default; // DEFERRED: Set invalid value
            target.SampleRate = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.SampleRate), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.AudioPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.SampleRate);
        }

        [TestMethod("AudioPropertySet SampleSize Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "AudioPropertySet.SampleSize: BIGINT \"SampleSize\" IS NULL OR (\"SampleSize\">=0 AND \"SampleSize\"<4294967296)")]
        [Ignore]
        public void AudioPropertySetSampleSizeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            Local.AudioPropertySet target = new() { SampleSize = expected };
            EntityEntry<Local.AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.SampleSize), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.SampleSize);

            expected = default; // DEFERRED: Set valid value
            target.SampleSize = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.SampleSize);

            expected = default; // DEFERRED: Set invalid value
            target.SampleSize = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.SampleSize), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.AudioPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.SampleSize);
        }

        [TestMethod("AudioPropertySet StreamName Validation Tests")]
        [Description("AudioPropertySet.StreamName: NVARCHAR(256)")]
        [Ignore]
        public void AudioPropertySetStreamNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            Local.AudioPropertySet target = new() { StreamName = expected };
            EntityEntry<Local.AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.StreamName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.StreamName);

            expected = default; // DEFERRED: Set valid value
            target.StreamName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.StreamName);

            expected = default; // DEFERRED: Set invalid value
            target.StreamName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.StreamName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.AudioPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.StreamName);
        }

        [TestMethod("AudioPropertySet StreamNumber Validation Tests")]
        [TestProperty(TestHelper.TestProperty_Description, "AudioPropertySet.StreamNumber: INT \"StreamNumber\" IS NULL OR (\"StreamNumber\">=0 AND \"StreamNumber\"<65536)")]
        [Ignore]
        public void AudioPropertySetStreamNumberTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using var dbContext = Hosting.ServiceProvider.GetService<Local.LocalDbContext>();
            ushort? expected = default; // DEFERRED: Set invalid value
            Local.AudioPropertySet target = new() { StreamNumber = expected };
            EntityEntry<Local.AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.StreamNumber), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.StreamNumber);

            expected = default; // DEFERRED: Set valid value
            target.StreamNumber = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.StreamNumber);

            expected = default; // DEFERRED: Set invalid value
            target.StreamNumber = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.AudioPropertySet.StreamNumber), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.AudioPropertySets.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.StreamNumber);
        }
    }
}
