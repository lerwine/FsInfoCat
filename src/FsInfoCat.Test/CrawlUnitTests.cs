using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using FsInfoCat.Models.Accounts;
using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.DB;
using FsInfoCat.Util;
using NUnit.Framework;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class CrawlUnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FsHostTest()
        {
            FsHost target = new FsHost();
            string actualString = target.MachineIdentifier;
            Assert.IsNull(actualString);
            actualString = target.MachineName;
            Assert.IsNull(actualString);
            Assert.NotNull(target.Roots);
            DriveInfo[] expectedDrives = DriveInfo.GetDrives();
            FsRoot[] roots = expectedDrives.Select(d => new FsRoot()
            {
                DriveFormat = d.DriveFormat,
                DriveType = d.DriveType,
                RootPathName = d.RootDirectory.FullName,
                VolumeName = d.VolumeLabel
            }).ToArray();
            for (int i = 0; i < roots.Length; i++)
                target.Roots.Add(roots[i]);
            Assert.AreEqual(roots.Length, target.Roots.Count);
            ISite site;
            NestedCollectionComponentContainer<FsHost, FsRoot> container = null;
            for (int i = 0; i < roots.Length; i++)
            {
                FsRoot actualItem = target.Roots[i];
                Assert.AreSame(actualItem, roots[i]);
                site = ((IComponent)actualItem).Site;
                Assert.IsNotNull(site);
                Assert.AreSame(site.Component, roots[i]);
                container = site.Container as NestedCollectionComponentContainer<FsHost, FsRoot>;
                Assert.NotNull(container);
                Assert.AreSame(target, container.Owner);
            }
            site = ((IComponent)roots[0]).Site;
            Assert.IsNotNull(site);
            container = site.Container as NestedCollectionComponentContainer<FsHost, FsRoot>;
            Assert.NotNull(container);
            Assert.AreSame(target, container.Owner);
            container.Remove(roots[0]);
            site = ((IComponent)roots[0]).Site;
            Assert.IsNull(site);
            for (int i = 1; i < roots.Length; i++)
            {
                FsRoot actualItem = target.Roots[i - 1];
                Assert.AreSame(actualItem, roots[i]);
                site = ((IComponent)actualItem).Site;
                Assert.IsNotNull(site);
                Assert.AreSame(site.Component, roots[i]);
                container = site.Container as NestedCollectionComponentContainer<FsHost, FsRoot>;
                Assert.NotNull(container);
                Assert.AreSame(target, container.Owner);
            }
            site = ((IComponent)roots[roots.Length - 1]).Site;
            Assert.IsNotNull(site);
            container = site.Container as NestedCollectionComponentContainer<FsHost, FsRoot>;
            Assert.NotNull(container);
            Assert.AreSame(target, container.Owner);
            container.Remove(roots[roots.Length - 1]);
            site = ((IComponent)roots[roots.Length - 1]).Site;
            Assert.IsNull(site);
            for (int i = 2; i < roots.Length; i++)
            {
                FsRoot actualItem = target.Roots[i - 2];
                Assert.AreSame(actualItem, roots[i - 1]);
                site = ((IComponent)actualItem).Site;
                Assert.IsNotNull(site);
                Assert.AreSame(site.Component, roots[i - 1]);
                container = site.Container as NestedCollectionComponentContainer<FsHost, FsRoot>;
                Assert.NotNull(container);
                Assert.AreSame(target, container.Owner);
            }
        }
    }
}
