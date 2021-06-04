using System;
using System.Linq;

namespace FsInfoCat.UnitTests
{
    public static class TestHelper
    {
        internal const string TestCategory_LocalDb = "LocalDb";
        internal const string TestProperty_Description = "Description";
        internal const string TEST_DB_PATH = @"Resources\TestLocal.db";

        internal static Local.FileSystem GetVFatFileSystem(Local.LocalDbContext dbContext)
        {
            Guid id = Guid.Parse("{53a9e9a4-f5f0-4b4c-9f1e-4e3a80a93cfd}");
            return dbContext.FileSystems.ToList().FirstOrDefault(fs => fs.Id.Equals(id));
        }

    }
}
