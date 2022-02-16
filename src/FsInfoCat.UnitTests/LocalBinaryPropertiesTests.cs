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

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalBinaryPropertiesTests
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

        [TestMethod("BinaryPropertySet Constructor Tests")]
        public void BinaryPropertiesConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            BinaryPropertySet target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.Hash);
            Assert.AreEqual(0L, target.Length);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            Assert.IsNotNull(target.RedundantSets);
            Assert.AreEqual(0, target.RedundantSets.Count);
        }

        [TestMethod]
        public void EqualsTestMethod1()
        {
            BinaryPropertySet target = new();
            BinaryPropertySet other = null;
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
            MD5Hash hash1 = new MD5Hash(Convert.FromBase64String("JHKSK4I8aid2KAi0+satWg=="));
            MD5Hash hash2 = new MD5Hash(Convert.FromBase64String("lJ+PRmeVDep5IoN7Wn6/6g=="));
            BinaryPropertySet target = new() { Id = id1 };
            yield return new object[] { target, target, true };
            yield return new object[] { target, null, false };
            target = new();
            yield return new object[] { target, target, true };
            yield return new object[] { target, new BinaryPropertySet() { Id = id1 }, false };
            yield return new object[] { target, null, false };
            (BinaryPropertySet, BinaryPropertySet)[] getEqualPropertyItems() => new (BinaryPropertySet, BinaryPropertySet)[]
            {
                (new BinaryPropertySet(), new BinaryPropertySet()),
                (new BinaryPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }, new BinaryPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }),
                (new BinaryPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }, new BinaryPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new BinaryPropertySet() { Hash = hash1 }, new BinaryPropertySet() { Hash = hash1 }),
                (new BinaryPropertySet() { Length = 1L }, new BinaryPropertySet() { Length = 1L })
            };
            foreach ((BinaryPropertySet t, BinaryPropertySet other) in getEqualPropertyItems())
                yield return new object[] { t, other, true };
            foreach ((BinaryPropertySet t, BinaryPropertySet other) in getEqualPropertyItems())
            {
                t.Id = id1;
                yield return new object[] { t, other, false };
            }
            foreach ((BinaryPropertySet t, BinaryPropertySet other) in getEqualPropertyItems())
            {
                other.Id = id2;
                yield return new object[] { t, other, false };
            }
            foreach ((BinaryPropertySet t, BinaryPropertySet other) in getEqualPropertyItems())
            {
                t.Id = id1;
                other.Id = id2;
                yield return new object[] { t, other, false };
            }
            (BinaryPropertySet, BinaryPropertySet)[] getDifferingPropertyItems() => new (BinaryPropertySet, BinaryPropertySet)[]
            {
                (new BinaryPropertySet(), new BinaryPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }),
                (new BinaryPropertySet() { CreatedOn = plus1, ModifiedOn = plus1 }, new BinaryPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }),
                (new BinaryPropertySet() { CreatedOn = createdOn, ModifiedOn = plus1 }, new BinaryPropertySet() { CreatedOn = plus1, ModifiedOn = plus1 }),
                (new BinaryPropertySet() { CreatedOn = createdOn, ModifiedOn = plus2 }, new BinaryPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id1, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new BinaryPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id1, CreatedOn = createdOn, ModifiedOn = plus2 }, new BinaryPropertySet() { CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new BinaryPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id1, CreatedOn = createdOn, ModifiedOn = plus2 }, new BinaryPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new BinaryPropertySet() { LastSynchronizedOn = plus2, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }, new BinaryPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new BinaryPropertySet() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }, new BinaryPropertySet() { LastSynchronizedOn = plus2, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2 }),
                (new BinaryPropertySet() { Hash = hash1 }, new BinaryPropertySet() { Hash = hash2 }),
                (new BinaryPropertySet() { Hash = hash1 }, new BinaryPropertySet()),
                (new BinaryPropertySet(), new BinaryPropertySet() { Hash = hash2 }),
                (new BinaryPropertySet(), new BinaryPropertySet() { Length = 1L }),
                (new BinaryPropertySet() { Length = 1L }, new BinaryPropertySet()),
                (new BinaryPropertySet() { Length = 1L }, new BinaryPropertySet() { Length = 2L })
            };
            foreach ((BinaryPropertySet t, BinaryPropertySet other) in getDifferingPropertyItems())
                yield return new object[] { t, other, false };
            foreach ((BinaryPropertySet t, BinaryPropertySet other) in getDifferingPropertyItems())
            {
                t.Id = other.Id = id1;
                yield return new object[] { t, other, true };
            }
        }

        [TestMethod]
        [DynamicData("GetEqualsTestData", DynamicDataSourceType.Method)]
        public void EqualsTestMethod2(BinaryPropertySet target, BinaryPropertySet other, bool expectedResult)
        {
            bool actualResult = target.Equals(other);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod("Guid Id")]
        public void IdTestMethod()
        {
            BinaryPropertySet target = new();
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
        [DataRow("68756572e69744609fb70de000baf4d3", "2/14/2022 10:30:21 PM", "2/14/2022 10:30:21 PM", "2/14/2022 10:30:21 PM")]
        [DataRow("f51d7039b01b4828853d3b7f1d3e9e95", "2/14/2022 10:31:04 PM", "2/14/2022 10:30:21 PM", "2/14/2022 10:31:04 PM")]
        [DataRow("", "", "2/14/2022 10:30:21 PM", "2/14/2022 10:31:04 PM")]
        [DataRow("f51d7039b01b4828853d3b7f1d3e9e95", "", "2/14/2022 10:30:21 PM", "2/14/2022 10:31:04 PM")]
        [DataRow("", "2/14/2022 10:31:04 PM", "2/14/2022 10:30:21 PM", "2/14/2022 10:31:04 PM")]
        [DataRow("9b59cbab78d947189d7e134b8a8a38dd", "2/15/2022 2:07:04 AM", "2/14/2022 10:30:21 PM", "2/14/2022 10:31:04 PM")]
        [DataRow("649abdaa012848c1a617f329b0e2d129", "2/14/2022 10:31:04 PM", "2/14/2022 10:30:21 PM", "2/15/2022 2:07:04 AM")]
        [DataRow("", "", "2/14/2022 10:30:21 PM", "2/15/2022 2:07:04 AM")]
        public void UpstreamIdAndDatePropertiesTestMethod(string upstreamId, string lastSynchronizedOn, string createdOn, string modifiedOn)
        {
            Guid? expectedUpstreamId = string.IsNullOrWhiteSpace(upstreamId) ? null : Guid.Parse(upstreamId);
            DateTime? expectedLastSynchronizedOn = string.IsNullOrWhiteSpace(lastSynchronizedOn) ? null : DateTime.Parse(lastSynchronizedOn);
            DateTime expectedCreatedOn = DateTime.Parse(createdOn);
            DateTime expectedModifiedOn = DateTime.Parse(modifiedOn);
            BinaryPropertySet target = new BinaryPropertySet
            {
                UpstreamId = expectedUpstreamId,
                LastSynchronizedOn = expectedLastSynchronizedOn,
                CreatedOn = expectedCreatedOn,
                ModifiedOn = expectedModifiedOn
            };
            Assert.AreEqual(expectedUpstreamId, target.UpstreamId);
            Assert.AreEqual(expectedLastSynchronizedOn, target.LastSynchronizedOn);
            Assert.AreEqual(expectedCreatedOn, target.CreatedOn);
            Assert.AreEqual(expectedModifiedOn, target.ModifiedOn);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            if (expectedCreatedOn > expectedModifiedOn)
            {
                Assert.IsFalse(success);
                Assert.AreEqual(1, results.Count);
                Assert.AreEqual(1, results[0].MemberNames.Count());
                Assert.AreEqual(nameof(BinaryPropertySet.LastSynchronizedOn), results[0].MemberNames.First());
                Assert.AreEqual("\"Modified On\" date cannot be earlier than the \"Created On\" date.", results[0].ErrorMessage);
            }
            else if (expectedUpstreamId.HasValue)
            {
                if (expectedLastSynchronizedOn.HasValue)
                {
                    if (expectedCreatedOn > expectedLastSynchronizedOn.Value)
                    {
                        Assert.IsFalse(success);
                        Assert.AreEqual(1, results.Count);
                        Assert.AreEqual(1, results[0].MemberNames.Count());
                        Assert.AreEqual(nameof(BinaryPropertySet.LastSynchronizedOn), results[0].MemberNames.First());
                        Assert.AreEqual("\"Last Synchronized On\" date cannot be earlier than the \"Created On\" date.", results[0].ErrorMessage);
                    }
                    else if (expectedModifiedOn < expectedLastSynchronizedOn.Value)
                    {
                        Assert.IsFalse(success);
                        Assert.AreEqual(1, results.Count);
                        Assert.AreEqual(1, results[0].MemberNames.Count());
                        Assert.AreEqual(nameof(BinaryPropertySet.LastSynchronizedOn), results[0].MemberNames.First());
                        Assert.AreEqual("\"Last Synchronized On\" date cannot be later than the \"Modified On\" date.", results[0].ErrorMessage);
                    }
                    else
                    {
                        Assert.IsTrue(success);
                        Assert.AreEqual(0, results.Count);
                    }
                }
                else
                {
                    Assert.IsFalse(success);
                    Assert.AreEqual(1, results.Count);
                    Assert.AreEqual(1, results[0].MemberNames.Count());
                    Assert.AreEqual(nameof(BinaryPropertySet.LastSynchronizedOn), results[0].MemberNames.First());
                    Assert.AreEqual("\"Last Synchronized On\" date is required when \"Upstream Id\" is specified.", results[0].ErrorMessage);
                }
            }
            else if (expectedLastSynchronizedOn.HasValue)
            {
                Assert.IsFalse(success);
                Assert.AreEqual(1, results.Count);
                Assert.AreEqual(1, results[0].MemberNames.Count());
                Assert.AreEqual(nameof(BinaryPropertySet.UpstreamId), results[0].MemberNames.First());
                Assert.AreEqual("\"Upstream Id\" is required when \"Last Synchronized On\" date is specified.", results[0].ErrorMessage);
            }
            else
            {
                Assert.IsTrue(success);
                Assert.AreEqual(0, results.Count);
            }
        }

        [DataTestMethod]
        [Description("BinaryProperties.Length: BIGINT NOT NULL CHECK(Length>=0)")]
        [DataRow(-1L)]
        [DataRow(0L)]
        [DataRow(2147483647L)]
        public void LengthTestMethod(long expected)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            while (dbContext.BinaryPropertySets.FirstOrDefault(e => e.Length == expected && e.Hash == null) is not null)
            {
                if (expected == long.MaxValue)
                    Assert.Inconclusive("Cannot find unique length");
                expected++;
            }
            BinaryPropertySet target = new BinaryPropertySet { Length = expected };
            Assert.AreEqual(expected, target.Length);
            EntityEntry<BinaryPropertySet> entityEntry = dbContext.BinaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            if (expected < 0L)
            {
                Assert.IsFalse(success);
                Assert.AreEqual(1, results.Count);
                Assert.AreEqual(1, results[0].MemberNames.Count());
                Assert.AreEqual(nameof(BinaryPropertySet.Length), results[0].MemberNames.First());
                Assert.AreEqual("File Length cannot be negative.", results[0].ErrorMessage);
                Assert.ThrowsException<AggregateException>(() => dbContext.SaveChanges());
                Assert.AreEqual(EntityState.Added, entityEntry.State);
            }
            else
            {
                Assert.IsTrue(success);
                Assert.AreEqual(0, results.Count);
                dbContext.SaveChanges();
                Assert.AreEqual(expected, target.Length);
                dbContext.BinaryPropertySets.Remove(target);
                dbContext.SaveChanges();
                Assert.AreEqual(EntityState.Detached, entityEntry.State);
            }
        }

        [DataTestMethod]
        [Description("BinaryProperties.Hash: BINARY(16) CHECK(Hash IS NULL OR length(HASH)=16) DEFAULT NULL")]
        [DataRow(1L, "QjzCr+4ZofV18BhXFu9eLQ==")]
        [DataRow(2147483647L, "I9noB4mkYzqVHkrwz3XFIw==")]
        public void HashTestMethod(long length, string expected)
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            MD5Hash hash = new MD5Hash(Convert.FromBase64String(expected));
            while (dbContext.BinaryPropertySets.FirstOrDefault(e => e.Length == length && e.Hash == hash) is not null)
            {
                if (length == long.MaxValue)
                    Assert.Inconclusive("Cannot find unique length");
                length++;
            }
            BinaryPropertySet target = new() { Hash = hash, Length = length };
            Assert.IsNotNull(target.Hash);
            Assert.AreEqual(hash, target.Hash);
            EntityEntry<BinaryPropertySet> entityEntry = dbContext.BinaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.IsNotNull(target.Hash);
            Assert.AreEqual(hash, target.Hash);
            dbContext.BinaryPropertySets.Remove(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("HashSet<DbFile> Files"), Ignore]
        public void FilesTestMethod()
        {
            // DEFERRED: Implement test for HashSet<DbFile> Files

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            HashSet<DbFile> expectedValue = default;
            target.Files = default;
            HashSet<DbFile> actualValue = target.Files;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<RedundantSet> RedundantSets"), Ignore]
        public void RedundantSetsTestMethod()
        {
            // DEFERRED: Implement test for HashSet<RedundantSet> RedundantSets

            BinaryPropertySet target = default; // TODO: Create and initialize BinaryProperties instance
            HashSet<RedundantSet> expectedValue = default;
            target.RedundantSets = default;
            HashSet<RedundantSet> actualValue = target.RedundantSets;
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
