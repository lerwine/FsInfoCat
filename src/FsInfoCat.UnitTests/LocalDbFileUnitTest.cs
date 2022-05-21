using FsInfoCat.Local.Model;
using FsInfoCat.Model;
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
    public class LocalDbFileUnitTest
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

        [TestMethod("DbFile Constructor Tests")]
        public void DbFileConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            DbFile target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.CreationTime);
            Assert.AreEqual(target.CreatedOn, target.LastWriteTime);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.AreEqual(string.Empty, target.Name);
            Assert.AreEqual(string.Empty, target.Notes);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.AreEqual(FileCorrelationStatus.Dissociated, target.Status);
            Assert.IsNull(target.Parent);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNotNull(target.BaselineComparisons);
            Assert.AreEqual(0, target.BaselineComparisons.Count);
            Assert.IsNotNull(target.CorrelativeComparisons);
            Assert.AreEqual(0, target.CorrelativeComparisons.Count);
            Assert.IsNotNull(target.PersonalTags);
            Assert.AreEqual(0, target.PersonalTags.Count);
            Assert.IsNotNull(target.SharedTags);
            Assert.AreEqual(0, target.SharedTags.Count);
            Assert.IsNull(target.AudioProperties);
            Assert.IsNull(target.AudioPropertySetId);
            Assert.IsNull(target.BinaryProperties);
            Assert.IsNull(target.BinaryPropertySetId);
            Assert.IsNull(target.DocumentProperties);
            Assert.IsNull(target.DocumentPropertySetId);
            Assert.IsNull(target.DRMProperties);
            Assert.IsNull(target.DRMPropertySetId);
            Assert.IsNull(target.GPSProperties);
            Assert.IsNull(target.GPSPropertySetId);
            Assert.IsNull(target.ImageProperties);
            Assert.IsNull(target.ImagePropertySetId);
            Assert.IsNull(target.LastHashCalculation);
            Assert.IsNull(target.MediaProperties);
            Assert.IsNull(target.MediaPropertySetId);
            Assert.IsNull(target.MusicProperties);
            Assert.IsNull(target.MusicPropertySetId);
            Assert.IsNull(target.PhotoProperties);
            Assert.IsNull(target.PhotoPropertySetId);
            Assert.IsNull(target.RecordedTVProperties);
            Assert.IsNull(target.RecordedTVPropertySetId);
            Assert.IsNull(target.Redundancy);
            Assert.IsNull(target.SummaryProperties);
            Assert.IsNull(target.SummaryPropertySetId);
            Assert.IsNull(target.VideoProperties);
            Assert.IsNull(target.VideoPropertySetId);
        }

        [TestMethod]
        public void EqualsTestMethod1()
        {
            DbFile target = new();
            DbFile other = null;
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
            Guid binaryPropertySetId = Guid.NewGuid();
            Guid parentId = Guid.NewGuid();
            DbFile target = new() { Id = id1, BinaryProperties = new(), Parent = new() };
            yield return new object[] { target, target, true };
            yield return new object[] { target, null, false };
            target = new() { BinaryProperties = new(), Parent = new() };
            yield return new object[] { target, target, true };
            yield return new object[] { target, new DbFile() { Id = id1 }, false };
            yield return new object[] { target, null, false };
            BinaryPropertySet binaryPropertiesA = new() { Id = binaryPropertySetId };
            BinaryPropertySet binaryPropertiesB = new() { Id = binaryPropertySetId };
            BinaryPropertySet binaryProperties2 = new() { Id = Guid.NewGuid() };
            Subdirectory parentA = new() { Id = parentId, Volume = new() { Id = Guid.NewGuid(), FileSystem = new() { Id = Guid.NewGuid() } } };
            parentA.Volume.RootDirectory = parentA;
            Subdirectory parentB = new() { Id = parentId, Volume = new() { Id = parentA.Volume.Id, FileSystem = new() { Id = parentA.Volume.FileSystemId } } };
            parentB.Volume.RootDirectory = parentB;
            Subdirectory parent2 = new() { Id = Guid.NewGuid(), Volume = new() { Id = Guid.NewGuid(), FileSystem = new() { Id = Guid.NewGuid() } } };
            parent2.Volume.RootDirectory = parent2;
            (DbFile, DbFile)[] getEqualPropertyItems() => new (DbFile, DbFile)[]
            {
                (new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { CreatedOn = createdOn, ModifiedOn = plus1, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { CreatedOn = createdOn, ModifiedOn = plus1, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA }, new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentB }),
                (new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA }, new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentB }),
                (new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA }, new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentB }),
                (new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA }, new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentB }),
                (new DbFile() { CreationTime = plus1, LastWriteTime = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { CreationTime = plus1, LastWriteTime = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastAccessed = plus2, LastWriteTime = plus1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { LastAccessed = plus2, LastWriteTime = plus1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastHashCalculation = plus1, LastAccessed = plus2, LastWriteTime = plus2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { LastHashCalculation = plus1, LastAccessed = plus2, LastWriteTime = plus2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentB }, new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Options = FileCrawlOptions.FlaggedForRescan, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { Options = FileCrawlOptions.FlaggedForRescan, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA }, new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesB  }),
                (new DbFile() { ParentId = parentId, BinaryProperties = binaryPropertiesA }, new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesB }),
                (new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA }, new DbFile() { ParentId = parentId, BinaryProperties = binaryPropertiesB }),
                (new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Status = FileCorrelationStatus.Correlated, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { Status = FileCorrelationStatus.Correlated, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB })
            };
            foreach ((DbFile t, DbFile other) in getEqualPropertyItems())
                yield return new object[] { t, other, true };
            foreach ((DbFile t, DbFile other) in getEqualPropertyItems())
            {
                t.Id = id1;
                yield return new object[] { t, other, false };
            }
            foreach ((DbFile t, DbFile other) in getEqualPropertyItems())
            {
                other.Id = id2;
                yield return new object[] { t, other, false };
            }
            foreach ((DbFile t, DbFile other) in getEqualPropertyItems())
            {
                t.Id = id1;
                other.Id = id2;
                yield return new object[] { t, other, false };
            }
            (DbFile, DbFile)[] getDifferingPropertyItems() => new (DbFile, DbFile)[]
            {
                (new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { CreatedOn = createdOn, ModifiedOn = plus1, BinaryProperties = binaryPropertiesA, Parent = parentA }),
                (new DbFile() { CreatedOn = plus1, ModifiedOn = plus1, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { CreatedOn = createdOn, ModifiedOn = plus1, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { CreatedOn = createdOn, ModifiedOn = plus1, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { CreatedOn = plus1, ModifiedOn = plus1, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastSynchronizedOn = plus2, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { LastSynchronizedOn = plus2, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { AudioProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { AudioProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { AudioPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { AudioPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { AudioPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA }, new DbFile() { BinaryProperties = new() { Id = Guid.NewGuid() }, Parent = parentB }),
                (new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA }, new DbFile() { BinaryProperties = new() { Id = Guid.NewGuid() }, Parent = parentB }),
                (new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA }, new DbFile() { BinaryPropertySetId = Guid.NewGuid(), Parent = parentB }),
                (new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA }, new DbFile() { BinaryPropertySetId = Guid.NewGuid(), Parent = parentB }),
                (new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA }, new DbFile() { Parent = parentB }),
                (new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA }, new DbFile() { Parent = parentB }),
                (new DbFile() { BinaryProperties = new() { Id = Guid.Empty }, Parent = parentA }, new DbFile() { Parent = parentB }),
                (new DbFile() { BinaryPropertySetId = Guid.Empty, Parent = parentA }, new DbFile() { Parent = parentB }),
                (new DbFile() { CreationTime = plus1, LastWriteTime = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { CreationTime = plus1, LastWriteTime = plus1, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { CreationTime = plus1, LastWriteTime = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { CreationTime = plus2, LastWriteTime = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DocumentProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DocumentProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DocumentPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DocumentPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DocumentPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DRMProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DRMProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DRMPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { DRMPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { DRMPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { GPSProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { GPSProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { GPSPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { GPSPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { GPSPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { ImageProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { ImageProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { ImagePropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { ImagePropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImageProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { ImagePropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastAccessed = plus2, LastWriteTime = plus1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { LastAccessed = plus1, LastWriteTime = plus1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastAccessed = plus2, LastWriteTime = plus1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { LastAccessed = plus2, LastWriteTime = plus2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { LastHashCalculation = plus1, LastAccessed = plus2, LastWriteTime = plus2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA },
                    new DbFile() { LastHashCalculation = plus2, LastAccessed = plus2, LastWriteTime = plus2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MediaProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MediaProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MediaPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MediaPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MediaPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MusicProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentB }, new DbFile() { MusicProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MusicPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { MusicPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { MusicPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { Name = "Test2", BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { Notes = "Test2", BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Options = FileCrawlOptions.FlaggedForRescan, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { Options = FileCrawlOptions.DoNotCompare, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA }, new DbFile() { Parent = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB  }),
                (new DbFile() { ParentId = parentId, BinaryProperties = binaryPropertiesA }, new DbFile() { Parent = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB }),
                (new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA }, new DbFile() { ParentId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB }),
                (new DbFile() { ParentId = parentId, BinaryProperties = binaryPropertiesA }, new DbFile() { BinaryProperties = binaryPropertiesB }),
                (new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA }, new DbFile() { BinaryProperties = binaryPropertiesB }),
                (new DbFile() { ParentId = Guid.Empty, BinaryProperties = binaryPropertiesA }, new DbFile() { BinaryProperties = binaryPropertiesB }),
                (new DbFile() { Parent = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA }, new DbFile() { BinaryProperties = binaryPropertiesB }),
                (new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { PhotoProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { PhotoProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { PhotoPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { PhotoPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { PhotoPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { RecordedTVProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { RecordedTVProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { RecordedTVPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { RecordedTVPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { RecordedTVPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { Status = FileCorrelationStatus.Correlated, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { SummaryProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { SummaryProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { SummaryPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { SummaryPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { SummaryPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { VideoProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { VideoProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { VideoPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { VideoPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB }),
                (new DbFile() { VideoPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA }, new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB })
            };
            foreach ((DbFile t, DbFile other) in getDifferingPropertyItems())
                yield return new object[] { t, other, false };
            foreach ((DbFile t, DbFile other) in getDifferingPropertyItems())
            {
                t.Id = other.Id = id1;
                yield return new object[] { t, other, true };
            }
        }

        [TestMethod]
        [DynamicData("GetEqualsTestData", DynamicDataSourceType.Method)]
        public void EqualsTestMethod2(DbFile target, DbFile other, bool expectedResult)
        {
            bool actualResult = target.Equals(other);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod("DbFile Add/Remove Tests"), Ignore]
        public void DbFileAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileSystem fileSystem1 = new() { DisplayName = "Subdirectory Add/Remove FileSystem" };
            dbContext.FileSystems.Add(fileSystem1);
            Volume volume1 = new()
            {
                DisplayName = "Subdirectory Add/Remove Item",
                VolumeName = "Subdirectory_Add_Remove_Name",
                Identifier = new(Guid.NewGuid()),
                FileSystem = fileSystem1
            };
            dbContext.Volumes.Add(volume1);
            string expectedName = "";
            Subdirectory parent1 = new() { Volume = volume1 };
            DbFile target = new() { /* DEFERRED: Initialize properties */ };
            EntityEntry<DbFile> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.Files.Add(target);
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
            Assert.IsNull(target.LastAccessed);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            Assert.IsNull(target.LastHashCalculation);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.AreEqual("", target.Notes);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("DbFile Content Validation Tests"), Ignore]
        [Description("DbFile.Content: UNIQUEIDENTIFIER FOREIGN REFERENCES BinaryProperties")]
        public void DbFileBinaryPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            BinaryPropertySet expected = default; // DEFERRED: Set invalid value
            DbFile target = new() { BinaryProperties = expected };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.BinaryProperties), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_BinaryPropertiesRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.BinaryProperties);

            expected = default; // DEFERRED: Set valid value
            target.BinaryProperties = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.BinaryProperties);

            expected = default; // DEFERRED: Set invalid value
            target.BinaryProperties = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.BinaryProperties), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_BinaryPropertiesRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.BinaryProperties);
            dbContext.Files.Remove(target);
        }

        [TestMethod("DbFile Name Validation Tests"), Ignore]
        [Description("DbFile.Name: NVARCHAR(1024) NOT NULL CHECK(length(trim(Name))>0) COLLATE NOCASE")]
        public void DbFileNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            DbFile target = new() { Name = expected };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Name);

            expected = default; // DEFERRED: Set valid value
            target.Name = expected;
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Name);

            expected = default; // DEFERRED: Set invalid value
            target.Name = expected;
            Assert.AreEqual(expected, target.Name);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Name), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Name);
            dbContext.Files.Remove(target);
        }

        [TestMethod("DbFile Parent Validation Tests"), Ignore]
        [Description("DbFile.Parent: UNIQUEIDENTIFIER NOT NULL FOREIGN REFERENCES Subdirectories")]
        public void DbFileParentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Subdirectory expected = default; // DEFERRED: Set invalid value
            DbFile target = new() { Parent = expected };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Parent);

            expected = default; // DEFERRED: Set valid value
            target.Parent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Parent);

            expected = default; // DEFERRED: Set invalid value
            target.Parent = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Parent), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_ParentRequired, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Parent);
            dbContext.Files.Remove(target);
        }

        [TestMethod("DbFile Options Validation Tests"), Ignore]
        [Description("DbFile.Options: TINYINT  NOT NULL CHECK(Options>=0 AND Options<15)")]
        public void DbFileOptionsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            FileCrawlOptions expected = default; // DEFERRED: Set invalid value
            DbFile target = new() { Options = expected };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Options), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileCrawlOption, results[0].ErrorMessage);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Options);

            expected = default; // DEFERRED: Set valid value
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Options);

            expected = default; // DEFERRED: Set invalid value
            target.Options = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.Options), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileCrawlOption, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Options);
        }

        [TestMethod("DbFile CreatedOn Validation Tests"), Ignore]
        [Description("DbFile.CreatedOn: CreatedOn<=ModifiedOn")]
        public void DbFileCreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new() {  /* DEFERRED: Initialize properties */ };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
            dbContext.SaveChanges();
            entityEntry.Reload();
            target.CreatedOn = target.ModifiedOn.AddSeconds(2);
            dbContext.Update(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(DbFile.CreatedOn), results[0].MemberNames.First());
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
            entityEntry = dbContext.Files.Update(target);
            dbContext.SaveChanges();
        }

        [TestMethod("DbFile LastSynchronizedOn Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description,
            "DbFile.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void DbFileLastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            DbFile target = new() {  /* DEFERRED: Initialize properties */ UpstreamId = Guid.NewGuid() };
            EntityEntry<DbFile> entityEntry = dbContext.Files.Add(target);
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
            entityEntry = dbContext.Files.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            entityEntry = dbContext.Files.Update(target);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);

            target.LastSynchronizedOn = target.CreatedOn.AddSeconds(-1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnBeforeCreatedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn.AddSeconds(1);
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(FileSystem.LastSynchronizedOn), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_LastSynchronizedOnAfterModifiedOn, results[0].ErrorMessage);
            entityEntry = dbContext.Files.Update(target);
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());

            target.LastSynchronizedOn = target.ModifiedOn;
            dbContext.SaveChanges();
        }
    }
}
