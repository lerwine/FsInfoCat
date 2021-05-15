using FsInfoCat.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Test
{
    public class CollectionsServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ServiceTest()
        {
            ICollectionsService x = Extensions.GetCollectionsService();
            Assert.That(x, Is.Not.Null);
            ICollectionsService y = Extensions.GetCollectionsService();
            Assert.That(y, Is.Not.Null);
            Assert.That(x, Is.SameAs(y));
        }
    }
}
