using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
            Services.Initialize(services =>
            {
                string dbPath = Path.Combine(AppContext.BaseDirectory, TestHelper.TEST_DB_PATH);
                Local.LocalDbContext.ConfigureServices(services, dbPath);
            }).Wait();
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
