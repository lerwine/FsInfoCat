using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalDbContextTests
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context) => TestHelper.AssemblyInit(context);

        [AssemblyCleanup()]
        public static void AssemblyCleanup() => TestHelper.AssemblyCleanup();

        [TestMethod("new LocalDbContext(DbContextOptions<LocalDbContext>)")]
        public void NewLocalDbContextDbContextOptionsTestMethod()
        {
            using LocalDbContext dbContext = Services.ServiceProvider.GetService<LocalDbContext>();

            Assert.IsNotNull(dbContext.FileSystems);
            Assert.IsNotNull(dbContext.SymbolicNames);
            Assert.IsNotNull(dbContext.Volumes);
            Assert.IsNotNull(dbContext.VolumeAccessErrors);
            Assert.IsNotNull(dbContext.CrawlConfigurations);
            Assert.IsNotNull(dbContext.Subdirectories);
            Assert.IsNotNull(dbContext.SubdirectoryAccessErrors);
            Assert.IsNotNull(dbContext.Files);
            Assert.IsNotNull(dbContext.FileAccessErrors);
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
            Assert.IsNotNull(dbContext.BinaryPropertySets);
            Assert.IsNotNull(dbContext.Comparisons);
            Assert.IsNotNull(dbContext.RedundantSets);
            Assert.IsNotNull(dbContext.Redundancies);
        }
    }
}
