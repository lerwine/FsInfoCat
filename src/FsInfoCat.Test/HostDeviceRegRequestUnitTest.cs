using System;
#if WINDOWS
using System.Security.Principal;
#endif
using FsInfoCat.Models.HostDevices;
using NUnit.Framework;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class HostDeviceRegRequestUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateForLocalTest()
        {
            HostDeviceRegRequest target = HostDeviceRegRequest.CreateForLocal();
            Assert.NotNull(target);
            Assert.AreEqual(Environment.MachineName, target.MachineName);
            Assert.NotNull(target.MachineIdentifer);
#if WINDOWS
            SecurityIdentifier securityIdentifier = new SecurityIdentifier(target.MachineIdentifer);
            Assert.NotNull(securityIdentifier);
#else
            Assert.IsTrue(Guid.TryParse(target.MachineIdentifer, out Guid id));
#endif
        }
    }
}
