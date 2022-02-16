using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

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
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            TestHelper.UndoChanges(dbContext);
        }

        [TestMethod("AudioPropertySet Constructor Tests")]
        public void AudioPropertySetConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            AudioPropertySet target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.AreEqual(string.Empty, target.Compression);
            Assert.IsNull(target.EncodingBitrate);
            Assert.AreEqual(string.Empty, target.Format);
            Assert.IsNull(target.IsVariableBitrate);
            Assert.IsNull(target.SampleRate);
            Assert.IsNull(target.SampleSize);
            Assert.AreEqual(string.Empty, target.StreamName);
            Assert.IsNull(target.StreamNumber);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
        }

        [TestMethod]
        public void EqualsTestMethod1()
        {
            AudioPropertySet target = new();
            AudioPropertySet other = null;
            Assert.IsFalse(target.Equals(other));
            other = new();
            Assert.AreEqual(target.CreatedOn.Equals(other.CreatedOn), target.Equals(other));
        }

        private static IEnumerable<object[]> GetEqualsTestData()
        {
            DateTime createdOn = DateTime.Now;
            DateTime plus1 = DateTime.Now.AddMilliseconds(1.0);
            DateTime plus2 = plus1.AddMilliseconds(2.0);
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            AudioPropertySet target = new() { Id = id1 };
            yield return new object[] { target, target, true };
            yield return new object[] { target, null, false };
            target = new();
            yield return new object[] { target, target, true };
            yield return new object[] { target, new AudioPropertySet() { Id = id1 }, false };
            yield return new object[] { target, null, false };
            (AudioPropertySet, AudioPropertySet)[] getEqualPropertyItems() => new (AudioPropertySet, AudioPropertySet)[]
            {
                (new AudioPropertySet(), new AudioPropertySet()),
                (new AudioPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }, new AudioPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }),
                (new AudioPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }, new AudioPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new AudioPropertySet() { Compression = "Test" }, new AudioPropertySet() { Compression = "Test" }),
                (new AudioPropertySet() { EncodingBitrate = 0u }, new AudioPropertySet() { EncodingBitrate = 0u }),
                (new AudioPropertySet() { Format = "Test" }, new AudioPropertySet() { Format = "Test" }),
                (new AudioPropertySet() { IsVariableBitrate = true }, new AudioPropertySet() { IsVariableBitrate = true }),
                (new AudioPropertySet() { SampleRate = 0u }, new AudioPropertySet() { SampleRate = 0u }),
                (new AudioPropertySet() { SampleSize = 0u }, new AudioPropertySet() { SampleSize = 0u }),
                (new AudioPropertySet() { StreamName = "Test" }, new AudioPropertySet() { StreamName = "Test" }),
                ( new AudioPropertySet() { StreamNumber = 0 }, new AudioPropertySet() { StreamNumber = 0 })
            };
            foreach ((AudioPropertySet t, AudioPropertySet other) in getEqualPropertyItems())
                yield return new object[] { t, other, true };
            foreach ((AudioPropertySet t, AudioPropertySet other) in getEqualPropertyItems())
            {
                t.Id = id1;
                yield return new object[] { t, other, false };
            }
            foreach ((AudioPropertySet t, AudioPropertySet other) in getEqualPropertyItems())
            {
                other.Id = id2;
                yield return new object[] { t, other, false };
            }
            foreach ((AudioPropertySet t, AudioPropertySet other) in getEqualPropertyItems())
            {
                t.Id = id1;
                other.Id = id2;
                yield return new object[] { t, other, false };
            }
            (AudioPropertySet, AudioPropertySet)[] getDifferingPropertyItems() => new (AudioPropertySet, AudioPropertySet)[]
            {
                (new AudioPropertySet(), new AudioPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }),
                (new AudioPropertySet() { CreatedOn = plus1, ModifiedOn = plus1 }, new AudioPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }),
                (new AudioPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }, new AudioPropertySet() { CreatedOn = plus1, ModifiedOn = plus1 }),
                (new AudioPropertySet() { CreatedOn = createdOn, ModifiedOn = plus2 }, new AudioPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id1, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new AudioPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id1, CreatedOn = createdOn, ModifiedOn = plus2 }, new AudioPropertySet() { CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new AudioPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id1, CreatedOn = createdOn, ModifiedOn = plus2 }, new AudioPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new AudioPropertySet() { LastSynchronizedOn = plus2, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }, new AudioPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new AudioPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }, new AudioPropertySet() { LastSynchronizedOn = plus2, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new AudioPropertySet() { Compression = "Test2" }, new AudioPropertySet() { Compression = "Test" }),
                (new AudioPropertySet() { Compression = "Test" }, new AudioPropertySet()),
                (new AudioPropertySet(), new AudioPropertySet() { Compression = "Test" }),
                (new AudioPropertySet(), new AudioPropertySet() { EncodingBitrate = 0u }),
                (new AudioPropertySet() { EncodingBitrate = 0u }, new AudioPropertySet()),
                (new AudioPropertySet() { EncodingBitrate = 0u }, new AudioPropertySet() { EncodingBitrate = 1u }),
                (new AudioPropertySet() { Format = "Test2" }, new AudioPropertySet() { Format = "Test" }),
                (new AudioPropertySet() { Format = "Test" }, new AudioPropertySet()),
                (new AudioPropertySet(), new AudioPropertySet() { Format = "Test" }),
                (new AudioPropertySet(), new AudioPropertySet() { IsVariableBitrate = true }),
                (new AudioPropertySet() { IsVariableBitrate = true }, new AudioPropertySet()),
                (new AudioPropertySet(), new AudioPropertySet() { SampleRate = 0u }),
                (new AudioPropertySet() { SampleRate = 0u }, new AudioPropertySet()),
                (new AudioPropertySet() { SampleRate = 0u }, new AudioPropertySet() { SampleRate = 0u }),
                (new AudioPropertySet() { SampleSize = 0u }, new AudioPropertySet() { SampleSize = 1u }),
                (new AudioPropertySet(), new AudioPropertySet() { StreamName = "Test" }),
                (new AudioPropertySet() { StreamName = "Test" }, new AudioPropertySet()),
                (new AudioPropertySet() { StreamName = "Test" }, new AudioPropertySet() { StreamName = "Test2" }),
                (new AudioPropertySet(), new AudioPropertySet() { StreamNumber = 0 }),
                (new AudioPropertySet() { StreamNumber = 0 }, new AudioPropertySet()),
                (new AudioPropertySet() { StreamNumber = 0 }, new AudioPropertySet() { StreamNumber = 1 })
            };
            foreach ((AudioPropertySet t, AudioPropertySet other) in getDifferingPropertyItems())
                yield return new object[] { t, other, false };
            foreach ((AudioPropertySet t, AudioPropertySet other) in getDifferingPropertyItems())
            {
                t.Id = other.Id = id1;
                yield return new object[] { t, other, true };
            }
        }

        [TestMethod]
        [DynamicData("GetEqualsTestData", DynamicDataSourceType.Method)]
        public void EqualsTestMethod2(AudioPropertySet target, AudioPropertySet other, bool expectedResult)
        {
            bool actualResult = target.Equals(other);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod("AudioPropertySet Add/Remove Tests")]
        public void AudioPropertySetAddRemoveTestMethod()
        {
            AudioPropertySet target = new();
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            EntityEntry<AudioPropertySet> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.AudioPropertySets.Add(target);
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            DateTime beforeSave = DateTime.Now;
            dbContext.SaveChanges();
            DateTime afterSave = DateTime.Now;
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            Assert.IsTrue(target.CreatedOn >= beforeSave);
            Assert.IsTrue(target.CreatedOn <= afterSave);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.AreEqual(string.Empty, target.Compression);
            Assert.IsNull(target.EncodingBitrate);
            Assert.AreEqual(string.Empty, target.Format);
            Assert.IsNull(target.IsVariableBitrate);
            Assert.IsNull(target.SampleRate);
            Assert.IsNull(target.SampleSize);
            Assert.AreEqual(string.Empty, target.StreamName);
            Assert.IsNull(target.StreamNumber);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("Guid Id")]
        public void AudioPropertySetIdTestMethod()
        {
            AudioPropertySet target = new();
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
        [Description("AudioPropertySet.Compression: NVARCHAR(256)")]
        [DataRow(null, "", true)]
        [DataRow("", "", true)]
        [DataRow(" ", "", true)]
        [DataRow(" \t", "", true)]
        [DataRow("\n\r", "", true)]
        [DataRow("Test", "Test", true)]
        [DataRow("\n Test \r", "Test", true)]
        [DataRow("Test Data", "Test Data", true)]
        [DataRow("Test\tData", "Test Data", true)]
        [DataRow(" Test Data ", "Test Data", true)]
        [DataRow(" Test  Data ", "Test Data", true)]
        [DataRow("\r\n Test \n Data \t", "Test Data", false)]
        public void AudioPropertySetCompressionTestMethod(string compression, string expected, bool isValid)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            AudioPropertySet target = new() { Compression = isValid ? compression : $"{compression} {new string('_', 256)}" };
            Assert.AreEqual(isValid ? expected : $"{expected} {new string('_', 256)}", target.Compression);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.AreEqual(isValid, success);
            EntityEntry<AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            if (isValid)
            {
                Assert.AreEqual(0, results.Count);
                dbContext.SaveChanges();
                Assert.AreEqual(isValid ? expected : $"{expected} {new string('_', 256)}", target.Compression);
                dbContext.AudioPropertySets.Remove(target);
                dbContext.SaveChanges();
                Assert.AreEqual(EntityState.Detached, entityEntry.State);
            }
            else
            {
                Assert.AreEqual(1, results.Count);
                Assert.AreEqual(1, results[0].MemberNames.Count());
                Assert.AreEqual(nameof(AudioPropertySet.Compression), results[0].MemberNames.First());
                Assert.AreEqual($"{nameof(AudioPropertySet.Compression)} text is too long.", results[0].ErrorMessage);
                Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
                Assert.AreEqual(EntityState.Added, entityEntry.State);
                Assert.AreEqual(isValid ? expected : $"{expected} {new string('_', 256)}", target.Compression);
            }
        }

        [DataTestMethod]
        [TestProperty(TestHelper.TestProperty_Description, "AudioPropertySet.EncodingBitrate: BIGINT \"EncodingBitrate\" IS NULL OR (\"EncodingBitrate\">=0 AND \"EncodingBitrate\"<4294967296)")]
        [DataRow(null)]
        [DataRow(0u)]
        [DataRow(256u)]
        [DataRow(4294967295u)]
        public void AudioPropertySetEncodingBitrateTestMethod(uint? expected)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            AudioPropertySet target = new() { EncodingBitrate = expected };
            Assert.AreEqual(expected, target.EncodingBitrate);
            EntityEntry<AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(expected, target.EncodingBitrate);
            dbContext.AudioPropertySets.Remove(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [DataTestMethod]
        [Description("AudioPropertySet.Format: NVARCHAR(256)")]
        [DataRow(null, "", true)]
        [DataRow("", "", true)]
        [DataRow(" ", "", true)]
        [DataRow(" \t", "", true)]
        [DataRow("\n\r", "", true)]
        [DataRow("Test", "Test", true)]
        [DataRow("\n Test \r", "Test", true)]
        [DataRow("Test Data", "Test Data", true)]
        [DataRow("Test\tData", "Test Data", true)]
        [DataRow(" Test Data ", "Test Data", true)]
        [DataRow(" Test  Data ", "Test Data", true)]
        [DataRow("\r\n Test \n Data \t", "Test Data", false)]
        public void AudioPropertySetFormatTestMethod(string format, string expected, bool isValid)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            AudioPropertySet target = new() { Format = isValid ? format : $"{format} {new string('_', 256)}" };
            Assert.AreEqual(isValid ? expected : $"{expected} {new string('_', 256)}", target.Format);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.AreEqual(isValid, success);
            EntityEntry<AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            if (isValid)
            {
                Assert.AreEqual(0, results.Count);
                dbContext.SaveChanges();
                Assert.AreEqual(isValid ? expected : $"{expected} {new string('_', 256)}", target.Format);
                dbContext.AudioPropertySets.Remove(target);
                dbContext.SaveChanges();
                Assert.AreEqual(EntityState.Detached, entityEntry.State);
            }
            else
            {
                Assert.AreEqual(1, results.Count);
                Assert.AreEqual(1, results[0].MemberNames.Count());
                Assert.AreEqual(nameof(AudioPropertySet.Format), results[0].MemberNames.First());
                Assert.AreEqual($"{nameof(AudioPropertySet.Format)} text is too long.", results[0].ErrorMessage);
                Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
                Assert.AreEqual(EntityState.Added, entityEntry.State);
                Assert.AreEqual(isValid ? expected : $"{expected} {new string('_', 256)}", target.Format);
            }
        }

        [DataTestMethod]
        [Description("AudioPropertySet.IsVariableBitrate: BIT")]
        [DataRow(null)]
        [DataRow(true)]
        [DataRow(false)]
        public void AudioPropertySetIsVariableBitrateTestMethod(bool? expected)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            AudioPropertySet target = new() { IsVariableBitrate = expected };
            Assert.AreEqual(expected, target.IsVariableBitrate);
            EntityEntry<AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count); ;
            dbContext.SaveChanges();
            Assert.AreEqual(expected, target.IsVariableBitrate);
            dbContext.AudioPropertySets.Remove(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [DataTestMethod]
        [TestProperty(TestHelper.TestProperty_Description, "AudioPropertySet.SampleRate: BIGINT \"SampleRate\" IS NULL OR (\"SampleRate\">=0 AND \"SampleRate\"<4294967296)")]
        [DataRow(null)]
        [DataRow(0u)]
        [DataRow(256u)]
        [DataRow(4294967295u)]
        public void AudioPropertySetSampleRateTestMethod(uint? expected)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            AudioPropertySet target = new() { SampleRate = expected };
            Assert.AreEqual(expected, target.SampleRate);
            EntityEntry<AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(expected, target.SampleRate);
            dbContext.AudioPropertySets.Remove(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [DataTestMethod]
        [TestProperty(TestHelper.TestProperty_Description, "AudioPropertySet.SampleSize: BIGINT \"SampleSize\" IS NULL OR (\"SampleSize\">=0 AND \"SampleSize\"<4294967296)")]
        [DataRow(null)]
        [DataRow(0u)]
        [DataRow(256u)]
        [DataRow(4294967295u)]
        public void AudioPropertySetSampleSizeTestMethod(uint? expected)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            AudioPropertySet target = new() { SampleSize = expected };
            Assert.AreEqual(expected, target.SampleSize);
            EntityEntry<AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(expected, target.SampleSize);
            dbContext.AudioPropertySets.Remove(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [DataTestMethod]
        [Description("AudioPropertySet.StreamName: NVARCHAR(256)")]
        [DataRow(null, "", true)]
        [DataRow("", "", true)]
        [DataRow(" ", "", true)]
        [DataRow(" \t", "", true)]
        [DataRow("\n\r", "", true)]
        [DataRow("Test", "Test", true)]
        [DataRow("\n Test \r", "Test", true)]
        [DataRow("Test Data", "Test Data", true)]
        [DataRow("Test\tData", "Test Data", true)]
        [DataRow(" Test Data ", "Test Data", true)]
        [DataRow(" Test  Data ", "Test Data", true)]
        [DataRow("\r\n Test \n Data \t", "Test Data", false)]
        public void AudioPropertySetStreamNameTestMethod(string streamName, string expected, bool isValid)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            AudioPropertySet target = new() { StreamName = isValid ? streamName : $"{streamName} {new string('_', 256)}" };
            Assert.AreEqual(isValid ? expected : $"{expected} {new string('_', 256)}", target.StreamName);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.AreEqual(isValid, success);
            EntityEntry<AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            if (isValid)
            {
                Assert.AreEqual(0, results.Count);
                dbContext.SaveChanges();
                Assert.AreEqual(isValid ? expected : $"{expected} {new string('_', 256)}", target.StreamName);
                dbContext.AudioPropertySets.Remove(target);
                dbContext.SaveChanges();
                Assert.AreEqual(EntityState.Detached, entityEntry.State);
            }
            else
            {
                Assert.AreEqual(1, results.Count);
                Assert.AreEqual(1, results[0].MemberNames.Count());
                Assert.AreEqual(nameof(AudioPropertySet.StreamName), results[0].MemberNames.First());
                Assert.AreEqual($"{nameof(AudioPropertySet.StreamName)} text is too long.", results[0].ErrorMessage);
                Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
                Assert.AreEqual(EntityState.Added, entityEntry.State);
                Assert.AreEqual(isValid ? expected : $"{expected} {new string('_', 256)}", target.StreamName);
            }
        }

        [DataTestMethod]
        [TestProperty(TestHelper.TestProperty_Description, "AudioPropertySet.StreamNumber: INT \"StreamNumber\" IS NULL OR (\"StreamNumber\">=0 AND \"StreamNumber\"<65536)")]
        [DataRow(null)]
        [DataRow((ushort)0)]
        [DataRow((ushort)256)]
        [DataRow((ushort)65535)]
        public void AudioPropertySetStreamNumberTestMethod(ushort? expected)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            AudioPropertySet target = new() { StreamNumber = expected };
            Assert.AreEqual(expected, target.StreamNumber);
            EntityEntry<AudioPropertySet> entityEntry = dbContext.AudioPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(expected, target.StreamNumber);
            dbContext.AudioPropertySets.Remove(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }
    }
}
