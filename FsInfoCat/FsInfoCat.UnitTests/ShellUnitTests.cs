using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class ShellUnitTests
    {

        [TestMethod("ExtendedProperties Add/Remove Tests")]
        public void ShellTestMethod()
        {
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            Object shell = Activator.CreateInstance(shellAppType);
            object folder = shellAppType.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { @"C:\Users\lerwi\OneDrive\Music\Discovery\Archive" });
            object item = shellAppType.InvokeMember("ParseName", System.Reflection.BindingFlags.InvokeMethod, null, folder, new object[] { @"10-On The run.mp3" });
            object items = shellAppType.InvokeMember("Items", System.Reflection.BindingFlags.InvokeMethod, null, folder, Array.Empty<object>());
            object count = shellAppType.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, items, Array.Empty<object>());
            for (int i = 0; i < 287; i++)
            {
                object extPropName = shellAppType.InvokeMember("GetDetailsOf", System.Reflection.BindingFlags.InvokeMethod, null, folder, new object[] { items, i });
                object extValName = shellAppType.InvokeMember("GetDetailsOf", System.Reflection.BindingFlags.InvokeMethod, null, folder, new object[] { item, i });
            }
            Marshal.ReleaseComObject(shell);
            Marshal.ReleaseComObject(folder);
            Marshal.ReleaseComObject(item);
            Marshal.ReleaseComObject(items);
        }
    }
}
