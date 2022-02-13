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
#pragma warning disable IDE0052 // Remove unread private members
        private static TestContext _testContext;
#pragma warning restore IDE0052 // Remove unread private members

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

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
