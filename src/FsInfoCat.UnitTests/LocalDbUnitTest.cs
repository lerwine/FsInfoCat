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

        [TestMethod]
        [Ignore]
        public void LocalDbContextTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Assert.IsNotNull(dbContext.FileSystems);
            Assert.IsNotNull(dbContext.SymbolicNames);
            Assert.IsNotNull(dbContext.Volumes);
            Assert.IsNotNull(dbContext.Subdirectories);
            Assert.IsNotNull(dbContext.Files);
            Assert.IsNotNull(dbContext.BinaryPropertySets);
            Assert.IsNotNull(dbContext.SummaryPropertySets);
            Assert.IsNotNull(dbContext.DocumentPropertySets);
            Assert.IsNotNull(dbContext.AudioPropertySets);
            Assert.IsNotNull(dbContext.DRMPropertySets);
            Assert.IsNotNull(dbContext.GPSPropertySets);
            Assert.IsNotNull(dbContext.ImagePropertySets);
            Assert.IsNotNull(dbContext.MediaPropertySets);
            Assert.IsNotNull(dbContext.MusicPropertySets);
            Assert.IsNotNull(dbContext.PhotoPropertySets);
            Assert.IsNotNull(dbContext.RecordedTVPropertySets);
            Assert.IsNotNull(dbContext.VideoPropertySets);
            Assert.IsNotNull(dbContext.Comparisons);
            Assert.IsNotNull(dbContext.RedundantSets);
            Assert.IsNotNull(dbContext.Redundancies);
        }
    }
}
