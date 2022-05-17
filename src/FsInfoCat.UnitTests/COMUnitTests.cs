using Microsoft.VisualStudio.TestTools.UnitTesting;
// using FsInfoCat.Desktop.COM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Shell32;

namespace FsInfoCat.UnitTests
{
    [TestClass()]
    public class COMUnitTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

    //     [TestMethod(), Priority(0)]
    //     public void GetExtendedPropertyMapAsyncTest()
    //     {
    //         using ShellAdapter shell = new();
    //         Folder folder = shell.NameSpace(_testContext.TestRunDirectory);
    //         Assert.IsNotNull(folder);
    //         Dictionary<int, string> result = folder.GetExtendedPropertyMapAsync().Result;
    //         Assert.IsNotNull(result);
    //         Assert.IsTrue(result.Values.Contains("Item type"));
    //         Assert.IsTrue(result.Values.Contains("Kind"));
    //         Assert.IsTrue(result.Values.Contains("Date created"));
    //    }

    //     [TestMethod(), Priority(0)]
    //     public void GetExtendedPropertyDictionaryAsyncTest()
    //     {
    //         using ShellAdapter shell = new();
    //         Folder folder = shell.NameSpace(_testContext.TestRunDirectory);
    //         Assert.IsNotNull(folder);
    //         Dictionary<ExtendedPropertyName, int[]> result = folder.GetExtendedPropertyDictionaryAsync().Result;
    //         Assert.IsNotNull(result);
    //         Assert.IsTrue(result.ContainsKey(ExtendedPropertyName.ItemType));
    //         Assert.IsTrue(result.ContainsKey(ExtendedPropertyName.Kind));
    //         Assert.IsTrue(result.ContainsKey(ExtendedPropertyName.DateCreated));
    //     }

    //     [TestMethod(), Priority(0)]
    //     public void GetExtendedPropertiesAsyncTest()
    //     {
    //         using ShellAdapter shell = new();
            
    //         Folder folder = shell.NameSpace(_testContext.DeploymentDirectory);
    //         (string Name, Dictionary<ExtendedPropertyName, string> properties) nameAndProperties = folder.GetExtendedPropertiesAsync(o => o.Name == "FsInfoCat.Desktop.dll").Result.FirstOrDefault();
    //         Assert.IsNotNull(nameAndProperties);
    //         Assert.IsTrue(nameAndProperties.properties.ContainsKey(ExtendedPropertyName.ProductVersion));
    //         Assert.IsTrue(Version.TryParse(nameAndProperties.properties[ExtendedPropertyName.ProductVersion], out Version version));
    //         Assert.AreEqual(typeof(ShellAdapter).Assembly.GetName().Version.ToString(3), version.ToString(3));

    //         nameAndProperties = folder.GetExtendedPropertiesAsync(o => o.Name == "Microsoft.Data.Sqlite.dll").Result.FirstOrDefault();
    //         Assert.IsNotNull(nameAndProperties);
    //         Assert.IsTrue(nameAndProperties.properties.ContainsKey(ExtendedPropertyName.ProductVersion));
    //         Assert.IsTrue(Version.TryParse(nameAndProperties.properties[ExtendedPropertyName.ProductVersion], out version));
    //         Assert.AreEqual(typeof(Microsoft.Data.Sqlite.SqliteConnectionStringBuilder).Assembly.GetName().Version.ToString(3), version.ToString(3));
    //     }

        [TestMethod(), Ignore]
        public void GetAllExtendedPropertiesAsyncTest()
        {
            Assert.Inconclusive();
        }
    }
}
