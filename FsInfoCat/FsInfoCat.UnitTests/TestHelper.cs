using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace FsInfoCat.UnitTests
{
    public static class TestHelper
    {
        internal const string TEST_DB_PATH = @"Resources\TestLocal.db2";

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            Services.Initialize(services =>
            {
                string dbPath = Path.Combine(AppContext.BaseDirectory, "TestLocal.db");
                Local.LocalDbContext.ConfigureServices(services, dbPath);
            }).Wait();
        }

        [AssemblyCleanup()]
        [DeploymentItem(TEST_DB_PATH)]
        public static void AssemblyCleanup()
        {
            using (Services.Host)
                Services.Host.StopAsync(TimeSpan.FromSeconds(5)).Wait();
        }
    }
}
