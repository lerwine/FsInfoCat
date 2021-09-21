using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FsInfoCat.UnitTests
{
    /// <summary>
    /// Defines test class VolumeIdentifierUnitTest.
    /// </summary>
    [TestClass]
    public class VolumeIdentifierUnitTest
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        public static void ConstructorEmptyTestMethod()
        {
            VolumeIdentifier volumeIdentifier = new();
            Assert.AreEqual(true, volumeIdentifier.IsEmpty());
            Assert.IsNotNull(volumeIdentifier.Location);
            Assert.AreEqual("", volumeIdentifier.Location.OriginalString);
            Assert.AreEqual("", volumeIdentifier.ToString());
            Assert.IsFalse(volumeIdentifier.SerialNumber.HasValue);
            Assert.IsFalse(volumeIdentifier.UUID.HasValue);
        }

        public record ConstructorResultExpected
        {
            public bool IsArgumentOutOfRangeException { get; init; }
            public bool IsEmpty { get; init; }
            public uint? SerialNumber { get; init; }
            public Guid? UUID { get; init; }
            public Uri Location { get; init; }
            internal static object[] CreateTestData(Guid uuid) => new object[] { uuid, new ConstructorResultExpected { UUID = uuid, Location = new Uri($"urn:uuid:{uuid:d}", UriKind.Absolute) } };
            internal static object[] CreateTestData(uint serialNumber) => new object[] { serialNumber, new ConstructorResultExpected { SerialNumber = serialNumber, Location = new Uri($"urn:volume:id:{(serialNumber >> 8):X4}-{(serialNumber & 0x0ffff):X4}", UriKind.Absolute) } };
            internal static object[] CreateTestData(string text, Uri location) => new object[] { text, new ConstructorResultExpected { Location = location } };
            internal static object[] CreateTestData(string text, Guid uuid) => new object[] { text, new ConstructorResultExpected { UUID = uuid, Location = new Uri($"urn:uuid:{uuid:d}", UriKind.Absolute) } };
            internal static object[] CreateTestData(string text, uint serialNumber) => new object[] { text, new ConstructorResultExpected { SerialNumber = serialNumber, Location = new Uri($"urn:volume:id:{(serialNumber >> 8):X4}-{(serialNumber & 0x0ffff):X4}", UriKind.Absolute) } };
            internal static object[] CreateEmptyTestData(string text) => new object[] { text, new ConstructorResultExpected { IsEmpty = true, Location = new("", UriKind.Relative) } };
            internal static object[] CreateArgumentOutOfRangeExceptionTestData(string text) => new object[] { text, new ConstructorResultExpected { IsArgumentOutOfRangeException = true } };
            internal static object[] CreateTestData(Uri uri, Uri location) => new object[] { uri, new ConstructorResultExpected { Location = location } };
            internal static object[] CreateTestData(Uri uri, Guid uuid) => new object[] { uri, new ConstructorResultExpected { UUID = uuid, Location = new Uri($"urn:uuid:{uuid:d}", UriKind.Absolute) } };
            internal static object[] CreateTestData(Uri uri, uint serialNumber) => new object[] { uri, new ConstructorResultExpected { SerialNumber = serialNumber, Location = new Uri($"urn:volume:id:{(serialNumber >> 8):X4}-{(serialNumber & 0x0ffff):X4}", UriKind.Absolute) } };
            internal static object[] CreateEmptyTestData(Uri uri) => new object[] { uri, new ConstructorResultExpected { IsEmpty = true, Location = new("", UriKind.Relative) } };
            internal static object[] CreateArgumentOutOfRangeExceptionTestData(Uri uri) => new object[] { uri, new ConstructorResultExpected { IsArgumentOutOfRangeException = true } };
        }

        public static IEnumerable<object[]> GetConstructorUriTestData()
        {
            yield return ConstructorResultExpected.CreateEmptyTestData((Uri)null);
            yield return ConstructorResultExpected.CreateEmptyTestData(new Uri("", UriKind.Relative));
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:volume:id:094F6-22A5", UriKind.Absolute), 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:volume:id:094F622A5", UriKind.Absolute), 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:volume:id:094f6-22a5", UriKind.Absolute), 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:volume:id:094f622a5", UriKind.Absolute), 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:volume:id:ffffffff", UriKind.Absolute), 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:volume:id:ffff-ffff", UriKind.Absolute), 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:volume:id:FFFF-FFFF", UriKind.Absolute), 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:volume:id:FFFFFFFF", UriKind.Absolute), 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:uuid:91502fe2-cb4b-4274-a8ad-8b70074132c3", UriKind.Absolute), Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:uuid:91502fe2cb4b4274a8ad8b70074132c3", UriKind.Absolute), Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:uuid:91502FE2-CB4B-4274-A8AD-8B70074132C3", UriKind.Absolute), Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:uuid:91502FE2CB4B4274A8AD8B70074132C3", UriKind.Absolute), Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:uuid:00000000-0000-0000-0000-000000000000", UriKind.Absolute), Guid.Empty);
            yield return ConstructorResultExpected.CreateTestData(new Uri("urn:uuid:00000000000000000000000000000000", UriKind.Absolute), Guid.Empty);
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://DESKTOP-10538/USERS/JLYNN/ANWR", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare/", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://desktop-10538/Users/jlynn/ANWR/", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE/", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://DESKTOP-10538/USERS/JLYNN/ANWR/", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare?", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://desktop-10538/Users/jlynn/ANWR?", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE?", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://DESKTOP-10538/USERS/JLYNN/ANWR?", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare/?", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://desktop-10538/Users/jlynn/ANWR/?", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE/?", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://DESKTOP-10538/USERS/JLYNN/ANWR/?", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare#", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://desktop-10538/Users/jlynn/ANWR#", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE#", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://DESKTOP-10538/USERS/JLYNN/ANWR#", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare/#", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://desktop-10538/Users/jlynn/ANWR/#", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE/#", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://DESKTOP-10538/USERS/JLYNN/ANWR/#", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare?#", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://desktop-10538/Users/jlynn/ANWR?#", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE?#", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://DESKTOP-10538/USERS/JLYNN/ANWR?#", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare/?#", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://desktop-10538/Users/jlynn/ANWR/?#", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE/?", UriKind.Absolute), new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(new Uri("file://DESKTOP-10538/USERS/JLYNN/ANWR/?#", UriKind.Absolute), new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("file://servicenowdiag479.file.core.windows.net", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("file://desktop-10538", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("file://servicenowdiag479.file.core.windows.net/", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("file://desktop-10538/", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("file:///testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("file:///Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("http://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("http://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("/servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(new Uri("/desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetConstructorUriTestData), DynamicDataSourceType.Method)]
        public void ConstructorUriTestMethod(Uri uri, ConstructorResultExpected expected)
        {
            if (expected.IsArgumentOutOfRangeException)
                Assert.ThrowsException< ArgumentOutOfRangeException>(() => new VolumeIdentifier(uri));
            else
            {
                VolumeIdentifier volumeIdentifier = new(uri);
                Assert.AreEqual(expected.IsEmpty, volumeIdentifier.IsEmpty());
                Assert.IsNotNull(volumeIdentifier.Location);
                Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.ToString());
                Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.ToString());
                Assert.AreEqual(expected.SerialNumber, volumeIdentifier.SerialNumber);
                Assert.AreEqual(expected.UUID, volumeIdentifier.UUID);
            }
        }

        public static IEnumerable<object[]> GetConstructorSerialNumberTestData()
        {
            yield return ConstructorResultExpected.CreateTestData(0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData(uint.MaxValue);
            yield return ConstructorResultExpected.CreateTestData(uint.MinValue);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetConstructorSerialNumberTestData), DynamicDataSourceType.Method)]
        public void ConstructorSerialNumberTestMethod(uint serialNumber, ConstructorResultExpected expected)
        {
            VolumeIdentifier volumeIdentifier = new(serialNumber);
            Assert.AreEqual(expected.IsEmpty, volumeIdentifier.IsEmpty());
            Assert.IsNotNull(volumeIdentifier.Location);
            Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.Location.ToString());
            Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.ToString());
            Assert.AreEqual(expected.SerialNumber, volumeIdentifier.SerialNumber);
            Assert.AreEqual(expected.UUID, volumeIdentifier.UUID);
        }

        public static IEnumerable<object[]> GetConstructorUuidTestData()
        {
            yield return ConstructorResultExpected.CreateTestData(Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"));
            yield return ConstructorResultExpected.CreateTestData(Guid.Empty);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetConstructorUuidTestData), DynamicDataSourceType.Method)]
        public void ConstructorUuidTestMethod(Guid uuid, ConstructorResultExpected expected)
        {
            VolumeIdentifier volumeIdentifier = new(uuid);
            Assert.AreEqual(expected.IsEmpty, volumeIdentifier.IsEmpty());
            Assert.IsNotNull(volumeIdentifier.Location);
            Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.Location.ToString());
            Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.ToString());
            Assert.AreEqual(expected.SerialNumber, volumeIdentifier.SerialNumber);
            Assert.AreEqual(expected.UUID, volumeIdentifier.UUID);
        }

        public static IEnumerable<object[]> GetParseTestData()
        {
            yield return ConstructorResultExpected.CreateEmptyTestData((string)null);
            yield return ConstructorResultExpected.CreateEmptyTestData("");
            yield return ConstructorResultExpected.CreateTestData("094F6-22A5", 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData("094F622A5", 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData("094f6-22a5", 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData("094f622a5", 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData("ffffffff", 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData("ffff-ffff", 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData("FFFF-FFFF", 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData("FFFFFFFF", 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData("91502fe2-cb4b-4274-a8ad-8b70074132c3", Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData("91502fe2cb4b4274a8ad8b70074132c3", Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData("91502FE2-CB4B-4274-A8AD-8B70074132C3", Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData("91502FE2CB4B4274A8AD8B70074132C3", Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData("00000000-0000-0000-0000-000000000000", Guid.Empty);
            yield return ConstructorResultExpected.CreateTestData("00000000000000000000000000000000", Guid.Empty);
            yield return ConstructorResultExpected.CreateTestData("urn:volume:id:094F6-22A5", 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData("urn:volume:id:094F622A5", 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData("urn:volume:id:094f6-22a5", 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData("urn:volume:id:094f622a5", 0x094f622a5u);
            yield return ConstructorResultExpected.CreateTestData("urn:volume:id:ffffffff", 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData("urn:volume:id:ffff-ffff", 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData("urn:volume:id:FFFF-FFFF", 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData("urn:volume:id:FFFFFFFF", 0xffffffffu);
            yield return ConstructorResultExpected.CreateTestData("urn:uuid:91502fe2-cb4b-4274-a8ad-8b70074132c3", Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData("urn:uuid:91502fe2cb4b4274a8ad8b70074132c3", Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData("urn:uuid:91502FE2-CB4B-4274-A8AD-8B70074132C3", Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData("urn:uuid:91502FE2CB4B4274A8AD8B70074132C3", Guid.Parse("91502fe2-cb4b-4274-a8ad-8b70074132c3"));
            yield return ConstructorResultExpected.CreateTestData("urn:uuid:00000000-0000-0000-0000-000000000000", Guid.Empty);
            yield return ConstructorResultExpected.CreateTestData("urn:uuid:00000000000000000000000000000000", Guid.Empty);
            yield return ConstructorResultExpected.CreateTestData("file://servicenowdiag479.file.core.windows.net/testazureshare", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://desktop-10538/Users/jlynn/ANWR", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://DESKTOP-10538/USERS/JLYNN/ANWR", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://servicenowdiag479.file.core.windows.net/testazureshare/", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://desktop-10538/Users/jlynn/ANWR/", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE/", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://DESKTOP-10538/USERS/JLYNN/ANWR/", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://servicenowdiag479.file.core.windows.net/testazureshare?", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://desktop-10538/Users/jlynn/ANWR?", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE?", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://DESKTOP-10538/USERS/JLYNN/ANWR?", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://servicenowdiag479.file.core.windows.net/testazureshare/?", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://desktop-10538/Users/jlynn/ANWR/?", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE/?", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://DESKTOP-10538/USERS/JLYNN/ANWR/?", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://servicenowdiag479.file.core.windows.net/testazureshare#", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://desktop-10538/Users/jlynn/ANWR#", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE#", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://DESKTOP-10538/USERS/JLYNN/ANWR#", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://servicenowdiag479.file.core.windows.net/testazureshare/#", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://desktop-10538/Users/jlynn/ANWR/#", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE/#", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://DESKTOP-10538/USERS/JLYNN/ANWR/#", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://servicenowdiag479.file.core.windows.net/testazureshare?#", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://desktop-10538/Users/jlynn/ANWR?#", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE?#", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://DESKTOP-10538/USERS/JLYNN/ANWR?#", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://servicenowdiag479.file.core.windows.net/testazureshare/?#", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://desktop-10538/Users/jlynn/ANWR/?#", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE/?", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("file://DESKTOP-10538/USERS/JLYNN/ANWR/?#", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(@"\\servicenowdiag479.file.core.windows.net\testazureshare", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(@"\\desktop-10538\Users\jlynn\ANWR", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(@"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\TESTAZURESHARE", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(@"\\DESKTOP-10538\USERS\JLYNN\ANWR", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(@"\\servicenowdiag479.file.core.windows.net\testazureshare\", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(@"\\desktop-10538\Users\jlynn\ANWR\", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(@"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\TESTAZURESHARE\", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData(@"\\DESKTOP-10538\USERS\JLYNN\ANWR\", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("//servicenowdiag479.file.core.windows.net/testazureshare", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("//desktop-10538/Users/jlynn/ANWR", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("//SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("//DESKTOP-10538/USERS/JLYNN/ANWR", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("//servicenowdiag479.file.core.windows.net/testazureshare/", new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("//desktop-10538/Users/jlynn/ANWR/", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("//SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE/", new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateTestData("//DESKTOP-10538/USERS/JLYNN/ANWR/", new Uri("file://desktop-10538/Users/jlynn/ANWR", UriKind.Absolute));
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("file://servicenowdiag479.file.core.windows.net");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("file://desktop-10538");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("file://servicenowdiag479.file.core.windows.net/");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("file://desktop-10538/");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("file:///testazureshare");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("file:///Users/jlynn/ANWR");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("http://servicenowdiag479.file.core.windows.net/testazureshare");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("http://desktop-10538/Users/jlynn/ANWR");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("/servicenowdiag479.file.core.windows.net/testazureshare");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData("/desktop-10538/Users/jlynn/ANWR");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(@"\\servicenowdiag479.file.core.windows.net");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(@"\\desktop-10538");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(@"\\servicenowdiag479.file.core.windows.net\");
            yield return ConstructorResultExpected.CreateArgumentOutOfRangeExceptionTestData(@"\\desktop-10538\");
        }

        [DataTestMethod]
        [DynamicData(nameof(GetParseTestData), DynamicDataSourceType.Method)]
        public void ParseTestMethod(string text, ConstructorResultExpected expected)
        {
            if (expected.IsArgumentOutOfRangeException)
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => VolumeIdentifier.Parse(text));
            else
            {
                VolumeIdentifier volumeIdentifier = VolumeIdentifier.Parse(text);
                Assert.AreEqual(expected.IsEmpty, volumeIdentifier.IsEmpty());
                Assert.IsNotNull(volumeIdentifier.Location);
                Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.Location.ToString());
                Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.ToString());
                Assert.AreEqual(expected.SerialNumber, volumeIdentifier.SerialNumber);
                Assert.AreEqual(expected.UUID, volumeIdentifier.UUID);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetParseTestData), DynamicDataSourceType.Method)]
        public void TryParseTestMethod(string text, ConstructorResultExpected expected)
        {
            bool result = VolumeIdentifier.TryParse(text, out VolumeIdentifier volumeIdentifier);
            Assert.AreEqual(expected.IsArgumentOutOfRangeException, result);
            if (!expected.IsArgumentOutOfRangeException)
            {
                Assert.AreEqual(expected.IsEmpty, volumeIdentifier.IsEmpty());
                Assert.IsNotNull(volumeIdentifier.Location);
                Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.Location.ToString());
                Assert.AreEqual(expected.Location.ToString(), volumeIdentifier.ToString());
                Assert.AreEqual(expected.SerialNumber, volumeIdentifier.SerialNumber);
                Assert.AreEqual(expected.UUID, volumeIdentifier.UUID);
            }
        }

        // TODO: Tests for equality where case is ignored
    }
}
