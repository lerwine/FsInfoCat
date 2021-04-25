using FsInfoCat.Desktop.Model;
using FsInfoCat.Desktop.ViewModel;
using NUnit.Framework;
using System.Collections.Generic;
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
