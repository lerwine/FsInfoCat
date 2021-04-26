using FsInfoCat.Desktop.Model;
using FsInfoCat.Desktop.ViewModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Test
{
    public class CimTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UUIDTest()
        {
            string result = new Guid(UUID.NameSpace_DNS.GetBytes().ToArray()).ToString();
            Assert.That(result, Is.EqualTo("6ba7b810-9dad-11d1-80b4-00c04fd430c8"));
            result = new Guid(UUID.NameSpace_URL.GetBytes().ToArray()).ToString();
            Assert.That(result, Is.EqualTo("6ba7b811-9dad-11d1-80b4-00c04fd430c8"));
            result = new Guid(UUID.NameSpace_OID.GetBytes().ToArray()).ToString();
            Assert.That(result, Is.EqualTo("6ba7b812-9dad-11d1-80b4-00c04fd430c8"));
            result = new Guid(UUID.NameSpace_X500.GetBytes().ToArray()).ToString();
            Assert.That(result, Is.EqualTo("6ba7b814-9dad-11d1-80b4-00c04fd430c8"));
            UUID uuid = UUID.CreateMD5FromName(UUID.NameSpace_URL, "https://github.com/lerwine/FsInfoCat");
            Guid guid = new Guid(uuid.GetBytes().ToArray());
            result = guid.ToString();
            Assert.That(result, Is.EqualTo("6ba7b811-ffff-3fff-80b4-00c04fd430c8"));
        }

        [Test]
        public void GetLogicalDiskRootDirectoriesTest()
        {
            ModalOperationStatusViewModel vm = new ModalOperationStatusViewModel();
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                IEnumerable<Win32_LogicalDiskRootDirectory> result = Win32_LogicalDiskRootDirectory.GetLogicalDiskRootDirectories(new ModalOperationStatusViewModel.Controller(vm, tokenSource.Token));
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.All.Not.Null);
                using (IEnumerator<Win32_LogicalDiskRootDirectory> enumerator = result.GetEnumerator())
                {
                    Assert.That(enumerator.MoveNext(), Is.True);
                    int index = -1;
                    do
                    {
                        Win32_LogicalDiskRootDirectory item = enumerator.Current;
                        Win32_Directory rootDirectory = item.PartComponent;
                        Assert.That(rootDirectory, Is.Not.Null, "RootDirectory {index}", ++index);
                        Win32_LogicalDisk logicalDisk = item.GroupComponent;
                        Assert.That(logicalDisk, Is.Not.Null, "LogicalDisk {index}", index);
                    } while (enumerator.MoveNext());
                }
            }
        }
    }
}
