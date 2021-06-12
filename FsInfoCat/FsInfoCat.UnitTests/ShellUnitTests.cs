using FsInfoCat.Desktop.COM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shell32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class ShellUnitTests
    {
        [TestMethod]
        [Ignore]
        public void ComObjectTestMethod()
        {
            using ShellAdapter shell = new();
            Folder folder = shell.NameSpace(@"C:\Users\lerwi\Git\FsInfoCat\FsInfoCat\FsInfoCat.UnitTests\bin\Debug\net5.0-windows7.0");
            FolderItems folderItems = folder.Items();
            (string Name, Dictionary<ExtendedPropertyName, string> properties) nameAndProperties = folder.GetExtendedPropertiesAsync(o => o.Name == "FsInfoCat.Desktop.dll").Result.FirstOrDefault();
            Assert.IsNotNull(nameAndProperties);
            Assert.IsTrue(nameAndProperties.properties.ContainsKey(ExtendedPropertyName.ProductVersion));
            Assert.IsTrue(Version.TryParse(nameAndProperties.properties[ExtendedPropertyName.ProductVersion], out Version version));
            Assert.AreEqual(typeof(ShellAdapter).Assembly.GetName().Version, version);

            string path = @"C:\Users\lerwi\Git\FsInfoCat\FsInfoCat\FsInfoCat.UnitTests\Resources\Properties.xml";
            XDocument document = XDocument.Load(path);
            Dictionary<int, string> keys = new();
            for (int i = 0; i < 0xffff; i++)
            {
                string name = folder.GetDetailsOf(folderItems, i);
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                switch (name)
                {
                    case "Total editing time":
                    case "Size":
                    case "Name":
                    case "Date modified":
                    case "Date accessed":
                    case "Attributes":
                    case "Owner":
                    case "Total size":
                    case "Computer":
                    case "Filename":
                    case "Space free":
                    case "Shared":
                    case "Folder name":
                    case "Folder path":
                    case "Folder":
                    case "Path":
                    case "Space used":
                    case "Sharing status":
                    case "Availability status":
                    case "Type":
                        break;
                    default:
                        keys.Add(i, name);
                        break;
                }
            }
            for (int f = 0; f < folderItems.Count; f++)
            {
                FolderItem folderItem = folderItems.Item(f);
                if (folderItem.Type == "File folder")
                    continue;
                XElement itemElement = new("Item", new XAttribute("Name", folderItem.Name), new XAttribute("Size", folderItem.Size),
                    new XAttribute("IsLink", folderItem.IsLink), new XAttribute("Type", folderItem.Type));
                foreach (int i in keys.Keys)
                {
                    string value = folder.GetDetailsOf(folderItem, i);
                    if (!string.IsNullOrWhiteSpace(value))
                        itemElement.Add(new XElement("Property", new XAttribute("Name", keys[i]), value));
                }
                document.Root.Add(itemElement);
            }
            using (XmlWriter writer = XmlWriter.Create(path, new XmlWriterSettings
            {
                Indent = true,
                Encoding = new UTF8Encoding(false, false)
            }))
            {
                document.WriteTo(writer);
                writer.Flush();
            }
        }

        [TestMethod("ExtendedProperties Add/Remove Tests")]
        [Ignore]
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
