using FsInfoCat.Local.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalDbUnitTest
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

        [TestMethod, Priority(10)]
        public void LocalDbContextTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Assert.IsNotNull(dbContext);
            Assert.IsNotNull(dbContext.AudioPropertiesListing);
            Assert.IsNotNull(dbContext.AudioPropertySets);
            Assert.IsNotNull(dbContext.BinaryPropertySets);
            Assert.IsNotNull(dbContext.Comparisons);
            Assert.IsNotNull(dbContext.CrawlConfigListing);
            Assert.IsNotNull(dbContext.CrawlConfigReport);
            Assert.IsNotNull(dbContext.CrawlConfigurations);
            Assert.IsNotNull(dbContext.CrawlJobListing);
            Assert.IsNotNull(dbContext.CrawlJobLogs);
            Assert.IsNotNull(dbContext.DocumentPropertiesListing);
            Assert.IsNotNull(dbContext.DocumentPropertySets);
            Assert.IsNotNull(dbContext.DRMPropertiesListing);
            Assert.IsNotNull(dbContext.DRMPropertySets);
            Assert.IsNotNull(dbContext.FileAccessErrors);
            Assert.IsNotNull(dbContext.FileListingWithAncestorNames);
            Assert.IsNotNull(dbContext.FileListingWithBinaryProperties);
            Assert.IsNotNull(dbContext.FileListingWithBinaryPropertiesAndAncestorNames);
            Assert.IsNotNull(dbContext.Files);
            Assert.IsNotNull(dbContext.FileSystemListing);
            Assert.IsNotNull(dbContext.FileSystems);
            Assert.IsNotNull(dbContext.GPSPropertiesListing);
            Assert.IsNotNull(dbContext.GPSPropertySets);
            Assert.IsNotNull(dbContext.ImagePropertiesListing);
            Assert.IsNotNull(dbContext.ImagePropertySets);
            Assert.IsNotNull(dbContext.MediaPropertiesListing);
            Assert.IsNotNull(dbContext.MediaPropertySets);
            Assert.IsNotNull(dbContext.MusicPropertiesListing);
            Assert.IsNotNull(dbContext.MusicPropertySets);
            Assert.IsNotNull(dbContext.PersonalFileTagListing);
            Assert.IsNotNull(dbContext.PersonalFileTags);
            Assert.IsNotNull(dbContext.PersonalSubdirectoryTagListing);
            Assert.IsNotNull(dbContext.PersonalSubdirectoryTags);
            Assert.IsNotNull(dbContext.PersonalTagDefinitionListing);
            Assert.IsNotNull(dbContext.PersonalTagDefinitions);
            Assert.IsNotNull(dbContext.PersonalVolumeTagListing);
            Assert.IsNotNull(dbContext.PersonalVolumeTags);
            Assert.IsNotNull(dbContext.PhotoPropertiesListing);
            Assert.IsNotNull(dbContext.PhotoPropertySets);
            Assert.IsNotNull(dbContext.RecordedTVPropertiesListing);
            Assert.IsNotNull(dbContext.RecordedTVPropertySets);
            Assert.IsNotNull(dbContext.Redundancies);
            Assert.IsNotNull(dbContext.RedundantSetListing);
            Assert.IsNotNull(dbContext.RedundantSets);
            Assert.IsNotNull(dbContext.SharedFileTagListing);
            Assert.IsNotNull(dbContext.SharedFileTags);
            Assert.IsNotNull(dbContext.SharedSubdirectoryTagListing);
            Assert.IsNotNull(dbContext.SharedSubdirectoryTags);
            Assert.IsNotNull(dbContext.SharedTagDefinitionListing);
            Assert.IsNotNull(dbContext.SharedTagDefinitions);
            Assert.IsNotNull(dbContext.SharedVolumeTagListing);
            Assert.IsNotNull(dbContext.SharedVolumeTags);
            Assert.IsNotNull(dbContext.Subdirectories);
            Assert.IsNotNull(dbContext.SubdirectoryAccessErrors);
            Assert.IsNotNull(dbContext.SubdirectoryAncestorNames);
            Assert.IsNotNull(dbContext.SubdirectoryListing);
            Assert.IsNotNull(dbContext.SubdirectoryListingWithAncestorNames);
            Assert.IsNotNull(dbContext.SummaryPropertiesListing);
            Assert.IsNotNull(dbContext.SummaryPropertySets);
            Assert.IsNotNull(dbContext.SymbolicNameListing);
            Assert.IsNotNull(dbContext.SymbolicNames);
            Assert.IsNotNull(dbContext.VideoPropertiesListing);
            Assert.IsNotNull(dbContext.VideoPropertySets);
            Assert.IsNotNull(dbContext.VolumeAccessErrors);
            Assert.IsNotNull(dbContext.VolumeListing);
            Assert.IsNotNull(dbContext.VolumeListingWithFileSystem);
            Assert.IsNotNull(dbContext.Volumes);
        }
    }
}
