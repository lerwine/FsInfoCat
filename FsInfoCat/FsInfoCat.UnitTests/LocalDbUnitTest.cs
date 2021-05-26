using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalDbUnitTest
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            _testContext = testContext;
            if (Services.ServiceProvider is null)
            {
                string path = Services.GetAppDataPath(typeof(LocalDbUnitTest).Assembly, AppDataPathLevel.Application);
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                Services.Initialize(services => Local.LocalDbContext.ConfigureServices(services, typeof(LocalDbUnitTest).Assembly, null));
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            string path = Services.GetAppDataPath(typeof(LocalDbUnitTest).Assembly, AppDataPathLevel.Application);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        [TestMethod]
        public void FileSystemTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem target = new()
            {
                DisplayName = "My File System"
            };
            EntityEntry<Local.FileSystem> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.FileSystems.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            Collection<ValidationResult> results = new();
            bool result = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(result);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            target.MaxNameLength = -1;
            dbContext.FileSystems.Update(target);
            Assert.AreEqual(entityEntry.State, EntityState.Modified);
            result = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsFalse(result);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(Local.FileSystem.MaxNameLength), results[0].MemberNames.First());
            Assert.ThrowsException<ValidationException>(() => dbContext.SaveChanges());
        }

        [TestMethod]
        public void SymbolicNameTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
            Local.FileSystem fileSystem = new()
            {
                DisplayName = "Parent File System"
            };
            dbContext.FileSystems.Add(fileSystem);
            Local.SymbolicName target = new()
            {
                Name = "MySymbolicName",
                CreatedOn = DateTime.Now.AddDays(1),
                ModifiedOn = DateTime.Now,
                FileSystemId = fileSystem.Id
            };
            Collection<ValidationResult> results = new();
            bool result = Validator.TryValidateObject(target, new ValidationContext(target), results);
            Assert.IsTrue(result);
            Assert.AreEqual(0, results.Count);
            EntityEntry<Local.SymbolicName> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(entityEntry.State, EntityState.Detached);
            entityEntry = dbContext.SymbolicNames.Add(target);
            Assert.AreEqual(entityEntry.State, EntityState.Added);
            dbContext.SaveChanges();
            Assert.AreEqual(entityEntry.State, EntityState.Unchanged);
            Assert.AreNotEqual(Guid.Empty, target.Id);
        }
    }
}
