using FsInfoCat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class EntityExtensionsTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod, Priority(0)]
        public void DirectoryToMatchedPairsTestMethod()
        {
            //Assert.IsNotNull(asyncActivityService);
            //Assert.IsNotNull(asyncActivityService.ActivityStartedObservable);
            Assert.Inconclusive("Not implemented");
        }

        [TestMethod]
        public void FileToMatchedPairsTestMethod()
        {
            Assert.Inconclusive("Not implemented");
        }
    }
}
