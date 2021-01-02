using System;
using System.Security.Principal;
using System.Threading.Tasks;
using FsInfoCat.Models;
using Microsoft.AspNetCore.Mvc;
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
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SecurityIdentifier securityIdentifier = new SecurityIdentifier(target.MachineIdentifer);
                Assert.NotNull(securityIdentifier);
            }
            else
                Assert.IsTrue(Guid.TryParse(target.MachineIdentifer, out Guid id));
        }
    }
}
