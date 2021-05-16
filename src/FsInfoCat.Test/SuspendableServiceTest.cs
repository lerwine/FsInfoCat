using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Test
{
    public class SuspendableServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ServiceTest()
        {
            ISuspendableService x = Services.GetSuspendableService();
            Assert.That(x, Is.Not.Null);
            ISuspendableService y = Services.GetSuspendableService();
            Assert.That(y, Is.Not.Null);
            Assert.That(x, Is.SameAs(y));
        }
    }
}
