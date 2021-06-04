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
        private static TestContext _testContext;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
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

        [TestMethod]
        public void LocalDbContextTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Assert.IsNotNull(dbContext.FileSystems);
            Assert.IsNotNull(dbContext.SymbolicNames);
            Assert.IsNotNull(dbContext.Volumes);
            Assert.IsNotNull(dbContext.Subdirectories);
            Assert.IsNotNull(dbContext.Files);
            Assert.IsNotNull(dbContext.ExtendedProperties);
            Assert.IsNotNull(dbContext.ContentInfos);
            Assert.IsNotNull(dbContext.Comparisons);
            Assert.IsNotNull(dbContext.RedundantSets);
            Assert.IsNotNull(dbContext.Redundancies);
        }
    }
}
