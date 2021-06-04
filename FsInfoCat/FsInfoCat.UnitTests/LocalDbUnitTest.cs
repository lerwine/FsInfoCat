using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalDbUnitTest
    {
        private const string TestProperty_Description = "Description";
        private static TestContext _testContext;

        [TestMethod]
        [DeploymentItem(TestHelper.TEST_DB_PATH)]
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
