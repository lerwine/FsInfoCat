using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using NUnit.Framework;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class CrawlUnitTests
    {
        public static IEnumerable<TestCaseData> GetFsHostAddTestCases()
        {
            Collection<Tuple<FsRoot, bool>> fsRoots = new Collection<Tuple<FsRoot, bool>>();
            fsRoots.Add(new Tuple<FsRoot, bool>(
                new FsRoot()
                {
                    CaseSensitive = false,
                    DriveFormat = "NTFS",
                    DriveType = DriveType.Fixed,
                    RootPathName = "C:\\",
                    Identifier = new VolumeIdentifier(0x9E497DE8),
                    VolumeName = "OS"
                },
                true
            ));
            yield return new TestCaseData(new Collection<Tuple<FsRoot, bool>>(fsRoots.ToArray()))
                .SetName("FsHostAddTest:Single item")
                .Returns(1);
            fsRoots.Add(new Tuple<FsRoot, bool>(fsRoots[0].Item1, false));
            yield return new TestCaseData(new Collection<Tuple<FsRoot, bool>>(fsRoots.ToArray()))
                .SetName("FsHostAddTest:Add same item twice")
                .Returns(1);
            fsRoots[1] = new Tuple<FsRoot, bool>(
                new FsRoot()
                {
                    CaseSensitive = false,
                    DriveFormat = "NTFS",
                    DriveType = DriveType.Fixed,
                    RootPathName = "D:\\",
                    Identifier = new VolumeIdentifier(0xB744F201),
                    VolumeName = "OS"
                },
                true
            );
            fsRoots.Insert(1, new Tuple<FsRoot, bool>(fsRoots[0].Item1, false));
            yield return new TestCaseData(new Collection<Tuple<FsRoot, bool>>(fsRoots.ToArray()))
                .SetName("FsHostAddTest:Add 2 different items, first one twice")
                .Returns(2);
        }

        public static IEnumerable<TestCaseData> GetFsHostRemoveTestCases()
        {
            Func<List<FsRoot>> getAddList = () =>
            {
                List<FsRoot> list = new List<FsRoot>();
                list.Add(new FsRoot()
                {
                    CaseSensitive = false,
                    DriveFormat = "MAFS",
                    DriveType = DriveType.Fixed,
                    RootPathName = "Z:\\",
                    Identifier = new VolumeIdentifier(@"\\servicenowdiag479.file.core.windows.net\testazureshare"),
                    VolumeName = ""
                });
                list.Add(new FsRoot()
                {
                    CaseSensitive = false,
                    DriveFormat = "FAT32",
                    DriveType = DriveType.Removable,
                    RootPathName = "F:\\",
                    Identifier = new VolumeIdentifier("urn:volume:id:3B51-8D4B"),
                    VolumeName = "HP_TOOLS"
                });
                list.Add(new FsRoot()
                {
                    CaseSensitive = true,
                    DriveFormat = "ext4",
                    DriveType = DriveType.Fixed,
                    RootPathName = "/",
                    Identifier = new VolumeIdentifier("urn:uuid:3756934c-31d3-413c-8df9-5b7c7b1a4451"),
                    VolumeName = "cloudimg-rootfs"
                });
                return list;
            };

            List<FsRoot> addList = getAddList();
            Collection<Tuple<FsRoot, int>> removeList = new Collection<Tuple<FsRoot, int>>();
            removeList.Add(new Tuple<FsRoot, int>(addList[0], 0));
            removeList.Add(new Tuple<FsRoot, int>(
                new FsRoot()
                {
                    CaseSensitive = false,
                    DriveFormat = "NTFS",
                    DriveType = DriveType.Fixed,
                    RootPathName = "C:\\",
                    Identifier = new VolumeIdentifier(0x9E497DE8),
                    VolumeName = "OS"
                },
                -1
            ));
            yield return new TestCaseData(addList, removeList)
                .SetName("FsHostRemoveTest:Remove first and non-existent")
                .Returns(2);
            addList = getAddList();
            removeList = new Collection<Tuple<FsRoot, int>>();
            removeList.Add(new Tuple<FsRoot, int>(addList[1], 1));
            yield return new TestCaseData(addList, removeList)
                .SetName("FsHostRemoveTest:Remove second")
                .Returns(2);
            addList = getAddList();
            removeList = new Collection<Tuple<FsRoot, int>>();
            removeList.Add(new Tuple<FsRoot, int>(addList[2], 2));
            yield return new TestCaseData(addList, removeList)
                .SetName("FsHostRemoveTest:Remove last")
                .Returns(2);
            addList = getAddList();
            removeList = new Collection<Tuple<FsRoot, int>>();
            removeList.Add(new Tuple<FsRoot, int>(addList[0], 0));
            removeList.Add(new Tuple<FsRoot, int>(addList[2], 1));
            yield return new TestCaseData(addList, removeList)
                .SetName("FsHostRemoveTest:Remove first then last")
                .Returns(1);
            addList = getAddList();
            removeList = new Collection<Tuple<FsRoot, int>>();
            removeList.Add(new Tuple<FsRoot, int>(addList[2], 2));
            removeList.Add(new Tuple<FsRoot, int>(addList[0], 0));
            removeList.Add(new Tuple<FsRoot, int>(addList[2], -1));
            yield return new TestCaseData(addList, removeList)
                .SetName("FsHostRemoveTest:Remove last then first, then last again")
                .Returns(1);
            addList = getAddList();
            removeList = new Collection<Tuple<FsRoot, int>>();
            removeList.Add(new Tuple<FsRoot, int>(addList[2], 2));
            removeList.Add(new Tuple<FsRoot, int>(addList[0], 0));
            removeList.Add(new Tuple<FsRoot, int>(addList[1], 0));
            yield return new TestCaseData(addList, removeList)
                .SetName("FsHostRemoveTest:Remove last, first, then second")
                .Returns(0);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test, Property("Priority", 1)]
        public void FsRootConstructorTest()
        {
            FsRoot target = new FsRoot();
            Assert.That(target.CaseSensitive, Is.False);
            Assert.That(target.DriveFormat, Is.Null);
            Assert.That(target.DriveType, Is.EqualTo(DriveType.Unknown));
            Assert.That(target.RootPathName, Is.Null);
            Assert.That(target.Identifier, Is.EqualTo(VolumeIdentifier.Empty));
            Assert.That(target.VolumeName, Is.Null);
            Assert.That(target.ChildNodes, Is.Not.Null.Or.Empty);
        }

        [Property("Priority", 1)]
        public void FsHostConstructorTest()
        {
            FsHost target = new FsHost();
            Assert.That(target.MachineIdentifier, Is.Null);
            Assert.That(target.MachineName, Is.Null);
            Assert.That(target.Roots, Is.Not.Null.Or.Empty);
        }

        [Test, Property("Priority", 2)]
        [TestCaseSource("GetFsHostAddTestCases")]
        public int FsHostAddTest(IList<Tuple<FsRoot, bool>> addList)
        {
            FsHost target = new FsHost();
            Collection<FsRoot> rootsList = new Collection<FsRoot>();
            foreach (Tuple<FsRoot, bool> testData in addList)
            {
                target.Roots.Add(testData.Item1);
                if (testData.Item2)
                    rootsList.Add(testData.Item1);
                for (int i = 0; i < rootsList.Count; i++)
                {
                    FsRoot fsRoot = target.Roots[i];
                    Assert.That(fsRoot, Is.SameAs(rootsList[i]));
                    INestedSite site = ((IComponent)fsRoot).Site as INestedSite;
                    Assert.That(site, Is.Not.Null);
                    Assert.That(site.Component, Is.SameAs(fsRoot));
                    INestedContainer container = site.Container as INestedContainer;
                    Assert.That(container, Is.Not.Null);
                    Assert.That(container.Owner, Is.SameAs(target));
                    Assert.That(ComponentList.IsPlaceHolderContainer(container), Is.False);
                }
            }
            return target.Roots.Count;
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetFsHostRemoveTestCases")]
        public int FsHostRemoveTest(IList<FsRoot> addList, IList<Tuple<FsRoot, int>> removeList)
        {
            FsHost target = new FsHost();
            foreach (FsRoot fsRoot in addList)
                target.Roots.Add(fsRoot);
            int expectedCount = addList.Count;
            foreach (Tuple<FsRoot, int> toRemove in removeList)
            {
                if (toRemove.Item2 < 0)
                    Assert.That(target.Roots.Remove(toRemove.Item1), Is.False);
                else
                {
                    expectedCount--;
                    FsRoot fsRoot = target.Roots[toRemove.Item2];
                    Assert.That(target.Roots.Remove(toRemove.Item1), Is.True);
                    Assert.That(fsRoot, Is.SameAs(addList[toRemove.Item2]));
                    addList.RemoveAt(toRemove.Item2);
                    INestedSite site = ((IComponent)fsRoot).Site as INestedSite;
                    Assert.That(site, Is.Null);
                }
                Assert.That(target.Roots.Count, Is.EqualTo(expectedCount));
                for (int i = 0; i < expectedCount; i++)
                {
                    FsRoot fsRoot = target.Roots[i];
                    Assert.That(fsRoot, Is.SameAs(addList[i]));
                    INestedSite site = ((IComponent)fsRoot).Site as INestedSite;
                    Assert.That(site, Is.Not.Null);
                    Assert.That(site.Component, Is.SameAs(fsRoot));
                    INestedContainer container = site.Container as INestedContainer;
                    Assert.That(container, Is.Not.Null);
                    // Assert.That(ReferenceEquals(container.Owner, target), Is.True);
                    Assert.That(container.Owner, Is.SameAs(target));
                    Assert.That(ComponentList.IsPlaceHolderContainer(container), Is.False);
                }
            }

            return target.Roots.Count;
        }
    }
}
