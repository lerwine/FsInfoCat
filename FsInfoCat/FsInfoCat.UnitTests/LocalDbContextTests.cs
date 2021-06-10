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
        private static TestContext _testContext;

        [AssemblyInitialize()]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void AssemblyInit(TestContext context)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            string dbPath = Path.Combine(AppContext.BaseDirectory, TestHelper.TEST_DB_PATH);
            Services.Initialize(services =>
            {
                LocalDbContext.ConfigureServices(services, dbPath);
            }).Wait();
            //var logger = Services.ServiceProvider.GetRequiredService<ILogger<LocalDbContextTests>>();
            //using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            //XDocument document = XDocument.Parse(Properties.Resources.DbCommands);
            //var logger = Services.ServiceProvider.GetService<ILogger<LocalDbContextTests>>();
            //foreach (XElement element in document.Root.Element("DropTables").Elements("Text"))
            //{
            //    logger.LogInformation(element.Attribute("Message").Value);
            //    dbContext.Database.ExecuteSqlRaw(element.Value.Trim());
            //}
            //foreach (XElement element in document.Root.Element("DbCreation").Elements("Text"))
            //{
            //    logger.LogInformation(element.Attribute("Message").Value);
            //    dbContext.Database.ExecuteSqlRaw(element.Value.Trim());
            //}
            //dbContext.Import(XDocument.Parse(Properties.Resources.TestData));
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            using (Services.Host)
                Services.Host.StopAsync(TimeSpan.FromSeconds(5)).Wait();
        }

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new LocalDbContext(DbContextOptions<LocalDbContext>)")]
        public void NewLocalDbContextDbContextOptionsTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();

            Assert.IsNotNull(dbContext.FileSystems);
            Assert.IsNotNull(dbContext.SymbolicNames);
            Assert.IsNotNull(dbContext.Volumes);
            Assert.IsNotNull(dbContext.VolumeAccessErrors);
            Assert.IsNotNull(dbContext.Subdirectories);
            Assert.IsNotNull(dbContext.SubdirectoryAccessErrors);
            Assert.IsNotNull(dbContext.Files);
            Assert.IsNotNull(dbContext.FileAccessErrors);
            Assert.IsNotNull(dbContext.ExtendedProperties);
            Assert.IsNotNull(dbContext.ContentInfos);
            Assert.IsNotNull(dbContext.Comparisons);
            Assert.IsNotNull(dbContext.RedundantSets);
            Assert.IsNotNull(dbContext.Redundancies);
        }
    }
}
