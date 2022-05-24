using FsInfoCat.Collections;
using FsInfoCat.Local.Model;
using FsInfoCat.Model;
using FsInfoCat.UnitTests.TestData;
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
            Assert.IsNull(target.BinaryProperties);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertySetId);
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
            #region Equal

            yield return new object[]
            {
                new DbFile()
                {
                    Name = TestFileData.Item1.Name, Status = TestFileData.Item1.Status, Options = TestFileData.Item1.Options, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, LastHashCalculation = TestFileData.Item1.LastHashCalculation,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id },
                    SummaryProperties = new() { Id = TestFileData.Item1.SummaryProperties.Id }, DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id },
                    AudioProperties = new() { Id = TestFileData.Item1.AudioProperties.Id }, DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id },
                    GPSProperties = new() { Id = TestFileData.Item1.GPSProperties.Id }, ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id },
                    MediaProperties = new() { Id = TestFileData.Item1.MediaProperties.Id }, PhotoProperties = new() { Id = TestFileData.Item1.PhotoProperties.Id },
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id },
                    VideoProperties = new() { Id = TestFileData.Item1.VideoProperties.Id }, Notes = TestFileData.Item1.Notes
                },
                new DbFile()
                {
                    Name = TestFileData.Item1.Name, Status = TestFileData.Item1.Status, Options = TestFileData.Item1.Options, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, LastHashCalculation = TestFileData.Item1.LastHashCalculation,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id },
                    SummaryProperties = new() { Id = TestFileData.Item1.SummaryProperties.Id }, DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id },
                    AudioProperties = new() { Id = TestFileData.Item1.AudioProperties.Id }, DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id },
                    GPSProperties = new() { Id = TestFileData.Item1.GPSProperties.Id }, ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id },
                    MediaProperties = new() { Id = TestFileData.Item1.MediaProperties.Id }, PhotoProperties = new() { Id = TestFileData.Item1.PhotoProperties.Id },
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id },
                    VideoProperties = new() { Id = TestFileData.Item1.VideoProperties.Id }, Notes = TestFileData.Item1.Notes
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Name = TestFileData.Item1.Name, Status = TestFileData.Item1.Status, Options = TestFileData.Item1.Options, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, LastHashCalculation = TestFileData.Item1.LastHashCalculation,
                    ParentId = TestFileData.Item1.ParentId, BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId,
                    SummaryPropertySetId = TestFileData.Item1.SummaryPropertySetId, DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId,
                    AudioPropertySetId = TestFileData.Item1.AudioPropertySetId, DRMPropertySetId = TestFileData.Item1.DRMPropertySetId,
                    GPSPropertySetId = TestFileData.Item1.GPSPropertySetId, ImagePropertySetId = TestFileData.Item1.ImagePropertySetId,
                    MediaPropertySetId = TestFileData.Item1.MediaPropertySetId, PhotoPropertySetId = TestFileData.Item1.PhotoPropertySetId,
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId,
                    VideoPropertySetId = TestFileData.Item1.VideoPropertySetId, Notes = TestFileData.Item1.Notes
                },
                new DbFile()
                {
                    Name = TestFileData.Item1.Name, Status = TestFileData.Item1.Status, Options = TestFileData.Item1.Options, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, LastHashCalculation = TestFileData.Item1.LastHashCalculation,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id },
                    SummaryProperties = new() { Id = TestFileData.Item1.SummaryProperties.Id }, DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id },
                    AudioProperties = new() { Id = TestFileData.Item1.AudioProperties.Id }, DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id },
                    GPSProperties = new() { Id = TestFileData.Item1.GPSProperties.Id }, ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id },
                    MediaProperties = new() { Id = TestFileData.Item1.MediaProperties.Id }, PhotoProperties = new() { Id = TestFileData.Item1.PhotoProperties.Id },
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id },
                    VideoProperties = new() { Id = TestFileData.Item1.VideoProperties.Id }, Notes = TestFileData.Item1.Notes
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Name = TestFileData.Item1.Name, Status = TestFileData.Item1.Status, Options = TestFileData.Item1.Options, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, LastHashCalculation = TestFileData.Item1.LastHashCalculation,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id },
                    SummaryProperties = new() { Id = TestFileData.Item1.SummaryProperties.Id }, DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id },
                    AudioProperties = new() { Id = TestFileData.Item1.AudioProperties.Id }, DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id },
                    GPSProperties = new() { Id = TestFileData.Item1.GPSProperties.Id }, ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id },
                    MediaProperties = new() { Id = TestFileData.Item1.MediaProperties.Id }, PhotoProperties = new() { Id = TestFileData.Item1.PhotoProperties.Id },
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id },
                    VideoProperties = new() { Id = TestFileData.Item1.VideoProperties.Id }, Notes = TestFileData.Item1.Notes
                },
                new DbFile()
                {
                    Name = TestFileData.Item1.Name, Status = TestFileData.Item1.Status, Options = TestFileData.Item1.Options, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, LastHashCalculation = TestFileData.Item1.LastHashCalculation,
                    ParentId = TestFileData.Item1.ParentId, BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId,
                    SummaryPropertySetId = TestFileData.Item1.SummaryPropertySetId, DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId,
                    AudioPropertySetId = TestFileData.Item1.AudioPropertySetId, DRMPropertySetId = TestFileData.Item1.DRMPropertySetId,
                    GPSPropertySetId = TestFileData.Item1.GPSPropertySetId, ImagePropertySetId = TestFileData.Item1.ImagePropertySetId,
                    MediaPropertySetId = TestFileData.Item1.MediaPropertySetId, PhotoPropertySetId = TestFileData.Item1.PhotoPropertySetId,
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId,
                    VideoPropertySetId = TestFileData.Item1.VideoPropertySetId, Notes = TestFileData.Item1.Notes
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Name = TestFileData.Item1.Name, Status = TestFileData.Item1.Status, Options = TestFileData.Item1.Options, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, LastHashCalculation = TestFileData.Item1.LastHashCalculation,
                    ParentId = TestFileData.Item1.ParentId, BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId,
                    SummaryPropertySetId = TestFileData.Item1.SummaryPropertySetId, DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId,
                    AudioPropertySetId = TestFileData.Item1.AudioPropertySetId, DRMPropertySetId = TestFileData.Item1.DRMPropertySetId,
                    GPSPropertySetId = TestFileData.Item1.GPSPropertySetId, ImagePropertySetId = TestFileData.Item1.ImagePropertySetId,
                    MediaPropertySetId = TestFileData.Item1.MediaPropertySetId, PhotoPropertySetId = TestFileData.Item1.PhotoPropertySetId,
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId,
                    VideoPropertySetId = TestFileData.Item1.VideoPropertySetId, Notes = TestFileData.Item1.Notes
                },
                new DbFile()
                {
                    Name = TestFileData.Item1.Name, Status = TestFileData.Item1.Status, Options = TestFileData.Item1.Options, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, LastHashCalculation = TestFileData.Item1.LastHashCalculation,
                    ParentId = TestFileData.Item1.ParentId, BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId,
                    SummaryPropertySetId = TestFileData.Item1.SummaryPropertySetId, DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId,
                    AudioPropertySetId = TestFileData.Item1.AudioPropertySetId, DRMPropertySetId = TestFileData.Item1.DRMPropertySetId,
                    GPSPropertySetId = TestFileData.Item1.GPSPropertySetId, ImagePropertySetId = TestFileData.Item1.ImagePropertySetId,
                    MediaPropertySetId = TestFileData.Item1.MediaPropertySetId, PhotoPropertySetId = TestFileData.Item1.PhotoPropertySetId,
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId,
                    VideoPropertySetId = TestFileData.Item1.VideoPropertySetId, Notes = TestFileData.Item1.Notes
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Notes = TestFileData.Item2.Notes, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    Notes = TestFileData.Item2.Notes, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    VideoProperties = new() { Id = TestFileData.Item2.VideoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    VideoProperties = new() { Id = TestFileData.Item2.VideoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    VideoPropertySetId = TestFileData.Item2.VideoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    VideoProperties = new() { Id = TestFileData.Item2.VideoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    VideoProperties = new() { Id = TestFileData.Item2.VideoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    VideoPropertySetId = TestFileData.Item2.VideoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    VideoPropertySetId = TestFileData.Item2.VideoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    VideoPropertySetId = TestFileData.Item2.VideoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    PhotoProperties = new() { Id = TestFileData.Item2.PhotoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    PhotoProperties = new() { Id = TestFileData.Item2.PhotoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    PhotoPropertySetId = TestFileData.Item2.PhotoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    PhotoProperties = new() { Id = TestFileData.Item2.PhotoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    PhotoProperties = new() { Id = TestFileData.Item2.PhotoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    PhotoPropertySetId = TestFileData.Item2.PhotoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    PhotoPropertySetId = TestFileData.Item2.PhotoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    PhotoPropertySetId = TestFileData.Item2.PhotoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    ImagePropertySetId = TestFileData.Item1.ImagePropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    ImagePropertySetId = TestFileData.Item1.ImagePropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    ImagePropertySetId = TestFileData.Item1.ImagePropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    ImagePropertySetId = TestFileData.Item1.ImagePropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MediaProperties = new() { Id = TestFileData.Item2.MediaProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MediaProperties = new() { Id = TestFileData.Item2.MediaProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MediaPropertySetId = TestFileData.Item2.MediaPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MediaProperties = new() { Id = TestFileData.Item2.MediaProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MediaProperties = new() { Id = TestFileData.Item2.MediaProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MediaPropertySetId = TestFileData.Item2.MediaPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MediaPropertySetId = TestFileData.Item2.MediaPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MediaPropertySetId = TestFileData.Item2.MediaPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DRMPropertySetId = TestFileData.Item1.DRMPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DRMPropertySetId = TestFileData.Item1.DRMPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DRMPropertySetId = TestFileData.Item1.DRMPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DRMPropertySetId = TestFileData.Item1.DRMPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    GPSProperties = new() { Id = TestFileData.Item2.GPSProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    GPSProperties = new() { Id = TestFileData.Item2.GPSProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    GPSPropertySetId = TestFileData.Item2.GPSPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    GPSProperties = new() { Id = TestFileData.Item2.GPSProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    GPSProperties = new() { Id = TestFileData.Item2.GPSProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    GPSPropertySetId = TestFileData.Item2.GPSPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    GPSPropertySetId = TestFileData.Item2.GPSPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    GPSPropertySetId = TestFileData.Item2.GPSPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    AudioProperties = new() { Id = TestFileData.Item2.AudioProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    AudioProperties = new() { Id = TestFileData.Item2.AudioProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    AudioPropertySetId = TestFileData.Item2.AudioPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    AudioProperties = new() { Id = TestFileData.Item2.AudioProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    AudioProperties = new() { Id = TestFileData.Item2.AudioProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    AudioPropertySetId = TestFileData.Item2.AudioPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    AudioPropertySetId = TestFileData.Item2.AudioPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    AudioPropertySetId = TestFileData.Item2.AudioPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    LastHashCalculation = TestFileData.Item1.LastHashCalculation, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    LastHashCalculation = TestFileData.Item1.LastHashCalculation, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    SummaryProperties = new() { Id = TestFileData.Item2.SummaryProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    SummaryProperties = new() { Id = TestFileData.Item2.SummaryProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    SummaryPropertySetId = TestFileData.Item2.SummaryPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    SummaryProperties = new() { Id = TestFileData.Item2.SummaryProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    SummaryProperties = new() { Id = TestFileData.Item2.SummaryProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    SummaryPropertySetId = TestFileData.Item2.SummaryPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    SummaryPropertySetId = TestFileData.Item2.SummaryPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    SummaryPropertySetId = TestFileData.Item2.SummaryPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Options = TestFileData.Item2.Options, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    Options = TestFileData.Item2.Options, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Status = TestFileData.Item1.Status, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, Parent = new() { Id = TestFileData.Item1.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    Status = TestFileData.Item1.Status, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, Parent = new() { Id = TestFileData.Item1.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Name = TestFileData.Item2.Name, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    Name = TestFileData.Item2.Name, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                true
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                true
            };

            #endregion

            #region Not Equal

            yield return new object[]
            {
                new DbFile()
                {
                    Notes = TestFileData.Item2.Notes, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    Notes = TestFileData.Item1.Notes, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    RecordedTVProperties = new() { Id = TestFileData.Item2.RecordedTVProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    RecordedTVProperties = new() { Id = TestFileData.Item2.RecordedTVProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    RecordedTVProperties = new() { Id = TestFileData.Item1.RecordedTVProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    RecordedTVPropertySetId = TestFileData.Item2.RecordedTVPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    RecordedTVPropertySetId = TestFileData.Item1.RecordedTVPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    RecordedTVPropertySetId = TestFileData.Item2.RecordedTVPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    VideoProperties = new() { Id = TestFileData.Item2.VideoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    VideoProperties = new() { Id = TestFileData.Item1.VideoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    VideoPropertySetId = TestFileData.Item2.VideoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    VideoProperties = new() { Id = TestFileData.Item1.VideoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    VideoProperties = new() { Id = TestFileData.Item2.VideoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    VideoPropertySetId = TestFileData.Item1.VideoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    VideoPropertySetId = TestFileData.Item2.VideoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    VideoPropertySetId = TestFileData.Item1.VideoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MusicProperties = new() { Id = TestFileData.Item2.MusicProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MusicProperties = new() { Id = TestFileData.Item2.MusicProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MusicProperties = new() { Id = TestFileData.Item1.MusicProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MusicPropertySetId = TestFileData.Item2.MusicPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MusicPropertySetId = TestFileData.Item1.MusicPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MusicPropertySetId = TestFileData.Item2.MusicPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    PhotoProperties = new() { Id = TestFileData.Item2.PhotoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    PhotoProperties = new() { Id = TestFileData.Item1.PhotoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    PhotoPropertySetId = TestFileData.Item2.PhotoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    PhotoProperties = new() { Id = TestFileData.Item1.PhotoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    PhotoProperties = new() { Id = TestFileData.Item2.PhotoProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    PhotoPropertySetId = TestFileData.Item1.PhotoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    PhotoPropertySetId = TestFileData.Item2.PhotoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    PhotoPropertySetId = TestFileData.Item1.PhotoPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    ImageProperties = new() { Id = TestFileData.Item2.ImageProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    ImagePropertySetId = TestFileData.Item1.ImagePropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    ImageProperties = new() { Id = TestFileData.Item2.ImageProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    ImageProperties = new() { Id = TestFileData.Item1.ImageProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    ImagePropertySetId = TestFileData.Item2.ImagePropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    ImagePropertySetId = TestFileData.Item1.ImagePropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    ImagePropertySetId = TestFileData.Item2.ImagePropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MediaProperties = new() { Id = TestFileData.Item2.MediaProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MediaProperties = new() { Id = TestFileData.Item1.MediaProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MediaPropertySetId = TestFileData.Item2.MediaPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MediaProperties = new() { Id = TestFileData.Item1.MediaProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MediaProperties = new() { Id = TestFileData.Item2.MediaProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MediaPropertySetId = TestFileData.Item1.MediaPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    MediaPropertySetId = TestFileData.Item2.MediaPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    MediaPropertySetId = TestFileData.Item1.MediaPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DRMProperties = new() { Id = TestFileData.Item2.DRMProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DRMPropertySetId = TestFileData.Item1.DRMPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DRMProperties = new() { Id = TestFileData.Item2.DRMProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DRMProperties = new() { Id = TestFileData.Item1.DRMProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DRMPropertySetId = TestFileData.Item2.DRMPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DRMPropertySetId = TestFileData.Item1.DRMPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DRMPropertySetId = TestFileData.Item2.DRMPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    GPSProperties = new() { Id = TestFileData.Item2.GPSProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    GPSProperties = new() { Id = TestFileData.Item1.GPSProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    GPSPropertySetId = TestFileData.Item2.GPSPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    GPSProperties = new() { Id = TestFileData.Item1.GPSProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    GPSProperties = new() { Id = TestFileData.Item2.GPSProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    GPSPropertySetId = TestFileData.Item1.GPSPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    GPSPropertySetId = TestFileData.Item2.GPSPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    GPSPropertySetId = TestFileData.Item1.GPSPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DocumentProperties = new() { Id = TestFileData.Item2.DocumentProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DocumentProperties = new() { Id = TestFileData.Item2.DocumentProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DocumentProperties = new() { Id = TestFileData.Item1.DocumentProperties.Id }, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DocumentPropertySetId = TestFileData.Item2.DocumentPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    DocumentPropertySetId = TestFileData.Item1.DocumentPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    DocumentPropertySetId = TestFileData.Item2.DocumentPropertySetId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    AudioProperties = new() { Id = TestFileData.Item2.AudioProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    AudioProperties = new() { Id = TestFileData.Item1.AudioProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    AudioPropertySetId = TestFileData.Item2.AudioPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    AudioProperties = new() { Id = TestFileData.Item1.AudioProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    AudioProperties = new() { Id = TestFileData.Item2.AudioProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    AudioPropertySetId = TestFileData.Item1.AudioPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    AudioPropertySetId = TestFileData.Item2.AudioPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    AudioPropertySetId = TestFileData.Item1.AudioPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    LastHashCalculation = TestFileData.Item1.LastHashCalculation, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    LastHashCalculation = TestFileData.Item2.LastHashCalculation, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    SummaryProperties = new() { Id = TestFileData.Item2.SummaryProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    SummaryProperties = new() { Id = TestFileData.Item1.SummaryProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    SummaryPropertySetId = TestFileData.Item2.SummaryPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    SummaryProperties = new() { Id = TestFileData.Item1.SummaryProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    SummaryProperties = new() { Id = TestFileData.Item2.SummaryProperties.Id }, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    SummaryPropertySetId = TestFileData.Item1.SummaryPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    SummaryPropertySetId = TestFileData.Item2.SummaryPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    SummaryPropertySetId = TestFileData.Item1.SummaryPropertySetId, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn,
                    CreationTime = TestFileData.Item2.CreationTime, LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed,
                    Parent = new() { Id = TestFileData.Item2.ParentId }, BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item2.UpstreamId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    LastSynchronizedOn = TestFileData.Item1.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    LastSynchronizedOn = TestFileData.Item2.LastSynchronizedOn, UpstreamId = TestFileData.Item1.UpstreamId, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn,
                    CreationTime = TestFileData.Item1.CreationTime, LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed,
                    Parent = new() { Id = TestFileData.Item1.ParentId }, BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Options = TestFileData.Item2.Options, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    Options = TestFileData.Item1.Options, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Status = TestFileData.Item1.Status, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, Parent = new() { Id = TestFileData.Item1.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                new DbFile()
                {
                    Status = TestFileData.Item2.Status, CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, Parent = new() { Id = TestFileData.Item1.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    Name = TestFileData.Item2.Name, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    Name = TestFileData.Item1.Name, CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, Parent = new() { Id = TestFileData.Item2.ParentId },
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryProperties = new() { Id = TestFileData.Item2.BinaryProperties.Id }
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryProperties = new() { Id = TestFileData.Item1.BinaryProperties.Id }
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, ParentId = TestFileData.Item1.ParentId,
                    BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, ParentId = TestFileData.Item1.ParentId,
                    BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, ParentId = TestFileData.Item1.ParentId,
                    BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item1.CreatedOn, ModifiedOn = TestFileData.Item1.ModifiedOn, CreationTime = TestFileData.Item1.CreationTime,
                    LastWriteTime = TestFileData.Item1.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, ParentId = TestFileData.Item1.ParentId,
                    BinaryPropertySetId = TestFileData.Item1.BinaryPropertySetId
                },
                false
            };
            yield return new object[]
            {
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item1.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                new DbFile()
                {
                    CreatedOn = TestFileData.Item2.CreatedOn, ModifiedOn = TestFileData.Item2.ModifiedOn, CreationTime = TestFileData.Item2.CreationTime,
                    LastWriteTime = TestFileData.Item2.LastWriteTime, LastAccessed = TestFileData.Item2.LastAccessed, ParentId = TestFileData.Item2.ParentId,
                    BinaryPropertySetId = TestFileData.Item2.BinaryPropertySetId
                },
                false
            };

            #endregion
        }

        private static IEnumerable<object[]> GetEqualsTestData_Old()
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
            Subdirectory parentA = new() { Id = parentId, Volume = new() { Id = Guid.NewGuid(), FileSystem = new() { Id = Guid.NewGuid() } } };
            parentA.Volume.RootDirectory = parentA;
            Subdirectory parentB = new() { Id = parentId, Volume = new() { Id = parentA.Volume.Id, FileSystem = new() { Id = parentA.Volume.FileSystemId } } };
            parentB.Volume.RootDirectory = parentB;
            Subdirectory parent2 = new() { Id = Guid.NewGuid(), Volume = new() { Id = Guid.NewGuid(), FileSystem = new() { Id = Guid.NewGuid() } } };
            parent2.Volume.RootDirectory = parent2;
            (DbFile, DbFile)[] getEqualPropertyItems() => new (DbFile, DbFile)[]
            {
                (
                    new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { CreatedOn = createdOn, ModifiedOn = plus1, BinaryProperties = binaryPropertiesA, Parent = parentA, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { CreatedOn = createdOn, ModifiedOn = plus1, BinaryProperties = binaryPropertiesB, Parent = parentB, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { CreationTime = plus1, LastWriteTime = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, LastAccessed = plus1 },
                    new DbFile() { CreationTime = plus1, LastWriteTime = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, LastAccessed = plus1 }
                ),
                (
                    new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { LastAccessed = plus2, LastWriteTime = plus1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreationTime = plus1 },
                    new DbFile() { LastAccessed = plus2, LastWriteTime = plus1, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreationTime = plus1 }
                ),
                (
                    new DbFile() { LastHashCalculation = plus1, LastAccessed = plus2, LastWriteTime = plus2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreationTime = plus1 },
                    new DbFile() { LastHashCalculation = plus1, LastAccessed = plus2, LastWriteTime = plus2, CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreationTime = plus1 }
                ),
                (
                    new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Options = FileCrawlOptions.FlaggedForRescan, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { Options = FileCrawlOptions.FlaggedForRescan, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ParentId = parentId, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { ParentId = parentId, BinaryProperties = binaryPropertiesB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Status = FileCorrelationStatus.Correlated, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { Status = FileCorrelationStatus.Correlated, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                ),
                (
                    new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 },
                    new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus2, LastWriteTime = plus2 }
                )
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
                (
                    new DbFile() { CreatedOn = createdOn, ModifiedOn = plus1, BinaryProperties = binaryPropertiesA, Parent = parentA, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { CreatedOn = createdOn, ModifiedOn = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id1, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id1, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id1, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { LastSynchronizedOn = plus2, UpstreamId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { LastSynchronizedOn = plus1, UpstreamId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { LastSynchronizedOn = plus2, UpstreamId = id2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { AudioProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { AudioProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { AudioPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { AudioPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { AudioPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = new() { Id = Guid.NewGuid() }, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = new() { Id = Guid.NewGuid() }, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryPropertySetId = Guid.NewGuid(), Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryPropertySetId = Guid.NewGuid(), Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryProperties = new() { Id = binaryPropertySetId }, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryPropertySetId = binaryPropertySetId, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryProperties = new() { Id = Guid.Empty }, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryPropertySetId = Guid.Empty, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { CreationTime = plus1, LastWriteTime = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, LastAccessed = plus1 },
                    new DbFile() { CreationTime = plus1, LastWriteTime = plus1, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, LastAccessed = plus1 }
                ),
                (
                    new DbFile() { CreationTime = plus1, LastWriteTime = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, LastAccessed = plus1 },
                    new DbFile() { CreationTime = plus2, LastWriteTime = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, LastAccessed = plus1 }
                ),
                (
                    new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DocumentProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DocumentProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DocumentPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DocumentPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DocumentProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DocumentPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DocumentProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DocumentPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DRMProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DRMProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DRMPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { DRMPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { DRMPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { GPSProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { GPSProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { GPSPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { GPSPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { GPSPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { ImageProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { ImageProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { ImagePropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { ImagePropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImageProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImagePropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImageProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ImagePropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { LastAccessed = plus2, LastWriteTime = plus1, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1 },
                    new DbFile() { LastAccessed = plus1, LastWriteTime = plus1, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1 }
                ),
                (
                    new DbFile() { LastAccessed = plus2, LastWriteTime = plus1, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1 },
                    new DbFile() { LastAccessed = plus2, LastWriteTime = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1 }
                ),
                (
                    new DbFile() { LastHashCalculation = plus1, LastAccessed = plus2, LastWriteTime = plus2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1 },
                    new DbFile() { LastHashCalculation = plus2, LastAccessed = plus2, LastWriteTime = plus2, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1 }
                ),
                (
                    new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MediaProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MediaProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MediaPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MediaPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MediaProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MediaPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MediaProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MediaPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MusicProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MusicProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MusicPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { MusicPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { MusicPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Name = "Test2", BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Name = "Test", BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Notes = "Test2", BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Notes = "Test", BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Options = FileCrawlOptions.FlaggedForRescan, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Options = FileCrawlOptions.DoNotCompare, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Parent = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB , CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ParentId = parentId, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { Parent = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { ParentId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ParentId = parentId, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Parent = new() { Id = parentId }, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { ParentId = Guid.Empty, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Parent = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { PhotoProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { PhotoProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { PhotoPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { PhotoPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { PhotoPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { RecordedTVProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { RecordedTVProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { RecordedTVPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { RecordedTVPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { RecordedTVPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { Status = FileCorrelationStatus.Correlated, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { SummaryProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { SummaryProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { SummaryPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { SummaryPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { SummaryPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { VideoProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { VideoProperties = new() { Id = Guid.NewGuid() }, BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { VideoPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { VideoPropertySetId = Guid.NewGuid(), BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoProperties = new() { Id = id2 }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoPropertySetId = id2, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoProperties = new() { Id = Guid.Empty }, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus1, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                ),
                (
                    new DbFile() { VideoPropertySetId = Guid.Empty, BinaryProperties = binaryPropertiesA, Parent = parentA, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 },
                    new DbFile() { BinaryProperties = binaryPropertiesB, Parent = parentB, CreatedOn = createdOn, ModifiedOn = plus2, CreationTime = plus1, LastAccessed = plus1, LastWriteTime = plus1 }
                )
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
        [DynamicData(nameof(GetEqualsTestData), DynamicDataSourceType.Method)]
        public void EqualsTestMethod2(DbFile target, DbFile other, bool expectedResult)
        {
            bool actualResult = target.Equals(other);
            actualResult = target.Equals(other);
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
